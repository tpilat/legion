using Legion.Queries.Sorting;

namespace Legion.ADF.Messaging.MessageBox.Services;

public partial class MessageBoxStore : IMessageBoxStore, IDisposable, IAsyncDisposable
{
	public async Task<IResult<QueryResult<List<Model.VwQueueMessages>>>> GetQueueMessagesAsync(
		IScopeContext scopeContext,
		bool includeInactiveQueues,
		ISortDescriptorBuilder<Model.VwQueueMessages> sortDescriptor,
		int pageIndex,
		int pageSize,
		bool checkPermissions,
		CancellationToken cancellationToken = default)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<QueryResult<List<Model.VwQueueMessages>>>();

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

		var data = await QUoW.VwQueueMessagesRepository
			.GetAllQueues(new Queries.VwQueueMessage.GetAllQueuesQuery(
					includeInactiveQueues,
					checkPermissions,
					AsNoTracking: true,
					DisableCahce: true,
					qb => qb
						.Sorting(s => s.Append(sortDescriptor, throwIfEmptySortStack: true))
						.Paging(pageIndex, pageSize)))
			.ToResultAsync(scopeContext, cancellationToken);

		var totalCount = await QUoW.VwQueueMessagesRepository
			.GetAllQueues(new Queries.VwQueueMessage.GetAllQueuesQuery(
					includeInactiveQueues,
					CheckReadPermissions: false,
					AsNoTracking: true,
					DisableCahce: true,
					QueryableBuilder: null))
			.TotalCountAsync(scopeContext, cancellationToken);

		return result.WithData(new QueryResult<List<Model.VwQueueMessages>>(data, totalCount)).Build();
	}

	public async Task<IResult<Model.VwQueue>> GetQueueAsync(
		IScopeContext scopeContext,
		Guid idQueue,
		ISortDescriptorBuilder<Model.VwQueue>? sortDescriptor,
		bool checkPermissions,
		CancellationToken cancellationToken = default)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<Model.VwQueue>();

		if (result.IsCancellationRequested(cancellationToken, scopeContext))
			return result.Build();

		if (result.IsDisposed(_disposed, scopeContext))
			return result.Build();

		var data = await QUoW.VwQueueRepository
			.GetVwQueueById(new Queries.VwQueue.GetVwQueueByIdQuery(idQueue, checkPermissions, AsNoTracking: true, DisableCahce: true,
				qb => qb.Sorting(s => s.Append(sortDescriptor, throwIfEmptySortStack: true))))
			.ToResultAsync(scopeContext, cancellationToken);

		return result.WithData(data).Build();
	}

	public async Task<IResult<QueryResult<List<Model.VwMessage>>>> GetAllQueuedMessagesAsync(
		IScopeContext scopeContext,
		Guid idQueue,
		ISortDescriptorBuilder<Model.VwMessage> sortDescriptor,
		int pageIndex,
		int pageSize,
		bool checkPermissions,
		CancellationToken cancellationToken = default)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<QueryResult<List<Model.VwMessage>>>();

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

		var data = await QUoW.VwMessageRepository
			.GetAllVwMessagesByIdQueue(
				new Queries.VwMessage.GetAllVwMessagesByIdQueueQuery(
					idQueue,
					checkPermissions,
					AsNoTracking: true,
					DisableCahce: true,
					qb => qb
						.Sorting(s => s.Append(sortDescriptor, throwIfEmptySortStack: true))
						.Paging(pageIndex, pageSize)))
			.ToResultAsync(scopeContext, cancellationToken);

		var totalCount = await QUoW.VwMessageRepository
			.GetAllVwMessagesByIdQueue(
				new Queries.VwMessage.GetAllVwMessagesByIdQueueQuery(
					idQueue,
					checkPermissions,
					AsNoTracking: true,
					DisableCahce: true,
					QueryableBuilder: null))
			.TotalCountAsync(scopeContext, cancellationToken);

		return result.WithData(new QueryResult<List<Model.VwMessage>>(data, totalCount)).Build();
	}

	public async Task<IResult<QueryResult<List<Model.VwMessageArchive>>>> GetAllQueuedMessageArchivesAsync(
		IScopeContext scopeContext,
		Guid idQueue,
		ISortDescriptorBuilder<Model.VwMessageArchive> sortDescriptor,
		int pageIndex,
		int pageSize,
		bool checkPermissions,
		CancellationToken cancellationToken = default)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<QueryResult<List<Model.VwMessageArchive>>>();

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

		var data = await QUoW.VwMessageArchiveRepository
			.GetAllVwMessageArchivesByIdQueue(
				new Queries.VwMessageArchive.GetAllVwMessageArchivesByIdQueueQuery(
					idQueue,
					checkPermissions,
					AsNoTracking: true,
					DisableCahce: true,
					qb => qb
						.Sorting(s => s.Append(sortDescriptor, throwIfEmptySortStack: true))
						.Paging(pageIndex, pageSize)))
			.ToResultAsync(scopeContext, cancellationToken);

		var totalCount = await QUoW.VwMessageArchiveRepository
			.GetAllVwMessageArchivesByIdQueue(
				new Queries.VwMessageArchive.GetAllVwMessageArchivesByIdQueueQuery(
					idQueue,
					checkPermissions,
					AsNoTracking: true,
					DisableCahce: true,
					QueryableBuilder: null))
			.TotalCountAsync(scopeContext, cancellationToken);

		return result.WithData(new QueryResult<List<Model.VwMessageArchive>>(data, totalCount)).Build();
	}

	public async Task<IResult<QueryResult<List<Model.VwTopicSubscriptionMessages>>>> GetTopicSubscriptionMessagesAsync(
		IScopeContext scopeContext,
		bool includeInactiveTopics,
		ISortDescriptorBuilder<Model.VwTopicSubscriptionMessages> sortDescriptor,
		int pageIndex,
		int pageSize,
		bool checkPermissions,
		CancellationToken cancellationToken = default)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<QueryResult<List<Model.VwTopicSubscriptionMessages>>>();

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

		var data = await QUoW.VwTopicSubscriptionMessagesRepository
			.GetAllTopicSubscriptions(new Queries.VwTopicSubscriptionMessage.GetAllTopicSubscriptionsQuery(
					includeInactiveTopics,
					checkPermissions,
					AsNoTracking: true,
					DisableCahce: true,
					qb => qb
						.Sorting(s => s.Append(sortDescriptor, throwIfEmptySortStack: true))
						.Paging(pageIndex, pageSize)))
			.ToResultAsync(scopeContext, cancellationToken);

		var totalCount = await QUoW.VwTopicSubscriptionMessagesRepository
			.GetAllTopicSubscriptions(new Queries.VwTopicSubscriptionMessage.GetAllTopicSubscriptionsQuery(
					includeInactiveTopics,
					CheckReadPermissions: false,
					AsNoTracking: true,
					DisableCahce: true,
					QueryableBuilder: null))
			.TotalCountAsync(scopeContext, cancellationToken);

		return result.WithData(new QueryResult<List<Model.VwTopicSubscriptionMessages>>(data, totalCount)).Build();
	}

	public async Task<IResult<Model.VwTopic>> GetTopicAsync(
		IScopeContext scopeContext,
		Guid idTopic,
		ISortDescriptorBuilder<Model.VwTopic>? sortDescriptor,
		bool checkPermissions,
		CancellationToken cancellationToken = default)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<Model.VwTopic>();

		if (result.IsCancellationRequested(cancellationToken, scopeContext))
			return result.Build();

		if (result.IsDisposed(_disposed, scopeContext))
			return result.Build();

		var data = await QUoW.VwTopicRepository
			.GetVwTopicById(new Queries.VwTopic.GetVwTopicByIdQuery(idTopic, checkPermissions, AsNoTracking: true, DisableCahce: true,
				qb => qb.Sorting(s => s.Append(sortDescriptor, throwIfEmptySortStack: true))))
			.ToResultAsync(scopeContext, cancellationToken);

		return result.WithData(data).Build();
	}

	public async Task<IResult<Model.VwTopicSubscription>> GetTopicSubscriptionAsync(
		IScopeContext scopeContext,
		Guid idTopicSubscription,
		ISortDescriptorBuilder<Model.VwTopicSubscription>? sortDescriptor,
		bool checkPermissions,
		CancellationToken cancellationToken = default)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<Model.VwTopicSubscription>();

		if (result.IsCancellationRequested(cancellationToken, scopeContext))
			return result.Build();

		if (result.IsDisposed(_disposed, scopeContext))
			return result.Build();

		var data = await QUoW.VwTopicSubscriptionRepository
			.GetVwTopicSubscriptionById(new Queries.VwTopicSubscription.GetVwTopicSubscriptionByIdQuery(idTopicSubscription, checkPermissions, AsNoTracking: true, DisableCahce: true,
				qb => qb.Sorting(s => s.Append(sortDescriptor, throwIfEmptySortStack: true))))
			.ToResultAsync(scopeContext, cancellationToken);

		return result.WithData(data).Build();
	}

	public async Task<IResult<QueryResult<List<Model.VwMessage>>>> GetAllSubscribedMessagesAsync(
		IScopeContext scopeContext,
		Guid idTopicSubscription,
		ISortDescriptorBuilder<Model.VwMessage> sortDescriptor,
		int pageIndex,
		int pageSize,
		bool checkPermissions,
		CancellationToken cancellationToken = default)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<QueryResult<List<Model.VwMessage>>>();

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

		var data = await QUoW.VwMessageRepository
			.GetAllVwMessagesByIdQueue(
				new Queries.VwMessage.GetAllVwMessagesByIdQueueQuery(
					idTopicSubscription,
					checkPermissions,
					AsNoTracking: true,
					DisableCahce: true,
					qb => qb
						.Sorting(s => s.Append(sortDescriptor, throwIfEmptySortStack: true))
						.Paging(pageIndex, pageSize)))
			.ToResultAsync(scopeContext, cancellationToken);

		var totalCount = await QUoW.VwMessageRepository
			.GetAllVwMessagesByIdQueue(
				new Queries.VwMessage.GetAllVwMessagesByIdQueueQuery(
					idTopicSubscription,
					checkPermissions,
					AsNoTracking: true,
					DisableCahce: true,
					QueryableBuilder: null))
			.TotalCountAsync(scopeContext, cancellationToken);

		return result.WithData(new QueryResult<List<Model.VwMessage>>(data, totalCount)).Build();
	}

	public async Task<IResult<QueryResult<List<Model.VwMessageArchive>>>> GetAllSubscribedMessageArchivesAsync(
		IScopeContext scopeContext,
		Guid idTopicSubscription,
		ISortDescriptorBuilder<Model.VwMessageArchive> sortDescriptor,
		int pageIndex,
		int pageSize,
		bool checkPermissions,
		CancellationToken cancellationToken = default)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<QueryResult<List<Model.VwMessageArchive>>>();

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

		var data = await QUoW.VwMessageArchiveRepository
			.GetAllVwMessageArchivesByIdQueue(
				new Queries.VwMessageArchive.GetAllVwMessageArchivesByIdQueueQuery(
					idTopicSubscription,
					checkPermissions,
					AsNoTracking: true,
					DisableCahce: true,
					qb => qb
						.Sorting(s => s.Append(sortDescriptor, throwIfEmptySortStack: true))
						.Paging(pageIndex, pageSize)))
			.ToResultAsync(scopeContext, cancellationToken);

		var totalCount = await QUoW.VwMessageArchiveRepository
			.GetAllVwMessageArchivesByIdQueue(
				new Queries.VwMessageArchive.GetAllVwMessageArchivesByIdQueueQuery(
					idTopicSubscription,
					checkPermissions,
					AsNoTracking: true,
					DisableCahce: true,
					QueryableBuilder: null))
			.TotalCountAsync(scopeContext, cancellationToken);

		return result.WithData(new QueryResult<List<Model.VwMessageArchive>>(data, totalCount)).Build();
	}

	public async Task<IResult<Model.VwMessage>> GetMessageAsync(
		IScopeContext scopeContext,
		Guid idMessage,
		bool checkPermissions,
		CancellationToken cancellationToken = default)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<Model.VwMessage>();

		if (result.IsCancellationRequested(cancellationToken, scopeContext))
			return result.Build();

		if (result.IsDisposed(_disposed, scopeContext))
			return result.Build();

		var data = await QUoW.VwMessageRepository
			.GetVwMessageByIdMessage(new Queries.VwMessage.GetVwMessageByIdMessageQuery(idMessage, checkPermissions, AsNoTracking: true))
			.ToResultAsync(scopeContext, cancellationToken);

		return result.WithData(data).Build();
	}

	public async Task<IResult<Model.VwMessageArchive>> GetMessageArchiveAsync(
		IScopeContext scopeContext,
		Guid idMessage,
		bool checkPermissions,
		CancellationToken cancellationToken = default)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<Model.VwMessageArchive>();

		if (result.IsCancellationRequested(cancellationToken, scopeContext))
			return result.Build();

		if (result.IsDisposed(_disposed, scopeContext))
			return result.Build();

		var data = await QUoW.VwMessageArchiveRepository
			.GetVwMessageArchiveByIdMessage(new Queries.VwMessageArchive.GetVwMessageArchiveByIdMessageQuery(idMessage, checkPermissions, AsNoTracking: true))
			.ToResultAsync(scopeContext, cancellationToken);

		return result.WithData(data).Build();
	}

	public async Task<IResult<Model.VwMessageContent>> GetMessageContentAsync(
		IScopeContext scopeContext,
		Guid idMessage,
		bool checkPermissions,
		CancellationToken cancellationToken = default)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<Model.VwMessageContent>();

		if (result.IsCancellationRequested(cancellationToken, scopeContext))
			return result.Build();

		if (result.IsDisposed(_disposed, scopeContext))
			return result.Build();

		var data = await QUoW.VwMessageContentRepository
			.GetVwMessageContentById(new Queries.VwMessageContent.GetVwMessageContentByIdQuery(idMessage, checkPermissions, AsNoTracking: true))
			.ToResultAsync(scopeContext, cancellationToken);

		return result.WithData(data).Build();
	}

	public async Task<IResult<List<Model.VwMessageProcessingLog>>> GetMessageProcessingLogsAsync(
		IScopeContext scopeContext,
		Guid idMessage,
		ISortDescriptorBuilder<Model.VwMessageProcessingLog>? sortDescriptor,
		bool checkPermissions,
		CancellationToken cancellationToken = default)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<List<Model.VwMessageProcessingLog>>();

		if (result.IsCancellationRequested(cancellationToken, scopeContext))
			return result.Build();

		if (result.IsDisposed(_disposed, scopeContext))
			return result.Build();

		var data = await QUoW.VwMessageProcessingLogRepository
			.GetVwMessageProcessingLogsByIdMessage(new Queries.VwMessageProcessingLog.GetVwMessageProcessingLogsByIdMessageQuery(idMessage, checkPermissions, AsNoTracking: true, DisableCahce: true,
				qb => qb.Sorting(s => s.Append(sortDescriptor, throwIfEmptySortStack: true))))
			.ToResultAsync(scopeContext, cancellationToken);

		return result.WithData(data).Build();
	}

	public async Task<IResult<List<Model.VwBlockedMessageType>>> GetBlockedMessageTypesAsync(
		IScopeContext scopeContext,
		ISortDescriptorBuilder<Model.VwBlockedMessageType>? sortDescriptor,
		bool checkPermissions,
		CancellationToken cancellationToken = default)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<List<Model.VwBlockedMessageType>>();

		if (result.IsCancellationRequested(cancellationToken, scopeContext))
			return result.Build();

		if (result.IsDisposed(_disposed, scopeContext))
			return result.Build();

		var data = await QUoW.VwBlockedMessageTypeRepository
			.GetAllVwBlockedMessageTypes(new Queries.VwBlockedMessageType.GetAllVwBlockedMessageTypesQuery(checkPermissions, AsNoTracking: true,
				qb => qb.Sorting(s => s.Append(sortDescriptor, throwIfEmptySortStack: true))))
			.ToResultAsync(scopeContext, cancellationToken);

		return result.WithData(data).Build();
	}

	public async Task<IResult<Guid>> CreateMessageAsync(
		IScopeContext scopeContext,
		DTOs.MessageBoxMessageDto messageDto,
		string? queueName,
		string? topicName,
		bool checkMessageExists,
		bool checkPermissions,
		CancellationToken cancellationToken = default)
	{
		scopeContext = scopeContext.CreateNew()
			.AddContextProperty(nameof(messageDto.MessageTypeNamespace), messageDto?.MessageTypeNamespace)
			.AddContextProperty(nameof(queueName), queueName)
			.AddContextProperty(nameof(topicName), topicName)
			.AddContextProperty(nameof(messageDto.MessageId), messageDto?.MessageId)
			.AddContextProperty(nameof(messageDto.BusinessId), messageDto?.BusinessId)
			.AddContextProperty(nameof(messageDto.SessionId), messageDto?.SessionId?.ToString())
			.AddContextProperty(nameof(messageDto.Publisher), messageDto?.Publisher)
			.AddContextProperty(nameof(messageDto.PublisherId), messageDto?.PublisherId);

		var result = new ResultBuilder<Guid>();

		if (result.IsCancellationRequested(cancellationToken, scopeContext))
			return result.Build();

		if (result.IsDisposed(_disposed, scopeContext))
			return result.Build();

		if (result.IsArgumentNull(scopeContext, messageDto))
			return result.Build();

		if (result.IsArgumentNullOrWhiteSpace(scopeContext, messageDto.MessageTypeNamespace))
			return result.Build();

		if (string.IsNullOrWhiteSpace(queueName) && string.IsNullOrWhiteSpace(topicName) && result.IsArgumentNullOrWhiteSpace(scopeContext, queueName, errorCode: null, detail: null, paramName: $"{nameof(queueName)} && {nameof(topicName)}"))
			return result.Build();

		if (!string.IsNullOrWhiteSpace(queueName) && !string.IsNullOrWhiteSpace(topicName))
			return result.WithArgumentException(scopeContext, queueName, errorCode: null, detail: "Both are set", paramName: $"{nameof(queueName)} != null && {nameof(topicName)} != null");

		if (result.IsArgumentNullOrWhiteSpace(scopeContext, messageDto.MessageId))
			return result.Build();

		if (result.IsArgumentNull(scopeContext, messageDto.Content))
			return result.Build();

		if (checkPermissions)
		{
			var operationName = nameof(MessagingPermissions.MessageBoxMessage.SaveMessage);
			if (AccessControlManager?.IsAuthorizedFor(scopeContext, operationName, (Model.Message?)null) == false)
				return result.WithUnauthorizedException(scopeContext, null, operationName);
		}

		var idMessageType = await UoW.MessageTypeRepository
			.GetMessageTypeByNamespace(new Queries.MessageType.GetMessageTypeByNamespaceQuery(messageDto.MessageTypeNamespace, checkPermissions, AsNoTracking: true))
			.GetIdMessageTypeAsync(scopeContext, cancellationToken);

		if (result.IsNullOrDefault(scopeContext, idMessageType, null, $"No messageBox message type found by {nameof(Queries.MessageType.GetMessageTypeByNamespaceQuery)} for {nameof(messageDto.MessageTypeNamespace)} = {messageDto.MessageTypeNamespace}"))
			return result.Build();

		Guid? idQueue = null;
		Guid? idTopic = null;
		List<Guid>? idTopicSubscriptions = null;

		if (!string.IsNullOrWhiteSpace(queueName))
		{
			idQueue = await UoW.QueueRepository
				.GetQueueByName(new Queries.Queue.GetQueueByNameQuery(queueName, checkPermissions, AsNoTracking: true))
				.GetIdQueueAsync(scopeContext, cancellationToken);

			if (result.IsNullOrDefault(scopeContext, idQueue, null, $"No messageBox queue found by {nameof(Queries.Queue.GetQueueByNameQuery)} for {nameof(queueName)} = {queueName}"))
				return result.Build();

			if (checkMessageExists)
			{
				var existingIdMessage = await UoW.MessageRepository
					.ExistsMessageByQueueMessageId(new Queries.Message.ExistsMessageByQueueMessageIdQuery(
						idQueue.Value,
						messageDto.MessageId!,
						checkPermissions,
						AsNoTracking: true))
					.GetIdMessageAsync(scopeContext, cancellationToken);

				if (existingIdMessage.HasValue && existingIdMessage != Guid.Empty)
				{
					var warningMessage = $"{nameof(Model.Message)} with {nameof(messageDto.MessageId)} = {messageDto.MessageId} and {nameof(queueName)} = {queueName} already exists.";
					scopeContext.Logger?.LogWarningMessage(scopeContext, null, x => x.InternalMessage(warningMessage));
					result.WithWarning(scopeContext, null, warningMessage);
					return result.WithData(existingIdMessage.Value).Build();
				}
			}
		}
		else //TopicSubscription
		{
			idTopic = await UoW.TopicRepository
				.GetTopicByName(new Queries.Topic.GetTopicByNameQuery(topicName!, checkPermissions, AsNoTracking: true))
				.GetIdTopicAsync(scopeContext, cancellationToken);

			//TODO: fix co je default Guid? pre idTopc null alebo Guid.Empty ?
			if (result.IsNullOrDefault(scopeContext, idTopic, null, $"No messageBox topic found by {nameof(Queries.Topic.GetTopicByNameQuery)} for {nameof(topicName)} = {topicName}"))
				return result.Build();

			if (checkMessageExists)
			{
				var existingIdMessage = await UoW.MessageRepository
					.ExistsMessageByTopicMessageId(new Queries.Message.ExistsMessageByTopicMessageIdQuery(
						idTopic.Value,
						messageDto.MessageId!,
						checkPermissions,
						AsNoTracking: true))
					.GetIdMessageAsync(scopeContext, cancellationToken);

				if (existingIdMessage.HasValue && existingIdMessage != Guid.Empty)
				{
					var warningMessage = $"{nameof(Model.Message)} with {nameof(messageDto.MessageId)} = {messageDto.MessageId} and {nameof(topicName)} = {topicName} already exists.";
					scopeContext.Logger?.LogWarningMessage(scopeContext, null, x => x.InternalMessage(warningMessage));
					result.WithWarning(scopeContext, null, warningMessage);
					return result.WithData(existingIdMessage.Value).Build();
				}
			}

			idTopicSubscriptions = await UoW.TopicSubscriptionRepository
				.GetAllTopicSubscriptionsByTopic(new Queries.TopicSubscription.GetAllTopicSubscriptionsByTopicQuery(idTopic.Value, checkPermissions, AsNoTracking: true))
				.GetIdTopicSubscriptionsAsync(scopeContext, cancellationToken);

			if (result.IsNullOrEmpty(scopeContext, idTopicSubscriptions, null, $"No messageBox topic subscription found by {nameof(Queries.TopicSubscription.GetTopicSubscriptionByTopicAndNameQuery)} for {nameof(idTopic)} = {idTopic}"))
				return result.Build();
		}

		Model.Message? dbMessage = null;

		if (idQueue.HasValue)
		{
			var createMessageResult = Model.Message.Create(
				scopeContext,
				idMessageType.Value,
				idQueue,
				null,
				messageDto);

			if (result.MergeHasError(createMessageResult))
				return result.Build();

			dbMessage = createMessageResult.Data!;

			UoW.MessageRepository.Add(scopeContext, dbMessage);

			var createQueuedMessageResult = Model.QueuedMessage.Create(
				scopeContext,
				dbMessage.IdMessage,
				idQueue.Value,
				messageDto);

			if (result.MergeHasError(createQueuedMessageResult))
				return result.Build();

			var dbQueuedMessage = createQueuedMessageResult.Data!;

			UoW.QueuedMessageRepository.Add(scopeContext, dbQueuedMessage);

			var createLogResult = Model.MessageProcessingLog.Create(scopeContext, dbQueuedMessage);

			if (result.MergeHasError(createLogResult))
				return result.Build();

			UoW.MessageProcessingLogRepository.Add(scopeContext, createLogResult.Data!);
		}
		else //idTopicSubscription
		{
			var createMessageResult = Model.Message.Create(
				scopeContext,
				idMessageType.Value,
				null,
				idTopic,
				messageDto);

			if (result.MergeHasError(createMessageResult))
				return result.Build();

			dbMessage = createMessageResult.Data!;

			UoW.MessageRepository.Add(scopeContext, dbMessage);

			foreach (var idTopicSubscription in idTopicSubscriptions!)
			{
				var createSubscribedMessageResult = Model.SubscribedMessage.Create(
					scopeContext,
					dbMessage.IdMessage,
					idTopicSubscription,
					messageDto);

				if (result.MergeHasError(createSubscribedMessageResult))
					return result.Build();

				var dbSubscribedMessage = createSubscribedMessageResult.Data!;

				UoW.SubscribedMessageRepository.Add(scopeContext, dbSubscribedMessage);

				var createLogResult = Model.MessageProcessingLog.Create(scopeContext, dbSubscribedMessage);

				if (result.MergeHasError(createLogResult))
					return result.Build();

				UoW.MessageProcessingLogRepository.Add(scopeContext, createLogResult.Data!);
			}
		}

		var saveResult = await SaveInternalAsync(scopeContext, force: false, cancellationToken);
		result.MergeHasError(saveResult);
		return result.WithData(dbMessage.IdMessage).Build();
	}

	public async Task<IResult<Guid>> CreateMessageAsync(
		IScopeContext scopeContext,
		DTOs.MessageBoxMessageDto messageDto,
		string? queueName,
		string? topicName,
		string? subscriptionName,
		bool checkMessageExists,
		bool checkPermissions,
		CancellationToken cancellationToken = default)
	{
		scopeContext = scopeContext.CreateNew()
			.AddContextProperty(nameof(messageDto.MessageTypeNamespace), messageDto?.MessageTypeNamespace)
			.AddContextProperty(nameof(queueName), queueName)
			.AddContextProperty(nameof(topicName), topicName)
			.AddContextProperty(nameof(subscriptionName), subscriptionName)
			.AddContextProperty(nameof(messageDto.MessageId), messageDto?.MessageId)
			.AddContextProperty(nameof(messageDto.BusinessId), messageDto?.BusinessId)
			.AddContextProperty(nameof(messageDto.SessionId), messageDto?.SessionId?.ToString())
			.AddContextProperty(nameof(messageDto.Publisher), messageDto?.Publisher)
			.AddContextProperty(nameof(messageDto.PublisherId), messageDto?.PublisherId);

		var result = new ResultBuilder<Guid>();

		if (result.IsCancellationRequested(cancellationToken, scopeContext))
			return result.Build();

		if (result.IsDisposed(_disposed, scopeContext))
			return result.Build();

		if (result.IsArgumentNull(scopeContext, messageDto))
			return result.Build();

		if (result.IsArgumentNullOrWhiteSpace(scopeContext, messageDto.MessageTypeNamespace))
			return result.Build();

		if (string.IsNullOrWhiteSpace(queueName) && string.IsNullOrWhiteSpace(topicName) && result.IsArgumentNullOrWhiteSpace(scopeContext, queueName, errorCode: null, detail: null, paramName: $"{nameof(queueName)} && {nameof(topicName)}"))
			return result.Build();

		if (!string.IsNullOrWhiteSpace(queueName) && !string.IsNullOrWhiteSpace(topicName))
			return result.WithArgumentException(scopeContext, queueName, errorCode: null, detail: "Both are set", paramName: $"{nameof(queueName)} != null && {nameof(topicName)} != null");

		if (!string.IsNullOrWhiteSpace(topicName) && result.IsArgumentNullOrWhiteSpace(scopeContext, subscriptionName))
			return result.Build();

		if (result.IsArgumentNullOrWhiteSpace(scopeContext, messageDto.MessageId))
			return result.Build();

		if (result.IsArgumentNull(scopeContext, messageDto.Content))
			return result.Build();

		if (checkPermissions)
		{
			var operationName = nameof(MessagingPermissions.MessageBoxMessage.SaveMessage);
			if (AccessControlManager?.IsAuthorizedFor(scopeContext, operationName, (Model.Message?)null) == false)
				return result.WithUnauthorizedException(scopeContext, null, operationName);
		}

		var idMessageType = await UoW.MessageTypeRepository
			.GetMessageTypeByNamespace(new Queries.MessageType.GetMessageTypeByNamespaceQuery(messageDto.MessageTypeNamespace, checkPermissions, AsNoTracking: true))
			.GetIdMessageTypeAsync(scopeContext, cancellationToken);

		if (result.IsNullOrDefault(scopeContext, idMessageType, null, $"No messageBox message type found by {nameof(Queries.MessageType.GetMessageTypeByNamespaceQuery)} for {nameof(messageDto.MessageTypeNamespace)} = {messageDto.MessageTypeNamespace}"))
			return result.Build();

		Guid? idQueue = null;
		Guid? idTopic = null;
		Guid? idTopicSubscription = null;

		if (!string.IsNullOrWhiteSpace(queueName))
		{
			idQueue = await UoW.QueueRepository
				.GetQueueByName(new Queries.Queue.GetQueueByNameQuery(queueName, checkPermissions, AsNoTracking: true))
				.GetIdQueueAsync(scopeContext, cancellationToken);

			if (result.IsNullOrDefault(scopeContext, idQueue, null, $"No messageBox queue found by {nameof(Queries.Queue.GetQueueByNameQuery)} for {nameof(queueName)} = {queueName}"))
				return result.Build();

			if (checkMessageExists)
			{
				var existingIdMessage = await UoW.MessageRepository
					.ExistsMessageByQueueMessageId(new Queries.Message.ExistsMessageByQueueMessageIdQuery(
						idQueue.Value,
						messageDto.MessageId!,
						checkPermissions,
						AsNoTracking: true))
					.GetIdMessageAsync(scopeContext, cancellationToken);

				if (existingIdMessage.HasValue && existingIdMessage != Guid.Empty)
				{
					var warningMessage = $"{nameof(Model.Message)} with {nameof(messageDto.MessageId)} = {messageDto.MessageId} and {nameof(queueName)} = {queueName} already exists.";
					scopeContext.Logger?.LogWarningMessage(scopeContext, null, x => x.InternalMessage(warningMessage));
					result.WithWarning(scopeContext, null, warningMessage);
					return result.WithData(existingIdMessage.Value).Build();
				}
			}
		}
		else //TopicSubscription
		{
			idTopic = await UoW.TopicRepository
				.GetTopicByName(new Queries.Topic.GetTopicByNameQuery(topicName!, checkPermissions, AsNoTracking: true))
				.GetIdTopicAsync(scopeContext, cancellationToken);

			//TODO: fix co je default Guid? pre idTopc null alebo Guid.Empty ?
			if (result.IsNullOrDefault(scopeContext, idTopic, null, $"No messageBox topic found by {nameof(Queries.Topic.GetTopicByNameQuery)} for {nameof(topicName)} = {topicName}"))
				return result.Build();

			if (checkMessageExists)
			{
				var existingIdMessage = await UoW.MessageRepository
					.ExistsMessageByTopicMessageId(new Queries.Message.ExistsMessageByTopicMessageIdQuery(
						idTopic.Value,
						messageDto.MessageId!,
						checkPermissions,
						AsNoTracking: true))
					.GetIdMessageAsync(scopeContext, cancellationToken);

				if (existingIdMessage.HasValue && existingIdMessage != Guid.Empty)
				{
					var warningMessage = $"{nameof(Model.Message)} with {nameof(messageDto.MessageId)} = {messageDto.MessageId} and {nameof(topicName)} = {topicName} already exists.";
					scopeContext.Logger?.LogWarningMessage(scopeContext, null, x => x.InternalMessage(warningMessage));
					result.WithWarning(scopeContext, null, warningMessage);
					return result.WithData(existingIdMessage.Value).Build();
				}
			}

			idTopicSubscription = await UoW.TopicSubscriptionRepository
				.GetTopicSubscriptionByTopicAndName(new Queries.TopicSubscription.GetTopicSubscriptionByTopicAndNameQuery(idTopic.Value, subscriptionName!, checkPermissions, AsNoTracking: true))
				.GetIdTopicSubscriptionAsync(scopeContext, cancellationToken);

			//TODO: fix co je default Guid? pre idTopicSubscription null alebo Guid.Empty ?
			if (result.IsNullOrDefault(scopeContext, idTopicSubscription, null, $"No messageBox topic subscription found by {nameof(Queries.TopicSubscription.GetTopicSubscriptionByTopicAndNameQuery)} for {nameof(idTopic)} = {idTopic} | {nameof(subscriptionName)} = {subscriptionName}"))
				return result.Build();
		}

		Model.Message? dbMessage = null;

		if (idQueue.HasValue)
		{
			var createMessageResult = Model.Message.Create(
				scopeContext,
				idMessageType.Value,
				idQueue,
				null,
				messageDto);

			if (result.MergeHasError(createMessageResult))
				return result.Build();

			dbMessage = createMessageResult.Data!;

			UoW.MessageRepository.Add(scopeContext, dbMessage);

			var createQueuedMessageResult = Model.QueuedMessage.Create(
				scopeContext,
				dbMessage.IdMessage,
				idQueue.Value,
				messageDto);

			if (result.MergeHasError(createQueuedMessageResult))
				return result.Build();

			var dbQueuedMessage = createQueuedMessageResult.Data!;

			UoW.QueuedMessageRepository.Add(scopeContext, dbQueuedMessage);

			var createLogResult = Model.MessageProcessingLog.Create(scopeContext, dbQueuedMessage);

			if (result.MergeHasError(createLogResult))
				return result.Build();

			UoW.MessageProcessingLogRepository.Add(scopeContext, createLogResult.Data!);
		}
		else //idTopicSubscription
		{
			var createMessageResult = Model.Message.Create(
				scopeContext,
				idMessageType.Value,
				null,
				idTopic,
				messageDto);

			if (result.MergeHasError(createMessageResult))
				return result.Build();

			dbMessage = createMessageResult.Data!;

			UoW.MessageRepository.Add(scopeContext, dbMessage);

			var createSubscribedMessageResult = Model.SubscribedMessage.Create(
				scopeContext,
				dbMessage.IdMessage,
				idTopicSubscription!.Value,
				messageDto);

			if (result.MergeHasError(createSubscribedMessageResult))
				return result.Build();

			var dbSubscribedMessage = createSubscribedMessageResult.Data!;

			UoW.SubscribedMessageRepository.Add(scopeContext, dbSubscribedMessage);

			var createLogResult = Model.MessageProcessingLog.Create(scopeContext, dbSubscribedMessage);

			if (result.MergeHasError(createLogResult))
				return result.Build();

			UoW.MessageProcessingLogRepository.Add(scopeContext, createLogResult.Data!);
		}

		var saveResult = await SaveInternalAsync(scopeContext, force: false, cancellationToken);
		result.MergeHasError(saveResult);
		return result.WithData(dbMessage.IdMessage).Build();
	}

	public async Task<IResult<Guid>> ArchivateMessageAsync(
		IScopeContext scopeContext,
		Guid idMessage,
		bool checkPermissions,
		CancellationToken cancellationToken = default)
	{
		scopeContext = scopeContext.CreateNew()
			.AddContextProperty(nameof(idMessage), idMessage.ToString());

		var result = new ResultBuilder<Guid>();

		if (result.IsCancellationRequested(cancellationToken, scopeContext))
			return result.Build();

		if (result.IsDisposed(_disposed, scopeContext))
			return result.Build();

		if (result.IsArgumentDefault(scopeContext, idMessage))
			return result.Build();

		var message = await UoW.MessageRepository
			.GetMessageById(new Queries.Message.GetMessageByIdQuery(idMessage, false, checkPermissions, AsNoTracking: false))
			.ToResultAsync(scopeContext, cancellationToken);

		if (result.IsNull(scopeContext, message, null, nameof(Queries.Message.GetMessageByIdQuery)))
			return result.Build();

		if (checkPermissions)
		{
			var operationName = nameof(MessagingPermissions.MessageBoxMessage.ArchivateMessage);
			if (AccessControlManager?.IsAuthorizedFor(scopeContext, operationName, message) == false)
				return result.WithUnauthorizedException(scopeContext, null, operationName);
		}

		var createResult = Model.MessageArchive.Create(
			scopeContext,
			message);

		if (result.MergeHasError(createResult))
			return result.Build();

		var dbMessageArchive = createResult.Data!;

		UoW.MessageArchiveRepository.Add(scopeContext, dbMessageArchive);

		UoW.MessageRepository.Remove(scopeContext, message);

		var saveResult = await SaveInternalAsync(scopeContext, force: false, cancellationToken);
		result.MergeHasError(saveResult);
		return result.WithData(dbMessageArchive.IdMessage).Build();
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

		var dbBlockedMessageTypeNamespaces = await UoW.BlockedMessageTypeRepository
			.GetBlockedMessageTypesByNamespaces(new Queries.BlockedMessageType.GetBlockedMessageTypesByNamespacesQuery(blockedNamespaces, CheckReadPermissions: false, AsNoTracking: true))
			.ToNamespacesAsync(scopeContext, cancellationToken);

		foreach (var blockedNamespace in blockedNamespaces.Where(x => !dbBlockedMessageTypeNamespaces.Contains(x)))
		{
			var createResult = Model.BlockedMessageType.Create(
				scopeContext,
				blockedNamespace);

			if (result.MergeHasError(createResult))
				return result.Build();

			var dbBlockedMessageType = createResult.Data!;

			if (checkPermissions)
			{
				var operationName = nameof(MessagingPermissions.BlockedMessageBoxMessageType.SaveBlockedMessageType);
				if (AccessControlManager?.IsAuthorizedFor(scopeContext, operationName, dbBlockedMessageType) == false)
					return result.WithUnauthorizedException(scopeContext, null, operationName);
			}

			UoW.BlockedMessageTypeRepository.Add(scopeContext, dbBlockedMessageType);
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

		var dbBlockedMessageTypes = await UoW.BlockedMessageTypeRepository
			.GetBlockedMessageTypesByNamespaces(new Queries.BlockedMessageType.GetBlockedMessageTypesByNamespacesQuery(blockedNamespaces, checkPermissions, AsNoTracking: false))
			.ToResultAsync(scopeContext, cancellationToken);

		foreach (var dbBlockedMessageType in dbBlockedMessageTypes)
		{
			if (checkPermissions)
			{
				var operationName = nameof(MessagingPermissions.BlockedMessageBoxMessageType.SaveBlockedMessageType);
				if (AccessControlManager?.IsAuthorizedFor(scopeContext, operationName, dbBlockedMessageType) == false)
					return result.WithUnauthorizedException(scopeContext, null, operationName);
			}

			UoW.BlockedMessageTypeRepository.Remove(scopeContext, dbBlockedMessageType);
		}

		var saveResult = await SaveInternalAsync(scopeContext, force: false, cancellationToken);
		result.MergeHasError(saveResult);
		return result.Build();
	}
}
