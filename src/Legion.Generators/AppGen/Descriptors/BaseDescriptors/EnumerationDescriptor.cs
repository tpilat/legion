using Legion.Generators.AppGen.AppGenGenerators;
using Legion.Generators.AppGen.Descriptors.MetaDescriptors;

namespace Legion.Generators.AppGen.Descriptors;

public abstract class EnumerationDescriptor<T> : TypeDescriptor<T>
{
	public EnumerationDescriptor(T data, GeneratorContext context)
		: base(data, context)
	{
	}
}
