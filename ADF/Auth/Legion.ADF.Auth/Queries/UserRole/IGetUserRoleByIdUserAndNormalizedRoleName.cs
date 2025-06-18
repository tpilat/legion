namespace Legion.ADF.Auth.Queries.UserRole;

public partial interface IGetUserRoleByIdUserAndNormalizedRoleName
{
	IQueryable<Legion.ADF.Auth.Model.UserRole> GetQuery(
		Legion.IScopeContext scopeContext);

	Task<Legion.ADF.Auth.Model.UserRole?> ToResultAsync(
		Legion.IScopeContext scopeContext,
		CancellationToken cancellationToken = default);
}
