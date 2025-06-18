namespace Legion.ADF.ServiceBus.DTOs.Jobs;

public class JobExecutionDto
{
	public Guid IdJob { get; private set; }

	public Guid TraceCorrelationId { get; private set; }

	public DateTime StartUtc { get; private set; }

	public DateTime? EndUtc { get; private set; }

	public Guid IdJobStatus { get; private set; }

	public string JobStatus { get; private set; }

}
