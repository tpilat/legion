namespace Legion.ADF.Cache.DTOs;

public class UpdateCacheDataDto
{
	public string Key { get; set; }
	public string OldValue { get; set; }
	public string NewValue { get; set; }
	public Guid CurrentRowVersion { get; set; }
	public TimeSpan? SlidingTime { get; set; }
	public DateTime? KeepUntil { get; set; }
}
