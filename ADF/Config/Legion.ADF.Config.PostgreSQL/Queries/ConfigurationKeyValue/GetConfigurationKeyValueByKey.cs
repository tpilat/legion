using Legion.ADF.Config.PostgreSQL;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Microsoft.EntityFrameworkCore;

namespace Legion.ADF.Config.Queries.ConfigurationKeyValue;

public class GetConfigurationKeyValueByKey :
	QueryDefinition<
		IConfigDbContext,
		Config.Model.ConfigurationKeyValue,
		Config.Model.ConfigurationKeyValue?,
		GetConfigurationKeyValueByKeyQuery>,
	IGetConfigurationKeyValueByKey
{
	public GetConfigurationKeyValueByKey(
		IEFConnectionProvider connectionProvider,
		GetConfigurationKeyValueByKeyQuery getConfigurationKeyValueByKey)
		: base(connectionProvider, getConfigurationKeyValueByKey)
	{
		Throw.IfArgumentNullOrWhiteSpace(getConfigurationKeyValueByKey?.Key);
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
			x => x.Key == QueryRequest.Key);
	}

	private string? _queryRequestType;
	private string? _entityType;
	private Caching.CacheKeyAndTags GetCacheKey(IQueryable<Config.Model.ConfigurationKeyValue> queryable)
	{
		//var expression = queryable.ToExpressionString();
		//var sql = queryable.ToQueryString();

		var queryRequestType = _queryRequestType ??= QueryRequest.GetType().FullName;
		var entityType = _entityType ??= typeof(Config.Model.ConfigurationKeyValue).FullName;
		return new Caching.CacheKeyAndTags($"#{entityType}#|{queryRequestType}|{QueryRequest.Key}|{QueryRequest.CheckReadPermissions}", [entityType]);
	}

	public override async Task<Config.Model.ConfigurationKeyValue?> ToResultAsync(
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

	public Config.Model.ConfigurationKeyValue? ToResult(IScopeContext scopeContext)
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
