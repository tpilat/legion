namespace Legion.ADF.Cache;

public static class CacheItemPriorityExtensions
{
	public static Microsoft.Extensions.Caching.Memory.CacheItemPriority Convert(this Legion.Caching.CacheItemPriority priority)
		=> priority switch
		{
			Caching.CacheItemPriority.Low => Microsoft.Extensions.Caching.Memory.CacheItemPriority.Low,
			Caching.CacheItemPriority.Normal => Microsoft.Extensions.Caching.Memory.CacheItemPriority.Normal,
			Caching.CacheItemPriority.High => Microsoft.Extensions.Caching.Memory.CacheItemPriority.High,
			Caching.CacheItemPriority.NeverRemove => Microsoft.Extensions.Caching.Memory.CacheItemPriority.NeverRemove,
			_ => Microsoft.Extensions.Caching.Memory.CacheItemPriority.NeverRemove,
		};
}
