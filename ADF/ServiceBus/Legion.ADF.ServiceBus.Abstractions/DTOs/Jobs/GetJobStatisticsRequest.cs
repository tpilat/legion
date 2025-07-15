namespace Legion.ADF.ServiceBus.DTOs.Jobs;

public record GetJobStatisticsRequest
{
	public Guid IdJob { get; set; }
	public DateTime FromUtc { get; set; }
	public DateTime? ToUtc { get; set; }
	public DTOs.Jobs.JobExecutionTypeEnum JobExecutionType { get; set; }
}
