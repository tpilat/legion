namespace Legion.ADF.ESB.Components.Queries.Adapter;

public partial interface IGetAllAdapters
{
	IQueryable<Legion.ADF.ESB.Components.Model.Adapter> GetQuery(
		Legion.IScopeContext scopeContext);

	Task<List<Legion.ADF.ESB.Components.Model.Adapter>> ToResultAsync(
		Legion.IScopeContext scopeContext,
		CancellationToken cancellationToken = default);
}
