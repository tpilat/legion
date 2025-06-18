namespace Legion.ADF.Auth.Queries.ExternalLogin;

public partial interface IGetExternalLoginByExternalIdentifier
{
	IQueryable<Legion.ADF.Auth.Model.ExternalLogin> GetQuery(
		Legion.IScopeContext scopeContext);

	Task<Legion.ADF.Auth.Model.ExternalLogin?> ToResultAsync(
		Legion.IScopeContext scopeContext,
		CancellationToken cancellationToken = default);
}
