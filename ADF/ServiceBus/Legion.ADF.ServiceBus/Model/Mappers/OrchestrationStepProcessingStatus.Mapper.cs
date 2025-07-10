using Legion;
using Legion.Model.Mappers;

namespace Legion.ADF.ServiceBus.Model;

public sealed partial class OrchestrationStepProcessingStatus : ServiceBus.ServiceBusBaseEntity, Legion.Model.IEntity
{
	public static ServiceBus.Model.OrchestrationStepProcessingStatus? Map(
		ServiceBus.Model.OrchestrationStepProcessingStatus source,
		ServiceBus.Model.OrchestrationStepProcessingStatus? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<ServiceBus.Model.OrchestrationStepProcessingStatus>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		Throw.IfArgumentNull(source);

		return source.MapTo(target, referenceModifier, conditions, instanceFactory, cache);
	}

	public ServiceBus.Model.OrchestrationStepProcessingStatus? Clone(
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<ServiceBus.Model.OrchestrationStepProcessingStatus>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
		=> MapTo(target: null, referenceModifier, conditions, instanceFactory, cache);

	public ServiceBus.Model.OrchestrationStepProcessingStatus? MapTo(
		ServiceBus.Model.OrchestrationStepProcessingStatus? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<ServiceBus.Model.OrchestrationStepProcessingStatus>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		cache ??= [];
		target ??= instanceFactory?.CreateInstance<Legion.ADF.ServiceBus.Model.OrchestrationStepProcessingStatus>(
				useActivatorIfNoFactoryFound: false,
				throwIfFactoryReturnsNull: true)
			?? new Legion.ADF.ServiceBus.Model.OrchestrationStepProcessingStatus();

		if (cache.TryGetValue(this, out var cached))
			return (ServiceBus.Model.OrchestrationStepProcessingStatus)cached;
			
		MappingConditions<ServiceBus.Model.OrchestrationStepProcessingStatus>? conds = null;
		if (conditions != null)
		{
			conds = new MappingConditions<ServiceBus.Model.OrchestrationStepProcessingStatus>();
			conditions.Invoke(conds);

			if (conds.CanMap(this, nameof(IdOrchestrationStepProcessingStatus)))
				target.IdOrchestrationStepProcessingStatus = IdOrchestrationStepProcessingStatus;
			if (conds.CanMap(this, nameof(Code)))
				target.Code = Code;
			if (conds.CanMap(this, nameof(Name)))
				target.Name = Name;
		}
		else
		{
			target.IdOrchestrationStepProcessingStatus = IdOrchestrationStepProcessingStatus;
			target.Code = Code;
			target.Name = Name;
		}

		cache.Add(this, target);

		if (referenceModifier == ReferenceModifier.MapAllReferences)
		{
			target._orchestrationStepProcessingLogs = MapperHelper.MapToList(OrchestrationStepProcessingLogs, target._orchestrationStepProcessingLogs, Legion.ADF.ServiceBus.Model.OrchestrationStepProcessingLog.Map, referenceModifier, conds?.GetConditions(x => x.OrchestrationStepProcessingLogs), instanceFactory, cache)!;
			target._orchestrationStepProcessings = MapperHelper.MapToList(OrchestrationStepProcessings, target._orchestrationStepProcessings, Legion.ADF.ServiceBus.Model.OrchestrationStepProcessing.Map, referenceModifier, conds?.GetConditions(x => x.OrchestrationStepProcessings), instanceFactory, cache)!;
		}
		else if (referenceModifier == ReferenceModifier.SetNull)
		{
			target._orchestrationStepProcessingLogs = [];
			target._orchestrationStepProcessings = [];
		}

		return target;
	}
}
