using Legion;
using Legion.Model.Mappers;

namespace Legion.ADF.ServiceBus.Model;

public sealed partial class JobStatistics : ServiceBus.ServiceBusBaseEntity, Legion.Model.IEntity
{
	public static ServiceBus.Model.JobStatistics? Map(
		ServiceBus.Model.JobStatistics source,
		ServiceBus.Model.JobStatistics? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<ServiceBus.Model.JobStatistics>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		Throw.IfArgumentNull(source);

		return source.MapTo(target, referenceModifier, conditions, instanceFactory, cache);
	}

	public ServiceBus.Model.JobStatistics? Clone(
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<ServiceBus.Model.JobStatistics>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
		=> MapTo(target: null, referenceModifier, conditions, instanceFactory, cache);

	public ServiceBus.Model.JobStatistics? MapTo(
		ServiceBus.Model.JobStatistics? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<ServiceBus.Model.JobStatistics>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		cache ??= [];
		target ??= instanceFactory?.CreateInstance<Legion.ADF.ServiceBus.Model.JobStatistics>(
				useActivatorIfNoFactoryFound: false,
				throwIfFactoryReturnsNull: true)
			?? new Legion.ADF.ServiceBus.Model.JobStatistics();

		if (cache.TryGetValue(this, out var cached))
			return (ServiceBus.Model.JobStatistics)cached;
			
		MappingConditions<ServiceBus.Model.JobStatistics>? conds = null;
		if (conditions != null)
		{
			conds = new MappingConditions<ServiceBus.Model.JobStatistics>();
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
