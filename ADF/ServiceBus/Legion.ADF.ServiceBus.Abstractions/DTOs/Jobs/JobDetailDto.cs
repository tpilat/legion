namespace Legion.ADF.ServiceBus.DTOs.Jobs;

public class JobDetailDto
{
	public Guid IdJob { get; set; }

	public string Name { get; set; }

	public string? Description { get; set; }

	public Guid IdJobRunType { get; set; }

	public JobRunTypeEnum JobRunType { get; set; }

	public string Namespace { get; set; }

	public string? Properties { get; set; }

	public int? DelayedStartInSeconds { get; set; }

	public int? IdleTimeoutInSeconds { get; set; }

	public string? CronExpression { get; set; }

	public bool CronExpressionIncludeSeconds { get; set; }

	public Guid IdDefaultHost { get; set; }

	public bool RequestedToDisable { get; set; }

	public int TimeoutForProcessingInSeconds { get; set; }

	public Guid IdJobStatus { get; set; }

	public string JobStatus { get; set; }

	public Guid? IdCurrentHost { get; set; }

	public DateTime? AttachedToCurrentHostAt { get; set; }

	public DateTime? LastStatusChangedAt { get; set; }

	public DateTime? LastProcessingStaredtAt { get; set; }

	public DateTime? LastProcessingFinishedAt { get; set; }

	public DateTime? DelayedToAt { get; set; }

	public TimeSpan ExecutionInterval { get; set; }
}
