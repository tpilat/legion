using Legion;
using Legion.Model.Mappers;

namespace Legion.ADF.Messaging.Inbox.Model;

public sealed partial class InboxQueueProcessingMode : Inbox.InboxBaseEntity, Legion.Model.IEntity
{
	public static Inbox.Model.InboxQueueProcessingMode? Map(
		Inbox.Model.InboxQueueProcessingMode source,
		Inbox.Model.InboxQueueProcessingMode? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Inbox.Model.InboxQueueProcessingMode>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		Throw.IfArgumentNull(source);

		return source.MapTo(target, referenceModifier, conditions, instanceFactory, cache);
	}

	public Inbox.Model.InboxQueueProcessingMode? Clone(
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Inbox.Model.InboxQueueProcessingMode>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
		=> MapTo(target: null, referenceModifier, conditions, instanceFactory, cache);

	public Inbox.Model.InboxQueueProcessingMode? MapTo(
		Inbox.Model.InboxQueueProcessingMode? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Inbox.Model.InboxQueueProcessingMode>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		cache ??= [];
		target ??= Legion.ADF.Messaging.Inbox.Model.InboxQueueProcessingMode.DictionaryMap.Value[IdInboxQueueProcessingMode];

		if (cache.TryGetValue(this, out var cached))
			return (Inbox.Model.InboxQueueProcessingMode)cached;
			
		MappingConditions<Inbox.Model.InboxQueueProcessingMode>? conds = null;
		if (conditions != null)
		{
			conds = new MappingConditions<Inbox.Model.InboxQueueProcessingMode>();
			conditions.Invoke(conds);
		}

		cache.Add(this, target);

		if (referenceModifier == ReferenceModifier.MapAllReferences)
		{
			target._inboxQueues = MapperHelper.MapToList(InboxQueues, target._inboxQueues, InboxQueue.Map, referenceModifier, conds?.GetConditions(x => x.InboxQueues), instanceFactory, cache)!;
			target._suspendingModeInboxQueues = MapperHelper.MapToList(SuspendingModeInboxQueues, target._suspendingModeInboxQueues, InboxQueue.Map, referenceModifier, conds?.GetConditions(x => x.SuspendingModeInboxQueues), instanceFactory, cache)!;
		}
		else if (referenceModifier == ReferenceModifier.SetNull)
		{
			target._inboxQueues = [];
			target._suspendingModeInboxQueues = [];
		}

		return target;
	}
}
