using Legion;
using Legion.Model.Mappers;

namespace Legion.ADF.Messaging.Outbox.Model;

public sealed partial class BlockedOutboxMessageType : Outbox.OutboxBaseEntity, Legion.Model.IEntity
{
	public static Outbox.Model.BlockedOutboxMessageType? Map(
		Outbox.Model.BlockedOutboxMessageType source,
		Outbox.Model.BlockedOutboxMessageType? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Outbox.Model.BlockedOutboxMessageType>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		Throw.IfArgumentNull(source);

		return source.MapTo(target, referenceModifier, conditions, instanceFactory, cache);
	}

	public Outbox.Model.BlockedOutboxMessageType? Clone(
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Outbox.Model.BlockedOutboxMessageType>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
		=> MapTo(target: null, referenceModifier, conditions, instanceFactory, cache);

	public Outbox.Model.BlockedOutboxMessageType? MapTo(
		Outbox.Model.BlockedOutboxMessageType? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Outbox.Model.BlockedOutboxMessageType>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		cache ??= [];
		target ??= instanceFactory?.CreateInstance<Legion.ADF.Messaging.Outbox.Model.BlockedOutboxMessageType>(
				useActivatorIfNoFactoryFound: false,
				throwIfFactoryReturnsNull: true)
			?? new Legion.ADF.Messaging.Outbox.Model.BlockedOutboxMessageType();

		if (cache.TryGetValue(this, out var cached))
			return (Outbox.Model.BlockedOutboxMessageType)cached;
			
		MappingConditions<Outbox.Model.BlockedOutboxMessageType>? conds = null;
		if (conditions != null)
		{
			conds = new MappingConditions<Outbox.Model.BlockedOutboxMessageType>();
			conditions.Invoke(conds);

			if (conds.CanMap(this, nameof(IdBlockedOutboxMessageType)))
				target.IdBlockedOutboxMessageType = IdBlockedOutboxMessageType;
			if (conds.CanMap(this, nameof(Namespace)))
				target.Namespace = Namespace;
			if (conds.CanMap(this, nameof(CreatedUtc)))
				target.CreatedUtc = CreatedUtc;
			if (conds.CanMap(this, nameof(IdOutboxInstance)))
				target.IdOutboxInstance = IdOutboxInstance;
		}
		else
		{
			target.IdBlockedOutboxMessageType = IdBlockedOutboxMessageType;
			target.Namespace = Namespace;
			target.CreatedUtc = CreatedUtc;
			target.IdOutboxInstance = IdOutboxInstance;
		}

		cache.Add(this, target);

		if (referenceModifier == ReferenceModifier.MapAllReferences)
		{
			target.OutboxInstance = OutboxInstance?.MapTo(target.OutboxInstance, referenceModifier, conds?.GetConditions(x => x.OutboxInstance), instanceFactory, cache)!;
		}
		else if (referenceModifier == ReferenceModifier.SetNull)
		{
			target.OutboxInstance = null!;
		}

		return target;
	}
}
