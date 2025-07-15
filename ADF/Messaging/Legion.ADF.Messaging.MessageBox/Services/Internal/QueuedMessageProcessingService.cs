using Legion.ADF.Messaging.Settings;
using Legion.Database;
using Legion.Logging;
using Legion.MessageBus;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Legion.ADF.Messaging.MessageBox.Services.Internal;

public class QueuedMessageProcessingService : BackgroundService
{
	private readonly MessageBoxMessageProcessingServiceOptions _options;
	private readonly IServiceProvider _serviceProvider;
	private readonly ILogger? _logger;

	public QueuedMessageProcessingService(
		IOptions<MessageBoxMessageProcessingServiceOptions> options,
		IServiceProvider serviceProvider)
	{
		Throw.IfArgumentNull(options);
		Throw.IfArgumentNull(serviceProvider);

		_options = options.Value;
		_serviceProvider = serviceProvider;

		if (_options.LogToStandardILogger)
			_logger = serviceProvider.GetRequiredService<ILoggerFactory>().CreateLogger<QueuedMessageProcessingService>();
	}

	private IMessageBoxUnitOfWork CreateUnitOfWork(
		IScopeContext scopeContext,
		IServiceProvider serviceProvider,
		IConnectionProviderFactory connectionProviderFactory,
		string messagingMessageBoxStoreId)
	{
		var connectionProvider = connectionProviderFactory.CreateWithNewTransactionByStoreId<ConnectionStringProvider>(
			serviceProvider!,
			messagingMessageBoxStoreId,
			transactionIsolationLevel: null,
			false,
			createAuditEntryStore: false);

		var messageBoxUowResult = connectionProvider.UnitOfWorkProvider.Create<IMessageBoxUnitOfWork>(scopeContext);

		if (messageBoxUowResult.HasError)
			messageBoxUowResult.ThrowIfErrorOrNullData(scopeContext, Legion.ADF.Messaging.Exceptions.Internal.ErrorCodes.MessageBoxUnitOfWorkException.InvalidUoW, true);

		var uow = messageBoxUowResult.Data!;
		return uow;
	}

	protected override async Task ExecuteAsync(CancellationToken cancellationToken)
	{
		var scopeContextGlobal = ScopeContext.Create(nameof(QueuedMessageProcessingService));

		Model.MessageBoxInstance messageBoxInstance = null!;

		var noQueue = false;
		while (!cancellationToken.IsCancellationRequested)
		{
			var scopeContext = ScopeContext.Create(scopeContextGlobal, correlationId: GlobalContext.Instance.NewGuid());

			if (_options.LogLevel <= LogLevel.Information)
				_logger?.LogInformationMessage(scopeContext, x => x.InternalMessage($"{nameof(QueuedMessageProcessingService)}.{nameof(ExecuteAsync)}: START"));

			List<Model.Queue> queues = [];
			bool processedAnyMessage = false;

			try
			{
				await using var asyncServiceScope = _serviceProvider.CreateAsyncScope();
				var scopedServiceProvider = asyncServiceScope.ServiceProvider;

				var connectionProviderFactory = scopedServiceProvider.GetRequiredService<IConnectionProviderFactory>();
				var storeOptions = scopedServiceProvider.GetRequiredService<IOptions<MessagingMessageBoxStoreOptions>>().Value;

				var messageBoxUoW = CreateUnitOfWork(
					scopeContext,
					scopedServiceProvider,
					connectionProviderFactory,
					storeOptions.MessagingMessageBoxStoreId);

				IReadOnlyList<string> blockedMessageNamespaces;

				var queueRegistry = scopedServiceProvider.GetRequiredService<QueueRegistry>();

				await using (var connectionProvider = messageBoxUoW.ConnectionProvider)
				{
					var nowUtc = GlobalContext.Instance.UtcNow;
					queues = await messageBoxUoW.QueueRepository.GetAllQueuesByEvents(
						new Queries.Queue.GetAllQueuesByEventsQuery(
							queueRegistry.GetAllRegisterdReceivedEventNamespaces(),
							CheckReadPermissions: false,
							AsNoTracking: true,
							QueryableBuilder: null))
						.ToResultAsync(scopeContext, cancellationToken);

					if (queues.Count == 0)
					{
						noQueue = true;

						var warning = new LogMessageBuilder(scopeContext, Exceptions.Internal.ErrorCodes.InboxMessageProcessingService.Default)
							.LogLevel(LogLevel.Warning)
							.InternalMessage("queues.Count == 0")
							.Build();

						await SaveMessageBoxProcessingLogAsync(
							scopeContext,
							warning,
							idQueue: null,
							scopedServiceProvider,
							connectionProviderFactory,
							storeOptions,
							cancellationToken);

						return;
					}

					blockedMessageNamespaces = messageBoxUoW.BlockedMessageTypeRepository
						.GetAllBlockedMessageTypes(new Queries.BlockedMessageType.GetAllBlockedMessageTypesQuery(false, true, null))
						.ToNamespaces(scopeContext);

					var dbMessageBoxInstance = await messageBoxUoW.MessageBoxInstanceRepository
						.GetMessageBoxInstanceById(new Queries.MessageBoxInstance.GetMessageBoxInstanceByIdQuery(Infrastructure.EnvironmentInfo.RUNTIME_UNIQUE_KEY, false, AsNoTracking: false))
						.ToResultAsync(scopeContext, cancellationToken);

					Throw.IfNull(dbMessageBoxInstance, scopeContext);

					messageBoxInstance = dbMessageBoxInstance;
				}

				var maxDegreeOfParallelism = messageBoxInstance.MaxDegreeOfQueueParallelism < 0
					? 1
					: (messageBoxInstance.MaxDegreeOfQueueParallelism == 0
						? Environment.ProcessorCount
						: messageBoxInstance.MaxDegreeOfQueueParallelism);

				if (1 < maxDegreeOfParallelism)
				{
					var parallelOptions = new ParallelOptions
					{
						MaxDegreeOfParallelism = maxDegreeOfParallelism,
						CancellationToken = cancellationToken
					};

					await Parallel.ForEachAsync(
						queues,
						parallelOptions,
						async (queue, cancelToken) =>
						{
							var processedMessagesCount = await ProcessQueueAsync(scopeContext, queue, blockedMessageNamespaces, scopedServiceProvider, connectionProviderFactory, storeOptions, cancelToken);
							if (0 < processedMessagesCount)
								processedAnyMessage = true;
						});
				}
				else
				{
					foreach (var queue in queues)
					{
						var processedMessagesCount = await ProcessQueueAsync(scopeContext, queue, blockedMessageNamespaces, scopedServiceProvider, connectionProviderFactory, storeOptions, cancellationToken);
						if (0 < processedMessagesCount)
							processedAnyMessage = true;
					}
				}
			}
			catch (Exception ex)
			{
				var error = new ErrorMessageBuilder(scopeContext, Exceptions.Internal.ErrorCodes.MessageBoxProcessingService.Default)
					.LogLevel(LogLevel.Error)
					.ExceptionInfo(ex)
					.Build();

				await using var asyncServiceScope = _serviceProvider.CreateAsyncScope();
				var scopedServiceProvider = asyncServiceScope.ServiceProvider;

				var connectionProviderFactory = scopedServiceProvider.GetRequiredService<IConnectionProviderFactory>();
				var storeOptions = scopedServiceProvider.GetRequiredService<IOptions<MessagingMessageBoxStoreOptions>>().Value;

				await SaveMessageBoxProcessingLogAsync(
					scopeContext,
					error,
					idQueue: null,
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

	private async Task<int> ProcessQueueAsync(
		IScopeContext scopeContext,
		Model.Queue queue,
		IReadOnlyList<string> blockedMessageNamespaces,
		IServiceProvider serviceProvider,
		IConnectionProviderFactory connectionProviderFactory,
		MessagingMessageBoxStoreOptions storeOptions,
		CancellationToken cancellationToken = default)
	{
		scopeContext = scopeContext.CreateNew()
			.AddContextProperty(nameof(queue.IdQueue), queue.IdQueue.ToString());

		Dictionary<Guid, DateTime> messageIdsDict = [];

		try
		{
			var messageBoxUoW = CreateUnitOfWork(
				scopeContext,
				serviceProvider,
				connectionProviderFactory,
				storeOptions.MessagingMessageBoxStoreId);

			await using (var connectionProvider = messageBoxUoW.ConnectionProvider)
			{
				var nowUtc = GlobalContext.Instance.UtcNow;
				messageIdsDict = await messageBoxUoW.QueuedMessageRepository.GetNextQueuedMessagesByQueue(
					new Queries.QueuedMessage.GetNextQueuedMessagesByQueueQuery(
						queue.IdQueue,
						queue.IsSequentialFIFO,
						BatchCount: queue.MessagesBatchCount,
						NowUtc: nowUtc,
						CheckReadPermissions: false,
						AsNoTracking: true,
						QueryableBuilder: null))
					.ToMessageIds(scopeContext, cancellationToken);
			}

			var maxDegreeOfParallelism = (!queue.MaxDegreeOfParallelism.HasValue || queue.MaxDegreeOfParallelism < 0)
				? 1
				: (queue.MaxDegreeOfParallelism == 0
					? Environment.ProcessorCount
					: (queue.MaxDegreeOfParallelism ?? 1));

			if (!queue.IsSequentialFIFO && 1 < maxDegreeOfParallelism)
			{
				var parallelOptions = new ParallelOptions
				{
					MaxDegreeOfParallelism = maxDegreeOfParallelism,
					CancellationToken = cancellationToken
				};

				await Parallel.ForEachAsync(
					messageIdsDict,
					parallelOptions,
					async (kvp, cancelToken) =>
					{
						await ProcessMessageAsync(scopeContext, kvp.Key, queue, blockedMessageNamespaces, serviceProvider, connectionProviderFactory, storeOptions, cancelToken);
					});
			}
			else
			{
				foreach (var kvp in messageIdsDict)
				{
					if (queue.IsSequentialFIFO)
					{
						var nowUtc = GlobalContext.Instance.UtcNow;
						if (nowUtc < kvp.Value)
						{
							//wait for the message to be processed
							await Task.Delay(kvp.Value - nowUtc, cancellationToken);
						}
					}

					await ProcessMessageAsync(scopeContext, kvp.Key, queue, blockedMessageNamespaces, serviceProvider, connectionProviderFactory, storeOptions, cancellationToken);
				}
			}
		}
		catch (Exception ex)
		{
			var error = new ErrorMessageBuilder(scopeContext, Exceptions.Internal.ErrorCodes.MessageBoxProcessingService.Default)
				.LogLevel(LogLevel.Error)
				.ExceptionInfo(ex)
				.Build();

			await SaveMessageBoxProcessingLogAsync(
				scopeContext,
				error,
				queue.IdQueue,
				serviceProvider,
				connectionProviderFactory,
				storeOptions,
				cancellationToken);
		}

		return messageIdsDict.Count;
	}

	private async Task ProcessMessageAsync(
		IScopeContext scopeContext,
		Guid idQueuedMessage,
		Model.Queue queue,
		IReadOnlyList<string> blockedMessageNamespaces,
		IServiceProvider serviceProvider,
		IConnectionProviderFactory connectionProviderFactory,
		MessagingMessageBoxStoreOptions storeOptions,
		CancellationToken cancellationToken = default)
	{
		scopeContext = ScopeContext.Create(scopeContext, correlationId: GlobalContext.Instance.NewGuid())
			.AddContextProperty(nameof(idQueuedMessage), idQueuedMessage.ToString());

		try
		{
			Model.QueuedMessage? queuedMessage;
			Model.Message? message;

			var messageBoxUoW = CreateUnitOfWork(
				scopeContext,
				serviceProvider,
				connectionProviderFactory,
				storeOptions.MessagingMessageBoxStoreId);

			await using (var connectionProvider = messageBoxUoW.ConnectionProvider)
			{
				queuedMessage = await messageBoxUoW.QueuedMessageRepository.GetQueuedMessageById(
					new Queries.QueuedMessage.GetQueuedMessageByIdQuery(
						idQueuedMessage,
						CheckReadPermissions: false,
						AsNoTracking: false,
						QueryableBuilder: null))
					.ToResultAsync(scopeContext, cancellationToken);

				Throw.IfNull(queuedMessage, scopeContext);

				message = await messageBoxUoW.MessageRepository.GetMessageById(
					new Queries.Message.GetMessageByIdQuery(
						queuedMessage.IdMessage,
						IncludeContent: true,
						CheckReadPermissions: false,
						AsNoTracking: false,
						QueryableBuilder: null))
					.ToResultAsync(scopeContext, cancellationToken);

				Throw.IfNull(message, scopeContext);
				Throw.IfNull(message.MessageType, scopeContext);

				scopeContext = scopeContext
					.AddContextProperty(nameof(message.IdMessage), message.IdMessage.ToString());

				var setResult = queuedMessage.SetProcessing(scopeContext, queue.TimeoutForMessageProcessing, _options.DisableMultiProcessingLog);
				setResult.ThrowIfError(scopeContext, null, true);

				if (setResult.Data != null)
					messageBoxUoW.MessageProcessingLogRepository.Add(scopeContext, setResult.Data);

				var saveResult = await messageBoxUoW.SaveAsync(scopeContext);
				saveResult.ThrowIfErrorOrNullData(scopeContext, null, true);

				var commitResult = connectionProvider.CommitAll(scopeContext);
				commitResult.ThrowIfErrorOrNullData(scopeContext, null, true);

				if (commitResult.Data != true)
					Throw.InvalidOperationException("Cannot commit transaction", scopeContext);
			}

			if (blockedMessageNamespaces.Contains(message.MessageType.Namespace))
			{
				var warningMessage = new LogMessageBuilder(
					scopeContext,
					Exceptions.Internal.ErrorCodes.DomainEventProcessingService.BlockedDomainEvent(message.MessageType.Namespace))
					.LogLevel(LogLevel.Warning)
					.Build();

				if (_options.LogLevel <= warningMessage.LogLevel)
					_logger?.LogWarningMessage(warningMessage);

				await SetBlockedAsync(
					scopeContext,
					idQueuedMessage,
					queue.IdQueue,
					warningMessage,
					serviceProvider,
					connectionProviderFactory,
					storeOptions,
					cancellationToken);

				return;
			}

			var messageTypeRegistry = serviceProvider.GetRequiredService<MessageTypeRegistry>();
			var idMessageType = messageTypeRegistry.GetIdMessageType(message.MessageType.Namespace, true);

			if (!idMessageType.HasValue)
			{
				var warningMessage = new LogMessageBuilder(
					scopeContext,
					Exceptions.Internal.ErrorCodes.MessageBoxProcessingService.MessageHasUnknownType(message.MessageType.Namespace))
					.LogLevel(LogLevel.Warning)
					.Build();

				if (_options.LogLevel <= warningMessage.LogLevel)
					_logger?.LogWarningMessage(warningMessage);

				await SetUnknownTypeAsync(
					scopeContext,
					idQueuedMessage,
					queue.IdQueue,
					warningMessage,
					serviceProvider,
					connectionProviderFactory,
					storeOptions,
					cancellationToken);

				return;
			}

			var queueRegistry = serviceProvider.GetRequiredService<QueueRegistry>();
			var messageReceivedEvent = queueRegistry.CreateQueueEvent(queue.ReceivedEventNamespace, message);
			if (messageReceivedEvent == null)
			{
				var errorMessage = new ErrorMessageBuilder(
					scopeContext,
					Exceptions.Internal.ErrorCodes.MessageBoxProcessingService.InvalidMessageBoxQueueReceivedEventType(queue.ReceivedEventNamespace))
					.Build();

				if (_options.LogLevel <= errorMessage.LogLevel)
					_logger?.LogErrorMessage(errorMessage);

				await SetSuspendedMessageAsync(
					scopeContext,
					idQueuedMessage,
					queue.IdQueue,
					errorMessage,
					serviceProvider,
					connectionProviderFactory,
					storeOptions,
					cancellationToken);

				return;
			}

			var messageBus = serviceProvider.GetRequiredService<IMessageBus<ConnectionStringProvider>>();
			var publisResult = await messageBus.PublishAsync(scopeContext, messageReceivedEvent, connectionProvider: null, cancellationToken);
			if (publisResult.HasError)
			{
				if (_options.LogLevel <= LogLevel.Error)
					_logger?.LogResultErrorMessages(publisResult, true, true);

				var errorMessage = publisResult.ErrorMessages[0];

				await SetFailedMessageAsync(
					scopeContext,
					idQueuedMessage,
					queue.IdQueue,
					queue.MaxMessageProcessingRetryCount,
					queue.TimeoutForMessageProcessing,
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
					Exceptions.Internal.ErrorCodes.MessageBoxProcessingService.MessageHasNoHandler(queue.ReceivedEventNamespace))
					.LogLevel(LogLevel.Warning)
					.Build();

				if (_options.LogLevel <= warningMessage.LogLevel)
					_logger?.LogWarningMessage(warningMessage);

				await SetNoHandlerAsync(
					scopeContext,
					idQueuedMessage,
					queue.IdQueue,
					warningMessage,
					serviceProvider,
					connectionProviderFactory,
					storeOptions,
					cancellationToken);

				return;
			}
			else
			{
				await SetProcessedMessageAsync(
					scopeContext,
					idQueuedMessage,
					queue.IdQueue,
					serviceProvider,
					connectionProviderFactory,
					storeOptions,
					cancellationToken);

				return;
			}
		}
		catch (Exception ex)
		{
			var error = new ErrorMessageBuilder(scopeContext, Exceptions.Internal.ErrorCodes.MessageBoxProcessingService.CanNotSetState(nameof(Model.MessageProcessingStatus.Processing)))
				.LogLevel(LogLevel.Error)
				.ExceptionInfo(ex)
				.Build();

			await SaveMessageBoxProcessingLogAsync(
				scopeContext,
				error,
				queue.IdQueue,
				serviceProvider,
				connectionProviderFactory,
				storeOptions,
				cancellationToken);
		}
	}

	private async Task SetProcessedMessageAsync(
		IScopeContext scopeContext,
		Guid idQueuedMessage,
		Guid idQueue,
		IServiceProvider serviceProvider,
		IConnectionProviderFactory connectionProviderFactory,
		MessagingMessageBoxStoreOptions storeOptions,
		CancellationToken cancellationToken = default)
	{
		scopeContext = scopeContext.CreateNew();

		try
		{
			var messageBoxUoW = CreateUnitOfWork(
				scopeContext,
				serviceProvider,
				connectionProviderFactory,
				storeOptions.MessagingMessageBoxStoreId);

			await using var connectionProvider = messageBoxUoW.ConnectionProvider;

			var queuedMessage = await messageBoxUoW.QueuedMessageRepository.GetQueuedMessageById(
				new Queries.QueuedMessage.GetQueuedMessageByIdQuery(
					idQueuedMessage,
					CheckReadPermissions: false,
					AsNoTracking: false,
					QueryableBuilder: null))
				.ToResultAsync(scopeContext, cancellationToken);

			Throw.IfNull(queuedMessage, scopeContext);

			var setResult = queuedMessage.SetProcessed(scopeContext, false);
			setResult.ThrowIfError(scopeContext, null, true);

			if (setResult.Data != null)
				messageBoxUoW.MessageProcessingLogRepository.Add(scopeContext, setResult.Data);

			var saveResult = await messageBoxUoW.SaveAsync(scopeContext);
			saveResult.ThrowIfErrorOrNullData(scopeContext, null, true);

			var commitResult = connectionProvider.CommitAll(scopeContext);
			commitResult.ThrowIfErrorOrNullData(scopeContext, null, true);

			if (commitResult.Data != true)
				Throw.InvalidOperationException("Cannot commit transaction", scopeContext);
		}
		catch (Exception ex)
		{
			var error = new ErrorMessageBuilder(scopeContext, Exceptions.Internal.ErrorCodes.MessageBoxProcessingService.CanNotSetState(nameof(Model.MessageProcessingStatus.Processed)))
				.LogLevel(LogLevel.Error)
				.ExceptionInfo(ex)
				.Build();

			await SaveMessageBoxProcessingLogAsync(
				scopeContext,
				error,
				idQueue,
				serviceProvider,
				connectionProviderFactory,
				storeOptions,
				cancellationToken);
		}
	}

	private async Task SetUnknownTypeAsync(
		IScopeContext scopeContext,
		Guid idQueuedMessage,
		Guid idQueue,
		ILogMessage? logMessage,
		IServiceProvider serviceProvider,
		IConnectionProviderFactory connectionProviderFactory,
		MessagingMessageBoxStoreOptions storeOptions,
		CancellationToken cancellationToken = default)
	{
		scopeContext = scopeContext.CreateNew();

		try
		{
			var messageBoxUoW = CreateUnitOfWork(
				scopeContext,
				serviceProvider,
				connectionProviderFactory,
				storeOptions.MessagingMessageBoxStoreId);

			await using var connectionProvider = messageBoxUoW.ConnectionProvider;

			var queuedMessage = await messageBoxUoW.QueuedMessageRepository.GetQueuedMessageById(
				new Queries.QueuedMessage.GetQueuedMessageByIdQuery(
					idQueuedMessage,
					CheckReadPermissions: false,
					AsNoTracking: false,
					QueryableBuilder: null))
				.ToResultAsync(scopeContext, cancellationToken);

			Throw.IfNull(queuedMessage, scopeContext);

			var setResult = queuedMessage.SetUnknownType(scopeContext, false, logMessage);
			setResult.ThrowIfError(scopeContext, null, true);

			if (setResult.Data != null)
				messageBoxUoW.MessageProcessingLogRepository.Add(scopeContext, setResult.Data);

			var saveResult = await messageBoxUoW.SaveAsync(scopeContext);
			saveResult.ThrowIfErrorOrNullData(scopeContext, null, true);

			var commitResult = connectionProvider.CommitAll(scopeContext);
			commitResult.ThrowIfErrorOrNullData(scopeContext, null, true);

			if (commitResult.Data != true)
				Throw.InvalidOperationException("Cannot commit transaction", scopeContext);
		}
		catch (Exception ex)
		{
			var error = new ErrorMessageBuilder(scopeContext, Exceptions.Internal.ErrorCodes.MessageBoxProcessingService.CanNotSetState(nameof(Model.MessageProcessingStatus.UnknownType)))
				.LogLevel(LogLevel.Error)
				.ExceptionInfo(ex)
				.Build();

			await SaveMessageBoxProcessingLogAsync(
				scopeContext,
				error,
				idQueue,
				serviceProvider,
				connectionProviderFactory,
				storeOptions,
				cancellationToken);
		}
	}

	private async Task SetNoHandlerAsync(
		IScopeContext scopeContext,
		Guid idQueuedMessage,
		Guid idQueue,
		ILogMessage? logMessage,
		IServiceProvider serviceProvider,
		IConnectionProviderFactory connectionProviderFactory,
		MessagingMessageBoxStoreOptions storeOptions,
		CancellationToken cancellationToken = default)
	{
		scopeContext = scopeContext.CreateNew();

		try
		{
			var messageBoxUoW = CreateUnitOfWork(
				scopeContext,
				serviceProvider,
				connectionProviderFactory,
				storeOptions.MessagingMessageBoxStoreId);

			await using var connectionProvider = messageBoxUoW.ConnectionProvider;

			var queuedMessage = await messageBoxUoW.QueuedMessageRepository.GetQueuedMessageById(
				new Queries.QueuedMessage.GetQueuedMessageByIdQuery(
					idQueuedMessage,
					CheckReadPermissions: false,
					AsNoTracking: false,
					QueryableBuilder: null))
				.ToResultAsync(scopeContext, cancellationToken);

			Throw.IfNull(queuedMessage, scopeContext);

			var setResult = queuedMessage.SetNoHandler(scopeContext, false, logMessage);
			setResult.ThrowIfError(scopeContext, null, true);

			if (setResult.Data != null)
				messageBoxUoW.MessageProcessingLogRepository.Add(scopeContext, setResult.Data);

			var saveResult = await messageBoxUoW.SaveAsync(scopeContext);
			saveResult.ThrowIfErrorOrNullData(scopeContext, null, true);

			var commitResult = connectionProvider.CommitAll(scopeContext);
			commitResult.ThrowIfErrorOrNullData(scopeContext, null, true);

			if (commitResult.Data != true)
				Throw.InvalidOperationException("Cannot commit transaction", scopeContext);
		}
		catch (Exception ex)
		{
			var error = new ErrorMessageBuilder(scopeContext, Exceptions.Internal.ErrorCodes.MessageBoxProcessingService.CanNotSetState(nameof(Model.MessageProcessingStatus.NoHandler)))
				.LogLevel(LogLevel.Error)
				.ExceptionInfo(ex)
				.Build();

			await SaveMessageBoxProcessingLogAsync(
				scopeContext,
				error,
				idQueue,
				serviceProvider,
				connectionProviderFactory,
				storeOptions,
				cancellationToken);
		}
	}

	private async Task SetBlockedAsync(
		IScopeContext scopeContext,
		Guid idQueuedMessage,
		Guid idQueue,
		ILogMessage? logMessage,
		IServiceProvider serviceProvider,
		IConnectionProviderFactory connectionProviderFactory,
		MessagingMessageBoxStoreOptions storeOptions,
		CancellationToken cancellationToken = default)
	{
		scopeContext = scopeContext.CreateNew();

		try
		{
			var messageBoxUoW = CreateUnitOfWork(
				scopeContext,
				serviceProvider,
				connectionProviderFactory,
				storeOptions.MessagingMessageBoxStoreId);

			await using var connectionProvider = messageBoxUoW.ConnectionProvider;

			var queuedMessage = await messageBoxUoW.QueuedMessageRepository.GetQueuedMessageById(
				new Queries.QueuedMessage.GetQueuedMessageByIdQuery(
					idQueuedMessage,
					CheckReadPermissions: false,
					AsNoTracking: false,
					QueryableBuilder: null))
				.ToResultAsync(scopeContext, cancellationToken);

			Throw.IfNull(queuedMessage, scopeContext);

			var setResult = queuedMessage.SetBlocked(scopeContext, false, logMessage);
			setResult.ThrowIfError(scopeContext, null, true);

			if (setResult.Data != null)
				messageBoxUoW.MessageProcessingLogRepository.Add(scopeContext, setResult.Data);

			var saveResult = await messageBoxUoW.SaveAsync(scopeContext);
			saveResult.ThrowIfErrorOrNullData(scopeContext, null, true);

			var commitResult = connectionProvider.CommitAll(scopeContext);
			commitResult.ThrowIfErrorOrNullData(scopeContext, null, true);

			if (commitResult.Data != true)
				Throw.InvalidOperationException("Cannot commit transaction", scopeContext);
		}
		catch (Exception ex)
		{
			var error = new ErrorMessageBuilder(scopeContext, Exceptions.Internal.ErrorCodes.MessageBoxProcessingService.CanNotSetState(nameof(Model.MessageProcessingStatus.Blocked)))
				.LogLevel(LogLevel.Error)
				.ExceptionInfo(ex)
				.Build();

			await SaveMessageBoxProcessingLogAsync(
				scopeContext,
				error,
				idQueue,
				serviceProvider,
				connectionProviderFactory,
				storeOptions,
				cancellationToken);
		}
	}

	private async Task SetFailedMessageAsync(
		IScopeContext scopeContext,
		Guid idQueuedMessage,
		Guid idQueue,
		int maxRetryCount,
		TimeSpan nextProcessingDelay,
		IErrorMessage errorMessage,
		IServiceProvider serviceProvider,
		IConnectionProviderFactory connectionProviderFactory,
		MessagingMessageBoxStoreOptions storeOptions,
		CancellationToken cancellationToken = default)
	{
		scopeContext = scopeContext.CreateNew();

		try
		{
			var messageBoxUoW = CreateUnitOfWork(
				scopeContext,
				serviceProvider,
				connectionProviderFactory,
				storeOptions.MessagingMessageBoxStoreId);

			await using var connectionProvider = messageBoxUoW.ConnectionProvider;

			var message = await messageBoxUoW.QueuedMessageRepository.GetQueuedMessageById(
				new Queries.QueuedMessage.GetQueuedMessageByIdQuery(
					idQueuedMessage,
					CheckReadPermissions: false,
					AsNoTracking: false,
					QueryableBuilder: null))
				.ToResultAsync(scopeContext, cancellationToken);

			Throw.IfNull(message, scopeContext);

			var setResult = message.SetFailed(
				scopeContext,
				maxRetryCount,
				nextProcessingDelay,
				false,
				errorMessage);
			setResult.ThrowIfError(scopeContext, null, true);

			if (setResult.Data != null)
				messageBoxUoW.MessageProcessingLogRepository.Add(scopeContext, setResult.Data);

			var saveResult = await messageBoxUoW.SaveAsync(scopeContext);
			saveResult.ThrowIfErrorOrNullData(scopeContext, null, true);

			var commitResult = connectionProvider.CommitAll(scopeContext);
			commitResult.ThrowIfErrorOrNullData(scopeContext, null, true);

			if (commitResult.Data != true)
				Throw.InvalidOperationException("Cannot commit transaction", scopeContext);
		}
		catch (Exception ex)
		{
			var error = new ErrorMessageBuilder(scopeContext, Exceptions.Internal.ErrorCodes.MessageBoxProcessingService.CanNotSetState(nameof(Model.MessageProcessingStatus.Failed)))
				.LogLevel(LogLevel.Error)
				.ExceptionInfo(ex)
				.Build();

			await SaveMessageBoxProcessingLogAsync(
				scopeContext,
				error,
				idQueue,
				serviceProvider,
				connectionProviderFactory,
				storeOptions,
				cancellationToken);
		}
	}

	private async Task SetSuspendedMessageAsync(
		IScopeContext scopeContext,
		Guid idQueuedMessage,
		Guid idQueue,
		IErrorMessage errorMessage,
		IServiceProvider serviceProvider,
		IConnectionProviderFactory connectionProviderFactory,
		MessagingMessageBoxStoreOptions storeOptions,
		CancellationToken cancellationToken = default)
	{
		scopeContext = scopeContext.CreateNew();

		try
		{
			var messageBoxUoW = CreateUnitOfWork(
				scopeContext,
				serviceProvider,
				connectionProviderFactory,
				storeOptions.MessagingMessageBoxStoreId);

			await using var connectionProvider = messageBoxUoW.ConnectionProvider;

			var queuedMessage = await messageBoxUoW.QueuedMessageRepository.GetQueuedMessageById(
				new Queries.QueuedMessage.GetQueuedMessageByIdQuery(
					idQueuedMessage,
					CheckReadPermissions: false,
					AsNoTracking: false,
					QueryableBuilder: null))
				.ToResultAsync(scopeContext, cancellationToken);

			Throw.IfNull(queuedMessage, scopeContext);

			var setResult = queuedMessage.SetSuspended(
				scopeContext,
				false,
				errorMessage);
			setResult.ThrowIfError(scopeContext, null, true);

			if (setResult.Data != null)
				messageBoxUoW.MessageProcessingLogRepository.Add(scopeContext, setResult.Data);

			var saveResult = await messageBoxUoW.SaveAsync(scopeContext);
			saveResult.ThrowIfErrorOrNullData(scopeContext, null, true);

			var commitResult = connectionProvider.CommitAll(scopeContext);
			commitResult.ThrowIfErrorOrNullData(scopeContext, null, true);

			if (commitResult.Data != true)
				Throw.InvalidOperationException("Cannot commit transaction", scopeContext);
		}
		catch (Exception ex)
		{
			var error = new ErrorMessageBuilder(scopeContext, Exceptions.Internal.ErrorCodes.MessageBoxProcessingService.CanNotSetState(nameof(Model.MessageProcessingStatus.Suspended)))
				.LogLevel(LogLevel.Error)
				.ExceptionInfo(ex)
				.Build();

			await SaveMessageBoxProcessingLogAsync(
				scopeContext,
				error,
				idQueue,
				serviceProvider,
				connectionProviderFactory,
				storeOptions,
				cancellationToken);
		}
	}

	private Model.MessageBoxProcessingLog? CreateLog(
		IScopeContext scopeContext,
		ILogMessage logMessage,
		Guid? idQueue)
	{
		if (logMessage == null)
			return null;

		if (logMessage.LogLevel < _options.LogLevel)
			return null;

		_logger?.LogMessage(logMessage);

		var result = Model.MessageBoxProcessingLog.Create(
			scopeContext,
			idQueue,
			idTopic: null,
			idTopicSubscription: null,
			logMessage);

		if (result.HasError)
			_logger?.LogResultErrorMessages(result, true, true);

		return result.Data;
	}

	private bool AddLog(
		IScopeContext scopeContext,
		ILogMessage logMessage,
		Guid? idQueue,
		IMessageBoxUnitOfWork messageBoxUnitOfWork)
	{
		var messageBoxProcessingLog = CreateLog(scopeContext, logMessage, idQueue);
		if (messageBoxProcessingLog == null)
			return false;

		messageBoxUnitOfWork.MessageBoxProcessingLogRepository.Add(scopeContext, messageBoxProcessingLog);
		return true;
	}

	private async Task SaveMessageBoxProcessingLogAsync(
		IScopeContext scopeContext,
		ILogMessage logMessage,
		Guid? idQueue,
		IServiceProvider serviceProvider,
		IConnectionProviderFactory connectionProviderFactory,
		MessagingMessageBoxStoreOptions storeOptions,
		CancellationToken cancellationToken = default)
	{
		scopeContext = scopeContext.CreateNew();

		try
		{
			var messageBoxUoW = CreateUnitOfWork(
				scopeContext,
				serviceProvider,
				connectionProviderFactory,
				storeOptions.MessagingMessageBoxStoreId);

			await using var connectionProvider = messageBoxUoW.ConnectionProvider;

			var added = AddLog(scopeContext, logMessage, idQueue, messageBoxUoW);
			if (!added)
			{
				_logger?.LogMessage(logMessage);
				return;
			}

			var saveResult = await messageBoxUoW.SaveAsync(scopeContext);
			saveResult.ThrowIfErrorOrNullData(scopeContext, null, true);

			var commitResult = connectionProvider.CommitAll(scopeContext);
			commitResult.ThrowIfErrorOrNullData(scopeContext, null, true);

			if (commitResult.Data != true)
				Throw.InvalidOperationException("Cannot commit transaction", scopeContext);
		}
		catch (Exception ex)
		{
			_logger?.LogErrorMessage(scopeContext, Exceptions.Internal.ErrorCodes.MessageBoxProcessingService.FailedToWriteProcessingLog, x => x.ExceptionInfo(ex));
		}
	}
}
