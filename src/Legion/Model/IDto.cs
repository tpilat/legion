namespace Legion.Model;

#if NET6_0_OR_GREATER
[Legion.Serializer.JsonPolymorphicConverter]
#endif
public interface IDto
{
	Guid? AuditOperation { get; }

	Dictionary<string, string> Acl { get; }
	
	Guid? ConcurrencyToken { get; }
}
