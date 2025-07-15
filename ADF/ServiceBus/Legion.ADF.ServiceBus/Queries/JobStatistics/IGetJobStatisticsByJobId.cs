namespace Legion.ADF.ServiceBus.Queries.JobStatistics;

public partial interface IGetJobStatisticsByJobId
{
	IQueryable<Legion.ADF.ServiceBus.Model.JobStatistics> GetQuery(
		Legion.IScopeContext scopeContext);

	Task<List<Legion.ADF.ServiceBus.Model.JobStatistics>> ToResultAsync(
		Legion.IScopeContext scopeContext,
		CancellationToken cancellationToken = default);

	List<Legion.ADF.ServiceBus.Model.JobStatistics> ToResult(
		Legion.IScopeContext scopeContext);
}
