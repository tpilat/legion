using System.Reflection;

namespace Legion.Reflection;

public interface ITypeWrapper
{
	ObjectInfo ObjectInfo { get; }

	object CreateNewInstanceWithoutConstructor();
}

public interface ITypeWrapper<T> : ITypeWrapper
{
	IReadOnlyDictionary<string, Func<T?, object?>> Getters { get; }
	IReadOnlyDictionary<string, Func<T?, object?>> StaticGetters { get; }
	IReadOnlyDictionary<string, Action<T?, object?>> Setters { get; }
	IReadOnlyDictionary<string, Action<T?, object?>> StaticSetters { get; }
}
