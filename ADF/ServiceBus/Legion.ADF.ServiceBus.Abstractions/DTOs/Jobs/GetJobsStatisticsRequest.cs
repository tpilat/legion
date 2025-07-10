namespace Legion.ADF.ServiceBus.DTOs.Jobs;

public record GetJobsStatisticsRequest
{
	public DateTime From { get; set; }
	public DateTime To { get; set; }
	public DTOs.Jobs.JobExecutionTypeEnum JobExecutionType { get; set; }
}
