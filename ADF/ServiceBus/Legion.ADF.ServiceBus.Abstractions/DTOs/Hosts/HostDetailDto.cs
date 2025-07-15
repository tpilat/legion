namespace Legion.ADF.ServiceBus.DTOs.Hosts;

public class HostDetailDto
{
	public Guid IdHost { get; set; }
	public string Name { get; set; }
	public string Description { get; set; }
	public bool IsEnabled { get; set; }
	public DateTime? StartedAt { get; set; }
	public DateTime? LastActivityAt { get; set; }
	public bool IsAvailable { get; set; }
	public bool IsDistributedManagerAvailable { get; set; }
	public string Configuration { get; set; }
}
