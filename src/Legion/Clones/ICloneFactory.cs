using System.Diagnostics.CodeAnalysis;

namespace Legion.Clones;

public interface ICloneFactory
{
	[return: NotNullIfNotNull(nameof(@object))]
	T? Clone<T>(T? @object);
}
