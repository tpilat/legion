namespace Legion.Caching;

public enum CacheCloneOption
{
	None = 0,
	CloneBeforeGet = 1,
	CloneBeforeSet = 2,
	CloneBeforeSetAndGet = CloneBeforeGet | CloneBeforeSet
}

public static class CacheCloneOptionExtensions
{
	public static bool CloneGet(this CacheCloneOption cacheCloneOption)
		=> (cacheCloneOption & CacheCloneOption.CloneBeforeGet) == CacheCloneOption.CloneBeforeGet;

	public static bool CloneSet(this CacheCloneOption cacheCloneOption)
		=> (cacheCloneOption & CacheCloneOption.CloneBeforeSet) == CacheCloneOption.CloneBeforeSet;
}
