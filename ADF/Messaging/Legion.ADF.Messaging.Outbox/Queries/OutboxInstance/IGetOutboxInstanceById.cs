namespace Legion.ADF.Messaging.Outbox.Queries.OutboxInstance;

public partial interface IGetOutboxInstanceById
{
	IQueryable<Legion.ADF.Messaging.Outbox.Model.OutboxInstance> GetQuery(
		Legion.IScopeContext scopeContext);

	Task<Legion.ADF.Messaging.Outbox.Model.OutboxInstance?> ToResultAsync(
		Legion.IScopeContext scopeContext,
		CancellationToken cancellationToken = default);

	Legion.ADF.Messaging.Outbox.Model.OutboxInstance? ToResult(
		Legion.IScopeContext scopeContext);
}
