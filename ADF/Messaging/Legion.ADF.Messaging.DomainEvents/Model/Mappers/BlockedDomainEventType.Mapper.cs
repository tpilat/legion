using Legion;
using Legion.Model.Mappers;

namespace Legion.ADF.Messaging.DomainEvents.Model;

public sealed partial class BlockedDomainEventType : DomainEvents.DomainEventsBaseEntity, Legion.Model.IEntity
{
	public static DomainEvents.Model.BlockedDomainEventType? Map(
		DomainEvents.Model.BlockedDomainEventType source,
		DomainEvents.Model.BlockedDomainEventType? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<DomainEvents.Model.BlockedDomainEventType>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		Throw.IfArgumentNull(source);

		return source.MapTo(target, referenceModifier, conditions, instanceFactory, cache);
	}

	public DomainEvents.Model.BlockedDomainEventType? Clone(
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<DomainEvents.Model.BlockedDomainEventType>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
		=> MapTo(target: null, referenceModifier, conditions, instanceFactory, cache);

	public DomainEvents.Model.BlockedDomainEventType? MapTo(
		DomainEvents.Model.BlockedDomainEventType? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<DomainEvents.Model.BlockedDomainEventType>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		cache ??= [];
		target ??= instanceFactory?.CreateInstance<Legion.ADF.Messaging.DomainEvents.Model.BlockedDomainEventType>(
				useActivatorIfNoFactoryFound: false,
				throwIfFactoryReturnsNull: true)
			?? new Legion.ADF.Messaging.DomainEvents.Model.BlockedDomainEventType();

		if (cache.TryGetValue(this, out var cached))
			return (DomainEvents.Model.BlockedDomainEventType)cached;
			
		MappingConditions<DomainEvents.Model.BlockedDomainEventType>? conds = null;
		if (conditions != null)
		{
			conds = new MappingConditions<DomainEvents.Model.BlockedDomainEventType>();
			conditions.Invoke(conds);

			if (conds.CanMap(this, nameof(IdBlockedDomainEventType)))
				target.IdBlockedDomainEventType = IdBlockedDomainEventType;
			if (conds.CanMap(this, nameof(Namespace)))
				target.Namespace = Namespace;
		}
		else
		{
			target.IdBlockedDomainEventType = IdBlockedDomainEventType;
			target.Namespace = Namespace;
		}

		cache.Add(this, target);

		return target;
	}
}
