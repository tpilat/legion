namespace Legion.ADF.ServiceBus.Queries.Job;

public partial interface IGetJobById
{
	IQueryable<Legion.ADF.ServiceBus.Model.Job> GetQuery(
		Legion.IScopeContext scopeContext);

	Task<Legion.ADF.ServiceBus.Model.Job?> ToResultAsync(
		Legion.IScopeContext scopeContext,
		CancellationToken cancellationToken = default);

	Legion.ADF.ServiceBus.Model.Job? ToResult(
		Legion.IScopeContext scopeContext);
}
