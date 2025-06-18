namespace Legion.ADF.Auth.Queries.RolePermission;

public partial interface IGetRolePermissionsByRoleIdAndClaimValue
{
	IQueryable<Legion.ADF.Auth.Model.RolePermission> GetQuery(
		Legion.IScopeContext scopeContext);

	Task<List<Legion.ADF.Auth.Model.RolePermission>> ToResultAsync(
		Legion.IScopeContext scopeContext,
		CancellationToken cancellationToken = default);
}
