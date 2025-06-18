using Legion;
using Legion.Model.Mappers;

namespace Legion.ADF.Messaging.MessageBox.Model;

public sealed partial class SubscribedMessage : MessageBox.MessageBoxBaseEntity, Legion.Model.IEntity
{
	public static MessageBox.Model.SubscribedMessage? Map(
		MessageBox.Model.SubscribedMessage source,
		MessageBox.Model.SubscribedMessage? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<MessageBox.Model.SubscribedMessage>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		Throw.IfArgumentNull(source);

		return source.MapTo(target, referenceModifier, conditions, instanceFactory, cache);
	}

	public MessageBox.Model.SubscribedMessage? Clone(
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<MessageBox.Model.SubscribedMessage>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
		=> MapTo(target: null, referenceModifier, conditions, instanceFactory, cache);

	public MessageBox.Model.SubscribedMessage? MapTo(
		MessageBox.Model.SubscribedMessage? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<MessageBox.Model.SubscribedMessage>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		cache ??= [];
		target ??= instanceFactory?.CreateInstance<Legion.ADF.Messaging.MessageBox.Model.SubscribedMessage>(
				useActivatorIfNoFactoryFound: false,
				throwIfFactoryReturnsNull: true)
			?? new Legion.ADF.Messaging.MessageBox.Model.SubscribedMessage();

		if (cache.TryGetValue(this, out var cached))
			return (MessageBox.Model.SubscribedMessage)cached;
			
		MappingConditions<MessageBox.Model.SubscribedMessage>? conds = null;
		if (conditions != null)
		{
			conds = new MappingConditions<MessageBox.Model.SubscribedMessage>();
			conditions.Invoke(conds);

			if (conds.CanMap(this, nameof(IdSubscribedMessage)))
				target.IdSubscribedMessage = IdSubscribedMessage;
			if (conds.CanMap(this, nameof(IdTopicSubscription)))
				target.IdTopicSubscription = IdTopicSubscription;
			if (conds.CanMap(this, nameof(IdMessage)))
				target.IdMessage = IdMessage;
			if (conds.CanMap(this, nameof(IdMessageProcessingStatus)))
				target.IdMessageProcessingStatus = IdMessageProcessingStatus;
			if (conds.CanMap(this, nameof(AssignedUtc)))
				target.AssignedUtc = AssignedUtc;
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
			if (conds.CanMap(this, nameof(IdMessageBoxInstance)))
				target.IdMessageBoxInstance = IdMessageBoxInstance;
		}
		else
		{
			target.IdSubscribedMessage = IdSubscribedMessage;
			target.IdTopicSubscription = IdTopicSubscription;
			target.IdMessage = IdMessage;
			target.IdMessageProcessingStatus = IdMessageProcessingStatus;
			target.AssignedUtc = AssignedUtc;
			target.ProcessedUtc = ProcessedUtc;
			target.SuspendedUtc = SuspendedUtc;
			target.LastProcessingUtc = LastProcessingUtc;
			target.LastProcessingTimeoutUtc = LastProcessingTimeoutUtc;
			target.NextProcessingUtc = NextProcessingUtc;
			target.RetryCount = RetryCount;
			target.IdMessageBoxInstance = IdMessageBoxInstance;
		}

		cache.Add(this, target);

		if (referenceModifier == ReferenceModifier.MapAllReferences)
		{
			target.MessageBoxInstance = MessageBoxInstance?.MapTo(target.MessageBoxInstance, referenceModifier, conds?.GetConditions(x => x.MessageBoxInstance), instanceFactory, cache)!;
			target.MessageProcessingStatus = MessageProcessingStatus?.MapTo(target.MessageProcessingStatus, referenceModifier, conds?.GetConditions(x => x.MessageProcessingStatus), instanceFactory, cache)!;
			target.TopicSubscription = TopicSubscription?.MapTo(target.TopicSubscription, referenceModifier, conds?.GetConditions(x => x.TopicSubscription), instanceFactory, cache)!;
			target._messageProcessingLogs = MapperHelper.MapToList(MessageProcessingLogs, target._messageProcessingLogs, MessageProcessingLog.Map, referenceModifier, conds?.GetConditions(x => x.MessageProcessingLogs), instanceFactory, cache)!;
		}
		else if (referenceModifier == ReferenceModifier.SetNull)
		{
			target.MessageBoxInstance = null!;
			target.MessageProcessingStatus = null!;
			target.TopicSubscription = null!;
			target._messageProcessingLogs = [];
		}

		return target;
	}
}
