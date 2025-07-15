using Legion.Logging;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations.Schema;

namespace Legion.ADF.ServiceBus.Model;

public sealed partial class JobActivity : ServiceBus.ServiceBusBaseEntity, Legion.Model.Concurrence.IConcurrent, Legion.Model.IEntity
{
	internal static IResult<JobActivity> Create(
		IScopeContext scopeContext,
		string hostName,
		Job job,
		Guid idCurrentHost)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<JobActivity>();

		if (result.IsArgumentNullOrWhiteSpace(scopeContext, hostName))
			return result.Build();

		if (result.IsArgumentNull(scopeContext, job))
			return result.Build();

		var utcNow = GlobalContext.Instance.UtcNow;
		var id = GlobalContext.Instance.NewGuid();
		var jobActivity = new JobActivity
		{
			__IsNewObject = true,
			IdJobActivity = id,
			Job = job,
			IdJobStatus = JobStatus.Idle,
			IdCurrentHost = idCurrentHost,
			AttachedToCurrentHostUtc = utcNow,
			LastStatusChangedUtc = utcNow,
			LastProcessingStartedUtc = null,
			LastProcessingFinishedUtc = null,
			DelayedToUtc = null,
			RowVersion = id
		};

		job.AttachActivity(scopeContext, jobActivity);

		var logResult = Model.JobLog.Create(
			scopeContext,
			job,
			nameof(Model.JobStatus.Disconnected),
			new LogMessageBuilder(scopeContext, errorCode: null)
				.LogLevel(LogLevel.Information)
				.InternalMessage($"Host {hostName}: Job {job.Name} is created as Disconnected")
				.Build(),
			Model.JobStatus.Disconnected,
			idMessageProcessingLog: null);

		if (result.MergeHasError(logResult))
			return result.Build();

		job.AddLog(logResult.Data!);

		var validationResult =
			DefaultDBValidator
				.Validate(jobActivity);

		if (result.MergeHasError(scopeContext, validationResult, true))
			return result.Build();

		return result.WithData(jobActivity).Build();
	}

	internal IResult Start(
		IScopeContext scopeContext,
		string hostName)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder();

		if (result.IsNull(scopeContext, Job))
			return result.Build();

		IdJobStatus = JobStatus.Started;
		LastStatusChangedUtc = GlobalContext.Instance.UtcNow;

		var logResult = Model.JobLog.Create(
			scopeContext,
			Job,
			nameof(Model.JobStatus.Started),
			new LogMessageBuilder(scopeContext, errorCode: null)
				.LogLevel(LogLevel.Information)
				.InternalMessage($"Host {hostName}: Job {Job.Name} is started")
				.Build(),
			Model.JobStatus.Started,
			idMessageProcessingLog: null);

		if (result.MergeHasError(logResult))
			return result.Build();

		Job.AddLog(logResult.Data!);

		var validationResult =
			DefaultDBValidator
				.Validate(this);

		if (result.MergeHasError(scopeContext, validationResult, true))
			return result.Build();

		return result.Build();
	}

	internal IResult<bool> RunRunOnHost(
		IScopeContext scopeContext,
		Guid? idHost,
		bool defaultHostIsAlive,
		bool currentHostIsAlive,
		string hostName)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<bool>();

		if (result.IsNull(scopeContext, Job))
			return result.Build();

		if (!CanRunOnHost(idHost, defaultHostIsAlive, currentHostIsAlive))
			return result.WithData(false).Build();

		IdJobStatus = JobStatus.Running;
		LastProcessingStartedUtc = GlobalContext.Instance.UtcNow;
		LastStatusChangedUtc = LastProcessingStartedUtc.Value;
		DelayedToUtc = null;

		var logResult = Model.JobLog.Create(
			scopeContext,
			Job,
			nameof(Model.JobStatus.Running),
			new LogMessageBuilder(scopeContext, errorCode: null)
				.LogLevel(LogLevel.Information)
				.InternalMessage($"Host {hostName}: Job {Job.Name} is running")
				.Build(),
			Model.JobStatus.Running,
			idMessageProcessingLog: null);

		if (result.MergeHasError(logResult))
			return result.Build();

		Job.AddLog(logResult.Data!);

		var validationResult =
			DefaultDBValidator
				.Validate(this);

		if (result.MergeHasError(scopeContext, validationResult, true))
			return result.Build();

		return result.WithData(true).Build();
	}

	internal IResult FinishedSuccessfully(
		IScopeContext scopeContext,
		string hostName,
		DateTime? delayedToUtc)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder();

		if (result.IsNull(scopeContext, Job))
			return result.Build();

		IdJobStatus = JobStatus.Idle;
		LastProcessingFinishedUtc = GlobalContext.Instance.UtcNow;
		LastStatusChangedUtc = LastProcessingFinishedUtc.Value;
		DelayedToUtc = delayedToUtc;

		var logResult = Model.JobLog.Create(
			scopeContext,
			Job,
			nameof(Model.JobStatus.Idle),
			new LogMessageBuilder(scopeContext, errorCode: null)
				.LogLevel(LogLevel.Information)
				.InternalMessage($"Host {hostName}: Job {Job.Name} successfully finished")
				.Build(),
			Model.JobStatus.Idle,
			idMessageProcessingLog: null);

		if (result.MergeHasError(logResult))
			return result.Build();

		Job.AddLog(logResult.Data!);

		var validationResult =
			DefaultDBValidator
				.Validate(this);

		if (result.MergeHasError(scopeContext, validationResult, true))
			return result.Build();

		return result.Build();
	}

	internal IResult FinishedWithError(
		IScopeContext scopeContext,
		string hostName,
		string? errorDetail,
		DateTime? delayedToUtc)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder();

		if (result.IsNull(scopeContext, Job))
			return result.Build();

		IdJobStatus = JobStatus.Error;
		LastProcessingFinishedUtc = GlobalContext.Instance.UtcNow;
		LastStatusChangedUtc = LastProcessingFinishedUtc.Value;
		DelayedToUtc = delayedToUtc;

		var logResult = Model.JobLog.Create(
			scopeContext,
			Job,
			nameof(Model.JobStatus.Error),
			new LogMessageBuilder(scopeContext, errorCode: null)
				.LogLevel(LogLevel.Error)
				.InternalMessage($"Host {hostName}: Job {Job.Name} finished with error")
				.Detail(errorDetail)
				.Build(),
			Model.JobStatus.Error,
			idMessageProcessingLog: null);

		if (result.MergeHasError(logResult))
			return result.Build();

		Job.AddLog(logResult.Data!);

		var validationResult =
			DefaultDBValidator
				.Validate(this);

		if (result.MergeHasError(scopeContext, validationResult, true))
			return result.Build();

		return result.Build();
	}

	internal IResult<bool> Disabling(
		IScopeContext scopeContext,
		string hostName)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<bool>();

		if (result.IsNull(scopeContext, Job))
			return result.Build();

		if (IdJobStatus == JobStatus.Disabling
			|| IdJobStatus == JobStatus.Disabled)
			return result.WithData(false).Build();

		IdJobStatus = JobStatus.Disabling;
		LastStatusChangedUtc = GlobalContext.Instance.UtcNow;

		var logResult = Model.JobLog.Create(
			scopeContext,
			Job,
			nameof(Model.JobStatus.Disabling),
			new LogMessageBuilder(scopeContext, errorCode: null)
				.LogLevel(LogLevel.Information)
				.InternalMessage($"Host {hostName}: Job {Job.Name} is disabling")
				.Build(),
			Model.JobStatus.Disabling,
			idMessageProcessingLog: null);

		if (result.MergeHasError(logResult))
			return result.Build();

		Job.AddLog(logResult.Data!);

		var validationResult =
			DefaultDBValidator
				.Validate(this);

		if (result.MergeHasError(scopeContext, validationResult, true))
			return result.Build();

		return result.WithData(true).Build();
	}

	internal IResult<bool> Disable(
		IScopeContext scopeContext,
		string hostName)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<bool>();

		if (result.IsNull(scopeContext, Job))
			return result.Build();

		if (!Job.RequestedToDisable
			|| IdJobStatus != JobStatus.Disabling)
			return result.WithData(false).Build();

		IdJobStatus = JobStatus.Disabled;
		LastProcessingFinishedUtc = GlobalContext.Instance.UtcNow;
		LastStatusChangedUtc = LastProcessingFinishedUtc.Value;

		var logResult = Model.JobLog.Create(
			scopeContext,
			Job,
			nameof(Model.JobStatus.Disabled),
			new LogMessageBuilder(scopeContext, errorCode: null)
				.LogLevel(LogLevel.Information)
				.InternalMessage($"Host {hostName}: Job {Job.Name} is disabled")
				.Build(),
			Model.JobStatus.Disabled,
			idMessageProcessingLog: null);

		if (result.MergeHasError(logResult))
			return result.Build();

		Job.AddLog(logResult.Data!);

		var validationResult =
			DefaultDBValidator
				.Validate(this);

		if (result.MergeHasError(scopeContext, validationResult, true))
			return result.Build();

		return result.WithData(true).Build();
	}

	internal IResult Canceling(
		IScopeContext scopeContext,
		string hostName)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder();

		if (result.IsNull(scopeContext, Job))
			return result.Build();

		IdJobStatus = JobStatus.Canceling;
		LastStatusChangedUtc = GlobalContext.Instance.UtcNow;

		var logResult = Model.JobLog.Create(
			scopeContext,
			Job,
			nameof(Model.JobStatus.Canceling),
			new LogMessageBuilder(scopeContext, errorCode: null)
				.LogLevel(LogLevel.Information)
				.InternalMessage($"Host {hostName}: Job {Job.Name} is canceling")
				.Build(),
			Model.JobStatus.Canceling,
			idMessageProcessingLog: null);

		if (result.MergeHasError(logResult))
			return result.Build();

		Job.AddLog(logResult.Data!);

		var validationResult =
			DefaultDBValidator
				.Validate(this);

		if (result.MergeHasError(scopeContext, validationResult, true))
			return result.Build();

		return result.Build();
	}

	internal DateTime GetMaximumCompletionTimeUtc()
		=> LastProcessingStartedUtc?.AddSeconds(Job.TimeoutForProcessingInSeconds) ?? DateTime.MinValue;

	internal bool IsRunning()
		=> IdJobStatus == JobStatus.Running
			&& GlobalContext.Instance.UtcNow < GetMaximumCompletionTimeUtc();

	internal bool CanRunOnHost(Guid? idHost, bool defaultHostIsAlive, bool currentHostIsAlive)
		=> (IdJobStatus == JobStatus.Started || IdJobStatus == JobStatus.Idle || IdJobStatus == JobStatus.Error)
			&& ((defaultHostIsAlive && Job.IdDefaultHost == idHost)
				|| (!defaultHostIsAlive && currentHostIsAlive && IdCurrentHost == idHost)
				|| (!defaultHostIsAlive && !currentHostIsAlive))
			&& !Job.RequestedToDisable
			&& (!DelayedToUtc.HasValue || DelayedToUtc <= GlobalContext.Instance.UtcNow);
}
