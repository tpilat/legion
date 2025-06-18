using Legion.Extensions;
using Legion.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Collections.Concurrent;

namespace Legion.Serializer.JsonConverters;

public class PrivateSetterContractResolver : DefaultContractResolver
{
	private readonly ConcurrentDictionary<Type, ObjectInfo> _typeObjectInfoDict;

	public PrivateSetterContractResolver()
	{
		_typeObjectInfoDict = [];
	}

	protected override JsonProperty CreateProperty(System.Reflection.MemberInfo member, MemberSerialization memberSerialization)
	{
		var property = base.CreateProperty(member, memberSerialization);

		if (property.PropertyType != null)
		{
			var propertyInfo = member as System.Reflection.PropertyInfo;
			if (propertyInfo != null)
			{
				var objectInfo = _typeObjectInfoDict.AddOrGet(propertyInfo.DeclaringType!, type => new ObjectInfo(type!));
				var hasSetter = objectInfo.GetPropertySetter(property.PropertyName!) != null;

				if (hasSetter)
					property.Writable = true;
			}
		}

		return property;
	}
}
