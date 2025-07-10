using Legion.ADF.Cache.PostgreSQL;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Microsoft.EntityFrameworkCore;

namespace Legion.ADF.Cache.Queries.CacheData;

public class GetCacheDataByKeyHash :
	QueryDefinition<
		ICacheDbContext,
		Cache.Model.CacheData,
		Cache.Model.CacheData?,
		GetCacheDataByKeyHashQuery>,
		IGetCacheDataByKeyHash
{
	public GetCacheDataByKeyHash(
		IEFConnectionProvider connectionProvider,
		GetCacheDataByKeyHashQuery getCacheDataByKeyHash)
		: base(connectionProvider, getCacheDataByKeyHash)
	{
		Throw.IfArgumentNullOrWhiteSpace(getCacheDataByKeyHash?.KeyHash);
	}

	protected override IQueryable<Cache.Model.CacheData> GetDefaultQuery(IScopeContext scopeContext)
	{
		var context = GetContext(scopeContext);
		return context.CacheData;
	}

	public override IQueryable<Cache.Model.CacheData> GetQuery(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return ApplyIncludesThenWhere<ICacheAccessControlManager>(
			scopeContext,
			QueryRequest.QueryableBuilder,
			QueryRequest.CheckReadPermissions,
			QueryRequest.AsNoTracking,
			x => x.KeyHash == QueryRequest.KeyHash);
	}

	public override async Task<Cache.Model.CacheData?> ToResultAsync(
		IScopeContext scopeContext,
		CancellationToken cancellationToken = default)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return await GetQuery(scopeContext)
			.FirstOrDefaultAsync(cancellationToken);
	}

	public Cache.Model.CacheData? ToResult(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return GetQuery(scopeContext).FirstOrDefault();
	}
}
