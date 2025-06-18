namespace Legion.ADF.Messaging.Outbox.Queries.VwOutboxMessage;

public partial interface IGetVwOutboxMessageByIdMessage
{
	IQueryable<Legion.ADF.Messaging.Outbox.Model.VwOutboxMessage> GetQuery(
		Legion.IScopeContext scopeContext);

	Task<Legion.ADF.Messaging.Outbox.Model.VwOutboxMessage?> ToResultAsync(
		Legion.IScopeContext scopeContext,
		CancellationToken cancellationToken = default);

	Legion.ADF.Messaging.Outbox.Model.VwOutboxMessage? ToResult(
		Legion.IScopeContext scopeContext);
}
