using Legion;
using Legion.Model.Mappers;

namespace Legion.ADF.Messaging.MessageBox.Model;

public sealed partial class MessageArchive : MessageBox.MessageBoxBaseEntity, Legion.Model.IEntity
{
	public static MessageBox.Model.MessageArchive? Map(
		MessageBox.Model.MessageArchive source,
		MessageBox.Model.MessageArchive? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<MessageBox.Model.MessageArchive>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		Throw.IfArgumentNull(source);

		return source.MapTo(target, referenceModifier, conditions, instanceFactory, cache);
	}

	public MessageBox.Model.MessageArchive? Clone(
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<MessageBox.Model.MessageArchive>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
		=> MapTo(target: null, referenceModifier, conditions, instanceFactory, cache);

	public MessageBox.Model.MessageArchive? MapTo(
		MessageBox.Model.MessageArchive? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<MessageBox.Model.MessageArchive>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		cache ??= [];
		target ??= instanceFactory?.CreateInstance<Legion.ADF.Messaging.MessageBox.Model.MessageArchive>(
				useActivatorIfNoFactoryFound: false,
				throwIfFactoryReturnsNull: true)
			?? new Legion.ADF.Messaging.MessageBox.Model.MessageArchive();

		if (cache.TryGetValue(this, out var cached))
			return (MessageBox.Model.MessageArchive)cached;
			
		MappingConditions<MessageBox.Model.MessageArchive>? conds = null;
		if (conditions != null)
		{
			conds = new MappingConditions<MessageBox.Model.MessageArchive>();
			conditions.Invoke(conds);

			if (conds.CanMap(this, nameof(IdMessage)))
				target.IdMessage = IdMessage;
			if (conds.CanMap(this, nameof(IdMessageType)))
				target.IdMessageType = IdMessageType;
			if (conds.CanMap(this, nameof(IdMessageStatus)))
				target.IdMessageStatus = IdMessageStatus;
			if (conds.CanMap(this, nameof(IdMessageContent)))
				target.IdMessageContent = IdMessageContent;
			if (conds.CanMap(this, nameof(IdQueue)))
				target.IdQueue = IdQueue;
			if (conds.CanMap(this, nameof(IdTopic)))
				target.IdTopic = IdTopic;
			if (conds.CanMap(this, nameof(CreatedUtc)))
				target.CreatedUtc = CreatedUtc;
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
			if (conds.CanMap(this, nameof(ValidToUtc)))
				target.ValidToUtc = ValidToUtc;
			if (conds.CanMap(this, nameof(Priority)))
				target.Priority = Priority;
			if (conds.CanMap(this, nameof(IdMessageBoxInstance)))
				target.IdMessageBoxInstance = IdMessageBoxInstance;
		}
		else
		{
			target.IdMessage = IdMessage;
			target.IdMessageType = IdMessageType;
			target.IdMessageStatus = IdMessageStatus;
			target.IdMessageContent = IdMessageContent;
			target.IdQueue = IdQueue;
			target.IdTopic = IdTopic;
			target.CreatedUtc = CreatedUtc;
			target.MessageId = MessageId;
			target.BusinessId = BusinessId;
			target.CorrelationId = CorrelationId;
			target.SessionId = SessionId;
			target.SessionMessagePartId = SessionMessagePartId;
			target.TraceCorrelationId = TraceCorrelationId;
			target.Properties = Properties;
			target.Publisher = Publisher;
			target.PublisherId = PublisherId;
			target.ValidToUtc = ValidToUtc;
			target.Priority = Priority;
			target.IdMessageBoxInstance = IdMessageBoxInstance;
		}

		cache.Add(this, target);

		if (referenceModifier == ReferenceModifier.MapAllReferences)
		{
			target.MessageBoxInstance = MessageBoxInstance?.MapTo(target.MessageBoxInstance, referenceModifier, conds?.GetConditions(x => x.MessageBoxInstance), instanceFactory, cache)!;
			target.MessageContent = MessageContent?.MapTo(target.MessageContent, referenceModifier, conds?.GetConditions(x => x.MessageContent), instanceFactory, cache)!;
			target.MessageStatus = MessageStatus?.MapTo(target.MessageStatus, referenceModifier, conds?.GetConditions(x => x.MessageStatus), instanceFactory, cache)!;
			target.MessageType = MessageType?.MapTo(target.MessageType, referenceModifier, conds?.GetConditions(x => x.MessageType), instanceFactory, cache)!;
			target.Queue = Queue?.MapTo(target.Queue, referenceModifier, conds?.GetConditions(x => x.Queue), instanceFactory, cache)!;
			target.Topic = Topic?.MapTo(target.Topic, referenceModifier, conds?.GetConditions(x => x.Topic), instanceFactory, cache)!;
		}
		else if (referenceModifier == ReferenceModifier.SetNull)
		{
			target.MessageBoxInstance = null!;
			target.MessageContent = null!;
			target.MessageStatus = null!;
			target.MessageType = null!;
			target.Queue = null!;
			target.Topic = null!;
		}

		return target;
	}
}
