using Legion;
using Legion.Model.Mappers;

namespace Legion.ADF.Messaging.MessageBox.Model;

public sealed partial class MessageBoxInstance : MessageBox.MessageBoxBaseEntity, Legion.Model.IEntity
{
	public static MessageBox.Model.MessageBoxInstance? Map(
		MessageBox.Model.MessageBoxInstance source,
		MessageBox.Model.MessageBoxInstance? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<MessageBox.Model.MessageBoxInstance>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		Throw.IfArgumentNull(source);

		return source.MapTo(target, referenceModifier, conditions, instanceFactory, cache);
	}

	public MessageBox.Model.MessageBoxInstance? Clone(
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<MessageBox.Model.MessageBoxInstance>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
		=> MapTo(target: null, referenceModifier, conditions, instanceFactory, cache);

	public MessageBox.Model.MessageBoxInstance? MapTo(
		MessageBox.Model.MessageBoxInstance? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<MessageBox.Model.MessageBoxInstance>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		cache ??= [];
		target ??= instanceFactory?.CreateInstance<Legion.ADF.Messaging.MessageBox.Model.MessageBoxInstance>(
				useActivatorIfNoFactoryFound: false,
				throwIfFactoryReturnsNull: true)
			?? new Legion.ADF.Messaging.MessageBox.Model.MessageBoxInstance();

		if (cache.TryGetValue(this, out var cached))
			return (MessageBox.Model.MessageBoxInstance)cached;
			
		MappingConditions<MessageBox.Model.MessageBoxInstance>? conds = null;
		if (conditions != null)
		{
			conds = new MappingConditions<MessageBox.Model.MessageBoxInstance>();
			conditions.Invoke(conds);

			if (conds.CanMap(this, nameof(IdMessageBoxInstance)))
				target.IdMessageBoxInstance = IdMessageBoxInstance;
			if (conds.CanMap(this, nameof(CreatedUtc)))
				target.CreatedUtc = CreatedUtc;
			if (conds.CanMap(this, nameof(Name)))
				target.Name = Name;
			if (conds.CanMap(this, nameof(Version)))
				target.Version = Version;
			if (conds.CanMap(this, nameof(MaxDegreeOfQueueParallelism)))
				target.MaxDegreeOfQueueParallelism = MaxDegreeOfQueueParallelism;
			if (conds.CanMap(this, nameof(MaxDegreeOfTopicParallelism)))
				target.MaxDegreeOfTopicParallelism = MaxDegreeOfTopicParallelism;
			if (conds.CanMap(this, nameof(IdLogLevel)))
				target.IdLogLevel = IdLogLevel;
		}
		else
		{
			target.IdMessageBoxInstance = IdMessageBoxInstance;
			target.CreatedUtc = CreatedUtc;
			target.Name = Name;
			target.Version = Version;
			target.MaxDegreeOfQueueParallelism = MaxDegreeOfQueueParallelism;
			target.MaxDegreeOfTopicParallelism = MaxDegreeOfTopicParallelism;
			target.IdLogLevel = IdLogLevel;
		}

		cache.Add(this, target);

		if (referenceModifier == ReferenceModifier.MapAllReferences)
		{
			target._blockedMessageTypes = MapperHelper.MapToList(BlockedMessageTypes, target._blockedMessageTypes, BlockedMessageType.Map, referenceModifier, conds?.GetConditions(x => x.BlockedMessageTypes), instanceFactory, cache)!;
			target._messageArchives = MapperHelper.MapToList(MessageArchives, target._messageArchives, MessageArchive.Map, referenceModifier, conds?.GetConditions(x => x.MessageArchives), instanceFactory, cache)!;
			target._messageBoxProcessingLogs = MapperHelper.MapToList(MessageBoxProcessingLogs, target._messageBoxProcessingLogs, MessageBoxProcessingLog.Map, referenceModifier, conds?.GetConditions(x => x.MessageBoxProcessingLogs), instanceFactory, cache)!;
			target._messageProcessingLogs = MapperHelper.MapToList(MessageProcessingLogs, target._messageProcessingLogs, MessageProcessingLog.Map, referenceModifier, conds?.GetConditions(x => x.MessageProcessingLogs), instanceFactory, cache)!;
			target._messages = MapperHelper.MapToList(Messages, target._messages, Message.Map, referenceModifier, conds?.GetConditions(x => x.Messages), instanceFactory, cache)!;
			target._messageTypes = MapperHelper.MapToList(MessageTypes, target._messageTypes, MessageType.Map, referenceModifier, conds?.GetConditions(x => x.MessageTypes), instanceFactory, cache)!;
			target._queuedMessages = MapperHelper.MapToList(QueuedMessages, target._queuedMessages, QueuedMessage.Map, referenceModifier, conds?.GetConditions(x => x.QueuedMessages), instanceFactory, cache)!;
			target._queues = MapperHelper.MapToList(Queues, target._queues, Queue.Map, referenceModifier, conds?.GetConditions(x => x.Queues), instanceFactory, cache)!;
			target._subscribedMessages = MapperHelper.MapToList(SubscribedMessages, target._subscribedMessages, SubscribedMessage.Map, referenceModifier, conds?.GetConditions(x => x.SubscribedMessages), instanceFactory, cache)!;
			target._topics = MapperHelper.MapToList(Topics, target._topics, Topic.Map, referenceModifier, conds?.GetConditions(x => x.Topics), instanceFactory, cache)!;
			target._topicSubscriptions = MapperHelper.MapToList(TopicSubscriptions, target._topicSubscriptions, TopicSubscription.Map, referenceModifier, conds?.GetConditions(x => x.TopicSubscriptions), instanceFactory, cache)!;
		}
		else if (referenceModifier == ReferenceModifier.SetNull)
		{
			target._blockedMessageTypes = [];
			target._messageArchives = [];
			target._messageBoxProcessingLogs = [];
			target._messageProcessingLogs = [];
			target._messages = [];
			target._messageTypes = [];
			target._queuedMessages = [];
			target._queues = [];
			target._subscribedMessages = [];
			target._topics = [];
			target._topicSubscriptions = [];
		}

		return target;
	}
}
