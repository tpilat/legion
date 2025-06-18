namespace Legion.Model.Concurrence;

#if NET6_0_OR_GREATER
[Legion.Serializer.JsonPolymorphicConverter]
#endif
public interface IConcurrent : IEntity
{
	[System.ComponentModel.DataAnnotations.Schema.NotMapped]
	string ConcurrencyTokenPropertyName { get; }

	void SetNewConcurrencyToken();
}
