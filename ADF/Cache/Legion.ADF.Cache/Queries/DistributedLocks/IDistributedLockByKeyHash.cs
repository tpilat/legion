namespace Legion.ADF.Cache.Queries.DistributedLock;

public partial interface IGetDistributedLockByKeyHash
{
	IQueryable<Legion.ADF.Cache.Model.DistributedLock> GetQuery(
		Legion.IScopeContext scopeContext);

	Task<Legion.ADF.Cache.Model.DistributedLock?> ToResultAsync(
		Legion.IScopeContext scopeContext,
		CancellationToken cancellationToken = default);

	Legion.ADF.Cache.Model.DistributedLock? ToResult(
		Legion.IScopeContext scopeContext);

	Task<bool> ExistsAsync(
		Legion.IScopeContext scopeContext,
		CancellationToken cancellationToken = default);

	bool Exists(
		Legion.IScopeContext scopeContext);

	Task<string?> GetMetadataAsync(
		Legion.IScopeContext scopeContext,
		CancellationToken cancellationToken = default);

	string? GetMetadata(
		Legion.IScopeContext scopeContext);
}
