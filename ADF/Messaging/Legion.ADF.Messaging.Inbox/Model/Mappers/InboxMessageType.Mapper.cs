using Legion;
using Legion.Model.Mappers;

namespace Legion.ADF.Messaging.Inbox.Model;

public sealed partial class InboxMessageType : Inbox.InboxBaseEntity, Legion.Model.IEntity
{
	public static Inbox.Model.InboxMessageType? Map(
		Inbox.Model.InboxMessageType source,
		Inbox.Model.InboxMessageType? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Inbox.Model.InboxMessageType>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		Throw.IfArgumentNull(source);

		return source.MapTo(target, referenceModifier, conditions, instanceFactory, cache);
	}

	public Inbox.Model.InboxMessageType? Clone(
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Inbox.Model.InboxMessageType>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
		=> MapTo(target: null, referenceModifier, conditions, instanceFactory, cache);

	public Inbox.Model.InboxMessageType? MapTo(
		Inbox.Model.InboxMessageType? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Inbox.Model.InboxMessageType>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		cache ??= [];
		target ??= instanceFactory?.CreateInstance<Legion.ADF.Messaging.Inbox.Model.InboxMessageType>(
				useActivatorIfNoFactoryFound: false,
				throwIfFactoryReturnsNull: true)
			?? new Legion.ADF.Messaging.Inbox.Model.InboxMessageType();

		if (cache.TryGetValue(this, out var cached))
			return (Inbox.Model.InboxMessageType)cached;
			
		MappingConditions<Inbox.Model.InboxMessageType>? conds = null;
		if (conditions != null)
		{
			conds = new MappingConditions<Inbox.Model.InboxMessageType>();
			conditions.Invoke(conds);

			if (conds.CanMap(this, nameof(IdInboxMessageType)))
				target.IdInboxMessageType = IdInboxMessageType;
			if (conds.CanMap(this, nameof(Code)))
				target.Code = Code;
			if (conds.CanMap(this, nameof(Name)))
				target.Name = Name;
			if (conds.CanMap(this, nameof(Namespace)))
				target.Namespace = Namespace;
			if (conds.CanMap(this, nameof(CreatedUtc)))
				target.CreatedUtc = CreatedUtc;
			if (conds.CanMap(this, nameof(IdInboxInstance)))
				target.IdInboxInstance = IdInboxInstance;
		}
		else
		{
			target.IdInboxMessageType = IdInboxMessageType;
			target.Code = Code;
			target.Name = Name;
			target.Namespace = Namespace;
			target.CreatedUtc = CreatedUtc;
			target.IdInboxInstance = IdInboxInstance;
		}

		cache.Add(this, target);

		if (referenceModifier == ReferenceModifier.MapAllReferences)
		{
			target.InboxInstance = InboxInstance?.MapTo(target.InboxInstance, referenceModifier, conds?.GetConditions(x => x.InboxInstance), instanceFactory, cache)!;
			target._inboxMessageArchives = MapperHelper.MapToList(InboxMessageArchives, target._inboxMessageArchives, InboxMessageArchive.Map, referenceModifier, conds?.GetConditions(x => x.InboxMessageArchives), instanceFactory, cache)!;
			target._inboxMessages = MapperHelper.MapToList(InboxMessages, target._inboxMessages, InboxMessage.Map, referenceModifier, conds?.GetConditions(x => x.InboxMessages), instanceFactory, cache)!;
			target._inboxQueues = MapperHelper.MapToList(InboxQueues, target._inboxQueues, InboxQueue.Map, referenceModifier, conds?.GetConditions(x => x.InboxQueues), instanceFactory, cache)!;
		}
		else if (referenceModifier == ReferenceModifier.SetNull)
		{
			target.InboxInstance = null!;
			target._inboxMessageArchives = [];
			target._inboxMessages = [];
			target._inboxQueues = [];
		}

		return target;
	}
}
