namespace Legion.ADF.Auth.Queries.ExternalLogin;

public partial interface IGetExternalLoginsByUserId
{
	IQueryable<Legion.ADF.Auth.Model.ExternalLogin> GetQuery(
		Legion.IScopeContext scopeContext);

	Task<List<Legion.ADF.Auth.Model.ExternalLogin>> ToResultAsync(
		Legion.IScopeContext scopeContext,
		CancellationToken cancellationToken = default);
}
