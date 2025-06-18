namespace Legion.Policy;

#if NET6_0_OR_GREATER
[Legion.Serializer.JsonPolymorphicConverter]
#endif
public interface IRetryTable
{
	IReadOnlyDictionary<int, TimeSpan> IterationRetryTable { get; } //Dictionary<IterationCount, TimeSpan>

	TimeSpan GetRetryTimeSpan(int currentRetryCount);
}
