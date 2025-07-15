namespace Legion.ADF.ServiceBus.Queries.HostLog;

public partial interface IGetHostLogsByIdHost
{
	IQueryable<Legion.ADF.ServiceBus.Model.HostLog> GetQuery(
		Legion.IScopeContext scopeContext);

	Task<List<Legion.ADF.ServiceBus.Model.HostLog>> ToResultAsync(
		Legion.IScopeContext scopeContext,
		CancellationToken cancellationToken = default);

	List<Legion.ADF.ServiceBus.Model.HostLog> ToResult(
		Legion.IScopeContext scopeContext);
}
