namespace Legion.ADF.ServiceBus.Queries.Job;

public partial interface IGetAllJobs
{
	IQueryable<Legion.ADF.ServiceBus.Model.Job> GetQuery(
		Legion.IScopeContext scopeContext);

	Task<List<Legion.ADF.ServiceBus.Model.Job>> ToResultAsync(
		Legion.IScopeContext scopeContext,
		CancellationToken cancellationToken = default);

	List<Legion.ADF.ServiceBus.Model.Job> ToResult(
		Legion.IScopeContext scopeContext);
}
