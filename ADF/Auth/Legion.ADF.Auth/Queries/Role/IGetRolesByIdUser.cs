namespace Legion.ADF.Auth.Queries.Role;

public partial interface IGetRolesByIdUser
{
	IQueryable<Legion.ADF.Auth.Model.Role> GetQuery(
		Legion.IScopeContext scopeContext);

	Task<List<string>> ToResultAsync(
		Legion.IScopeContext scopeContext,
		CancellationToken cancellationToken = default);
}
