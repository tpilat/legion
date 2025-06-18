using Legion.Extensions;
using Legion.Reflection;

namespace Legion.Generators.Mappers;

public class TypeMapperDescriptor
{
	private readonly Dictionary<Type, ObjectInfo> _objectInfoCache = [];

	private TypeMapperDescriptorContext Context { get; }

	public string MapperBaseClassesNamespace => Context.MapperBaseClassesNamespace;
	public bool IsLegionFramework => Context.IsLegionFramework;
	public List<Type>? TypesMappedByReference => Context.TypesMappedByReference;
	public bool LessThanCSharp12 => Context.LessThanCSharp12;
	//public bool EmbedToTargetType => Context.EmbedToTargetType;
	//public bool MapNotEmbededInternalProperties => Context.MapNotEmbededInternalProperties;
	//public bool MapNotEmbededInternalFields => Context.MapNotEmbededInternalFields;
	public List<Type> TypesGeneratedByFactory => Context.TypesGeneratedByFactory;

	public Type TargetType { get; }
	public Type SourceType { get; }
	public string TargetTypeName { get; }
	public string SourceTypeName { get; }
	public bool TargetAndSourceTypesAreTheSame { get; }

	public string MapperTargetFolder { get; set; }
	public string MapperFileName { get; set; }
	public string MapperNamespace { get; }
	public string MapperName { get; }

	public List<PropertyMapper> ValueTypeProperties { get; }

	public Dictionary<PropertyMapper, TypeMapperDescriptor> ReferenceTypeProperties { get; }

	public Dictionary<PropertyMapper, TypeMapperDescriptor> CollectionOfReferenceTypeProperties { get; }

	public TypeMapperDescriptor(Type type, TypeMapperDescriptorContext context)
		: this(type, type, context)
	{
		TargetAndSourceTypesAreTheSame = true;
	}

	public TypeMapperDescriptor(Type sourceType, Type targetType, TypeMapperDescriptorContext context)
	{
		Throw.IfArgumentNull(sourceType);
		Throw.IfArgumentNull(targetType);
		Throw.IfArgumentNull(context);

		Context = context;

		if (targetType.IsSimpleType())
			Throw.InvalidOperationException($"Cannot create {nameof(TypeMapperDescriptor)} from target valueType");

		if (targetType.IsEnumerableType())
			Throw.InvalidOperationException($"Cannot create {nameof(TypeMapperDescriptor)} from target IEnumerable type");

		TargetType = targetType;
		SourceType = sourceType;

		TargetTypeName = TargetType.FullName!;
		SourceTypeName = sourceType.FullName!;

		MapperNamespace = Context.MapperNamespace ?? TargetType.Namespace!;
		MapperName = $"{TargetType.Name}Mapper";
		MapperFileName = $"{MapperName}.cs";
		MapperTargetFolder = Context.TargetFolder;

		Context.TypeMapperDescriptors[TargetType] = this;

		var targetObjectInfo = GetOrCreateObjectInfo(TargetType);
		var sourceObjectInfo = GetOrCreateObjectInfo(SourceType);

		ValueTypeProperties = targetObjectInfo.Properties
			.Where(p => !p.IsStatic()
				&& (p.PropertyType.IsSimpleType()
					|| p.PropertyType.IsEnumerableSimpleType()))
			.OrderBy(p => p.Name)
			.Select(p => new PropertyMapper(p))
			.ToList();

		var targetReferenceTypeProperties = targetObjectInfo.Properties
			.Where(p => !p.IsStatic()
				&& !p.PropertyType.IsSimpleType()
				&& !p.PropertyType.IsEnumerableSimpleType()
				&& !p.PropertyType.IsEnumerableType())
			.OrderBy(p => p.Name)
			.Select(p => new PropertyMapper(p))
			.ToList();

		ReferenceTypeProperties = [];
		foreach (var targetReferenceTypeProperty in targetReferenceTypeProperties)
		{
			var sourceReferenceTypeProperty = sourceObjectInfo.Properties.Where(x => x.Name == targetReferenceTypeProperty.Name && x.CanRead).Select(p => new PropertyMapper(p)).FirstOrDefault()
				?? targetReferenceTypeProperty;

			ReferenceTypeProperties.Add(
				targetReferenceTypeProperty,
				GetOrAddTypeMapperDescriptor(sourceReferenceTypeProperty.PropertyType, targetReferenceTypeProperty.PropertyType));
		}

		var targetCollectionTypeProperties = targetObjectInfo.Properties
			.Where(p => !p.IsStatic()
				&& !p.PropertyType.IsSimpleType()
				&& !p.PropertyType.IsEnumerableSimpleType()
				&& p.PropertyType.IsEnumerableType())
			.OrderBy(p => p.Name)
			.Select(p => new PropertyMapper(p))
			.ToList();

		CollectionOfReferenceTypeProperties = [];
		foreach (var targetCollectionTypeProperty in targetCollectionTypeProperties)
		{
			var sourceReferenceTypeProperty = sourceObjectInfo.Properties.Where(x => x.Name == targetCollectionTypeProperty.Name && x.CanRead).Select(p => new PropertyMapper(p)).FirstOrDefault()
				?? targetCollectionTypeProperty;

			var target = targetCollectionTypeProperty.PropertyType.GetEnumerableElementType();
			var source = sourceReferenceTypeProperty.PropertyType.IsGenericType
				? sourceReferenceTypeProperty.PropertyType.GetEnumerableElementType()
				: sourceReferenceTypeProperty.PropertyType;

			CollectionOfReferenceTypeProperties.Add(
				targetCollectionTypeProperty,
				GetOrAddTypeMapperDescriptor(source, target));
		}
	}

	private TypeMapperDescriptor GetOrAddTypeMapperDescriptor(Type sourceType, Type targetType)
	{
		if (!Context.TypeMapperDescriptors.TryGetValue(targetType, out var typeMapperDescriptor))
			typeMapperDescriptor = new TypeMapperDescriptor(sourceType, targetType, Context);

		return typeMapperDescriptor;
	}

	private ObjectInfo GetOrCreateObjectInfo(Type type)
	{
		if (!_objectInfoCache.TryGetValue(type, out ObjectInfo? objectInfo))
			objectInfo = new ObjectInfo(type);

		return objectInfo;
	}

	public bool ExistsInSource(PropertyMapper propertyMapper)
	{
		var sourceObjectInfo = GetOrCreateObjectInfo(SourceType);
		return sourceObjectInfo.Properties.Any(x => x.Name == propertyMapper.Name);
	}

	public AppGen.ModelResult Generate()
		=> GenerateInternal([]);

	private AppGen.ModelResult GenerateInternal(HashSet<TypeMapperDescriptor> alreadyGeneratedMappers)
	{
		var result = new AppGen.ModelResult();

		if (!alreadyGeneratedMappers.Add(this))
			return result;

		result = AppGen.AppGenGenerators.GeneratorInvoker
				.Generate<TypeMapperGenerator>(
					Path.Combine(MapperTargetFolder, MapperFileName),
					new Dictionary<string, object> { { nameof(TypeMapperDescriptor), this } });

		foreach (var referenceTypePropertyDescriptor in ReferenceTypeProperties.Values)
			if (IsComapredByReference(referenceTypePropertyDescriptor.TargetType) != true)
				referenceTypePropertyDescriptor.GenerateInternal(alreadyGeneratedMappers);

		foreach (var collectionOfReferenceTypePropertyDescriptor in CollectionOfReferenceTypeProperties.Values)
			if (IsComapredByReference(collectionOfReferenceTypePropertyDescriptor.TargetType) != true)
				collectionOfReferenceTypePropertyDescriptor.GenerateInternal(alreadyGeneratedMappers);

		return result;
	}

	public bool IsComapredByReference(Type type)
	{
		//Object is compared by reference
		if (typeof(object) == type)
			return true;

		//all delegates are compared by reference
		if (typeof(Delegate).IsAssignableFrom(type))
			return true;

		if (TypesMappedByReference == null || TypesMappedByReference.Count == 0)
			return false;

		foreach (var typeMappedByReference in TypesMappedByReference)
		{
			if (typeMappedByReference.IsGenericTypeDefinition)
			{
				if (type.IsGenericTypeDefinition)
				{
					if (typeMappedByReference == type)
						return true;
				}
				else
				{
					if (typeMappedByReference == type.GetGenericTypeDefinitionIfExists())
						return true;
				}
			}
			else
			{
				if (typeMappedByReference == type)
					return true;
			}
		}

		return false;
	}
}
