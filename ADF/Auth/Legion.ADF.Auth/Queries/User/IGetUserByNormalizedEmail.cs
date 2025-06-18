namespace Legion.ADF.Auth.Queries.User;

public partial interface IGetUserByNormalizedEmail
{
	IQueryable<Legion.ADF.Auth.Model.User> GetQuery(
		Legion.IScopeContext scopeContext);

	Task<Legion.ADF.Auth.Model.User?> ToResultAsync(
		Legion.IScopeContext scopeContext,
		CancellationToken cancellationToken = default);
}
