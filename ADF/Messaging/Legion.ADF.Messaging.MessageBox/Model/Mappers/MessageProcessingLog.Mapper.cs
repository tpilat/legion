using Legion;
using Legion.Model.Mappers;

namespace Legion.ADF.Messaging.MessageBox.Model;

public sealed partial class MessageProcessingLog : MessageBox.MessageBoxBaseEntity, Legion.Model.IEntity
{
	public static MessageBox.Model.MessageProcessingLog? Map(
		MessageBox.Model.MessageProcessingLog source,
		MessageBox.Model.MessageProcessingLog? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<MessageBox.Model.MessageProcessingLog>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		Throw.IfArgumentNull(source);

		return source.MapTo(target, referenceModifier, conditions, instanceFactory, cache);
	}

	public MessageBox.Model.MessageProcessingLog? Clone(
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<MessageBox.Model.MessageProcessingLog>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
		=> MapTo(target: null, referenceModifier, conditions, instanceFactory, cache);

	public MessageBox.Model.MessageProcessingLog? MapTo(
		MessageBox.Model.MessageProcessingLog? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<MessageBox.Model.MessageProcessingLog>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		cache ??= [];
		target ??= instanceFactory?.CreateInstance<Legion.ADF.Messaging.MessageBox.Model.MessageProcessingLog>(
				useActivatorIfNoFactoryFound: false,
				throwIfFactoryReturnsNull: true)
			?? new Legion.ADF.Messaging.MessageBox.Model.MessageProcessingLog();

		if (cache.TryGetValue(this, out var cached))
			return (MessageBox.Model.MessageProcessingLog)cached;
			
		MappingConditions<MessageBox.Model.MessageProcessingLog>? conds = null;
		if (conditions != null)
		{
			conds = new MappingConditions<MessageBox.Model.MessageProcessingLog>();
			conditions.Invoke(conds);

			if (conds.CanMap(this, nameof(IdMessageProcessingLog)))
				target.IdMessageProcessingLog = IdMessageProcessingLog;
			if (conds.CanMap(this, nameof(IdMessage)))
				target.IdMessage = IdMessage;
			if (conds.CanMap(this, nameof(IdQueuedMessage)))
				target.IdQueuedMessage = IdQueuedMessage;
			if (conds.CanMap(this, nameof(IdSubscribedMessage)))
				target.IdSubscribedMessage = IdSubscribedMessage;
			if (conds.CanMap(this, nameof(CreatedUtc)))
				target.CreatedUtc = CreatedUtc;
			if (conds.CanMap(this, nameof(IdMessageProcessingStatus)))
				target.IdMessageProcessingStatus = IdMessageProcessingStatus;
			if (conds.CanMap(this, nameof(TraceCorrelationId)))
				target.TraceCorrelationId = TraceCorrelationId;
			if (conds.CanMap(this, nameof(IdLogMessage)))
				target.IdLogMessage = IdLogMessage;
			if (conds.CanMap(this, nameof(Code)))
				target.Code = Code;
			if (conds.CanMap(this, nameof(Detail)))
				target.Detail = Detail;
			if (conds.CanMap(this, nameof(IdMessageBoxInstance)))
				target.IdMessageBoxInstance = IdMessageBoxInstance;
		}
		else
		{
			target.IdMessageProcessingLog = IdMessageProcessingLog;
			target.IdMessage = IdMessage;
			target.IdQueuedMessage = IdQueuedMessage;
			target.IdSubscribedMessage = IdSubscribedMessage;
			target.CreatedUtc = CreatedUtc;
			target.IdMessageProcessingStatus = IdMessageProcessingStatus;
			target.TraceCorrelationId = TraceCorrelationId;
			target.IdLogMessage = IdLogMessage;
			target.Code = Code;
			target.Detail = Detail;
			target.IdMessageBoxInstance = IdMessageBoxInstance;
		}

		cache.Add(this, target);

		if (referenceModifier == ReferenceModifier.MapAllReferences)
		{
			target.MessageBoxInstance = MessageBoxInstance?.MapTo(target.MessageBoxInstance, referenceModifier, conds?.GetConditions(x => x.MessageBoxInstance), instanceFactory, cache)!;
			target.MessageProcessingStatus = MessageProcessingStatus?.MapTo(target.MessageProcessingStatus, referenceModifier, conds?.GetConditions(x => x.MessageProcessingStatus), instanceFactory, cache)!;
			target.QueuedMessage = QueuedMessage?.MapTo(target.QueuedMessage, referenceModifier, conds?.GetConditions(x => x.QueuedMessage), instanceFactory, cache)!;
			target.SubscribedMessage = SubscribedMessage?.MapTo(target.SubscribedMessage, referenceModifier, conds?.GetConditions(x => x.SubscribedMessage), instanceFactory, cache)!;
		}
		else if (referenceModifier == ReferenceModifier.SetNull)
		{
			target.MessageBoxInstance = null!;
			target.MessageProcessingStatus = null!;
			target.QueuedMessage = null!;
			target.SubscribedMessage = null!;
		}

		return target;
	}
}
