using Legion;
using Legion.Model.Mappers;

namespace Legion.ADF.Messaging.MessageBox.Model;

public sealed partial class TopicSubscription : MessageBox.MessageBoxBaseEntity, Legion.Model.IEntity
{
	public static MessageBox.Model.TopicSubscription? Map(
		MessageBox.Model.TopicSubscription source,
		MessageBox.Model.TopicSubscription? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<MessageBox.Model.TopicSubscription>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		Throw.IfArgumentNull(source);

		return source.MapTo(target, referenceModifier, conditions, instanceFactory, cache);
	}

	public MessageBox.Model.TopicSubscription? Clone(
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<MessageBox.Model.TopicSubscription>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
		=> MapTo(target: null, referenceModifier, conditions, instanceFactory, cache);

	public MessageBox.Model.TopicSubscription? MapTo(
		MessageBox.Model.TopicSubscription? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<MessageBox.Model.TopicSubscription>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		cache ??= [];
		target ??= instanceFactory?.CreateInstance<Legion.ADF.Messaging.MessageBox.Model.TopicSubscription>(
				useActivatorIfNoFactoryFound: false,
				throwIfFactoryReturnsNull: true)
			?? new Legion.ADF.Messaging.MessageBox.Model.TopicSubscription();

		if (cache.TryGetValue(this, out var cached))
			return (MessageBox.Model.TopicSubscription)cached;
			
		MappingConditions<MessageBox.Model.TopicSubscription>? conds = null;
		if (conditions != null)
		{
			conds = new MappingConditions<MessageBox.Model.TopicSubscription>();
			conditions.Invoke(conds);

			if (conds.CanMap(this, nameof(IdTopicSubscription)))
				target.IdTopicSubscription = IdTopicSubscription;
			if (conds.CanMap(this, nameof(IdTopic)))
				target.IdTopic = IdTopic;
			if (conds.CanMap(this, nameof(SubscriptionName)))
				target.SubscriptionName = SubscriptionName;
			if (conds.CanMap(this, nameof(ReceivedEventNamespace)))
				target.ReceivedEventNamespace = ReceivedEventNamespace;
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
			if (conds.CanMap(this, nameof(IdJob)))
				target.IdJob = IdJob;
			if (conds.CanMap(this, nameof(IdOrchestration)))
				target.IdOrchestration = IdOrchestration;
			if (conds.CanMap(this, nameof(IdMessageBoxInstance)))
				target.IdMessageBoxInstance = IdMessageBoxInstance;
		}
		else
		{
			target.IdTopicSubscription = IdTopicSubscription;
			target.IdTopic = IdTopic;
			target.SubscriptionName = SubscriptionName;
			target.ReceivedEventNamespace = ReceivedEventNamespace;
			target.IsActive = IsActive;
			target.IsSequentialFIFO = IsSequentialFIFO;
			target.MessagesBatchCount = MessagesBatchCount;
			target.MaxDegreeOfParallelism = MaxDegreeOfParallelism;
			target.TimeoutForMessageProcessing = TimeoutForMessageProcessing;
			target.MaxMessageProcessingRetryCount = MaxMessageProcessingRetryCount;
			target.Properties = Properties;
			target.IdProcessingMode = IdProcessingMode;
			target.IdSuspendingMode = IdSuspendingMode;
			target.IdJob = IdJob;
			target.IdOrchestration = IdOrchestration;
			target.IdMessageBoxInstance = IdMessageBoxInstance;
		}

		cache.Add(this, target);

		if (referenceModifier == ReferenceModifier.MapAllReferences)
		{
			target.MessageBoxInstance = MessageBoxInstance?.MapTo(target.MessageBoxInstance, referenceModifier, conds?.GetConditions(x => x.MessageBoxInstance), instanceFactory, cache)!;
			target.ProcessingMode = ProcessingMode?.MapTo(target.ProcessingMode, referenceModifier, conds?.GetConditions(x => x.ProcessingMode), instanceFactory, cache)!;
			target.SuspendingMode = SuspendingMode?.MapTo(target.SuspendingMode, referenceModifier, conds?.GetConditions(x => x.SuspendingMode), instanceFactory, cache)!;
			target.Topic = Topic?.MapTo(target.Topic, referenceModifier, conds?.GetConditions(x => x.Topic), instanceFactory, cache)!;
			target._messageBoxProcessingLogs = MapperHelper.MapToList(MessageBoxProcessingLogs, target._messageBoxProcessingLogs, MessageBoxProcessingLog.Map, referenceModifier, conds?.GetConditions(x => x.MessageBoxProcessingLogs), instanceFactory, cache)!;
			target._subscribedMessages = MapperHelper.MapToList(SubscribedMessages, target._subscribedMessages, SubscribedMessage.Map, referenceModifier, conds?.GetConditions(x => x.SubscribedMessages), instanceFactory, cache)!;
		}
		else if (referenceModifier == ReferenceModifier.SetNull)
		{
			target.MessageBoxInstance = null!;
			target.ProcessingMode = null!;
			target.SuspendingMode = null!;
			target.Topic = null!;
			target._messageBoxProcessingLogs = [];
			target._subscribedMessages = [];
		}

		return target;
	}
}
