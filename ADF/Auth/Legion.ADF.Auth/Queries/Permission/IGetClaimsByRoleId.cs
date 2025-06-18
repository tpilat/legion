namespace Legion.ADF.Auth.Queries.Permission;

public partial interface IGetClaimsByRoleId
{
	IQueryable<Legion.ADF.Auth.Model.Permission> GetQuery(
		Legion.IScopeContext scopeContext);

	Task<List<System.Security.Claims.Claim>> ToResultAsync(
		Legion.IScopeContext scopeContext,
		CancellationToken cancellationToken = default);
}
