using Legion;
using Legion.Model.Mappers;

namespace Legion.ADF.Messaging.MessageBox.Model;

public sealed partial class QueueProcessingMode : MessageBox.MessageBoxBaseEntity, Legion.Model.IEntity
{
	public static MessageBox.Model.QueueProcessingMode? Map(
		MessageBox.Model.QueueProcessingMode source,
		MessageBox.Model.QueueProcessingMode? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<MessageBox.Model.QueueProcessingMode>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		Throw.IfArgumentNull(source);

		return source.MapTo(target, referenceModifier, conditions, instanceFactory, cache);
	}

	public MessageBox.Model.QueueProcessingMode? Clone(
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<MessageBox.Model.QueueProcessingMode>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
		=> MapTo(target: null, referenceModifier, conditions, instanceFactory, cache);

	public MessageBox.Model.QueueProcessingMode? MapTo(
		MessageBox.Model.QueueProcessingMode? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<MessageBox.Model.QueueProcessingMode>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		cache ??= [];
		target ??= Legion.ADF.Messaging.MessageBox.Model.QueueProcessingMode.DictionaryMap.Value[IdQueueProcessingMode];

		if (cache.TryGetValue(this, out var cached))
			return (MessageBox.Model.QueueProcessingMode)cached;
			
		MappingConditions<MessageBox.Model.QueueProcessingMode>? conds = null;
		if (conditions != null)
		{
			conds = new MappingConditions<MessageBox.Model.QueueProcessingMode>();
			conditions.Invoke(conds);
		}

		cache.Add(this, target);

		if (referenceModifier == ReferenceModifier.MapAllReferences)
		{
			target._queues = MapperHelper.MapToList(Queues, target._queues, Queue.Map, referenceModifier, conds?.GetConditions(x => x.Queues), instanceFactory, cache)!;
			target._suspendingModeQueues = MapperHelper.MapToList(SuspendingModeQueues, target._suspendingModeQueues, Queue.Map, referenceModifier, conds?.GetConditions(x => x.SuspendingModeQueues), instanceFactory, cache)!;
			target._suspendingModeTopics = MapperHelper.MapToList(SuspendingModeTopics, target._suspendingModeTopics, Topic.Map, referenceModifier, conds?.GetConditions(x => x.SuspendingModeTopics), instanceFactory, cache)!;
			target._suspendingModeTopicSubscriptions = MapperHelper.MapToList(SuspendingModeTopicSubscriptions, target._suspendingModeTopicSubscriptions, TopicSubscription.Map, referenceModifier, conds?.GetConditions(x => x.SuspendingModeTopicSubscriptions), instanceFactory, cache)!;
			target._topics = MapperHelper.MapToList(Topics, target._topics, Topic.Map, referenceModifier, conds?.GetConditions(x => x.Topics), instanceFactory, cache)!;
			target._topicSubscriptions = MapperHelper.MapToList(TopicSubscriptions, target._topicSubscriptions, TopicSubscription.Map, referenceModifier, conds?.GetConditions(x => x.TopicSubscriptions), instanceFactory, cache)!;
		}
		else if (referenceModifier == ReferenceModifier.SetNull)
		{
			target._queues = [];
			target._suspendingModeQueues = [];
			target._suspendingModeTopics = [];
			target._suspendingModeTopicSubscriptions = [];
			target._topics = [];
			target._topicSubscriptions = [];
		}

		return target;
	}
}
