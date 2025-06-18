using Legion.ADF.Messaging.Settings;
using Legion.Database;
using Legion.Infrastructure;
using Legion.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Legion.ADF.Messaging.MessageBox.Services.Internal;

internal static class Initializer
{
	public static async Task<Model.MessageBoxInstance> InitializeAsync(
		IScopeContext scopeContext,
		IServiceProvider serviceProvider,
		IConnectionProviderFactory connectionProviderFactory,
		MessageBoxMessageProcessingServiceOptions messageBoxMessageProcessingServiceOptions,
		MessagingMessageBoxStoreOptions storeOptions,
		ILogger? logger,
		CancellationToken cancellationToken = default)
	{
		Throw.IfArgumentNull(connectionProviderFactory);
		Throw.IfArgumentNull(serviceProvider);
		Throw.IfArgumentNull(messageBoxMessageProcessingServiceOptions);
		Throw.IfArgumentNull(storeOptions);

		scopeContext = scopeContext.CreateNew();

		try
		{
			await using var connectionProvider = connectionProviderFactory.CreateWithNewTransactionByStoreId<ConnectionStringProvider>(
				serviceProvider!,
				storeOptions.MessagingMessageBoxStoreId,
				transactionIsolationLevel: null,
				false,
				createAuditEntryStore: false);

			var messageBoxUowResult = connectionProvider.UnitOfWorkProvider.Create<IMessageBoxUnitOfWork>(scopeContext);

			if (messageBoxUowResult.HasError)
				messageBoxUowResult.ThrowIfErrorOrNullData(scopeContext, Legion.ADF.Messaging.Exceptions.Internal.ErrorCodes.MessageBoxUnitOfWorkException.InvalidUoW, true);

			var messageBoxUoW = messageBoxUowResult.Data!;

			var dbMessageBoxInstance = await messageBoxUoW.MessageBoxInstanceRepository
				.GetMessageBoxInstanceById(new Queries.MessageBoxInstance.GetMessageBoxInstanceByIdQuery(EnvironmentInfo.RUNTIME_UNIQUE_KEY, false, AsNoTracking: false))
				.ToResultAsync(scopeContext, cancellationToken);

			if (dbMessageBoxInstance != null)
				return dbMessageBoxInstance;

			var createResult = Model.MessageBoxInstance.Create(
				scopeContext,
				messageBoxMessageProcessingServiceOptions.MessageProcessingServiceName ?? EnvironmentInfoProviderCache.Instance.EntryAssemblyName ?? nameof(Initializer),
				messageBoxMessageProcessingServiceOptions.MessageProcessingServiceVersion ?? EnvironmentInfoProviderCache.Instance.EntryAssemblyVersion ?? "0.0.0.0",
				messageBoxMessageProcessingServiceOptions.MaxDegreeOfQueueParallelism,
				messageBoxMessageProcessingServiceOptions.MaxDegreeOfTopicParallelism,
				messageBoxMessageProcessingServiceOptions.LogLevel);

			createResult.ThrowIfErrorOrNullData(scopeContext, null, true);

			dbMessageBoxInstance = createResult.Data!;

			messageBoxUoW.MessageBoxInstanceRepository.Add(scopeContext, dbMessageBoxInstance);

			var messageTypeRegistry = serviceProvider.GetRequiredService<MessageTypeRegistry>();

			var registeredMessageTypes = messageTypeRegistry.GetAllMessageTypesClones();

			if (0 < registeredMessageTypes.Count)
			{
				var dbMessageTypes = await messageBoxUoW.MessageTypeRepository
					.GetAllMessageTypes(new Queries.MessageType.GetAllMessageTypesQuery(
						CheckReadPermissions: false,
						AsNoTracking: true,
						DisableCahce: true,
						QueryableBuilder: null))
					.ToResultAsync(scopeContext, cancellationToken);

				var existingDbMessageTypes = dbMessageTypes.Where(dbm => registeredMessageTypes.Any(rm => rm.Namespace == dbm.Namespace)).ToList();

				foreach (var existingDbMessageType in existingDbMessageTypes)
				{
					var reg = registeredMessageTypes.First(rm => rm.Namespace == existingDbMessageType.Namespace);
					existingDbMessageType.Update(scopeContext, reg.Code, reg.Name);
				}

				var newMessageTypes = registeredMessageTypes.Where(imt => !dbMessageTypes.Any(dbimt => dbimt.Namespace == imt.Namespace)).ToList();

				if (0 < newMessageTypes.Count)
				{
					messageBoxUoW.MessageTypeRepository.AddRange(scopeContext, newMessageTypes);

					dbMessageTypes.AddRange(newMessageTypes.Select(x => x.Clone(referenceModifier: Legion.Model.Mappers.ReferenceModifier.SetNull)!));
					messageTypeRegistry.ResetMessageTypes(scopeContext, dbMessageTypes);
				}
			}
			else
			{
				Throw.InvalidOperationException("messageTypes.Count == 0", scopeContext);
			}

			var queueRegistry = serviceProvider.GetRequiredService<QueueRegistry>();

			var registeredQueues = queueRegistry.GetAllQueueClones();

			if (0 < registeredQueues.Count)
			{
				var dbQueues = await messageBoxUoW.QueueRepository
					.GetAllQueues(new Queries.Queue.GetAllQueuesQuery(
						IncludeInactiveQueues: false,
						CheckReadPermissions: true,
						AsNoTracking: true,
						DisableCahce: true,
						QueryableBuilder: null))
					.ToResultAsync(scopeContext, cancellationToken);

				var existingDbQueues = dbQueues.Where(dbm => registeredQueues.Any(rm => rm.Name == dbm.Name)).ToList();

				foreach (var existingDbQueue in existingDbQueues)
				{
					var reg = dbQueues.First(rm => rm.Name == existingDbQueue.Name);
					existingDbQueue.Update(
						scopeContext,
						reg.IsSequentialFIFO,
						reg.MessagesBatchCount,
						reg.MaxDegreeOfParallelism,
						reg.TimeoutForMessageProcessing,
						reg.MaxMessageProcessingRetryCount,
						reg.Properties,
						reg.IdProcessingMode,
						reg.IdSuspendingMode);
				}

				var newQueues = registeredQueues.Where(q => !dbQueues.Any(dbq => dbq.Name == q.Name)).ToList();

				if (0 < newQueues.Count)
					messageBoxUoW.QueueRepository.AddRange(scopeContext, newQueues);

				queueRegistry.Lock();
			}

			var topicRegistry = serviceProvider.GetRequiredService<TopicRegistry>();

			var registeredTopics = topicRegistry.GetAllTopicsClones();

			if (0 < registeredTopics.Count)
			{
				var dbTopics = await messageBoxUoW.TopicRepository
					.GetAllTopics(new Queries.Topic.GetAllTopicsQuery(
						IncludeInactiveTopics: false,
						CheckReadPermissions: true,
						AsNoTracking: true,
						DisableCahce: true,
						QueryableBuilder: null))
					.ToResultAsync(scopeContext, cancellationToken);

				var existingDbTopics = dbTopics.Where(dbm => registeredTopics.Any(rm => rm.Name == dbm.Name)).ToList();

				foreach (var existingDbTopic in existingDbTopics)
				{
					var reg = dbTopics.First(rm => rm.Name == existingDbTopic.Name);
					existingDbTopic.Update(
						scopeContext,
						reg.IsSequentialFIFO,
						reg.MessagesBatchCount,
						reg.MaxDegreeOfParallelism,
						reg.TimeoutForMessageProcessing,
						reg.MaxMessageProcessingRetryCount,
						reg.Properties,
						reg.IdProcessingMode,
						reg.IdSuspendingMode);
				}

				var newTopics = registeredTopics.Where(q => !dbTopics.Any(dbq => dbq.Name == q.Name)).ToList();

				if (0 < newTopics.Count)
					messageBoxUoW.TopicRepository.AddRange(scopeContext, newTopics);

				var dbTopicSubscriptions = await messageBoxUoW.TopicSubscriptionRepository
					.GetAllTopicSubscriptions(new Queries.TopicSubscription.GetAllTopicSubscriptionsQuery(
						IncludeInactiveTopics: false,
						CheckReadPermissions: true,
						AsNoTracking: true,
						DisableCahce: true,
						QueryableBuilder: null))
					.ToResultAsync(scopeContext, cancellationToken);

				foreach (var topic in registeredTopics)
				{
					var registeredTopicSubscriptions = topicRegistry.GetAllTopicSubscriptioinsClones(topic.Name);

					var existingDbTopicSubscriptions = dbTopicSubscriptions.Where(dbm => registeredTopicSubscriptions.Any(rm => rm.SubscriptionName == dbm.SubscriptionName)).ToList();

					foreach (var existingDbTopicSubscription in existingDbTopicSubscriptions)
					{
						var reg = dbTopicSubscriptions.First(rm => rm.SubscriptionName == existingDbTopicSubscription.SubscriptionName);
						existingDbTopicSubscription.Update(
							scopeContext,
							reg.IsSequentialFIFO,
							reg.MessagesBatchCount,
							reg.MaxDegreeOfParallelism,
							reg.TimeoutForMessageProcessing,
							reg.MaxMessageProcessingRetryCount,
							reg.Properties,
							reg.IdProcessingMode,
							reg.IdSuspendingMode);
					}

					var newTopicSubscriptions = registeredTopicSubscriptions.Where(q => !dbTopicSubscriptions.Any(dbq => dbq.SubscriptionName == q.SubscriptionName)).ToList();

					if (0 < newTopicSubscriptions.Count)
						messageBoxUoW.TopicSubscriptionRepository.AddRange(scopeContext, newTopicSubscriptions);
				}

				topicRegistry.Lock();
			}

			if (registeredQueues.Count == 0 && registeredTopics.Count == 0)
			{
				Throw.InvalidOperationException("queues.Count == 0 && topics.Count == 0", scopeContext);
			}

			var saveResult = await messageBoxUoW.SaveAsync(scopeContext);
			saveResult.ThrowIfErrorOrNullData(scopeContext, Legion.ADF.Messaging.Exceptions.Internal.ErrorCodes.MessageBoxUnitOfWorkException.SaveFailed, true);

			var commitResult = connectionProvider.CommitAll(scopeContext);
			commitResult.ThrowIfErrorOrNullData(scopeContext, null, true);

			if (commitResult.Data != true)
				Throw.InvalidOperationException("Cannot commit transaction", scopeContext);

			return dbMessageBoxInstance;
		}
		catch (Exception ex)
		{
			var error = new ErrorMessageBuilder(scopeContext.CreateNew(), Exceptions.Internal.ErrorCodes.MessageBoxProcessingService.CannotCreateMessageBoxInstance)
				.LogLevel(LogLevel.Error)
				.ExceptionInfo(ex)
				.Build();

			try
			{
				await using var connectionProvider = connectionProviderFactory.CreateWithNewTransactionByStoreId<ConnectionStringProvider>(
					serviceProvider!,
					storeOptions.MessagingMessageBoxStoreId,
					transactionIsolationLevel: null,
					false,
					createAuditEntryStore: false);

				var messageBoxUowResult = connectionProvider.UnitOfWorkProvider.Create<IMessageBoxUnitOfWork>(scopeContext.CreateNew());

				if (messageBoxUowResult.HasError)
					messageBoxUowResult.ThrowIfErrorOrNullData(scopeContext.CreateNew(), Legion.ADF.Messaging.Exceptions.Internal.ErrorCodes.MessageBoxUnitOfWorkException.InvalidUoW, true);

				var messageBoxUoW = messageBoxUowResult.Data!;

				var added = false;

				if (messageBoxMessageProcessingServiceOptions.LogLevel <= error.LogLevel)
				{
					logger?.LogMessage(error);

					var result = Model.MessageBoxProcessingLog.Create(
						scopeContext.CreateNew(),
						idQueue: null,
						idTopic: null,
						idTopicSubscription: null,
						error);

					if (result.HasErrorOrNullData)
					{
						logger?.LogResultErrorMessages(result, true, true);
					}
					else
					{
						var messageBoxProcessingLog = result.Data;

						if (messageBoxProcessingLog != null)
						{
							messageBoxUoW.MessageBoxProcessingLogRepository.Add(scopeContext.CreateNew(), messageBoxProcessingLog);
							added = true;
						}
					}
				}

				if (!added)
				{
					logger?.LogMessage(error);
				}
				else
				{
					var saveResult = await messageBoxUoW.SaveAsync(scopeContext.CreateNew());
					saveResult.ThrowIfErrorOrNullData(scopeContext.CreateNew(), null, true);

					var commitResult = connectionProvider.CommitAll(scopeContext.CreateNew());
					commitResult.ThrowIfErrorOrNullData(scopeContext.CreateNew(), null, true);

					if (commitResult.Data != true)
						Throw.InvalidOperationException("Cannot commit transaction", scopeContext.CreateNew());
				}
			}
			catch (Exception ex2)
			{
				scopeContext = scopeContext.CreateNew();
				logger?.LogErrorMessage(scopeContext, Exceptions.Internal.ErrorCodes.MessageBoxProcessingService.FailedToWriteProcessingLog, x => x.ExceptionInfo(ex));
				logger?.LogErrorMessage(scopeContext, Exceptions.Internal.ErrorCodes.MessageBoxProcessingService.FailedToWriteProcessingLog, x => x.ExceptionInfo(ex2));
			}

			throw;
		}
	}
}
