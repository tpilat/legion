namespace Legion.ADF.Auth.Queries.Permission;

public partial interface IGetPermissionsByClaimValues
{
	IQueryable<Legion.ADF.Auth.Model.Permission> GetQuery(
		Legion.IScopeContext scopeContext);

	Task<List<Legion.ADF.Auth.Model.Permission>> ToResultAsync(
		Legion.IScopeContext scopeContext,
		CancellationToken cancellationToken = default);
}
