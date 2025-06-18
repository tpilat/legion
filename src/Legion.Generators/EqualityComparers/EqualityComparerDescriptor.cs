using Legion.Extensions;
using Legion.Reflection;

namespace Legion.Generators.EqualityComparers;

public class EqualityComparerDescriptor
{
	private readonly Dictionary<Type, ObjectInfo> _objectInfoCache = [];

	private EqualityComparerDescriptorContext Context { get; }

	public string ComparerBaseClassesNamespace => Context.ComparerBaseClassesNamespace;
	public bool IsLegionFramework => Context.IsLegionFramework;
	public List<Type>? TypesComapredByReference => Context.TypesComapredByReference;
	public bool LessThanCSharp12 => Context.LessThanCSharp12;
	public bool EmbedToTargetType => Context.EmbedToTargetType;
	public bool CompareNotEmbededInternalProperties => Context.CompareNotEmbededInternalProperties;
	public bool CompareNotEmbededInternalFields => Context.CompareNotEmbededInternalFields;

	public Type TargetType { get; }
	public string TargetTypeSimpleName { get; }
	public string TargetTypeName { get; }
	public string TargetTypeNamespace { get; }

	public string ComparerTargetFolder { get; set; }
	public string ComparerFileName { get; set; }
	public string ComparerNamespace { get; }
	public string ComparerName { get; }

	public List<PropertyComparer> ValueTypeProperties { get; }

	public Dictionary<PropertyComparer, EqualityComparerDescriptor> ReferenceTypeProperties { get; }

	public Dictionary<PropertyComparer, EqualityComparerDescriptor> CollectionOfReferenceTypeProperties { get; }

	public List<FieldComparer> ValueTypeFields { get; }

	public Dictionary<FieldComparer, EqualityComparerDescriptor> ReferenceTypeFields { get; }

	public Dictionary<FieldComparer, EqualityComparerDescriptor> CollectionOfReferenceTypeFields { get; }

	public EqualityComparerDescriptor(Type targetType, EqualityComparerDescriptorContext context)
	{
		Throw.IfArgumentNull(targetType);
		Throw.IfArgumentNull(context);

		Context = context;

		if (targetType.IsSimpleType())
			Throw.InvalidOperationException($"Cannot create {nameof(EqualityComparerDescriptor)} from target valueType");

		if (targetType.IsEnumerableType())
			Throw.InvalidOperationException($"Cannot create {nameof(EqualityComparerDescriptor)} from target IEnumerable type");

		TargetType = targetType;

		TargetTypeSimpleName = TargetType.Name;
		TargetTypeName = TargetType.FullName!;
		TargetTypeNamespace = TargetType.Namespace!;

		ComparerNamespace = Context.ComparerNamespace ?? TargetType.Namespace!;
		ComparerName = $"{TargetTypeSimpleName}EqualityComparer";
		ComparerFileName = $"{ComparerName}.cs";
		ComparerTargetFolder = Context.TargetFolder;

		Context.EqualityComparerDescriptors[TargetType] = this;

		ReferenceTypeProperties = [];
		CollectionOfReferenceTypeProperties = [];
		ReferenceTypeFields = [];
		CollectionOfReferenceTypeFields = [];
		if (IsComapredByReference(TargetType) == true)
		{
			ValueTypeProperties = [];
			ValueTypeFields = [];
			return;
		}

		var targetObjectInfo = GetOrCreateObjectInfo(TargetType);

		ValueTypeProperties = targetObjectInfo.Properties
			.Where(p => !p.IsStatic()
				&& (Context.EmbedToTargetType || p.IsPublic() || (Context.CompareNotEmbededInternalProperties && p.IsInternal()))
				&& (p.PropertyType.IsSimpleType()
					|| p.PropertyType.IsEnumerableSimpleType()))
			.OrderBy(p => p.Name)
			.Select(p => new PropertyComparer(p))
			.ToList();

		var targetReferenceTypeProperties = targetObjectInfo.Properties
			.Where(p => !p.IsStatic()
				&& (Context.EmbedToTargetType || p.IsPublic() || (Context.CompareNotEmbededInternalProperties && p.IsInternal()))
				&& !p.PropertyType.IsSimpleType()
				&& !p.PropertyType.IsEnumerableSimpleType()
				&& !p.PropertyType.IsEnumerableType())
			.OrderBy(p => p.Name)
			.Select(p => new PropertyComparer(p))
			.ToList();

		foreach (var targetReferenceTypeProperty in targetReferenceTypeProperties)
		{
			ReferenceTypeProperties.Add(
				targetReferenceTypeProperty,
				GetOrAddEqualityComparerDescriptor(targetReferenceTypeProperty.PropertyType));
		}

		var targetCollectionTypeProperties = targetObjectInfo.Properties
			.Where(p => !p.IsStatic()
				&& (Context.EmbedToTargetType || p.IsPublic() || (Context.CompareNotEmbededInternalProperties && p.IsInternal()))
				&& !p.PropertyType.IsSimpleType()
				&& !p.PropertyType.IsEnumerableSimpleType()
				&& p.PropertyType.IsEnumerableType())
			.OrderBy(p => p.Name)
			.Select(p => new PropertyComparer(p))
			.ToList();

		foreach (var targetCollectionTypeProperty in targetCollectionTypeProperties)
		{
			var target = targetCollectionTypeProperty.PropertyType.GetEnumerableElementType();

			CollectionOfReferenceTypeProperties.Add(
				targetCollectionTypeProperty,
				GetOrAddEqualityComparerDescriptor(target));
		}

		ValueTypeFields = targetObjectInfo.Fields
			.Where(p => !p.IsStatic
				&& (Context.EmbedToTargetType || p.IsPublic || (Context.CompareNotEmbededInternalProperties && p.IsAssembly))
				&& !p.IsBackingField()
				&& (p.FieldType.IsSimpleType()
					|| p.FieldType.IsEnumerableSimpleType()))
			.OrderBy(p => p.Name)
			.Select(p => new FieldComparer(p))
			.ToList();

		var targetReferenceTypeFields = targetObjectInfo.Fields
			.Where(p => !p.IsStatic
				&& (Context.EmbedToTargetType || p.IsPublic || (Context.CompareNotEmbededInternalProperties && p.IsAssembly))
				&& !p.IsBackingField()
				&& !p.FieldType.IsSimpleType()
				&& !p.FieldType.IsEnumerableSimpleType()
				&& !p.FieldType.IsEnumerableType())
			.OrderBy(p => p.Name)
			.Select(p => new FieldComparer(p))
			.ToList();

		foreach (var targetReferenceTypeField in targetReferenceTypeFields)
		{
			ReferenceTypeFields.Add(
				targetReferenceTypeField,
				GetOrAddEqualityComparerDescriptor(targetReferenceTypeField.FieldType));
		}

		var targetCollectionTypeFields = targetObjectInfo.Fields
			.Where(p => !p.IsStatic
				&& (Context.EmbedToTargetType || p.IsPublic || (Context.CompareNotEmbededInternalProperties && p.IsAssembly))
				&& !p.IsBackingField()
				&& !p.FieldType.IsSimpleType()
				&& !p.FieldType.IsEnumerableSimpleType()
				&& p.FieldType.IsEnumerableType())
			.OrderBy(p => p.Name)
			.Select(p => new FieldComparer(p))
			.ToList();

		foreach (var targetCollectionTypeField in targetCollectionTypeFields)
		{
			var target = targetCollectionTypeField.FieldType.GetEnumerableElementType();

			CollectionOfReferenceTypeFields.Add(
				targetCollectionTypeField,
				GetOrAddEqualityComparerDescriptor(target));
		}
	}

	private EqualityComparerDescriptor GetOrAddEqualityComparerDescriptor(Type targetType)
	{
		if (!Context.EqualityComparerDescriptors.TryGetValue(targetType, out var equalityComparerDescriptor))
			equalityComparerDescriptor = new EqualityComparerDescriptor(targetType, Context);

		return equalityComparerDescriptor;
	}

	private ObjectInfo GetOrCreateObjectInfo(Type type)
	{
		if (!_objectInfoCache.TryGetValue(type, out ObjectInfo? objectInfo))
			objectInfo = new ObjectInfo(type);

		return objectInfo;
	}

	public AppGen.ModelResult Generate()
		=> GenerateInternal([]);

	private AppGen.ModelResult GenerateInternal(HashSet<EqualityComparerDescriptor> alreadyGeneratedComparers)
	{
		var result = new AppGen.ModelResult();

		if (!alreadyGeneratedComparers.Add(this))
			return result;

		result = AppGen.AppGenGenerators.GeneratorInvoker
				.Generate<EqualityComparerGenerator>(
					Path.Combine(ComparerTargetFolder, ComparerFileName),
					new Dictionary<string, object> { { nameof(EqualityComparerDescriptor), this } });

		foreach (var referenceTypePropertyDescriptor in ReferenceTypeProperties.Values)
			if (IsComapredByReference(referenceTypePropertyDescriptor.TargetType) != true)
				referenceTypePropertyDescriptor.GenerateInternal(alreadyGeneratedComparers);

		foreach (var collectionOfReferenceTypePropertyDescriptor in CollectionOfReferenceTypeProperties.Values)
			if (IsComapredByReference(collectionOfReferenceTypePropertyDescriptor.TargetType) != true)
				collectionOfReferenceTypePropertyDescriptor.GenerateInternal(alreadyGeneratedComparers);

		foreach (var referenceTypeFieldDescriptor in ReferenceTypeFields.Values)
			if (IsComapredByReference(referenceTypeFieldDescriptor.TargetType) != true)
				referenceTypeFieldDescriptor.GenerateInternal(alreadyGeneratedComparers);

		foreach (var collectionOfReferenceTypeFieldDescriptor in CollectionOfReferenceTypeFields.Values)
			if (IsComapredByReference(collectionOfReferenceTypeFieldDescriptor.TargetType) != true)
				collectionOfReferenceTypeFieldDescriptor.GenerateInternal(alreadyGeneratedComparers);

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

		if (TypesComapredByReference == null || TypesComapredByReference.Count == 0)
			return false;

		foreach (var typeComapredByReference in TypesComapredByReference)
		{
			if (typeComapredByReference.IsGenericTypeDefinition)
			{
				if (type.IsGenericTypeDefinition)
				{
					if (typeComapredByReference == type)
						return true;
				}
				else
				{
					if (typeComapredByReference == type.GetGenericTypeDefinitionIfExists())
						return true;
				}
			}
			else
			{
				if (typeComapredByReference == type)
					return true;
			}
		}

		return false;
	}
}
