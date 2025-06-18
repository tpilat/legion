namespace Legion;

public record QueryResult<T>(
	T Data,
	long TotalCount = -1);
