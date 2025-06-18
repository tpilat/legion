using Legion;
using Legion.Model.Mappers;

namespace Legion.ADF.Messaging.Inbox.Model;

public sealed partial class InboxMessage : Inbox.InboxBaseEntity, Legion.Model.IEntity
{
	public static Inbox.Model.InboxMessage? Map(
		Inbox.Model.InboxMessage source,
		Inbox.Model.InboxMessage? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Inbox.Model.InboxMessage>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		Throw.IfArgumentNull(source);

		return source.MapTo(target, referenceModifier, conditions, instanceFactory, cache);
	}

	public Inbox.Model.InboxMessage? Clone(
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Inbox.Model.InboxMessage>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
		=> MapTo(target: null, referenceModifier, conditions, instanceFactory, cache);

	public Inbox.Model.InboxMessage? MapTo(
		Inbox.Model.InboxMessage? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Inbox.Model.InboxMessage>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		cache ??= [];
		target ??= instanceFactory?.CreateInstance<Legion.ADF.Messaging.Inbox.Model.InboxMessage>(
				useActivatorIfNoFactoryFound: false,
				throwIfFactoryReturnsNull: true)
			?? new Legion.ADF.Messaging.Inbox.Model.InboxMessage();

		if (cache.TryGetValue(this, out var cached))
			return (Inbox.Model.InboxMessage)cached;
			
		MappingConditions<Inbox.Model.InboxMessage>? conds = null;
		if (conditions != null)
		{
			conds = new MappingConditions<Inbox.Model.InboxMessage>();
			conditions.Invoke(conds);

			if (conds.CanMap(this, nameof(IdInboxMessage)))
				target.IdInboxMessage = IdInboxMessage;
			if (conds.CanMap(this, nameof(IdMessageType)))
				target.IdMessageType = IdMessageType;
			if (conds.CanMap(this, nameof(IdInboxMessageStatus)))
				target.IdInboxMessageStatus = IdInboxMessageStatus;
			if (conds.CanMap(this, nameof(IdMessageContent)))
				target.IdMessageContent = IdMessageContent;
			if (conds.CanMap(this, nameof(IdInboxQueue)))
				target.IdInboxQueue = IdInboxQueue;
			if (conds.CanMap(this, nameof(MessageId)))
				target.MessageId = MessageId;
			if (conds.CanMap(this, nameof(BusinessId)))
				target.BusinessId = BusinessId;
			if (conds.CanMap(this, nameof(CorrelationId)))
				target.CorrelationId = CorrelationId;
			if (conds.CanMap(this, nameof(SessionId)))
				target.SessionId = SessionId;
			if (conds.CanMap(this, nameof(SessionMessagePartId)))
				target.SessionMessagePartId = SessionMessagePartId;
			if (conds.CanMap(this, nameof(TraceCorrelationId)))
				target.TraceCorrelationId = TraceCorrelationId;
			if (conds.CanMap(this, nameof(Properties)))
				target.Properties = Properties;
			if (conds.CanMap(this, nameof(Publisher)))
				target.Publisher = Publisher;
			if (conds.CanMap(this, nameof(PublisherId)))
				target.PublisherId = PublisherId;
			if (conds.CanMap(this, nameof(CreatedUtc)))
				target.CreatedUtc = CreatedUtc;
			if (conds.CanMap(this, nameof(ProcessedUtc)))
				target.ProcessedUtc = ProcessedUtc;
			if (conds.CanMap(this, nameof(SuspendedUtc)))
				target.SuspendedUtc = SuspendedUtc;
			if (conds.CanMap(this, nameof(LastProcessingUtc)))
				target.LastProcessingUtc = LastProcessingUtc;
			if (conds.CanMap(this, nameof(LastProcessingTimeoutUtc)))
				target.LastProcessingTimeoutUtc = LastProcessingTimeoutUtc;
			if (conds.CanMap(this, nameof(NextProcessingUtc)))
				target.NextProcessingUtc = NextProcessingUtc;
			if (conds.CanMap(this, nameof(RetryCount)))
				target.RetryCount = RetryCount;
			if (conds.CanMap(this, nameof(TargetTopic)))
				target.TargetTopic = TargetTopic;
			if (conds.CanMap(this, nameof(TargetQueueName)))
				target.TargetQueueName = TargetQueueName;
			if (conds.CanMap(this, nameof(IdInboxInstance)))
				target.IdInboxInstance = IdInboxInstance;
		}
		else
		{
			target.IdInboxMessage = IdInboxMessage;
			target.IdMessageType = IdMessageType;
			target.IdInboxMessageStatus = IdInboxMessageStatus;
			target.IdMessageContent = IdMessageContent;
			target.IdInboxQueue = IdInboxQueue;
			target.MessageId = MessageId;
			target.BusinessId = BusinessId;
			target.CorrelationId = CorrelationId;
			target.SessionId = SessionId;
			target.SessionMessagePartId = SessionMessagePartId;
			target.TraceCorrelationId = TraceCorrelationId;
			target.Properties = Properties;
			target.Publisher = Publisher;
			target.PublisherId = PublisherId;
			target.CreatedUtc = CreatedUtc;
			target.ProcessedUtc = ProcessedUtc;
			target.SuspendedUtc = SuspendedUtc;
			target.LastProcessingUtc = LastProcessingUtc;
			target.LastProcessingTimeoutUtc = LastProcessingTimeoutUtc;
			target.NextProcessingUtc = NextProcessingUtc;
			target.RetryCount = RetryCount;
			target.TargetTopic = TargetTopic;
			target.TargetQueueName = TargetQueueName;
			target.IdInboxInstance = IdInboxInstance;
		}

		cache.Add(this, target);

		if (referenceModifier == ReferenceModifier.MapAllReferences)
		{
			target.InboxInstance = InboxInstance?.MapTo(target.InboxInstance, referenceModifier, conds?.GetConditions(x => x.InboxInstance), instanceFactory, cache)!;
			target.InboxMessageStatus = InboxMessageStatus?.MapTo(target.InboxMessageStatus, referenceModifier, conds?.GetConditions(x => x.InboxMessageStatus), instanceFactory, cache)!;
			target.InboxQueue = InboxQueue?.MapTo(target.InboxQueue, referenceModifier, conds?.GetConditions(x => x.InboxQueue), instanceFactory, cache)!;
			target.MessageContent = MessageContent?.MapTo(target.MessageContent, referenceModifier, conds?.GetConditions(x => x.MessageContent), instanceFactory, cache)!;
			target.MessageType = MessageType?.MapTo(target.MessageType, referenceModifier, conds?.GetConditions(x => x.MessageType), instanceFactory, cache)!;
		}
		else if (referenceModifier == ReferenceModifier.SetNull)
		{
			target.InboxInstance = null!;
			target.InboxMessageStatus = null!;
			target.InboxQueue = null!;
			target.MessageContent = null!;
			target.MessageType = null!;
		}

		return target;
	}
}
