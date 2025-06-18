namespace Legion.ADF.Messaging.Inbox.Queries.InboxMessageType;

public partial interface IGetInboxMessageTypeByNamespace
{
	IQueryable<Legion.ADF.Messaging.Inbox.Model.InboxMessageType> GetQuery(
		Legion.IScopeContext scopeContext);

	Task<Legion.ADF.Messaging.Inbox.Model.InboxMessageType?> ToResultAsync(
		Legion.IScopeContext scopeContext,
		CancellationToken cancellationToken = default);

	Legion.ADF.Messaging.Inbox.Model.InboxMessageType? ToResult(
		Legion.IScopeContext scopeContext);

	Task<Guid?> GetIdInboxMessageTypeAsync(
		Legion.IScopeContext scopeContext,
		CancellationToken cancellationToken = default);

	Guid? GetIdInboxMessageType(
		Legion.IScopeContext scopeContext);

	Task<bool> ExistsAsync(
		Legion.IScopeContext scopeContext,
		CancellationToken cancellationToken = default);

	bool Exists(
		Legion.IScopeContext scopeContext);
}
