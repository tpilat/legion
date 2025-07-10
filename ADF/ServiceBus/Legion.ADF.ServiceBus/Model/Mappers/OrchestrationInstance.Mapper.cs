using Legion;
using Legion.Model.Mappers;

namespace Legion.ADF.ServiceBus.Model;

public sealed partial class OrchestrationInstance : ServiceBus.ServiceBusBaseEntity, Legion.Model.IEntity
{
	public static ServiceBus.Model.OrchestrationInstance? Map(
		ServiceBus.Model.OrchestrationInstance source,
		ServiceBus.Model.OrchestrationInstance? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<ServiceBus.Model.OrchestrationInstance>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		Throw.IfArgumentNull(source);

		return source.MapTo(target, referenceModifier, conditions, instanceFactory, cache);
	}

	public ServiceBus.Model.OrchestrationInstance? Clone(
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<ServiceBus.Model.OrchestrationInstance>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
		=> MapTo(target: null, referenceModifier, conditions, instanceFactory, cache);

	public ServiceBus.Model.OrchestrationInstance? MapTo(
		ServiceBus.Model.OrchestrationInstance? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<ServiceBus.Model.OrchestrationInstance>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		cache ??= [];
		target ??= instanceFactory?.CreateInstance<Legion.ADF.ServiceBus.Model.OrchestrationInstance>(
				useActivatorIfNoFactoryFound: false,
				throwIfFactoryReturnsNull: true)
			?? new Legion.ADF.ServiceBus.Model.OrchestrationInstance();

		if (cache.TryGetValue(this, out var cached))
			return (ServiceBus.Model.OrchestrationInstance)cached;
			
		MappingConditions<ServiceBus.Model.OrchestrationInstance>? conds = null;
		if (conditions != null)
		{
			conds = new MappingConditions<ServiceBus.Model.OrchestrationInstance>();
			conditions.Invoke(conds);

			if (conds.CanMap(this, nameof(IdOrchestrationInstance)))
				target.IdOrchestrationInstance = IdOrchestrationInstance;
			if (conds.CanMap(this, nameof(IdOrchestration)))
				target.IdOrchestration = IdOrchestration;
			if (conds.CanMap(this, nameof(IdOrchestrationStatus)))
				target.IdOrchestrationStatus = IdOrchestrationStatus;
			if (conds.CanMap(this, nameof(CreatedUtc)))
				target.CreatedUtc = CreatedUtc;
		}
		else
		{
			target.IdOrchestrationInstance = IdOrchestrationInstance;
			target.IdOrchestration = IdOrchestration;
			target.IdOrchestrationStatus = IdOrchestrationStatus;
			target.CreatedUtc = CreatedUtc;
		}

		cache.Add(this, target);

		if (referenceModifier == ReferenceModifier.MapAllReferences)
		{
			target.Orchestration = Orchestration?.MapTo(target.Orchestration, referenceModifier, conds?.GetConditions(x => x.Orchestration), instanceFactory, cache)!;
			target.OrchestrationStatus = OrchestrationStatus?.MapTo(target.OrchestrationStatus, referenceModifier, conds?.GetConditions(x => x.OrchestrationStatus), instanceFactory, cache)!;
			target._orchestrationStepProcessings = MapperHelper.MapToList(OrchestrationStepProcessings, target._orchestrationStepProcessings, Legion.ADF.ServiceBus.Model.OrchestrationStepProcessing.Map, referenceModifier, conds?.GetConditions(x => x.OrchestrationStepProcessings), instanceFactory, cache)!;
		}
		else if (referenceModifier == ReferenceModifier.SetNull)
		{
			target.Orchestration = null!;
			target.OrchestrationStatus = null!;
			target._orchestrationStepProcessings = [];
		}

		return target;
	}
}
