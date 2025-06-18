using Legion.Extensions;
using System.Reflection;

namespace Legion.Generators.EqualityComparers;

public class FieldComparer
{
	public FieldInfo FieldInfo { get; set; }
	public string Name => FieldInfo.Name;
	public Type FieldType => FieldInfo.FieldType;

	public FieldComparer(FieldInfo fieldInfo)
	{
		Throw.IfArgumentNull(fieldInfo);

		FieldInfo = fieldInfo;
	}

	public bool IsArray()
		=> FieldInfo.IsArray();

	public bool IsEnumerable()
		=> FieldInfo.IsEnumerable();

	public bool IsDictionary()
		=> FieldInfo.IsDictionary();

	public override string ToString()
		=> $"{FieldType.ToFriendlyName()} {Name}";
}
