using Legion;
using Legion.Model.Mappers;

namespace Legion.ADF.ServiceBus.Model;

public sealed partial class Orchestration : ServiceBus.ServiceBusBaseEntity, Legion.Model.IEntity
{
	public static ServiceBus.Model.Orchestration? Map(
		ServiceBus.Model.Orchestration source,
		ServiceBus.Model.Orchestration? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<ServiceBus.Model.Orchestration>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		Throw.IfArgumentNull(source);

		return source.MapTo(target, referenceModifier, conditions, instanceFactory, cache);
	}

	public ServiceBus.Model.Orchestration? Clone(
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<ServiceBus.Model.Orchestration>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
		=> MapTo(target: null, referenceModifier, conditions, instanceFactory, cache);

	public ServiceBus.Model.Orchestration? MapTo(
		ServiceBus.Model.Orchestration? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<ServiceBus.Model.Orchestration>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		cache ??= [];
		target ??= instanceFactory?.CreateInstance<Legion.ADF.ServiceBus.Model.Orchestration>(
				useActivatorIfNoFactoryFound: false,
				throwIfFactoryReturnsNull: true)
			?? new Legion.ADF.ServiceBus.Model.Orchestration();

		if (cache.TryGetValue(this, out var cached))
			return (ServiceBus.Model.Orchestration)cached;
			
		MappingConditions<ServiceBus.Model.Orchestration>? conds = null;
		if (conditions != null)
		{
			conds = new MappingConditions<ServiceBus.Model.Orchestration>();
			conditions.Invoke(conds);

			if (conds.CanMap(this, nameof(IdOrchestration)))
				target.IdOrchestration = IdOrchestration;
			if (conds.CanMap(this, nameof(Name)))
				target.Name = Name;
			if (conds.CanMap(this, nameof(Description)))
				target.Description = Description;
			if (conds.CanMap(this, nameof(IsSingleton)))
				target.IsSingleton = IsSingleton;
			if (conds.CanMap(this, nameof(Namespace)))
				target.Namespace = Namespace;
			if (conds.CanMap(this, nameof(Version)))
				target.Version = Version;
			if (conds.CanMap(this, nameof(Properties)))
				target.Properties = Properties;
		}
		else
		{
			target.IdOrchestration = IdOrchestration;
			target.Name = Name;
			target.Description = Description;
			target.IsSingleton = IsSingleton;
			target.Namespace = Namespace;
			target.Version = Version;
			target.Properties = Properties;
		}

		cache.Add(this, target);

		if (referenceModifier == ReferenceModifier.MapAllReferences)
		{
			target._orchestrationInstances = MapperHelper.MapToList(OrchestrationInstances, target._orchestrationInstances, Legion.ADF.ServiceBus.Model.OrchestrationInstance.Map, referenceModifier, conds?.GetConditions(x => x.OrchestrationInstances), instanceFactory, cache)!;
			target._orchestrationSteps = MapperHelper.MapToList(OrchestrationSteps, target._orchestrationSteps, Legion.ADF.ServiceBus.Model.OrchestrationStep.Map, referenceModifier, conds?.GetConditions(x => x.OrchestrationSteps), instanceFactory, cache)!;
		}
		else if (referenceModifier == ReferenceModifier.SetNull)
		{
			target._orchestrationInstances = [];
			target._orchestrationSteps = [];
		}

		return target;
	}
}
