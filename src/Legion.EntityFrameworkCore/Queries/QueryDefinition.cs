using Legion.ACL;
using Legion.Caching;
using Legion.Clones;
using Legion.Extensions;
using Legion.MessageBus.Messages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Linq.Expressions;

namespace Legion.EntityFrameworkCore.Queries;

public abstract class QueryDefinition<TContext, Q, TResponse, TQuery> : IQueryDefinition<TContext, Q>
	where TContext : IDbContext
	where Q : class
	where TQuery: IQueryRequest<Q, TResponse>
{
	private readonly Lazy<IAccessControlManager<Q>?> _accessControlManager;
	private readonly Lazy<IADFCache?> _cache;

	public IEFConnectionProvider ConnectionProvider { get; }
	public IServiceProvider ServiceProvider => ConnectionProvider.ServiceProvider;
	public TQuery QueryRequest { get; }
	public IAccessControlManager<Q>? AccessControlManager => _accessControlManager.Value;
	public IADFCache? Cache => _cache.Value;

	public QueryDefinition(
		IEFConnectionProvider connectionProvider,
		TQuery queryRequest)
	{
		Throw.IfArgumentNull(connectionProvider);
		Throw.IfArgumentNull(queryRequest);

		ConnectionProvider = connectionProvider;
		QueryRequest = queryRequest;
		_accessControlManager = new(() => ServiceProvider.GetService<IAccessControlManager<Q>>());
		_cache = new(() => ServiceProvider.GetService<IADFCache>());
	}

	protected virtual TContext GetContext(IScopeContext scopeContext)
		=> ConnectionProvider.GetOrCreateDbContext<TContext>(scopeContext);

	protected abstract IQueryable<Q> GetDefaultQuery(IScopeContext scopeContext);

	protected IQueryable<Q> GetAuthorizedQuery(
		IScopeContext scopeContext,
		bool checkReadPermissions,
		bool asNoTracking)
	{
		var queryable = GetDefaultQuery(scopeContext);

		if (asNoTracking)
			queryable = queryable.AsNoTracking();

		if (checkReadPermissions)
			SetAuthorizationQuery(scopeContext, queryable);

		return queryable;
	}

	protected IQueryable<Q> GetAuthorizedQuery(
		IScopeContext scopeContext,
		bool checkReadPermissions,
		bool asNoTracking,
		IGeneralAccessControlManager? accessControlManager)
	{
		var queryable = GetDefaultQuery(scopeContext);

		if (asNoTracking)
			queryable = queryable.AsNoTracking();

		if (accessControlManager != null)
			SetAuthorizationQuery(scopeContext, queryable, accessControlManager);

		return queryable;
	}

	protected IQueryable<Q> SetAuthorizationQuery(IScopeContext scopeContext, IQueryable<Q> queryable)
	{
		AccessControlManager?.SetAuthorizationQuery<TQuery, TResponse>(scopeContext, QueryRequest, queryable);

		return queryable;
	}

	protected IQueryable<Q> SetAuthorizationQuery(
		IScopeContext scopeContext,
		IQueryable<Q> queryable,
		IGeneralAccessControlManager? accessControlManager)
	{
		if (accessControlManager?.IsAuthorizedFor<Q>(scopeContext) == false)
			return Enumerable.Empty<Q>().AsAsyncQueryable();

		return queryable;
	}

	public abstract IQueryable<Q> GetQuery(IScopeContext scopeContext);

	public abstract Task<TResponse> ToResultAsync(IScopeContext scopeContext, CancellationToken cancellationToken = default);
		
	public T ToPermanentlyCachedResult<T>(
		IScopeContext scopeContext,
		Func<IQueryable<Q>, CacheKeyAndTags> cacheKey,
		Func<IQueryable<Q>, T> value,
		bool forceSet,
		bool setNullValue,
		ICloneFactory? cloneFactory = null)
	{
		if (Cache != null)
		{
			var keyAndTags = cacheKey(GetQuery(scopeContext));

			return Cache.GetOrSetValuePermanently(
				keyAndTags.Key,
				() => value(GetQuery(scopeContext)),
				keyAndTags.Tags,
				forceSet,
				setNullValue, CacheCloneOption.CloneBeforeSetAndGet,
				cloneFactory);
		}

		return value(GetQuery(scopeContext));
	}

	public T ToAbsoluteExpirationCachedResult<T>(
		IScopeContext scopeContext,
		Func<IQueryable<Q>, CacheKeyAndTags> cacheKey,
		Func<IQueryable<Q>, T> value,
		DateTime keepUntil,
		bool forceSet,
		bool setNullValue,
		ICloneFactory? cloneFactory = null,
		CacheItemPriority priority = CacheItemPriority.Normal)
	{
		if (Cache != null)
		{
			var keyAndTags = cacheKey(GetQuery(scopeContext));

			return Cache.GetOrSetValueWithAbsoluteExpiration(
				keyAndTags.Key,
				() => value(GetQuery(scopeContext)),
				keepUntil,
				keyAndTags.Tags,
				priority,
				forceSet,
				setNullValue,
				CacheCloneOption.CloneBeforeSetAndGet,
				cloneFactory);
		}

		return value(GetQuery(scopeContext));
	}

	public T ToSlidingExpirationCachedResult<T>(
		IScopeContext scopeContext,
		Func<IQueryable<Q>, CacheKeyAndTags> cacheKey,
		Func<IQueryable<Q>, T> value,
		TimeSpan slidingTime,
		bool forceSet,
		bool setNullValue,
		ICloneFactory? cloneFactory = null,
		CacheItemPriority priority = CacheItemPriority.Normal)
	{
		if (Cache != null)
		{
			var keyAndTags = cacheKey(GetQuery(scopeContext));

			return Cache.GetOrSetValueWithSlidingExpiration(
				keyAndTags.Key,
				() => value(GetQuery(scopeContext)),
				slidingTime,
				keyAndTags.Tags,
				priority,
				forceSet,
				setNullValue,
				CacheCloneOption.CloneBeforeSetAndGet,
				cloneFactory);
		}

		return value(GetQuery(scopeContext));
	}

	public async Task<T> ToPermanentlyCachedResultAsync<T>(
		IScopeContext scopeContext,
		Func<IQueryable<Q>, CacheKeyAndTags> cacheKey,
		Func<IQueryable<Q>, CancellationToken, Task<T>> value,
		bool forceSet,
		bool setNullValue,
		ICloneFactory? cloneFactory = null,
		CancellationToken cancellationToken = default)
	{
		if (Cache != null)
		{
			var keyAndTags = cacheKey(GetQuery(scopeContext));

			return await Cache.GetOrSetValuePermanentlyAsync(
				keyAndTags.Key,
				async ct => await value(GetQuery(scopeContext), ct),
				keyAndTags.Tags,
				forceSet,
				setNullValue,
				CacheCloneOption.CloneBeforeSetAndGet,
				cloneFactory,
				cancellationToken);
		}

		return await value.Invoke(GetQuery(scopeContext), cancellationToken);
	}

	public async Task<T> ToAbsoluteExpirationCachedResultAsync<T>(
		IScopeContext scopeContext,
		Func<IQueryable<Q>, CacheKeyAndTags> cacheKey,
		Func<IQueryable<Q>, CancellationToken, Task<T>> value,
		DateTime keepUntil,
		bool forceSet,
		bool setNullValue,
		ICloneFactory? cloneFactory = null,
		CacheItemPriority priority = CacheItemPriority.Normal,
		CancellationToken cancellationToken = default)
	{
		if (Cache != null)
		{
			var keyAndTags = cacheKey(GetQuery(scopeContext));

			return await Cache.GetOrSetValueWithAbsoluteExpirationAsync(
				keyAndTags.Key,
				async ct => await value(GetQuery(scopeContext), ct),
				keepUntil,
				keyAndTags.Tags,
				priority,
				forceSet,
				setNullValue,
				CacheCloneOption.CloneBeforeSetAndGet,
				cloneFactory,
				cancellationToken);
		}

		return await value.Invoke(GetQuery(scopeContext), cancellationToken);
	}

	public async Task<T> ToSlidingExpirationaCachedResultAsync<T>(
		IScopeContext scopeContext,
		Func<IQueryable<Q>, CacheKeyAndTags> cacheKey,
		Func<IQueryable<Q>, CancellationToken, Task<T>> value,
		TimeSpan slidingTime,
		bool forceSet,
		bool setNullValue,
		ICloneFactory? cloneFactory = null,
		CacheItemPriority priority = CacheItemPriority.Normal,
		CancellationToken cancellationToken = default)
	{
		if (Cache != null)
		{
			var keyAndTags = cacheKey(GetQuery(scopeContext));

			return await Cache.GetOrSetValueWithSlidingExpirationAsync(
				keyAndTags.Key,
				async ct => await value(GetQuery(scopeContext), ct),
				slidingTime,
				keyAndTags.Tags,
				priority,
				forceSet,
				setNullValue,
				CacheCloneOption.CloneBeforeSetAndGet,
				cloneFactory,
				cancellationToken);
		}

		return await value.Invoke(GetQuery(scopeContext), cancellationToken);
	}

	public IQueryable<Q> ApplyIncludesThenWhere(
		IScopeContext scopeContext,
		Action<QueryableBuilder<Q>>? queryableBuilder,
		bool checkReadPermissions,
		bool asNoTracking,
		Expression<Func<Q, bool>>? predicate)
	{
		return predicate != null
			? GetAuthorizedQuery(scopeContext, checkReadPermissions, asNoTracking)
				.ApplyIncludes(queryableBuilder)
				.Where(predicate)
			: GetAuthorizedQuery(scopeContext, checkReadPermissions, asNoTracking)
				.ApplyIncludes(queryableBuilder);
	}

	public IQueryable<Q> ApplyIncludesThenWhere(
		IScopeContext scopeContext,
		Action<QueryableBuilder<Q>>? queryableBuilder,
		bool checkReadPermissions,
		bool asNoTracking,
		IGeneralAccessControlManager? accessControlManager,
		Expression<Func<Q, bool>>? predicate)
	{
		return predicate != null
			? GetAuthorizedQuery(scopeContext, checkReadPermissions, asNoTracking, accessControlManager)
				.ApplyIncludes(queryableBuilder)
				.Where(predicate)
			: GetAuthorizedQuery(scopeContext, checkReadPermissions, asNoTracking, accessControlManager)
				.ApplyIncludes(queryableBuilder);
	}

	public IQueryable<Q> ApplyIncludesThenWhere<TGeneralAccessControlManager>(
		IScopeContext scopeContext,
		Action<QueryableBuilder<Q>>? queryableBuilder,
		bool checkReadPermissions,
		bool asNoTracking,
		Expression<Func<Q, bool>>? predicate)
		where TGeneralAccessControlManager : IGeneralAccessControlManager
	{
		var accessControlManager = ServiceProvider.GetService<TGeneralAccessControlManager>();

		return predicate != null
			? GetAuthorizedQuery(scopeContext, checkReadPermissions, asNoTracking, accessControlManager)
				.ApplyIncludes(queryableBuilder)
				.Where(predicate)
			: GetAuthorizedQuery(scopeContext, checkReadPermissions, asNoTracking, accessControlManager)
				.ApplyIncludes(queryableBuilder);
	}

	//public IQueryable<Q> ApplyIncludesThenWhere(
	//	IScopeContext scopeContext,
	//	Action<QueryableBuilder<Q>>? queryableBuilder,
	//	Expression<Func<Q, bool>>? predicate)
	//		=> ApplyIncludesThenWhere(
	//			scopeContext,
	//			queryableBuilder,
	//			false,
	//			predicate);

	public IQueryable<Q> ApplyQueryBuilder(
		IScopeContext scopeContext,
		bool checkReadPermissions,
		bool asNoTracking)
	{
		return GetAuthorizedQuery(scopeContext, checkReadPermissions, asNoTracking)
			.ApplyIncludes(QueryRequest.QueryableBuilder)
			.ApplySort(QueryRequest.QueryableBuilder)
			.ApplyPaging(QueryRequest.QueryableBuilder);
	}

	public IQueryable<Q> ApplyQueryBuilder(
		IScopeContext scopeContext,
		bool checkReadPermissions,
		bool asNoTracking,
		IGeneralAccessControlManager? accessControlManager)
	{
		return GetAuthorizedQuery(scopeContext, checkReadPermissions, asNoTracking, accessControlManager)
			.ApplyIncludes(QueryRequest.QueryableBuilder)
			.ApplySort(QueryRequest.QueryableBuilder)
			.ApplyPaging(QueryRequest.QueryableBuilder);
	}

	public IQueryable<Q> ApplyQueryBuilder<TGeneralAccessControlManager>(
		IScopeContext scopeContext,
		bool checkReadPermissions,
		bool asNoTracking)
		where TGeneralAccessControlManager : IGeneralAccessControlManager
	{
		var accessControlManager = ServiceProvider.GetService<TGeneralAccessControlManager>();

		return GetAuthorizedQuery(scopeContext, checkReadPermissions, asNoTracking, accessControlManager)
			.ApplyIncludes(QueryRequest.QueryableBuilder)
			.ApplySort(QueryRequest.QueryableBuilder)
			.ApplyPaging(QueryRequest.QueryableBuilder);
	}
}
