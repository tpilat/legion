using Legion;
using Legion.Model.Mappers;

namespace Legion.ADF.ServiceBus.Jobs.Model;

public sealed partial class JobStatistics : Jobs.JobsBaseEntity, Legion.Model.IEntity
{
	public static Jobs.Model.JobStatistics? Map(
		Jobs.Model.JobStatistics source,
		Jobs.Model.JobStatistics? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Jobs.Model.JobStatistics>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		Throw.IfArgumentNull(source);

		return source.MapTo(target, referenceModifier, conditions, instanceFactory, cache);
	}

	public Jobs.Model.JobStatistics? Clone(
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Jobs.Model.JobStatistics>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
		=> MapTo(target: null, referenceModifier, conditions, instanceFactory, cache);

	public Jobs.Model.JobStatistics? MapTo(
		Jobs.Model.JobStatistics? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Jobs.Model.JobStatistics>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		cache ??= [];
		target ??= instanceFactory?.CreateInstance<Legion.ADF.ServiceBus.Jobs.Model.JobStatistics>(
				useActivatorIfNoFactoryFound: false,
				throwIfFactoryReturnsNull: true)
			?? new Legion.ADF.ServiceBus.Jobs.Model.JobStatistics();

		if (cache.TryGetValue(this, out var cached))
			return (Jobs.Model.JobStatistics)cached;
			
		MappingConditions<Jobs.Model.JobStatistics>? conds = null;
		if (conditions != null)
		{
			conds = new MappingConditions<Jobs.Model.JobStatistics>();
			conditions.Invoke(conds);

			if (conds.CanMap(this, nameof(IdJobStatistics)))
				target.IdJobStatistics = IdJobStatistics;
			if (conds.CanMap(this, nameof(IdJob)))
				target.IdJob = IdJob;
			if (conds.CanMap(this, nameof(StartHourUtc)))
				target.StartHourUtc = StartHourUtc;
			if (conds.CanMap(this, nameof(ExecutionCount)))
				target.ExecutionCount = ExecutionCount;
			if (conds.CanMap(this, nameof(ErrorCount)))
				target.ErrorCount = ErrorCount;
			if (conds.CanMap(this, nameof(AverageDuration)))
				target.AverageDuration = AverageDuration;
		}
		else
		{
			target.IdJobStatistics = IdJobStatistics;
			target.IdJob = IdJob;
			target.StartHourUtc = StartHourUtc;
			target.ExecutionCount = ExecutionCount;
			target.ErrorCount = ErrorCount;
			target.AverageDuration = AverageDuration;
		}

		cache.Add(this, target);

		if (referenceModifier == ReferenceModifier.MapAllReferences)
		{
			target.Job = Job?.MapTo(target.Job, referenceModifier, conds?.GetConditions(x => x.Job), instanceFactory, cache)!;
		}
		else if (referenceModifier == ReferenceModifier.SetNull)
		{
			target.Job = null!;
		}

		return target;
	}
}
