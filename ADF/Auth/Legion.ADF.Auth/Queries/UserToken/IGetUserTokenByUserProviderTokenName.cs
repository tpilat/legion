namespace Legion.ADF.Auth.Queries.UserToken;

public partial interface IGetUserTokenByUserProviderTokenName
{
	IQueryable<Legion.ADF.Auth.Model.UserToken> GetQuery(
		Legion.IScopeContext scopeContext);

	Task<Legion.ADF.Auth.Model.UserToken?> ToResultAsync(
		Legion.IScopeContext scopeContext,
		CancellationToken cancellationToken = default);
}
