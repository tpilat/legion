using Legion.Extensions;
using System.Collections;

namespace Legion.Generators.AppGen.Descriptors.MetaDescriptors;

public class BasePropertyDescriptor
{
	private readonly Lazy<Type> _stringType = new(() => typeof(string));

	public TypeDescriptor DeclaringTypeDescriptor { get; }
	public string CSharpType { get; set; }
	public Type ClrType { get; set; }
	public string Name { get; set; }
	public string FullName => $"{DeclaringTypeDescriptor.Name}.{Name}";
	public string AsPrivateFieldName => GeneratorHelper.AsPrivateFieldName(Name);
	public string AsFieldName => GeneratorHelper.AsFieldName(Name);
	public string AsParameterName => GeneratorHelper.AsFieldName(Name);

	public BasePropertyDescriptor(TypeDescriptor typeDescriptor)
	{
		DeclaringTypeDescriptor = typeDescriptor ?? throw new ArgumentNullException(nameof(typeDescriptor));
	}

	public bool IsArray()
		=> ClrType.IsArray;

	public bool IsCollection()
		=> ClrType != _stringType.Value && ClrType.IsEnumerable();
}