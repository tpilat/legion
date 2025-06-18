using Legion;
using Legion.Model.Mappers;

namespace Legion.ADF.Messaging.MessageBox.Model;

public sealed partial class MessageProcessingStatus : MessageBox.MessageBoxBaseEntity, Legion.Model.IEntity
{
	public static MessageBox.Model.MessageProcessingStatus? Map(
		MessageBox.Model.MessageProcessingStatus source,
		MessageBox.Model.MessageProcessingStatus? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<MessageBox.Model.MessageProcessingStatus>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		Throw.IfArgumentNull(source);

		return source.MapTo(target, referenceModifier, conditions, instanceFactory, cache);
	}

	public MessageBox.Model.MessageProcessingStatus? Clone(
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<MessageBox.Model.MessageProcessingStatus>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
		=> MapTo(target: null, referenceModifier, conditions, instanceFactory, cache);

	public MessageBox.Model.MessageProcessingStatus? MapTo(
		MessageBox.Model.MessageProcessingStatus? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<MessageBox.Model.MessageProcessingStatus>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		cache ??= [];
		target ??= Legion.ADF.Messaging.MessageBox.Model.MessageProcessingStatus.DictionaryMap.Value[IdMessageProcessingStatus];

		if (cache.TryGetValue(this, out var cached))
			return (MessageBox.Model.MessageProcessingStatus)cached;
			
		MappingConditions<MessageBox.Model.MessageProcessingStatus>? conds = null;
		if (conditions != null)
		{
			conds = new MappingConditions<MessageBox.Model.MessageProcessingStatus>();
			conditions.Invoke(conds);
		}

		cache.Add(this, target);

		if (referenceModifier == ReferenceModifier.MapAllReferences)
		{
			target._messageProcessingLogs = MapperHelper.MapToList(MessageProcessingLogs, target._messageProcessingLogs, MessageProcessingLog.Map, referenceModifier, conds?.GetConditions(x => x.MessageProcessingLogs), instanceFactory, cache)!;
			target._queuedMessages = MapperHelper.MapToList(QueuedMessages, target._queuedMessages, QueuedMessage.Map, referenceModifier, conds?.GetConditions(x => x.QueuedMessages), instanceFactory, cache)!;
			target._subscribedMessages = MapperHelper.MapToList(SubscribedMessages, target._subscribedMessages, SubscribedMessage.Map, referenceModifier, conds?.GetConditions(x => x.SubscribedMessages), instanceFactory, cache)!;
		}
		else if (referenceModifier == ReferenceModifier.SetNull)
		{
			target._messageProcessingLogs = [];
			target._queuedMessages = [];
			target._subscribedMessages = [];
		}

		return target;
	}
}
