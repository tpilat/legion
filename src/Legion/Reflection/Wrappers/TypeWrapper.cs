using Legion.Extensions;
using Legion.Reflection.Internal;
using System.Collections.Concurrent;
using System.Reflection;

namespace Legion.Reflection;

public class TypeWrapper<T> : ITypeWrapper<T>, ITypeWrapper
{
	private static readonly ConcurrentDictionary<Type, ITypeWrapper> _typeManagerCache = new ();
	private static readonly Type _delegateType = typeof(Delegate);

	public Dictionary<string, Func<T?, object?>> Getters { get; }
	public Dictionary<string, Func<T?, object?>> StaticGetters { get; }
	public Dictionary<string, Action<T?, object?>> Setters { get; }
	public Dictionary<string, Action<T?, object?>> StaticSetters { get; }
	//public Dictionary<string, MethodCall<T?, object?>> Methods { get; }
	//public Dictionary<string, MethodCall<T?, object?>> StaticMethods { get; }

	public ObjectInfo ObjectInfo { get; }

	IReadOnlyDictionary<string, Func<T?, object?>> ITypeWrapper<T>.Getters => Getters;

	IReadOnlyDictionary<string, Func<T?, object?>> ITypeWrapper<T>.StaticGetters => StaticGetters;

	IReadOnlyDictionary<string, Action<T?, object?>> ITypeWrapper<T>.Setters => Setters;

	IReadOnlyDictionary<string, Action<T?, object?>> ITypeWrapper<T>.StaticSetters => StaticSetters;

	private TypeWrapper(ObjectInfoOptions objectInfoOptions)
	{
		Getters = [];
		StaticGetters = [];
		Setters = [];
		StaticSetters = [];
		//Methods = [];
		//StaticMethods = [];

		ObjectInfo = new ObjectInfo(typeof(T), objectInfoOptions);

		foreach (var property in ObjectInfo.Properties)
		{
			//skip indexers
			if (0 < property.GetIndexParameters()?.Length)
				continue;

			//skip events
			if (_delegateType.IsAssignableFrom(property.PropertyType))
				continue;

			if (property.IsStatic())
			{
				if (property.CanRead)
				{
					var getter = DelegateFactory.Instance.CreateGet<T>(property);
					StaticGetters.TryAdd(property.Name, getter);
				}
				if (property.CanWrite)
				{
					var setter = DelegateFactory.Instance.CreateSet<T>(property);
					StaticSetters.TryAdd(property.Name, setter);
				}
			}
			else
			{
				if (property.CanRead)
				{
					var getter = DelegateFactory.Instance.CreateGet<T>(property);
					Getters.TryAdd(property.Name, getter);
				}
				if (property.CanWrite)
				{
					var setter = DelegateFactory.Instance.CreateSet<T>(property);
					Setters.TryAdd(property.Name, setter);
				}
			}
		}

		foreach (var field in ObjectInfo.Fields)
		{
			//skip events
			if (_delegateType.IsAssignableFrom(field.FieldType))
				continue;

			var getter = DelegateFactory.Instance.CreateGet<T>(field);

			if (field.IsStatic())
			{
				StaticGetters.TryAdd(field.Name, getter);
				
				if (!field.IsConst())
				{
					var setter = DelegateFactory.Instance.CreateSet<T>(field);
					StaticSetters.TryAdd(field.Name, setter);
				}
			}
			else
			{
				Getters.TryAdd(field.Name, getter);

				if (!field.IsConst())
				{
					var setter = DelegateFactory.Instance.CreateSet<T>(field);
					Setters.TryAdd(field.Name, setter);
				}
			}
		}

		//var methods = type.Methods();
		//foreach (var method in methods)
		//{
		//	var methodCall = DelegateFactory.Instance.CreateMethodCall<T>(method);

		//	if (method.IsStatic())
		//	{
		//		StaticMethods.TryAdd(method.Name, methodCall);
		//	}
		//	else
		//	{
		//		Methods.TryAdd(method.Name, methodCall);
		//	}
		//}
	}

	public static TypeWrapper<T> Create(BindingFlags bindingFlags)
		=> (TypeWrapper<T>)_typeManagerCache.GetOrAdd(typeof(T), type => new TypeWrapper<T>(new ObjectInfoOptions { BindingFlags = bindingFlags }));

#pragma warning disable SYSLIB0050 // Type or member is obsolete
	private object CreateNewInstanceWithoutConstructorInternal()
	{
#if NET8_0_OR_GREATER
		var instance = System.Runtime.CompilerServices.RuntimeHelpers.GetUninitializedObject(ObjectInfo.Type);
#else
		var instance = System.Runtime.Serialization.FormatterServices.GetUninitializedObject(ObjectInfo.Type);
#endif

		return instance;
	}

#pragma warning restore SYSLIB0050 // Type or member is obsolete

	public T CreateNewInstanceWithoutConstructor()
		=> (T)CreateNewInstanceWithoutConstructorInternal();

	object ITypeWrapper.CreateNewInstanceWithoutConstructor()
		=> CreateNewInstanceWithoutConstructorInternal();
}
