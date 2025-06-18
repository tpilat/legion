namespace Legion.ADF.Auth.Queries.Role;

public partial interface IGetRoleByNormalizedName
{
	IQueryable<Legion.ADF.Auth.Model.Role> GetQuery(
		Legion.IScopeContext scopeContext);

	Task<Legion.ADF.Auth.Model.Role?> ToResultAsync(
		Legion.IScopeContext scopeContext,
		CancellationToken cancellationToken = default);
}
