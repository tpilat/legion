using Legion;
using Legion.Model.Mappers;

namespace Legion.ADF.Messaging.Inbox.Model;

public sealed partial class InboxQueue : Inbox.InboxBaseEntity, Legion.Model.IEntity
{
	public static Inbox.Model.InboxQueue? Map(
		Inbox.Model.InboxQueue source,
		Inbox.Model.InboxQueue? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Inbox.Model.InboxQueue>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		Throw.IfArgumentNull(source);

		return source.MapTo(target, referenceModifier, conditions, instanceFactory, cache);
	}

	public Inbox.Model.InboxQueue? Clone(
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Inbox.Model.InboxQueue>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
		=> MapTo(target: null, referenceModifier, conditions, instanceFactory, cache);

	public Inbox.Model.InboxQueue? MapTo(
		Inbox.Model.InboxQueue? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Inbox.Model.InboxQueue>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		cache ??= [];
		target ??= instanceFactory?.CreateInstance<Legion.ADF.Messaging.Inbox.Model.InboxQueue>(
				useActivatorIfNoFactoryFound: false,
				throwIfFactoryReturnsNull: true)
			?? new Legion.ADF.Messaging.Inbox.Model.InboxQueue();

		if (cache.TryGetValue(this, out var cached))
			return (Inbox.Model.InboxQueue)cached;
			
		MappingConditions<Inbox.Model.InboxQueue>? conds = null;
		if (conditions != null)
		{
			conds = new MappingConditions<Inbox.Model.InboxQueue>();
			conditions.Invoke(conds);

			if (conds.CanMap(this, nameof(IdInboxQueue)))
				target.IdInboxQueue = IdInboxQueue;
			if (conds.CanMap(this, nameof(Name)))
				target.Name = Name;
			if (conds.CanMap(this, nameof(ReceivedEventNamespace)))
				target.ReceivedEventNamespace = ReceivedEventNamespace;
			if (conds.CanMap(this, nameof(IdMessageType)))
				target.IdMessageType = IdMessageType;
			if (conds.CanMap(this, nameof(IsActive)))
				target.IsActive = IsActive;
			if (conds.CanMap(this, nameof(IsSequentialFIFO)))
				target.IsSequentialFIFO = IsSequentialFIFO;
			if (conds.CanMap(this, nameof(MessagesBatchCount)))
				target.MessagesBatchCount = MessagesBatchCount;
			if (conds.CanMap(this, nameof(MaxDegreeOfParallelism)))
				target.MaxDegreeOfParallelism = MaxDegreeOfParallelism;
			if (conds.CanMap(this, nameof(TimeoutForMessageProcessing)))
				target.TimeoutForMessageProcessing = TimeoutForMessageProcessing;
			if (conds.CanMap(this, nameof(MaxMessageProcessingRetryCount)))
				target.MaxMessageProcessingRetryCount = MaxMessageProcessingRetryCount;
			if (conds.CanMap(this, nameof(Properties)))
				target.Properties = Properties;
			if (conds.CanMap(this, nameof(IdProcessingMode)))
				target.IdProcessingMode = IdProcessingMode;
			if (conds.CanMap(this, nameof(IdSuspendingMode)))
				target.IdSuspendingMode = IdSuspendingMode;
			if (conds.CanMap(this, nameof(IdInboxInstance)))
				target.IdInboxInstance = IdInboxInstance;
		}
		else
		{
			target.IdInboxQueue = IdInboxQueue;
			target.Name = Name;
			target.ReceivedEventNamespace = ReceivedEventNamespace;
			target.IdMessageType = IdMessageType;
			target.IsActive = IsActive;
			target.IsSequentialFIFO = IsSequentialFIFO;
			target.MessagesBatchCount = MessagesBatchCount;
			target.MaxDegreeOfParallelism = MaxDegreeOfParallelism;
			target.TimeoutForMessageProcessing = TimeoutForMessageProcessing;
			target.MaxMessageProcessingRetryCount = MaxMessageProcessingRetryCount;
			target.Properties = Properties;
			target.IdProcessingMode = IdProcessingMode;
			target.IdSuspendingMode = IdSuspendingMode;
			target.IdInboxInstance = IdInboxInstance;
		}

		cache.Add(this, target);

		if (referenceModifier == ReferenceModifier.MapAllReferences)
		{
			target.InboxInstance = InboxInstance?.MapTo(target.InboxInstance, referenceModifier, conds?.GetConditions(x => x.InboxInstance), instanceFactory, cache)!;
			target.MessageType = MessageType?.MapTo(target.MessageType, referenceModifier, conds?.GetConditions(x => x.MessageType), instanceFactory, cache)!;
			target.ProcessingMode = ProcessingMode?.MapTo(target.ProcessingMode, referenceModifier, conds?.GetConditions(x => x.ProcessingMode), instanceFactory, cache)!;
			target.SuspendingMode = SuspendingMode?.MapTo(target.SuspendingMode, referenceModifier, conds?.GetConditions(x => x.SuspendingMode), instanceFactory, cache)!;
			target._inboxMessageArchives = MapperHelper.MapToList(InboxMessageArchives, target._inboxMessageArchives, InboxMessageArchive.Map, referenceModifier, conds?.GetConditions(x => x.InboxMessageArchives), instanceFactory, cache)!;
			target._inboxMessageProcessingLogs = MapperHelper.MapToList(InboxMessageProcessingLogs, target._inboxMessageProcessingLogs, InboxMessageProcessingLog.Map, referenceModifier, conds?.GetConditions(x => x.InboxMessageProcessingLogs), instanceFactory, cache)!;
			target._inboxMessages = MapperHelper.MapToList(InboxMessages, target._inboxMessages, InboxMessage.Map, referenceModifier, conds?.GetConditions(x => x.InboxMessages), instanceFactory, cache)!;
			target._inboxProcessingLogs = MapperHelper.MapToList(InboxProcessingLogs, target._inboxProcessingLogs, InboxProcessingLog.Map, referenceModifier, conds?.GetConditions(x => x.InboxProcessingLogs), instanceFactory, cache)!;
		}
		else if (referenceModifier == ReferenceModifier.SetNull)
		{
			target.InboxInstance = null!;
			target.MessageType = null!;
			target.ProcessingMode = null!;
			target.SuspendingMode = null!;
			target._inboxMessageArchives = [];
			target._inboxMessageProcessingLogs = [];
			target._inboxMessages = [];
			target._inboxProcessingLogs = [];
		}

		return target;
	}
}
