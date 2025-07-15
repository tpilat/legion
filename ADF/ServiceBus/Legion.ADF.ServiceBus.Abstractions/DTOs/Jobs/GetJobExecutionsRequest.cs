namespace Legion.ADF.ServiceBus.DTOs.Jobs;

public record GetJobExecutionsRequest
{
	public Guid IdJob { get; set; }
	public DateTime FromUtc { get; set; }
	public DateTime ToUtc { get; set; }
	public int PageIndex { get; set; }
	public int PageSize { get; set; }
}
