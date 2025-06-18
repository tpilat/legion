namespace Legion.ADF.ServiceBus.DTOs.Jobs;

public class JobLogDto
{
	public Guid IdJob { get; private set; }

	public int IdLogLevel { get; private set; }

	public DateTime CreatedUtc { get; private set; }

	public Guid IdJobStatus { get; private set; }

	public string JobStatus { get; private set; }

	public Guid TraceCorrelationId { get; private set; }

	public Guid? IdLogMessage { get; private set; }

	public string Code { get; private set; }

	public string? Detail { get; private set; }

	public Guid? IdMessageProcessingLog { get; private set; }
}
