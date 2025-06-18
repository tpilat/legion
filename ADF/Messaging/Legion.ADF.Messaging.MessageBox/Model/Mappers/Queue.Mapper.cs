using Legion;
using Legion.Model.Mappers;

namespace Legion.ADF.Messaging.MessageBox.Model;

public sealed partial class Queue : MessageBox.MessageBoxBaseEntity, Legion.Model.IEntity
{
	public static MessageBox.Model.Queue? Map(
		MessageBox.Model.Queue source,
		MessageBox.Model.Queue? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<MessageBox.Model.Queue>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		Throw.IfArgumentNull(source);

		return source.MapTo(target, referenceModifier, conditions, instanceFactory, cache);
	}

	public MessageBox.Model.Queue? Clone(
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<MessageBox.Model.Queue>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
		=> MapTo(target: null, referenceModifier, conditions, instanceFactory, cache);

	public MessageBox.Model.Queue? MapTo(
		MessageBox.Model.Queue? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<MessageBox.Model.Queue>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		cache ??= [];
		target ??= instanceFactory?.CreateInstance<Legion.ADF.Messaging.MessageBox.Model.Queue>(
				useActivatorIfNoFactoryFound: false,
				throwIfFactoryReturnsNull: true)
			?? new Legion.ADF.Messaging.MessageBox.Model.Queue();

		if (cache.TryGetValue(this, out var cached))
			return (MessageBox.Model.Queue)cached;
			
		MappingConditions<MessageBox.Model.Queue>? conds = null;
		if (conditions != null)
		{
			conds = new MappingConditions<MessageBox.Model.Queue>();
			conditions.Invoke(conds);

			if (conds.CanMap(this, nameof(IdQueue)))
				target.IdQueue = IdQueue;
			if (conds.CanMap(this, nameof(Name)))
				target.Name = Name;
			if (conds.CanMap(this, nameof(ReceivedEventNamespace)))
				target.ReceivedEventNamespace = ReceivedEventNamespace;
			if (conds.CanMap(this, nameof(IdMessageType)))
				target.IdMessageType = IdMessageType;
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
			target.IdQueue = IdQueue;
			target.Name = Name;
			target.ReceivedEventNamespace = ReceivedEventNamespace;
			target.IdMessageType = IdMessageType;
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
			target.MessageType = MessageType?.MapTo(target.MessageType, referenceModifier, conds?.GetConditions(x => x.MessageType), instanceFactory, cache)!;
			target.ProcessingMode = ProcessingMode?.MapTo(target.ProcessingMode, referenceModifier, conds?.GetConditions(x => x.ProcessingMode), instanceFactory, cache)!;
			target.SuspendingMode = SuspendingMode?.MapTo(target.SuspendingMode, referenceModifier, conds?.GetConditions(x => x.SuspendingMode), instanceFactory, cache)!;
			target._messageArchives = MapperHelper.MapToList(MessageArchives, target._messageArchives, MessageArchive.Map, referenceModifier, conds?.GetConditions(x => x.MessageArchives), instanceFactory, cache)!;
			target._messageBoxProcessingLogs = MapperHelper.MapToList(MessageBoxProcessingLogs, target._messageBoxProcessingLogs, MessageBoxProcessingLog.Map, referenceModifier, conds?.GetConditions(x => x.MessageBoxProcessingLogs), instanceFactory, cache)!;
			target._messages = MapperHelper.MapToList(Messages, target._messages, Message.Map, referenceModifier, conds?.GetConditions(x => x.Messages), instanceFactory, cache)!;
			target._queuedMessages = MapperHelper.MapToList(QueuedMessages, target._queuedMessages, QueuedMessage.Map, referenceModifier, conds?.GetConditions(x => x.QueuedMessages), instanceFactory, cache)!;
		}
		else if (referenceModifier == ReferenceModifier.SetNull)
		{
			target.MessageBoxInstance = null!;
			target.MessageType = null!;
			target.ProcessingMode = null!;
			target.SuspendingMode = null!;
			target._messageArchives = [];
			target._messageBoxProcessingLogs = [];
			target._messages = [];
			target._queuedMessages = [];
		}

		return target;
	}
}
