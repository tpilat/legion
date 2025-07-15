namespace Legion.ADF.ServiceBus.DTOs.Jobs;

public class JobDto
{
	public Guid IdJob { get; set; }

	public string Name { get; set; }

	public string? Description { get; set; }

	public Guid IdDefaultHost { get; set; }

	public bool RequestedToDisable { get; set; }

	public int TimeoutForProcessingInSeconds { get; set; }

	public Guid IdJobStatus { get; set; }

	public string JobStatus { get; set; }

	public Guid? IdCurrentHost { get; set; }

	public DateTime? LastProcessingStaredAt { get; set; }

	public DateTime? LastProcessingFinishedAt { get; set; }

	public DateTime? DelayedToAt { get; set; }
}
