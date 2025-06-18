using System.Collections.Concurrent;
using System.Linq.Expressions;

namespace Legion.Reflection;

public class ArrayTypeWrapper
{
	private static readonly ConcurrentDictionary<Type, ArrayTypeWrapper> _cache = [];

	public static ArrayTypeWrapper Create(object arrayInstance)
	{
		Throw.IfArgumentNull(arrayInstance);

		var arrayType = arrayInstance.GetType();

		if (arrayInstance is not Array)
			Throw.ArgumentException(arrayInstance, $"Not an array | {arrayType.FullName}");

		return Create(arrayType);
	}

	public static ArrayTypeWrapper Create(Type arrayType)
	{
		Throw.IfArgumentNull(arrayType);
		return _cache.GetOrAdd(arrayType, t => new ArrayTypeWrapper(t));
	}

	private readonly Type _arrayType;
	private readonly Type _elementType;
	public Func<Array, int, object?> Getter { get; }
	public Action<Array, int, object?> Setter { get; }

	private ArrayTypeWrapper(Type arrayType)
	{
		_arrayType = arrayType;
		_elementType = _arrayType.GetElementType()!;

		Getter = CreateArrayElementGetter(_arrayType);
		Setter = CreateArrayElementSetter(_arrayType, _elementType);
	}

	private static Func<Array, int, object?> CreateArrayElementGetter(Type arrayType)
	{
		var arrayParam = Expression.Parameter(typeof(Array), "array");
		var indexParam = Expression.Parameter(typeof(int), "index");

		var castArray = Expression.Convert(arrayParam, arrayType);

		MethodCallExpression getValueCall = Expression.Call(
			castArray,
			arrayType.GetMethod("GetValue", [typeof(int)])!,
			indexParam);

		return Expression.Lambda<Func<Array, int, object?>>(
			Expression.Convert(getValueCall, typeof(object)),
			arrayParam,
			indexParam
		).Compile();
	}

	private static Action<Array, int, object?> CreateArrayElementSetter(Type arrayType, Type elementType)
	{
		var arrayParam = Expression.Parameter(typeof(Array), "array");
		var indexParam = Expression.Parameter(typeof(int), "index");
		var valueParam = Expression.Parameter(typeof(object), "value");

		var castArray = Expression.Convert(arrayParam, arrayType);
		var castValue = Expression.Convert(valueParam, elementType);

		// Create the expression to access the array at the specified index
		MethodCallExpression setValueCall = Expression.Call(
			castArray,
			arrayType.GetMethod("SetValue", [typeof(object), typeof(int)])!,
			Expression.Convert(castValue, typeof(object)),
			indexParam);

		// Compile the expression into a delegate
		return Expression.Lambda<Action<Array, int, object?>>(setValueCall, arrayParam, indexParam, valueParam).Compile();
	}

	public object CreateNewArrayInstance(int arrayLength)
		=> Array.CreateInstance(_elementType, arrayLength);
}
