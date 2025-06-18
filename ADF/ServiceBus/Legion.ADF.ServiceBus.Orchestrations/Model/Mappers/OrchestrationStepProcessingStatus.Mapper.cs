using Legion;
using Legion.Model.Mappers;

namespace Legion.ADF.ServiceBus.Orchestrations.Model;

public sealed partial class OrchestrationStepProcessingStatus : Orchestrations.OrchestrationsBaseEntity, Legion.Model.IEntity
{
	public static Orchestrations.Model.OrchestrationStepProcessingStatus? Map(
		Orchestrations.Model.OrchestrationStepProcessingStatus source,
		Orchestrations.Model.OrchestrationStepProcessingStatus? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Orchestrations.Model.OrchestrationStepProcessingStatus>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		Throw.IfArgumentNull(source);

		return source.MapTo(target, referenceModifier, conditions, instanceFactory, cache);
	}

	public Orchestrations.Model.OrchestrationStepProcessingStatus? Clone(
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Orchestrations.Model.OrchestrationStepProcessingStatus>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
		=> MapTo(target: null, referenceModifier, conditions, instanceFactory, cache);

	public Orchestrations.Model.OrchestrationStepProcessingStatus? MapTo(
		Orchestrations.Model.OrchestrationStepProcessingStatus? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Orchestrations.Model.OrchestrationStepProcessingStatus>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		cache ??= [];
		target ??= instanceFactory?.CreateInstance<Legion.ADF.ServiceBus.Orchestrations.Model.OrchestrationStepProcessingStatus>(
				useActivatorIfNoFactoryFound: false,
				throwIfFactoryReturnsNull: true)
			?? new Legion.ADF.ServiceBus.Orchestrations.Model.OrchestrationStepProcessingStatus();

		if (cache.TryGetValue(this, out var cached))
			return (Orchestrations.Model.OrchestrationStepProcessingStatus)cached;
			
		MappingConditions<Orchestrations.Model.OrchestrationStepProcessingStatus>? conds = null;
		if (conditions != null)
		{
			conds = new MappingConditions<Orchestrations.Model.OrchestrationStepProcessingStatus>();
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
			target._orchestrationStepProcessingLogs = MapperHelper.MapToList(OrchestrationStepProcessingLogs, target._orchestrationStepProcessingLogs, Legion.ADF.ServiceBus.Orchestrations.Model.OrchestrationStepProcessingLog.Map, referenceModifier, conds?.GetConditions(x => x.OrchestrationStepProcessingLogs), instanceFactory, cache)!;
			target._orchestrationStepProcessings = MapperHelper.MapToList(OrchestrationStepProcessings, target._orchestrationStepProcessings, Legion.ADF.ServiceBus.Orchestrations.Model.OrchestrationStepProcessing.Map, referenceModifier, conds?.GetConditions(x => x.OrchestrationStepProcessings), instanceFactory, cache)!;
		}
		else if (referenceModifier == ReferenceModifier.SetNull)
		{
			target._orchestrationStepProcessingLogs = [];
			target._orchestrationStepProcessings = [];
		}

		return target;
	}
}
