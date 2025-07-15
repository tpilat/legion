using System.Globalization;

namespace Legion.ADF.ServiceBus.DTOs.Jobs;

public class JobStatisticsDto
{
	public Guid IdJob { get; set; }

	public DateTime StartHourUtc { get; set; }

	public int ExecutionCount { get; set; }

	public int ErrorCount { get; set; }

	public decimal AverageDurationInSeconds { get; set; }

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
					AverageDurationInSeconds = g.Average(x => x.AverageDurationInSeconds)
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
					AverageDurationInSeconds = g.Average(x => x.AverageDurationInSeconds)
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
					AverageDurationInSeconds = g.Average(x => x.AverageDurationInSeconds)
				})
				.ToList();
		}

		Throw.NotSupportedException(
			$"Job execution type '{jobExecutionType}' is not supported for aggregation in JobStatisticsDto."
		);

		return null;
	}
}
