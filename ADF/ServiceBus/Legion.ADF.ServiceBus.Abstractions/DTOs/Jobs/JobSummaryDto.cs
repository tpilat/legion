namespace Legion.ADF.ServiceBus.DTOs.Jobs;

public class JobSummaryDto
{
	public Guid IdJob { get; private set; }

	public string Name { get; private set; }

	public string? Description { get; private set; }

	public Guid IdJobRunType { get; private set; }

	public JobRunTypeEnum JobRunType { get; private set; }

	public Guid IdJobStatus { get; private set; }

	public string JobStatus { get; private set; }

	public DateTime? LastProcessingUtc { get; private set; }

	public DateTime NextProcessinUtc { get; private set; }

	public int TimeoutForProcessingInSeconds { get; private set; }

	public Hosts.HostSummaryDto DefaultHost { get; private set; }

	public Hosts.HostSummaryDto CurrentHost { get; private set; }

	public List<JobExecutionTypeEnum> JobExecutionTypes { get; set; }
}
