using System.Linq.Expressions;

namespace Legion.Expressions;

public static class ExpressionCacheHelper
{
	private static readonly Dictionary<int, Delegate> _cache = [];

	public static Func<T, object> CompileExpression<T>(Expression<Func<T, object>> expression)
	{
		var key = expression.GetHashCode();

		if (_cache.TryGetValue(key, out var cachedDelegate))
			return (Func<T, object>)cachedDelegate;

		var compiledDelegate = expression.Compile();
		_cache[key] = compiledDelegate;

		return compiledDelegate;
	}
}