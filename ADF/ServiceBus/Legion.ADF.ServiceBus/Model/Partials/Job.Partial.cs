using Legion.Enums;
using Legion.Extensions;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations.Schema;

namespace Legion.ADF.ServiceBus.Model;

public sealed partial class Job : ServiceBus.ServiceBusBaseEntity, Legion.Model.Concurrence.IConcurrent, Legion.Model.IEntity
{
	[NotMapped]
	internal Host? DefaultHost { get; set; }

	[NotMapped]
	internal Host? CurrentHost { get; set; }

	internal static IResult<Job> Create(
		IScopeContext scopeContext,
		string name,
		string description,
		bool isEnabled)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<Job>();

		if (result.IsArgumentNullOrWhiteSpace(scopeContext, name))
			return result.Build();

		if (result.IsArgumentNullOrWhiteSpace(scopeContext, description))
			return result.Build();

		var utcNow = GlobalContext.Instance.UtcNow;
		var id = GlobalContext.Instance.NewGuid();
		var job = new Job
		{
			__IsNewObject = true,
			IdJob = id,
			Name = name,
			Description = description,
		};

		var validationResult =
			DefaultDBValidator
				.Validate(job);

		if (result.MergeHasError(scopeContext, validationResult, true))
			return result.Build();

		return result.WithData(job).Build();
	}

	internal IResult AttachActivity(
		IScopeContext scopeContext,
		JobActivity jobActivity)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder();

		if (result.IsArgumentNull(scopeContext, jobActivity))
			return result.Build();

		JobActivity = jobActivity;

		var validationResult =
			DefaultDBValidator
				.Validate(this);

		if (result.MergeHasError(scopeContext, validationResult, true))
			return result.Build();

		return result.Build();
	}

	internal void AddLog(JobLog jobLog)
	{
		_jobLogs.Add(jobLog);
	}

	internal IResult Enable(IScopeContext scopeContext)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder();

		RequestedToDisable = true;

		var validationResult =
			DefaultDBValidator
				.Validate(this);

		if (result.MergeHasError(scopeContext, validationResult, true))
			return result.Build();

		return result.Build();
	}

	internal IResult Disable(IScopeContext scopeContext)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder();

		RequestedToDisable = false;

		var validationResult =
			DefaultDBValidator
				.Validate(this);

		if (result.MergeHasError(scopeContext, validationResult, true))
			return result.Build();

		return result.Build();
	}

	internal string GetDistributedCacheKey(
		string systemName,
		string operation)
		=> string.IsNullOrEmpty(operation)
			? $"{systemName}:Legion.ADF.ServiceBus.Model.JobActivity:{Name}"
			: $"{systemName}:Legion.ADF.ServiceBus.Model.JobActivity:{Name}:{operation}";

	internal bool IsRunning()
		=> JobActivity?.IsRunning() == true;

	internal bool CanRunOnHost(Guid? idHost, bool defaultHostIsAlive, bool currentHostIsAlive)
		=> JobActivity == null || JobActivity.CanRunOnHost(idHost, defaultHostIsAlive, currentHostIsAlive);

	internal List<DateTime> GetNextExecutionTimes(int count = 10)
	{
		var result = new List<DateTime>();

		if (IdJobRunType == JobRunType.SequentialTimer)
		{
			if (!IdleTimeoutInSeconds.HasValue)
				return result;

			var startTime = GlobalContext.Instance.UtcNow;

			if (JobActivity?.LastProcessingStartedUtc != null)
			{
				if (JobActivity.LastProcessingFinishedUtc.HasValue)
				{
					if (JobActivity.LastProcessingStartedUtc < JobActivity.LastProcessingFinishedUtc)
					{
						//finished, waiting to start
						startTime = JobActivity.LastProcessingFinishedUtc.Value.AddSeconds(IdleTimeoutInSeconds.Value);
						result.Add(startTime);
						count--;
					}
					else
					{
						//started, but not finished
						startTime = JobActivity.LastProcessingStartedUtc.Value.AddSeconds(TimeoutForProcessingInSeconds + IdleTimeoutInSeconds.Value);
						result.Add(startTime);
						count--;
					}
				}
				else
				{
					//started, but not finished
					startTime = JobActivity.LastProcessingStartedUtc.Value.AddSeconds(TimeoutForProcessingInSeconds + IdleTimeoutInSeconds.Value);
					result.Add(startTime);
					count--;
				}
			}

			for (int i = 0; i < count; i++)
			{
				startTime = startTime.AddSeconds(TimeoutForProcessingInSeconds + IdleTimeoutInSeconds.Value);
				result.Add(startTime);
			}
		}
		else if (IdJobRunType == JobRunType.PeriodicTimer)
		{
			if (!IdleTimeoutInSeconds.HasValue)
				return result;

			var startTime = JobActivity?.LastProcessingStartedUtc != null
				? JobActivity.LastProcessingStartedUtc.Value
				: GlobalContext.Instance.UtcNow;

			for (int i = 0; i < count; i++)
			{
				startTime = startTime.AddSeconds(IdleTimeoutInSeconds.Value);
				result.Add(startTime);
			}
		}
		else if (IdJobRunType == JobRunType.Cron)
		{
			if (string.IsNullOrWhiteSpace(CronExpression))
				return result;

			DateTimeOffset? startTime = GlobalContext.Instance.DateTimeOffsetUtcNow;

			if (JobActivity?.LastProcessingStartedUtc != null)
			{
				if (JobActivity.LastProcessingFinishedUtc.HasValue)
				{
					if (JobActivity.LastProcessingStartedUtc < JobActivity.LastProcessingFinishedUtc)
					{
						//finished, waiting to start
						startTime = JobActivity.LastProcessingFinishedUtc.Value;
						result.Add(startTime.Value.UtcDateTime);
						count--;
					}
					else
					{
						//started, but not finished
						startTime = JobActivity.LastProcessingStartedUtc.Value.AddSeconds(TimeoutForProcessingInSeconds);
						result.Add(startTime.Value.UtcDateTime);
						count--;
					}
				}
				else
				{
					//started, but not finished
					startTime = JobActivity.LastProcessingStartedUtc.Value.AddSeconds(TimeoutForProcessingInSeconds);
					result.Add(startTime.Value.UtcDateTime);
					count--;
				}
			}

			for (int i = 0; i < count; i++)
			{
				startTime = Calendar.CronExpression.Parse(
					CronExpression,
					CronExpressionIncludeSeconds
						? Calendar.CronFormat.IncludeSeconds
						: Calendar.CronFormat.Standard)
					.GetNextOccurrence(startTime.Value, TimeZoneInfo.Utc);

				if (!startTime.HasValue)
					return result;
				else
					result.Add(startTime.Value.UtcDateTime);
			}
		}

		return result;
	}

	internal TimeSpan GetExecutionInterval()
	{
		var nextExecutions = GetNextExecutionTimes(3).Skip(1).ToList();
		if (nextExecutions.Count < 2)
		{
			return TimeSpan.FromHours(1);
		}

		return nextExecutions[1] - nextExecutions[0];
	}

	internal DTOs.Jobs.JobDto ToDto()
	{
		var dto = new DTOs.Jobs.JobDto
		{
			IdJob = IdJob,
			Name = Name,
			Description = Description,
			IdDefaultHost = IdDefaultHost,
			RequestedToDisable = RequestedToDisable,
			TimeoutForProcessingInSeconds = TimeoutForProcessingInSeconds,
			IdJobStatus = JobActivity?.IdJobStatus ?? JobStatus.Disconnected,
			JobStatus = JobActivity == null
				? nameof(JobStatus.Disconnected)
				: JobStatus.FromId(JobActivity.IdJobStatus)!.Code!.ToCammelCase(removeUnderscores: false, throwIfEmpty: false)!,
			IdCurrentHost = JobActivity?.IdCurrentHost,
			LastProcessingStaredAt = JobActivity?.LastProcessingStartedUtc?.ToLocalTime(),
			LastProcessingFinishedAt = JobActivity?.LastProcessingFinishedUtc?.ToLocalTime(),
			DelayedToAt = JobActivity?.DelayedToUtc?.ToLocalTime()
		};

		return dto;
	}

	internal DTOs.Jobs.JobDetailDto ToDetailDto()
	{
		var dto = new DTOs.Jobs.JobDetailDto
		{
			IdJob = IdJob,
			Name = Name,
			Description = Description,
			IdJobRunType = IdJobRunType,
			JobRunType = EnumHelper.ConvertStringToEnum<DTOs.Jobs.JobRunTypeEnum>(
				Model.JobRunType.FromId(IdJobRunType)!.Code!.ToCammelCase(removeUnderscores: false, throwIfEmpty: false)!),
			Namespace = Namespace,
			Properties = Properties,
			DelayedStartInSeconds = DelayedStartInSeconds,
			IdleTimeoutInSeconds = IdleTimeoutInSeconds,
			CronExpression = CronExpression,
			CronExpressionIncludeSeconds = CronExpressionIncludeSeconds,
			IdDefaultHost = IdDefaultHost,
			RequestedToDisable = RequestedToDisable,
			TimeoutForProcessingInSeconds = TimeoutForProcessingInSeconds,
			IdJobStatus = JobActivity?.IdJobStatus ?? JobStatus.Disconnected,
			JobStatus = JobActivity == null
				? nameof(JobStatus.Disconnected)
				: JobStatus.FromId(JobActivity.IdJobStatus)!.Code!.ToCammelCase(removeUnderscores: false, throwIfEmpty: false)!,
			IdCurrentHost = JobActivity?.IdCurrentHost,
			AttachedToCurrentHostAt = JobActivity?.AttachedToCurrentHostUtc.ToLocalTime(),
			LastStatusChangedAt = JobActivity?.LastStatusChangedUtc.ToLocalTime(),
			LastProcessingStaredtAt = JobActivity?.LastProcessingStartedUtc?.ToLocalTime(),
			LastProcessingFinishedAt = JobActivity?.LastProcessingFinishedUtc?.ToLocalTime(),
			DelayedToAt = JobActivity?.DelayedToUtc?.ToLocalTime(),
			ExecutionInterval = GetExecutionInterval()
		};

		return dto;
	}
}
