namespace Legion.Model.Synchronyzation;

#if NET6_0_OR_GREATER
[Legion.Serializer.JsonPolymorphicConverter]
#endif
public interface ISynchronizable : IEntity
{
	Guid SyncToken { get; set; }

	List<string>? GetIgnoredSynchronizationProperties();
}
