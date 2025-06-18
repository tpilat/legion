using Legion;
using Legion.Model.Mappers;

namespace Legion.ADF.Messaging.MessageBox.Model;

public sealed partial class MessageBoxProcessingLog : MessageBox.MessageBoxBaseEntity, Legion.Model.IEntity
{
	public static MessageBox.Model.MessageBoxProcessingLog? Map(
		MessageBox.Model.MessageBoxProcessingLog source,
		MessageBox.Model.MessageBoxProcessingLog? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<MessageBox.Model.MessageBoxProcessingLog>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		Throw.IfArgumentNull(source);

		return source.MapTo(target, referenceModifier, conditions, instanceFactory, cache);
	}

	public MessageBox.Model.MessageBoxProcessingLog? Clone(
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<MessageBox.Model.MessageBoxProcessingLog>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
		=> MapTo(target: null, referenceModifier, conditions, instanceFactory, cache);

	public MessageBox.Model.MessageBoxProcessingLog? MapTo(
		MessageBox.Model.MessageBoxProcessingLog? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<MessageBox.Model.MessageBoxProcessingLog>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		cache ??= [];
		target ??= instanceFactory?.CreateInstance<Legion.ADF.Messaging.MessageBox.Model.MessageBoxProcessingLog>(
				useActivatorIfNoFactoryFound: false,
				throwIfFactoryReturnsNull: true)
			?? new Legion.ADF.Messaging.MessageBox.Model.MessageBoxProcessingLog();

		if (cache.TryGetValue(this, out var cached))
			return (MessageBox.Model.MessageBoxProcessingLog)cached;
			
		MappingConditions<MessageBox.Model.MessageBoxProcessingLog>? conds = null;
		if (conditions != null)
		{
			conds = new MappingConditions<MessageBox.Model.MessageBoxProcessingLog>();
			conditions.Invoke(conds);

			if (conds.CanMap(this, nameof(IdMessageBoxProcessingLog)))
				target.IdMessageBoxProcessingLog = IdMessageBoxProcessingLog;
			if (conds.CanMap(this, nameof(IdMessageBoxInstance)))
				target.IdMessageBoxInstance = IdMessageBoxInstance;
			if (conds.CanMap(this, nameof(IdQueue)))
				target.IdQueue = IdQueue;
			if (conds.CanMap(this, nameof(IdTopic)))
				target.IdTopic = IdTopic;
			if (conds.CanMap(this, nameof(IdTopicSubscription)))
				target.IdTopicSubscription = IdTopicSubscription;
			if (conds.CanMap(this, nameof(CreatedUtc)))
				target.CreatedUtc = CreatedUtc;
			if (conds.CanMap(this, nameof(IdLogLevel)))
				target.IdLogLevel = IdLogLevel;
			if (conds.CanMap(this, nameof(TraceCorrelationId)))
				target.TraceCorrelationId = TraceCorrelationId;
			if (conds.CanMap(this, nameof(IdLogMessage)))
				target.IdLogMessage = IdLogMessage;
			if (conds.CanMap(this, nameof(Code)))
				target.Code = Code;
			if (conds.CanMap(this, nameof(Detail)))
				target.Detail = Detail;
		}
		else
		{
			target.IdMessageBoxProcessingLog = IdMessageBoxProcessingLog;
			target.IdMessageBoxInstance = IdMessageBoxInstance;
			target.IdQueue = IdQueue;
			target.IdTopic = IdTopic;
			target.IdTopicSubscription = IdTopicSubscription;
			target.CreatedUtc = CreatedUtc;
			target.IdLogLevel = IdLogLevel;
			target.TraceCorrelationId = TraceCorrelationId;
			target.IdLogMessage = IdLogMessage;
			target.Code = Code;
			target.Detail = Detail;
		}

		cache.Add(this, target);

		if (referenceModifier == ReferenceModifier.MapAllReferences)
		{
			target.MessageBoxInstance = MessageBoxInstance?.MapTo(target.MessageBoxInstance, referenceModifier, conds?.GetConditions(x => x.MessageBoxInstance), instanceFactory, cache)!;
			target.Queue = Queue?.MapTo(target.Queue, referenceModifier, conds?.GetConditions(x => x.Queue), instanceFactory, cache)!;
			target.Topic = Topic?.MapTo(target.Topic, referenceModifier, conds?.GetConditions(x => x.Topic), instanceFactory, cache)!;
			target.TopicSubscription = TopicSubscription?.MapTo(target.TopicSubscription, referenceModifier, conds?.GetConditions(x => x.TopicSubscription), instanceFactory, cache)!;
		}
		else if (referenceModifier == ReferenceModifier.SetNull)
		{
			target.MessageBoxInstance = null!;
			target.Queue = null!;
			target.Topic = null!;
			target.TopicSubscription = null!;
		}

		return target;
	}
}
