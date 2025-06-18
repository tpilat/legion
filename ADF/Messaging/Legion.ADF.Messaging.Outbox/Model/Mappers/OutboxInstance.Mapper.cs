using Legion;
using Legion.Model.Mappers;

namespace Legion.ADF.Messaging.Outbox.Model;

public sealed partial class OutboxInstance : Outbox.OutboxBaseEntity, Legion.Model.IEntity
{
	public static Outbox.Model.OutboxInstance? Map(
		Outbox.Model.OutboxInstance source,
		Outbox.Model.OutboxInstance? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Outbox.Model.OutboxInstance>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		Throw.IfArgumentNull(source);

		return source.MapTo(target, referenceModifier, conditions, instanceFactory, cache);
	}

	public Outbox.Model.OutboxInstance? Clone(
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Outbox.Model.OutboxInstance>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
		=> MapTo(target: null, referenceModifier, conditions, instanceFactory, cache);

	public Outbox.Model.OutboxInstance? MapTo(
		Outbox.Model.OutboxInstance? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Outbox.Model.OutboxInstance>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		cache ??= [];
		target ??= instanceFactory?.CreateInstance<Legion.ADF.Messaging.Outbox.Model.OutboxInstance>(
				useActivatorIfNoFactoryFound: false,
				throwIfFactoryReturnsNull: true)
			?? new Legion.ADF.Messaging.Outbox.Model.OutboxInstance();

		if (cache.TryGetValue(this, out var cached))
			return (Outbox.Model.OutboxInstance)cached;
			
		MappingConditions<Outbox.Model.OutboxInstance>? conds = null;
		if (conditions != null)
		{
			conds = new MappingConditions<Outbox.Model.OutboxInstance>();
			conditions.Invoke(conds);

			if (conds.CanMap(this, nameof(IdOutboxInstance)))
				target.IdOutboxInstance = IdOutboxInstance;
			if (conds.CanMap(this, nameof(CreatedUtc)))
				target.CreatedUtc = CreatedUtc;
			if (conds.CanMap(this, nameof(Name)))
				target.Name = Name;
			if (conds.CanMap(this, nameof(Version)))
				target.Version = Version;
			if (conds.CanMap(this, nameof(MaxDegreeOfQueueParallelism)))
				target.MaxDegreeOfQueueParallelism = MaxDegreeOfQueueParallelism;
			if (conds.CanMap(this, nameof(IdLogLevel)))
				target.IdLogLevel = IdLogLevel;
		}
		else
		{
			target.IdOutboxInstance = IdOutboxInstance;
			target.CreatedUtc = CreatedUtc;
			target.Name = Name;
			target.Version = Version;
			target.MaxDegreeOfQueueParallelism = MaxDegreeOfQueueParallelism;
			target.IdLogLevel = IdLogLevel;
		}

		cache.Add(this, target);

		if (referenceModifier == ReferenceModifier.MapAllReferences)
		{
			target._blockedOutboxMessageTypes = MapperHelper.MapToList(BlockedOutboxMessageTypes, target._blockedOutboxMessageTypes, BlockedOutboxMessageType.Map, referenceModifier, conds?.GetConditions(x => x.BlockedOutboxMessageTypes), instanceFactory, cache)!;
			target._outboxMessageArchives = MapperHelper.MapToList(OutboxMessageArchives, target._outboxMessageArchives, OutboxMessageArchive.Map, referenceModifier, conds?.GetConditions(x => x.OutboxMessageArchives), instanceFactory, cache)!;
			target._outboxMessageProcessingLogs = MapperHelper.MapToList(OutboxMessageProcessingLogs, target._outboxMessageProcessingLogs, OutboxMessageProcessingLog.Map, referenceModifier, conds?.GetConditions(x => x.OutboxMessageProcessingLogs), instanceFactory, cache)!;
			target._outboxMessages = MapperHelper.MapToList(OutboxMessages, target._outboxMessages, OutboxMessage.Map, referenceModifier, conds?.GetConditions(x => x.OutboxMessages), instanceFactory, cache)!;
			target._outboxMessageTypes = MapperHelper.MapToList(OutboxMessageTypes, target._outboxMessageTypes, OutboxMessageType.Map, referenceModifier, conds?.GetConditions(x => x.OutboxMessageTypes), instanceFactory, cache)!;
			target._outboxProcessingLogs = MapperHelper.MapToList(OutboxProcessingLogs, target._outboxProcessingLogs, OutboxProcessingLog.Map, referenceModifier, conds?.GetConditions(x => x.OutboxProcessingLogs), instanceFactory, cache)!;
			target._outboxQueues = MapperHelper.MapToList(OutboxQueues, target._outboxQueues, OutboxQueue.Map, referenceModifier, conds?.GetConditions(x => x.OutboxQueues), instanceFactory, cache)!;
		}
		else if (referenceModifier == ReferenceModifier.SetNull)
		{
			target._blockedOutboxMessageTypes = [];
			target._outboxMessageArchives = [];
			target._outboxMessageProcessingLogs = [];
			target._outboxMessages = [];
			target._outboxMessageTypes = [];
			target._outboxProcessingLogs = [];
			target._outboxQueues = [];
		}

		return target;
	}
}
