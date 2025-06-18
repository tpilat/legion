using Microsoft.EntityFrameworkCore.Query;
using System.Collections;
using System.Linq.Expressions;

namespace Legion.EntityFrameworkCore;

public static class AsyncEnumerableExtensions
{
	public static IQueryable<T> AsAsyncQueryable<T>(this IEnumerable<T> enumerable)
		=> new AsyncEnumerable<T>(enumerable);
}








internal class EmptyExpressionVisitor : ExpressionVisitor
{
}

internal abstract class QueryProvider<T> : IOrderedQueryable<T>, IQueryProvider
{
	private IEnumerable<T> _enumerable;

	public Type ElementType => typeof(T);

	public Expression Expression { get; }

	public IQueryProvider Provider => this;

	protected QueryProvider(Expression expression)
	{
		Expression = expression;
	}

	protected QueryProvider(IEnumerable<T> enumerable)
	{
		_enumerable = enumerable;
		Expression = enumerable.AsQueryable().Expression;
	}

	public IQueryable CreateQuery(Expression expression)
	{
		if (expression is MethodCallExpression m)
		{
			var resultType = m.Method.ReturnType; // it should be IQueryable<T>
			var tElement = resultType.GetGenericArguments().First();
			return (IQueryable)CreateInstance(tElement, expression);
		}

		return CreateQuery<T>(expression);
	}

	public IQueryable<TEntity> CreateQuery<TEntity>(Expression expression)
		=> (IQueryable<TEntity>)CreateInstance(typeof(TEntity), expression);

	private object CreateInstance(Type tElement, Expression expression)
	{
		var queryType = GetType().GetGenericTypeDefinition().MakeGenericType(tElement);
		return Activator.CreateInstance(queryType, expression)!;
	}

	public object Execute(Expression expression)
		=> CompileExpressionItem<object>(expression);

	public TResult Execute<TResult>(Expression expression)
		=> CompileExpressionItem<TResult>(expression);

	IEnumerator<T> IEnumerable<T>.GetEnumerator()
	{
		_enumerable ??= CompileExpressionItem<IEnumerable<T>>(Expression);
		return _enumerable.GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		_enumerable ??= CompileExpressionItem<IEnumerable<T>>(Expression);
		return _enumerable.GetEnumerator();
	}

	private static TResult CompileExpressionItem<TResult>(Expression expression)
	{
		var visitor = new EmptyExpressionVisitor();
		var body = visitor.Visit(expression);
		var f = Expression.Lambda<Func<TResult>>(body ?? throw new InvalidOperationException($"{nameof(body)} is null"), (IEnumerable<ParameterExpression>)null!);
		return f.Compile()();
	}
}

internal class AsyncEnumerator<T>(IEnumerator<T> enumerator) : IAsyncEnumerator<T>
{
	private readonly IEnumerator<T> _enumerator = enumerator ?? throw new ArgumentNullException(nameof(enumerator));

	public T Current => _enumerator.Current;

	public ValueTask<bool> MoveNextAsync()
		=> new(_enumerator.MoveNext());

	public ValueTask DisposeAsync()
	{
		_enumerator.Dispose();
		return new ValueTask();
	}
}

internal class AsyncEnumerable<T> : QueryProvider<T>, IAsyncEnumerable<T>, IAsyncQueryProvider
{
	public AsyncEnumerable(Expression expression)
		: base(expression)
	{
	}

	public AsyncEnumerable(IEnumerable<T> enumerable)
		: base(enumerable)
	{
	}

	public TResult ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken)
	{
		var expectedResultType = typeof(TResult).GetGenericArguments()[0];
		var executionResult = typeof(IQueryProvider)
			.GetMethods()
			.First(method => method.Name == nameof(IQueryProvider.Execute) && method.IsGenericMethod)
			.MakeGenericMethod(expectedResultType)
			.Invoke(this, [expression]);

		return (TResult)typeof(Task).GetMethod(nameof(Task.FromResult))!
			.MakeGenericMethod(expectedResultType)
			.Invoke(null, [executionResult])!;
	}

	public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default)
		=> new AsyncEnumerator<T>(this.AsEnumerable().GetEnumerator());
}
