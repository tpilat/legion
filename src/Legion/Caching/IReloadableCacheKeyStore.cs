namespace Legion.Caching;

public interface IReloadableCacheKeyStore : IDisposable, IAsyncDisposable
{
	Task<IResult> SaveReloadableCacheKeyAsync(
		IScopeContext scopeContext,
		string? key,
		List<string>? tags,
		DateTime? reloadAtUtc,
		bool checkPermissions,
		CancellationToken cancellationToken = default);

	IResult SaveReloadableCacheKey(
		IScopeContext scopeContext,
		string? key,
		List<string>? tags,
		DateTime? reloadAtUtc,
		bool checkPermissions);

	Task<IResult> SaveAsync(IScopeContext scopeContext, CancellationToken cancellationToken = default);

	IResult Save(IScopeContext scopeContext);
}
