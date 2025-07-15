namespace Legion.ADF.ServiceBus.DTOs.Jobs;

public class JobExecutionDto
{
	public Guid IdJob { get; set; }

	public Guid TraceCorrelationId { get; set; }

	public DateTime StartUtc { get; set; }

	public DateTime? EndUtc { get; set; }

	public Guid IdJobStatus { get; set; }

	public string JobStatus { get; set; }

	public DateTime StatisticsStartHourAt { get; set; }

}
