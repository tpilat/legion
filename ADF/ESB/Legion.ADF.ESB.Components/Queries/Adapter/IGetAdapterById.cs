namespace Legion.ADF.ESB.Components.Queries.Adapter;

public partial interface IGetAdapterById
{
	IQueryable<Legion.ADF.ESB.Components.Model.Adapter> GetQuery(
		Legion.IScopeContext scopeContext);

	Task<Legion.ADF.ESB.Components.Model.Adapter?> ToResultAsync(
		Legion.IScopeContext scopeContext,
		CancellationToken cancellationToken = default);
}
