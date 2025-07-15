namespace Legion.ADF.ServiceBus.DTOs.Jobs;

public class JobLogDto
{
	public Guid IdJob { get; set; }

	public int IdLogLevel { get; set; }

	public DateTime CreatedUtc { get; set; }

	public Guid IdJobStatus { get; set; }

	public string JobStatus { get; set; }

	public Guid TraceCorrelationId { get; set; }

	public Guid? IdLogMessage { get; set; }

	public string Code { get; set; }

	public string? Detail { get; set; }

	public Guid? IdMessageProcessingLog { get; set; }
}
