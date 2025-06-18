using Legion;
using Legion.Model.Mappers;

namespace Legion.ADF.ServiceBus.Jobs.Model;

public sealed partial class JobStatus : Jobs.JobsBaseEntity, Legion.Model.IEntity
{
	public static Jobs.Model.JobStatus? Map(
		Jobs.Model.JobStatus source,
		Jobs.Model.JobStatus? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Jobs.Model.JobStatus>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		Throw.IfArgumentNull(source);

		return source.MapTo(target, referenceModifier, conditions, instanceFactory, cache);
	}

	public Jobs.Model.JobStatus? Clone(
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Jobs.Model.JobStatus>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
		=> MapTo(target: null, referenceModifier, conditions, instanceFactory, cache);

	public Jobs.Model.JobStatus? MapTo(
		Jobs.Model.JobStatus? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Jobs.Model.JobStatus>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		cache ??= [];
		target ??= Legion.ADF.ServiceBus.Jobs.Model.JobStatus.DictionaryMap.Value[IdJobStatus];

		if (cache.TryGetValue(this, out var cached))
			return (Jobs.Model.JobStatus)cached;
			
		MappingConditions<Jobs.Model.JobStatus>? conds = null;
		if (conditions != null)
		{
			conds = new MappingConditions<Jobs.Model.JobStatus>();
			conditions.Invoke(conds);
		}

		cache.Add(this, target);

		if (referenceModifier == ReferenceModifier.MapAllReferences)
		{
			target._jobExecutions = MapperHelper.MapToList(JobExecutions, target._jobExecutions, Legion.ADF.ServiceBus.Jobs.Model.JobExecution.Map, referenceModifier, conds?.GetConditions(x => x.JobExecutions), instanceFactory, cache)!;
			target._jobLogs = MapperHelper.MapToList(JobLogs, target._jobLogs, Legion.ADF.ServiceBus.Jobs.Model.JobLog.Map, referenceModifier, conds?.GetConditions(x => x.JobLogs), instanceFactory, cache)!;
			target._jobs = MapperHelper.MapToList(Jobs, target._jobs, Legion.ADF.ServiceBus.Jobs.Model.Job.Map, referenceModifier, conds?.GetConditions(x => x.Jobs), instanceFactory, cache)!;
		}
		else if (referenceModifier == ReferenceModifier.SetNull)
		{
			target._jobExecutions = [];
			target._jobLogs = [];
			target._jobs = [];
		}

		return target;
	}
}
