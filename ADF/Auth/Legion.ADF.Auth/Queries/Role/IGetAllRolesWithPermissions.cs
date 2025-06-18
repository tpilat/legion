namespace Legion.ADF.Auth.Queries.Role;

public partial interface IGetAllRolesWithPermissions
{
	IQueryable<Legion.ADF.Auth.Model.Role> GetQuery(
		Legion.IScopeContext scopeContext);

	Task<List<Legion.ADF.Auth.Model.Role>> ToResultAsync(
		Legion.IScopeContext scopeContext,
		CancellationToken cancellationToken = default);
}
