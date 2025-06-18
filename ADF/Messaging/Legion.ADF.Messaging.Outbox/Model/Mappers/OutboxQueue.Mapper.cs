using Legion;
using Legion.Model.Mappers;

namespace Legion.ADF.Messaging.Outbox.Model;

public sealed partial class OutboxQueue : Outbox.OutboxBaseEntity, Legion.Model.IEntity
{
	public static Outbox.Model.OutboxQueue? Map(
		Outbox.Model.OutboxQueue source,
		Outbox.Model.OutboxQueue? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Outbox.Model.OutboxQueue>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		Throw.IfArgumentNull(source);

		return source.MapTo(target, referenceModifier, conditions, instanceFactory, cache);
	}

	public Outbox.Model.OutboxQueue? Clone(
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Outbox.Model.OutboxQueue>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
		=> MapTo(target: null, referenceModifier, conditions, instanceFactory, cache);

	public Outbox.Model.OutboxQueue? MapTo(
		Outbox.Model.OutboxQueue? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Outbox.Model.OutboxQueue>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		cache ??= [];
		target ??= instanceFactory?.CreateInstance<Legion.ADF.Messaging.Outbox.Model.OutboxQueue>(
				useActivatorIfNoFactoryFound: false,
				throwIfFactoryReturnsNull: true)
			?? new Legion.ADF.Messaging.Outbox.Model.OutboxQueue();

		if (cache.TryGetValue(this, out var cached))
			return (Outbox.Model.OutboxQueue)cached;
			
		MappingConditions<Outbox.Model.OutboxQueue>? conds = null;
		if (conditions != null)
		{
			conds = new MappingConditions<Outbox.Model.OutboxQueue>();
			conditions.Invoke(conds);

			if (conds.CanMap(this, nameof(IdOutboxQueue)))
				target.IdOutboxQueue = IdOutboxQueue;
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
			if (conds.CanMap(this, nameof(IdOutboxInstance)))
				target.IdOutboxInstance = IdOutboxInstance;
		}
		else
		{
			target.IdOutboxQueue = IdOutboxQueue;
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
			target.IdOutboxInstance = IdOutboxInstance;
		}

		cache.Add(this, target);

		if (referenceModifier == ReferenceModifier.MapAllReferences)
		{
			target.MessageType = MessageType?.MapTo(target.MessageType, referenceModifier, conds?.GetConditions(x => x.MessageType), instanceFactory, cache)!;
			target.OutboxInstance = OutboxInstance?.MapTo(target.OutboxInstance, referenceModifier, conds?.GetConditions(x => x.OutboxInstance), instanceFactory, cache)!;
			target.ProcessingMode = ProcessingMode?.MapTo(target.ProcessingMode, referenceModifier, conds?.GetConditions(x => x.ProcessingMode), instanceFactory, cache)!;
			target.SuspendingMode = SuspendingMode?.MapTo(target.SuspendingMode, referenceModifier, conds?.GetConditions(x => x.SuspendingMode), instanceFactory, cache)!;
			target._outboxMessageArchives = MapperHelper.MapToList(OutboxMessageArchives, target._outboxMessageArchives, OutboxMessageArchive.Map, referenceModifier, conds?.GetConditions(x => x.OutboxMessageArchives), instanceFactory, cache)!;
			target._outboxMessageProcessingLogs = MapperHelper.MapToList(OutboxMessageProcessingLogs, target._outboxMessageProcessingLogs, OutboxMessageProcessingLog.Map, referenceModifier, conds?.GetConditions(x => x.OutboxMessageProcessingLogs), instanceFactory, cache)!;
			target._outboxMessages = MapperHelper.MapToList(OutboxMessages, target._outboxMessages, OutboxMessage.Map, referenceModifier, conds?.GetConditions(x => x.OutboxMessages), instanceFactory, cache)!;
			target._outboxProcessingLogs = MapperHelper.MapToList(OutboxProcessingLogs, target._outboxProcessingLogs, OutboxProcessingLog.Map, referenceModifier, conds?.GetConditions(x => x.OutboxProcessingLogs), instanceFactory, cache)!;
		}
		else if (referenceModifier == ReferenceModifier.SetNull)
		{
			target.MessageType = null!;
			target.OutboxInstance = null!;
			target.ProcessingMode = null!;
			target.SuspendingMode = null!;
			target._outboxMessageArchives = [];
			target._outboxMessageProcessingLogs = [];
			target._outboxMessages = [];
			target._outboxProcessingLogs = [];
		}

		return target;
	}
}
