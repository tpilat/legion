using Legion;
using Legion.Model.Mappers;

namespace Legion.ADF.Messaging.Outbox.Model;

public sealed partial class OutboxMessageStatus : Outbox.OutboxBaseEntity, Legion.Model.IEntity
{
	public static Outbox.Model.OutboxMessageStatus? Map(
		Outbox.Model.OutboxMessageStatus source,
		Outbox.Model.OutboxMessageStatus? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Outbox.Model.OutboxMessageStatus>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		Throw.IfArgumentNull(source);

		return source.MapTo(target, referenceModifier, conditions, instanceFactory, cache);
	}

	public Outbox.Model.OutboxMessageStatus? Clone(
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Outbox.Model.OutboxMessageStatus>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
		=> MapTo(target: null, referenceModifier, conditions, instanceFactory, cache);

	public Outbox.Model.OutboxMessageStatus? MapTo(
		Outbox.Model.OutboxMessageStatus? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Outbox.Model.OutboxMessageStatus>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		cache ??= [];
		target ??= Legion.ADF.Messaging.Outbox.Model.OutboxMessageStatus.DictionaryMap.Value[IdOutboxMessageStatus];

		if (cache.TryGetValue(this, out var cached))
			return (Outbox.Model.OutboxMessageStatus)cached;
			
		MappingConditions<Outbox.Model.OutboxMessageStatus>? conds = null;
		if (conditions != null)
		{
			conds = new MappingConditions<Outbox.Model.OutboxMessageStatus>();
			conditions.Invoke(conds);
		}

		cache.Add(this, target);

		if (referenceModifier == ReferenceModifier.MapAllReferences)
		{
			target._outboxMessageArchives = MapperHelper.MapToList(OutboxMessageArchives, target._outboxMessageArchives, OutboxMessageArchive.Map, referenceModifier, conds?.GetConditions(x => x.OutboxMessageArchives), instanceFactory, cache)!;
			target._outboxMessageProcessingLogs = MapperHelper.MapToList(OutboxMessageProcessingLogs, target._outboxMessageProcessingLogs, OutboxMessageProcessingLog.Map, referenceModifier, conds?.GetConditions(x => x.OutboxMessageProcessingLogs), instanceFactory, cache)!;
			target._outboxMessages = MapperHelper.MapToList(OutboxMessages, target._outboxMessages, OutboxMessage.Map, referenceModifier, conds?.GetConditions(x => x.OutboxMessages), instanceFactory, cache)!;
		}
		else if (referenceModifier == ReferenceModifier.SetNull)
		{
			target._outboxMessageArchives = [];
			target._outboxMessageProcessingLogs = [];
			target._outboxMessages = [];
		}

		return target;
	}
}
