namespace Legion.ADF.ServiceBus.DTOs.Hosts;

public record GetHostLogsRequest
{
	public Guid IdHost { get; set; }
	public DateTime FromUtc { get; set; }
	public DateTime ToUtc { get; set; }
	public int PageIndex { get; set; }
	public int PageSize { get; set; }
}
