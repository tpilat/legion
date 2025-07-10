namespace Legion.ADF.ServiceBus.DTOs.Jobs;

public record GetJobExecutionsRequest
{
	public Guid IdJob { get; set; }
	public DateTime From { get; set; }
	public DateTime To { get; set; }
	public int PageIndex { get; set; }
	public int PageSize { get; set; }
}
