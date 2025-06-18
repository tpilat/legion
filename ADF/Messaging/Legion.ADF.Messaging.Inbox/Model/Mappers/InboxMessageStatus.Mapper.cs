using Legion;
using Legion.Model.Mappers;

namespace Legion.ADF.Messaging.Inbox.Model;

public sealed partial class InboxMessageStatus : Inbox.InboxBaseEntity, Legion.Model.IEntity
{
	public static Inbox.Model.InboxMessageStatus? Map(
		Inbox.Model.InboxMessageStatus source,
		Inbox.Model.InboxMessageStatus? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Inbox.Model.InboxMessageStatus>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		Throw.IfArgumentNull(source);

		return source.MapTo(target, referenceModifier, conditions, instanceFactory, cache);
	}

	public Inbox.Model.InboxMessageStatus? Clone(
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Inbox.Model.InboxMessageStatus>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
		=> MapTo(target: null, referenceModifier, conditions, instanceFactory, cache);

	public Inbox.Model.InboxMessageStatus? MapTo(
		Inbox.Model.InboxMessageStatus? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Inbox.Model.InboxMessageStatus>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		cache ??= [];
		target ??= Legion.ADF.Messaging.Inbox.Model.InboxMessageStatus.DictionaryMap.Value[IdInboxMessageStatus];

		if (cache.TryGetValue(this, out var cached))
			return (Inbox.Model.InboxMessageStatus)cached;
			
		MappingConditions<Inbox.Model.InboxMessageStatus>? conds = null;
		if (conditions != null)
		{
			conds = new MappingConditions<Inbox.Model.InboxMessageStatus>();
			conditions.Invoke(conds);
		}

		cache.Add(this, target);

		if (referenceModifier == ReferenceModifier.MapAllReferences)
		{
			target._inboxMessageArchives = MapperHelper.MapToList(InboxMessageArchives, target._inboxMessageArchives, InboxMessageArchive.Map, referenceModifier, conds?.GetConditions(x => x.InboxMessageArchives), instanceFactory, cache)!;
			target._inboxMessageProcessingLogs = MapperHelper.MapToList(InboxMessageProcessingLogs, target._inboxMessageProcessingLogs, InboxMessageProcessingLog.Map, referenceModifier, conds?.GetConditions(x => x.InboxMessageProcessingLogs), instanceFactory, cache)!;
			target._inboxMessages = MapperHelper.MapToList(InboxMessages, target._inboxMessages, InboxMessage.Map, referenceModifier, conds?.GetConditions(x => x.InboxMessages), instanceFactory, cache)!;
		}
		else if (referenceModifier == ReferenceModifier.SetNull)
		{
			target._inboxMessageArchives = [];
			target._inboxMessageProcessingLogs = [];
			target._inboxMessages = [];
		}

		return target;
	}
}
