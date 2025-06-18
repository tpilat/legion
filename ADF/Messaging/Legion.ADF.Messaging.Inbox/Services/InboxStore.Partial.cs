using Legion.Queries.Sorting;

namespace Legion.ADF.Messaging.Inbox.Services;

public partial class InboxStore : IInboxStore, IDisposable, IAsyncDisposable
{
	public async Task<IResult<QueryResult<List<Model.VwInboxQueueMessages>>>> GetInboxQueueMessagesAsync(
		IScopeContext scopeContext,
		bool includeInactiveQueues,
		ISortDescriptorBuilder<Model.VwInboxQueueMessages> sortDescriptor,
		int pageIndex,
		int pageSize,
		bool checkPermissions,
		CancellationToken cancellationToken = default)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<QueryResult<List<Model.VwInboxQueueMessages>>>();

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

		var data = await QUoW.VwInboxQueueMessagesRepository
			.GetAllInboxQueues(new Queries.VwInboxQueueMessage.GetAllInboxQueuesQuery(
					includeInactiveQueues,
					checkPermissions,
					AsNoTracking: true,
					DisableCahce: true,
					qb => qb
						.Sorting(s => s.Append(sortDescriptor, throwIfEmptySortStack: true))
						.Paging(pageIndex, pageSize)))
			.ToResultAsync(scopeContext, cancellationToken);

		var totalCount = await QUoW.VwInboxQueueMessagesRepository
			.GetAllInboxQueues(new Queries.VwInboxQueueMessage.GetAllInboxQueuesQuery(
					includeInactiveQueues,
					CheckReadPermissions: false,
					AsNoTracking: true,
					DisableCahce: true,
					QueryableBuilder: null))
			.TotalCountAsync(scopeContext, cancellationToken);

		return result.WithData(new QueryResult<List<Model.VwInboxQueueMessages>>(data, totalCount)).Build();
	}

	public async Task<IResult<Model.VwInboxQueue>> GetInboxQueueAsync(
		IScopeContext scopeContext,
		Guid idInboxQueue,
		ISortDescriptorBuilder<Model.VwInboxQueue>? sortDescriptor,
		bool checkPermissions,
		CancellationToken cancellationToken = default)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<Model.VwInboxQueue>();

		if (result.IsCancellationRequested(cancellationToken, scopeContext))
			return result.Build();

		if (result.IsDisposed(_disposed, scopeContext))
			return result.Build();

		var data = await QUoW.VwInboxQueueRepository
			.GetVwInboxQueueById(new Queries.VwInboxQueue.GetVwInboxQueueByIdQuery(idInboxQueue, checkPermissions, AsNoTracking: true, DisableCahce: true,
				qb => qb.Sorting(s => s.Append(sortDescriptor, throwIfEmptySortStack: true))))
			.ToResultAsync(scopeContext, cancellationToken);

		return result.WithData(data).Build();
	}

	public async Task<IResult<QueryResult<List<Model.VwInboxMessage>>>> GetAllInboxMessagesAsync(
		IScopeContext scopeContext,
		Guid idInboxQueue,
		ISortDescriptorBuilder<Model.VwInboxMessage> sortDescriptor,
		int pageIndex,
		int pageSize,
		bool checkPermissions,
		CancellationToken cancellationToken = default)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<QueryResult<List<Model.VwInboxMessage>>>();

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

		var data = await QUoW.VwInboxMessageRepository
			.GetAllVwInboxMessagesByIdQueue(
				new Queries.VwInboxMessage.GetAllVwInboxMessagesByIdQueueQuery(
					idInboxQueue,
					checkPermissions,
					AsNoTracking: true,
					DisableCahce: true,
					qb => qb
						.Sorting(s => s.Append(sortDescriptor, throwIfEmptySortStack: true))
						.Paging(pageIndex, pageSize)))
			.ToResultAsync(scopeContext, cancellationToken);

		var totalCount = await QUoW.VwInboxMessageRepository
			.GetAllVwInboxMessagesByIdQueue(
				new Queries.VwInboxMessage.GetAllVwInboxMessagesByIdQueueQuery(
					idInboxQueue,
					checkPermissions,
					AsNoTracking: true,
					DisableCahce: true,
					QueryableBuilder: null))
			.TotalCountAsync(scopeContext, cancellationToken);

		return result.WithData(new QueryResult<List<Model.VwInboxMessage>>(data, totalCount)).Build();
	}

	public async Task<IResult<QueryResult<List<Model.VwInboxMessageArchive>>>> GetAllInboxMessageArchivesAsync(
		IScopeContext scopeContext,
		Guid idInboxQueue,
		ISortDescriptorBuilder<Model.VwInboxMessageArchive> sortDescriptor,
		int pageIndex,
		int pageSize,
		bool checkPermissions,
		CancellationToken cancellationToken = default)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<QueryResult<List<Model.VwInboxMessageArchive>>>();

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

		var data = await QUoW.VwInboxMessageArchiveRepository
			.GetAllVwInboxMessageArchivesByIdQueue(
				new Queries.VwInboxMessageArchive.GetAllVwInboxMessageArchivesByIdQueueQuery(
					idInboxQueue,
					checkPermissions,
					AsNoTracking: true,
					DisableCahce: true,
					qb => qb
						.Sorting(s => s.Append(sortDescriptor, throwIfEmptySortStack: true))
						.Paging(pageIndex, pageSize)))
			.ToResultAsync(scopeContext, cancellationToken);

		var totalCount = await QUoW.VwInboxMessageArchiveRepository
			.GetAllVwInboxMessageArchivesByIdQueue(
				new Queries.VwInboxMessageArchive.GetAllVwInboxMessageArchivesByIdQueueQuery(
					idInboxQueue,
					checkPermissions,
					AsNoTracking: true,
					DisableCahce: true,
					QueryableBuilder: null))
			.TotalCountAsync(scopeContext, cancellationToken);

		return result.WithData(new QueryResult<List<Model.VwInboxMessageArchive>>(data, totalCount)).Build();
	}

	public async Task<IResult<Model.VwInboxMessage>> GetInboxMessageAsync(
		IScopeContext scopeContext,
		Guid idInboxMessage,
		bool checkPermissions,
		CancellationToken cancellationToken = default)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<Model.VwInboxMessage>();

		if (result.IsCancellationRequested(cancellationToken, scopeContext))
			return result.Build();

		if (result.IsDisposed(_disposed, scopeContext))
			return result.Build();

		var data = await QUoW.VwInboxMessageRepository
			.GetVwInboxMessageByIdMessage(new Queries.VwInboxMessage.GetVwInboxMessageByIdMessageQuery(idInboxMessage, checkPermissions, AsNoTracking: true))
			.ToResultAsync(scopeContext, cancellationToken);

		return result.WithData(data).Build();
	}

	public async Task<IResult<Model.VwInboxMessageArchive>> GetInboxMessageArchiveAsync(
		IScopeContext scopeContext,
		Guid idInboxMessage,
		bool checkPermissions,
		CancellationToken cancellationToken = default)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<Model.VwInboxMessageArchive>();

		if (result.IsCancellationRequested(cancellationToken, scopeContext))
			return result.Build();

		if (result.IsDisposed(_disposed, scopeContext))
			return result.Build();

		var data = await QUoW.VwInboxMessageArchiveRepository
			.GetVwInboxMessageArchiveByIdMessage(new Queries.VwInboxMessageArchive.GetVwInboxMessageArchiveByIdMessageQuery(idInboxMessage, checkPermissions, AsNoTracking: true))
			.ToResultAsync(scopeContext, cancellationToken);

		return result.WithData(data).Build();
	}

	public async Task<IResult<Model.VwInboxMessageContent>> GetInboxMessageContentAsync(
		IScopeContext scopeContext,
		Guid idInboxMessage,
		bool checkPermissions,
		CancellationToken cancellationToken = default)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<Model.VwInboxMessageContent>();

		if (result.IsCancellationRequested(cancellationToken, scopeContext))
			return result.Build();

		if (result.IsDisposed(_disposed, scopeContext))
			return result.Build();

		var data = await QUoW.VwInboxMessageContentRepository
			.GetVwInboxMessageContentById(new Queries.VwInboxMessageContent.GetVwInboxMessageContentByIdQuery(idInboxMessage, checkPermissions, AsNoTracking: true))
			.ToResultAsync(scopeContext, cancellationToken);

		return result.WithData(data).Build();
	}

	public async Task<IResult<List<Model.VwInboxMessageProcessingLog>>> GetInboxMessageProcessingLogsAsync(
		IScopeContext scopeContext,
		Guid idInboxMessage,
		ISortDescriptorBuilder<Model.VwInboxMessageProcessingLog>? sortDescriptor,
		bool checkPermissions,
		CancellationToken cancellationToken = default)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<List<Model.VwInboxMessageProcessingLog>>();

		if (result.IsCancellationRequested(cancellationToken, scopeContext))
			return result.Build();

		if (result.IsDisposed(_disposed, scopeContext))
			return result.Build();

		var data = await QUoW.VwInboxMessageProcessingLogRepository
			.GetVwInboxMessageProcessingLogsByIdMessage(new Queries.VwInboxMessageProcessingLog.GetVwInboxMessageProcessingLogsByIdMessageQuery(idInboxMessage, checkPermissions, AsNoTracking: true, DisableCahce: true,
				qb => qb.Sorting(s => s.Append(sortDescriptor, throwIfEmptySortStack: true))))
			.ToResultAsync(scopeContext, cancellationToken);

		return result.WithData(data).Build();
	}

	public async Task<IResult<List<Model.VwBlockedInboxMessageType>>> GetBlockedInboxMessageTypesAsync(
		IScopeContext scopeContext,
		ISortDescriptorBuilder<Model.VwBlockedInboxMessageType>? sortDescriptor,
		bool checkPermissions,
		CancellationToken cancellationToken = default)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<List<Model.VwBlockedInboxMessageType>>();

		if (result.IsCancellationRequested(cancellationToken, scopeContext))
			return result.Build();

		if (result.IsDisposed(_disposed, scopeContext))
			return result.Build();

		var data = await QUoW.VwBlockedInboxMessageTypeRepository
			.GetAllVwBlockedInboxMessageTypes(new Queries.VwBlockedInboxMessageType.GetAllVwBlockedInboxMessageTypesQuery(checkPermissions, AsNoTracking: true,
				qb => qb.Sorting(s => s.Append(sortDescriptor, throwIfEmptySortStack: true))))
			.ToResultAsync(scopeContext, cancellationToken);

		return result.WithData(data).Build();
	}

	public async Task<IResult<Guid>> CreateInboxMessageAsync(
		IScopeContext scopeContext,
		DTOs.InboxMessageDto inboxMessageDto,
		string queueName,
		bool checkMessageExists,
		bool checkPermissions,
		CancellationToken cancellationToken = default)
	{
		scopeContext = scopeContext.CreateNew()
			.AddContextProperty(nameof(inboxMessageDto.MessageTypeNamespace), inboxMessageDto?.MessageTypeNamespace)
			.AddContextProperty(nameof(queueName), queueName)
			.AddContextProperty(nameof(inboxMessageDto.MessageId), inboxMessageDto?.MessageId)
			.AddContextProperty(nameof(inboxMessageDto.BusinessId), inboxMessageDto?.BusinessId)
			.AddContextProperty(nameof(inboxMessageDto.SessionId), inboxMessageDto?.SessionId?.ToString())
			.AddContextProperty(nameof(inboxMessageDto.Publisher), inboxMessageDto?.Publisher)
			.AddContextProperty(nameof(inboxMessageDto.PublisherId), inboxMessageDto?.PublisherId);

		var result = new ResultBuilder<Guid>();

		if (result.IsCancellationRequested(cancellationToken, scopeContext))
			return result.Build();

		if (result.IsDisposed(_disposed, scopeContext))
			return result.Build();

		if (result.IsArgumentNull(scopeContext, inboxMessageDto))
			return result.Build();

		if (result.IsArgumentNullOrWhiteSpace(scopeContext, inboxMessageDto.MessageTypeNamespace))
			return result.Build();

		if (result.IsArgumentNullOrWhiteSpace(scopeContext, queueName))
			return result.Build();

		if (result.IsArgumentNullOrWhiteSpace(scopeContext, inboxMessageDto.MessageId))
			return result.Build();

		if (result.IsArgumentNull(scopeContext, inboxMessageDto.Content))
			return result.Build();

		if (checkPermissions)
		{
			var operationName = nameof(MessagingPermissions.InboxMessage.SaveInboxMessage);
			if (AccessControlManager?.IsAuthorizedFor(scopeContext, operationName, (Model.InboxMessage?)null) == false)
				return result.WithUnauthorizedException(scopeContext, null, operationName);
		}

		var idInboxMessageType = await UoW.InboxMessageTypeRepository
			.GetInboxMessageTypeByNamespace(new Queries.InboxMessageType.GetInboxMessageTypeByNamespaceQuery(inboxMessageDto.MessageTypeNamespace, checkPermissions, AsNoTracking: true))
			.GetIdInboxMessageTypeAsync(scopeContext, cancellationToken);

		if (result.IsNullOrDefault(scopeContext, idInboxMessageType, null, $"No inbox message type found by {nameof(Queries.InboxMessageType.GetInboxMessageTypeByNamespaceQuery)} for {nameof(inboxMessageDto.MessageTypeNamespace)} = {inboxMessageDto.MessageTypeNamespace}"))
			return result.Build();

		var idInboxQueue = await UoW.InboxQueueRepository
			.GetInboxQueueByName(new Queries.InboxQueue.GetInboxQueueByNameQuery(queueName, checkPermissions, AsNoTracking: true))
			.GetIdInboxQueueAsync(scopeContext, cancellationToken);

		if (result.IsNullOrDefault(scopeContext, idInboxQueue, null, $"No inbox queue found by {nameof(Queries.InboxQueue.GetInboxQueueByNameQuery)} for {nameof(queueName)} = {queueName}"))
			return result.Build();

		if (checkMessageExists)
		{
			var existingIdInboxMessage = await UoW.InboxMessageRepository
				.ExistsInboxMessageByQueueMessageId(new Queries.InboxMessage.ExistsInboxMessageByQueueMessageIdQuery(
					idInboxQueue.Value,
					inboxMessageDto.MessageId!,
					checkPermissions,
					AsNoTracking: true))
				.GetIdInboxMessageAsync(scopeContext, cancellationToken);

			if (existingIdInboxMessage.HasValue && existingIdInboxMessage != Guid.Empty)
			{
				var warningMessage = $"{nameof(Model.InboxMessage)} with {nameof(inboxMessageDto.MessageId)} = {inboxMessageDto.MessageId} and {nameof(queueName)} = {queueName} already exists.";
				scopeContext.Logger?.LogWarningMessage(scopeContext, null, x => x.InternalMessage(warningMessage));
				result.WithWarning(scopeContext, null, warningMessage);
				return result.WithData(existingIdInboxMessage.Value).Build();
			}
		}

		var createResult = Model.InboxMessage.Create(
			scopeContext,
			idInboxMessageType.Value,
			idInboxQueue.Value,
			inboxMessageDto);

		if (result.MergeHasError(createResult))
			return result.Build();

		var dbInboxMessage = createResult.Data!;

		UoW.InboxMessageRepository.Add(scopeContext, dbInboxMessage);

		var createLogResult = Model.InboxMessageProcessingLog.Create(scopeContext, dbInboxMessage);

		if (result.MergeHasError(createLogResult))
			return result.Build();

		UoW.InboxMessageProcessingLogRepository.Add(scopeContext, createLogResult.Data!);

		var saveResult = await SaveInternalAsync(scopeContext, force: false, cancellationToken);
		result.MergeHasError(saveResult);
		return result.WithData(dbInboxMessage.IdInboxMessage).Build();
	}

	public async Task<IResult<Guid>> ArchivateInboxMessageAsync(
		IScopeContext scopeContext,
		Guid idInboxMessage,
		bool checkPermissions,
		CancellationToken cancellationToken = default)
	{
		scopeContext = scopeContext.CreateNew()
			.AddContextProperty(nameof(idInboxMessage), idInboxMessage.ToString());

		var result = new ResultBuilder<Guid>();

		if (result.IsCancellationRequested(cancellationToken, scopeContext))
			return result.Build();

		if (result.IsDisposed(_disposed, scopeContext))
			return result.Build();

		if (result.IsArgumentDefault(scopeContext, idInboxMessage))
			return result.Build();

		var inboxMessage = await UoW.InboxMessageRepository
			.GetInboxMessageById(new Queries.InboxMessage.GetInboxMessageByIdQuery(idInboxMessage, false, checkPermissions, AsNoTracking: false))
			.ToResultAsync(scopeContext, cancellationToken);

		if (result.IsNull(scopeContext, inboxMessage, null, nameof(Queries.InboxMessage.GetInboxMessageByIdQuery)))
			return result.Build();

		if (checkPermissions)
		{
			var operationName = nameof(MessagingPermissions.InboxMessage.ArchivateInboxMessage);
			if (AccessControlManager?.IsAuthorizedFor(scopeContext, operationName, inboxMessage) == false)
				return result.WithUnauthorizedException(scopeContext, null, operationName);
		}

		var createResult = Model.InboxMessageArchive.Create(
			scopeContext,
			inboxMessage);

		if (result.MergeHasError(createResult))
			return result.Build();

		var dbInboxMessageArchive = createResult.Data!;

		UoW.InboxMessageArchiveRepository.Add(scopeContext, dbInboxMessageArchive);

		UoW.InboxMessageRepository.Remove(scopeContext, inboxMessage);

		var saveResult = await SaveInternalAsync(scopeContext, force: false, cancellationToken);
		result.MergeHasError(saveResult);
		return result.WithData(dbInboxMessageArchive.IdInboxMessage).Build();
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

		var dbBlockedInboxMessageTypeNamespaces = await UoW.BlockedInboxMessageTypeRepository
			.GetBlockedInboxMessageTypesByNamespaces(new Queries.BlockedInboxMessageType.GetBlockedInboxMessageTypesByNamespacesQuery(blockedNamespaces, CheckReadPermissions: false, AsNoTracking: true))
			.ToNamespacesAsync(scopeContext, cancellationToken);

		foreach (var blockedNamespace in blockedNamespaces.Where(x => !dbBlockedInboxMessageTypeNamespaces.Contains(x)))
		{
			var createResult = Model.BlockedInboxMessageType.Create(
				scopeContext,
				blockedNamespace);

			if (result.MergeHasError(createResult))
				return result.Build();

			var dbBlockedInboxMessageType = createResult.Data!;

			if (checkPermissions)
			{
				var operationName = nameof(MessagingPermissions.BlockedInboxMessageType.SaveBlockedInboxMessageType);
				if (AccessControlManager?.IsAuthorizedFor(scopeContext, operationName, dbBlockedInboxMessageType) == false)
					return result.WithUnauthorizedException(scopeContext, null, operationName);
			}

			UoW.BlockedInboxMessageTypeRepository.Add(scopeContext, dbBlockedInboxMessageType);
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

		var dbBlockedInboxMessageTypes = await UoW.BlockedInboxMessageTypeRepository
			.GetBlockedInboxMessageTypesByNamespaces(new Queries.BlockedInboxMessageType.GetBlockedInboxMessageTypesByNamespacesQuery(blockedNamespaces, checkPermissions, AsNoTracking: false))
			.ToResultAsync(scopeContext, cancellationToken);

		foreach (var dbBlockedInboxMessageType in dbBlockedInboxMessageTypes)
		{
			if (checkPermissions)
			{
				var operationName = nameof(MessagingPermissions.BlockedInboxMessageType.SaveBlockedInboxMessageType);
				if (AccessControlManager?.IsAuthorizedFor(scopeContext, operationName, dbBlockedInboxMessageType) == false)
					return result.WithUnauthorizedException(scopeContext, null, operationName);
			}

			UoW.BlockedInboxMessageTypeRepository.Remove(scopeContext, dbBlockedInboxMessageType);
		}

		var saveResult = await SaveInternalAsync(scopeContext, force: false, cancellationToken);
		result.MergeHasError(saveResult);
		return result.Build();
	}
}
