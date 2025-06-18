using Legion.Extensions;
using System.Collections.Concurrent;
using System.Reflection;

namespace Legion.Reflection;

public class ObjectInfo
{
	private static readonly Type _delegateType = typeof(Delegate);

	private readonly Type _type;
	private readonly Lazy<PropertyInfo[]> _properties;
	private readonly Lazy<FieldInfo[]> _fields;
	private readonly ConcurrentDictionary<string, MethodInfo?> _getters;
	private readonly ConcurrentDictionary<string, MethodInfo?> _setters;

	public Type Type => _type;
	public IEnumerable<PropertyInfo> Properties => _properties.Value;
	public IEnumerable<FieldInfo> Fields => _fields.Value;

	public ObjectInfo(Type type)
		: this(type, new ObjectInfoOptions())
	{
	}

	public ObjectInfo(Type type, ObjectInfoOptions? options)
	{
		Throw.IfArgumentNull(type);

		_type = type;
		_properties = new(() =>
		{
			var o = options ?? new ObjectInfoOptions();
			var result = _type.GetProperties(o.BindingFlags);

			if (!o.ReadEvents && !o.ReadIndexers)
				return result;

			return result
				.Where(p => (o.ReadIndexers || !(0 < p.GetIndexParameters()?.Length))
					&& (o.ReadEvents || !_delegateType.IsAssignableFrom(p.PropertyType))).ToArray();
		});
		_fields = new(() =>
		{
			var o = options ?? new ObjectInfoOptions();
			var result = _type.GetFields(o.BindingFlags);

			if (!o.ReadEvents)
				return result;

			return result
				.Where(p => o.ReadEvents || !_delegateType.IsAssignableFrom(p.FieldType)).ToArray();
		});
		_getters = [];
		_setters = [];
	}

	public MethodInfo? GetPropertyGetter(string propertyName)
	{
		if (propertyName == null)
			return null;

		var getter = _getters.AddOrGet(propertyName, prop => _properties.Value.FirstOrDefault(p => p.Name == propertyName)?.GetGetMethod(true));
		return getter;
	}

	public MethodInfo? GetPropertySetter(string propertyName)
	{
		if (propertyName == null)
			return null;

		var setter = _setters.AddOrGet(propertyName, prop => _properties.Value.FirstOrDefault(p => p.Name == propertyName)?.GetSetMethod(true));
		return setter;
	}
}

public class ObjectInfo<T> : ObjectInfo
	where T : class
{
	public ObjectInfo()
		: base(typeof(T))
	{
	}

	public ObjectInfo(ObjectInfoOptions options)
		: base(typeof(T), options)
	{
	}
}
