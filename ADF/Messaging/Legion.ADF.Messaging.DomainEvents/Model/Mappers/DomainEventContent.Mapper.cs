using Legion;
using Legion.Model.Mappers;

namespace Legion.ADF.Messaging.DomainEvents.Model;

public sealed partial class DomainEventContent : DomainEvents.DomainEventsBaseEntity, Legion.Model.IEntity
{
	public static DomainEvents.Model.DomainEventContent? Map(
		DomainEvents.Model.DomainEventContent source,
		DomainEvents.Model.DomainEventContent? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<DomainEvents.Model.DomainEventContent>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		Throw.IfArgumentNull(source);

		return source.MapTo(target, referenceModifier, conditions, instanceFactory, cache);
	}

	public DomainEvents.Model.DomainEventContent? Clone(
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<DomainEvents.Model.DomainEventContent>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
		=> MapTo(target: null, referenceModifier, conditions, instanceFactory, cache);

	public DomainEvents.Model.DomainEventContent? MapTo(
		DomainEvents.Model.DomainEventContent? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<DomainEvents.Model.DomainEventContent>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		cache ??= [];
		target ??= instanceFactory?.CreateInstance<Legion.ADF.Messaging.DomainEvents.Model.DomainEventContent>(
				useActivatorIfNoFactoryFound: false,
				throwIfFactoryReturnsNull: true)
			?? new Legion.ADF.Messaging.DomainEvents.Model.DomainEventContent();

		if (cache.TryGetValue(this, out var cached))
			return (DomainEvents.Model.DomainEventContent)cached;
			
		MappingConditions<DomainEvents.Model.DomainEventContent>? conds = null;
		if (conditions != null)
		{
			conds = new MappingConditions<DomainEvents.Model.DomainEventContent>();
			conditions.Invoke(conds);

			if (conds.CanMap(this, nameof(IdDomainEventContent)))
				target.IdDomainEventContent = IdDomainEventContent;
			if (conds.CanMap(this, nameof(Content)))
				target.Content = Content;
		}
		else
		{
			target.IdDomainEventContent = IdDomainEventContent;
			target.Content = Content;
		}

		cache.Add(this, target);

		if (referenceModifier == ReferenceModifier.MapAllReferences)
		{
			target.DomainEvent = DomainEvent?.MapTo(target.DomainEvent, referenceModifier, conds?.GetConditions(x => x.DomainEvent), instanceFactory, cache)!;
		}
		else if (referenceModifier == ReferenceModifier.SetNull)
		{
			target.DomainEvent = null!;
		}

		return target;
	}
}
