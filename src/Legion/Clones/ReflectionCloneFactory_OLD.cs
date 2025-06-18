//using Legion.Extensions;
//using Legion.Reflection;
//using System.Collections.Concurrent;
//using System.Diagnostics.CodeAnalysis;
//using System.Reflection;

//namespace Legion.Clones;

//public class ReflectionCloneFactory : ICloneFactory
//{
//	private static readonly Type _reflectionCloneFactoryType = typeof(ReflectionCloneFactory);
//	private static readonly Type _dictionaryType = typeof(Dictionary<object, object?>);
//	private static readonly Type _stringType = typeof(string);
//	private static readonly ConcurrentDictionary<Type, Func<object[], object>?> _genericCloneInternalDelegates = [];

//	[return: NotNullIfNotNull(nameof(@object))]
//	public T? Clone<T>(T? @object)
//		=> CloneInternal(@object, []);

//	private static T? CloneInternal<T>(T? @object, Dictionary<object, object?> instances)
//	{
//		if (@object == null)
//			return @object;

//		if (instances!.TryGetValue(@object, out T? clone))
//			return clone;

//		var objectType = @object.GetType();
//		if (objectType.IsValueType || objectType == _stringType)
//			return @object;

//		if (@object is Array objectArray)
//		{
//			var cloneArrayTypeWrapper = ArrayTypeWrapper.Create(objectArray);
//			clone = (T)cloneArrayTypeWrapper.CreateNewArrayInstance(objectArray.Length);
//			var cloneArrayWrapper = new ArrayWrapper(cloneArrayTypeWrapper, clone);

//			var i = 0;
//			foreach (var objectItem in objectArray)
//			{
//				cloneArrayWrapper.SetValue(i, objectItem);
//				i++;
//			}

//			return clone!;
//		}

//		var objectWrapper = new ObjectWrapper<T>(@object, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
//		clone = objectWrapper.TypeWrapper.CreateNewInstanceWithoutConstructor();
//		instances[@object] = clone;

//		var cloneWrapper = new ObjectWrapper<T>(clone, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

//		foreach (var field in objectWrapper.TypeWrapper.ObjectInfo.Fields.Where(x => !x.IsStatic))
//		{
//			var fieldName = field.Name;
//			var fieldValue = objectWrapper.GetNonStaticValue(fieldName);
//			var fieldType = field.FieldType;

//			if (fieldValue != null && !fieldType.IsValueType && fieldType != _stringType)
//			{
//				var @delegate = _genericCloneInternalDelegates.GetOrAdd(fieldType, t => Reflection.Delegates.DelegateFactory.StaticGenericMethod(_reflectionCloneFactoryType, nameof(CloneInternal), [t, _dictionaryType], [t]))!;
				
//				//Throw.IfNull(@delegate, (IErrorCode?)null, fieldType.FullName);

//				var clonedValue = @delegate([fieldValue, instances]);

//				cloneWrapper.SetNonStaticValue(fieldName, clonedValue);
//			}
//			else
//			{
//				cloneWrapper.SetNonStaticValue(fieldName, fieldValue);
//			}
//		}

//		foreach (var parentType in objectType.GetBaseTypes())
//		{
//			CloneParentInternal(parentType, @object, clone!, instances);
//		}

//		return clone!;
//	}

//	//private static object? Clone(object @object, Dictionary<object, object?> instances)
//	//{
//	//	if (@object == null)
//	//		return null;

//	//	var objectType = @object.GetType();

//	//	var objectWrapper = ObjectWrapperFactory.Create(
//	//		objectType,
//	//		@object,
//	//		BindingFlags.Public |
//	//		BindingFlags.NonPublic |
//	//		BindingFlags.Instance);
		
//	//	var clone = objectWrapper.TypeWrapper.CreateNewInstanceWithoutConstructor();

//	//	var cloneWrapper = ObjectWrapperFactory.Create(
//	//		objectType,
//	//		clone,
//	//		BindingFlags.Public |
//	//		BindingFlags.NonPublic |
//	//		BindingFlags.Instance);

//	//	foreach (var field in objectWrapper.TypeWrapper.ObjectInfo.Fields.Where(x => !x.IsStatic))
//	//	{
//	//		var fieldName = field.Name;
//	//		var fieldValue = objectWrapper.GetNonStaticValue(fieldName);
//	//		var fieldType = field.FieldType;

//	//		if (fieldValue != null && !fieldType.IsValueType && fieldType != _stringType)
//	//		{
//	//			var @delegate = _genericCloneInternalDelegates.GetOrAdd(fieldType, t => Reflection.Delegates.DelegateFactory.StaticGenericMethod(_reflectionCloneFactoryType, nameof(CloneInternal), [t, _dictionaryType], [t]))!;

//	//			//Throw.IfNull(@delegate, (IErrorCode?)null, fieldType.FullName);

//	//			var clonedValue = @delegate([fieldValue, instances]);

//	//			cloneWrapper.SetNonStaticValue(fieldName, clonedValue);
//	//		}
//	//		else
//	//		{
//	//			cloneWrapper.SetNonStaticValue(fieldName, fieldValue);
//	//		}
//	//	}

//	//	foreach (var parentType in objectType.GetBaseTypes())
//	//	{
//	//		CloneParentInternal(parentType, @object, clone!, instances);
//	//	}

//	//	return clone;
//	//}

//	private static void CloneParentInternal(Type type, object @object, object clone, Dictionary<object, object?> instances)
//	{
//		if (@object == null || clone == null)
//			return;

//		var objectWrapper = ObjectWrapperFactory.Create(
//			type,
//			@object,
//			BindingFlags.Public |
//			BindingFlags.NonPublic |
//			BindingFlags.Instance);

//		var cloneWrapper = ObjectWrapperFactory.Create(
//			type,
//			clone,
//			BindingFlags.Public |
//			BindingFlags.NonPublic |
//			BindingFlags.Instance);

//		foreach (var field in objectWrapper.TypeWrapper.ObjectInfo.Fields.Where(x => !x.IsStatic))
//		{
//			var fieldName = field.Name;
//			var fieldValue = objectWrapper.GetNonStaticValue(fieldName);
//			var fieldType = field.FieldType;

//			if (fieldValue != null && !fieldType.IsValueType && fieldType != _stringType)
//			{
//				var @delegate = _genericCloneInternalDelegates.GetOrAdd(fieldType, t => Reflection.Delegates.DelegateFactory.StaticGenericMethod(_reflectionCloneFactoryType, nameof(CloneInternal), [t, _dictionaryType], [t]))!;

//				//Throw.IfNull(@delegate, (IErrorCode?)null, fieldType.FullName);

//				var clonedValue = @delegate([fieldValue, instances]);

//				cloneWrapper.SetNonStaticValue(fieldName, clonedValue);
//			}
//			else
//			{
//				cloneWrapper.SetNonStaticValue(fieldName, fieldValue);
//			}
//		}
//	}
//}
