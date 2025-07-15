namespace Legion.ADF.ServiceBus.Queries.JobExecution;

public partial interface IGetJobExecutionById
{
	IQueryable<Legion.ADF.ServiceBus.Model.JobExecution> GetQuery(
		Legion.IScopeContext scopeContext);

	Task<List<Legion.ADF.ServiceBus.Model.JobExecution>> ToResultAsync(
		Legion.IScopeContext scopeContext,
		CancellationToken cancellationToken = default);

	List<Legion.ADF.ServiceBus.Model.JobExecution> ToResult(
		Legion.IScopeContext scopeContext);
}
