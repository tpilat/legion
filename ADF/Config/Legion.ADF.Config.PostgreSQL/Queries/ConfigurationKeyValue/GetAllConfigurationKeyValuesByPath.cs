using Legion.ADF.Config.PostgreSQL;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Microsoft.EntityFrameworkCore;

namespace Legion.ADF.Config.Queries.ConfigurationKeyValue;

public class GetAllConfigurationKeyValuesByPath :
	QueryDefinition<
		IConfigDbContext,
		Config.Model.ConfigurationKeyValue,
		List<Config.Model.ConfigurationKeyValue>,
		GetAllConfigurationKeyValuesByPathQuery>,
	IGetAllConfigurationKeyValuesByPath
{
	public GetAllConfigurationKeyValuesByPath(
		IEFConnectionProvider connectionProvider,
		GetAllConfigurationKeyValuesByPathQuery getAllConfigurationKeyValuesByPath)
		: base(connectionProvider, getAllConfigurationKeyValuesByPath)
	{
		Throw.IfArgumentNullOrWhiteSpace(getAllConfigurationKeyValuesByPath?.Path);
	}

	protected override IQueryable<Config.Model.ConfigurationKeyValue> GetDefaultQuery(IScopeContext scopeContext)
	{
		var context = GetContext(scopeContext);
		return context.ConfigurationKeyValue;
	}

	public override IQueryable<Config.Model.ConfigurationKeyValue> GetQuery(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return ApplyIncludesThenWhere<IConfigAccessControlManager>(
			scopeContext,
			QueryRequest.QueryableBuilder,
			QueryRequest.CheckReadPermissions,
			QueryRequest.AsNoTracking,
			x => x.Key.StartsWith(QueryRequest.Path));
	}

	private string? _queryRequestType;
	private string? _entityType;
	private Caching.CacheKeyAndTags GetCacheKey(IQueryable<Config.Model.ConfigurationKeyValue> queryable)
	{
		//var expression = queryable.ToExpressionString();
		//var sql = queryable.ToQueryString();

		var queryRequestType = _queryRequestType ??= QueryRequest.GetType().FullName;
		var entityType = _entityType ??= typeof(Config.Model.ConfigurationKeyValue).FullName;
		return new Caching.CacheKeyAndTags($"#{entityType}#|{queryRequestType}|{QueryRequest.Path}|{QueryRequest.CheckReadPermissions}", [entityType]);
	}

	public override async Task<List<Config.Model.ConfigurationKeyValue>> ToResultAsync(
		IScopeContext scopeContext,
		CancellationToken cancellationToken = default)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return QueryRequest.DisableCahce
			? await GetQuery(scopeContext).ToListAsync(cancellationToken)
			: await ToPermanentlyCachedResultAsync(
				scopeContext,
				GetCacheKey,
				async (queryable, ct) => await queryable.ToListAsync(ct),
				forceSet: false,
				setNullValue: false,
				cloneFactory: null,
				cancellationToken);
	}

	public List<Config.Model.ConfigurationKeyValue> ToResult(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return QueryRequest.DisableCahce
			? GetQuery(scopeContext).ToList()
			: ToPermanentlyCachedResult(
				scopeContext,
				GetCacheKey,
				queryable => queryable.ToList(),
				forceSet: false,
				setNullValue: false,
				cloneFactory: null);
	}
}
