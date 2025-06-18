using Legion;
using Legion.Model.Mappers;

namespace Legion.ADF.ServiceBus.Orchestrations.Model;

public sealed partial class OrchestrationStep : Orchestrations.OrchestrationsBaseEntity, Legion.Model.IEntity
{
	public static Orchestrations.Model.OrchestrationStep? Map(
		Orchestrations.Model.OrchestrationStep source,
		Orchestrations.Model.OrchestrationStep? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Orchestrations.Model.OrchestrationStep>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		Throw.IfArgumentNull(source);

		return source.MapTo(target, referenceModifier, conditions, instanceFactory, cache);
	}

	public Orchestrations.Model.OrchestrationStep? Clone(
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Orchestrations.Model.OrchestrationStep>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
		=> MapTo(target: null, referenceModifier, conditions, instanceFactory, cache);

	public Orchestrations.Model.OrchestrationStep? MapTo(
		Orchestrations.Model.OrchestrationStep? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Orchestrations.Model.OrchestrationStep>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		cache ??= [];
		target ??= instanceFactory?.CreateInstance<Legion.ADF.ServiceBus.Orchestrations.Model.OrchestrationStep>(
				useActivatorIfNoFactoryFound: false,
				throwIfFactoryReturnsNull: true)
			?? new Legion.ADF.ServiceBus.Orchestrations.Model.OrchestrationStep();

		if (cache.TryGetValue(this, out var cached))
			return (Orchestrations.Model.OrchestrationStep)cached;
			
		MappingConditions<Orchestrations.Model.OrchestrationStep>? conds = null;
		if (conditions != null)
		{
			conds = new MappingConditions<Orchestrations.Model.OrchestrationStep>();
			conditions.Invoke(conds);

			if (conds.CanMap(this, nameof(IdOrchestrationStep)))
				target.IdOrchestrationStep = IdOrchestrationStep;
			if (conds.CanMap(this, nameof(IdOrchestration)))
				target.IdOrchestration = IdOrchestration;
			if (conds.CanMap(this, nameof(IsMainEntry)))
				target.IsMainEntry = IsMainEntry;
			if (conds.CanMap(this, nameof(Order)))
				target.Order = Order;
			if (conds.CanMap(this, nameof(Name)))
				target.Name = Name;
			if (conds.CanMap(this, nameof(Description)))
				target.Description = Description;
			if (conds.CanMap(this, nameof(Namespace)))
				target.Namespace = Namespace;
			if (conds.CanMap(this, nameof(Properties)))
				target.Properties = Properties;
			if (conds.CanMap(this, nameof(TimeoutForMessageProcessingInSeconds)))
				target.TimeoutForMessageProcessingInSeconds = TimeoutForMessageProcessingInSeconds;
			if (conds.CanMap(this, nameof(MaxMessageProcessingRetryCount)))
				target.MaxMessageProcessingRetryCount = MaxMessageProcessingRetryCount;
		}
		else
		{
			target.IdOrchestrationStep = IdOrchestrationStep;
			target.IdOrchestration = IdOrchestration;
			target.IsMainEntry = IsMainEntry;
			target.Order = Order;
			target.Name = Name;
			target.Description = Description;
			target.Namespace = Namespace;
			target.Properties = Properties;
			target.TimeoutForMessageProcessingInSeconds = TimeoutForMessageProcessingInSeconds;
			target.MaxMessageProcessingRetryCount = MaxMessageProcessingRetryCount;
		}

		cache.Add(this, target);

		if (referenceModifier == ReferenceModifier.MapAllReferences)
		{
			target.Orchestration = Orchestration?.MapTo(target.Orchestration, referenceModifier, conds?.GetConditions(x => x.Orchestration), instanceFactory, cache)!;
			target._orchestrationStepProcessings = MapperHelper.MapToList(OrchestrationStepProcessings, target._orchestrationStepProcessings, Legion.ADF.ServiceBus.Orchestrations.Model.OrchestrationStepProcessing.Map, referenceModifier, conds?.GetConditions(x => x.OrchestrationStepProcessings), instanceFactory, cache)!;
		}
		else if (referenceModifier == ReferenceModifier.SetNull)
		{
			target.Orchestration = null!;
			target._orchestrationStepProcessings = [];
		}

		return target;
	}
}
