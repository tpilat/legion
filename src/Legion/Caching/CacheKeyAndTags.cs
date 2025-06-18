namespace Legion.Caching;

public record CacheKeyAndTags(
	string Key,
	List<string> Tags);
