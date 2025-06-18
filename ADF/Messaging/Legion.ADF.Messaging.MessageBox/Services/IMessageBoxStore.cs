using Legion.Queries.Sorting;

namespace Legion.ADF.Messaging.MessageBox.Services;

public interface IMessageBoxStore : IDisposable, IAsyncDisposable
{
	Task<IResult<QueryResult<List<Model.VwQueueMessages>>>> GetQueueMessagesAsync(
		IScopeContext scopeContext,
		bool includeInactiveQueues,
		ISortDescriptorBuilder<Model.VwQueueMessages> sortDescriptor,
		int pageIndex,
		int pageSize,
		bool checkPermissions,
		CancellationToken cancellationToken = default);

	Task<IResult<Model.VwQueue>> GetQueueAsync(
		IScopeContext scopeContext,
		Guid idQueue,
		ISortDescriptorBuilder<Model.VwQueue>? sortDescriptor,
		bool checkPermissions,
		CancellationToken cancellationToken = default);

	Task<IResult<QueryResult<List<Model.VwMessage>>>> GetAllQueuedMessagesAsync(
		IScopeContext scopeContext,
		Guid idQueue,
		ISortDescriptorBuilder<Model.VwMessage> sortDescriptor,
		int pageIndex,
		int pageSize,
		bool checkPermissions,
		CancellationToken cancellationToken = default);

	Task<IResult<QueryResult<List<Model.VwMessageArchive>>>> GetAllQueuedMessageArchivesAsync(
		IScopeContext scopeContext,
		Guid idQueue,
		ISortDescriptorBuilder<Model.VwMessageArchive> sortDescriptor,
		int pageIndex,
		int pageSize,
		bool checkPermissions,
		CancellationToken cancellationToken = default);

	Task<IResult<QueryResult<List<Model.VwTopicSubscriptionMessages>>>> GetTopicSubscriptionMessagesAsync(
		IScopeContext scopeContext,
		bool includeInactiveTopics,
		ISortDescriptorBuilder<Model.VwTopicSubscriptionMessages> sortDescriptor,
		int pageIndex,
		int pageSize,
		bool checkPermissions,
		CancellationToken cancellationToken = default);

	Task<IResult<Model.VwTopic>> GetTopicAsync(
		IScopeContext scopeContext,
		Guid idTopic,
		ISortDescriptorBuilder<Model.VwTopic>? sortDescriptor,
		bool checkPermissions,
		CancellationToken cancellationToken = default);

	Task<IResult<Model.VwTopicSubscription>> GetTopicSubscriptionAsync(
		IScopeContext scopeContext,
		Guid idTopicSubscription,
		ISortDescriptorBuilder<Model.VwTopicSubscription>? sortDescriptor,
		bool checkPermissions,
		CancellationToken cancellationToken = default);

	Task<IResult<QueryResult<List<Model.VwMessage>>>> GetAllSubscribedMessagesAsync(
		IScopeContext scopeContext,
		Guid idTopicSubscription,
		ISortDescriptorBuilder<Model.VwMessage> sortDescriptor,
		int pageIndex,
		int pageSize,
		bool checkPermissions,
		CancellationToken cancellationToken = default);

	Task<IResult<QueryResult<List<Model.VwMessageArchive>>>> GetAllSubscribedMessageArchivesAsync(
		IScopeContext scopeContext,
		Guid idTopicSubscription,
		ISortDescriptorBuilder<Model.VwMessageArchive> sortDescriptor,
		int pageIndex,
		int pageSize,
		bool checkPermissions,
		CancellationToken cancellationToken = default);

	Task<IResult<Model.VwMessage>> GetMessageAsync(
		IScopeContext scopeContext,
		Guid idMessage,
		bool checkPermissions,
		CancellationToken cancellationToken = default);

	Task<IResult<Model.VwMessageArchive>> GetMessageArchiveAsync(
		IScopeContext scopeContext,
		Guid idMessage,
		bool checkPermissions,
		CancellationToken cancellationToken = default);

	Task<IResult<Model.VwMessageContent>> GetMessageContentAsync(
		IScopeContext scopeContext,
		Guid idMessage,
		bool checkPermissions,
		CancellationToken cancellationToken = default);

	Task<IResult<List<Model.VwMessageProcessingLog>>> GetMessageProcessingLogsAsync(
		IScopeContext scopeContext,
		Guid idMessage,
		ISortDescriptorBuilder<Model.VwMessageProcessingLog>? sortDescriptor,
		bool checkPermissions,
		CancellationToken cancellationToken = default);

	Task<IResult<List<Model.VwBlockedMessageType>>> GetBlockedMessageTypesAsync(
		IScopeContext scopeContext,
		ISortDescriptorBuilder<Model.VwBlockedMessageType>? sortDescriptor,
		bool checkPermissions,
		CancellationToken cancellationToken = default);

	Task<IResult<Guid>> CreateMessageAsync(
		IScopeContext scopeContext,
		DTOs.MessageBoxMessageDto messageDto,
		string? queueName,
		string? topicName,
		bool checkMessageExists,
		bool checkPermissions,
		CancellationToken cancellationToken = default);

	Task<IResult<Guid>> CreateMessageAsync(
		IScopeContext scopeContext,
		DTOs.MessageBoxMessageDto messageDto,
		string? queueName,
		string? topicName,
		string? subscriptionName,
		bool checkMessageExists,
		bool checkPermissions,
		CancellationToken cancellationToken = default);

	Task<IResult<Guid>> ArchivateMessageAsync(
		IScopeContext scopeContext,
		Guid idMessage,
		bool checkPermissions,
		CancellationToken cancellationToken = default);

	Task<IResult> AddBlockedMessageTypesAsync(
		IScopeContext scopeContext,
		List<string> blockedNamespaces,
		bool checkPermissions,
		CancellationToken cancellationToken = default);

	Task<IResult> RemoveBlockedMessageTypesAsync(
		IScopeContext scopeContext,
		List<string> blockedNamespaces,
		bool checkPermissions,
		CancellationToken cancellationToken = default);
}
