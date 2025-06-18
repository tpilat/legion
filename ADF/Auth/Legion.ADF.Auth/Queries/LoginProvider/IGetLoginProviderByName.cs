namespace Legion.ADF.Auth.Queries.LoginProvider;

public partial interface IGetLoginProviderByName
{
	IQueryable<Legion.ADF.Auth.Model.LoginProvider> GetQuery(
		Legion.IScopeContext scopeContext);

	Task<Legion.ADF.Auth.Model.LoginProvider?> ToResultAsync(
		Legion.IScopeContext scopeContext,
		CancellationToken cancellationToken = default);
}
