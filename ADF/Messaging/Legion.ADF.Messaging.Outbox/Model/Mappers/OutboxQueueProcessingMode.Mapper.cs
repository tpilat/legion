using Legion;
using Legion.Model.Mappers;

namespace Legion.ADF.Messaging.Outbox.Model;

public sealed partial class OutboxQueueProcessingMode : Outbox.OutboxBaseEntity, Legion.Model.IEntity
{
	public static Outbox.Model.OutboxQueueProcessingMode? Map(
		Outbox.Model.OutboxQueueProcessingMode source,
		Outbox.Model.OutboxQueueProcessingMode? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Outbox.Model.OutboxQueueProcessingMode>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		Throw.IfArgumentNull(source);

		return source.MapTo(target, referenceModifier, conditions, instanceFactory, cache);
	}

	public Outbox.Model.OutboxQueueProcessingMode? Clone(
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Outbox.Model.OutboxQueueProcessingMode>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
		=> MapTo(target: null, referenceModifier, conditions, instanceFactory, cache);

	public Outbox.Model.OutboxQueueProcessingMode? MapTo(
		Outbox.Model.OutboxQueueProcessingMode? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Outbox.Model.OutboxQueueProcessingMode>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		cache ??= [];
		target ??= Legion.ADF.Messaging.Outbox.Model.OutboxQueueProcessingMode.DictionaryMap.Value[IdOutboxQueueProcessingMode];

		if (cache.TryGetValue(this, out var cached))
			return (Outbox.Model.OutboxQueueProcessingMode)cached;
			
		MappingConditions<Outbox.Model.OutboxQueueProcessingMode>? conds = null;
		if (conditions != null)
		{
			conds = new MappingConditions<Outbox.Model.OutboxQueueProcessingMode>();
			conditions.Invoke(conds);
		}

		cache.Add(this, target);

		if (referenceModifier == ReferenceModifier.MapAllReferences)
		{
			target._outboxQueues = MapperHelper.MapToList(OutboxQueues, target._outboxQueues, OutboxQueue.Map, referenceModifier, conds?.GetConditions(x => x.OutboxQueues), instanceFactory, cache)!;
			target._suspendingModeOutboxQueues = MapperHelper.MapToList(SuspendingModeOutboxQueues, target._suspendingModeOutboxQueues, OutboxQueue.Map, referenceModifier, conds?.GetConditions(x => x.SuspendingModeOutboxQueues), instanceFactory, cache)!;
		}
		else if (referenceModifier == ReferenceModifier.SetNull)
		{
			target._outboxQueues = [];
			target._suspendingModeOutboxQueues = [];
		}

		return target;
	}
}
