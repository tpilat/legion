namespace Legion.ADF.ServiceBus.Queries.Host;

public partial interface IGetAllHosts
{
	IQueryable<Legion.ADF.ServiceBus.Model.Host> GetQuery(
		Legion.IScopeContext scopeContext);

	Task<List<Legion.ADF.ServiceBus.Model.Host>> ToResultAsync(
		Legion.IScopeContext scopeContext,
		CancellationToken cancellationToken = default);

	List<Legion.ADF.ServiceBus.Model.Host> ToResult(
		Legion.IScopeContext scopeContext);
}
