namespace Legion.ADF.ServiceBus.Hosts.Queries.Host;

public partial interface IGetHostByName
{
	IQueryable<Legion.ADF.ServiceBus.Hosts.Model.Host> GetQuery(
		Legion.IScopeContext scopeContext);

	Task<Legion.ADF.ServiceBus.Hosts.Model.Host?> ToResultAsync(
		Legion.IScopeContext scopeContext,
		CancellationToken cancellationToken = default);

	Legion.ADF.ServiceBus.Hosts.Model.Host? ToResult(
		Legion.IScopeContext scopeContext);

	Task<Guid?> GetIdAsync(
		Legion.IScopeContext scopeContext,
		CancellationToken cancellationToken = default);

	Guid? GetId(
		Legion.IScopeContext scopeContext);

	Task<bool> ExistsAsync(
		Legion.IScopeContext scopeContext,
		CancellationToken cancellationToken = default);

	bool Exists(
		Legion.IScopeContext scopeContext);
}
