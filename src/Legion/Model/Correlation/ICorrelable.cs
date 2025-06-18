namespace Legion.Model.Correlation;

#if NET6_0_OR_GREATER
[Legion.Serializer.JsonPolymorphicConverter]
#endif
public interface ICorrelable : IEntity
{
	Guid CorrelationId { get; set; }
}
