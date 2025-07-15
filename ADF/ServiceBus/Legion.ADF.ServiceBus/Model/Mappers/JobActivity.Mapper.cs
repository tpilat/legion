using Legion;
using Legion.Model.Mappers;

namespace Legion.ADF.ServiceBus.Model;

public sealed partial class JobActivity : ServiceBus.ServiceBusBaseEntity, Legion.Model.Concurrence.IConcurrent, Legion.Model.IEntity
{
	public static ServiceBus.Model.JobActivity? Map(
		ServiceBus.Model.JobActivity source,
		ServiceBus.Model.JobActivity? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<ServiceBus.Model.JobActivity>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		Throw.IfArgumentNull(source);

		return source.MapTo(target, referenceModifier, conditions, instanceFactory, cache);
	}

	public ServiceBus.Model.JobActivity? Clone(
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<ServiceBus.Model.JobActivity>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
		=> MapTo(target: null, referenceModifier, conditions, instanceFactory, cache);

	public ServiceBus.Model.JobActivity? MapTo(
		ServiceBus.Model.JobActivity? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<ServiceBus.Model.JobActivity>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		cache ??= [];
		target ??= instanceFactory?.CreateInstance<Legion.ADF.ServiceBus.Model.JobActivity>(
				useActivatorIfNoFactoryFound: false,
				throwIfFactoryReturnsNull: true)
			?? new Legion.ADF.ServiceBus.Model.JobActivity();

		if (cache.TryGetValue(this, out var cached))
			return (ServiceBus.Model.JobActivity)cached;
			
		MappingConditions<ServiceBus.Model.JobActivity>? conds = null;
		if (conditions != null)
		{
			conds = new MappingConditions<ServiceBus.Model.JobActivity>();
			conditions.Invoke(conds);

			if (conds.CanMap(this, nameof(IdJobActivity)))
				target.IdJobActivity = IdJobActivity;
			if (conds.CanMap(this, nameof(IdJob)))
				target.IdJob = IdJob;
			if (conds.CanMap(this, nameof(IdJobStatus)))
				target.IdJobStatus = IdJobStatus;
			if (conds.CanMap(this, nameof(IdCurrentHost)))
				target.IdCurrentHost = IdCurrentHost;
			if (conds.CanMap(this, nameof(AttachedToCurrentHostUtc)))
				target.AttachedToCurrentHostUtc = AttachedToCurrentHostUtc;
			if (conds.CanMap(this, nameof(LastStatusChangedUtc)))
				target.LastStatusChangedUtc = LastStatusChangedUtc;
			if (conds.CanMap(this, nameof(LastProcessingStartedUtc)))
				target.LastProcessingStartedUtc = LastProcessingStartedUtc;
			if (conds.CanMap(this, nameof(LastProcessingFinishedUtc)))
				target.LastProcessingFinishedUtc = LastProcessingFinishedUtc;
			if (conds.CanMap(this, nameof(DelayedToUtc)))
				target.DelayedToUtc = DelayedToUtc;
			if (conds.CanMap(this, nameof(RowVersion)))
				target.RowVersion = RowVersion;
		}
		else
		{
			target.IdJobActivity = IdJobActivity;
			target.IdJob = IdJob;
			target.IdJobStatus = IdJobStatus;
			target.IdCurrentHost = IdCurrentHost;
			target.AttachedToCurrentHostUtc = AttachedToCurrentHostUtc;
			target.LastStatusChangedUtc = LastStatusChangedUtc;
			target.LastProcessingStartedUtc = LastProcessingStartedUtc;
			target.LastProcessingFinishedUtc = LastProcessingFinishedUtc;
			target.DelayedToUtc = DelayedToUtc;
			target.RowVersion = RowVersion;
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
