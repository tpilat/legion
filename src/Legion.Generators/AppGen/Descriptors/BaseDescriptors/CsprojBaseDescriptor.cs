using Legion.Generators.AppGen.AppGenGenerators;
using Legion.Generators.AppGen.Descriptors.MetaDescriptors;

namespace Legion.Generators.AppGen.Descriptors.BaseDescriptors;

public abstract class CsprojBaseDescriptor : TypeDescriptor
{
	public CsprojBaseDescriptor(GeneratorContext generatorContext)
		: base(generatorContext)
	{
	}
}
