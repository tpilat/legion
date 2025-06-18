using Legion;
using Legion.Model.Mappers;

namespace Legion.ADF.Messaging.Inbox.Model;

public sealed partial class BlockedInboxMessageType : Inbox.InboxBaseEntity, Legion.Model.IEntity
{
	public static Inbox.Model.BlockedInboxMessageType? Map(
		Inbox.Model.BlockedInboxMessageType source,
		Inbox.Model.BlockedInboxMessageType? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Inbox.Model.BlockedInboxMessageType>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		Throw.IfArgumentNull(source);

		return source.MapTo(target, referenceModifier, conditions, instanceFactory, cache);
	}

	public Inbox.Model.BlockedInboxMessageType? Clone(
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Inbox.Model.BlockedInboxMessageType>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
		=> MapTo(target: null, referenceModifier, conditions, instanceFactory, cache);

	public Inbox.Model.BlockedInboxMessageType? MapTo(
		Inbox.Model.BlockedInboxMessageType? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Inbox.Model.BlockedInboxMessageType>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		cache ??= [];
		target ??= instanceFactory?.CreateInstance<Legion.ADF.Messaging.Inbox.Model.BlockedInboxMessageType>(
				useActivatorIfNoFactoryFound: false,
				throwIfFactoryReturnsNull: true)
			?? new Legion.ADF.Messaging.Inbox.Model.BlockedInboxMessageType();

		if (cache.TryGetValue(this, out var cached))
			return (Inbox.Model.BlockedInboxMessageType)cached;
			
		MappingConditions<Inbox.Model.BlockedInboxMessageType>? conds = null;
		if (conditions != null)
		{
			conds = new MappingConditions<Inbox.Model.BlockedInboxMessageType>();
			conditions.Invoke(conds);

			if (conds.CanMap(this, nameof(IdBlockedInboxMessageType)))
				target.IdBlockedInboxMessageType = IdBlockedInboxMessageType;
			if (conds.CanMap(this, nameof(Namespace)))
				target.Namespace = Namespace;
			if (conds.CanMap(this, nameof(CreatedUtc)))
				target.CreatedUtc = CreatedUtc;
			if (conds.CanMap(this, nameof(IdInboxInstance)))
				target.IdInboxInstance = IdInboxInstance;
		}
		else
		{
			target.IdBlockedInboxMessageType = IdBlockedInboxMessageType;
			target.Namespace = Namespace;
			target.CreatedUtc = CreatedUtc;
			target.IdInboxInstance = IdInboxInstance;
		}

		cache.Add(this, target);

		if (referenceModifier == ReferenceModifier.MapAllReferences)
		{
			target.InboxInstance = InboxInstance?.MapTo(target.InboxInstance, referenceModifier, conds?.GetConditions(x => x.InboxInstance), instanceFactory, cache)!;
		}
		else if (referenceModifier == ReferenceModifier.SetNull)
		{
			target.InboxInstance = null!;
		}

		return target;
	}
}
