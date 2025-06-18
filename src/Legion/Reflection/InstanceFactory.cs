namespace Legion.Reflection;

public class InstanceFactory
{
	private readonly Dictionary<Type, Func<InstanceFactory, object>> _factories;

	private readonly HashSet<Type> _constructorlessUninitializedObjectTypes;

	public Dictionary<string, object> Context { get; }
	public HashSet<Type> ConstructorlessUninitializedObjectTypes => _constructorlessUninitializedObjectTypes;
	public IReadOnlyDictionary<Type, Func<InstanceFactory, object>> Factories => _factories;

	public InstanceFactory()
		: this(null!)
	{
	}

	public InstanceFactory(Dictionary<string, object> context)
	{
		_factories = [];
		_constructorlessUninitializedObjectTypes = [];
		Context = context ?? [];
	}

	public T? CreateInstance<T>(bool useActivatorIfNoFactoryFound = false, bool throwIfFactoryReturnsNull = true)
		where T : class
	{
		var type = typeof(T);

		if (ConstructorlessUninitializedObjectTypes.Contains(type))
		{
#if NET8_0_OR_GREATER
					return (T)System.Runtime.CompilerServices.RuntimeHelpers.GetUninitializedObject(type);
#else
			return (T)System.Runtime.Serialization.FormatterServices.GetUninitializedObject(type);
#endif
		}
		else
		{
			if (Factories.TryGetValue(type, out var factory))
			{
				var instance = (T)factory.Invoke(this);

				if (throwIfFactoryReturnsNull && instance == null)
					Throw.InvalidOperationException($"{nameof(InstanceFactory)} for type {type.FullName} returned null");

				return instance;
			}
			else
			{
				return useActivatorIfNoFactoryFound
					? Activator.CreateInstance<T>()
					: null;
			}
		}
	}

	public InstanceFactory WithFactory<T>(Func<InstanceFactory, T> factory)
		where T : class
	{
		_factories[typeof(T)] = factory;
		return this;
	}

	public InstanceFactory WithConstructorlessUninitializedObjectType<T>()
		where T : class
	{
		_constructorlessUninitializedObjectTypes.Add(typeof(T));
		return this;
	}
}
