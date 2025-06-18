namespace Legion.ADF.Auth.Queries.User;

public partial interface IGetUserByNormalizedRoleName
{
	IQueryable<Legion.ADF.Auth.Model.User> GetQuery(
		Legion.IScopeContext scopeContext);

	Task<List<Legion.ADF.Auth.Model.User>> ToResultAsync(
		Legion.IScopeContext scopeContext,
		CancellationToken cancellationToken = default);
}
