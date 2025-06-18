#if NET6_0_OR_GREATER

namespace Legion.Serializer;

[AttributeUsage(AttributeTargets.Interface)]
public class JsonPolymorphicSerializerAttribute : Attribute
{
}
#endif
