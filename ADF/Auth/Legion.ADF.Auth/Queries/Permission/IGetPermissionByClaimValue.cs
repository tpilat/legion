namespace Legion.ADF.Auth.Queries.Permission;

public partial interface IGetPermissionByClaimValue
{
	IQueryable<Legion.ADF.Auth.Model.Permission> GetQuery(
		Legion.IScopeContext scopeContext);

	Task<Legion.ADF.Auth.Model.Permission?> ToResultAsync(
		Legion.IScopeContext scopeContext,
		CancellationToken cancellationToken = default);
}
