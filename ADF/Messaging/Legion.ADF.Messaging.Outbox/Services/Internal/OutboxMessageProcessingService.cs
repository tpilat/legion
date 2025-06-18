using Legion.ADF.Messaging.Settings;
using Legion.Database;
using Legion.Logging;
using Legion.MessageBus;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Legion.ADF.Messaging.Outbox.Services.Internal;

public class OutboxMessageProcessingService : BackgroundService
{
	private readonly OutboxMessageProcessingServiceOptions _options;
	private readonly IServiceProvider _serviceProvider;
	private readonly ILogger? _logger;

	public OutboxMessageProcessingService(
		IOptions<OutboxMessageProcessingServiceOptions> options,
		IServiceProvider serviceProvider)
	{
		Throw.IfArgumentNull(options);
		Throw.IfArgumentNull(serviceProvider);

		_options = options.Value;
		_serviceProvider = serviceProvider;

		if (_options.LogToStandardILogger)
			_logger = serviceProvider.GetRequiredService<ILoggerFactory>().CreateLogger<OutboxMessageProcessingService>();
	}

	private IOutboxUnitOfWork CreateUnitOfWork(
		IScopeContext scopeContext,
		IServiceProvider serviceProvider,
		IConnectionProviderFactory connectionProviderFactory,
		string messagingOutboxStoreId)
	{
		var connectionProvider = connectionProviderFactory.CreateWithNewTransactionByStoreId<ConnectionStringProvider>(
			serviceProvider!,
			messagingOutboxStoreId,
			transactionIsolationLevel: null,
			false,
			createAuditEntryStore: false);

		var outboxUowResult = connectionProvider.UnitOfWorkProvider.Create<IOutboxUnitOfWork>(scopeContext);

		if (outboxUowResult.HasError)
			outboxUowResult.ThrowIfErrorOrNullData(scopeContext, Legion.ADF.Messaging.Exceptions.Internal.ErrorCodes.OutboxUnitOfWorkException.InvalidUoW, true);

		var uow = outboxUowResult.Data!;
		return uow;
	}

	protected override async Task ExecuteAsync(CancellationToken cancellationToken)
	{
		var scopeContextGlobal = ScopeContext.Create(nameof(OutboxMessageProcessingService));

		Model.OutboxInstance outboxInstance = null!;

		var noQueue = false;
		while (!cancellationToken.IsCancellationRequested)
		{
			var scopeContext = ScopeContext.Create(scopeContextGlobal, correlationId: Guid.NewGuid());

			if (_options.LogLevel <= LogLevel.Information)
				_logger?.LogInformationMessage(scopeContext, x => x.InternalMessage($"{nameof(OutboxMessageProcessingService)}.{nameof(ExecuteAsync)}: START"));

			List<Model.OutboxQueue> outboxQueues = [];
			bool processedAnyMessage = false;

			try
			{
				await using var asyncServiceScope = _serviceProvider.CreateAsyncScope();
				var scopedServiceProvider = asyncServiceScope.ServiceProvider;

				var connectionProviderFactory = scopedServiceProvider.GetRequiredService<IConnectionProviderFactory>();
				var storeOptions = scopedServiceProvider.GetRequiredService<IOptions<MessagingOutboxStoreOptions>>().Value;

				var outboxUoW = CreateUnitOfWork(
					scopeContext,
					scopedServiceProvider,
					connectionProviderFactory,
					storeOptions.MessagingOutboxStoreId);

				IReadOnlyList<string> blockedOutboxMessageNamespaces;

				var outboxQueueRegistry = scopedServiceProvider.GetRequiredService<OutboxQueueRegistry>();

				await using (var connectionProvider = outboxUoW.ConnectionProvider)
				{
					var nowUtc = GlobalContext.Instance.UtcNow;
					
					outboxQueues = await outboxUoW.OutboxQueueRepository.GetAllOutboxQueuesByEvents(
						new Queries.OutboxQueue.GetAllOutboxQueuesByEventsQuery(
							outboxQueueRegistry.GetAllRegisterdReceivedEventNamespaces(),
							CheckReadPermissions: false,
							AsNoTracking: true,
							QueryableBuilder: null))
						.ToResultAsync(scopeContext, cancellationToken);

					if (outboxQueues.Count == 0)
					{
						noQueue = true;

						var warning = new LogMessageBuilder(scopeContext, Exceptions.Internal.ErrorCodes.OutboxMessageProcessingService.Default)
							.LogLevel(LogLevel.Warning)
							.InternalMessage("outboxQueues.Count == 0")
							.Build();

						await SaveOutboxProcessingLogAsync(
							scopeContext,
							warning,
							idOutboxQueue: null,
							scopedServiceProvider,
							connectionProviderFactory,
							storeOptions,
							cancellationToken);

						return;
					}

					blockedOutboxMessageNamespaces = outboxUoW.BlockedOutboxMessageTypeRepository
						.GetAllBlockedOutboxMessageTypes(new Queries.BlockedOutboxMessageType.GetAllBlockedOutboxMessageTypesQuery(false, true, null))
						.ToNamespaces(scopeContext);

					var dbOutboxInstance = await outboxUoW.OutboxInstanceRepository
						.GetOutboxInstanceById(new Queries.OutboxInstance.GetOutboxInstanceByIdQuery(Infrastructure.EnvironmentInfo.RUNTIME_UNIQUE_KEY, false, AsNoTracking: false))
						.ToResultAsync(scopeContext, cancellationToken);

					Throw.IfNull(dbOutboxInstance, scopeContext);

					outboxInstance = dbOutboxInstance;
				}

				var maxDegreeOfParallelism = outboxInstance.MaxDegreeOfQueueParallelism < 0
					? 1
					: (outboxInstance.MaxDegreeOfQueueParallelism == 0
						? Environment.ProcessorCount
						: outboxInstance.MaxDegreeOfQueueParallelism);

				if (1 < maxDegreeOfParallelism)
				{
					var parallelOptions = new ParallelOptions
					{
						MaxDegreeOfParallelism = maxDegreeOfParallelism,
						CancellationToken = cancellationToken
					};

					await Parallel.ForEachAsync(
						outboxQueues,
						parallelOptions,
						async (outboxQueue, cancelToken) =>
						{
							var processedMessagesCount = await ProcessOutboxQueueAsync(scopeContext, outboxQueue, blockedOutboxMessageNamespaces, scopedServiceProvider, connectionProviderFactory, storeOptions, cancelToken);
							if (0 < processedMessagesCount)
								processedAnyMessage = true;
						});
				}
				else
				{
					foreach (var outboxQueue in outboxQueues)
					{
						var processedMessagesCount = await ProcessOutboxQueueAsync(scopeContext, outboxQueue, blockedOutboxMessageNamespaces, scopedServiceProvider, connectionProviderFactory, storeOptions, cancellationToken);
						if (0 < processedMessagesCount)
							processedAnyMessage = true;
					}
				}
			}
			catch (Exception ex)
			{
				var error = new ErrorMessageBuilder(scopeContext, Exceptions.Internal.ErrorCodes.OutboxMessageProcessingService.Default)
					.LogLevel(LogLevel.Error)
					.ExceptionInfo(ex)
					.Build();

				await using var asyncServiceScope = _serviceProvider.CreateAsyncScope();
				var scopedServiceProvider = asyncServiceScope.ServiceProvider;

				var connectionProviderFactory = scopedServiceProvider.GetRequiredService<IConnectionProviderFactory>();
				var storeOptions = scopedServiceProvider.GetRequiredService<IOptions<MessagingOutboxStoreOptions>>().Value;

				await SaveOutboxProcessingLogAsync(
					scopeContext,
					error,
					idOutboxQueue: null,
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

	private async Task<int> ProcessOutboxQueueAsync(
		IScopeContext scopeContext,
		Model.OutboxQueue outboxQueue,
		IReadOnlyList<string> blockedOutboxMessageNamespaces,
		IServiceProvider serviceProvider,
		IConnectionProviderFactory connectionProviderFactory,
		MessagingOutboxStoreOptions storeOptions,
		CancellationToken cancellationToken = default)
	{
		scopeContext = scopeContext.CreateNew()
			.AddContextProperty(nameof(outboxQueue.IdOutboxQueue), outboxQueue.IdOutboxQueue.ToString());

		Dictionary<Guid, DateTime> outboxMessageIdsDict = [];

		try
		{
			var outboxUoW = CreateUnitOfWork(
				scopeContext,
				serviceProvider,
				connectionProviderFactory,
				storeOptions.MessagingOutboxStoreId);

			await using (var connectionProvider = outboxUoW.ConnectionProvider)
			{
				var nowUtc = GlobalContext.Instance.UtcNow;
				outboxMessageIdsDict = await outboxUoW.OutboxMessageRepository.GetNextOutboxMessagesByQueue(
					new Queries.OutboxMessage.GetNextOutboxMessagesByQueueQuery(
						outboxQueue.IdOutboxQueue,
						outboxQueue.IsSequentialFIFO,
						BatchCount: outboxQueue.MessagesBatchCount,
						NowUtc: nowUtc,
						CheckReadPermissions: false,
						AsNoTracking: true,
						QueryableBuilder: null))
					.ToOutboxMessageIds(scopeContext, cancellationToken);
			}

			var maxDegreeOfParallelism = (!outboxQueue.MaxDegreeOfParallelism.HasValue || outboxQueue.MaxDegreeOfParallelism < 0)
				? 1
				: (outboxQueue.MaxDegreeOfParallelism == 0
					? Environment.ProcessorCount
					: (outboxQueue.MaxDegreeOfParallelism ?? 1));

			if (!outboxQueue.IsSequentialFIFO && 1 < maxDegreeOfParallelism)
			{
				var parallelOptions = new ParallelOptions
				{
					MaxDegreeOfParallelism = maxDegreeOfParallelism,
					CancellationToken = cancellationToken
				};

				await Parallel.ForEachAsync(
					outboxMessageIdsDict,
					parallelOptions,
					async (kvp, cancelToken) =>
					{
						await ProcessOutboxMessageAsync(scopeContext, kvp.Key, outboxQueue, blockedOutboxMessageNamespaces, serviceProvider, connectionProviderFactory, storeOptions, cancelToken);
					});
			}
			else
			{
				foreach (var kvp in outboxMessageIdsDict)
				{
					if (outboxQueue.IsSequentialFIFO)
					{
						var nowUtc = GlobalContext.Instance.UtcNow;
						if (nowUtc < kvp.Value)
						{
							//wait for the message to be processed
							await Task.Delay(kvp.Value - nowUtc, cancellationToken);
						}
					}

					await ProcessOutboxMessageAsync(scopeContext, kvp.Key, outboxQueue, blockedOutboxMessageNamespaces, serviceProvider, connectionProviderFactory, storeOptions, cancellationToken);
				}
			}
		}
		catch (Exception ex)
		{
			var error = new ErrorMessageBuilder(scopeContext, Exceptions.Internal.ErrorCodes.OutboxMessageProcessingService.Default)
				.LogLevel(LogLevel.Error)
				.ExceptionInfo(ex)
				.Build();

			await SaveOutboxProcessingLogAsync(
				scopeContext,
				error,
				outboxQueue.IdOutboxQueue,
				serviceProvider,
				connectionProviderFactory,
				storeOptions,
				cancellationToken);
		}

		return outboxMessageIdsDict.Count;
	}

	private async Task ProcessOutboxMessageAsync(
		IScopeContext scopeContext,
		Guid idOutboxMessage,
		Model.OutboxQueue outboxQueue,
		IReadOnlyList<string> blockedOutboxMessageNamespaces,
		IServiceProvider serviceProvider,
		IConnectionProviderFactory connectionProviderFactory,
		MessagingOutboxStoreOptions storeOptions,
		CancellationToken cancellationToken = default)
	{
		scopeContext = ScopeContext.Create(scopeContext, correlationId: Guid.NewGuid())
			.AddContextProperty(nameof(idOutboxMessage), idOutboxMessage.ToString());

		try
		{
			Model.OutboxMessage? outboxMessage;

			var outboxUoW = CreateUnitOfWork(
				scopeContext,
				serviceProvider,
				connectionProviderFactory,
				storeOptions.MessagingOutboxStoreId);

			await using (var connectionProvider = outboxUoW.ConnectionProvider)
			{
				outboxMessage = await outboxUoW.OutboxMessageRepository.GetOutboxMessageById(
					new Queries.OutboxMessage.GetOutboxMessageByIdQuery(
						idOutboxMessage,
						IncludeContent: true,
						CheckReadPermissions: false,
						AsNoTracking: false,
						QueryableBuilder: null))
					.ToResultAsync(scopeContext, cancellationToken);

				Throw.IfNull(outboxMessage, scopeContext);
				Throw.IfNull(outboxMessage.MessageType, scopeContext);

				var setResult = outboxMessage.SetProcessing(scopeContext, outboxQueue.TimeoutForMessageProcessing, _options.DisableMultiProcessingLog);
				setResult.ThrowIfError(scopeContext, null, true);

				if (setResult.Data != null)
					outboxUoW.OutboxMessageProcessingLogRepository.Add(scopeContext, setResult.Data);

				var saveResult = await outboxUoW.SaveAsync(scopeContext);
				saveResult.ThrowIfErrorOrNullData(scopeContext, null, true);

				var commitResult = connectionProvider.CommitAll(scopeContext);
				commitResult.ThrowIfErrorOrNullData(scopeContext, null, true);

				if (commitResult.Data != true)
					Throw.InvalidOperationException("Cannot commit transaction", scopeContext);
			}

			if (blockedOutboxMessageNamespaces.Contains(outboxMessage.MessageType.Namespace))
			{
				var warningMessage = new LogMessageBuilder(
					scopeContext,
					Exceptions.Internal.ErrorCodes.DomainEventProcessingService.BlockedDomainEvent(outboxMessage.MessageType.Namespace))
					.LogLevel(LogLevel.Warning)
					.Build();

				if (_options.LogLevel <= warningMessage.LogLevel)
					_logger?.LogWarningMessage(warningMessage);

				await SetBlockedAsync(
					scopeContext,
					idOutboxMessage,
					outboxQueue.IdOutboxQueue,
					warningMessage,
					serviceProvider,
					connectionProviderFactory,
					storeOptions,
					cancellationToken);

				return;
			}

			var outboxMessageTypeRegistry = serviceProvider.GetRequiredService<OutboxMessageTypeRegistry>();
			var idOutboxMessageType = outboxMessageTypeRegistry.GetIdOutboxMessageType(outboxMessage.MessageType.Namespace, true);

			if (!idOutboxMessageType.HasValue)
			{
				var warningMessage = new LogMessageBuilder(
					scopeContext,
					Exceptions.Internal.ErrorCodes.OutboxMessageProcessingService.OutboxMessageHasUnknownType(outboxMessage.MessageType.Namespace))
					.LogLevel(LogLevel.Warning)
					.Build();

				if (_options.LogLevel <= warningMessage.LogLevel)
					_logger?.LogWarningMessage(warningMessage);

				await SetUnknownTypeAsync(
					scopeContext,
					idOutboxMessage,
					outboxQueue.IdOutboxQueue,
					warningMessage,
					serviceProvider,
					connectionProviderFactory,
					storeOptions,
					cancellationToken);

				return;
			}

			var outboxQueueRegistry = serviceProvider.GetRequiredService<OutboxQueueRegistry>();
			var outboxMessageReceivedEvent = outboxQueueRegistry.CreateQueueEvent(outboxQueue.ReceivedEventNamespace, outboxMessage);
			if (outboxMessageReceivedEvent == null)
			{
				var errorMessage = new ErrorMessageBuilder(
					scopeContext,
					Exceptions.Internal.ErrorCodes.OutboxMessageProcessingService.InvalidOutboxQueueReceivedEventType(outboxQueue.ReceivedEventNamespace))
					.Build();
				
				if (_options.LogLevel <= errorMessage.LogLevel)
					_logger?.LogErrorMessage(errorMessage);

				await SetSuspendedOutboxMessageAsync(
					scopeContext,
					idOutboxMessage,
					outboxQueue.IdOutboxQueue,
					errorMessage,
					serviceProvider,
					connectionProviderFactory,
					storeOptions,
					cancellationToken);

				return;
			}

			var messageBus = serviceProvider.GetRequiredService<IMessageBus<ConnectionStringProvider>>();
			var publisResult = await messageBus.PublishAsync(scopeContext, outboxMessageReceivedEvent, connectionProvider: null, cancellationToken);
			if (publisResult.HasError)
			{
				if (_options.LogLevel <= LogLevel.Error)
					_logger?.LogResultErrorMessages(publisResult, true, true);

				var errorMessage = publisResult.ErrorMessages[0];

				await SetFailedOutboxMessageAsync(
					scopeContext,
					idOutboxMessage,
					outboxQueue.IdOutboxQueue,
					outboxQueue.MaxMessageProcessingRetryCount,
					outboxQueue.TimeoutForMessageProcessing,
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
					Exceptions.Internal.ErrorCodes.OutboxMessageProcessingService.OutboxMessageHasNoHandler(outboxQueue.ReceivedEventNamespace))
					.LogLevel(LogLevel.Warning)
					.Build();

				if (_options.LogLevel <= warningMessage.LogLevel)
					_logger?.LogWarningMessage(warningMessage);

				await SetNoHandlerAsync(
					scopeContext,
					idOutboxMessage,
					outboxQueue.IdOutboxQueue,
					warningMessage,
					serviceProvider,
					connectionProviderFactory,
					storeOptions,
					cancellationToken);

				return;
			}
			else
			{
				await SetProcessedOutboxMessageAsync(
					scopeContext,
					idOutboxMessage,
					outboxQueue.IdOutboxQueue,
					serviceProvider,
					connectionProviderFactory,
					storeOptions,
					cancellationToken);

				return;
			}
		}
		catch (Exception ex)
		{
			var error = new ErrorMessageBuilder(scopeContext, Exceptions.Internal.ErrorCodes.OutboxMessageProcessingService.CanNotSetState(nameof(Model.OutboxMessageStatus.Processing)))
				.LogLevel(LogLevel.Error)
				.ExceptionInfo(ex)
				.Build();

			await SaveOutboxProcessingLogAsync(
				scopeContext,
				error,
				outboxQueue.IdOutboxQueue,
				serviceProvider,
				connectionProviderFactory,
				storeOptions,
				cancellationToken);
		}
	}

	private async Task SetProcessedOutboxMessageAsync(
		IScopeContext scopeContext,
		Guid idOutboxMessage,
		Guid idOutboxQueue,
		IServiceProvider serviceProvider,
		IConnectionProviderFactory connectionProviderFactory,
		MessagingOutboxStoreOptions storeOptions,
		CancellationToken cancellationToken = default)
	{
		scopeContext = scopeContext.CreateNew();

		try
		{
			var outboxUoW = CreateUnitOfWork(
				scopeContext,
				serviceProvider,
				connectionProviderFactory,
				storeOptions.MessagingOutboxStoreId);

			await using var connectionProvider = outboxUoW.ConnectionProvider;

			var outboxMessage = await outboxUoW.OutboxMessageRepository.GetOutboxMessageById(
				new Queries.OutboxMessage.GetOutboxMessageByIdQuery(
					idOutboxMessage,
					IncludeContent: false,
					CheckReadPermissions: false,
					AsNoTracking: false,
					QueryableBuilder: null))
				.ToResultAsync(scopeContext, cancellationToken);

			Throw.IfNull(outboxMessage, scopeContext);

			var setResult = outboxMessage.SetProcessed(scopeContext, false);
			setResult.ThrowIfError(scopeContext, null, true);

			if (setResult.Data != null)
				outboxUoW.OutboxMessageProcessingLogRepository.Add(scopeContext, setResult.Data);

			var saveResult = await outboxUoW.SaveAsync(scopeContext);
			saveResult.ThrowIfErrorOrNullData(scopeContext, null, true);

			var commitResult = connectionProvider.CommitAll(scopeContext);
			commitResult.ThrowIfErrorOrNullData(scopeContext, null, true);

			if (commitResult.Data != true)
				Throw.InvalidOperationException("Cannot commit transaction", scopeContext);
		}
		catch (Exception ex)
		{
			var error = new ErrorMessageBuilder(scopeContext, Exceptions.Internal.ErrorCodes.OutboxMessageProcessingService.CanNotSetState(nameof(Model.OutboxMessageStatus.Processed)))
				.LogLevel(LogLevel.Error)
				.ExceptionInfo(ex)
				.Build();

			await SaveOutboxProcessingLogAsync(
				scopeContext,
				error,
				idOutboxQueue,
				serviceProvider,
				connectionProviderFactory,
				storeOptions,
				cancellationToken);
		}
	}

	private async Task SetUnknownTypeAsync(
		IScopeContext scopeContext,
		Guid idOutboxMessage,
		Guid idOutboxQueue,
		ILogMessage? logMessage,
		IServiceProvider serviceProvider,
		IConnectionProviderFactory connectionProviderFactory,
		MessagingOutboxStoreOptions storeOptions,
		CancellationToken cancellationToken = default)
	{
		scopeContext = scopeContext.CreateNew();

		try
		{
			var outboxUoW = CreateUnitOfWork(
				scopeContext,
				serviceProvider,
				connectionProviderFactory,
				storeOptions.MessagingOutboxStoreId);

			await using var connectionProvider = outboxUoW.ConnectionProvider;

			var outboxMessage = await outboxUoW.OutboxMessageRepository.GetOutboxMessageById(
				new Queries.OutboxMessage.GetOutboxMessageByIdQuery(
					idOutboxMessage,
					IncludeContent: false,
					CheckReadPermissions: false,
					AsNoTracking: false,
					QueryableBuilder: null))
				.ToResultAsync(scopeContext, cancellationToken);

			Throw.IfNull(outboxMessage, scopeContext);

			var setResult = outboxMessage.SetUnknownType(scopeContext, false, logMessage);
			setResult.ThrowIfError(scopeContext, null, true);

			if (setResult.Data != null)
				outboxUoW.OutboxMessageProcessingLogRepository.Add(scopeContext, setResult.Data);

			var saveResult = await outboxUoW.SaveAsync(scopeContext);
			saveResult.ThrowIfErrorOrNullData(scopeContext, null, true);

			var commitResult = connectionProvider.CommitAll(scopeContext);
			commitResult.ThrowIfErrorOrNullData(scopeContext, null, true);

			if (commitResult.Data != true)
				Throw.InvalidOperationException("Cannot commit transaction", scopeContext);
		}
		catch (Exception ex)
		{
			var error = new ErrorMessageBuilder(scopeContext, Exceptions.Internal.ErrorCodes.OutboxMessageProcessingService.CanNotSetState(nameof(Model.OutboxMessageStatus.UnknownType)))
				.LogLevel(LogLevel.Error)
				.ExceptionInfo(ex)
				.Build();

			await SaveOutboxProcessingLogAsync(
				scopeContext,
				error,
				idOutboxQueue,
				serviceProvider,
				connectionProviderFactory,
				storeOptions,
				cancellationToken);
		}
	}

	private async Task SetNoHandlerAsync(
		IScopeContext scopeContext,
		Guid idOutboxMessage,
		Guid idOutboxQueue,
		ILogMessage? logMessage,
		IServiceProvider serviceProvider,
		IConnectionProviderFactory connectionProviderFactory,
		MessagingOutboxStoreOptions storeOptions,
		CancellationToken cancellationToken = default)
	{
		scopeContext = scopeContext.CreateNew();

		try
		{
			var outboxUoW = CreateUnitOfWork(
				scopeContext,
				serviceProvider,
				connectionProviderFactory,
				storeOptions.MessagingOutboxStoreId);

			await using var connectionProvider = outboxUoW.ConnectionProvider;

			var outboxMessage = await outboxUoW.OutboxMessageRepository.GetOutboxMessageById(
				new Queries.OutboxMessage.GetOutboxMessageByIdQuery(
					idOutboxMessage,
					IncludeContent: false,
					CheckReadPermissions: false,
					AsNoTracking: false,
					QueryableBuilder: null))
				.ToResultAsync(scopeContext, cancellationToken);

			Throw.IfNull(outboxMessage, scopeContext);

			var setResult = outboxMessage.SetNoHandler(scopeContext, false, logMessage);
			setResult.ThrowIfError(scopeContext, null, true);

			if (setResult.Data != null)
				outboxUoW.OutboxMessageProcessingLogRepository.Add(scopeContext, setResult.Data);

			var saveResult = await outboxUoW.SaveAsync(scopeContext);
			saveResult.ThrowIfErrorOrNullData(scopeContext, null, true);

			var commitResult = connectionProvider.CommitAll(scopeContext);
			commitResult.ThrowIfErrorOrNullData(scopeContext, null, true);

			if (commitResult.Data != true)
				Throw.InvalidOperationException("Cannot commit transaction", scopeContext);
		}
		catch (Exception ex)
		{
			var error = new ErrorMessageBuilder(scopeContext, Exceptions.Internal.ErrorCodes.OutboxMessageProcessingService.CanNotSetState(nameof(Model.OutboxMessageStatus.NoHandler)))
				.LogLevel(LogLevel.Error)
				.ExceptionInfo(ex)
				.Build();

			await SaveOutboxProcessingLogAsync(
				scopeContext,
				error,
				idOutboxQueue,
				serviceProvider,
				connectionProviderFactory,
				storeOptions,
				cancellationToken);
		}
	}

	private async Task SetBlockedAsync(
		IScopeContext scopeContext,
		Guid idOutboxMessage,
		Guid idOutboxQueue,
		ILogMessage? logMessage,
		IServiceProvider serviceProvider,
		IConnectionProviderFactory connectionProviderFactory,
		MessagingOutboxStoreOptions storeOptions,
		CancellationToken cancellationToken = default)
	{
		scopeContext = scopeContext.CreateNew();

		try
		{
			var outboxUoW = CreateUnitOfWork(
				scopeContext,
				serviceProvider,
				connectionProviderFactory,
				storeOptions.MessagingOutboxStoreId);

			await using var connectionProvider = outboxUoW.ConnectionProvider;

			var outboxMessage = await outboxUoW.OutboxMessageRepository.GetOutboxMessageById(
				new Queries.OutboxMessage.GetOutboxMessageByIdQuery(
					idOutboxMessage,
					IncludeContent: false,
					CheckReadPermissions: false,
					AsNoTracking: false,
					QueryableBuilder: null))
				.ToResultAsync(scopeContext, cancellationToken);

			Throw.IfNull(outboxMessage, scopeContext);

			var setResult = outboxMessage.SetBlocked(scopeContext, false, logMessage);
			setResult.ThrowIfError(scopeContext, null, true);

			if (setResult.Data != null)
				outboxUoW.OutboxMessageProcessingLogRepository.Add(scopeContext, setResult.Data);

			var saveResult = await outboxUoW.SaveAsync(scopeContext);
			saveResult.ThrowIfErrorOrNullData(scopeContext, null, true);

			var commitResult = connectionProvider.CommitAll(scopeContext);
			commitResult.ThrowIfErrorOrNullData(scopeContext, null, true);

			if (commitResult.Data != true)
				Throw.InvalidOperationException("Cannot commit transaction", scopeContext);
		}
		catch (Exception ex)
		{
			var error = new ErrorMessageBuilder(scopeContext, Exceptions.Internal.ErrorCodes.OutboxMessageProcessingService.CanNotSetState(nameof(Model.OutboxMessageStatus.Blocked)))
				.LogLevel(LogLevel.Error)
				.ExceptionInfo(ex)
				.Build();

			await SaveOutboxProcessingLogAsync(
				scopeContext,
				error,
				idOutboxQueue,
				serviceProvider,
				connectionProviderFactory,
				storeOptions,
				cancellationToken);
		}
	}

	private async Task SetFailedOutboxMessageAsync(
		IScopeContext scopeContext,
		Guid idOutboxMessage,
		Guid idOutboxQueue,
		int maxRetryCount,
		TimeSpan nextProcessingDelay,
		IErrorMessage errorMessage,
		IServiceProvider serviceProvider,
		IConnectionProviderFactory connectionProviderFactory,
		MessagingOutboxStoreOptions storeOptions,
		CancellationToken cancellationToken = default)
	{
		scopeContext = scopeContext.CreateNew();

		try
		{
			var outboxUoW = CreateUnitOfWork(
				scopeContext,
				serviceProvider,
				connectionProviderFactory,
				storeOptions.MessagingOutboxStoreId);

			await using var connectionProvider = outboxUoW.ConnectionProvider;

			var outboxMessage = await outboxUoW.OutboxMessageRepository.GetOutboxMessageById(
				new Queries.OutboxMessage.GetOutboxMessageByIdQuery(
					idOutboxMessage,
					IncludeContent: false,
					CheckReadPermissions: false,
					AsNoTracking: false,
					QueryableBuilder: null))
				.ToResultAsync(scopeContext, cancellationToken);

			Throw.IfNull(outboxMessage, scopeContext);

			var setResult = outboxMessage.SetFailed(
				scopeContext,
				maxRetryCount,
				nextProcessingDelay,
				false,
				errorMessage);
			setResult.ThrowIfError(scopeContext, null, true);

			if (setResult.Data != null)
				outboxUoW.OutboxMessageProcessingLogRepository.Add(scopeContext, setResult.Data);

			var saveResult = await outboxUoW.SaveAsync(scopeContext);
			saveResult.ThrowIfErrorOrNullData(scopeContext, null, true);

			var commitResult = connectionProvider.CommitAll(scopeContext);
			commitResult.ThrowIfErrorOrNullData(scopeContext, null, true);

			if (commitResult.Data != true)
				Throw.InvalidOperationException("Cannot commit transaction", scopeContext);
		}
		catch (Exception ex)
		{
			var error = new ErrorMessageBuilder(scopeContext, Exceptions.Internal.ErrorCodes.OutboxMessageProcessingService.CanNotSetState(nameof(Model.OutboxMessageStatus.Failed)))
				.LogLevel(LogLevel.Error)
				.ExceptionInfo(ex)
				.Build();

			await SaveOutboxProcessingLogAsync(
				scopeContext,
				error,
				idOutboxQueue,
				serviceProvider,
				connectionProviderFactory,
				storeOptions,
				cancellationToken);
		}
	}

	private async Task SetSuspendedOutboxMessageAsync(
		IScopeContext scopeContext,
		Guid idOutboxMessage,
		Guid idOutboxQueue,
		IErrorMessage errorMessage,
		IServiceProvider serviceProvider,
		IConnectionProviderFactory connectionProviderFactory,
		MessagingOutboxStoreOptions storeOptions,
		CancellationToken cancellationToken = default)
	{
		scopeContext = scopeContext.CreateNew();

		try
		{
			var outboxUoW = CreateUnitOfWork(
				scopeContext,
				serviceProvider,
				connectionProviderFactory,
				storeOptions.MessagingOutboxStoreId);

			await using var connectionProvider = outboxUoW.ConnectionProvider;

			var outboxMessage = await outboxUoW.OutboxMessageRepository.GetOutboxMessageById(
				new Queries.OutboxMessage.GetOutboxMessageByIdQuery(
					idOutboxMessage,
					IncludeContent: false,
					CheckReadPermissions: false,
					AsNoTracking: false,
					QueryableBuilder: null))
				.ToResultAsync(scopeContext, cancellationToken);

			Throw.IfNull(outboxMessage, scopeContext);

			var setResult = outboxMessage.SetSuspended(
				scopeContext,
				false,
				errorMessage);
			setResult.ThrowIfError(scopeContext, null, true);

			if (setResult.Data != null)
				outboxUoW.OutboxMessageProcessingLogRepository.Add(scopeContext, setResult.Data);

			var saveResult = await outboxUoW.SaveAsync(scopeContext);
			saveResult.ThrowIfErrorOrNullData(scopeContext, null, true);

			var commitResult = connectionProvider.CommitAll(scopeContext);
			commitResult.ThrowIfErrorOrNullData(scopeContext, null, true);

			if (commitResult.Data != true)
				Throw.InvalidOperationException("Cannot commit transaction", scopeContext);
		}
		catch (Exception ex)
		{
			var error = new ErrorMessageBuilder(scopeContext, Exceptions.Internal.ErrorCodes.OutboxMessageProcessingService.CanNotSetState(nameof(Model.OutboxMessageStatus.Suspended)))
				.LogLevel(LogLevel.Error)
				.ExceptionInfo(ex)
				.Build();

			await SaveOutboxProcessingLogAsync(
				scopeContext,
				error,
				idOutboxQueue,
				serviceProvider,
				connectionProviderFactory,
				storeOptions,
				cancellationToken);
		}
	}

	private Model.OutboxProcessingLog? CreateLog(
		IScopeContext scopeContext,
		ILogMessage logMessage,
		Guid? idOutboxQueue)
	{
		if (logMessage == null)
			return null;

		if (logMessage.LogLevel < _options.LogLevel)
			return null;

		_logger?.LogMessage(logMessage);

		var result = Model.OutboxProcessingLog.Create(
			scopeContext,
			idOutboxQueue,
			logMessage);

		if (result.HasError)
			_logger?.LogResultErrorMessages(result, true, true);

		return result.Data;
	}

	private bool AddLog(
		IScopeContext scopeContext,
		ILogMessage logMessage,
		Guid? idOutboxQueue,
		IOutboxUnitOfWork outboxUnitOfWork)
	{
		var outboxProcessingLog = CreateLog(scopeContext, logMessage, idOutboxQueue);
		if (outboxProcessingLog == null)
			return false;

		outboxUnitOfWork.OutboxProcessingLogRepository.Add(scopeContext, outboxProcessingLog);
		return true;
	}

	private async Task SaveOutboxProcessingLogAsync(
		IScopeContext scopeContext,
		ILogMessage logMessage,
		Guid? idOutboxQueue,
		IServiceProvider serviceProvider,
		IConnectionProviderFactory connectionProviderFactory,
		MessagingOutboxStoreOptions storeOptions,
		CancellationToken cancellationToken = default)
	{
		scopeContext = scopeContext.CreateNew();

		try
		{
			var outboxUoW = CreateUnitOfWork(
				scopeContext,
				serviceProvider,
				connectionProviderFactory,
				storeOptions.MessagingOutboxStoreId);

			await using var connectionProvider = outboxUoW.ConnectionProvider;

			var added = AddLog(scopeContext, logMessage, idOutboxQueue, outboxUoW);
			if (!added)
			{
				_logger?.LogMessage(logMessage);
				return;
			}

			var saveResult = await outboxUoW.SaveAsync(scopeContext);
			saveResult.ThrowIfErrorOrNullData(scopeContext, null, true);

			var commitResult = connectionProvider.CommitAll(scopeContext);
			commitResult.ThrowIfErrorOrNullData(scopeContext, null, true);

			if (commitResult.Data != true)
				Throw.InvalidOperationException("Cannot commit transaction", scopeContext);
		}
		catch (Exception ex)
		{
			_logger?.LogErrorMessage(scopeContext, Exceptions.Internal.ErrorCodes.OutboxMessageProcessingService.FailedToWriteProcessingLog, x => x.ExceptionInfo(ex));
		}
	}
}
