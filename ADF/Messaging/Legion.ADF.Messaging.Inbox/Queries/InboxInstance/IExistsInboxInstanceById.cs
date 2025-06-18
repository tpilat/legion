namespace Legion.ADF.Messaging.Inbox.Queries.InboxInstance;

public partial interface IExistsInboxInstanceById
{
	IQueryable<Legion.ADF.Messaging.Inbox.Model.InboxInstance> GetQuery(
		Legion.IScopeContext scopeContext);

	Task<bool> ToResultAsync(
		Legion.IScopeContext scopeContext,
		CancellationToken cancellationToken = default);

	bool ToResult(
		Legion.IScopeContext scopeContext);
}
