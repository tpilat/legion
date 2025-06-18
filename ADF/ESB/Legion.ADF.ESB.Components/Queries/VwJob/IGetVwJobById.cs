namespace Legion.ADF.ESB.Components.Queries.VwJob;

public partial interface IGetVwJobById
{
	IQueryable<Legion.ADF.ESB.Components.Model.VwJob> GetQuery(
		Legion.IScopeContext scopeContext);

	Task<Legion.ADF.ESB.Components.Model.VwJob?> ToResultAsync(
		Legion.IScopeContext scopeContext,
		CancellationToken cancellationToken = default);
}
