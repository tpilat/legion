namespace Legion.ADF.Messaging.Inbox.Queries.VwInboxMessage;

public partial interface IGetVwInboxMessageByIdMessage
{
	IQueryable<Legion.ADF.Messaging.Inbox.Model.VwInboxMessage> GetQuery(
		Legion.IScopeContext scopeContext);

	Task<Legion.ADF.Messaging.Inbox.Model.VwInboxMessage?> ToResultAsync(
		Legion.IScopeContext scopeContext,
		CancellationToken cancellationToken = default);

	Legion.ADF.Messaging.Inbox.Model.VwInboxMessage? ToResult(
		Legion.IScopeContext scopeContext);
}
