using Legion;
using Legion.Model.Mappers;

namespace Legion.ADF.ServiceBus.Model;

public sealed partial class Job : ServiceBus.ServiceBusBaseEntity, Legion.Model.Concurrence.IConcurrent, Legion.Model.IEntity
{
	public static ServiceBus.Model.Job? Map(
		ServiceBus.Model.Job source,
		ServiceBus.Model.Job? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<ServiceBus.Model.Job>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		Throw.IfArgumentNull(source);

		return source.MapTo(target, referenceModifier, conditions, instanceFactory, cache);
	}

	public ServiceBus.Model.Job? Clone(
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<ServiceBus.Model.Job>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
		=> MapTo(target: null, referenceModifier, conditions, instanceFactory, cache);

	public ServiceBus.Model.Job? MapTo(
		ServiceBus.Model.Job? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<ServiceBus.Model.Job>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		cache ??= [];
		target ??= instanceFactory?.CreateInstance<Legion.ADF.ServiceBus.Model.Job>(
				useActivatorIfNoFactoryFound: false,
				throwIfFactoryReturnsNull: true)
			?? new Legion.ADF.ServiceBus.Model.Job();

		if (cache.TryGetValue(this, out var cached))
			return (ServiceBus.Model.Job)cached;
			
		MappingConditions<ServiceBus.Model.Job>? conds = null;
		if (conditions != null)
		{
			conds = new MappingConditions<ServiceBus.Model.Job>();
			conditions.Invoke(conds);

			if (conds.CanMap(this, nameof(IdJob)))
				target.IdJob = IdJob;
			if (conds.CanMap(this, nameof(Name)))
				target.Name = Name;
			if (conds.CanMap(this, nameof(Description)))
				target.Description = Description;
			if (conds.CanMap(this, nameof(IdJobRunType)))
				target.IdJobRunType = IdJobRunType;
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
			if (conds.CanMap(this, nameof(IdDefaultHost)))
				target.IdDefaultHost = IdDefaultHost;
			if (conds.CanMap(this, nameof(RequestedToDisable)))
				target.RequestedToDisable = RequestedToDisable;
			if (conds.CanMap(this, nameof(TimeoutForProcessingInSeconds)))
				target.TimeoutForProcessingInSeconds = TimeoutForProcessingInSeconds;
			if (conds.CanMap(this, nameof(RowVersion)))
				target.RowVersion = RowVersion;
		}
		else
		{
			target.IdJob = IdJob;
			target.Name = Name;
			target.Description = Description;
			target.IdJobRunType = IdJobRunType;
			target.Namespace = Namespace;
			target.Properties = Properties;
			target.DelayedStartInSeconds = DelayedStartInSeconds;
			target.IdleTimeoutInSeconds = IdleTimeoutInSeconds;
			target.CronExpression = CronExpression;
			target.CronExpressionIncludeSeconds = CronExpressionIncludeSeconds;
			target.IdDefaultHost = IdDefaultHost;
			target.RequestedToDisable = RequestedToDisable;
			target.TimeoutForProcessingInSeconds = TimeoutForProcessingInSeconds;
			target.RowVersion = RowVersion;
		}

		cache.Add(this, target);

		if (referenceModifier == ReferenceModifier.MapAllReferences)
		{
			target.JobRunType = JobRunType?.MapTo(target.JobRunType, referenceModifier, conds?.GetConditions(x => x.JobRunType), instanceFactory, cache)!;
			target.JobActivity = JobActivity?.MapTo(target.JobActivity, referenceModifier, conds?.GetConditions(x => x.JobActivity), instanceFactory, cache)!;
			target._jobDatas = MapperHelper.MapToList(JobDatas, target._jobDatas, Legion.ADF.ServiceBus.Model.JobData.Map, referenceModifier, conds?.GetConditions(x => x.JobDatas), instanceFactory, cache)!;
			target._jobExecutions = MapperHelper.MapToList(JobExecutions, target._jobExecutions, Legion.ADF.ServiceBus.Model.JobExecution.Map, referenceModifier, conds?.GetConditions(x => x.JobExecutions), instanceFactory, cache)!;
			target._jobLogs = MapperHelper.MapToList(JobLogs, target._jobLogs, Legion.ADF.ServiceBus.Model.JobLog.Map, referenceModifier, conds?.GetConditions(x => x.JobLogs), instanceFactory, cache)!;
			target._jobMessages = MapperHelper.MapToList(JobMessages, target._jobMessages, Legion.ADF.ServiceBus.Model.JobMessage.Map, referenceModifier, conds?.GetConditions(x => x.JobMessages), instanceFactory, cache)!;
			target._jobStatistics = MapperHelper.MapToList(JobStatistics, target._jobStatistics, Legion.ADF.ServiceBus.Model.JobStatistics.Map, referenceModifier, conds?.GetConditions(x => x.JobStatistics), instanceFactory, cache)!;
		}
		else if (referenceModifier == ReferenceModifier.SetNull)
		{
			target.JobRunType = null!;
			target.JobActivity = null!;
			target._jobDatas = [];
			target._jobExecutions = [];
			target._jobLogs = [];
			target._jobMessages = [];
			target._jobStatistics = [];
		}

		return target;
	}
}
