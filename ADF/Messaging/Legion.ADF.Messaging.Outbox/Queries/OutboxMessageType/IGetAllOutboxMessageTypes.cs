namespace Legion.ADF.Messaging.Outbox.Queries.OutboxMessageType;

public partial interface IGetAllOutboxMessageTypes
{
	IQueryable<Legion.ADF.Messaging.Outbox.Model.OutboxMessageType> GetQuery(
		Legion.IScopeContext scopeContext);

	Task<List<Legion.ADF.Messaging.Outbox.Model.OutboxMessageType>> ToResultAsync(
		Legion.IScopeContext scopeContext,
		CancellationToken cancellationToken = default);

	List<Legion.ADF.Messaging.Outbox.Model.OutboxMessageType> ToResult(
		Legion.IScopeContext scopeContext);
}
