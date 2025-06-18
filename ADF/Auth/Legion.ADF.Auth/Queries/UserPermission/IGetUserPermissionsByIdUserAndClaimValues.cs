namespace Legion.ADF.Auth.Queries.UserPermission;

public partial interface IGetUserPermissionsByIdUserAndClaimValues
{
	IQueryable<Legion.ADF.Auth.Model.UserPermission> GetQuery(
		Legion.IScopeContext scopeContext);

	Task<List<Legion.ADF.Auth.Model.UserPermission>> ToResultAsync(
		Legion.IScopeContext scopeContext,
		CancellationToken cancellationToken = default);
}
