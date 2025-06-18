using Legion.ADF.Cache.PostgreSQL;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Microsoft.EntityFrameworkCore;

namespace Legion.ADF.Cache.Queries.ReloadableCacheKey;

public class GetAllReloadableCacheKeys :
	QueryDefinition<
		ICacheDbContext,
		Cache.Model.ReloadableCacheKey,
		List<Cache.Model.ReloadableCacheKey>,
		GetAllReloadableCacheKeysQuery>,
	IGetAllReloadableCacheKeys
{
	public GetAllReloadableCacheKeys(
		IEFConnectionProvider connectionProvider,
		GetAllReloadableCacheKeysQuery getAllReloadableCacheKeys)
		: base(connectionProvider, getAllReloadableCacheKeys)
	{
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
			null);
	}

	public override async Task<List<Cache.Model.ReloadableCacheKey>> ToResultAsync(
		IScopeContext scopeContext,
		CancellationToken cancellationToken = default)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return await GetQuery(scopeContext)
			.ToListAsync(cancellationToken);
	}

	public Cache.Model.ReloadableCacheKey? ToResult(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return GetQuery(scopeContext).FirstOrDefault();
	}
}
