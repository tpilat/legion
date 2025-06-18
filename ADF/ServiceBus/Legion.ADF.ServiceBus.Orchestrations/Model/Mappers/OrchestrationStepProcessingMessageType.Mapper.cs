using Legion;
using Legion.Model.Mappers;

namespace Legion.ADF.ServiceBus.Orchestrations.Model;

public sealed partial class OrchestrationStepProcessingMessageType : Orchestrations.OrchestrationsBaseEntity, Legion.Model.IEntity
{
	public static Orchestrations.Model.OrchestrationStepProcessingMessageType? Map(
		Orchestrations.Model.OrchestrationStepProcessingMessageType source,
		Orchestrations.Model.OrchestrationStepProcessingMessageType? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Orchestrations.Model.OrchestrationStepProcessingMessageType>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		Throw.IfArgumentNull(source);

		return source.MapTo(target, referenceModifier, conditions, instanceFactory, cache);
	}

	public Orchestrations.Model.OrchestrationStepProcessingMessageType? Clone(
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Orchestrations.Model.OrchestrationStepProcessingMessageType>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
		=> MapTo(target: null, referenceModifier, conditions, instanceFactory, cache);

	public Orchestrations.Model.OrchestrationStepProcessingMessageType? MapTo(
		Orchestrations.Model.OrchestrationStepProcessingMessageType? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Orchestrations.Model.OrchestrationStepProcessingMessageType>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		cache ??= [];
		target ??= instanceFactory?.CreateInstance<Legion.ADF.ServiceBus.Orchestrations.Model.OrchestrationStepProcessingMessageType>(
				useActivatorIfNoFactoryFound: false,
				throwIfFactoryReturnsNull: true)
			?? new Legion.ADF.ServiceBus.Orchestrations.Model.OrchestrationStepProcessingMessageType();

		if (cache.TryGetValue(this, out var cached))
			return (Orchestrations.Model.OrchestrationStepProcessingMessageType)cached;
			
		MappingConditions<Orchestrations.Model.OrchestrationStepProcessingMessageType>? conds = null;
		if (conditions != null)
		{
			conds = new MappingConditions<Orchestrations.Model.OrchestrationStepProcessingMessageType>();
			conditions.Invoke(conds);

			if (conds.CanMap(this, nameof(IdOrchestrationStepProcessingMessageType)))
				target.IdOrchestrationStepProcessingMessageType = IdOrchestrationStepProcessingMessageType;
			if (conds.CanMap(this, nameof(Code)))
				target.Code = Code;
			if (conds.CanMap(this, nameof(Name)))
				target.Name = Name;
		}
		else
		{
			target.IdOrchestrationStepProcessingMessageType = IdOrchestrationStepProcessingMessageType;
			target.Code = Code;
			target.Name = Name;
		}

		cache.Add(this, target);

		if (referenceModifier == ReferenceModifier.MapAllReferences)
		{
			target._orchestrationStepProcessingMessages = MapperHelper.MapToList(OrchestrationStepProcessingMessages, target._orchestrationStepProcessingMessages, Legion.ADF.ServiceBus.Orchestrations.Model.OrchestrationStepProcessingMessage.Map, referenceModifier, conds?.GetConditions(x => x.OrchestrationStepProcessingMessages), instanceFactory, cache)!;
		}
		else if (referenceModifier == ReferenceModifier.SetNull)
		{
			target._orchestrationStepProcessingMessages = [];
		}

		return target;
	}
}
