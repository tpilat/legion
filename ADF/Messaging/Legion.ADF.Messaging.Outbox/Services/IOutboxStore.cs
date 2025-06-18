using Legion.Queries.Sorting;

namespace Legion.ADF.Messaging.Outbox.Services;

public interface  IOutboxStore : IDisposable, IAsyncDisposable
{
	Task<IResult<QueryResult<List<Model.VwOutboxQueueMessages>>>> GetOutboxQueueMessagesAsync(
		IScopeContext scopeContext,
		bool includeInactiveQueues,
		ISortDescriptorBuilder<Model.VwOutboxQueueMessages> sortDescriptor,
		int pageIndex,
		int pageSize,
		bool checkPermissions,
		CancellationToken cancellationToken = default);

	Task<IResult<Model.VwOutboxQueue>> GetOutboxQueueAsync(
		IScopeContext scopeContext,
		Guid idOutboxQueue,
		ISortDescriptorBuilder<Model.VwOutboxQueue>? sortDescriptor,
		bool checkPermissions,
		CancellationToken cancellationToken = default);

	Task<IResult<QueryResult<List<Model.VwOutboxMessage>>>> GetAllOutboxMessagesAsync(
		IScopeContext scopeContext,
		Guid idOutboxQueue,
		ISortDescriptorBuilder<Model.VwOutboxMessage> sortDescriptor,
		int pageIndex,
		int pageSize,
		bool checkPermissions,
		CancellationToken cancellationToken = default);

	Task<IResult<QueryResult<List<Model.VwOutboxMessageArchive>>>> GetAllOutboxMessageArchivesAsync(
		IScopeContext scopeContext,
		Guid idOutboxQueue,
		ISortDescriptorBuilder<Model.VwOutboxMessageArchive> sortDescriptor,
		int pageIndex,
		int pageSize,
		bool checkPermissions,
		CancellationToken cancellationToken = default);

	Task<IResult<Model.VwOutboxMessage>> GetOutboxMessageAsync(
		IScopeContext scopeContext,
		Guid idOutboxMessage,
		bool checkPermissions,
		CancellationToken cancellationToken = default);

	Task<IResult<Model.VwOutboxMessageArchive>> GetOutboxMessageArchiveAsync(
		IScopeContext scopeContext,
		Guid idOutboxMessage,
		bool checkPermissions,
		CancellationToken cancellationToken = default);

	Task<IResult<Model.VwOutboxMessageContent>> GetOutboxMessageContentAsync(
		IScopeContext scopeContext,
		Guid idOutboxMessage,
		bool checkPermissions,
		CancellationToken cancellationToken = default);

	Task<IResult<List<Model.VwOutboxMessageProcessingLog>>> GetOutboxMessageProcessingLogsAsync(
		IScopeContext scopeContext,
		Guid idOutboxMessage,
		ISortDescriptorBuilder<Model.VwOutboxMessageProcessingLog>? sortDescriptor,
		bool checkPermissions,
		CancellationToken cancellationToken = default);

	Task<IResult<List<Model.VwBlockedOutboxMessageType>>> GetBlockedOutboxMessageTypesAsync(
		IScopeContext scopeContext,
		ISortDescriptorBuilder<Model.VwBlockedOutboxMessageType>? sortDescriptor,
		bool checkPermissions,
		CancellationToken cancellationToken = default);

	Task<IResult<Guid>> CreateOutboxMessageAsync(
		IScopeContext scopeContext,
		DTOs.OutboxMessageDto outboxMessageDto,
		string queueName,
		bool checkMessageExists,
		bool checkPermissions,
		CancellationToken cancellationToken = default);

	Task<IResult<Guid>> ArchivateOutboxMessageAsync(
		IScopeContext scopeContext,
		Guid idOutboxMessage,
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
