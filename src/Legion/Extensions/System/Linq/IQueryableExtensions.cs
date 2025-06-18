using Legion.Exceptions;
using Legion.Expressions;
using Legion.Queries;
using Legion.Queries.Paging;
using Legion.Queries.Sorting;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Reflection;

namespace Legion.Extensions;

public static class IQueryableExtensions
{
	public static string ToExpressionString<TSource>(this IQueryable<TSource> query)
		=> ExpressionPrinter.Print(query);

	[return: NotNullIfNotNull(nameof(query))]
	public static IQueryable<TSource>? OrderSafe<TSource, TKey>(this IQueryable<TSource> query, Expression<Func<TSource, TKey>> keySelector, ListSortDirection direction = ListSortDirection.Ascending)
	{
		if (query == null || keySelector == null)
			return query;

		return direction == ListSortDirection.Ascending
			? query.OrderBy(keySelector)
			: query.OrderByDescending(keySelector);
	}

	[return: NotNullIfNotNull(nameof(query))]
	public static IOrderedQueryable<TSource>? OrderBySafe<TSource, TKey>(this IQueryable<TSource> query, Expression<Func<TSource, TKey>> keySelector)
	{
		if (query == null || keySelector == null)
			return (IOrderedQueryable<TSource>?)query;

		return query.OrderBy(keySelector);
	}

	[return: NotNullIfNotNull(nameof(query))]
	public static IOrderedQueryable<TSource>? OrderByDescendingSafe<TSource, TKey>(this IQueryable<TSource> query, Expression<Func<TSource, TKey>> keySelector)
	{
		if (query == null || keySelector == null)
			return (IOrderedQueryable<TSource>?)query;

		return query.OrderByDescending(keySelector);
	}

	[return: NotNullIfNotNull(nameof(query))]
	public static IOrderedQueryable<TSource>? OrderBySafe<TSource, TKey>(this IQueryable<TSource> query, string propertyName)
	{
		if (query == null)
			return (IOrderedQueryable<TSource>?)query;

		return query.OrderBy(propertyName);
	}

	[return: NotNullIfNotNull(nameof(query))]
	public static IOrderedQueryable<TSource>? OrderByDescendingSafe<TSource, TKey>(this IQueryable<TSource> query, string propertyName)
	{
		if (query == null)
			return (IOrderedQueryable<TSource>?)query;

		return query.OrderByDescending(propertyName);
	}

	public static IOrderedQueryable<TSource> OrderBy<TSource>(this IQueryable<TSource> query, string propertyName)
	{
		ParameterExpression param = Expression.Parameter(typeof(TSource), "p");
		MemberExpression property = Expression.PropertyOrField(param, propertyName);
		LambdaExpression sort = Expression.Lambda(property, param);

		var call = Expression.Call(
			typeof(Queryable),
			"OrderBy",
			new[] { typeof(TSource), property.Type },
			query.Expression,
			Expression.Quote(sort));

		return (IOrderedQueryable<TSource>)query.Provider.CreateQuery<TSource>(call);
	}

	public static IOrderedQueryable<TSource> OrderByDescending<TSource>(this IQueryable<TSource> query, string propertyName)
	{
		ParameterExpression param = Expression.Parameter(typeof(TSource), "p");
		MemberExpression property = Expression.PropertyOrField(param, propertyName);
		LambdaExpression sort = Expression.Lambda(property, param);

		var call = Expression.Call(
			typeof(Queryable),
			"OrderByDescending",
			new[] { typeof(TSource), property.Type },
			query.Expression,
			Expression.Quote(sort));

		return (IOrderedQueryable<TSource>)query.Provider.CreateQuery<TSource>(call);
	}

	[return: NotNullIfNotNull(nameof(query))]
	public static IQueryable<TSource>? SkipSafe<TSource>(this IQueryable<TSource> query, int count)
	{
		if (query == null || count < 1)
			return query;

		return query.Skip(count);
	}

	[return: NotNullIfNotNull(nameof(query))]
	public static IQueryable<TSource>? TakeSafe<TSource>(this IQueryable<TSource> query, int count)
	{
		if (query == null || count < 1)
			return query;

		return query.Take(count);
	}

	[return: NotNullIfNotNull(nameof(query))]
	public static IQueryable<TSource>? WhereSafe<TSource>(this IQueryable<TSource> query, Expression<Func<TSource, bool>> whereExpression)
	{
		if (query == null || whereExpression == null)
			return query;

		return query.Where(whereExpression);
	}

	[return: NotNullIfNotNull(nameof(query))]
	public static IQueryable<TSource>? SelectSafe<TSource>(this IQueryable<TSource> query, Expression<Func<TSource, TSource>> selector)
	{
		if (query == null || selector == null)
			return query;

		return query.Select(selector);
	}

	public static bool AnySafe<TSource>(this IQueryable<TSource> query, Expression<Func<TSource, bool>> predicate)
	{
		if (query == null)
			return false;
		if (predicate == null)
			return query.Any();

		return query.Any(predicate);
	}

	public static bool AllSafe<TSource>(this IQueryable<TSource> query, Expression<Func<TSource, bool>> predicate)
	{
		if (query == null || predicate == null)
			return false;

		return query.All(predicate);
	}

	public static IQueryable<TEntity> FullTextSearch<TEntity>(this IQueryable<TEntity> queryable, string searchText, bool exactMatch = false)
	{
		if (searchText == null)
			throw new ArgumentNullException(nameof(searchText));

		ParameterExpression parameter = Expression.Parameter(typeof(TEntity), "c");

		var containsMethod = typeof(string).GetMethod("Contains", new Type[] { typeof(string) });
		if (containsMethod == null)
			throw new InvalidOperationException($"{nameof(containsMethod)} == null");

		var toStringMethod = typeof(object).GetMethod("ToString", []);

		var publicStringProperties = typeof(TEntity).GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly).Where(p => p.PropertyType == typeof(string));
		Expression? orExpressions = null;

		string[] searchTextParts;
		if (exactMatch)
		{
			searchTextParts = [searchText];
		}
		else
		{
			searchTextParts = searchText.Split(' ');
		}

		foreach (var property in publicStringProperties)
		{
			Expression nameProperty = Expression.Property(parameter, property);
			foreach (var searchTextPart in searchTextParts)
			{
				Expression searchTextExpression = Expression.Constant(searchTextPart);
				Expression callContainsMethod = Expression.Call(nameProperty, containsMethod, searchTextExpression);
				if (orExpressions == null)
				{
					orExpressions = callContainsMethod;
				}
				else
				{
					orExpressions = Expression.Or(orExpressions, callContainsMethod);
				}
			}
		}

		if (orExpressions == null)
			throw new InvalidOperationException($"No {nameof(publicStringProperties)} | {nameof(orExpressions)} == null");

		MethodCallExpression whereCallExpression = Expression.Call(
			typeof(Queryable),
			"Where",
			new Type[] { queryable.ElementType },
			queryable.Expression,
			Expression.Lambda<Func<TEntity, bool>>(orExpressions, new ParameterExpression[] { parameter }));

		return queryable.Provider.CreateQuery<TEntity>(whereCallExpression);
	}

	public static IQueryable<T> Sort<T>(this IQueryable<T> source, Action<SortDescriptorBuilder<T>>? sorting)
	{
		ArgNullException.ThrowIfNull(source);

		if (sorting == null)
			return source;

		var builder = new SortDescriptorBuilder<T>();
		sorting.Invoke(builder);
		return ((IQueryModifier<T>)builder).ApplySort(source);
	}

	public static IQueryable<T> GetPage<T>(this IQueryable<T> source, Action<PagingDescriptorBuilder<T>>? paging)
	{
		ArgNullException.ThrowIfNull(source);

		if (paging == null)
			return source;

		var builder = new PagingDescriptorBuilder<T>();
		paging.Invoke(builder);

		return ((IQueryModifier<T>)builder).ApplyPaging(source);
	}

	public static IQueryable<T> Apply<T>(this IQueryable<T> source, Action<QueryableBuilder<T>>? queryableBuilder)
		where T: class
	{
		ArgNullException.ThrowIfNull(source);

		if (queryableBuilder == null)
			return source;

		var builder = new QueryableBuilder<T>();
		queryableBuilder.Invoke(builder);

		return ((IQueryModifier<T>)builder).Apply(source);
	}

	public static IQueryable<T> Apply<T>(this IQueryable<T> source, IQueryableBuilder<T>? queryableBuilder)
		where T : class
	{
		ArgNullException.ThrowIfNull(source);

		if (queryableBuilder == null)
			return source;

		return queryableBuilder.Apply(source);
	}

	public static IQueryable<T> ApplyIncludes<T>(this IQueryable<T> source, Action<QueryableBuilder<T>>? queryableBuilder)
		where T : class
	{
		ArgNullException.ThrowIfNull(source);

		if (queryableBuilder == null)
			return source;

		var builder = new QueryableBuilder<T>();
		queryableBuilder.Invoke(builder);

		return ((IQueryModifier<T>)builder).ApplyIncludes(source);
	}

	public static IQueryable<T> ApplyIncludes<T>(this IQueryable<T> source, IQueryableBuilder<T>? queryableBuilder)
		where T : class
	{
		ArgNullException.ThrowIfNull(source);

		if (queryableBuilder == null)
			return source;

		return queryableBuilder.ApplyIncludes(source);
	}

	public static IQueryable<T> ApplyPaging<T>(this IQueryable<T> source, Action<QueryableBuilder<T>>? queryableBuilder)
		where T : class
	{
		ArgNullException.ThrowIfNull(source);

		if (queryableBuilder == null)
			return source;

		var builder = new QueryableBuilder<T>();
		queryableBuilder.Invoke(builder);

		return ((IQueryModifier<T>)builder).ApplyPaging(source);
	}

	public static IQueryable<T> ApplyPaging<T>(this IQueryable<T> source, IQueryableBuilder<T>? queryableBuilder)
		where T : class
	{
		ArgNullException.ThrowIfNull(source);

		if (queryableBuilder == null)
			return source;

		return queryableBuilder.ApplyPaging(source);
	}

	public static IQueryable<T> ApplySort<T>(this IQueryable<T> source, Action<QueryableBuilder<T>>? queryableBuilder)
		where T : class
	{
		ArgNullException.ThrowIfNull(source);

		if (queryableBuilder == null)
			return source;

		var builder = new QueryableBuilder<T>();
		queryableBuilder.Invoke(builder);

		return ((IQueryModifier<T>)builder).ApplySort(source);
	}

	public static IQueryable<T> ApplySort<T>(this IQueryable<T> source, IQueryableBuilder<T>? queryableBuilder)
		where T : class
	{
		ArgNullException.ThrowIfNull(source);

		if (queryableBuilder == null)
			return source;

		return queryableBuilder.ApplySort(source);
	}
}
