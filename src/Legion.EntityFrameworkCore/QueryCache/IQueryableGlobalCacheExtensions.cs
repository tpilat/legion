using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Caching.Memory;
using Legion.EntityFrameworkCore.QueryCache;
using Legion.Extensions;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace Legion.EntityFrameworkCore;

public static partial class IQueryableGlobalCacheExtensions
{
#pragma warning disable EF1001 // Internal EF Core API usage.
	private readonly static ConcurrentDictionary<Type, Func<object, object?>?> _relationalQueryContextCache = new();

	public static RelationalQueryContext? GetRelationalQueryContext<TSource>(this IQueryable<TSource> query)
		where TSource : class
	{
		//neoptimalizovane:
		//var enumerable = query.Provider.Execute<IEnumerable<TSource>>(query.Expression);
		//var relationalQueryContext = enumerable.Private<RelationalQueryContext>("_relationalQueryContext");
		//return relationalQueryContext;

		var enumerable = query.Provider.Execute<IEnumerable<TSource>>(query.Expression);
		RelationalQueryContext? relationalQueryContext = null;

		if (enumerable is IQueryingEnumerable queryingEnumerable)
		{
			var enumerableType = enumerable.GetType();

			if (enumerable is Microsoft.EntityFrameworkCore.Query.Internal.SingleQueryingEnumerable<TSource>)
			{
				var func =
					_relationalQueryContextCache.GetOrAdd(
						enumerableType,
						_ => Legion.Reflection.Delegates.DelegateFactory.FieldGet<Microsoft.EntityFrameworkCore.Query.Internal.SingleQueryingEnumerable<TSource>, RelationalQueryContext>("_relationalQueryContext")?.ToNonGeneric());

				relationalQueryContext = (RelationalQueryContext?)func?.Invoke(enumerable);
			}
			else if (enumerable is Microsoft.EntityFrameworkCore.Query.Internal.SplitQueryingEnumerable<TSource>)
			{
				var func =
					_relationalQueryContextCache.GetOrAdd(
						enumerableType,
						_ => Legion.Reflection.Delegates.DelegateFactory.FieldGet<Microsoft.EntityFrameworkCore.Query.Internal.SplitQueryingEnumerable<TSource>, RelationalQueryContext>("_relationalQueryContext")?.ToNonGeneric());

				relationalQueryContext = (RelationalQueryContext?)func?.Invoke(enumerable);
			}
			else if (enumerable is Microsoft.EntityFrameworkCore.Query.Internal.FromSqlQueryingEnumerable<TSource>)
			{
				var func =
					_relationalQueryContextCache.GetOrAdd(
						enumerableType,
						_ => Legion.Reflection.Delegates.DelegateFactory.FieldGet<Microsoft.EntityFrameworkCore.Query.Internal.FromSqlQueryingEnumerable<TSource>, RelationalQueryContext>("_relationalQueryContext")?.ToNonGeneric());

				relationalQueryContext = (RelationalQueryContext?)func?.Invoke(enumerable);
			}
		}

		return relationalQueryContext;
	}

	public static DbContextBase GetDbContext<TSource>(this IQueryable<TSource> query)
		where TSource : class
	{
		var relationalQueryContext = GetRelationalQueryContext(query);

		if (relationalQueryContext?.Context is not DbContextBase dbContextBase)
			throw new NotSupportedException($"Cannot get {nameof(DbContextBase)} from {query.GetType()?.FullName ?? "NULL"}");

		return dbContextBase;
	}

	public static QueryCacheManager GetDbContextQueryCacheManager<TSource>(this IQueryable<TSource> query)
		where TSource : class
	{
		var dbContext = GetDbContext<TSource>(query);

		if (dbContext.QueryCacheManager == null)
			throw new NotSupportedException($"Cannot get {nameof(QueryCacheManager)} from {query.GetType()?.FullName} >> {dbContext?.GetType()?.FullName ?? "NULL"}");

		return dbContext.QueryCacheManager;
	}

	public static bool TryGetDbContext<TSource>(this IQueryable<TSource> query, [NotNullWhen(true)] out DbContextBase? dbContextBase)
		where TSource : class
	{
		var relationalQueryContext = GetRelationalQueryContext(query);
		dbContextBase = relationalQueryContext?.Context as DbContextBase;
		return dbContextBase != null;
	}

	public static bool TryGetDbContextQueryCacheManager<TSource>(this IQueryable<TSource> query, [NotNullWhen(true)] out QueryCacheManager? queryCacheManager)
		where TSource : class
	{
		if (TryGetDbContext(query, out DbContextBase? dbContext))
			queryCacheManager = dbContext.QueryCacheManager;
		else
			queryCacheManager = null;

		return queryCacheManager != null;
	}
#pragma warning restore EF1001 // Internal EF Core API usage.


	/// <summary>A DbSet&lt;T&gt; extension method that expire cache.</summary>
	/// <param name="dbSet">The dbSet to act on.</param>
#pragma warning disable IDE0060 // Remove unused parameter
	public static void ExpireGlobalCache<T>(this DbSet<T> dbSet)
		where T : class
			=> GlobalQueryCacheManager.ExpireType(typeof(T));
#pragma warning restore IDE0060 // Remove unused parameter

	/// <summary>
	///     Return the result of the <paramref name="query" /> from the cache. If the query is not cached
	///     yet, the query is materialized asynchronously and cached before being returned.
	/// </summary>
	/// <typeparam name="T">The generic type of the query.</typeparam>
	/// <param name="query">The query to cache in the GlobalQueryCacheManager.</param>
	/// <param name="tags">
	///     A variable-length parameters list containing tags to expire cached
	///     entries.
	/// </param>
	/// <returns>The result of the query.</returns>
	public static List<T> FromGlobalCacheToList<T>(this IQueryable<T> query, params string[] tags)
		where T : class
		=> FromGlobalCacheToList<T>(query, false, null, tags);

	/// <summary>
	///     Return the result of the <paramref name="query" /> from the cache. If the query is not cached
	///     yet, the query is materialized asynchronously and cached before being returned.
	/// </summary>
	/// <typeparam name="T">The generic type of the query.</typeparam>
	/// <param name="query">The query to cache in the GlobalQueryCacheManager.</param>
	/// <param name="withChangeTracking">Indicates if the results of a query are tracked by the Microsoft.EntityFrameworkCore.ChangeTracking.ChangeTracker.</param>
	/// <param name="tags">
	///     A variable-length parameters list containing tags to expire cached
	///     entries.
	/// </param>
	/// <returns>The result of the query.</returns>
	public static List<T> FromGlobalCacheToList<T>(this IQueryable<T> query, bool withChangeTracking, params string[] tags)
		where T : class
		=> FromGlobalCacheToList<T>(query, withChangeTracking, null, tags);

	/// <summary>
	///     Return the result of the <paramref name="query" /> from the cache. If the query is not cached
	///     yet, the query is materialized asynchronously and cached before being returned.
	/// </summary>
	/// <typeparam name="T">The generic type of the query.</typeparam>
	/// <param name="query">The query to cache in the GlobalQueryCacheManager.</param>
	/// <param name="withChangeTracking">Indicates if the results of a query are tracked by the Microsoft.EntityFrameworkCore.ChangeTracking.ChangeTracker.</param>
	/// <param name="options">The cache entry options to use to cache the query.</param>
	/// <param name="tags">
	///     A variable-length parameters list containing tags to expire cached
	///     entries.
	/// </param>
	/// <returns>The result of the query.</returns>
	public static List<T> FromGlobalCacheToList<T>(this IQueryable<T> query, bool withChangeTracking, MemoryCacheEntryOptions? options, params string[] tags)
		where T : class
	{
		if (!GlobalQueryCacheManager.IsEnabled)
		{
			if (withChangeTracking)
				return query.ToList();
			else
				return query.AsNoTracking().ToList();
		}

		var key = GlobalQueryCacheManager.GetCacheKey(query, "ToList", tags);

		if (!GlobalQueryCacheManager.Cache.TryGetValue(key, out object? item))
		{
			options ??= GlobalQueryCacheManager.DefaultMemoryCacheEntryOptions ?? new MemoryCacheEntryOptions();

			options.RegisterPostEvictionCallback(GlobalQueryCacheManager.RemoveCallback);
			item = withChangeTracking
				? query.ToList()
				: query.AsNoTracking().ToList();
			item = GlobalQueryCacheManager.Cache.Set(key, item, options);
			GlobalQueryCacheManager.AddCacheTag(key, tags);
			GlobalQueryCacheManager.AddCacheTag(key, $"{typeof(T).Name}{GlobalQueryCacheManager.CacheTypePostfix}");
		}

		if (item == null || item == DBNull.Value)
			return [];

		return (List<T>)item;
	}

	/// <summary>
	///     Return the result of the <paramref name="query" /> from the cache. If the query is not cached
	///     yet, the query is materialized and cached before being returned.
	/// </summary>
	/// <typeparam name="T">The generic type of the query.</typeparam>
	/// <param name="query">The query to cache in the GlobalQueryCacheManager.</param>
	/// <param name="cancellationToken">The cancellation token.</param>
	/// <param name="tags">
	///     A variable-length parameters list containing tags to expire cached
	///     entries.
	/// </param>
	/// <returns>The result of the query.</returns>
	public static Task<List<T>> FromGlobalCacheToListAsync<T>(this IQueryable<T> query, CancellationToken cancellationToken = default, params string[] tags)
		where T : class
		=> FromGlobalCacheToListAsync(query, false, null, cancellationToken, tags);

	/// <summary>
	///     Return the result of the <paramref name="query" /> from the cache. If the query is not cached
	///     yet, the query is materialized and cached before being returned.
	/// </summary>
	/// <typeparam name="T">The generic type of the query.</typeparam>
	/// <param name="query">The query to cache in the GlobalQueryCacheManager.</param>
	/// <param name="withChangeTracking"></param>
	/// <param name="cancellationToken">The cancellation token.</param>
	/// <param name="tags">
	///     A variable-length parameters list containing tags to expire cached
	///     entries.
	/// </param>
	/// <returns>The result of the query.</returns>
	public static Task<List<T>> FromGlobalCacheToListAsync<T>(this IQueryable<T> query, bool withChangeTracking, CancellationToken cancellationToken = default, params string[] tags)
		where T : class
		=> FromGlobalCacheToListAsync(query, withChangeTracking, null, cancellationToken, tags);

	/// <summary>
	///     Return the result of the <paramref name="query" /> from the cache. If the query is not cached
	///     yet, the query is materialized and cached before being returned.
	/// </summary>
	/// <typeparam name="T">The generic type of the query.</typeparam>
	/// <param name="query">The query to cache in the GlobalQueryCacheManager.</param>
	/// <param name="withChangeTracking">Indicates if the results of a query are tracked by the Microsoft.EntityFrameworkCore.ChangeTracking.ChangeTracker.</param>
	/// <param name="options">The cache entry options to use to cache the query.</param>
	/// <param name="cancellationToken">The cancellation token.</param>
	/// <param name="tags">
	///     A variable-length parameters list containing tags to expire cached
	///     entries.
	/// </param>
	/// <returns>The result of the query.</returns>
	public static async Task<List<T>> FromGlobalCacheToListAsync<T>(this IQueryable<T> query, bool withChangeTracking, MemoryCacheEntryOptions? options, CancellationToken cancellationToken = default, params string[] tags)
		where T : class
	{
		if (!GlobalQueryCacheManager.IsEnabled)
		{
			if (withChangeTracking)
				return await query.ToListAsync(cancellationToken).ConfigureAwait(false);
			else
				return await query.AsNoTracking().ToListAsync(cancellationToken).ConfigureAwait(false);
		}

		var key = GlobalQueryCacheManager.GetCacheKey(query, "ToListAsync", tags);

		if (!GlobalQueryCacheManager.Cache.TryGetValue(key, out object? item))
		{
			options ??= GlobalQueryCacheManager.DefaultMemoryCacheEntryOptions ?? new MemoryCacheEntryOptions();

			options.RegisterPostEvictionCallback(GlobalQueryCacheManager.RemoveCallback);
			item = withChangeTracking
				? await query.ToListAsync(cancellationToken).ConfigureAwait(false)
				: await query.AsNoTracking().ToListAsync(cancellationToken).ConfigureAwait(false);
			item = GlobalQueryCacheManager.Cache.Set(key, item, options);
			GlobalQueryCacheManager.AddCacheTag(key, tags);
			GlobalQueryCacheManager.AddCacheTag(key, $"{typeof(T).Name}{GlobalQueryCacheManager.CacheTypePostfix}");
		}

		if (item == null || item == DBNull.Value)
			return [];

		return (List<T>)item;
	}

	public static T? FromGlobalCacheFirstOrDefault<T>(this IQueryable<T> query, params string[] tags)
		where T : class
		=> FromGlobalCacheFirstOrDefault<T>(query, false, null, tags);

	public static T? FromGlobalCacheFirstOrDefault<T>(this IQueryable<T> query, bool withChangeTracking, params string[] tags)
		where T : class
		=> FromGlobalCacheFirstOrDefault<T>(query, withChangeTracking, null, tags);

	public static T? FromGlobalCacheFirstOrDefault<T>(this IQueryable<T> query, bool withChangeTracking, MemoryCacheEntryOptions? options, params string[] tags)
		where T : class
	{
		if (!GlobalQueryCacheManager.IsEnabled)
		{
			if (withChangeTracking)
				return query.FirstOrDefault();
			else
				return query.AsNoTracking().FirstOrDefault();
		}

		var key = GlobalQueryCacheManager.GetCacheKey(query, "FirstOrDefault", tags);

		if (!GlobalQueryCacheManager.Cache.TryGetValue(key, out object? item))
		{
			options ??= GlobalQueryCacheManager.DefaultMemoryCacheEntryOptions ?? new MemoryCacheEntryOptions();

			options.RegisterPostEvictionCallback(GlobalQueryCacheManager.RemoveCallback);
			item = withChangeTracking
				? query.FirstOrDefault()
				: query.AsNoTracking().FirstOrDefault();
			item = GlobalQueryCacheManager.Cache.Set(key, item, options);
			GlobalQueryCacheManager.AddCacheTag(key, tags);
			GlobalQueryCacheManager.AddCacheTag(key, $"{typeof(T).Name}{GlobalQueryCacheManager.CacheTypePostfix}");
		}

		if (item == null || item == DBNull.Value)
			return default;

		return (T?)item;
	}

	public static Task<T?> FromGlobalCacheFirstOrDefaultAsync<T>(this IQueryable<T> query, CancellationToken cancellationToken = default, params string[] tags)
		where T : class
		=> FromGlobalCacheFirstOrDefaultAsync(query, false, null, cancellationToken, tags);

	public static Task<T?> FromGlobalCacheFirstOrDefaultAsync<T>(this IQueryable<T> query, bool withChangeTracking, CancellationToken cancellationToken = default, params string[] tags)
		where T : class
		=> FromGlobalCacheFirstOrDefaultAsync(query, withChangeTracking, null, cancellationToken, tags);

	public static async Task<T?> FromGlobalCacheFirstOrDefaultAsync<T>(this IQueryable<T> query, bool withChangeTracking, MemoryCacheEntryOptions? options, CancellationToken cancellationToken = default, params string[] tags)
		where T : class
	{
		if (!GlobalQueryCacheManager.IsEnabled)
		{
			if (withChangeTracking)
				return await query.FirstOrDefaultAsync(cancellationToken).ConfigureAwait(false);
			else
				return await query.AsNoTracking().FirstOrDefaultAsync(cancellationToken).ConfigureAwait(false);
		}

		var key = GlobalQueryCacheManager.GetCacheKey(query, "FirstOrDefaultAsync", tags);

		if (!GlobalQueryCacheManager.Cache.TryGetValue(key, out object? item))
		{
			options ??= GlobalQueryCacheManager.DefaultMemoryCacheEntryOptions ?? new MemoryCacheEntryOptions();

			options.RegisterPostEvictionCallback(GlobalQueryCacheManager.RemoveCallback);
			item = withChangeTracking
				? await query.FirstOrDefaultAsync(cancellationToken).ConfigureAwait(false)
				: await query.AsNoTracking().FirstOrDefaultAsync(cancellationToken).ConfigureAwait(false);
			item = GlobalQueryCacheManager.Cache.Set(key, item, options);
			GlobalQueryCacheManager.AddCacheTag(key, tags);
			GlobalQueryCacheManager.AddCacheTag(key, $"{typeof(T).Name}{GlobalQueryCacheManager.CacheTypePostfix}");
		}

		if (item == null || item == DBNull.Value)
			return default;

		return (T?)item;
	}

	public static bool FromGlobalCacheAll<T>(this IQueryable<T> query, Expression<Func<T, bool>> predicate, params string[] tags)
		where T : class
		=> FromGlobalCacheAll<T>(query, predicate, null, tags);

	public static bool FromGlobalCacheAll<T>(this IQueryable<T> query, Expression<Func<T, bool>> predicate, MemoryCacheEntryOptions? options, params string[] tags)
		where T : class
	{
		if (!GlobalQueryCacheManager.IsEnabled)
		{
			return query.AsNoTracking().All(predicate);
		}

		var key = GlobalQueryCacheManager.GetCacheKey(query, "All", tags);

		if (!GlobalQueryCacheManager.Cache.TryGetValue(key, out object? item))
		{
			options ??= GlobalQueryCacheManager.DefaultMemoryCacheEntryOptions ?? new MemoryCacheEntryOptions();

			options.RegisterPostEvictionCallback(GlobalQueryCacheManager.RemoveCallback);
			item = query.AsNoTracking().All(predicate);
			item = GlobalQueryCacheManager.Cache.Set(key, item, options);
			GlobalQueryCacheManager.AddCacheTag(key, tags);
			GlobalQueryCacheManager.AddCacheTag(key, $"{typeof(T).Name}{GlobalQueryCacheManager.CacheTypePostfix}");
		}

		if (item == null || item == DBNull.Value)
			return default;

		return (bool)item;
	}

	public static Task<bool> FromGlobalCacheAllAsync<T>(this IQueryable<T> query, Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default, params string[] tags)
		where T : class
		=> FromGlobalCacheAllAsync(query, predicate, null, cancellationToken, tags);

	public static async Task<bool> FromGlobalCacheAllAsync<T>(this IQueryable<T> query, Expression<Func<T, bool>> predicate, MemoryCacheEntryOptions? options, CancellationToken cancellationToken = default, params string[] tags)
		where T : class
	{
		if (!GlobalQueryCacheManager.IsEnabled)
		{
			return await query.AsNoTracking().AllAsync(predicate, cancellationToken).ConfigureAwait(false);
		}

		var key = GlobalQueryCacheManager.GetCacheKey(query, "AllAsync", tags);

		if (!GlobalQueryCacheManager.Cache.TryGetValue(key, out object? item))
		{
			options ??= GlobalQueryCacheManager.DefaultMemoryCacheEntryOptions ?? new MemoryCacheEntryOptions();

			options.RegisterPostEvictionCallback(GlobalQueryCacheManager.RemoveCallback);
			item = await query.AsNoTracking().AllAsync(predicate, cancellationToken).ConfigureAwait(false);
			item = GlobalQueryCacheManager.Cache.Set(key, item, options);
			GlobalQueryCacheManager.AddCacheTag(key, tags);
			GlobalQueryCacheManager.AddCacheTag(key, $"{typeof(T).Name}{GlobalQueryCacheManager.CacheTypePostfix}");
		}

		if (item == null || item == DBNull.Value)
			return default;

		return (bool)item;
	}

	public static bool FromGlobalCacheAny<T>(this IQueryable<T> query, params string[] tags)
		where T : class
		=> FromGlobalCacheAny<T>(query, (MemoryCacheEntryOptions?)null, tags);

	public static bool FromGlobalCacheAny<T>(this IQueryable<T> query, MemoryCacheEntryOptions? options, params string[] tags)
		where T : class
	{
		if (!GlobalQueryCacheManager.IsEnabled)
		{
			return query.AsNoTracking().Any();
		}

		var key = GlobalQueryCacheManager.GetCacheKey(query, "Any", tags);

		if (!GlobalQueryCacheManager.Cache.TryGetValue(key, out object? item))
		{
			options ??= GlobalQueryCacheManager.DefaultMemoryCacheEntryOptions ?? new MemoryCacheEntryOptions();

			options.RegisterPostEvictionCallback(GlobalQueryCacheManager.RemoveCallback);
			item = query.AsNoTracking().Any();
			item = GlobalQueryCacheManager.Cache.Set(key, item, options);
			GlobalQueryCacheManager.AddCacheTag(key, tags);
			GlobalQueryCacheManager.AddCacheTag(key, $"{typeof(T).Name}{GlobalQueryCacheManager.CacheTypePostfix}");
		}

		if (item == null || item == DBNull.Value)
			return default;

		return (bool)item;
	}

	public static Task<bool> FromGlobalCacheAnyAsync<T>(this IQueryable<T> query, CancellationToken cancellationToken = default, params string[] tags)
		where T : class
		=> FromGlobalCacheAnyAsync(query, (MemoryCacheEntryOptions?)null, cancellationToken, tags);

	public static async Task<bool> FromGlobalCacheAnyAsync<T>(this IQueryable<T> query, MemoryCacheEntryOptions? options, CancellationToken cancellationToken = default, params string[] tags)
		where T : class
	{
		if (!GlobalQueryCacheManager.IsEnabled)
		{
			return await query.AsNoTracking().AnyAsync(cancellationToken).ConfigureAwait(false);
		}

		var key = GlobalQueryCacheManager.GetCacheKey(query, "AnyAsync", tags);

		if (!GlobalQueryCacheManager.Cache.TryGetValue(key, out object? item))
		{
			options ??= GlobalQueryCacheManager.DefaultMemoryCacheEntryOptions ?? new MemoryCacheEntryOptions();

			options.RegisterPostEvictionCallback(GlobalQueryCacheManager.RemoveCallback);
			item = await query.AsNoTracking().AnyAsync(cancellationToken).ConfigureAwait(false);
			item = GlobalQueryCacheManager.Cache.Set(key, item, options);
			GlobalQueryCacheManager.AddCacheTag(key, tags);
			GlobalQueryCacheManager.AddCacheTag(key, $"{typeof(T).Name}{GlobalQueryCacheManager.CacheTypePostfix}");
		}

		if (item == null || item == DBNull.Value)
			return default;

		return (bool)item;
	}

	public static bool FromGlobalCacheAny<T>(this IQueryable<T> query, Expression<Func<T, bool>> predicate, params string[] tags)
		where T : class
		=> FromGlobalCacheAny<T>(query, predicate, null, tags);

	public static bool FromGlobalCacheAny<T>(this IQueryable<T> query, Expression<Func<T, bool>> predicate, MemoryCacheEntryOptions? options, params string[] tags)
		where T : class
	{
		if (!GlobalQueryCacheManager.IsEnabled)
		{
			return query.AsNoTracking().Any(predicate);
		}

		var key = GlobalQueryCacheManager.GetCacheKey(query, "Any_Predicate", tags);

		if (!GlobalQueryCacheManager.Cache.TryGetValue(key, out object? item))
		{
			options ??= GlobalQueryCacheManager.DefaultMemoryCacheEntryOptions ?? new MemoryCacheEntryOptions();

			options.RegisterPostEvictionCallback(GlobalQueryCacheManager.RemoveCallback);
			item = query.AsNoTracking().Any(predicate);
			item = GlobalQueryCacheManager.Cache.Set(key, item, options);
			GlobalQueryCacheManager.AddCacheTag(key, tags);
			GlobalQueryCacheManager.AddCacheTag(key, $"{typeof(T).Name}{GlobalQueryCacheManager.CacheTypePostfix}");
		}

		if (item == null || item == DBNull.Value)
			return default;

		return (bool)item;
	}

	public static Task<bool> FromGlobalCacheAnyAsync<T>(this IQueryable<T> query, Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default, params string[] tags)
		where T : class
		=> FromGlobalCacheAnyAsync(query, predicate, null, cancellationToken, tags);

	public static async Task<bool> FromGlobalCacheAnyAsync<T>(this IQueryable<T> query, Expression<Func<T, bool>> predicate, MemoryCacheEntryOptions? options, CancellationToken cancellationToken = default, params string[] tags)
		where T : class
	{
		if (!GlobalQueryCacheManager.IsEnabled)
		{
			return await query.AsNoTracking().AnyAsync(predicate, cancellationToken).ConfigureAwait(false);
		}

		var key = GlobalQueryCacheManager.GetCacheKey(query, "AnyAsync_Predicate", tags);

		if (!GlobalQueryCacheManager.Cache.TryGetValue(key, out object? item))
		{
			options ??= GlobalQueryCacheManager.DefaultMemoryCacheEntryOptions ?? new MemoryCacheEntryOptions();

			options.RegisterPostEvictionCallback(GlobalQueryCacheManager.RemoveCallback);
			item = await query.AsNoTracking().AnyAsync(predicate, cancellationToken).ConfigureAwait(false);
			item = GlobalQueryCacheManager.Cache.Set(key, item, options);
			GlobalQueryCacheManager.AddCacheTag(key, tags);
			GlobalQueryCacheManager.AddCacheTag(key, $"{typeof(T).Name}{GlobalQueryCacheManager.CacheTypePostfix}");
		}

		if (item == null || item == DBNull.Value)
			return default;

		return (bool)item;
	}

	public static int FromGlobalCacheCount<T>(this IQueryable<T> query, params string[] tags)
		where T : class
		=> FromGlobalCacheCount<T>(query, (MemoryCacheEntryOptions?)null, tags);

	public static int FromGlobalCacheCount<T>(this IQueryable<T> query, MemoryCacheEntryOptions? options, params string[] tags)
		where T : class
	{
		if (!GlobalQueryCacheManager.IsEnabled)
		{
			return query.AsNoTracking().Count();
		}

		var key = GlobalQueryCacheManager.GetCacheKey(query, "Count", tags);

		if (!GlobalQueryCacheManager.Cache.TryGetValue(key, out object? item))
		{
			options ??= GlobalQueryCacheManager.DefaultMemoryCacheEntryOptions ?? new MemoryCacheEntryOptions();

			options.RegisterPostEvictionCallback(GlobalQueryCacheManager.RemoveCallback);
			item = query.AsNoTracking().Count();
			item = GlobalQueryCacheManager.Cache.Set(key, item, options);
			GlobalQueryCacheManager.AddCacheTag(key, tags);
			GlobalQueryCacheManager.AddCacheTag(key, $"{typeof(T).Name}{GlobalQueryCacheManager.CacheTypePostfix}");
		}

		if (item == null || item == DBNull.Value)
			return default;

		return (int)item;
	}

	public static Task<int> FromGlobalCacheCountAsync<T>(this IQueryable<T> query, CancellationToken cancellationToken = default, params string[] tags)
		where T : class
		=> FromGlobalCacheCountAsync(query, (MemoryCacheEntryOptions?)null, cancellationToken, tags);

	public static async Task<int> FromGlobalCacheCountAsync<T>(this IQueryable<T> query, MemoryCacheEntryOptions? options, CancellationToken cancellationToken = default, params string[] tags)
		where T : class
	{
		if (!GlobalQueryCacheManager.IsEnabled)
		{
			return await query.AsNoTracking().CountAsync(cancellationToken).ConfigureAwait(false);
		}

		var key = GlobalQueryCacheManager.GetCacheKey(query, "CountAsync", tags);

		if (!GlobalQueryCacheManager.Cache.TryGetValue(key, out object? item))
		{
			options ??= GlobalQueryCacheManager.DefaultMemoryCacheEntryOptions ?? new MemoryCacheEntryOptions();

			options.RegisterPostEvictionCallback(GlobalQueryCacheManager.RemoveCallback);
			item = await query.AsNoTracking().CountAsync(cancellationToken).ConfigureAwait(false);
			item = GlobalQueryCacheManager.Cache.Set(key, item, options);
			GlobalQueryCacheManager.AddCacheTag(key, tags);
			GlobalQueryCacheManager.AddCacheTag(key, $"{typeof(T).Name}{GlobalQueryCacheManager.CacheTypePostfix}");
		}

		if (item == null || item == DBNull.Value)
			return default;

		return (int)item;
	}

	public static int FromGlobalCacheCount<T>(this IQueryable<T> query, Expression<Func<T, bool>> predicate, params string[] tags)
		where T : class
		=> FromGlobalCacheCount<T>(query, predicate, null, tags);

	public static int FromGlobalCacheCount<T>(this IQueryable<T> query, Expression<Func<T, bool>> predicate, MemoryCacheEntryOptions? options, params string[] tags)
		where T : class
	{
		if (!GlobalQueryCacheManager.IsEnabled)
		{
			return query.AsNoTracking().Count(predicate);
		}

		var key = GlobalQueryCacheManager.GetCacheKey(query, "Count_Predicate", tags);

		if (!GlobalQueryCacheManager.Cache.TryGetValue(key, out object? item))
		{
			options ??= GlobalQueryCacheManager.DefaultMemoryCacheEntryOptions ?? new MemoryCacheEntryOptions();

			options.RegisterPostEvictionCallback(GlobalQueryCacheManager.RemoveCallback);
			item = query.AsNoTracking().Count(predicate);
			item = GlobalQueryCacheManager.Cache.Set(key, item, options);
			GlobalQueryCacheManager.AddCacheTag(key, tags);
			GlobalQueryCacheManager.AddCacheTag(key, $"{typeof(T).Name}{GlobalQueryCacheManager.CacheTypePostfix}");
		}

		if (item == null || item == DBNull.Value)
			return default;

		return (int)item;
	}

	public static Task<int> FromGlobalCacheCountAsync<T>(this IQueryable<T> query, Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default, params string[] tags)
		where T : class
		=> FromGlobalCacheCountAsync(query, predicate, null, cancellationToken, tags);

	public static async Task<int> FromGlobalCacheCountAsync<T>(this IQueryable<T> query, Expression<Func<T, bool>> predicate, MemoryCacheEntryOptions? options, CancellationToken cancellationToken = default, params string[] tags)
		where T : class
	{
		if (!GlobalQueryCacheManager.IsEnabled)
		{
			return await query.AsNoTracking().CountAsync(predicate, cancellationToken).ConfigureAwait(false);
		}

		var key = GlobalQueryCacheManager.GetCacheKey(query, "CountAsync_Predicate", tags);

		if (!GlobalQueryCacheManager.Cache.TryGetValue(key, out object? item))
		{
			options ??= GlobalQueryCacheManager.DefaultMemoryCacheEntryOptions ?? new MemoryCacheEntryOptions();

			options.RegisterPostEvictionCallback(GlobalQueryCacheManager.RemoveCallback);
			item = await query.AsNoTracking().CountAsync(predicate, cancellationToken).ConfigureAwait(false);
			item = GlobalQueryCacheManager.Cache.Set(key, item, options);
			GlobalQueryCacheManager.AddCacheTag(key, tags);
			GlobalQueryCacheManager.AddCacheTag(key, $"{typeof(T).Name}{GlobalQueryCacheManager.CacheTypePostfix}");
		}

		if (item == null || item == DBNull.Value)
			return default;

		return (int)item;
	}
}
