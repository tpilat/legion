using Legion;
using Legion.Model.Mappers;

namespace Legion.ADF.ServiceBus.Jobs.Model;

public sealed partial class JobExecution : Jobs.JobsBaseEntity, Legion.Model.IEntity
{
	public static Jobs.Model.JobExecution? Map(
		Jobs.Model.JobExecution source,
		Jobs.Model.JobExecution? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Jobs.Model.JobExecution>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		Throw.IfArgumentNull(source);

		return source.MapTo(target, referenceModifier, conditions, instanceFactory, cache);
	}

	public Jobs.Model.JobExecution? Clone(
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Jobs.Model.JobExecution>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
		=> MapTo(target: null, referenceModifier, conditions, instanceFactory, cache);

	public Jobs.Model.JobExecution? MapTo(
		Jobs.Model.JobExecution? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Jobs.Model.JobExecution>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		cache ??= [];
		target ??= instanceFactory?.CreateInstance<Legion.ADF.ServiceBus.Jobs.Model.JobExecution>(
				useActivatorIfNoFactoryFound: false,
				throwIfFactoryReturnsNull: true)
			?? new Legion.ADF.ServiceBus.Jobs.Model.JobExecution();

		if (cache.TryGetValue(this, out var cached))
			return (Jobs.Model.JobExecution)cached;
			
		MappingConditions<Jobs.Model.JobExecution>? conds = null;
		if (conditions != null)
		{
			conds = new MappingConditions<Jobs.Model.JobExecution>();
			conditions.Invoke(conds);

			if (conds.CanMap(this, nameof(IdJobExecution)))
				target.IdJobExecution = IdJobExecution;
			if (conds.CanMap(this, nameof(IdJob)))
				target.IdJob = IdJob;
			if (conds.CanMap(this, nameof(TraceCorrelationId)))
				target.TraceCorrelationId = TraceCorrelationId;
			if (conds.CanMap(this, nameof(StartUtc)))
				target.StartUtc = StartUtc;
			if (conds.CanMap(this, nameof(EndUtc)))
				target.EndUtc = EndUtc;
			if (conds.CanMap(this, nameof(IdJobStatus)))
				target.IdJobStatus = IdJobStatus;
		}
		else
		{
			target.IdJobExecution = IdJobExecution;
			target.IdJob = IdJob;
			target.TraceCorrelationId = TraceCorrelationId;
			target.StartUtc = StartUtc;
			target.EndUtc = EndUtc;
			target.IdJobStatus = IdJobStatus;
		}

		cache.Add(this, target);

		if (referenceModifier == ReferenceModifier.MapAllReferences)
		{
			target.Job = Job?.MapTo(target.Job, referenceModifier, conds?.GetConditions(x => x.Job), instanceFactory, cache)!;
			target.JobStatus = JobStatus?.MapTo(target.JobStatus, referenceModifier, conds?.GetConditions(x => x.JobStatus), instanceFactory, cache)!;
		}
		else if (referenceModifier == ReferenceModifier.SetNull)
		{
			target.Job = null!;
			target.JobStatus = null!;
		}

		return target;
	}
}
