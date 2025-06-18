using Legion.ADF.Messaging.Settings;
using Legion.Database;
using Legion.Infrastructure;
using Legion.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Legion.ADF.Messaging.Inbox.Services.Internal;

internal static class Initializer
{
	public static async Task<Model.InboxInstance> InitializeAsync(
		IScopeContext scopeContext,
		IServiceProvider serviceProvider,
		IConnectionProviderFactory connectionProviderFactory,
		InboxMessageProcessingServiceOptions inboxMessageProcessingServiceOptions,
		MessagingInboxStoreOptions storeOptions,
		ILogger? logger,
		CancellationToken cancellationToken = default)
	{
		Throw.IfArgumentNull(connectionProviderFactory);
		Throw.IfArgumentNull(serviceProvider);
		Throw.IfArgumentNull(inboxMessageProcessingServiceOptions);
		Throw.IfArgumentNull(storeOptions);

		scopeContext = scopeContext.CreateNew();

		try
		{
			await using var connectionProvider = connectionProviderFactory.CreateWithNewTransactionByStoreId<ConnectionStringProvider>(
				serviceProvider!,
				storeOptions.MessagingInboxStoreId,
				transactionIsolationLevel: null,
				false,
				createAuditEntryStore: false);

			var inboxUowResult = connectionProvider.UnitOfWorkProvider.Create<IInboxUnitOfWork>(scopeContext);

			if (inboxUowResult.HasError)
				inboxUowResult.ThrowIfErrorOrNullData(scopeContext, Legion.ADF.Messaging.Exceptions.Internal.ErrorCodes.InboxUnitOfWorkException.InvalidUoW, true);

			var inboxUoW = inboxUowResult.Data!;

			var dbInboxInstance = await inboxUoW.InboxInstanceRepository
				.GetInboxInstanceById(new Queries.InboxInstance.GetInboxInstanceByIdQuery(EnvironmentInfo.RUNTIME_UNIQUE_KEY, false, AsNoTracking: false))
				.ToResultAsync(scopeContext, cancellationToken);

			if (dbInboxInstance != null)
				return dbInboxInstance;

			var createResult = Model.InboxInstance.Create(
				scopeContext,
				inboxMessageProcessingServiceOptions.InboxMessageProcessingServiceName ?? EnvironmentInfoProviderCache.Instance.EntryAssemblyName ?? nameof(Initializer),
				inboxMessageProcessingServiceOptions.InboxMessageProcessingServiceVersion ?? EnvironmentInfoProviderCache.Instance.EntryAssemblyVersion ?? "0.0.0.0",
				inboxMessageProcessingServiceOptions.MaxDegreeOfQueueParallelism,
				inboxMessageProcessingServiceOptions.LogLevel);

			createResult.ThrowIfErrorOrNullData(scopeContext, null, true);

			dbInboxInstance = createResult.Data!;

			inboxUoW.InboxInstanceRepository.Add(scopeContext, dbInboxInstance);

			var inboxMessageTypeRegistry = serviceProvider.GetRequiredService<InboxMessageTypeRegistry>();

			var registeredMessageTypes = inboxMessageTypeRegistry.GetAllInboxMessageTypesClones();

			if (0 < registeredMessageTypes.Count)
			{
				var dbMessageTypes = await inboxUoW.InboxMessageTypeRepository
					.GetAllInboxMessageTypes(new Queries.InboxMessageType.GetAllInboxMessageTypesQuery(
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

				var newInboxMessageTypes = registeredMessageTypes.Where(imt => !dbMessageTypes.Any(dbimt => dbimt.Namespace == imt.Namespace)).ToList();

				if (0 < newInboxMessageTypes.Count)
				{
					inboxUoW.InboxMessageTypeRepository.AddRange(scopeContext, newInboxMessageTypes);

					dbMessageTypes.AddRange(newInboxMessageTypes.Select(x => x.Clone(referenceModifier: Legion.Model.Mappers.ReferenceModifier.SetNull)!));
					inboxMessageTypeRegistry.ResetInboxMessageTypes(scopeContext, dbMessageTypes);
				}
			}
			else
			{
				Throw.InvalidOperationException("messageTypes.Count == 0", scopeContext);
			}

			var inboxQueueRegistry = serviceProvider.GetRequiredService<InboxQueueRegistry>();

			var registeredQueues = inboxQueueRegistry.GetAllInboxQueueClones();

			if (0 < registeredQueues.Count)
			{
				var dbQueues = await inboxUoW.InboxQueueRepository
					.GetAllInboxQueues(new Queries.InboxQueue.GetAllInboxQueuesQuery(
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

				var newInboxQueues = registeredQueues.Where(iq => !dbQueues.Any(dbiq => dbiq.Name == iq.Name)).ToList();

				if (0 < newInboxQueues.Count)
					inboxUoW.InboxQueueRepository.AddRange(scopeContext, newInboxQueues);

				inboxQueueRegistry.Lock();
			}
			else
			{
				Throw.InvalidOperationException("queues.Count == 0", scopeContext);
			}

			var saveResult = await inboxUoW.SaveAsync(scopeContext);
			saveResult.ThrowIfErrorOrNullData(scopeContext, Legion.ADF.Messaging.Exceptions.Internal.ErrorCodes.InboxUnitOfWorkException.SaveFailed, true);

			var commitResult = connectionProvider.CommitAll(scopeContext);
			commitResult.ThrowIfErrorOrNullData(scopeContext, null, true);

			if (commitResult.Data != true)
				Throw.InvalidOperationException("Cannot commit transaction", scopeContext);

			return dbInboxInstance;
		}
		catch (Exception ex)
		{
			var error = new ErrorMessageBuilder(scopeContext.CreateNew(), Exceptions.Internal.ErrorCodes.InboxMessageProcessingService.CannotCreateInboxInstance)
				.LogLevel(LogLevel.Error)
				.ExceptionInfo(ex)
				.Build();

			try
			{
				await using var connectionProvider = connectionProviderFactory.CreateWithNewTransactionByStoreId<ConnectionStringProvider>(
					serviceProvider!,
					storeOptions.MessagingInboxStoreId,
					transactionIsolationLevel: null,
					false,
					createAuditEntryStore: false);

				var inboxUowResult = connectionProvider.UnitOfWorkProvider.Create<IInboxUnitOfWork>(scopeContext.CreateNew());

				if (inboxUowResult.HasError)
					inboxUowResult.ThrowIfErrorOrNullData(scopeContext.CreateNew(), Legion.ADF.Messaging.Exceptions.Internal.ErrorCodes.InboxUnitOfWorkException.InvalidUoW, true);

				var inboxUoW = inboxUowResult.Data!;

				var added = false;

				if (inboxMessageProcessingServiceOptions.LogLevel <= error.LogLevel)
				{
					logger?.LogMessage(error);

					var result = Model.InboxProcessingLog.Create(
						scopeContext.CreateNew(),
						idInboxQueue: null,
						error);

					if (result.HasErrorOrNullData)
					{
						logger?.LogResultErrorMessages(result, true, true);
					}
					else
					{
						var inboxProcessingLog = result.Data;

						if (inboxProcessingLog != null)
						{
							inboxUoW.InboxProcessingLogRepository.Add(scopeContext.CreateNew(), inboxProcessingLog);
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
					var saveResult = await inboxUoW.SaveAsync(scopeContext.CreateNew());
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
				logger?.LogErrorMessage(scopeContext, Exceptions.Internal.ErrorCodes.InboxMessageProcessingService.FailedToWriteProcessingLog, x => x.ExceptionInfo(ex));
				logger?.LogErrorMessage(scopeContext, Exceptions.Internal.ErrorCodes.InboxMessageProcessingService.FailedToWriteProcessingLog, x => x.ExceptionInfo(ex2));
			}

			throw;
		}
	}
}
