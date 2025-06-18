using Legion;
using Legion.Model.Mappers;

namespace Legion.ADF.ServiceBus.Orchestrations.Model;

public sealed partial class OrchestrationStepProcessingDirection : Orchestrations.OrchestrationsBaseEntity, Legion.Model.IEntity
{
	public static Orchestrations.Model.OrchestrationStepProcessingDirection? Map(
		Orchestrations.Model.OrchestrationStepProcessingDirection source,
		Orchestrations.Model.OrchestrationStepProcessingDirection? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Orchestrations.Model.OrchestrationStepProcessingDirection>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		Throw.IfArgumentNull(source);

		return source.MapTo(target, referenceModifier, conditions, instanceFactory, cache);
	}

	public Orchestrations.Model.OrchestrationStepProcessingDirection? Clone(
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Orchestrations.Model.OrchestrationStepProcessingDirection>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
		=> MapTo(target: null, referenceModifier, conditions, instanceFactory, cache);

	public Orchestrations.Model.OrchestrationStepProcessingDirection? MapTo(
		Orchestrations.Model.OrchestrationStepProcessingDirection? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Orchestrations.Model.OrchestrationStepProcessingDirection>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		cache ??= [];
		target ??= instanceFactory?.CreateInstance<Legion.ADF.ServiceBus.Orchestrations.Model.OrchestrationStepProcessingDirection>(
				useActivatorIfNoFactoryFound: false,
				throwIfFactoryReturnsNull: true)
			?? new Legion.ADF.ServiceBus.Orchestrations.Model.OrchestrationStepProcessingDirection();

		if (cache.TryGetValue(this, out var cached))
			return (Orchestrations.Model.OrchestrationStepProcessingDirection)cached;
			
		MappingConditions<Orchestrations.Model.OrchestrationStepProcessingDirection>? conds = null;
		if (conditions != null)
		{
			conds = new MappingConditions<Orchestrations.Model.OrchestrationStepProcessingDirection>();
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
