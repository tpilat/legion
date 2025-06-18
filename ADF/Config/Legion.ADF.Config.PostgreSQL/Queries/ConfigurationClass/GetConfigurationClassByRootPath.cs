using Legion.ADF.Config.PostgreSQL;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Microsoft.EntityFrameworkCore;

namespace Legion.ADF.Config.Queries.ConfigurationClass;

public class GetConfigurationClassByRootPath :
	QueryDefinition<
		IConfigDbContext,
		Config.Model.ConfigurationClass,
		Config.Model.ConfigurationClass?,
		GetConfigurationClassByRootPathQuery>,
	IGetConfigurationClassByRootPath
{
	public GetConfigurationClassByRootPath(
		IEFConnectionProvider connectionProvider,
		GetConfigurationClassByRootPathQuery getConfigurationClassByRootPathQuery)
		: base(connectionProvider, getConfigurationClassByRootPathQuery)
	{
	}

	protected override IQueryable<Config.Model.ConfigurationClass> GetDefaultQuery(IScopeContext scopeContext)
	{
		var context = GetContext(scopeContext);
		return context.ConfigurationClass;
	}

	public override IQueryable<Config.Model.ConfigurationClass> GetQuery(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return ApplyIncludesThenWhere<IConfigAccessControlManager>(
			scopeContext,
			QueryRequest.QueryableBuilder,
			QueryRequest.CheckReadPermissions,
			QueryRequest.AsNoTracking,
			x => x.RootPath == QueryRequest.RootPath);
	}

	private string? _queryRequestType;
	private string? _entityType;
	private Caching.CacheKeyAndTags GetCacheKey(IQueryable<Config.Model.ConfigurationClass> queryable)
	{
		//var expression = queryable.ToExpressionString();
		//var sql = queryable.ToQueryString();

		var queryRequestType = _queryRequestType ??= QueryRequest.GetType().FullName;
		var entityType = _entityType ??= typeof(Config.Model.ConfigurationClass).FullName;
		return new Caching.CacheKeyAndTags($"#{entityType}#|{queryRequestType}|{QueryRequest.RootPath}|{QueryRequest.CheckReadPermissions}", [entityType]);
	}

	public override async Task<Config.Model.ConfigurationClass?> ToResultAsync(
		IScopeContext scopeContext,
		CancellationToken cancellationToken = default)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return QueryRequest.DisableCahce
			? await GetQuery(scopeContext).FirstOrDefaultAsync(cancellationToken)
			: await ToPermanentlyCachedResultAsync(
				scopeContext,
				GetCacheKey,
				async (queryable, ct) => await queryable.FirstOrDefaultAsync(ct),
				forceSet: false,
				setNullValue: false,
				cloneFactory: null,
				cancellationToken);
	}

	public Config.Model.ConfigurationClass? ToResult(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return QueryRequest.DisableCahce
			? GetQuery(scopeContext).FirstOrDefault()
			: ToPermanentlyCachedResult(
				scopeContext,
				GetCacheKey,
				queryable => queryable.FirstOrDefault(),
				forceSet: false,
				setNullValue: false,
				cloneFactory: null);
	}
}
