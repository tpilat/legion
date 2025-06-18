namespace Legion.ADF.Messaging.Inbox.Queries.InboxInstance;

public partial interface IGetInboxInstanceById
{
	IQueryable<Legion.ADF.Messaging.Inbox.Model.InboxInstance> GetQuery(
		Legion.IScopeContext scopeContext);

	Task<Legion.ADF.Messaging.Inbox.Model.InboxInstance?> ToResultAsync(
		Legion.IScopeContext scopeContext,
		CancellationToken cancellationToken = default);

	Legion.ADF.Messaging.Inbox.Model.InboxInstance? ToResult(
		Legion.IScopeContext scopeContext);
}
