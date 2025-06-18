using Legion.Extensions;
using System.Reflection;

namespace Legion.Generators.EqualityComparers;

public class PropertyComparer
{
	public PropertyInfo PropertyInfo { get; set; }
	public string Name => PropertyInfo.Name;
	public Type PropertyType => PropertyInfo.PropertyType;
	public bool CanRead => PropertyInfo.CanRead;
	public bool CanWrite => PropertyInfo.CanWrite;

	public PropertyComparer(PropertyInfo propertyInfo)
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
