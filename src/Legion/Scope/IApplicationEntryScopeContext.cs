namespace Legion;

#if NET6_0_OR_GREATER
[Legion.Serializer.JsonPolymorphicConverter]
#endif
public interface IApplicationEntryScopeContext : IScopeContext
{
	IApplicationEntryScopeContext Clone();
}
