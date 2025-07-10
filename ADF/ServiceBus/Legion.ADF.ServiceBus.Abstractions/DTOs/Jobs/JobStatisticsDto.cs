using System.Globalization;

namespace Legion.ADF.ServiceBus.DTOs.Jobs;

public class JobStatisticsDto
{
	public Guid IdJob { get; private set; }

	public DateTime StartHourUtc { get; private set; }

	public int ExecutionCount { get; private set; }

	public int ErrorCount { get; private set; }

	public decimal AverageDuration { get; private set; }

	public static List<JobStatisticsDto> Aggregate(List<JobStatisticsDto> data, JobExecutionTypeEnum jobExecutionType)
	{
		if (jobExecutionType == JobExecutionTypeEnum.Hourly)
			return data;

		if (jobExecutionType == JobExecutionTypeEnum.Dayly)
		{
			return data
				.GroupBy(x => new { x.IdJob, x.StartHourUtc.Date })
				.Select(g => new JobStatisticsDto
				{
					IdJob = g.Key.IdJob,
					StartHourUtc = g.Key.Date,
					ExecutionCount = g.Sum(x => x.ExecutionCount),
					ErrorCount = g.Sum(x => x.ErrorCount),
					AverageDuration = g.Average(x => x.AverageDuration)
				})
				.ToList();
		}

		if (jobExecutionType == JobExecutionTypeEnum.Weekly)
		{
			return data
				.GroupBy(x => new
				{
					x.IdJob,
					Year = x.StartHourUtc.Year,
					Week = CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(
						x.StartHourUtc,
						CalendarWeekRule.FirstDay,
						DayOfWeek.Monday)
				})
				.Select(g => new JobStatisticsDto
				{
					IdJob = g.Key.IdJob,
					StartHourUtc = new DateTime(g.Key.Year, 1, 1).AddDays((g.Key.Week - 1) * 7), // Start of the week
					ExecutionCount = g.Sum(x => x.ExecutionCount),
					ErrorCount = g.Sum(x => x.ErrorCount),
					AverageDuration = g.Average(x => x.AverageDuration)
				})
				.ToList();
		}

		if (jobExecutionType == JobExecutionTypeEnum.Monthly)
		{
			return data
				.GroupBy(x => new { x.IdJob, Year = x.StartHourUtc.Year, Month = x.StartHourUtc.Month })
				.Select(g => new JobStatisticsDto
				{
					IdJob = g.Key.IdJob,
					StartHourUtc = new DateTime(g.Key.Year, g.Key.Month, 1),
					ExecutionCount = g.Sum(x => x.ExecutionCount),
					ErrorCount = g.Sum(x => x.ErrorCount),
					AverageDuration = g.Average(x => x.AverageDuration)
				})
				.ToList();
		}

		Throw.NotSupportedException(
			$"Job execution type '{jobExecutionType}' is not supported for aggregation in JobStatisticsDto."
		);

		return null;
	}
}
