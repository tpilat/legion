using Legion.Extensions;
using System.Reflection;

namespace Legion.Generators.Mappers;

public class PropertyMapper
{
	public PropertyInfo PropertyInfo { get; set; }
	public string Name => PropertyInfo.Name;
	public Type PropertyType => PropertyInfo.PropertyType;
	public bool CanRead => PropertyInfo.CanRead;
	public bool CanWrite => PropertyInfo.CanWrite && PropertyInfo.HasPublicSetterWithoutInit();

	public PropertyMapper(PropertyInfo propertyInfo)
	{
		Throw.IfArgumentNull(propertyInfo);

		PropertyInfo = propertyInfo;
	}

	public bool IsArray()
		=> PropertyInfo.IsArray();

	public bool IsEnumerable()
		=> PropertyInfo.IsEnumerable();

	public bool IsDictionary()
		=> PropertyInfo.IsDictionary();

	public override string ToString()
		=> $"{PropertyType.ToFriendlyName()} {Name}";
}
