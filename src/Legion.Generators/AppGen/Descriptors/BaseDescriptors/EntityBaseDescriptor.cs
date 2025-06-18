using Legion.Extensions;
using Legion.Generators.AppGen.AppGenGenerators;
using Legion.Generators.AppGen.Descriptors.MetaDescriptors;
using Legion.Generators.AppGen.Model;

namespace Legion.Generators.AppGen.Descriptors;

public abstract class EntityBaseDescriptor : TypeDescriptor<EntityBase>
{
	public string AsCommandPrefix => Data.AsCommandPrefix;

	public List<PropertyDescriptor<EntityBase, PropertyBase>> Properties { get; }
	public List<PropertyDescriptor<EntityBase, NavigationBase>> Navigations { get; }
	public List<PropertyDescriptor<EntityBase, BackNavigationBase>> BackNavigations { get; }

	protected EntityBaseDescriptor(EntityBase data, GeneratorContext context)
		: base(data, context)
	{
		Properties = new List<PropertyDescriptor<EntityBase, PropertyBase>>();
		Navigations = new List<PropertyDescriptor<EntityBase, NavigationBase>>();
		BackNavigations = new List<PropertyDescriptor<EntityBase, BackNavigationBase>>();

		foreach (var property in Data.Properties)
			AddProperty(property, p => {
				p.CSharpType = property.CSharpType;
				p.ClrType = property.ClrType;
				p.Name = property.Name;
			});

		foreach (var navigation in Data.Navigations)
			AddNavigation(navigation, p => {
				p.CSharpType = navigation.CSharpType;
				p.ClrType = navigation.ClrType;
				p.Name = navigation.Name;
			});

		foreach (var backNavigation in Data.BackNavigations)
			AddBackNavigation(backNavigation, p => {
				p.CSharpType = backNavigation.CSharpType;
				p.ClrType = backNavigation.ClrType;
				p.Name = backNavigation.Name;
			});
	}

	public EntityBaseDescriptor AddProperty(PropertyDescriptor<EntityBase, PropertyBase> property)
	{
		if (property == null)
			throw new ArgumentNullException(nameof(property));

		Properties.Add(property);
		return this;
	}

	public EntityBaseDescriptor AddProperty(PropertyBase data, Action<PropertyDescriptor<EntityBase, PropertyBase>> configurator)
	{
		if (data == null)
			throw new ArgumentNullException(nameof(data));
		if (configurator == null)
			throw new ArgumentNullException(nameof(configurator));

		var property = new PropertyDescriptor<EntityBase, PropertyBase>(this, data);
		configurator.Invoke(property);
		Properties.Add(property);
		return this;
	}

	public EntityBaseDescriptor AddNavigation(PropertyDescriptor<EntityBase, NavigationBase> navigation)
	{
		if (navigation == null)
			throw new ArgumentNullException(nameof(navigation));

		Navigations.Add(navigation);
		return this;
	}

	public EntityBaseDescriptor AddNavigation(NavigationBase data, Action<PropertyDescriptor<EntityBase, NavigationBase>> configurator)
	{
		if (data == null)
			throw new ArgumentNullException(nameof(data));
		if (configurator == null)
			throw new ArgumentNullException(nameof(configurator));

		var navigation = new PropertyDescriptor<EntityBase, NavigationBase>(this, data);
		configurator.Invoke(navigation);
		Navigations.Add(navigation);
		return this;
	}

	public EntityBaseDescriptor AddBackNavigation(PropertyDescriptor<EntityBase, BackNavigationBase> backNavigation)
	{
		if (backNavigation == null)
			throw new ArgumentNullException(nameof(backNavigation));

		BackNavigations.Add(backNavigation);
		return this;
	}

	public EntityBaseDescriptor AddBackNavigation(BackNavigationBase data, Action<PropertyDescriptor<EntityBase, BackNavigationBase>> configurator)
	{
		if (data == null)
			throw new ArgumentNullException(nameof(data));
		if (configurator == null)
			throw new ArgumentNullException(nameof(configurator));

		var backNavigation = new PropertyDescriptor<EntityBase, BackNavigationBase>(this, data);
		configurator.Invoke(backNavigation);
		BackNavigations.Add(backNavigation);
		return this;
	}

	public string GetPropertiesAsMethodParameters()
	{
		if (Properties.Count <= 0)
			return "";

		return Properties.Count == 1
			? $"{Properties[0].Data.CSharpType} {GeneratorHelper.AsFieldName(Properties[0].Name)}"
			: $"{string.Join(", ", Properties.Select(p => $"{p.Data.CSharpType} {GeneratorHelper.AsFieldName(p.Name)}"))}";
	}

	public string GetKeyPropertiesAsLambda(KeyBase key, string lambdaIdentifier)
	{
		if (key.Properties.Count <= 0)
			return "";

		return key.Properties.Count == 1
			? $"{lambdaIdentifier}.{key.Properties[0].Name}"
			: $"new {{ {string.Join(", ", key.Properties.Select(p => lambdaIdentifier + "." + p.Name))} }}";
	}

	public string GetKeyPropertiesAsMethodParameters(KeyBase key)
	{
		if (key.Properties.Count <= 0)
			return "";

		return key.Properties.Count == 1
			? $"{key.Properties[0].CSharpType} {GeneratorHelper.AsFieldName(key.Properties[0].Name)}"
			: $"{string.Join(", ", key.Properties.Select(p => $"{p.CSharpType} {GeneratorHelper.AsFieldName(p.Name)}"))}";
	}

	public string GetKeyPropertiesAsJsonFormatter(KeyBase key)
	{
		if (key.Properties.Count <= 0)
			return "";

		var pks = new List<string>();
		for (int i = 0; i < key.Properties.Count; i++)
		{
			var prop = key.Properties[i];
			if (prop.ClrType == typeof(int)
				|| prop.ClrType == typeof(uint)
				|| prop.ClrType == typeof(long)
				|| prop.ClrType == typeof(ulong)
				|| prop.ClrType == typeof(decimal)
				|| prop.ClrType == typeof(float)
				|| prop.ClrType == typeof(double)
				|| prop.ClrType == typeof(bool)
				|| prop.ClrType == typeof(byte)
				|| prop.ClrType == typeof(sbyte)
				|| prop.ClrType == typeof(short)
				|| prop.ClrType == typeof(ushort))
			{
				pks.Add($"\\\"{prop.Name}\\\":{{{i}}}");
			}
			else
			{
				pks.Add($"\\\"{prop.Name}\\\":\\\"{{{i}}}\\\"");
			}
		}

		return $"{{{{{string.Join(",", pks).Trim()}}}}}";
	}

	public string GetKeyPropertiesAsWhereCondition(KeyBase key, string lambdaIdentifier)
	{
		if (key.Properties.Count <= 0)
			return "";

		return key.Properties.Count == 1
			? $"{lambdaIdentifier}.{key.Properties[0].Name} == {GeneratorHelper.AsFieldName(key.Properties[0].Name)}"
			: $"{string.Join(" && ", key.Properties.Select(p => $"{lambdaIdentifier}.{p.Name} == {GeneratorHelper.AsFieldName(p.Name)}"))}";
	}

	public string GetKeyPropertiesAsWhereCondition(KeyBase key, string lambdaIdentifier, string objName)
	{
		if (key.Properties.Count <= 0)
			return "";

		return key.Properties.Count == 1
			? $"{lambdaIdentifier}.{key.Properties[0].Name} == {objName}.{key.Properties[0].Name}"
			: $"{string.Join(" && ", key.Properties.Select(p => $"{lambdaIdentifier}.{p.Name} == {objName}.{p.Name}"))}";
	}

	public string GetIndexPropertiesAsLambda(IndexBase index, string lambdaIdentifier)
	{
		if (index.Properties.Count <= 0)
			return "";

		return index.Properties.Count == 1
			? $"{lambdaIdentifier}.{index.Properties[0].Name}"
			: $"new {{ {string.Join(", ", index.Properties.Select(p => lambdaIdentifier + "." + p.Name))} }}";
	}

	public string GetForeignKeyPropertiesAsLambda(ForeignKeyBase foreignKey, string lambdaIdentifier)
	{
		if (foreignKey.Properties.Count <= 0)
			return "";

		return foreignKey.Properties.Count == 1
			? $"{lambdaIdentifier}.{foreignKey.Properties[0].Name}"
			: $"new {{ {string.Join(", ", foreignKey.Properties.Select(p => lambdaIdentifier + "." + p.Name))} }}";
	}

	public string GetForeignKeyPrincipalKeyPropertiesAsLambda(ForeignKeyBase foreignKey, string lambdaIdentifier)
	{
		if (foreignKey.Properties.Count <= 0)
			return "";

		return foreignKey.Properties.Count == 1
			? $"{lambdaIdentifier}.{foreignKey.Properties[0].Name}"
			: $"new {{ {string.Join(", ", foreignKey.Properties.Select(p => lambdaIdentifier + "." + p.Name))} }}";
	}

	public string NavigationToFormViewModelSourceName(NavigationBase navigation)
		=> $"{navigation}Source";

	public string PropertyIsSet_LeftSide(PropertyBase property)
	{
		if (property == null)
			throw new ArgumentNullException(nameof(property));

		var type = property.UnderlyingNullableType;
		if (type == typeof(int) || type == typeof(long))
			return "0 <";

		if (type == typeof(DateTime))
			return $"{nameof(System)}.{nameof(DateTime)}.{nameof(DateTime.MinValue)} <";

		if (type == typeof(Guid))
			return $"{nameof(System)}.{nameof(Guid)}.{nameof(Guid.Empty)} !=";

		if (type == typeof(string))
			return "string.Empty !=";

		throw new NotSupportedException(type.FullName);
	}

	public string PropertyIsNotSet_RightSide(PropertyBase property)
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

	public string PropertyToString(PropertyBase property)
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
