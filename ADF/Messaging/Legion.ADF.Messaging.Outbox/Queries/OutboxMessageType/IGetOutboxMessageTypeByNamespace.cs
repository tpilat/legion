namespace Legion.ADF.Messaging.Outbox.Queries.OutboxMessageType;

public partial interface IGetOutboxMessageTypeByNamespace
{
	IQueryable<Legion.ADF.Messaging.Outbox.Model.OutboxMessageType> GetQuery(
		Legion.IScopeContext scopeContext);

	Task<Legion.ADF.Messaging.Outbox.Model.OutboxMessageType?> ToResultAsync(
		Legion.IScopeContext scopeContext,
		CancellationToken cancellationToken = default);

	Legion.ADF.Messaging.Outbox.Model.OutboxMessageType? ToResult(
		Legion.IScopeContext scopeContext);

	Task<Guid?> GetIdOutboxMessageTypeAsync(
		Legion.IScopeContext scopeContext,
		CancellationToken cancellationToken = default);

	Guid? GetIdOutboxMessageType(
		Legion.IScopeContext scopeContext);

	Task<bool> ExistsAsync(
		Legion.IScopeContext scopeContext,
		CancellationToken cancellationToken = default);

	bool Exists(
		Legion.IScopeContext scopeContext);
}
