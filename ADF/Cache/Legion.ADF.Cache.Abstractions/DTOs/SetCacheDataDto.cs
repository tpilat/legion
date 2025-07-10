namespace Legion.ADF.Cache.DTOs;

public class SetCacheDataDto
{
	public string Key { get; set; }
	public string Value { get; set; }
	public TimeSpan? SlidingTime { get; set; }
	public DateTime? KeepUntil { get; set; }
}
