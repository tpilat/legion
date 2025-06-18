using Legion.Extensions;
using Legion.Generators.AppGen.AppGenGenerators;
using Legion.Generators.AppGen.Descriptors.MetaDescriptors;
using Legion.Generators.AppGen.Model;

namespace Legion.Generators.AppGen.Descriptors;

public abstract class QueryEntityBaseDescriptor : TypeDescriptor<QueryEntityBase>
{
	public string AsCommandPrefix => $"{string.Join("_", GetPackagesStructure.Select(p => p.Name))}_{GetDataName}";

	public string GetDataName
		=> Data.Name;

	public string GetPackageNamespacePart
		=> Data.Package.NamespacePart;

	public string GetPackagePathPart
		=> Data.Package.PathPart;

	public List<PackageBase> GetPackagesStructure
		=> Data.Package.GetPackagesStructure();

	public List<PropertyDescriptor<QueryEntityBase, QueryPropertyBase>> Properties { get; }

	protected QueryEntityBaseDescriptor(QueryEntityBase data, GeneratorContext context)
		: base(data, context)
	{
		Properties = new List<PropertyDescriptor<QueryEntityBase, QueryPropertyBase>>();

		foreach (var property in data.Properties)
			AddProperty(property, p => {
				p.CSharpType = property.CSharpType;
				p.ClrType = property.ClrType;
				p.Name = property.Name;
			});
	}

	public QueryEntityBaseDescriptor AddProperty(PropertyDescriptor<QueryEntityBase, QueryPropertyBase> property)
	{
		if (property == null)
			throw new ArgumentNullException(nameof(property));

		Properties.Add(property);
		return this;
	}

	public QueryEntityBaseDescriptor AddProperty(QueryPropertyBase data, Action<PropertyDescriptor<QueryEntityBase, QueryPropertyBase>> configurator)
	{
		if (data == null)
			throw new ArgumentNullException(nameof(data));
		if (configurator == null)
			throw new ArgumentNullException(nameof(configurator));

		var property = new PropertyDescriptor<QueryEntityBase, QueryPropertyBase>(this, data);
		configurator.Invoke(property);
		Properties.Add(property);
		return this;
	}

	public string PropertyIsNotSet_RightSide(QueryPropertyBase property)
	{
		if (property == null)
			throw new ArgumentNullException(nameof(property));

		var type = property.UnderlyingNullableType;
		if (type == typeof(int) || type == typeof(long))
			return "<= 0";

		if (type == typeof(DateTime))
			return $"== {nameof(System)}.{nameof(DateTime)}.{nameof(DateTime.MinValue)}";

		if (type == typeof(Guid))
			return $"== {nameof(System)}.{nameof(Guid)}.{nameof(Guid.Empty)}";

		if (type == typeof(string))
			return "== string.Empty";

		throw new NotSupportedException(type.FullName);
	}

	public string PropertyToString(QueryPropertyBase property)
	{
		var result = property.Name;

		if (property.ClrType != typeof(string))
		{
			if (Nullable.GetUnderlyingType(property.ClrType) != null)
			{
				result += "?";
			}

			if (property.ClrType.GetUnderlyingNullableType() == typeof(DateTime))
			{
				result += ".ToString(\"dd.MM.yyyy HH:mm:ss\", System.Globalization.CultureInfo.InvariantCulture)";
			}
			else
			{
				result += ".ToString()";
			}
		}

		return result;
	}
}
