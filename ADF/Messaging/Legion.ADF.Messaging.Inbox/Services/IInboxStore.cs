using Legion.Queries.Sorting;

namespace Legion.ADF.Messaging.Inbox.Services;

public interface  IInboxStore : IDisposable, IAsyncDisposable
{
	Task<IResult<QueryResult<List<Model.VwInboxQueueMessages>>>> GetInboxQueueMessagesAsync(
		IScopeContext scopeContext,
		bool includeInactiveQueues,
		ISortDescriptorBuilder<Model.VwInboxQueueMessages> sortDescriptor,
		int pageIndex,
		int pageSize,
		bool checkPermissions,
		CancellationToken cancellationToken = default);

	Task<IResult<Model.VwInboxQueue>> GetInboxQueueAsync(
		IScopeContext scopeContext,
		Guid idInboxQueue,
		ISortDescriptorBuilder<Model.VwInboxQueue>? sortDescriptor,
		bool checkPermissions,
		CancellationToken cancellationToken = default);

	Task<IResult<QueryResult<List<Model.VwInboxMessage>>>> GetAllInboxMessagesAsync(
		IScopeContext scopeContext,
		Guid idInboxQueue,
		ISortDescriptorBuilder<Model.VwInboxMessage> sortDescriptor,
		int pageIndex,
		int pageSize,
		bool checkPermissions,
		CancellationToken cancellationToken = default);

	Task<IResult<QueryResult<List<Model.VwInboxMessageArchive>>>> GetAllInboxMessageArchivesAsync(
		IScopeContext scopeContext,
		Guid idInboxQueue,
		ISortDescriptorBuilder<Model.VwInboxMessageArchive> sortDescriptor,
		int pageIndex,
		int pageSize,
		bool checkPermissions,
		CancellationToken cancellationToken = default);

	Task<IResult<Model.VwInboxMessage>> GetInboxMessageAsync(
		IScopeContext scopeContext,
		Guid idInboxMessage,
		bool checkPermissions,
		CancellationToken cancellationToken = default);

	Task<IResult<Model.VwInboxMessageArchive>> GetInboxMessageArchiveAsync(
		IScopeContext scopeContext,
		Guid idInboxMessage,
		bool checkPermissions,
		CancellationToken cancellationToken = default);

	Task<IResult<Model.VwInboxMessageContent>> GetInboxMessageContentAsync(
		IScopeContext scopeContext,
		Guid idInboxMessage,
		bool checkPermissions,
		CancellationToken cancellationToken = default);

	Task<IResult<List<Model.VwInboxMessageProcessingLog>>> GetInboxMessageProcessingLogsAsync(
		IScopeContext scopeContext,
		Guid idInboxMessage,
		ISortDescriptorBuilder<Model.VwInboxMessageProcessingLog>? sortDescriptor,
		bool checkPermissions,
		CancellationToken cancellationToken = default);

	Task<IResult<List<Model.VwBlockedInboxMessageType>>> GetBlockedInboxMessageTypesAsync(
		IScopeContext scopeContext,
		ISortDescriptorBuilder<Model.VwBlockedInboxMessageType>? sortDescriptor,
		bool checkPermissions,
		CancellationToken cancellationToken = default);

	Task<IResult<Guid>> CreateInboxMessageAsync(
		IScopeContext scopeContext,
		DTOs.InboxMessageDto inboxMessageDto,
		string queueName,
		bool checkMessageExists,
		bool checkPermissions,
		CancellationToken cancellationToken = default);

	Task<IResult<Guid>> ArchivateInboxMessageAsync(
		IScopeContext scopeContext,
		Guid idInboxMessage,
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
