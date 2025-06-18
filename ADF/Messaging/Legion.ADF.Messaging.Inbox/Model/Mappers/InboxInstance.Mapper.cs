using Legion;
using Legion.Model.Mappers;

namespace Legion.ADF.Messaging.Inbox.Model;

public sealed partial class InboxInstance : Inbox.InboxBaseEntity, Legion.Model.IEntity
{
	public static Inbox.Model.InboxInstance? Map(
		Inbox.Model.InboxInstance source,
		Inbox.Model.InboxInstance? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Inbox.Model.InboxInstance>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		Throw.IfArgumentNull(source);

		return source.MapTo(target, referenceModifier, conditions, instanceFactory, cache);
	}

	public Inbox.Model.InboxInstance? Clone(
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Inbox.Model.InboxInstance>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
		=> MapTo(target: null, referenceModifier, conditions, instanceFactory, cache);

	public Inbox.Model.InboxInstance? MapTo(
		Inbox.Model.InboxInstance? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Inbox.Model.InboxInstance>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		cache ??= [];
		target ??= instanceFactory?.CreateInstance<Legion.ADF.Messaging.Inbox.Model.InboxInstance>(
				useActivatorIfNoFactoryFound: false,
				throwIfFactoryReturnsNull: true)
			?? new Legion.ADF.Messaging.Inbox.Model.InboxInstance();

		if (cache.TryGetValue(this, out var cached))
			return (Inbox.Model.InboxInstance)cached;
			
		MappingConditions<Inbox.Model.InboxInstance>? conds = null;
		if (conditions != null)
		{
			conds = new MappingConditions<Inbox.Model.InboxInstance>();
			conditions.Invoke(conds);

			if (conds.CanMap(this, nameof(IdInboxInstance)))
				target.IdInboxInstance = IdInboxInstance;
			if (conds.CanMap(this, nameof(CreatedUtc)))
				target.CreatedUtc = CreatedUtc;
			if (conds.CanMap(this, nameof(Name)))
				target.Name = Name;
			if (conds.CanMap(this, nameof(Version)))
				target.Version = Version;
			if (conds.CanMap(this, nameof(MaxDegreeOfQueueParallelism)))
				target.MaxDegreeOfQueueParallelism = MaxDegreeOfQueueParallelism;
			if (conds.CanMap(this, nameof(IdLogLevel)))
				target.IdLogLevel = IdLogLevel;
		}
		else
		{
			target.IdInboxInstance = IdInboxInstance;
			target.CreatedUtc = CreatedUtc;
			target.Name = Name;
			target.Version = Version;
			target.MaxDegreeOfQueueParallelism = MaxDegreeOfQueueParallelism;
			target.IdLogLevel = IdLogLevel;
		}

		cache.Add(this, target);

		if (referenceModifier == ReferenceModifier.MapAllReferences)
		{
			target._blockedInboxMessageTypes = MapperHelper.MapToList(BlockedInboxMessageTypes, target._blockedInboxMessageTypes, BlockedInboxMessageType.Map, referenceModifier, conds?.GetConditions(x => x.BlockedInboxMessageTypes), instanceFactory, cache)!;
			target._inboxMessageArchives = MapperHelper.MapToList(InboxMessageArchives, target._inboxMessageArchives, InboxMessageArchive.Map, referenceModifier, conds?.GetConditions(x => x.InboxMessageArchives), instanceFactory, cache)!;
			target._inboxMessageProcessingLogs = MapperHelper.MapToList(InboxMessageProcessingLogs, target._inboxMessageProcessingLogs, InboxMessageProcessingLog.Map, referenceModifier, conds?.GetConditions(x => x.InboxMessageProcessingLogs), instanceFactory, cache)!;
			target._inboxMessages = MapperHelper.MapToList(InboxMessages, target._inboxMessages, InboxMessage.Map, referenceModifier, conds?.GetConditions(x => x.InboxMessages), instanceFactory, cache)!;
			target._inboxMessageTypes = MapperHelper.MapToList(InboxMessageTypes, target._inboxMessageTypes, InboxMessageType.Map, referenceModifier, conds?.GetConditions(x => x.InboxMessageTypes), instanceFactory, cache)!;
			target._inboxProcessingLogs = MapperHelper.MapToList(InboxProcessingLogs, target._inboxProcessingLogs, InboxProcessingLog.Map, referenceModifier, conds?.GetConditions(x => x.InboxProcessingLogs), instanceFactory, cache)!;
			target._inboxQueues = MapperHelper.MapToList(InboxQueues, target._inboxQueues, InboxQueue.Map, referenceModifier, conds?.GetConditions(x => x.InboxQueues), instanceFactory, cache)!;
		}
		else if (referenceModifier == ReferenceModifier.SetNull)
		{
			target._blockedInboxMessageTypes = [];
			target._inboxMessageArchives = [];
			target._inboxMessageProcessingLogs = [];
			target._inboxMessages = [];
			target._inboxMessageTypes = [];
			target._inboxProcessingLogs = [];
			target._inboxQueues = [];
		}

		return target;
	}
}
