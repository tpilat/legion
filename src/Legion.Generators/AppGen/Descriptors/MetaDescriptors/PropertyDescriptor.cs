namespace Legion.Generators.AppGen.Descriptors.MetaDescriptors;

public class PropertyDescriptor<T, P> : BasePropertyDescriptor
{
	public P Data { get; set; }

	public PropertyDescriptor(TypeDescriptor<T> typeDescriptor, P data)
		: base(typeDescriptor)
	{
		Data = data;
	}
}
