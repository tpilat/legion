using Legion.ADF.Cache.SqlServer;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Microsoft.EntityFrameworkCore;

namespace Legion.ADF.Cache.Queries.ReloadableCacheKey;

public class GetReloadableCacheKeyByKey :
	QueryDefinition<
		ICacheDbContext,
		Cache.Model.ReloadableCacheKey,
		Cache.Model.ReloadableCacheKey?,
		GetReloadableCacheKeyByKeyQuery>,
	IGetReloadableCacheKeyByKey
{
	public GetReloadableCacheKeyByKey(
		IEFConnectionProvider connectionProvider,
		GetReloadableCacheKeyByKeyQuery getReloadableCacheKeyByKey)
		: base(connectionProvider, getReloadableCacheKeyByKey)
	{
		Throw.IfArgumentNullOrWhiteSpace(getReloadableCacheKeyByKey?.Key);
	}

	protected override IQueryable<Cache.Model.ReloadableCacheKey> GetDefaultQuery(IScopeContext scopeContext)
	{
		var context = GetContext(scopeContext);
		return context.ReloadableCacheKey;
	}

	public override IQueryable<Cache.Model.ReloadableCacheKey> GetQuery(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return ApplyIncludesThenWhere<ICacheAccessControlManager>(
			scopeContext,
			QueryRequest.QueryableBuilder,
			QueryRequest.CheckReadPermissions,
			QueryRequest.AsNoTracking,
			x => x.Key == QueryRequest.Key);
	}

	public override async Task<Cache.Model.ReloadableCacheKey?> ToResultAsync(
		IScopeContext scopeContext,
		CancellationToken cancellationToken = default)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return await GetQuery(scopeContext)
			.FirstOrDefaultAsync(cancellationToken);
	}

	public Cache.Model.ReloadableCacheKey? ToResult(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return GetQuery(scopeContext).FirstOrDefault();
	}
}
