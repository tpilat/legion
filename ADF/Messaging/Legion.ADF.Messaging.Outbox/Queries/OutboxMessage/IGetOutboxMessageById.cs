namespace Legion.ADF.Messaging.Outbox.Queries.OutboxMessage;

public partial interface IGetOutboxMessageById
{
	IQueryable<Legion.ADF.Messaging.Outbox.Model.OutboxMessage> GetQuery(
		Legion.IScopeContext scopeContext);

	Task<Legion.ADF.Messaging.Outbox.Model.OutboxMessage?> ToResultAsync(
		Legion.IScopeContext scopeContext,
		CancellationToken cancellationToken = default);

	Legion.ADF.Messaging.Outbox.Model.OutboxMessage? ToResult(
		Legion.IScopeContext scopeContext);
}
