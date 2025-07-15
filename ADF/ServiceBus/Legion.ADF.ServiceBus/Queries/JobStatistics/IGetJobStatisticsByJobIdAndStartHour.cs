namespace Legion.ADF.ServiceBus.Queries.JobStatistics;

public partial interface IGetJobStatisticsByJobIdAndStartHour
{
	IQueryable<Legion.ADF.ServiceBus.Model.JobStatistics> GetQuery(
		Legion.IScopeContext scopeContext);

	Task<Legion.ADF.ServiceBus.Model.JobStatistics?> ToResultAsync(
		Legion.IScopeContext scopeContext,
		CancellationToken cancellationToken = default);

	Legion.ADF.ServiceBus.Model.JobStatistics? ToResult(
		Legion.IScopeContext scopeContext);
}
