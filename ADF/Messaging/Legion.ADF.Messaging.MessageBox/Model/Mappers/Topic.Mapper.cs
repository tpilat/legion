using Legion;
using Legion.Model.Mappers;

namespace Legion.ADF.Messaging.MessageBox.Model;

public sealed partial class Topic : MessageBox.MessageBoxBaseEntity, Legion.Model.IEntity
{
	public static MessageBox.Model.Topic? Map(
		MessageBox.Model.Topic source,
		MessageBox.Model.Topic? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<MessageBox.Model.Topic>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		Throw.IfArgumentNull(source);

		return source.MapTo(target, referenceModifier, conditions, instanceFactory, cache);
	}

	public MessageBox.Model.Topic? Clone(
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<MessageBox.Model.Topic>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
		=> MapTo(target: null, referenceModifier, conditions, instanceFactory, cache);

	public MessageBox.Model.Topic? MapTo(
		MessageBox.Model.Topic? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<MessageBox.Model.Topic>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		cache ??= [];
		target ??= instanceFactory?.CreateInstance<Legion.ADF.Messaging.MessageBox.Model.Topic>(
				useActivatorIfNoFactoryFound: false,
				throwIfFactoryReturnsNull: true)
			?? new Legion.ADF.Messaging.MessageBox.Model.Topic();

		if (cache.TryGetValue(this, out var cached))
			return (MessageBox.Model.Topic)cached;
			
		MappingConditions<MessageBox.Model.Topic>? conds = null;
		if (conditions != null)
		{
			conds = new MappingConditions<MessageBox.Model.Topic>();
			conditions.Invoke(conds);

			if (conds.CanMap(this, nameof(IdTopic)))
				target.IdTopic = IdTopic;
			if (conds.CanMap(this, nameof(Name)))
				target.Name = Name;
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
			if (conds.CanMap(this, nameof(IdMessageBoxInstance)))
				target.IdMessageBoxInstance = IdMessageBoxInstance;
		}
		else
		{
			target.IdTopic = IdTopic;
			target.Name = Name;
			target.IsActive = IsActive;
			target.IsSequentialFIFO = IsSequentialFIFO;
			target.MessagesBatchCount = MessagesBatchCount;
			target.MaxDegreeOfParallelism = MaxDegreeOfParallelism;
			target.TimeoutForMessageProcessing = TimeoutForMessageProcessing;
			target.MaxMessageProcessingRetryCount = MaxMessageProcessingRetryCount;
			target.Properties = Properties;
			target.IdProcessingMode = IdProcessingMode;
			target.IdSuspendingMode = IdSuspendingMode;
			target.IdMessageBoxInstance = IdMessageBoxInstance;
		}

		cache.Add(this, target);

		if (referenceModifier == ReferenceModifier.MapAllReferences)
		{
			target.MessageBoxInstance = MessageBoxInstance?.MapTo(target.MessageBoxInstance, referenceModifier, conds?.GetConditions(x => x.MessageBoxInstance), instanceFactory, cache)!;
			target.ProcessingMode = ProcessingMode?.MapTo(target.ProcessingMode, referenceModifier, conds?.GetConditions(x => x.ProcessingMode), instanceFactory, cache)!;
			target.SuspendingMode = SuspendingMode?.MapTo(target.SuspendingMode, referenceModifier, conds?.GetConditions(x => x.SuspendingMode), instanceFactory, cache)!;
			target._messageArchives = MapperHelper.MapToList(MessageArchives, target._messageArchives, MessageArchive.Map, referenceModifier, conds?.GetConditions(x => x.MessageArchives), instanceFactory, cache)!;
			target._messageBoxProcessingLogs = MapperHelper.MapToList(MessageBoxProcessingLogs, target._messageBoxProcessingLogs, MessageBoxProcessingLog.Map, referenceModifier, conds?.GetConditions(x => x.MessageBoxProcessingLogs), instanceFactory, cache)!;
			target._messages = MapperHelper.MapToList(Messages, target._messages, Message.Map, referenceModifier, conds?.GetConditions(x => x.Messages), instanceFactory, cache)!;
			target._topicSubscriptions = MapperHelper.MapToList(TopicSubscriptions, target._topicSubscriptions, TopicSubscription.Map, referenceModifier, conds?.GetConditions(x => x.TopicSubscriptions), instanceFactory, cache)!;
		}
		else if (referenceModifier == ReferenceModifier.SetNull)
		{
			target.MessageBoxInstance = null!;
			target.ProcessingMode = null!;
			target.SuspendingMode = null!;
			target._messageArchives = [];
			target._messageBoxProcessingLogs = [];
			target._messages = [];
			target._topicSubscriptions = [];
		}

		return target;
	}
}
