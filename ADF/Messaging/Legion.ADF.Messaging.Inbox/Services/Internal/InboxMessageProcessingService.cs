using Legion.ADF.Messaging.Settings;
using Legion.Database;
using Legion.Logging;
using Legion.MessageBus;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Legion.ADF.Messaging.Inbox.Services.Internal;

public class InboxMessageProcessingService : BackgroundService
{
	private readonly InboxMessageProcessingServiceOptions _options;
	private readonly IServiceProvider _serviceProvider;
	private readonly ILogger? _logger;

	public InboxMessageProcessingService(
		IOptions<InboxMessageProcessingServiceOptions> options,
		IServiceProvider serviceProvider)
	{
		Throw.IfArgumentNull(options);
		Throw.IfArgumentNull(serviceProvider);

		_options = options.Value;
		_serviceProvider = serviceProvider;

		if (_options.LogToStandardILogger)
			_logger = serviceProvider.GetRequiredService<ILoggerFactory>().CreateLogger<InboxMessageProcessingService>();
	}

	private IInboxUnitOfWork CreateUnitOfWork(
		IScopeContext scopeContext,
		IServiceProvider serviceProvider,
		IConnectionProviderFactory connectionProviderFactory,
		string messagingInboxStoreId)
	{
		var connectionProvider = connectionProviderFactory.CreateWithNewTransactionByStoreId<ConnectionStringProvider>(
			serviceProvider!,
			messagingInboxStoreId,
			transactionIsolationLevel: null,
			false,
			createAuditEntryStore: false);

		var inboxUowResult = connectionProvider.UnitOfWorkProvider.Create<IInboxUnitOfWork>(scopeContext);

		if (inboxUowResult.HasError)
			inboxUowResult.ThrowIfErrorOrNullData(scopeContext, Legion.ADF.Messaging.Exceptions.Internal.ErrorCodes.InboxUnitOfWorkException.InvalidUoW, true);

		var uow = inboxUowResult.Data!;
		return uow;
	}

	protected override async Task ExecuteAsync(CancellationToken cancellationToken)
	{
		var scopeContextGlobal = ScopeContext.Create(nameof(InboxMessageProcessingService));

		Model.InboxInstance inboxInstance = null!;

		var noQueue = false;
		while (!cancellationToken.IsCancellationRequested)
		{
			var scopeContext = ScopeContext.Create(scopeContextGlobal, correlationId: Guid.NewGuid());

			if (_options.LogLevel <= LogLevel.Information)
				_logger?.LogInformationMessage(scopeContext, x => x.InternalMessage($"{nameof(InboxMessageProcessingService)}.{nameof(ExecuteAsync)}: START"));

			List<Model.InboxQueue> inboxQueues = [];
			bool processedAnyMessage = false;

			try
			{
				await using var asyncServiceScope = _serviceProvider.CreateAsyncScope();
				var scopedServiceProvider = asyncServiceScope.ServiceProvider;

				var connectionProviderFactory = scopedServiceProvider.GetRequiredService<IConnectionProviderFactory>();
				var storeOptions = scopedServiceProvider.GetRequiredService<IOptions<MessagingInboxStoreOptions>>().Value;

				var inboxUoW = CreateUnitOfWork(
					scopeContext,
					scopedServiceProvider,
					connectionProviderFactory,
					storeOptions.MessagingInboxStoreId);

				IReadOnlyList<string> blockedInboxMessageNamespaces;

				var inboxQueueRegistry = scopedServiceProvider.GetRequiredService<InboxQueueRegistry>();

				await using (var connectionProvider = inboxUoW.ConnectionProvider)
				{
					var nowUtc = GlobalContext.Instance.UtcNow;
					
					inboxQueues = await inboxUoW.InboxQueueRepository.GetAllInboxQueuesByEvents(
						new Queries.InboxQueue.GetAllInboxQueuesByEventsQuery(
							inboxQueueRegistry.GetAllRegisterdReceivedEventNamespaces(),
							CheckReadPermissions: false,
							AsNoTracking: true,
							QueryableBuilder: null))
						.ToResultAsync(scopeContext, cancellationToken);

					if (inboxQueues.Count == 0)
					{
						noQueue = true;

						var warning = new LogMessageBuilder(scopeContext, Exceptions.Internal.ErrorCodes.InboxMessageProcessingService.Default)
							.LogLevel(LogLevel.Warning)
							.InternalMessage("inboxQueues.Count == 0")
							.Build();

						await SaveInboxProcessingLogAsync(
							scopeContext,
							warning,
							idInboxQueue: null,
							scopedServiceProvider,
							connectionProviderFactory,
							storeOptions,
							cancellationToken);

						return;
					}

					blockedInboxMessageNamespaces = inboxUoW.BlockedInboxMessageTypeRepository
						.GetAllBlockedInboxMessageTypes(new Queries.BlockedInboxMessageType.GetAllBlockedInboxMessageTypesQuery(false, true, null))
						.ToNamespaces(scopeContext);

					var dbInboxInstance = await inboxUoW.InboxInstanceRepository
						.GetInboxInstanceById(new Queries.InboxInstance.GetInboxInstanceByIdQuery(Infrastructure.EnvironmentInfo.RUNTIME_UNIQUE_KEY, false, AsNoTracking: false))
						.ToResultAsync(scopeContext, cancellationToken);

					Throw.IfNull(dbInboxInstance, scopeContext);

					inboxInstance = dbInboxInstance;
				}

				var maxDegreeOfParallelism = inboxInstance.MaxDegreeOfQueueParallelism < 0
					? 1
					: (inboxInstance.MaxDegreeOfQueueParallelism == 0
						? Environment.ProcessorCount
						: inboxInstance.MaxDegreeOfQueueParallelism);

				if (1 < maxDegreeOfParallelism)
				{
					var parallelOptions = new ParallelOptions
					{
						MaxDegreeOfParallelism = maxDegreeOfParallelism,
						CancellationToken = cancellationToken
					};

					await Parallel.ForEachAsync(
						inboxQueues,
						parallelOptions,
						async (inboxQueue, cancelToken) =>
						{
							var processedMessagesCount = await ProcessInboxQueueAsync(scopeContext, inboxQueue, blockedInboxMessageNamespaces, scopedServiceProvider, connectionProviderFactory, storeOptions, cancelToken);
							if (0 < processedMessagesCount)
								processedAnyMessage = true;
						});
				}
				else
				{
					foreach (var inboxQueue in inboxQueues)
					{
						var processedMessagesCount = await ProcessInboxQueueAsync(scopeContext, inboxQueue, blockedInboxMessageNamespaces, scopedServiceProvider, connectionProviderFactory, storeOptions, cancellationToken);
						if (0 < processedMessagesCount)
							processedAnyMessage = true;
					}
				}
			}
			catch (Exception ex)
			{
				var error = new ErrorMessageBuilder(scopeContext, Exceptions.Internal.ErrorCodes.InboxMessageProcessingService.Default)
					.LogLevel(LogLevel.Error)
					.ExceptionInfo(ex)
					.Build();

				await using var asyncServiceScope = _serviceProvider.CreateAsyncScope();
				var scopedServiceProvider = asyncServiceScope.ServiceProvider;

				var connectionProviderFactory = scopedServiceProvider.GetRequiredService<IConnectionProviderFactory>();
				var storeOptions = scopedServiceProvider.GetRequiredService<IOptions<MessagingInboxStoreOptions>>().Value;

				await SaveInboxProcessingLogAsync(
					scopeContext,
					error,
					idInboxQueue: null,
					scopedServiceProvider,
					connectionProviderFactory,
					storeOptions,
					cancellationToken);
			}
			finally
			{
				if (noQueue)
				{
					//exit;
				}
				else
				{
					//only if no message was processed
					if (!processedAnyMessage && !cancellationToken.IsCancellationRequested)
					{
						try
						{
							await Task.Delay(_options.IdleTimeout, cancellationToken);
						}
						catch { }
					}
				}
			}
		}
	}

	private async Task<int> ProcessInboxQueueAsync(
		IScopeContext scopeContext,
		Model.InboxQueue inboxQueue,
		IReadOnlyList<string> blockedInboxMessageNamespaces,
		IServiceProvider serviceProvider,
		IConnectionProviderFactory connectionProviderFactory,
		MessagingInboxStoreOptions storeOptions,
		CancellationToken cancellationToken = default)
	{
		scopeContext = scopeContext.CreateNew()
			.AddContextProperty(nameof(inboxQueue.IdInboxQueue), inboxQueue.IdInboxQueue.ToString());

		Dictionary<Guid, DateTime> inboxMessageIdsDict = [];

		try
		{
			var inboxUoW = CreateUnitOfWork(
				scopeContext,
				serviceProvider,
				connectionProviderFactory,
				storeOptions.MessagingInboxStoreId);

			await using (var connectionProvider = inboxUoW.ConnectionProvider)
			{
				var nowUtc = GlobalContext.Instance.UtcNow;
				inboxMessageIdsDict = await inboxUoW.InboxMessageRepository.GetNextInboxMessagesByQueue(
					new Queries.InboxMessage.GetNextInboxMessagesByQueueQuery(
						inboxQueue.IdInboxQueue,
						inboxQueue.IsSequentialFIFO,
						BatchCount: inboxQueue.MessagesBatchCount,
						NowUtc: nowUtc,
						CheckReadPermissions: false,
						AsNoTracking: true,
						QueryableBuilder: null))
					.ToInboxMessageIds(scopeContext, cancellationToken);
			}

			var maxDegreeOfParallelism = (!inboxQueue.MaxDegreeOfParallelism.HasValue || inboxQueue.MaxDegreeOfParallelism < 0)
				? 1
				: (inboxQueue.MaxDegreeOfParallelism == 0
					? Environment.ProcessorCount
					: (inboxQueue.MaxDegreeOfParallelism ?? 1));

			if (!inboxQueue.IsSequentialFIFO && 1 < maxDegreeOfParallelism)
			{
				var parallelOptions = new ParallelOptions
				{
					MaxDegreeOfParallelism = maxDegreeOfParallelism,
					CancellationToken = cancellationToken
				};

				await Parallel.ForEachAsync(
					inboxMessageIdsDict,
					parallelOptions,
					async (kvp, cancelToken) =>
					{
						await ProcessInboxMessageAsync(scopeContext, kvp.Key, inboxQueue, blockedInboxMessageNamespaces, serviceProvider, connectionProviderFactory, storeOptions, cancelToken);
					});
			}
			else
			{
				foreach (var kvp in inboxMessageIdsDict)
				{
					if (inboxQueue.IsSequentialFIFO)
					{
						var nowUtc = GlobalContext.Instance.UtcNow;
						if (nowUtc < kvp.Value)
						{
							//wait for the message to be processed
							await Task.Delay(kvp.Value - nowUtc, cancellationToken);
						}
					}

					await ProcessInboxMessageAsync(scopeContext, kvp.Key, inboxQueue, blockedInboxMessageNamespaces, serviceProvider, connectionProviderFactory, storeOptions, cancellationToken);
				}
			}
		}
		catch (Exception ex)
		{
			var error = new ErrorMessageBuilder(scopeContext, Exceptions.Internal.ErrorCodes.InboxMessageProcessingService.Default)
				.LogLevel(LogLevel.Error)
				.ExceptionInfo(ex)
				.Build();

			await SaveInboxProcessingLogAsync(
				scopeContext,
				error,
				inboxQueue.IdInboxQueue,
				serviceProvider,
				connectionProviderFactory,
				storeOptions,
				cancellationToken);
		}

		return inboxMessageIdsDict.Count;
	}

	private async Task ProcessInboxMessageAsync(
		IScopeContext scopeContext,
		Guid idInboxMessage,
		Model.InboxQueue inboxQueue,
		IReadOnlyList<string> blockedInboxMessageNamespaces,
		IServiceProvider serviceProvider,
		IConnectionProviderFactory connectionProviderFactory,
		MessagingInboxStoreOptions storeOptions,
		CancellationToken cancellationToken = default)
	{
		scopeContext = ScopeContext.Create(scopeContext, correlationId: Guid.NewGuid())
			.AddContextProperty(nameof(idInboxMessage), idInboxMessage.ToString());

		try
		{
			Model.InboxMessage? inboxMessage;

			var inboxUoW = CreateUnitOfWork(
				scopeContext,
				serviceProvider,
				connectionProviderFactory,
				storeOptions.MessagingInboxStoreId);

			await using (var connectionProvider = inboxUoW.ConnectionProvider)
			{
				inboxMessage = await inboxUoW.InboxMessageRepository.GetInboxMessageById(
					new Queries.InboxMessage.GetInboxMessageByIdQuery(
						idInboxMessage,
						IncludeContent: true,
						CheckReadPermissions: false,
						AsNoTracking: false,
						QueryableBuilder: null))
					.ToResultAsync(scopeContext, cancellationToken);

				Throw.IfNull(inboxMessage, scopeContext);
				Throw.IfNull(inboxMessage.MessageType, scopeContext);

				var setResult = inboxMessage.SetProcessing(scopeContext, inboxQueue.TimeoutForMessageProcessing, _options.DisableMultiProcessingLog);
				setResult.ThrowIfError(scopeContext, null, true);

				if (setResult.Data != null)
					inboxUoW.InboxMessageProcessingLogRepository.Add(scopeContext, setResult.Data);

				var saveResult = await inboxUoW.SaveAsync(scopeContext);
				saveResult.ThrowIfErrorOrNullData(scopeContext, null, true);

				var commitResult = connectionProvider.CommitAll(scopeContext);
				commitResult.ThrowIfErrorOrNullData(scopeContext, null, true);

				if (commitResult.Data != true)
					Throw.InvalidOperationException("Cannot commit transaction", scopeContext);
			}

			if (blockedInboxMessageNamespaces.Contains(inboxMessage.MessageType.Namespace))
			{
				var warningMessage = new LogMessageBuilder(
					scopeContext,
					Exceptions.Internal.ErrorCodes.DomainEventProcessingService.BlockedDomainEvent(inboxMessage.MessageType.Namespace))
					.LogLevel(LogLevel.Warning)
					.Build();

				if (_options.LogLevel <= warningMessage.LogLevel)
					_logger?.LogWarningMessage(warningMessage);

				await SetBlockedAsync(
					scopeContext,
					idInboxMessage,
					inboxQueue.IdInboxQueue,
					warningMessage,
					serviceProvider,
					connectionProviderFactory,
					storeOptions,
					cancellationToken);

				return;
			}

			var inboxMessageTypeRegistry = serviceProvider.GetRequiredService<InboxMessageTypeRegistry>();
			var idInboxMessageType = inboxMessageTypeRegistry.GetIdInboxMessageType(inboxMessage.MessageType.Namespace, true);

			if (!idInboxMessageType.HasValue)
			{
				var warningMessage = new LogMessageBuilder(
					scopeContext,
					Exceptions.Internal.ErrorCodes.InboxMessageProcessingService.InboxMessageHasUnknownType(inboxMessage.MessageType.Namespace))
					.LogLevel(LogLevel.Warning)
					.Build();

				if (_options.LogLevel <= warningMessage.LogLevel)
					_logger?.LogWarningMessage(warningMessage);

				await SetUnknownTypeAsync(
					scopeContext,
					idInboxMessage,
					inboxQueue.IdInboxQueue,
					warningMessage,
					serviceProvider,
					connectionProviderFactory,
					storeOptions,
					cancellationToken);

				return;
			}

			var inboxQueueRegistry = serviceProvider.GetRequiredService<InboxQueueRegistry>();
			var inboxMessageReceivedEvent = inboxQueueRegistry.CreateQueueEvent(inboxQueue.ReceivedEventNamespace, inboxMessage);
			if (inboxMessageReceivedEvent == null)
			{
				var errorMessage = new ErrorMessageBuilder(
					scopeContext,
					Exceptions.Internal.ErrorCodes.InboxMessageProcessingService.InvalidInboxQueueReceivedEventType(inboxQueue.ReceivedEventNamespace))
					.Build();
				
				if (_options.LogLevel <= errorMessage.LogLevel)
					_logger?.LogErrorMessage(errorMessage);

				await SetSuspendedInboxMessageAsync(
					scopeContext,
					idInboxMessage,
					inboxQueue.IdInboxQueue,
					errorMessage,
					serviceProvider,
					connectionProviderFactory,
					storeOptions,
					cancellationToken);

				return;
			}

			var messageBus = serviceProvider.GetRequiredService<IMessageBus<ConnectionStringProvider>>();
			var publisResult = await messageBus.PublishAsync(scopeContext, inboxMessageReceivedEvent, connectionProvider: null, cancellationToken);
			if (publisResult.HasError)
			{
				if (_options.LogLevel <= LogLevel.Error)
					_logger?.LogResultErrorMessages(publisResult, true, true);

				var errorMessage = publisResult.ErrorMessages[0];

				await SetFailedInboxMessageAsync(
					scopeContext,
					idInboxMessage,
					inboxQueue.IdInboxQueue,
					inboxQueue.MaxMessageProcessingRetryCount,
					inboxQueue.TimeoutForMessageProcessing,
					errorMessage,
					serviceProvider,
					connectionProviderFactory,
					storeOptions,
					cancellationToken);

				return;
			}
			else if (!publisResult.DataWasSet || publisResult.Data != true)
			{
				var warningMessage = new LogMessageBuilder(
					scopeContext,
					Exceptions.Internal.ErrorCodes.InboxMessageProcessingService.InboxMessageHasNoHandler(inboxQueue.ReceivedEventNamespace))
					.LogLevel(LogLevel.Warning)
					.Build();

				if (_options.LogLevel <= warningMessage.LogLevel)
					_logger?.LogWarningMessage(warningMessage);

				await SetNoHandlerAsync(
					scopeContext,
					idInboxMessage,
					inboxQueue.IdInboxQueue,
					warningMessage,
					serviceProvider,
					connectionProviderFactory,
					storeOptions,
					cancellationToken);

				return;
			}
			else
			{
				await SetProcessedInboxMessageAsync(
					scopeContext,
					idInboxMessage,
					inboxQueue.IdInboxQueue,
					serviceProvider,
					connectionProviderFactory,
					storeOptions,
					cancellationToken);

				return;
			}
		}
		catch (Exception ex)
		{
			var error = new ErrorMessageBuilder(scopeContext, Exceptions.Internal.ErrorCodes.InboxMessageProcessingService.CanNotSetState(nameof(Model.InboxMessageStatus.Processing)))
				.LogLevel(LogLevel.Error)
				.ExceptionInfo(ex)
				.Build();

			await SaveInboxProcessingLogAsync(
				scopeContext,
				error,
				inboxQueue.IdInboxQueue,
				serviceProvider,
				connectionProviderFactory,
				storeOptions,
				cancellationToken);
		}
	}

	private async Task SetProcessedInboxMessageAsync(
		IScopeContext scopeContext,
		Guid idInboxMessage,
		Guid idInboxQueue,
		IServiceProvider serviceProvider,
		IConnectionProviderFactory connectionProviderFactory,
		MessagingInboxStoreOptions storeOptions,
		CancellationToken cancellationToken = default)
	{
		scopeContext = scopeContext.CreateNew();

		try
		{
			var inboxUoW = CreateUnitOfWork(
				scopeContext,
				serviceProvider,
				connectionProviderFactory,
				storeOptions.MessagingInboxStoreId);

			await using var connectionProvider = inboxUoW.ConnectionProvider;

			var inboxMessage = await inboxUoW.InboxMessageRepository.GetInboxMessageById(
				new Queries.InboxMessage.GetInboxMessageByIdQuery(
					idInboxMessage,
					IncludeContent: false,
					CheckReadPermissions: false,
					AsNoTracking: false,
					QueryableBuilder: null))
				.ToResultAsync(scopeContext, cancellationToken);

			Throw.IfNull(inboxMessage, scopeContext);

			var setResult = inboxMessage.SetProcessed(scopeContext, false);
			setResult.ThrowIfError(scopeContext, null, true);

			if (setResult.Data != null)
				inboxUoW.InboxMessageProcessingLogRepository.Add(scopeContext, setResult.Data);

			var saveResult = await inboxUoW.SaveAsync(scopeContext);
			saveResult.ThrowIfErrorOrNullData(scopeContext, null, true);

			var commitResult = connectionProvider.CommitAll(scopeContext);
			commitResult.ThrowIfErrorOrNullData(scopeContext, null, true);

			if (commitResult.Data != true)
				Throw.InvalidOperationException("Cannot commit transaction", scopeContext);
		}
		catch (Exception ex)
		{
			var error = new ErrorMessageBuilder(scopeContext, Exceptions.Internal.ErrorCodes.InboxMessageProcessingService.CanNotSetState(nameof(Model.InboxMessageStatus.Processed)))
				.LogLevel(LogLevel.Error)
				.ExceptionInfo(ex)
				.Build();

			await SaveInboxProcessingLogAsync(
				scopeContext,
				error,
				idInboxQueue,
				serviceProvider,
				connectionProviderFactory,
				storeOptions,
				cancellationToken);
		}
	}

	private async Task SetUnknownTypeAsync(
		IScopeContext scopeContext,
		Guid idInboxMessage,
		Guid idInboxQueue,
		ILogMessage? logMessage,
		IServiceProvider serviceProvider,
		IConnectionProviderFactory connectionProviderFactory,
		MessagingInboxStoreOptions storeOptions,
		CancellationToken cancellationToken = default)
	{
		scopeContext = scopeContext.CreateNew();

		try
		{
			var inboxUoW = CreateUnitOfWork(
				scopeContext,
				serviceProvider,
				connectionProviderFactory,
				storeOptions.MessagingInboxStoreId);

			await using var connectionProvider = inboxUoW.ConnectionProvider;

			var inboxMessage = await inboxUoW.InboxMessageRepository.GetInboxMessageById(
				new Queries.InboxMessage.GetInboxMessageByIdQuery(
					idInboxMessage,
					IncludeContent: false,
					CheckReadPermissions: false,
					AsNoTracking: false,
					QueryableBuilder: null))
				.ToResultAsync(scopeContext, cancellationToken);

			Throw.IfNull(inboxMessage, scopeContext);

			var setResult = inboxMessage.SetUnknownType(scopeContext, false, logMessage);
			setResult.ThrowIfError(scopeContext, null, true);

			if (setResult.Data != null)
				inboxUoW.InboxMessageProcessingLogRepository.Add(scopeContext, setResult.Data);

			var saveResult = await inboxUoW.SaveAsync(scopeContext);
			saveResult.ThrowIfErrorOrNullData(scopeContext, null, true);

			var commitResult = connectionProvider.CommitAll(scopeContext);
			commitResult.ThrowIfErrorOrNullData(scopeContext, null, true);

			if (commitResult.Data != true)
				Throw.InvalidOperationException("Cannot commit transaction", scopeContext);
		}
		catch (Exception ex)
		{
			var error = new ErrorMessageBuilder(scopeContext, Exceptions.Internal.ErrorCodes.InboxMessageProcessingService.CanNotSetState(nameof(Model.InboxMessageStatus.UnknownType)))
				.LogLevel(LogLevel.Error)
				.ExceptionInfo(ex)
				.Build();

			await SaveInboxProcessingLogAsync(
				scopeContext,
				error,
				idInboxQueue,
				serviceProvider,
				connectionProviderFactory,
				storeOptions,
				cancellationToken);
		}
	}

	private async Task SetNoHandlerAsync(
		IScopeContext scopeContext,
		Guid idInboxMessage,
		Guid idInboxQueue,
		ILogMessage? logMessage,
		IServiceProvider serviceProvider,
		IConnectionProviderFactory connectionProviderFactory,
		MessagingInboxStoreOptions storeOptions,
		CancellationToken cancellationToken = default)
	{
		scopeContext = scopeContext.CreateNew();

		try
		{
			var inboxUoW = CreateUnitOfWork(
				scopeContext,
				serviceProvider,
				connectionProviderFactory,
				storeOptions.MessagingInboxStoreId);

			await using var connectionProvider = inboxUoW.ConnectionProvider;

			var inboxMessage = await inboxUoW.InboxMessageRepository.GetInboxMessageById(
				new Queries.InboxMessage.GetInboxMessageByIdQuery(
					idInboxMessage,
					IncludeContent: false,
					CheckReadPermissions: false,
					AsNoTracking: false,
					QueryableBuilder: null))
				.ToResultAsync(scopeContext, cancellationToken);

			Throw.IfNull(inboxMessage, scopeContext);

			var setResult = inboxMessage.SetNoHandler(scopeContext, false, logMessage);
			setResult.ThrowIfError(scopeContext, null, true);

			if (setResult.Data != null)
				inboxUoW.InboxMessageProcessingLogRepository.Add(scopeContext, setResult.Data);

			var saveResult = await inboxUoW.SaveAsync(scopeContext);
			saveResult.ThrowIfErrorOrNullData(scopeContext, null, true);

			var commitResult = connectionProvider.CommitAll(scopeContext);
			commitResult.ThrowIfErrorOrNullData(scopeContext, null, true);

			if (commitResult.Data != true)
				Throw.InvalidOperationException("Cannot commit transaction", scopeContext);
		}
		catch (Exception ex)
		{
			var error = new ErrorMessageBuilder(scopeContext, Exceptions.Internal.ErrorCodes.InboxMessageProcessingService.CanNotSetState(nameof(Model.InboxMessageStatus.NoHandler)))
				.LogLevel(LogLevel.Error)
				.ExceptionInfo(ex)
				.Build();

			await SaveInboxProcessingLogAsync(
				scopeContext,
				error,
				idInboxQueue,
				serviceProvider,
				connectionProviderFactory,
				storeOptions,
				cancellationToken);
		}
	}

	private async Task SetBlockedAsync(
		IScopeContext scopeContext,
		Guid idInboxMessage,
		Guid idInboxQueue,
		ILogMessage? logMessage,
		IServiceProvider serviceProvider,
		IConnectionProviderFactory connectionProviderFactory,
		MessagingInboxStoreOptions storeOptions,
		CancellationToken cancellationToken = default)
	{
		scopeContext = scopeContext.CreateNew();

		try
		{
			var inboxUoW = CreateUnitOfWork(
				scopeContext,
				serviceProvider,
				connectionProviderFactory,
				storeOptions.MessagingInboxStoreId);

			await using var connectionProvider = inboxUoW.ConnectionProvider;

			var inboxMessage = await inboxUoW.InboxMessageRepository.GetInboxMessageById(
				new Queries.InboxMessage.GetInboxMessageByIdQuery(
					idInboxMessage,
					IncludeContent: false,
					CheckReadPermissions: false,
					AsNoTracking: false,
					QueryableBuilder: null))
				.ToResultAsync(scopeContext, cancellationToken);

			Throw.IfNull(inboxMessage, scopeContext);

			var setResult = inboxMessage.SetBlocked(scopeContext, false, logMessage);
			setResult.ThrowIfError(scopeContext, null, true);

			if (setResult.Data != null)
				inboxUoW.InboxMessageProcessingLogRepository.Add(scopeContext, setResult.Data);

			var saveResult = await inboxUoW.SaveAsync(scopeContext);
			saveResult.ThrowIfErrorOrNullData(scopeContext, null, true);

			var commitResult = connectionProvider.CommitAll(scopeContext);
			commitResult.ThrowIfErrorOrNullData(scopeContext, null, true);

			if (commitResult.Data != true)
				Throw.InvalidOperationException("Cannot commit transaction", scopeContext);
		}
		catch (Exception ex)
		{
			var error = new ErrorMessageBuilder(scopeContext, Exceptions.Internal.ErrorCodes.InboxMessageProcessingService.CanNotSetState(nameof(Model.InboxMessageStatus.Blocked)))
				.LogLevel(LogLevel.Error)
				.ExceptionInfo(ex)
				.Build();

			await SaveInboxProcessingLogAsync(
				scopeContext,
				error,
				idInboxQueue,
				serviceProvider,
				connectionProviderFactory,
				storeOptions,
				cancellationToken);
		}
	}

	private async Task SetFailedInboxMessageAsync(
		IScopeContext scopeContext,
		Guid idInboxMessage,
		Guid idInboxQueue,
		int maxRetryCount,
		TimeSpan nextProcessingDelay,
		IErrorMessage errorMessage,
		IServiceProvider serviceProvider,
		IConnectionProviderFactory connectionProviderFactory,
		MessagingInboxStoreOptions storeOptions,
		CancellationToken cancellationToken = default)
	{
		scopeContext = scopeContext.CreateNew();

		try
		{
			var inboxUoW = CreateUnitOfWork(
				scopeContext,
				serviceProvider,
				connectionProviderFactory,
				storeOptions.MessagingInboxStoreId);

			await using var connectionProvider = inboxUoW.ConnectionProvider;

			var inboxMessage = await inboxUoW.InboxMessageRepository.GetInboxMessageById(
				new Queries.InboxMessage.GetInboxMessageByIdQuery(
					idInboxMessage,
					IncludeContent: false,
					CheckReadPermissions: false,
					AsNoTracking: false,
					QueryableBuilder: null))
				.ToResultAsync(scopeContext, cancellationToken);

			Throw.IfNull(inboxMessage, scopeContext);

			var setResult = inboxMessage.SetFailed(
				scopeContext,
				maxRetryCount,
				nextProcessingDelay,
				false,
				errorMessage);
			setResult.ThrowIfError(scopeContext, null, true);

			if (setResult.Data != null)
				inboxUoW.InboxMessageProcessingLogRepository.Add(scopeContext, setResult.Data);

			var saveResult = await inboxUoW.SaveAsync(scopeContext);
			saveResult.ThrowIfErrorOrNullData(scopeContext, null, true);

			var commitResult = connectionProvider.CommitAll(scopeContext);
			commitResult.ThrowIfErrorOrNullData(scopeContext, null, true);

			if (commitResult.Data != true)
				Throw.InvalidOperationException("Cannot commit transaction", scopeContext);
		}
		catch (Exception ex)
		{
			var error = new ErrorMessageBuilder(scopeContext, Exceptions.Internal.ErrorCodes.InboxMessageProcessingService.CanNotSetState(nameof(Model.InboxMessageStatus.Failed)))
				.LogLevel(LogLevel.Error)
				.ExceptionInfo(ex)
				.Build();

			await SaveInboxProcessingLogAsync(
				scopeContext,
				error,
				idInboxQueue,
				serviceProvider,
				connectionProviderFactory,
				storeOptions,
				cancellationToken);
		}
	}

	private async Task SetSuspendedInboxMessageAsync(
		IScopeContext scopeContext,
		Guid idInboxMessage,
		Guid idInboxQueue,
		IErrorMessage errorMessage,
		IServiceProvider serviceProvider,
		IConnectionProviderFactory connectionProviderFactory,
		MessagingInboxStoreOptions storeOptions,
		CancellationToken cancellationToken = default)
	{
		scopeContext = scopeContext.CreateNew();

		try
		{
			var inboxUoW = CreateUnitOfWork(
				scopeContext,
				serviceProvider,
				connectionProviderFactory,
				storeOptions.MessagingInboxStoreId);

			await using var connectionProvider = inboxUoW.ConnectionProvider;

			var inboxMessage = await inboxUoW.InboxMessageRepository.GetInboxMessageById(
				new Queries.InboxMessage.GetInboxMessageByIdQuery(
					idInboxMessage,
					IncludeContent: false,
					CheckReadPermissions: false,
					AsNoTracking: false,
					QueryableBuilder: null))
				.ToResultAsync(scopeContext, cancellationToken);

			Throw.IfNull(inboxMessage, scopeContext);

			var setResult = inboxMessage.SetSuspended(
				scopeContext,
				false,
				errorMessage);
			setResult.ThrowIfError(scopeContext, null, true);

			if (setResult.Data != null)
				inboxUoW.InboxMessageProcessingLogRepository.Add(scopeContext, setResult.Data);

			var saveResult = await inboxUoW.SaveAsync(scopeContext);
			saveResult.ThrowIfErrorOrNullData(scopeContext, null, true);

			var commitResult = connectionProvider.CommitAll(scopeContext);
			commitResult.ThrowIfErrorOrNullData(scopeContext, null, true);

			if (commitResult.Data != true)
				Throw.InvalidOperationException("Cannot commit transaction", scopeContext);
		}
		catch (Exception ex)
		{
			var error = new ErrorMessageBuilder(scopeContext, Exceptions.Internal.ErrorCodes.InboxMessageProcessingService.CanNotSetState(nameof(Model.InboxMessageStatus.Suspended)))
				.LogLevel(LogLevel.Error)
				.ExceptionInfo(ex)
				.Build();

			await SaveInboxProcessingLogAsync(
				scopeContext,
				error,
				idInboxQueue,
				serviceProvider,
				connectionProviderFactory,
				storeOptions,
				cancellationToken);
		}
	}

	private Model.InboxProcessingLog? CreateLog(
		IScopeContext scopeContext,
		ILogMessage logMessage,
		Guid? idInboxQueue)
	{
		if (logMessage == null)
			return null;

		if (logMessage.LogLevel < _options.LogLevel)
			return null;

		_logger?.LogMessage(logMessage);

		var result = Model.InboxProcessingLog.Create(
			scopeContext,
			idInboxQueue,
			logMessage);

		if (result.HasError)
			_logger?.LogResultErrorMessages(result, true, true);

		return result.Data;
	}

	private bool AddLog(
		IScopeContext scopeContext,
		ILogMessage logMessage,
		Guid? idInboxQueue,
		IInboxUnitOfWork inboxUnitOfWork)
	{
		var inboxProcessingLog = CreateLog(scopeContext, logMessage, idInboxQueue);
		if (inboxProcessingLog == null)
			return false;

		inboxUnitOfWork.InboxProcessingLogRepository.Add(scopeContext, inboxProcessingLog);
		return true;
	}

	private async Task SaveInboxProcessingLogAsync(
		IScopeContext scopeContext,
		ILogMessage logMessage,
		Guid? idInboxQueue,
		IServiceProvider serviceProvider,
		IConnectionProviderFactory connectionProviderFactory,
		MessagingInboxStoreOptions storeOptions,
		CancellationToken cancellationToken = default)
	{
		scopeContext = scopeContext.CreateNew();

		try
		{
			var inboxUoW = CreateUnitOfWork(
				scopeContext,
				serviceProvider,
				connectionProviderFactory,
				storeOptions.MessagingInboxStoreId);

			await using var connectionProvider = inboxUoW.ConnectionProvider;

			var added = AddLog(scopeContext, logMessage, idInboxQueue, inboxUoW);
			if (!added)
			{
				_logger?.LogMessage(logMessage);
				return;
			}

			var saveResult = await inboxUoW.SaveAsync(scopeContext);
			saveResult.ThrowIfErrorOrNullData(scopeContext, null, true);

			var commitResult = connectionProvider.CommitAll(scopeContext);
			commitResult.ThrowIfErrorOrNullData(scopeContext, null, true);

			if (commitResult.Data != true)
				Throw.InvalidOperationException("Cannot commit transaction", scopeContext);
		}
		catch (Exception ex)
		{
			_logger?.LogErrorMessage(scopeContext, Exceptions.Internal.ErrorCodes.InboxMessageProcessingService.FailedToWriteProcessingLog, x => x.ExceptionInfo(ex));
		}
	}
}
