namespace Legion.ADF.Cache.DTOs;

public class AcquireDistributedLockDto
{
	public string Key { get; set; }
	public TimeSpan LockTimeout { get; set; }
	public string? Metadata { get; set; }
	public TimeSpan? RetryDelay { get; set; }
	public int? MaxRetries { get; set; }
}
