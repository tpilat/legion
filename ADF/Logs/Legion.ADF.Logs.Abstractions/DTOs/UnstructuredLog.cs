using Microsoft.Extensions.Logging;

namespace Legion.ADF.Logs.DTOs;

public class UnstructuredLog
{
	public Guid? Id { get; set; }
	public DateTime? CreatedUtc { get; set; }
	public Microsoft.Extensions.Logging.LogLevel LogLevel { get; set; }
	public EventId EventId { get; set; }
	public string? Message { get; set; }
	public Exception? Exception { get; set; }
	public string? SourceContext { get; set; }
}
