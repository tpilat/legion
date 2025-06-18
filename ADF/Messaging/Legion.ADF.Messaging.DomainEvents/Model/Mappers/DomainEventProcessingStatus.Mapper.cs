using Legion;
using Legion.Model.Mappers;

namespace Legion.ADF.Messaging.DomainEvents.Model;

public sealed partial class DomainEventProcessingStatus : DomainEvents.DomainEventsBaseEntity, Legion.Model.IEntity
{
	public static DomainEvents.Model.DomainEventProcessingStatus? Map(
		DomainEvents.Model.DomainEventProcessingStatus source,
		DomainEvents.Model.DomainEventProcessingStatus? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<DomainEvents.Model.DomainEventProcessingStatus>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		Throw.IfArgumentNull(source);

		return source.MapTo(target, referenceModifier, conditions, instanceFactory, cache);
	}

	public DomainEvents.Model.DomainEventProcessingStatus? Clone(
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<DomainEvents.Model.DomainEventProcessingStatus>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
		=> MapTo(target: null, referenceModifier, conditions, instanceFactory, cache);

	public DomainEvents.Model.DomainEventProcessingStatus? MapTo(
		DomainEvents.Model.DomainEventProcessingStatus? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<DomainEvents.Model.DomainEventProcessingStatus>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		cache ??= [];
		target ??= Legion.ADF.Messaging.DomainEvents.Model.DomainEventProcessingStatus.DictionaryMap.Value[IdDomainEventProcessingStatus];

		if (cache.TryGetValue(this, out var cached))
			return (DomainEvents.Model.DomainEventProcessingStatus)cached;
			
		MappingConditions<DomainEvents.Model.DomainEventProcessingStatus>? conds = null;
		if (conditions != null)
		{
			conds = new MappingConditions<DomainEvents.Model.DomainEventProcessingStatus>();
			conditions.Invoke(conds);
		}

		cache.Add(this, target);

		if (referenceModifier == ReferenceModifier.MapAllReferences)
		{
			target._domainEventProcessingLogs = MapperHelper.MapToList(DomainEventProcessingLogs, target._domainEventProcessingLogs, DomainEventProcessingLog.Map, referenceModifier, conds?.GetConditions(x => x.DomainEventProcessingLogs), instanceFactory, cache)!;
			target._domainEvents = MapperHelper.MapToList(DomainEvents, target._domainEvents, DomainEvent.Map, referenceModifier, conds?.GetConditions(x => x.DomainEvents), instanceFactory, cache)!;
		}
		else if (referenceModifier == ReferenceModifier.SetNull)
		{
			target._domainEventProcessingLogs = [];
			target._domainEvents = [];
		}

		return target;
	}
}
