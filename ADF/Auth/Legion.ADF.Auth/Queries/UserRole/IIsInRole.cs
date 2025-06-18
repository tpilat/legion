namespace Legion.ADF.Auth.Queries.UserRole;

public partial interface IIsInRole
{
	IQueryable<Legion.ADF.Auth.Model.UserRole> GetQuery(
		Legion.IScopeContext scopeContext);

	Task<bool> ToResultAsync(
		Legion.IScopeContext scopeContext,
		CancellationToken cancellationToken = default);
}
