namespace Legion.ADF.ServiceBus.DTOs.Hosts;

public class HostLogDto
{
	public Guid IdHost { get; private set; }

	public int IdLogLevel { get; set; }

	public DateTime CreatedUtc { get; set; }

	public bool IsRunning { get; set; }

	public Guid TraceCorrelationId { get; set; }

	public Guid? IdLogMessage { get; set; }

	public string Code { get; set; }

	public string? Detail { get; set; }
}
