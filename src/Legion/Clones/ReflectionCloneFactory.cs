using Legion.Extensions;
using Legion.Reflection;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace Legion.Clones;

public class ReflectionCloneFactory: ICloneFactory
{
	internal static readonly Type _delegateType = typeof(Delegate);
	private static readonly Type _stringType = typeof(string);
	private static readonly BindingFlags _bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

	[return: NotNullIfNotNull(nameof(@object))]
	public T? Clone<T>(T? @object)
		=> (T)Clone((object)@object!)!;

	[return: NotNullIfNotNull(nameof(@object))]
	public object? Clone(object? @object)
		=> CloneInternal(@object, []);

	private static object? CloneInternal(object? @object, Dictionary<object, object?> instances)
	{
		if (@object == null)
			return @object;

		if (instances!.TryGetValue(@object, out object? clone))
			return clone;

		var objectType = @object.GetType();
		if (objectType.IsValueType || objectType == _stringType)
			return @object;

		if (@object is Array objectArray)
		{
			var cloneArrayTypeWrapper = ArrayTypeWrapper.Create(objectArray);
			clone = cloneArrayTypeWrapper.CreateNewArrayInstance(objectArray.Length);
			var cloneArrayWrapper = new ArrayWrapper(cloneArrayTypeWrapper, clone);

			var i = 0;
			foreach (var objectItem in objectArray)
			{
				var clonedItem = CloneInternal(objectItem, instances);
				cloneArrayWrapper.SetValue(i, clonedItem!);
				i++;
			}

			return clone!;
		}

		var objectWrapper = ObjectWrapperFactory.Create(
			objectType,
			@object,
			_bindingFlags);

		clone = objectWrapper.TypeWrapper.CreateNewInstanceWithoutConstructor();
		instances[@object] = clone;

		var cloneWrapper = ObjectWrapperFactory.Create(
			objectType,
			clone,
			_bindingFlags);

		foreach (var field in objectWrapper.TypeWrapper.ObjectInfo.Fields.Where(x => !x.IsStatic))
		{
			if (_delegateType.IsAssignableFrom(field.FieldType))
				continue;

			var fieldName = field.Name;
			var fieldValue = objectWrapper.GetNonStaticValue(fieldName);
			var fieldType = field.FieldType;

			if (fieldValue != null && !fieldType.IsValueType && fieldType != _stringType)
			{
				var clonedValue = CloneInternal(fieldValue, instances);
				cloneWrapper.SetNonStaticValue(fieldName, clonedValue);
			}
			else
			{
				cloneWrapper.SetNonStaticValue(fieldName, fieldValue);
			}
		}

		foreach (var parentType in objectType.GetBaseTypes())
		{
			CloneParentInternal(parentType, @object, clone!, instances);
		}

		return clone!;
	}

	private static void CloneParentInternal(Type type, object @object, object clone, Dictionary<object, object?> instances)
	{
		if (@object == null || clone == null)
			return;

		var objectWrapper = ObjectWrapperFactory.Create(
			type,
			@object,
			_bindingFlags);

		var cloneWrapper = ObjectWrapperFactory.Create(
			type,
			clone,
			_bindingFlags);

		foreach (var field in objectWrapper.TypeWrapper.ObjectInfo.Fields.Where(x => !x.IsStatic))
		{
			if (_delegateType.IsAssignableFrom(field.FieldType))
				continue;

			var fieldName = field.Name;
			var fieldValue = objectWrapper.GetNonStaticValue(fieldName);
			var fieldType = field.FieldType;

			if (fieldValue != null && !fieldType.IsValueType && fieldType != _stringType)
			{
				var clonedValue = CloneInternal(fieldValue, instances);
				cloneWrapper.SetNonStaticValue(fieldName, clonedValue);
			}
			else
			{
				cloneWrapper.SetNonStaticValue(fieldName, fieldValue);
			}
		}
	}
}
