using Legion;
using Legion.Model.Mappers;

namespace Legion.ADF.ServiceBus.Model;

public sealed partial class OrchestrationStepProcessingDirection : ServiceBus.ServiceBusBaseEntity, Legion.Model.IEntity
{
	public static ServiceBus.Model.OrchestrationStepProcessingDirection? Map(
		ServiceBus.Model.OrchestrationStepProcessingDirection source,
		ServiceBus.Model.OrchestrationStepProcessingDirection? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<ServiceBus.Model.OrchestrationStepProcessingDirection>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		Throw.IfArgumentNull(source);

		return source.MapTo(target, referenceModifier, conditions, instanceFactory, cache);
	}

	public ServiceBus.Model.OrchestrationStepProcessingDirection? Clone(
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<ServiceBus.Model.OrchestrationStepProcessingDirection>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
		=> MapTo(target: null, referenceModifier, conditions, instanceFactory, cache);

	public ServiceBus.Model.OrchestrationStepProcessingDirection? MapTo(
		ServiceBus.Model.OrchestrationStepProcessingDirection? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<ServiceBus.Model.OrchestrationStepProcessingDirection>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		cache ??= [];
		target ??= instanceFactory?.CreateInstance<Legion.ADF.ServiceBus.Model.OrchestrationStepProcessingDirection>(
				useActivatorIfNoFactoryFound: false,
				throwIfFactoryReturnsNull: true)
			?? new Legion.ADF.ServiceBus.Model.OrchestrationStepProcessingDirection();

		if (cache.TryGetValue(this, out var cached))
			return (ServiceBus.Model.OrchestrationStepProcessingDirection)cached;
			
		MappingConditions<ServiceBus.Model.OrchestrationStepProcessingDirection>? conds = null;
		if (conditions != null)
		{
			conds = new MappingConditions<ServiceBus.Model.OrchestrationStepProcessingDirection>();
			conditions.Invoke(conds);

			if (conds.CanMap(this, nameof(IdOrchestrationStepProcessingDirection)))
				target.IdOrchestrationStepProcessingDirection = IdOrchestrationStepProcessingDirection;
			if (conds.CanMap(this, nameof(IdFromStep)))
				target.IdFromStep = IdFromStep;
			if (conds.CanMap(this, nameof(IdToStep)))
				target.IdToStep = IdToStep;
			if (conds.CanMap(this, nameof(CreatedUtc)))
				target.CreatedUtc = CreatedUtc;
		}
		else
		{
			target.IdOrchestrationStepProcessingDirection = IdOrchestrationStepProcessingDirection;
			target.IdFromStep = IdFromStep;
			target.IdToStep = IdToStep;
			target.CreatedUtc = CreatedUtc;
		}

		cache.Add(this, target);

		if (referenceModifier == ReferenceModifier.MapAllReferences)
		{
			target.FromStep = FromStep?.MapTo(target.FromStep, referenceModifier, conds?.GetConditions(x => x.FromStep), instanceFactory, cache)!;
			target.ToStep = ToStep?.MapTo(target.ToStep, referenceModifier, conds?.GetConditions(x => x.ToStep), instanceFactory, cache)!;
		}
		else if (referenceModifier == ReferenceModifier.SetNull)
		{
			target.FromStep = null!;
			target.ToStep = null!;
		}

		return target;
	}
}
