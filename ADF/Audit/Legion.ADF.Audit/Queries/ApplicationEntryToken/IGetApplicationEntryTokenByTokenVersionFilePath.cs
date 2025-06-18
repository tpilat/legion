namespace Legion.ADF.Audit.Queries.ApplicationEntryToken;

public partial interface IGetApplicationEntryTokenByTokenVersionFilePath
{
	IQueryable<Legion.ADF.Audit.Model.ApplicationEntryToken> GetQuery(
		Legion.IScopeContext scopeContext);

	Task<Legion.ADF.Audit.Model.ApplicationEntryToken?> ToResultAsync(
		Legion.IScopeContext scopeContext,
		CancellationToken cancellationToken = default);
}
