using Legion;
using Legion.Model.Mappers;

namespace Legion.ADF.ServiceBus.Jobs.Model;

public sealed partial class JobMessage : Jobs.JobsBaseEntity, Legion.Model.IEntity
{
	public static Jobs.Model.JobMessage? Map(
		Jobs.Model.JobMessage source,
		Jobs.Model.JobMessage? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Jobs.Model.JobMessage>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		Throw.IfArgumentNull(source);

		return source.MapTo(target, referenceModifier, conditions, instanceFactory, cache);
	}

	public Jobs.Model.JobMessage? Clone(
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Jobs.Model.JobMessage>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
		=> MapTo(target: null, referenceModifier, conditions, instanceFactory, cache);

	public Jobs.Model.JobMessage? MapTo(
		Jobs.Model.JobMessage? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Jobs.Model.JobMessage>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		cache ??= [];
		target ??= instanceFactory?.CreateInstance<Legion.ADF.ServiceBus.Jobs.Model.JobMessage>(
				useActivatorIfNoFactoryFound: false,
				throwIfFactoryReturnsNull: true)
			?? new Legion.ADF.ServiceBus.Jobs.Model.JobMessage();

		if (cache.TryGetValue(this, out var cached))
			return (Jobs.Model.JobMessage)cached;
			
		MappingConditions<Jobs.Model.JobMessage>? conds = null;
		if (conditions != null)
		{
			conds = new MappingConditions<Jobs.Model.JobMessage>();
			conditions.Invoke(conds);

			if (conds.CanMap(this, nameof(IdJobMessage)))
				target.IdJobMessage = IdJobMessage;
			if (conds.CanMap(this, nameof(IdJob)))
				target.IdJob = IdJob;
			if (conds.CanMap(this, nameof(IdMessage)))
				target.IdMessage = IdMessage;
			if (conds.CanMap(this, nameof(IdJobMessageType)))
				target.IdJobMessageType = IdJobMessageType;
			if (conds.CanMap(this, nameof(CreatedUtc)))
				target.CreatedUtc = CreatedUtc;
		}
		else
		{
			target.IdJobMessage = IdJobMessage;
			target.IdJob = IdJob;
			target.IdMessage = IdMessage;
			target.IdJobMessageType = IdJobMessageType;
			target.CreatedUtc = CreatedUtc;
		}

		cache.Add(this, target);

		if (referenceModifier == ReferenceModifier.MapAllReferences)
		{
			target.Job = Job?.MapTo(target.Job, referenceModifier, conds?.GetConditions(x => x.Job), instanceFactory, cache)!;
			target.JobMessageType = JobMessageType?.MapTo(target.JobMessageType, referenceModifier, conds?.GetConditions(x => x.JobMessageType), instanceFactory, cache)!;
		}
		else if (referenceModifier == ReferenceModifier.SetNull)
		{
			target.Job = null!;
			target.JobMessageType = null!;
		}

		return target;
	}
}
