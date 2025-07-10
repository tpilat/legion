namespace Legion.ADF.ServiceBus.Queries.Host;

public partial interface IGetHostById
{
	IQueryable<Legion.ADF.ServiceBus.Model.Host> GetQuery(
		Legion.IScopeContext scopeContext);

	Task<Legion.ADF.ServiceBus.Model.Host?> ToResultAsync(
		Legion.IScopeContext scopeContext,
		CancellationToken cancellationToken = default);

	Legion.ADF.ServiceBus.Model.Host? ToResult(
		Legion.IScopeContext scopeContext);

	Task<bool> ExistsAsync(
		Legion.IScopeContext scopeContext,
		CancellationToken cancellationToken = default);

	bool Exists(
		Legion.IScopeContext scopeContext);
}
