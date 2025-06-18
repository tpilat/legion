using Legion;
using Legion.Model.Mappers;

namespace Legion.ADF.ServiceBus.Jobs.Model;

public sealed partial class Job : Jobs.JobsBaseEntity, Legion.Model.IEntity
{
	public static Jobs.Model.Job? Map(
		Jobs.Model.Job source,
		Jobs.Model.Job? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Jobs.Model.Job>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		Throw.IfArgumentNull(source);

		return source.MapTo(target, referenceModifier, conditions, instanceFactory, cache);
	}

	public Jobs.Model.Job? Clone(
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Jobs.Model.Job>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
		=> MapTo(target: null, referenceModifier, conditions, instanceFactory, cache);

	public Jobs.Model.Job? MapTo(
		Jobs.Model.Job? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Jobs.Model.Job>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		cache ??= [];
		target ??= instanceFactory?.CreateInstance<Legion.ADF.ServiceBus.Jobs.Model.Job>(
				useActivatorIfNoFactoryFound: false,
				throwIfFactoryReturnsNull: true)
			?? new Legion.ADF.ServiceBus.Jobs.Model.Job();

		if (cache.TryGetValue(this, out var cached))
			return (Jobs.Model.Job)cached;
			
		MappingConditions<Jobs.Model.Job>? conds = null;
		if (conditions != null)
		{
			conds = new MappingConditions<Jobs.Model.Job>();
			conditions.Invoke(conds);

			if (conds.CanMap(this, nameof(IdJob)))
				target.IdJob = IdJob;
			if (conds.CanMap(this, nameof(Name)))
				target.Name = Name;
			if (conds.CanMap(this, nameof(Description)))
				target.Description = Description;
			if (conds.CanMap(this, nameof(IdJobRunType)))
				target.IdJobRunType = IdJobRunType;
			if (conds.CanMap(this, nameof(IdJobStatus)))
				target.IdJobStatus = IdJobStatus;
			if (conds.CanMap(this, nameof(Namespace)))
				target.Namespace = Namespace;
			if (conds.CanMap(this, nameof(Properties)))
				target.Properties = Properties;
			if (conds.CanMap(this, nameof(DelayedStartInSeconds)))
				target.DelayedStartInSeconds = DelayedStartInSeconds;
			if (conds.CanMap(this, nameof(IdleTimeoutInSeconds)))
				target.IdleTimeoutInSeconds = IdleTimeoutInSeconds;
			if (conds.CanMap(this, nameof(CronExpression)))
				target.CronExpression = CronExpression;
			if (conds.CanMap(this, nameof(CronExpressionIncludeSeconds)))
				target.CronExpressionIncludeSeconds = CronExpressionIncludeSeconds;
			if (conds.CanMap(this, nameof(LastProcessingUtc)))
				target.LastProcessingUtc = LastProcessingUtc;
			if (conds.CanMap(this, nameof(NextProcessinUtc)))
				target.NextProcessinUtc = NextProcessinUtc;
			if (conds.CanMap(this, nameof(TimeoutForProcessingInSeconds)))
				target.TimeoutForProcessingInSeconds = TimeoutForProcessingInSeconds;
			if (conds.CanMap(this, nameof(MaxProcessingRetryCount)))
				target.MaxProcessingRetryCount = MaxProcessingRetryCount;
		}
		else
		{
			target.IdJob = IdJob;
			target.Name = Name;
			target.Description = Description;
			target.IdJobRunType = IdJobRunType;
			target.IdJobStatus = IdJobStatus;
			target.Namespace = Namespace;
			target.Properties = Properties;
			target.DelayedStartInSeconds = DelayedStartInSeconds;
			target.IdleTimeoutInSeconds = IdleTimeoutInSeconds;
			target.CronExpression = CronExpression;
			target.CronExpressionIncludeSeconds = CronExpressionIncludeSeconds;
			target.LastProcessingUtc = LastProcessingUtc;
			target.NextProcessinUtc = NextProcessinUtc;
			target.TimeoutForProcessingInSeconds = TimeoutForProcessingInSeconds;
			target.MaxProcessingRetryCount = MaxProcessingRetryCount;
		}

		cache.Add(this, target);

		if (referenceModifier == ReferenceModifier.MapAllReferences)
		{
			target.JobRunType = JobRunType?.MapTo(target.JobRunType, referenceModifier, conds?.GetConditions(x => x.JobRunType), instanceFactory, cache)!;
			target.JobStatus = JobStatus?.MapTo(target.JobStatus, referenceModifier, conds?.GetConditions(x => x.JobStatus), instanceFactory, cache)!;
			target._jobDatas = MapperHelper.MapToList(JobDatas, target._jobDatas, Legion.ADF.ServiceBus.Jobs.Model.JobData.Map, referenceModifier, conds?.GetConditions(x => x.JobDatas), instanceFactory, cache)!;
			target._jobExecutions = MapperHelper.MapToList(JobExecutions, target._jobExecutions, Legion.ADF.ServiceBus.Jobs.Model.JobExecution.Map, referenceModifier, conds?.GetConditions(x => x.JobExecutions), instanceFactory, cache)!;
			target._jobLogs = MapperHelper.MapToList(JobLogs, target._jobLogs, Legion.ADF.ServiceBus.Jobs.Model.JobLog.Map, referenceModifier, conds?.GetConditions(x => x.JobLogs), instanceFactory, cache)!;
			target._jobMessages = MapperHelper.MapToList(JobMessages, target._jobMessages, Legion.ADF.ServiceBus.Jobs.Model.JobMessage.Map, referenceModifier, conds?.GetConditions(x => x.JobMessages), instanceFactory, cache)!;
			target._jobStatistics = MapperHelper.MapToList(JobStatistics, target._jobStatistics, Legion.ADF.ServiceBus.Jobs.Model.JobStatistics.Map, referenceModifier, conds?.GetConditions(x => x.JobStatistics), instanceFactory, cache)!;
		}
		else if (referenceModifier == ReferenceModifier.SetNull)
		{
			target.JobRunType = null!;
			target.JobStatus = null!;
			target._jobDatas = [];
			target._jobExecutions = [];
			target._jobLogs = [];
			target._jobMessages = [];
			target._jobStatistics = [];
		}

		return target;
	}
}
