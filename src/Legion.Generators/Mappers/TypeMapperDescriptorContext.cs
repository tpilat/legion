namespace Legion.Generators.Mappers;

public class TypeMapperDescriptorContext
{
	internal Dictionary<Type, TypeMapperDescriptor> TypeMapperDescriptors { get; }

	public string TargetFolder { get; }
	public string MapperBaseClassesNamespace { get; }
	public bool IsLegionFramework { get; set; }
	public string? MapperNamespace { get; set; }
	public List<Type>? TypesMappedByReference { get; set; }
	public bool LessThanCSharp12 { get; set; }
	//public bool EmbedToTargetType { get; set; }
	//public bool MapNotEmbededInternalProperties { get; set; }
	//public bool MapNotEmbededInternalFields { get; set; }
	public List<Type> TypesGeneratedByFactory { get; set; }

	public TypeMapperDescriptorContext(string targetFolder, string mapperBaseClassesNamespace)
	{
		Throw.IfArgumentNullOrWhiteSpace(targetFolder);
		Throw.IfArgumentNullOrWhiteSpace(mapperBaseClassesNamespace);

		TargetFolder = targetFolder;
		TypeMapperDescriptors = [];
		MapperBaseClassesNamespace = mapperBaseClassesNamespace;
		TypesGeneratedByFactory = [];
	}
}
