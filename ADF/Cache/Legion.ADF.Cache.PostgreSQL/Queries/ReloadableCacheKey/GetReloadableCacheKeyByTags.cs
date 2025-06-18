using Legion.ADF.Cache.PostgreSQL;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Microsoft.EntityFrameworkCore;

namespace Legion.ADF.Cache.Queries.ReloadableCacheKey;

public class GetReloadableCacheKeyByTags :
	QueryDefinition<
		ICacheDbContext,
		Cache.Model.ReloadableCacheKey,
		Cache.Model.ReloadableCacheKey?,
		GetReloadableCacheKeyByTagsQuery>,
	IGetReloadableCacheKeyByTags
{
	public GetReloadableCacheKeyByTags(
		IEFConnectionProvider connectionProvider,
		GetReloadableCacheKeyByTagsQuery getReloadableCacheKeyByTags)
		: base(connectionProvider, getReloadableCacheKeyByTags)
	{
		Throw.IfArgumentNullOrEmpty(getReloadableCacheKeyByTags?.Tags);
	}

	protected override IQueryable<Cache.Model.ReloadableCacheKey> GetDefaultQuery(IScopeContext scopeContext)
	{
		var context = GetContext(scopeContext);
		return context.ReloadableCacheKey;
	}

	public override IQueryable<Cache.Model.ReloadableCacheKey> GetQuery(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		var tags = QueryRequest.Tags.OrderBy(x => x).ToList();

		return ApplyIncludesThenWhere<ICacheAccessControlManager>(
			scopeContext,
			QueryRequest.QueryableBuilder,
			QueryRequest.CheckReadPermissions,
			QueryRequest.AsNoTracking,
			x => x.Tags == tags);
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
