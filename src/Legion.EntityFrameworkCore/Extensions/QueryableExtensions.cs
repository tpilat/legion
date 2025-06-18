using Legion.EntityFrameworkCore.Expressions;
using Legion.EntityFrameworkCore.Expressions.Sorting;
using Legion.EntityFrameworkCore.Interceptors;
using Legion.EntityFrameworkCore.Queries;
using Microsoft.EntityFrameworkCore;

namespace Legion.Extensions;

public static class QueryableExtensions
{
	public static IQueryable OrderBy(this IQueryable source, IEnumerable<SortDescriptor> sortDescriptors)
	{
		var builder = new SortDescriptorCollectionExpressionBuilder(source, sortDescriptors);
		return builder.Sort();
	}

	public static IQueryable<T> OrderBy<T>(this IQueryable<T> source, IEnumerable<SortDescriptor> sortDescriptors)
	{
		var builder = new SortDescriptorCollectionExpressionBuilder<T>(source, sortDescriptors);
		return builder.Sort();
	}

	public static IQueryable<T> Apply<T>(this IQueryable<T> source, Action<QueryableBuilder<T>>? queryableBuilder)
		where T: class
	{
		Throw.IfArgumentNull(source);

		if (queryableBuilder == null)
			return source;

		var builder = new QueryableBuilder<T>();
		queryableBuilder.Invoke(builder);

		return ((Legion.Queries.IQueryModifier<T>)builder).Apply(source);
	}

	public static IQueryable<T> ApplyIncludes<T>(this IQueryable<T> source, Action<QueryableBuilder<T>>? queryableBuilder)
		where T : class
	{
		Throw.IfArgumentNull(source);

		if (queryableBuilder == null)
			return source;

		var builder = new QueryableBuilder<T>();
		queryableBuilder.Invoke(builder);

		return ((Legion.Queries.IQueryModifier<T>)builder).ApplyIncludes(source);
	}

	public static IQueryable<T> ApplyPaging<T>(this IQueryable<T> source, Action<QueryableBuilder<T>>? queryableBuilder)
		where T : class
	{
		Throw.IfArgumentNull(source);

		if (queryableBuilder == null)
			return source;

		var builder = new QueryableBuilder<T>();
		queryableBuilder.Invoke(builder);

		return ((Legion.Queries.IQueryModifier<T>)builder).ApplyPaging(source);
	}

	public static IQueryable<T> ApplySort<T>(this IQueryable<T> source, Action<QueryableBuilder<T>>? queryableBuilder)
		where T : class
	{
		Throw.IfArgumentNull(source);

		if (queryableBuilder == null)
			return source;

		var builder = new QueryableBuilder<T>();
		queryableBuilder.Invoke(builder);

		return ((Legion.Queries.IQueryModifier<T>)builder).ApplySort(source);
	}

	//public static IEnumerable ToDataSourceResult(
	//	this IEnumerable enumerable,
	//	DataSourceRequest request)
	//{
	//	return enumerable.AsQueryable().ToDataSourceResult(request);
	//}

	//public static Task<IEnumerable> ToDataSourceResultAsync(
	//	this IEnumerable enumerable,
	//	DataSourceRequest request)
	//{
	//	return Task.Run(() => ToDataSourceResult(enumerable, request));
	//}

	//public static Task<IEnumerable> ToDataSourceResultAsync(
	//	this IEnumerable enumerable,
	//	DataSourceRequest request,
	//	CancellationToken cancellation)
	//{
	//	return Task.Run(() => ToDataSourceResult(enumerable, request), cancellation);
	//}

	//public static IQueryable ToDataSourceResult(
	//	this IQueryable queryable,
	//	DataSourceRequest request)
	//{
	//	return queryable.CreateDataSourceResult(request);
	//}

	//public static Task<IQueryable> ToDataSourceResultAsync(
	//	this IQueryable queryable,
	//	DataSourceRequest request)
	//{
	//	return Task.Run(() => ToDataSourceResult(queryable, request));
	//}

	//public static Task<IQueryable> ToDataSourceResultAsync(
	//	this IQueryable queryable,
	//	DataSourceRequest request,
	//	CancellationToken cancellationToken)
	//{
	//	return Task.Run(() => ToDataSourceResult(queryable, request), cancellationToken);
	//}

	//public static IEnumerable<T> ToDataSourceResult<T>(
	//	this IEnumerable<T> enumerable,
	//	DataSourceRequest request)
	//{
	//	return enumerable.AsQueryable().CreateDataSourceResult<T>(request);
	//}

	//public static Task<IEnumerable<T>> ToDataSourceResultAsync<T>(
	//	this IEnumerable<T> enumerable,
	//	DataSourceRequest request)
	//{
	//	return Task.Run(() => ToDataSourceResult(enumerable, request));
	//}

	//public static Task<IEnumerable<T>> ToDataSourceResultAsync<T>(
	//	this IEnumerable<T> enumerable,
	//	DataSourceRequest request,
	//	CancellationToken cancellationToken)
	//{
	//	return Task.Run(() => ToDataSourceResult(enumerable, request), cancellationToken);
	//}

	//public static IQueryable<T> ToDataSourceResult<T>(
	//	this IQueryable<T> enumerable,
	//	DataSourceRequest request)
	//{
	//	return enumerable.CreateDataSourceResult<T>(request);
	//}

	//public static Task<IQueryable<T>> ToDataSourceResultAsync<T>(
	//	this IQueryable<T> queryable,
	//	DataSourceRequest request)
	//{
	//	return Task.Run(() => ToDataSourceResult(queryable, request));
	//}

	//public static Task<IQueryable<T>> ToDataSourceResultAsync<T>
	//	(this IQueryable<T> queryable, DataSourceRequest request, CancellationToken cancellationToken)
	//{
	//	return Task.Run(() => ToDataSourceResult<T>(queryable, request), cancellationToken);
	//}

	//private static IQueryable CreateDataSourceResult(this IQueryable queryable, DataSourceRequest request)
	//{
	//	var result = queryable;

	//	if (request.Sorts?.Any() == true)
	//		result = result.OrderBy(request.Sorts);

	//	return result;
	//}

	//private static IQueryable<T> CreateDataSourceResult<T>(this IQueryable<T> queryable, DataSourceRequest request)
	//{
	//	var result = queryable;

	//	if (request.Sorts?.Any() == true)
	//		result = result.OrderBy(request.Sorts);

	//	return result;
	//}

	//private static IEnumerable Execute(this IQueryable source)
	//{
	//	if (source == null)
	//		throw new ArgumentNullException(nameof(source));

	//	var type = source.ElementType;

	//	var list = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(type))!;

	//	foreach (var item in source)
	//	{
	//		list.Add(item);
	//	}

	//	return list;
	//}

	//private static IEnumerable<T> Execute<T>(this IQueryable<T> source)
	//{
	//	if (source == null)
	//		throw new ArgumentNullException(nameof(source));

	//	var list = new List<T>();

	//	foreach (var item in source)
	//	{
	//		list.Add(item);
	//	}

	//	return list;
	//}

	/// <summary>
	/// Waits for the lock to be released.
	/// <para><strong>PostgreSQL</strong> - SELECT * FROM table <strong>FOR UPDATE;</strong></para>
	/// <para><strong>SqlServer</strong> - SELECT * FROM table <strong>WITH (UPDLOCK)</strong></para>
	/// </summary>
	public static IQueryable<T> TryLockForUpdate<T>(this IQueryable<T> source)
	{
		Throw.IfArgumentNull(source);

		return source
			.TagWith(nameof(RowLockHints.LEGION_FOR_UPDATE));
	}

	/// <summary>
	/// Throws an error immediately.
	/// <para><strong>PostgreSQL</strong> - SELECT * FROM table <strong>FOR UPDATE NOWAIT;</strong></para>
	/// <para><strong>SqlServer</strong> - SELECT * FROM table <strong>WITH (UPDLOCK, NOWAIT)</strong></para>
	/// </summary>
	public static IQueryable<T> TryLockNowait<T>(this IQueryable<T> source)
	{
		Throw.IfArgumentNull(source);

		return source
			.TagWith(nameof(RowLockHints.LEGION_NOWAIT));
	}

	/// <summary>
	/// Skips the locked rows.
	/// <para><strong>PostgreSQL</strong> - SELECT * FROM table <strong>FOR UPDATE SKIP LOCKED;</strong></para>
	/// <para><strong>SqlServer</strong> - SELECT * FROM table <strong>WITH (UPDLOCK, READPAST)</strong></para>
	/// </summary>
	public static IQueryable<T> TryLockSkipLocked<T>(this IQueryable<T> source)
	{
		Throw.IfArgumentNull(source);

		return source
			.TagWith(nameof(RowLockHints.LEGION_SKIP_LOCKED));
	}

	/// <summary>
	/// Acquires a shared lock on rows, allowing other transactions to read the rows but not modify them.
	/// <para><strong>PostgreSQL</strong> - SELECT * FROM table <strong>FOR SHARE;</strong></para>
	/// <para><strong>SqlServer</strong> - SELECT * FROM table <strong>WITH (ROWLOCK, HOLDLOCK)</strong></para>
	/// </summary>
	public static IQueryable<T> TryLockForShare<T>(this IQueryable<T> source)
	{
		Throw.IfArgumentNull(source);

		return source
			.TagWith(nameof(RowLockHints.LEGION_FOR_SHARE));
	}
}
