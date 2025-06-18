using Legion;
using Legion.Model.Mappers;

namespace Legion.ADF.ServiceBus.Orchestrations.Model;

public sealed partial class Orchestration : Orchestrations.OrchestrationsBaseEntity, Legion.Model.IEntity
{
	public static Orchestrations.Model.Orchestration? Map(
		Orchestrations.Model.Orchestration source,
		Orchestrations.Model.Orchestration? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Orchestrations.Model.Orchestration>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		Throw.IfArgumentNull(source);

		return source.MapTo(target, referenceModifier, conditions, instanceFactory, cache);
	}

	public Orchestrations.Model.Orchestration? Clone(
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Orchestrations.Model.Orchestration>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
		=> MapTo(target: null, referenceModifier, conditions, instanceFactory, cache);

	public Orchestrations.Model.Orchestration? MapTo(
		Orchestrations.Model.Orchestration? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Orchestrations.Model.Orchestration>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		cache ??= [];
		target ??= instanceFactory?.CreateInstance<Legion.ADF.ServiceBus.Orchestrations.Model.Orchestration>(
				useActivatorIfNoFactoryFound: false,
				throwIfFactoryReturnsNull: true)
			?? new Legion.ADF.ServiceBus.Orchestrations.Model.Orchestration();

		if (cache.TryGetValue(this, out var cached))
			return (Orchestrations.Model.Orchestration)cached;
			
		MappingConditions<Orchestrations.Model.Orchestration>? conds = null;
		if (conditions != null)
		{
			conds = new MappingConditions<Orchestrations.Model.Orchestration>();
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
			target._orchestrationInstances = MapperHelper.MapToList(OrchestrationInstances, target._orchestrationInstances, Legion.ADF.ServiceBus.Orchestrations.Model.OrchestrationInstance.Map, referenceModifier, conds?.GetConditions(x => x.OrchestrationInstances), instanceFactory, cache)!;
			target._orchestrationSteps = MapperHelper.MapToList(OrchestrationSteps, target._orchestrationSteps, Legion.ADF.ServiceBus.Orchestrations.Model.OrchestrationStep.Map, referenceModifier, conds?.GetConditions(x => x.OrchestrationSteps), instanceFactory, cache)!;
		}
		else if (referenceModifier == ReferenceModifier.SetNull)
		{
			target._orchestrationInstances = [];
			target._orchestrationSteps = [];
		}

		return target;
	}
}
