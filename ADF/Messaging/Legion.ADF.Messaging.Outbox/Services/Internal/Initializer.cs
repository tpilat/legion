using Legion.ADF.Messaging.Settings;
using Legion.Database;
using Legion.Infrastructure;
using Legion.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Legion.ADF.Messaging.Outbox.Services.Internal;

internal static class Initializer
{
	public static async Task<Model.OutboxInstance> InitializeAsync(
		IScopeContext scopeContext,
		IServiceProvider serviceProvider,
		IConnectionProviderFactory connectionProviderFactory,
		OutboxMessageProcessingServiceOptions outboxMessageProcessingServiceOptions,
		MessagingOutboxStoreOptions storeOptions,
		ILogger? logger,
		CancellationToken cancellationToken = default)
	{
		Throw.IfArgumentNull(connectionProviderFactory);
		Throw.IfArgumentNull(serviceProvider);
		Throw.IfArgumentNull(outboxMessageProcessingServiceOptions);
		Throw.IfArgumentNull(storeOptions);

		scopeContext = scopeContext.CreateNew();

		try
		{
			await using var connectionProvider = connectionProviderFactory.CreateWithNewTransactionByStoreId<ConnectionStringProvider>(
				serviceProvider!,
				storeOptions.MessagingOutboxStoreId,
				transactionIsolationLevel: null,
				false,
				createAuditEntryStore: false);

			var outboxUowResult = connectionProvider.UnitOfWorkProvider.Create<IOutboxUnitOfWork>(scopeContext);

			if (outboxUowResult.HasError)
				outboxUowResult.ThrowIfErrorOrNullData(scopeContext, Legion.ADF.Messaging.Exceptions.Internal.ErrorCodes.OutboxUnitOfWorkException.InvalidUoW, true);

			var outboxUoW = outboxUowResult.Data!;

			var dbOutboxInstance = await outboxUoW.OutboxInstanceRepository
				.GetOutboxInstanceById(new Queries.OutboxInstance.GetOutboxInstanceByIdQuery(EnvironmentInfo.RUNTIME_UNIQUE_KEY, false, AsNoTracking: false))
				.ToResultAsync(scopeContext, cancellationToken);

			if (dbOutboxInstance != null)
				return dbOutboxInstance;

			var createResult = Model.OutboxInstance.Create(
				scopeContext,
				outboxMessageProcessingServiceOptions.OutboxMessageProcessingServiceName ?? EnvironmentInfoProviderCache.Instance.EntryAssemblyName ?? nameof(Initializer),
				outboxMessageProcessingServiceOptions.OutboxMessageProcessingServiceVersion ?? EnvironmentInfoProviderCache.Instance.EntryAssemblyVersion ?? "0.0.0.0",
				outboxMessageProcessingServiceOptions.MaxDegreeOfQueueParallelism,
				outboxMessageProcessingServiceOptions.LogLevel);

			createResult.ThrowIfErrorOrNullData(scopeContext, null, true);

			dbOutboxInstance = createResult.Data!;

			outboxUoW.OutboxInstanceRepository.Add(scopeContext, dbOutboxInstance);

			var outboxMessageTypeRegistry = serviceProvider.GetRequiredService<OutboxMessageTypeRegistry>();

			var registeredMessageTypes = outboxMessageTypeRegistry.GetAllOutboxMessageTypesClones();

			if (0 < registeredMessageTypes.Count)
			{
				var dbMessageTypes = await outboxUoW.OutboxMessageTypeRepository
					.GetAllOutboxMessageTypes(new Queries.OutboxMessageType.GetAllOutboxMessageTypesQuery(
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

				var newOutboxMessageTypes = registeredMessageTypes.Where(imt => !dbMessageTypes.Any(dbimt => dbimt.Namespace == imt.Namespace)).ToList();

				if (0 < newOutboxMessageTypes.Count)
				{
					outboxUoW.OutboxMessageTypeRepository.AddRange(scopeContext, newOutboxMessageTypes);

					dbMessageTypes.AddRange(newOutboxMessageTypes.Select(x => x.Clone(referenceModifier: Legion.Model.Mappers.ReferenceModifier.SetNull)!));
					outboxMessageTypeRegistry.ResetOutboxMessageTypes(scopeContext, dbMessageTypes);
				}
			}
			else
			{
				Throw.InvalidOperationException("messageTypes.Count == 0", scopeContext);
			}

			var outboxQueueRegistry = serviceProvider.GetRequiredService<OutboxQueueRegistry>();

			var registeredQueues = outboxQueueRegistry.GetAllOutboxQueueClones();

			if (0 < registeredQueues.Count)
			{
				var dbQueues = await outboxUoW.OutboxQueueRepository
					.GetAllOutboxQueues(new Queries.OutboxQueue.GetAllOutboxQueuesQuery(
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

				var newOutboxQueues = registeredQueues.Where(iq => !dbQueues.Any(dbiq => dbiq.Name == iq.Name)).ToList();

				if (0 < newOutboxQueues.Count)
					outboxUoW.OutboxQueueRepository.AddRange(scopeContext, newOutboxQueues);

				outboxQueueRegistry.Lock();
			}
			else
			{
				Throw.InvalidOperationException("queues.Count == 0", scopeContext);
			}

			var saveResult = await outboxUoW.SaveAsync(scopeContext);
			saveResult.ThrowIfErrorOrNullData(scopeContext, Legion.ADF.Messaging.Exceptions.Internal.ErrorCodes.OutboxUnitOfWorkException.SaveFailed, true);

			var commitResult = connectionProvider.CommitAll(scopeContext);
			commitResult.ThrowIfErrorOrNullData(scopeContext, null, true);

			if (commitResult.Data != true)
				Throw.InvalidOperationException("Cannot commit transaction", scopeContext);

			return dbOutboxInstance;
		}
		catch (Exception ex)
		{
			var error = new ErrorMessageBuilder(scopeContext.CreateNew(), Exceptions.Internal.ErrorCodes.OutboxMessageProcessingService.CannotCreateOutboxInstance)
				.LogLevel(LogLevel.Error)
				.ExceptionInfo(ex)
				.Build();

			try
			{
				await using var connectionProvider = connectionProviderFactory.CreateWithNewTransactionByStoreId<ConnectionStringProvider>(
					serviceProvider!,
					storeOptions.MessagingOutboxStoreId,
					transactionIsolationLevel: null,
					false,
					createAuditEntryStore: false);

				var outboxUowResult = connectionProvider.UnitOfWorkProvider.Create<IOutboxUnitOfWork>(scopeContext.CreateNew());

				if (outboxUowResult.HasError)
					outboxUowResult.ThrowIfErrorOrNullData(scopeContext.CreateNew(), Legion.ADF.Messaging.Exceptions.Internal.ErrorCodes.OutboxUnitOfWorkException.InvalidUoW, true);

				var outboxUoW = outboxUowResult.Data!;

				var added = false;

				if (outboxMessageProcessingServiceOptions.LogLevel <= error.LogLevel)
				{
					logger?.LogMessage(error);

					var result = Model.OutboxProcessingLog.Create(
						scopeContext.CreateNew(),
						idOutboxQueue: null,
						error);

					if (result.HasErrorOrNullData)
					{
						logger?.LogResultErrorMessages(result, true, true);
					}
					else
					{
						var outboxProcessingLog = result.Data;

						if (outboxProcessingLog != null)
						{
							outboxUoW.OutboxProcessingLogRepository.Add(scopeContext.CreateNew(), outboxProcessingLog);
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
					var saveResult = await outboxUoW.SaveAsync(scopeContext.CreateNew());
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
				logger?.LogErrorMessage(scopeContext, Exceptions.Internal.ErrorCodes.OutboxMessageProcessingService.FailedToWriteProcessingLog, x => x.ExceptionInfo(ex));
				logger?.LogErrorMessage(scopeContext, Exceptions.Internal.ErrorCodes.OutboxMessageProcessingService.FailedToWriteProcessingLog, x => x.ExceptionInfo(ex2));
			}

			throw;
		}
	}
}
