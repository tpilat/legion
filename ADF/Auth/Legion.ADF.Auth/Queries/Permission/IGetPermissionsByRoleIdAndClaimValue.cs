namespace Legion.ADF.Auth.Queries.Permission;

public partial interface IGetPermissionsByRoleIdAndClaimValue
{
	IQueryable<Legion.ADF.Auth.Model.Permission> GetQuery(
		Legion.IScopeContext scopeContext);

	Task<List<Guid>> ToResultAsync(
		Legion.IScopeContext scopeContext,
		CancellationToken cancellationToken = default);
}
