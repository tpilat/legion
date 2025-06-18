using Legion.Queries.Sorting;
using System.ComponentModel;

namespace Legion.ADF.Messaging.Outbox.Services;

public partial class OutboxStore : IOutboxStore, IDisposable, IAsyncDisposable
{
	public async Task<IResult<QueryResult<List<Model.VwOutboxQueueMessages>>>> GetOutboxQueueMessagesAsync(
		IScopeContext scopeContext,
		bool includeInactiveQueues,
		ISortDescriptorBuilder<Model.VwOutboxQueueMessages> sortDescriptor,
		int pageIndex,
		int pageSize,
		bool checkPermissions,
		CancellationToken cancellationToken = default)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<QueryResult<List<Model.VwOutboxQueueMessages>>>();

		if (result.IsCancellationRequested(cancellationToken, scopeContext))
			return result.Build();

		if (result.IsDisposed(_disposed, scopeContext))
			return result.Build();

		if (result.IsArgumentNull(scopeContext, sortDescriptor))
			return result.Build();

		if (result.IsLessThan(scopeContext, pageIndex, 0))
			return result.Build();

		if (result.IsLessThanOrEqual(scopeContext, pageSize, 0))
			return result.Build();

		var data = await QUoW.VwOutboxQueueMessagesRepository
			.GetAllOutboxQueues(new Queries.VwOutboxQueueMessage.GetAllOutboxQueuesQuery(
					includeInactiveQueues,
					checkPermissions,
					AsNoTracking: true,
					DisableCahce: true,
					qb => qb
						.Sorting(s => s.Append(sortDescriptor, throwIfEmptySortStack: true))
						.Paging(pageIndex, pageSize)))
			.ToResultAsync(scopeContext, cancellationToken);

		var totalCount = await QUoW.VwOutboxQueueMessagesRepository
			.GetAllOutboxQueues(new Queries.VwOutboxQueueMessage.GetAllOutboxQueuesQuery(
					includeInactiveQueues,
					CheckReadPermissions: false,
					AsNoTracking: true,
					DisableCahce: true,
					QueryableBuilder: null))
			.TotalCountAsync(scopeContext, cancellationToken);

		return result.WithData(new QueryResult<List<Model.VwOutboxQueueMessages>>(data, totalCount)).Build();
	}

	public async Task<IResult<Model.VwOutboxQueue>> GetOutboxQueueAsync(
		IScopeContext scopeContext,
		Guid idOutboxQueue,
		ISortDescriptorBuilder<Model.VwOutboxQueue>? sortDescriptor,
		bool checkPermissions,
		CancellationToken cancellationToken = default)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<Model.VwOutboxQueue>();

		if (result.IsCancellationRequested(cancellationToken, scopeContext))
			return result.Build();

		if (result.IsDisposed(_disposed, scopeContext))
			return result.Build();

		var data = await QUoW.VwOutboxQueueRepository
			.GetVwOutboxQueueById(new Queries.VwOutboxQueue.GetVwOutboxQueueByIdQuery(idOutboxQueue, checkPermissions, AsNoTracking: true, DisableCahce: true,
				qb => qb.Sorting(s => s.Append(sortDescriptor, throwIfEmptySortStack: true))))
			.ToResultAsync(scopeContext, cancellationToken);

		return result.WithData(data).Build();
	}

	public async Task<IResult<QueryResult<List<Model.VwOutboxMessage>>>> GetAllOutboxMessagesAsync(
		IScopeContext scopeContext,
		Guid idOutboxQueue,
		ISortDescriptorBuilder<Model.VwOutboxMessage> sortDescriptor,
		int pageIndex,
		int pageSize,
		bool checkPermissions,
		CancellationToken cancellationToken = default)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<QueryResult<List<Model.VwOutboxMessage>>>();

		if (result.IsCancellationRequested(cancellationToken, scopeContext))
			return result.Build();

		if (result.IsDisposed(_disposed, scopeContext))
			return result.Build();

		if (result.IsArgumentNull(scopeContext, sortDescriptor))
			return result.Build();

		if (result.IsLessThan(scopeContext, pageIndex, 0))
			return result.Build();

		if (result.IsLessThanOrEqual(scopeContext, pageSize, 0))
			return result.Build();

		var data = await QUoW.VwOutboxMessageRepository
			.GetAllVwOutboxMessagesByIdQueue(
				new Queries.VwOutboxMessage.GetAllVwOutboxMessagesByIdQueueQuery(
					idOutboxQueue,
					checkPermissions,
					AsNoTracking: true,
					DisableCahce: true,
					qb => qb
						.Sorting(s => s.Append(sortDescriptor, throwIfEmptySortStack: true))
						.Paging(pageIndex, pageSize)))
			.ToResultAsync(scopeContext, cancellationToken);

		var totalCount = await QUoW.VwOutboxMessageRepository
			.GetAllVwOutboxMessagesByIdQueue(
				new Queries.VwOutboxMessage.GetAllVwOutboxMessagesByIdQueueQuery(
					idOutboxQueue,
					checkPermissions,
					AsNoTracking: true,
					DisableCahce: true,
					QueryableBuilder: null))
			.TotalCountAsync(scopeContext, cancellationToken);

		return result.WithData(new QueryResult<List<Model.VwOutboxMessage>>(data, totalCount)).Build();
	}

	public async Task<IResult<QueryResult<List<Model.VwOutboxMessageArchive>>>> GetAllOutboxMessageArchivesAsync(
		IScopeContext scopeContext,
		Guid idOutboxQueue,
		ISortDescriptorBuilder<Model.VwOutboxMessageArchive> sortDescriptor,
		int pageIndex,
		int pageSize,
		bool checkPermissions,
		CancellationToken cancellationToken = default)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<QueryResult<List<Model.VwOutboxMessageArchive>>>();

		if (result.IsCancellationRequested(cancellationToken, scopeContext))
			return result.Build();

		if (result.IsDisposed(_disposed, scopeContext))
			return result.Build();

		if (result.IsArgumentNull(scopeContext, sortDescriptor))
			return result.Build();

		if (result.IsLessThan(scopeContext, pageIndex, 0))
			return result.Build();

		if (result.IsLessThanOrEqual(scopeContext, pageSize, 0))
			return result.Build();

		var data = await QUoW.VwOutboxMessageArchiveRepository
			.GetAllVwOutboxMessageArchivesByIdQueue(
				new Queries.VwOutboxMessageArchive.GetAllVwOutboxMessageArchivesByIdQueueQuery(
					idOutboxQueue,
					checkPermissions,
					AsNoTracking: true,
					DisableCahce: true,
					qb => qb
						.Sorting(s => s.Append(sortDescriptor, throwIfEmptySortStack: true))
						.Paging(pageIndex, pageSize)))
			.ToResultAsync(scopeContext, cancellationToken);

		var totalCount = await QUoW.VwOutboxMessageArchiveRepository
			.GetAllVwOutboxMessageArchivesByIdQueue(
				new Queries.VwOutboxMessageArchive.GetAllVwOutboxMessageArchivesByIdQueueQuery(
					idOutboxQueue,
					checkPermissions,
					AsNoTracking: true,
					DisableCahce: true,
					QueryableBuilder: null))
			.TotalCountAsync(scopeContext, cancellationToken);

		return result.WithData(new QueryResult<List<Model.VwOutboxMessageArchive>>(data, totalCount)).Build();
	}

	public async Task<IResult<Model.VwOutboxMessage>> GetOutboxMessageAsync(
		IScopeContext scopeContext,
		Guid idOutboxMessage,
		bool checkPermissions,
		CancellationToken cancellationToken = default)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<Model.VwOutboxMessage>();

		if (result.IsCancellationRequested(cancellationToken, scopeContext))
			return result.Build();

		if (result.IsDisposed(_disposed, scopeContext))
			return result.Build();

		var data = await QUoW.VwOutboxMessageRepository
			.GetVwOutboxMessageByIdMessage(new Queries.VwOutboxMessage.GetVwOutboxMessageByIdMessageQuery(idOutboxMessage, checkPermissions, AsNoTracking: true))
			.ToResultAsync(scopeContext, cancellationToken);

		return result.WithData(data).Build();
	}

	public async Task<IResult<Model.VwOutboxMessageArchive>> GetOutboxMessageArchiveAsync(
		IScopeContext scopeContext,
		Guid idOutboxMessage,
		bool checkPermissions,
		CancellationToken cancellationToken = default)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<Model.VwOutboxMessageArchive>();

		if (result.IsCancellationRequested(cancellationToken, scopeContext))
			return result.Build();

		if (result.IsDisposed(_disposed, scopeContext))
			return result.Build();

		var data = await QUoW.VwOutboxMessageArchiveRepository
			.GetVwOutboxMessageArchiveByIdMessage(new Queries.VwOutboxMessageArchive.GetVwOutboxMessageArchiveByIdMessageQuery(idOutboxMessage, checkPermissions, AsNoTracking: true))
			.ToResultAsync(scopeContext, cancellationToken);

		return result.WithData(data).Build();
	}

	public async Task<IResult<Model.VwOutboxMessageContent>> GetOutboxMessageContentAsync(
		IScopeContext scopeContext,
		Guid idOutboxMessage,
		bool checkPermissions,
		CancellationToken cancellationToken = default)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<Model.VwOutboxMessageContent>();

		if (result.IsCancellationRequested(cancellationToken, scopeContext))
			return result.Build();

		if (result.IsDisposed(_disposed, scopeContext))
			return result.Build();

		var data = await QUoW.VwOutboxMessageContentRepository
			.GetVwOutboxMessageContentById(new Queries.VwOutboxMessageContent.GetVwOutboxMessageContentByIdQuery(idOutboxMessage, checkPermissions, AsNoTracking: true))
			.ToResultAsync(scopeContext, cancellationToken);

		return result.WithData(data).Build();
	}

	public async Task<IResult<List<Model.VwOutboxMessageProcessingLog>>> GetOutboxMessageProcessingLogsAsync(
		IScopeContext scopeContext,
		Guid idOutboxMessage,
		ISortDescriptorBuilder<Model.VwOutboxMessageProcessingLog>? sortDescriptor,
		bool checkPermissions,
		CancellationToken cancellationToken = default)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<List<Model.VwOutboxMessageProcessingLog>>();

		if (result.IsCancellationRequested(cancellationToken, scopeContext))
			return result.Build();

		if (result.IsDisposed(_disposed, scopeContext))
			return result.Build();

		var data = await QUoW.VwOutboxMessageProcessingLogRepository
			.GetVwOutboxMessageProcessingLogsByIdMessage(new Queries.VwOutboxMessageProcessingLog.GetVwOutboxMessageProcessingLogsByIdMessageQuery(idOutboxMessage, checkPermissions, AsNoTracking: true, DisableCahce: true,
				qb => qb.Sorting(s => s.Append(sortDescriptor, throwIfEmptySortStack: true))))
			.ToResultAsync(scopeContext, cancellationToken);

		return result.WithData(data).Build();
	}

	public async Task<IResult<List<Model.VwBlockedOutboxMessageType>>> GetBlockedOutboxMessageTypesAsync(
		IScopeContext scopeContext,
		ISortDescriptorBuilder<Model.VwBlockedOutboxMessageType>? sortDescriptor,
		bool checkPermissions,
		CancellationToken cancellationToken = default)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<List<Model.VwBlockedOutboxMessageType>>();

		if (result.IsCancellationRequested(cancellationToken, scopeContext))
			return result.Build();

		if (result.IsDisposed(_disposed, scopeContext))
			return result.Build();

		var data = await QUoW.VwBlockedOutboxMessageTypeRepository
			.GetAllVwBlockedOutboxMessageTypes(new Queries.VwBlockedOutboxMessageType.GetAllVwBlockedOutboxMessageTypesQuery(checkPermissions, AsNoTracking: true,
				qb => qb.Sorting(s => s.Append(sortDescriptor, throwIfEmptySortStack: true))))
			.ToResultAsync(scopeContext, cancellationToken);

		return result.WithData(data).Build();
	}

	public async Task<IResult<Guid>> CreateOutboxMessageAsync(
		IScopeContext scopeContext,
		DTOs.OutboxMessageDto outboxMessageDto,
		string queueName,
		bool checkMessageExists,
		bool checkPermissions,
		CancellationToken cancellationToken = default)
	{
		scopeContext = scopeContext.CreateNew()
			.AddContextProperty(nameof(outboxMessageDto.MessageTypeNamespace), outboxMessageDto?.MessageTypeNamespace)
			.AddContextProperty(nameof(queueName), queueName)
			.AddContextProperty(nameof(outboxMessageDto.MessageId), outboxMessageDto?.MessageId)
			.AddContextProperty(nameof(outboxMessageDto.BusinessId), outboxMessageDto?.BusinessId)
			.AddContextProperty(nameof(outboxMessageDto.SessionId), outboxMessageDto?.SessionId?.ToString())
			.AddContextProperty(nameof(outboxMessageDto.Publisher), outboxMessageDto?.Publisher)
			.AddContextProperty(nameof(outboxMessageDto.PublisherId), outboxMessageDto?.PublisherId);

		var result = new ResultBuilder<Guid>();

		if (result.IsCancellationRequested(cancellationToken, scopeContext))
			return result.Build();

		if (result.IsDisposed(_disposed, scopeContext))
			return result.Build();

		if (result.IsArgumentNull(scopeContext, outboxMessageDto))
			return result.Build();

		if (result.IsArgumentNullOrWhiteSpace(scopeContext, outboxMessageDto.MessageTypeNamespace))
			return result.Build();

		if (result.IsArgumentNullOrWhiteSpace(scopeContext, queueName))
			return result.Build();

		if (result.IsArgumentNullOrWhiteSpace(scopeContext, outboxMessageDto.MessageId))
			return result.Build();

		if (result.IsArgumentNull(scopeContext, outboxMessageDto.Content))
			return result.Build();

		if (checkPermissions)
		{
			var operationName = nameof(MessagingPermissions.OutboxMessage.SaveOutboxMessage);
			if (AccessControlManager?.IsAuthorizedFor(scopeContext, operationName, (Model.OutboxMessage?)null) == false)
				return result.WithUnauthorizedException(scopeContext, null, operationName);
		}

		var idOutboxMessageType = await UoW.OutboxMessageTypeRepository
			.GetOutboxMessageTypeByNamespace(new Queries.OutboxMessageType.GetOutboxMessageTypeByNamespaceQuery(outboxMessageDto.MessageTypeNamespace, checkPermissions, AsNoTracking: true))
			.GetIdOutboxMessageTypeAsync(scopeContext, cancellationToken);

		if (result.IsNullOrDefault(scopeContext, idOutboxMessageType, null, $"No outbox message type found by {nameof(Queries.OutboxMessageType.GetOutboxMessageTypeByNamespaceQuery)} for {nameof(outboxMessageDto.MessageTypeNamespace)} = {outboxMessageDto.MessageTypeNamespace}"))
			return result.Build();

		var idOutboxQueue = await UoW.OutboxQueueRepository
			.GetOutboxQueueByName(new Queries.OutboxQueue.GetOutboxQueueByNameQuery(queueName, checkPermissions, AsNoTracking: true))
			.GetIdOutboxQueueAsync(scopeContext, cancellationToken);

		if (result.IsNullOrDefault(scopeContext, idOutboxQueue, null, $"No outbox queue found by {nameof(Queries.OutboxQueue.GetOutboxQueueByNameQuery)} for {nameof(queueName)} = {queueName}"))
			return result.Build();

		if (checkMessageExists)
		{
			var existingIdOutboxMessage = await UoW.OutboxMessageRepository
				.ExistsOutboxMessageByQueueMessageId(new Queries.OutboxMessage.ExistsOutboxMessageByQueueMessageIdQuery(
					idOutboxQueue.Value,
					outboxMessageDto.MessageId!,
					checkPermissions,
					AsNoTracking: true))
				.GetIdOutboxMessageAsync(scopeContext, cancellationToken);

			if (existingIdOutboxMessage.HasValue && existingIdOutboxMessage != Guid.Empty)
			{
				var warningMessage = $"{nameof(Model.OutboxMessage)} with {nameof(outboxMessageDto.MessageId)} = {outboxMessageDto.MessageId} and {nameof(queueName)} = {queueName} already exists.";
				scopeContext.Logger?.LogWarningMessage(scopeContext, null, x => x.InternalMessage(warningMessage));
				result.WithWarning(scopeContext, null, warningMessage);
				return result.WithData(existingIdOutboxMessage.Value).Build();
			}
		}

		var createResult = Model.OutboxMessage.Create(
			scopeContext,
			idOutboxMessageType.Value,
			idOutboxQueue.Value,
			outboxMessageDto);

		if (result.MergeHasError(createResult))
			return result.Build();

		var dbOutboxMessage = createResult.Data!;

		UoW.OutboxMessageRepository.Add(scopeContext, dbOutboxMessage);

		var createLogResult = Model.OutboxMessageProcessingLog.Create(scopeContext, dbOutboxMessage);

		if (result.MergeHasError(createLogResult))
			return result.Build();

		UoW.OutboxMessageProcessingLogRepository.Add(scopeContext, createLogResult.Data!);

		var saveResult = await SaveInternalAsync(scopeContext, force: false, cancellationToken);
		result.MergeHasError(saveResult);
		return result.WithData(dbOutboxMessage.IdOutboxMessage).Build();
	}

	public async Task<IResult<Guid>> ArchivateOutboxMessageAsync(
		IScopeContext scopeContext,
		Guid idOutboxMessage,
		bool checkPermissions,
		CancellationToken cancellationToken = default)
	{
		scopeContext = scopeContext.CreateNew()
			.AddContextProperty(nameof(idOutboxMessage), idOutboxMessage.ToString());

		var result = new ResultBuilder<Guid>();

		if (result.IsCancellationRequested(cancellationToken, scopeContext))
			return result.Build();

		if (result.IsDisposed(_disposed, scopeContext))
			return result.Build();

		if (result.IsArgumentDefault(scopeContext, idOutboxMessage))
			return result.Build();

		var outboxMessage = await UoW.OutboxMessageRepository
			.GetOutboxMessageById(new Queries.OutboxMessage.GetOutboxMessageByIdQuery(idOutboxMessage, false, checkPermissions, AsNoTracking: false))
			.ToResultAsync(scopeContext, cancellationToken);

		if (result.IsNull(scopeContext, outboxMessage, null, nameof(Queries.OutboxMessage.GetOutboxMessageByIdQuery)))
			return result.Build();

		if (checkPermissions)
		{
			var operationName = nameof(MessagingPermissions.OutboxMessage.ArchivateOutboxMessage);
			if (AccessControlManager?.IsAuthorizedFor(scopeContext, operationName, outboxMessage) == false)
				return result.WithUnauthorizedException(scopeContext, null, operationName);
		}

		var createResult = Model.OutboxMessageArchive.Create(
			scopeContext,
			outboxMessage);

		if (result.MergeHasError(createResult))
			return result.Build();

		var dbOutboxMessageArchive = createResult.Data!;

		UoW.OutboxMessageArchiveRepository.Add(scopeContext, dbOutboxMessageArchive);

		UoW.OutboxMessageRepository.Remove(scopeContext, outboxMessage);

		var saveResult = await SaveInternalAsync(scopeContext, force: false, cancellationToken);
		result.MergeHasError(saveResult);
		return result.WithData(dbOutboxMessageArchive.IdOutboxMessage).Build();
	}

	public async Task<IResult> AddBlockedMessageTypesAsync(
		IScopeContext scopeContext,
		List<string> blockedNamespaces,
		bool checkPermissions,
		CancellationToken cancellationToken = default)
	{
		scopeContext = scopeContext.CreateNew()
			.AddContextProperty(nameof(blockedNamespaces), blockedNamespaces?.Count.ToString());

		var result = new ResultBuilder();

		if (result.IsCancellationRequested(cancellationToken, scopeContext))
			return result.Build();

		if (result.IsDisposed(_disposed, scopeContext))
			return result.Build();

		if (result.IsArgumentNullOrEmpty(scopeContext, blockedNamespaces))
			return result.Build();

		var dbBlockedOutboxMessageTypeNamespaces = await UoW.BlockedOutboxMessageTypeRepository
			.GetBlockedOutboxMessageTypesByNamespaces(new Queries.BlockedOutboxMessageType.GetBlockedOutboxMessageTypesByNamespacesQuery(blockedNamespaces, CheckReadPermissions: false, AsNoTracking: true))
			.ToNamespacesAsync(scopeContext, cancellationToken);

		foreach (var blockedNamespace in blockedNamespaces.Where(x => !dbBlockedOutboxMessageTypeNamespaces.Contains(x)))
		{
			var createResult = Model.BlockedOutboxMessageType.Create(
				scopeContext,
				blockedNamespace);

			if (result.MergeHasError(createResult))
				return result.Build();

			var dbBlockedOutboxMessageType = createResult.Data!;

			if (checkPermissions)
			{
				var operationName = nameof(MessagingPermissions.BlockedOutboxMessageType.SaveBlockedOutboxMessageType);
				if (AccessControlManager?.IsAuthorizedFor(scopeContext, operationName, dbBlockedOutboxMessageType) == false)
					return result.WithUnauthorizedException(scopeContext, null, operationName);
			}

			UoW.BlockedOutboxMessageTypeRepository.Add(scopeContext, dbBlockedOutboxMessageType);
		}

		var saveResult = await SaveInternalAsync(scopeContext, force: false, cancellationToken);
		result.MergeHasError(saveResult);
		return result.Build();
	}

	public async Task<IResult> RemoveBlockedMessageTypesAsync(
		IScopeContext scopeContext,
		List<string> blockedNamespaces,
		bool checkPermissions,
		CancellationToken cancellationToken = default)
	{
		scopeContext = scopeContext.CreateNew()
			.AddContextProperty(nameof(blockedNamespaces), blockedNamespaces?.Count.ToString());

		var result = new ResultBuilder();

		if (result.IsCancellationRequested(cancellationToken, scopeContext))
			return result.Build();

		if (result.IsDisposed(_disposed, scopeContext))
			return result.Build();

		if (result.IsArgumentNullOrEmpty(scopeContext, blockedNamespaces))
			return result.Build();

		var dbBlockedOutboxMessageTypes = await UoW.BlockedOutboxMessageTypeRepository
			.GetBlockedOutboxMessageTypesByNamespaces(new Queries.BlockedOutboxMessageType.GetBlockedOutboxMessageTypesByNamespacesQuery(blockedNamespaces, checkPermissions, AsNoTracking: false))
			.ToResultAsync(scopeContext, cancellationToken);

		foreach (var dbBlockedOutboxMessageType in dbBlockedOutboxMessageTypes)
		{
			if (checkPermissions)
			{
				var operationName = nameof(MessagingPermissions.BlockedOutboxMessageType.SaveBlockedOutboxMessageType);
				if (AccessControlManager?.IsAuthorizedFor(scopeContext, operationName, dbBlockedOutboxMessageType) == false)
					return result.WithUnauthorizedException(scopeContext, null, operationName);
			}

			UoW.BlockedOutboxMessageTypeRepository.Remove(scopeContext, dbBlockedOutboxMessageType);
		}

		var saveResult = await SaveInternalAsync(scopeContext, force: false, cancellationToken);
		result.MergeHasError(saveResult);
		return result.Build();
	}
}
