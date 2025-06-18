namespace Legion.Generators.EqualityComparers;

public class EqualityComparerDescriptorContext
{
	internal Dictionary<Type, EqualityComparerDescriptor> EqualityComparerDescriptors { get; }

	public string TargetFolder { get; }
	public string ComparerBaseClassesNamespace { get; }
	public bool IsLegionFramework { get; set; }
	public string? ComparerNamespace { get; set; }
	public List<Type>? TypesComapredByReference { get; set; }
	public bool LessThanCSharp12 { get; set; }
	public bool EmbedToTargetType { get; set; }
	public bool CompareNotEmbededInternalProperties { get; set; }
	public bool CompareNotEmbededInternalFields { get; set; }

	public EqualityComparerDescriptorContext(string targetFolder, string comparerBaseClassesNamespace)
	{
		Throw.IfArgumentNullOrWhiteSpace(targetFolder);
		Throw.IfArgumentNullOrWhiteSpace(comparerBaseClassesNamespace);

		TargetFolder = targetFolder;
		EqualityComparerDescriptors = [];
		ComparerBaseClassesNamespace = comparerBaseClassesNamespace;
	}
}
