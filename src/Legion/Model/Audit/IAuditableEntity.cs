namespace Legion.Model.Audit;

#if NET6_0_OR_GREATER
[Legion.Serializer.JsonPolymorphicConverter]
#endif
public interface IAuditableEntity : IEntity
{
	IReadOnlyDictionary<string, string>? GetIgnoredAuditPropertiesWithDefaultValue();
}
