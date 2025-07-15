using Legion;
using Legion.Model.Mappers;

namespace Legion.ADF.ServiceBus.Model;

public sealed partial class JobStatus : ServiceBus.ServiceBusBaseEntity, Legion.Model.IEntity
{
	public static ServiceBus.Model.JobStatus? Map(
		ServiceBus.Model.JobStatus source,
		ServiceBus.Model.JobStatus? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<ServiceBus.Model.JobStatus>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		Throw.IfArgumentNull(source);

		return source.MapTo(target, referenceModifier, conditions, instanceFactory, cache);
	}

	public ServiceBus.Model.JobStatus? Clone(
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<ServiceBus.Model.JobStatus>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
		=> MapTo(target: null, referenceModifier, conditions, instanceFactory, cache);

	public ServiceBus.Model.JobStatus? MapTo(
		ServiceBus.Model.JobStatus? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<ServiceBus.Model.JobStatus>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		cache ??= [];
		target ??= Legion.ADF.ServiceBus.Model.JobStatus.DictionaryMap.Value[IdJobStatus];

		if (cache.TryGetValue(this, out var cached))
			return (ServiceBus.Model.JobStatus)cached;
			
		MappingConditions<ServiceBus.Model.JobStatus>? conds = null;
		if (conditions != null)
		{
			conds = new MappingConditions<ServiceBus.Model.JobStatus>();
			conditions.Invoke(conds);
		}

		cache.Add(this, target);

		if (referenceModifier == ReferenceModifier.MapAllReferences)
		{
			target._jobActivities = MapperHelper.MapToList(JobActivities, target._jobActivities, Legion.ADF.ServiceBus.Model.JobActivity.Map, referenceModifier, conds?.GetConditions(x => x.JobActivities), instanceFactory, cache)!;
			target._jobExecutions = MapperHelper.MapToList(JobExecutions, target._jobExecutions, Legion.ADF.ServiceBus.Model.JobExecution.Map, referenceModifier, conds?.GetConditions(x => x.JobExecutions), instanceFactory, cache)!;
			target._jobLogs = MapperHelper.MapToList(JobLogs, target._jobLogs, Legion.ADF.ServiceBus.Model.JobLog.Map, referenceModifier, conds?.GetConditions(x => x.JobLogs), instanceFactory, cache)!;
		}
		else if (referenceModifier == ReferenceModifier.SetNull)
		{
			target._jobActivities = [];
			target._jobExecutions = [];
			target._jobLogs = [];
		}

		return target;
	}
}
