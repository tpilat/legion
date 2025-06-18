namespace Legion.Generators.AppGen.Descriptors.MetaDescriptors;

public class MethodArgumentDescriptor
{
	public MethodDescriptor DeclaringMethodDescriptor { get; }
	public string CSharpTypeNamespace { get; set; }
	public string CSharpType { get; set; }
	public string DefaultValue { get; set; }
	public Type Type { get; set; }
	public string Name { get; set; }
	public string FullName => $"{DeclaringMethodDescriptor.Name}.{Name}";
	public string FullStaticName => $"{DeclaringMethodDescriptor.FullName}.{Name}";

	public MethodArgumentDescriptor(MethodDescriptor methodDescriptor)
	{
		DeclaringMethodDescriptor = methodDescriptor ?? throw new ArgumentNullException(nameof(methodDescriptor));
	}

	public string ToDefinition()
		=> $"{CSharpType} {Name}{(string.IsNullOrWhiteSpace(DefaultValue) ? "" : $" = {DefaultValue}")}";
}
