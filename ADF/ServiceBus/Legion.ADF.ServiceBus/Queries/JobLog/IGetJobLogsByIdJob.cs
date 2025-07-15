namespace Legion.ADF.ServiceBus.Queries.JobLog;

public partial interface IGetJobLogsByIdJob
{
	IQueryable<Legion.ADF.ServiceBus.Model.JobLog> GetQuery(
		Legion.IScopeContext scopeContext);

	Task<List<Legion.ADF.ServiceBus.Model.JobLog>> ToResultAsync(
		Legion.IScopeContext scopeContext,
		CancellationToken cancellationToken = default);

	List<Legion.ADF.ServiceBus.Model.JobLog> ToResult(
		Legion.IScopeContext scopeContext);
}
