using Legion.ADF.Messaging.Settings;
using Legion.Database;
using Legion.Logging;
using Legion.MessageBus;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Legion.ADF.Messaging.MessageBox.Services.Internal;

public class SubscribedMessageProcessingService : BackgroundService
{
	private readonly MessageBoxMessageProcessingServiceOptions _options;
	private readonly IServiceProvider _serviceProvider;
	private readonly ILogger? _logger;

	public SubscribedMessageProcessingService(
		IOptions<MessageBoxMessageProcessingServiceOptions> options,
		IServiceProvider serviceProvider)
	{
		Throw.IfArgumentNull(options);
		Throw.IfArgumentNull(serviceProvider);

		_options = options.Value;
		_serviceProvider = serviceProvider;

		if (_options.LogToStandardILogger)
			_logger = serviceProvider.GetRequiredService<ILoggerFactory>().CreateLogger<SubscribedMessageProcessingService>();
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
		var scopeContextGlobal = ScopeContext.Create(nameof(SubscribedMessageProcessingService));

		Model.MessageBoxInstance messageBoxInstance = null!;

		var noTopic = false;
		while (!cancellationToken.IsCancellationRequested)
		{
			var scopeContext = ScopeContext.Create(scopeContextGlobal, correlationId: GlobalContext.Instance.NewGuid());

			if (_options.LogLevel <= LogLevel.Information)
				_logger?.LogInformationMessage(scopeContext, x => x.InternalMessage($"{nameof(SubscribedMessageProcessingService)}.{nameof(ExecuteAsync)}: START"));

			List<Model.Topic> topics = [];
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

				var topicRegistry = scopedServiceProvider.GetRequiredService<TopicRegistry>();

				await using (var connectionProvider = messageBoxUoW.ConnectionProvider)
				{
					var nowUtc = GlobalContext.Instance.UtcNow;
					topics = await messageBoxUoW.TopicRepository.GetAllTopicsByNames(
						new Queries.Topic.GetAllTopicsByNamesQuery(
							topicRegistry.GetAllTopicNames(),
							CheckReadPermissions: false,
							AsNoTracking: true,
							QueryableBuilder: null))
						.ToResultAsync(scopeContext, cancellationToken);

					if (topics.Count == 0)
					{
						noTopic = true;

						var warning = new LogMessageBuilder(scopeContext, Exceptions.Internal.ErrorCodes.InboxMessageProcessingService.Default)
							.LogLevel(LogLevel.Warning)
							.InternalMessage("topics.Count == 0")
							.Build();

						await SaveMessageBoxProcessingLogAsync(
							scopeContext,
							warning,
							idTopic: null,
							idTopicSubscription: null,
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

				var maxDegreeOfParallelism = messageBoxInstance.MaxDegreeOfTopicParallelism < 0
					? 1
					: (messageBoxInstance.MaxDegreeOfTopicParallelism == 0
						? Environment.ProcessorCount
						: messageBoxInstance.MaxDegreeOfTopicParallelism);

				if (1 < maxDegreeOfParallelism)
				{
					var parallelOptions = new ParallelOptions
					{
						MaxDegreeOfParallelism = maxDegreeOfParallelism,
						CancellationToken = cancellationToken
					};

					await Parallel.ForEachAsync(
						topics,
						parallelOptions,
						async (topic, cancelToken) =>
						{
							var processedMessagesCount = await ProcessTopicAsync(scopeContext, topic, blockedMessageNamespaces, scopedServiceProvider, connectionProviderFactory, storeOptions, cancelToken);
							if (0 < processedMessagesCount)
								processedAnyMessage = true;
						});
				}
				else
				{
					foreach (var topic in topics)
					{
						var processedMessagesCount = await ProcessTopicAsync(scopeContext, topic, blockedMessageNamespaces, scopedServiceProvider, connectionProviderFactory, storeOptions, cancellationToken);
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
					idTopic: null,
					idTopicSubscription: null,
					scopedServiceProvider,
					connectionProviderFactory,
					storeOptions,
					cancellationToken);
			}
			finally
			{
				if (noTopic)
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

	private async Task<int> ProcessTopicAsync(
		IScopeContext scopeContext,
		Model.Topic topic,
		IReadOnlyList<string> blockedMessageNamespaces,
		IServiceProvider serviceProvider,
		IConnectionProviderFactory connectionProviderFactory,
		MessagingMessageBoxStoreOptions storeOptions,
		CancellationToken cancellationToken = default)
	{
		scopeContext = scopeContext.CreateNew()
			.AddContextProperty(nameof(topic.IdTopic), topic.IdTopic.ToString());

		List<Model.TopicSubscription> topicSubscriptions = [];

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
				topicSubscriptions = await messageBoxUoW.TopicSubscriptionRepository.GetAllTopicSubscriptionsByTopic(
					new Queries.TopicSubscription.GetAllTopicSubscriptionsByTopicQuery(
						topic.IdTopic,
						CheckReadPermissions: true,
						AsNoTracking: true,
						DisableCahce: true,
						QueryableBuilder: null))
					.ToResultAsync(scopeContext, cancellationToken);
			}

			var maxDegreeOfParallelism = (!topic.MaxDegreeOfParallelism.HasValue || topic.MaxDegreeOfParallelism < 0)
				? 1
				: (topic.MaxDegreeOfParallelism == 0
					? Environment.ProcessorCount
					: (topic.MaxDegreeOfParallelism ?? 1));

			if (!topic.IsSequentialFIFO && 1 < maxDegreeOfParallelism)
			{
				var parallelOptions = new ParallelOptions
				{
					MaxDegreeOfParallelism = maxDegreeOfParallelism,
					CancellationToken = cancellationToken
				};

				await Parallel.ForEachAsync(
					topicSubscriptions,
					parallelOptions,
					async (topicSubscription, cancelToken) =>
					{
						await ProcessTopicSubscriptionAsync(scopeContext, topic, topicSubscription, blockedMessageNamespaces, serviceProvider, connectionProviderFactory, storeOptions, cancelToken);
					});
			}
			else
			{
				foreach (var topicSubscription in topicSubscriptions)
				{
					await ProcessTopicSubscriptionAsync(scopeContext, topic, topicSubscription, blockedMessageNamespaces, serviceProvider, connectionProviderFactory, storeOptions, cancellationToken);
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
				topic.IdTopic,
				idTopicSubscription: null,
				serviceProvider,
				connectionProviderFactory,
				storeOptions,
				cancellationToken);
		}

		return topicSubscriptions.Count;
	}

	private async Task<int> ProcessTopicSubscriptionAsync(
		IScopeContext scopeContext,
		Model.Topic topic,
		Model.TopicSubscription topicSubscription,
		IReadOnlyList<string> blockedMessageNamespaces,
		IServiceProvider serviceProvider,
		IConnectionProviderFactory connectionProviderFactory,
		MessagingMessageBoxStoreOptions storeOptions,
		CancellationToken cancellationToken = default)
	{
		scopeContext = scopeContext.CreateNew()
			.AddContextProperty(nameof(topic.IdTopic), topic.IdTopic.ToString())
			.AddContextProperty(nameof(topicSubscription.IdTopicSubscription), topicSubscription.IdTopicSubscription.ToString());

		Dictionary<Guid, DateTime> subscribedMessagesDict = [];

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
				subscribedMessagesDict = await messageBoxUoW.SubscribedMessageRepository.GetNextSubscribedMessagesBySubscription(
					new Queries.SubscribedMessage.GetNextSubscribedMessagesBySubscriptionQuery(
						topicSubscription.IdTopicSubscription,
						topicSubscription.IsSequentialFIFO,
						BatchCount: topicSubscription.MessagesBatchCount,
						NowUtc: nowUtc,
						CheckReadPermissions: false,
						AsNoTracking: true,
						QueryableBuilder: null))
					.ToSubscribedMessageIds(scopeContext, cancellationToken);
			}

			var maxDegreeOfParallelism = (!topicSubscription.MaxDegreeOfParallelism.HasValue || topicSubscription.MaxDegreeOfParallelism < 0)
				? 1
				: (topicSubscription.MaxDegreeOfParallelism == 0
					? Environment.ProcessorCount
					: (topicSubscription.MaxDegreeOfParallelism ?? 1));

			if (!topicSubscription.IsSequentialFIFO && 1 < maxDegreeOfParallelism)
			{
				var parallelOptions = new ParallelOptions
				{
					MaxDegreeOfParallelism = maxDegreeOfParallelism,
					CancellationToken = cancellationToken
				};

				await Parallel.ForEachAsync(
					subscribedMessagesDict,
					parallelOptions,
					async (kvp, cancelToken) =>
					{
						await ProcessMessageAsync(scopeContext, kvp.Key, topic, topicSubscription, blockedMessageNamespaces, serviceProvider, connectionProviderFactory, storeOptions, cancelToken);
					});
			}
			else
			{
				foreach (var kvp in subscribedMessagesDict)
				{
					if (topicSubscription.IsSequentialFIFO)
					{
						var nowUtc = GlobalContext.Instance.UtcNow;
						if (nowUtc < kvp.Value)
						{
							//wait for the message to be processed
							await Task.Delay(kvp.Value - nowUtc, cancellationToken);
						}
					}

					await ProcessMessageAsync(scopeContext, kvp.Key, topic, topicSubscription, blockedMessageNamespaces, serviceProvider, connectionProviderFactory, storeOptions, cancellationToken);
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
				topicSubscription.IdTopic,
				idTopicSubscription: null,
				serviceProvider,
				connectionProviderFactory,
				storeOptions,
				cancellationToken);
		}

		return subscribedMessagesDict.Count;
	}

	private async Task ProcessMessageAsync(
		IScopeContext scopeContext,
		Guid idSubscribedMessage,
		Model.Topic topic,
		Model.TopicSubscription topicSubscription,
		IReadOnlyList<string> blockedMessageNamespaces,
		IServiceProvider serviceProvider,
		IConnectionProviderFactory connectionProviderFactory,
		MessagingMessageBoxStoreOptions storeOptions,
		CancellationToken cancellationToken = default)
	{
		scopeContext = ScopeContext.Create(scopeContext, correlationId: GlobalContext.Instance.NewGuid())
			.AddContextProperty(nameof(idSubscribedMessage), idSubscribedMessage.ToString());

		try
		{
			Model.SubscribedMessage? subscribedMessage;
			Model.Message? message;

			var messageBoxUoW = CreateUnitOfWork(
				scopeContext,
				serviceProvider,
				connectionProviderFactory,
				storeOptions.MessagingMessageBoxStoreId);

			await using (var connectionProvider = messageBoxUoW.ConnectionProvider)
			{
				subscribedMessage = await messageBoxUoW.SubscribedMessageRepository.GetSubscribedMessageById(
					new Queries.SubscribedMessage.GetSubscribedMessageByIdQuery(
						idSubscribedMessage,
						CheckReadPermissions: false,
						AsNoTracking: false,
						QueryableBuilder: null))
					.ToResultAsync(scopeContext, cancellationToken);

				Throw.IfNull(subscribedMessage, scopeContext);

				message = await messageBoxUoW.MessageRepository.GetMessageById(
					new Queries.Message.GetMessageByIdQuery(
						subscribedMessage.IdMessage,
						IncludeContent: true,
						CheckReadPermissions: false,
						AsNoTracking: false,
						QueryableBuilder: null))
					.ToResultAsync(scopeContext, cancellationToken);

				Throw.IfNull(message, scopeContext);
				Throw.IfNull(message.MessageType, scopeContext);

				scopeContext = scopeContext
					.AddContextProperty(nameof(message.IdMessage), message.IdMessage.ToString());

				var setResult = subscribedMessage.SetProcessing(scopeContext, topic.TimeoutForMessageProcessing, _options.DisableMultiProcessingLog);
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
					idSubscribedMessage,
					topic.IdTopic,
					topicSubscription.IdTopicSubscription,
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
					idSubscribedMessage,
					topic.IdTopic,
					topicSubscription.IdTopicSubscription,
					warningMessage,
					serviceProvider,
					connectionProviderFactory,
					storeOptions,
					cancellationToken);

				return;
			}

			var topicRegistry = serviceProvider.GetRequiredService<TopicRegistry>();
			var messageReceivedEvent = topicRegistry.CreateTopicEvent(topic.Name, topicSubscription.ReceivedEventNamespace, message);
			if (messageReceivedEvent == null)
			{
				var errorMessage = new ErrorMessageBuilder(
					scopeContext,
					Exceptions.Internal.ErrorCodes.MessageBoxProcessingService.InvalidMessageBoxQueueReceivedEventType(topicSubscription.ReceivedEventNamespace))
					.Build();

				if (_options.LogLevel <= errorMessage.LogLevel)
					_logger?.LogErrorMessage(errorMessage);

				await SetSuspendedMessageAsync(
					scopeContext,
					idSubscribedMessage,
					topic.IdTopic,
					topicSubscription.IdTopicSubscription,
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
					idSubscribedMessage,
					topic.IdTopic,
					topicSubscription.IdTopicSubscription,
					topic.MaxMessageProcessingRetryCount,
					topic.TimeoutForMessageProcessing,
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
					Exceptions.Internal.ErrorCodes.MessageBoxProcessingService.MessageHasNoHandler(topicSubscription.ReceivedEventNamespace))
					.LogLevel(LogLevel.Warning)
					.Build();

				if (_options.LogLevel <= warningMessage.LogLevel)
					_logger?.LogWarningMessage(warningMessage);

				await SetNoHandlerAsync(
					scopeContext,
					idSubscribedMessage,
					topic.IdTopic,
					topicSubscription.IdTopicSubscription,
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
					idSubscribedMessage,
					topic.IdTopic,
					topicSubscription.IdTopicSubscription,
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
				topic.IdTopic,
				topicSubscription.IdTopicSubscription,
				serviceProvider,
				connectionProviderFactory,
				storeOptions,
				cancellationToken);
		}
	}

	private async Task SetProcessedMessageAsync(
		IScopeContext scopeContext,
		Guid idSubscribedMessage,
		Guid idTopic,
		Guid idTopicSubscription,
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

			var subscribedMessage = await messageBoxUoW.SubscribedMessageRepository.GetSubscribedMessageById(
				new Queries.SubscribedMessage.GetSubscribedMessageByIdQuery(
					idSubscribedMessage,
					CheckReadPermissions: false,
					AsNoTracking: false,
					QueryableBuilder: null))
				.ToResultAsync(scopeContext, cancellationToken);

			Throw.IfNull(subscribedMessage, scopeContext);

			var setResult = subscribedMessage.SetProcessed(scopeContext, false);
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
				idTopic,
				idTopicSubscription,
				serviceProvider,
				connectionProviderFactory,
				storeOptions,
				cancellationToken);
		}
	}

	private async Task SetUnknownTypeAsync(
		IScopeContext scopeContext,
		Guid idSubscribedMessage,
		Guid idTopic,
		Guid idTopicSubscription,
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

			var subscribedMessage = await messageBoxUoW.SubscribedMessageRepository.GetSubscribedMessageById(
				new Queries.SubscribedMessage.GetSubscribedMessageByIdQuery(
					idSubscribedMessage,
					CheckReadPermissions: false,
					AsNoTracking: false,
					QueryableBuilder: null))
				.ToResultAsync(scopeContext, cancellationToken);

			Throw.IfNull(subscribedMessage, scopeContext);

			var setResult = subscribedMessage.SetUnknownType(scopeContext, false, logMessage);
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
				idTopic,
				idTopicSubscription,
				serviceProvider,
				connectionProviderFactory,
				storeOptions,
				cancellationToken);
		}
	}

	private async Task SetNoHandlerAsync(
		IScopeContext scopeContext,
		Guid idSubscribedMessage,
		Guid idTopic,
		Guid idTopicSubscription,
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

			var subscribedMessage = await messageBoxUoW.SubscribedMessageRepository.GetSubscribedMessageById(
				new Queries.SubscribedMessage.GetSubscribedMessageByIdQuery(
					idSubscribedMessage,
					CheckReadPermissions: false,
					AsNoTracking: false,
					QueryableBuilder: null))
				.ToResultAsync(scopeContext, cancellationToken);

			Throw.IfNull(subscribedMessage, scopeContext);

			var setResult = subscribedMessage.SetNoHandler(scopeContext, false, logMessage);
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
				idTopic,
				idTopicSubscription,
				serviceProvider,
				connectionProviderFactory,
				storeOptions,
				cancellationToken);
		}
	}

	private async Task SetBlockedAsync(
		IScopeContext scopeContext,
		Guid idSubscribedMessage,
		Guid idTopic,
		Guid idTopicSubscription,
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

			var subscribedMessage = await messageBoxUoW.SubscribedMessageRepository.GetSubscribedMessageById(
				new Queries.SubscribedMessage.GetSubscribedMessageByIdQuery(
					idSubscribedMessage,
					CheckReadPermissions: false,
					AsNoTracking: false,
					QueryableBuilder: null))
				.ToResultAsync(scopeContext, cancellationToken);

			Throw.IfNull(subscribedMessage, scopeContext);

			var setResult = subscribedMessage.SetBlocked(scopeContext, false, logMessage);
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
				idTopic,
				idTopicSubscription,
				serviceProvider,
				connectionProviderFactory,
				storeOptions,
				cancellationToken);
		}
	}

	private async Task SetFailedMessageAsync(
		IScopeContext scopeContext,
		Guid idSubscribedMessage,
		Guid idTopic,
		Guid idTopicSubscription,
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

			var message = await messageBoxUoW.SubscribedMessageRepository.GetSubscribedMessageById(
				new Queries.SubscribedMessage.GetSubscribedMessageByIdQuery(
					idSubscribedMessage,
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
				idTopic,
				idTopicSubscription,
				serviceProvider,
				connectionProviderFactory,
				storeOptions,
				cancellationToken);
		}
	}

	private async Task SetSuspendedMessageAsync(
		IScopeContext scopeContext,
		Guid idSubscribedMessage,
		Guid idTopic,
		Guid idTopicSubscription,
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

			var subscribedMessage = await messageBoxUoW.SubscribedMessageRepository.GetSubscribedMessageById(
				new Queries.SubscribedMessage.GetSubscribedMessageByIdQuery(
					idSubscribedMessage,
					CheckReadPermissions: false,
					AsNoTracking: false,
					QueryableBuilder: null))
				.ToResultAsync(scopeContext, cancellationToken);

			Throw.IfNull(subscribedMessage, scopeContext);

			var setResult = subscribedMessage.SetSuspended(
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
				idTopic,
				idTopicSubscription,
				serviceProvider,
				connectionProviderFactory,
				storeOptions,
				cancellationToken);
		}
	}

	private Model.MessageBoxProcessingLog? CreateLog(
		IScopeContext scopeContext,
		ILogMessage logMessage,
		Guid? idTopic,
		Guid? idTopicSubscription)
	{
		if (logMessage == null)
			return null;

		if (logMessage.LogLevel < _options.LogLevel)
			return null;

		_logger?.LogMessage(logMessage);

		var result = Model.MessageBoxProcessingLog.Create(
			scopeContext,
			idQueue: null,
			idTopic,
			idTopicSubscription,
			logMessage);

		if (result.HasError)
			_logger?.LogResultErrorMessages(result, true, true);

		return result.Data;
	}

	private bool AddLog(
		IScopeContext scopeContext,
		ILogMessage logMessage,
		Guid? idTopic,
		Guid? idTopicSubscription,
		IMessageBoxUnitOfWork messageBoxUnitOfWork)
	{
		var messageBoxProcessingLog = CreateLog(scopeContext, logMessage, idTopic, idTopicSubscription);
		if (messageBoxProcessingLog == null)
			return false;

		messageBoxUnitOfWork.MessageBoxProcessingLogRepository.Add(scopeContext, messageBoxProcessingLog);
		return true;
	}

	private async Task SaveMessageBoxProcessingLogAsync(
		IScopeContext scopeContext,
		ILogMessage logMessage,
		Guid? idTopic,
		Guid? idTopicSubscription,
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

			var added = AddLog(scopeContext, logMessage, idTopic, idTopicSubscription, messageBoxUoW);
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
