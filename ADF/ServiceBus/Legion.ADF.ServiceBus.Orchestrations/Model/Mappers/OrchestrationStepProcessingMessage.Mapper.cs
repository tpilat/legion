using Legion;
using Legion.Model.Mappers;

namespace Legion.ADF.ServiceBus.Orchestrations.Model;

public sealed partial class OrchestrationStepProcessingMessage : Orchestrations.OrchestrationsBaseEntity, Legion.Model.IEntity
{
	public static Orchestrations.Model.OrchestrationStepProcessingMessage? Map(
		Orchestrations.Model.OrchestrationStepProcessingMessage source,
		Orchestrations.Model.OrchestrationStepProcessingMessage? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Orchestrations.Model.OrchestrationStepProcessingMessage>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		Throw.IfArgumentNull(source);

		return source.MapTo(target, referenceModifier, conditions, instanceFactory, cache);
	}

	public Orchestrations.Model.OrchestrationStepProcessingMessage? Clone(
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Orchestrations.Model.OrchestrationStepProcessingMessage>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
		=> MapTo(target: null, referenceModifier, conditions, instanceFactory, cache);

	public Orchestrations.Model.OrchestrationStepProcessingMessage? MapTo(
		Orchestrations.Model.OrchestrationStepProcessingMessage? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Orchestrations.Model.OrchestrationStepProcessingMessage>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		cache ??= [];
		target ??= instanceFactory?.CreateInstance<Legion.ADF.ServiceBus.Orchestrations.Model.OrchestrationStepProcessingMessage>(
				useActivatorIfNoFactoryFound: false,
				throwIfFactoryReturnsNull: true)
			?? new Legion.ADF.ServiceBus.Orchestrations.Model.OrchestrationStepProcessingMessage();

		if (cache.TryGetValue(this, out var cached))
			return (Orchestrations.Model.OrchestrationStepProcessingMessage)cached;
			
		MappingConditions<Orchestrations.Model.OrchestrationStepProcessingMessage>? conds = null;
		if (conditions != null)
		{
			conds = new MappingConditions<Orchestrations.Model.OrchestrationStepProcessingMessage>();
			conditions.Invoke(conds);

			if (conds.CanMap(this, nameof(IdOrchestrationStepProcessingMessage)))
				target.IdOrchestrationStepProcessingMessage = IdOrchestrationStepProcessingMessage;
			if (conds.CanMap(this, nameof(IdOrchestrationStepProcessing)))
				target.IdOrchestrationStepProcessing = IdOrchestrationStepProcessing;
			if (conds.CanMap(this, nameof(IdMessage)))
				target.IdMessage = IdMessage;
			if (conds.CanMap(this, nameof(IdOrchestrationStepProcessingMessageType)))
				target.IdOrchestrationStepProcessingMessageType = IdOrchestrationStepProcessingMessageType;
			if (conds.CanMap(this, nameof(CreatedUtc)))
				target.CreatedUtc = CreatedUtc;
		}
		else
		{
			target.IdOrchestrationStepProcessingMessage = IdOrchestrationStepProcessingMessage;
			target.IdOrchestrationStepProcessing = IdOrchestrationStepProcessing;
			target.IdMessage = IdMessage;
			target.IdOrchestrationStepProcessingMessageType = IdOrchestrationStepProcessingMessageType;
			target.CreatedUtc = CreatedUtc;
		}

		cache.Add(this, target);

		if (referenceModifier == ReferenceModifier.MapAllReferences)
		{
			target.OrchestrationStepProcessing = OrchestrationStepProcessing?.MapTo(target.OrchestrationStepProcessing, referenceModifier, conds?.GetConditions(x => x.OrchestrationStepProcessing), instanceFactory, cache)!;
			target.OrchestrationStepProcessingMessageType = OrchestrationStepProcessingMessageType?.MapTo(target.OrchestrationStepProcessingMessageType, referenceModifier, conds?.GetConditions(x => x.OrchestrationStepProcessingMessageType), instanceFactory, cache)!;
		}
		else if (referenceModifier == ReferenceModifier.SetNull)
		{
			target.OrchestrationStepProcessing = null!;
			target.OrchestrationStepProcessingMessageType = null!;
		}

		return target;
	}
}
