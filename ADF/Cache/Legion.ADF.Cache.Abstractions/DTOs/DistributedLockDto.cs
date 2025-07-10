namespace Legion.ADF.Cache.DTOs;

public class DistributedLockDto
{
	public string Key { get; set; }
	public string LockId { get; set; }
	public TimeSpan LockTimeout { get; set; }
}
