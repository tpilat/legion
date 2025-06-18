namespace Legion.ADF.Messaging.Outbox.Queries.VwOutboxMessageContent;

public partial interface IGetVwOutboxMessageContentById
{
	IQueryable<Legion.ADF.Messaging.Outbox.Model.VwOutboxMessageContent> GetQuery(
		Legion.IScopeContext scopeContext);

	Task<Legion.ADF.Messaging.Outbox.Model.VwOutboxMessageContent?> ToResultAsync(
		Legion.IScopeContext scopeContext,
		CancellationToken cancellationToken = default);

	Legion.ADF.Messaging.Outbox.Model.VwOutboxMessageContent? ToResult(
		Legion.IScopeContext scopeContext);
}
