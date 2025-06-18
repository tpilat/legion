using Legion;
using Legion.Model.Mappers;

namespace Legion.ADF.Messaging.Outbox.Model;

public sealed partial class OutboxMessage : Outbox.OutboxBaseEntity, Legion.Model.IEntity
{
	public static Outbox.Model.OutboxMessage? Map(
		Outbox.Model.OutboxMessage source,
		Outbox.Model.OutboxMessage? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Outbox.Model.OutboxMessage>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		Throw.IfArgumentNull(source);

		return source.MapTo(target, referenceModifier, conditions, instanceFactory, cache);
	}

	public Outbox.Model.OutboxMessage? Clone(
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Outbox.Model.OutboxMessage>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
		=> MapTo(target: null, referenceModifier, conditions, instanceFactory, cache);

	public Outbox.Model.OutboxMessage? MapTo(
		Outbox.Model.OutboxMessage? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Outbox.Model.OutboxMessage>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		cache ??= [];
		target ??= instanceFactory?.CreateInstance<Legion.ADF.Messaging.Outbox.Model.OutboxMessage>(
				useActivatorIfNoFactoryFound: false,
				throwIfFactoryReturnsNull: true)
			?? new Legion.ADF.Messaging.Outbox.Model.OutboxMessage();

		if (cache.TryGetValue(this, out var cached))
			return (Outbox.Model.OutboxMessage)cached;
			
		MappingConditions<Outbox.Model.OutboxMessage>? conds = null;
		if (conditions != null)
		{
			conds = new MappingConditions<Outbox.Model.OutboxMessage>();
			conditions.Invoke(conds);

			if (conds.CanMap(this, nameof(IdOutboxMessage)))
				target.IdOutboxMessage = IdOutboxMessage;
			if (conds.CanMap(this, nameof(IdMessageType)))
				target.IdMessageType = IdMessageType;
			if (conds.CanMap(this, nameof(IdOutboxMessageStatus)))
				target.IdOutboxMessageStatus = IdOutboxMessageStatus;
			if (conds.CanMap(this, nameof(IdMessageContent)))
				target.IdMessageContent = IdMessageContent;
			if (conds.CanMap(this, nameof(IdOutboxQueue)))
				target.IdOutboxQueue = IdOutboxQueue;
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
			if (conds.CanMap(this, nameof(IdOutboxInstance)))
				target.IdOutboxInstance = IdOutboxInstance;
		}
		else
		{
			target.IdOutboxMessage = IdOutboxMessage;
			target.IdMessageType = IdMessageType;
			target.IdOutboxMessageStatus = IdOutboxMessageStatus;
			target.IdMessageContent = IdMessageContent;
			target.IdOutboxQueue = IdOutboxQueue;
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
			target.IdOutboxInstance = IdOutboxInstance;
		}

		cache.Add(this, target);

		if (referenceModifier == ReferenceModifier.MapAllReferences)
		{
			target.MessageContent = MessageContent?.MapTo(target.MessageContent, referenceModifier, conds?.GetConditions(x => x.MessageContent), instanceFactory, cache)!;
			target.MessageType = MessageType?.MapTo(target.MessageType, referenceModifier, conds?.GetConditions(x => x.MessageType), instanceFactory, cache)!;
			target.OutboxInstance = OutboxInstance?.MapTo(target.OutboxInstance, referenceModifier, conds?.GetConditions(x => x.OutboxInstance), instanceFactory, cache)!;
			target.OutboxMessageStatus = OutboxMessageStatus?.MapTo(target.OutboxMessageStatus, referenceModifier, conds?.GetConditions(x => x.OutboxMessageStatus), instanceFactory, cache)!;
			target.OutboxQueue = OutboxQueue?.MapTo(target.OutboxQueue, referenceModifier, conds?.GetConditions(x => x.OutboxQueue), instanceFactory, cache)!;
		}
		else if (referenceModifier == ReferenceModifier.SetNull)
		{
			target.MessageContent = null!;
			target.MessageType = null!;
			target.OutboxInstance = null!;
			target.OutboxMessageStatus = null!;
			target.OutboxQueue = null!;
		}

		return target;
	}
}
