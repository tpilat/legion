namespace Legion.ADF.ServiceBus.DTOs.Hosts;

public class HostSummaryDto
{
	public string Name { get; private set; }
	public string Description { get; private set; }
	public bool IsAvailable { get; private set; }
	public bool IsEnabled { get; private set; }
	public DateTime? StartedUtc { get; private set; }
	public DateTime LastActivityUtc { get; private set; }
	public bool IsDistributedManagerAvailable { get; private set; }
}
