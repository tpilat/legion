namespace Legion.ADF.ServiceBus.DTOs.Jobs;

public class JobStatisticsDto
{
	public Guid IdJob { get; private set; }

	public DateTime StartHourUtc { get; private set; }

	public int ExecutionCount { get; private set; }

	public int ErrorCount { get; private set; }

	public decimal AverageDuration { get; private set; }
}
