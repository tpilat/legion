using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace Legion.Extensions;

public static class IOrderedQueryableExtensions
{
	[return: NotNullIfNotNull(nameof(query))]
	public static IOrderedQueryable<TSource>? ThenBySafe<TSource, TKey>(this IOrderedQueryable<TSource> query, Expression<Func<TSource, TKey>> keySelector)
	{
		if (query == null || keySelector == null)
			return query;

		return query.ThenBy(keySelector);
	}

	[return: NotNullIfNotNull(nameof(query))]
	public static IOrderedQueryable<TSource>? ThenByDescendingSafe<TSource, TKey>(this IOrderedQueryable<TSource> query, Expression<Func<TSource, TKey>> keySelector)
	{
		if (query == null || keySelector == null)
			return query;

		return query.ThenByDescending(keySelector);
	}

	[return: NotNullIfNotNull(nameof(query))]
	public static IOrderedQueryable<TSource>? ThenBySafe<TSource, TKey>(this IOrderedQueryable<TSource> query, string propertyName)
	{
		if (query == null)
			return query;

		return query.ThenBy(propertyName);
	}

	[return: NotNullIfNotNull(nameof(query))]
	public static IOrderedQueryable<TSource>? ThenByDescendingSafe<TSource, TKey>(this IOrderedQueryable<TSource> query, string propertyName)
	{
		if (query == null)
			return query;

		return query.ThenByDescending(propertyName);
	}

	public static IOrderedQueryable<TSource> ThenBy<TSource>(this IOrderedQueryable<TSource> query, string propertyName)
	{
		ParameterExpression param = Expression.Parameter(typeof(TSource), "p");
		MemberExpression property = Expression.PropertyOrField(param, propertyName);
		LambdaExpression sort = Expression.Lambda(property, param);

		var call = Expression.Call(
			typeof(Queryable),
			"ThenBy",
			new[] { typeof(TSource), property.Type },
			query.Expression,
			Expression.Quote(sort));

		return (IOrderedQueryable<TSource>)query.Provider.CreateQuery<TSource>(call);
	}

	public static IOrderedQueryable<TSource> ThenByDescending<TSource>(this IOrderedQueryable<TSource> query, string propertyName)
	{
		ParameterExpression param = Expression.Parameter(typeof(TSource), "p");
		MemberExpression property = Expression.PropertyOrField(param, propertyName);
		LambdaExpression sort = Expression.Lambda(property, param);

		var call = Expression.Call(
			typeof(Queryable),
			"ThenByDescending",
			new[] { typeof(TSource), property.Type },
			query.Expression,
			Expression.Quote(sort));

		return (IOrderedQueryable<TSource>)query.Provider.CreateQuery<TSource>(call);
	}
}
