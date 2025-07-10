using Legion;
using Legion.Model.Mappers;

namespace Legion.ADF.ServiceBus.Model;

public sealed partial class OrchestrationStepProcessingMessage : ServiceBus.ServiceBusBaseEntity, Legion.Model.IEntity
{
	public static ServiceBus.Model.OrchestrationStepProcessingMessage? Map(
		ServiceBus.Model.OrchestrationStepProcessingMessage source,
		ServiceBus.Model.OrchestrationStepProcessingMessage? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<ServiceBus.Model.OrchestrationStepProcessingMessage>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		Throw.IfArgumentNull(source);

		return source.MapTo(target, referenceModifier, conditions, instanceFactory, cache);
	}

	public ServiceBus.Model.OrchestrationStepProcessingMessage? Clone(
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<ServiceBus.Model.OrchestrationStepProcessingMessage>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
		=> MapTo(target: null, referenceModifier, conditions, instanceFactory, cache);

	public ServiceBus.Model.OrchestrationStepProcessingMessage? MapTo(
		ServiceBus.Model.OrchestrationStepProcessingMessage? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<ServiceBus.Model.OrchestrationStepProcessingMessage>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		cache ??= [];
		target ??= instanceFactory?.CreateInstance<Legion.ADF.ServiceBus.Model.OrchestrationStepProcessingMessage>(
				useActivatorIfNoFactoryFound: false,
				throwIfFactoryReturnsNull: true)
			?? new Legion.ADF.ServiceBus.Model.OrchestrationStepProcessingMessage();

		if (cache.TryGetValue(this, out var cached))
			return (ServiceBus.Model.OrchestrationStepProcessingMessage)cached;
			
		MappingConditions<ServiceBus.Model.OrchestrationStepProcessingMessage>? conds = null;
		if (conditions != null)
		{
			conds = new MappingConditions<ServiceBus.Model.OrchestrationStepProcessingMessage>();
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
