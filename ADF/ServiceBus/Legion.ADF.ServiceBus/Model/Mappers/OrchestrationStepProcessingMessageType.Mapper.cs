using Legion;
using Legion.Model.Mappers;

namespace Legion.ADF.ServiceBus.Model;

public sealed partial class OrchestrationStepProcessingMessageType : ServiceBus.ServiceBusBaseEntity, Legion.Model.IEntity
{
	public static ServiceBus.Model.OrchestrationStepProcessingMessageType? Map(
		ServiceBus.Model.OrchestrationStepProcessingMessageType source,
		ServiceBus.Model.OrchestrationStepProcessingMessageType? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<ServiceBus.Model.OrchestrationStepProcessingMessageType>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		Throw.IfArgumentNull(source);

		return source.MapTo(target, referenceModifier, conditions, instanceFactory, cache);
	}

	public ServiceBus.Model.OrchestrationStepProcessingMessageType? Clone(
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<ServiceBus.Model.OrchestrationStepProcessingMessageType>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
		=> MapTo(target: null, referenceModifier, conditions, instanceFactory, cache);

	public ServiceBus.Model.OrchestrationStepProcessingMessageType? MapTo(
		ServiceBus.Model.OrchestrationStepProcessingMessageType? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<ServiceBus.Model.OrchestrationStepProcessingMessageType>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		cache ??= [];
		target ??= instanceFactory?.CreateInstance<Legion.ADF.ServiceBus.Model.OrchestrationStepProcessingMessageType>(
				useActivatorIfNoFactoryFound: false,
				throwIfFactoryReturnsNull: true)
			?? new Legion.ADF.ServiceBus.Model.OrchestrationStepProcessingMessageType();

		if (cache.TryGetValue(this, out var cached))
			return (ServiceBus.Model.OrchestrationStepProcessingMessageType)cached;
			
		MappingConditions<ServiceBus.Model.OrchestrationStepProcessingMessageType>? conds = null;
		if (conditions != null)
		{
			conds = new MappingConditions<ServiceBus.Model.OrchestrationStepProcessingMessageType>();
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
			target._orchestrationStepProcessingMessages = MapperHelper.MapToList(OrchestrationStepProcessingMessages, target._orchestrationStepProcessingMessages, Legion.ADF.ServiceBus.Model.OrchestrationStepProcessingMessage.Map, referenceModifier, conds?.GetConditions(x => x.OrchestrationStepProcessingMessages), instanceFactory, cache)!;
		}
		else if (referenceModifier == ReferenceModifier.SetNull)
		{
			target._orchestrationStepProcessingMessages = [];
		}

		return target;
	}
}
