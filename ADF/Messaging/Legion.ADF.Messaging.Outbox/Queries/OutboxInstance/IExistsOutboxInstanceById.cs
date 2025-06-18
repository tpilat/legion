namespace Legion.ADF.Messaging.Outbox.Queries.OutboxInstance;

public partial interface IExistsOutboxInstanceById
{
	IQueryable<Legion.ADF.Messaging.Outbox.Model.OutboxInstance> GetQuery(
		Legion.IScopeContext scopeContext);

	Task<bool> ToResultAsync(
		Legion.IScopeContext scopeContext,
		CancellationToken cancellationToken = default);

	bool ToResult(
		Legion.IScopeContext scopeContext);
}
