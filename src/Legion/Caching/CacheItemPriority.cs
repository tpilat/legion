namespace Legion.Caching;

public enum CacheItemPriority
{
	/// <summary>
	/// The cache entry should be removed as soon as possible during memory pressure triggered cleanup.
	/// </summary>
	Low,

	/// <summary>
	/// The cache entry should be removed if there is no other low priority cache entries during memory pressure triggered cleanup.
	/// </summary>
	Normal,

	/// <summary>
	/// The cache entry should be removed only when there is no other low or normal priority cache entries during memory pressure triggered cleanup.
	/// </summary>
	High,

	/// <summary>
	/// The cache entry should never be removed during memory pressure triggered cleanup.
	/// </summary>
	NeverRemove,
}
