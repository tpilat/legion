using Legion;
using Legion.Model.Mappers;

namespace Legion.ADF.Messaging.Outbox.Model;

public sealed partial class OutboxMessageProcessingLog : Outbox.OutboxBaseEntity, Legion.Model.IEntity
{
	public static Outbox.Model.OutboxMessageProcessingLog? Map(
		Outbox.Model.OutboxMessageProcessingLog source,
		Outbox.Model.OutboxMessageProcessingLog? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Outbox.Model.OutboxMessageProcessingLog>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		Throw.IfArgumentNull(source);

		return source.MapTo(target, referenceModifier, conditions, instanceFactory, cache);
	}

	public Outbox.Model.OutboxMessageProcessingLog? Clone(
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Outbox.Model.OutboxMessageProcessingLog>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
		=> MapTo(target: null, referenceModifier, conditions, instanceFactory, cache);

	public Outbox.Model.OutboxMessageProcessingLog? MapTo(
		Outbox.Model.OutboxMessageProcessingLog? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Outbox.Model.OutboxMessageProcessingLog>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		cache ??= [];
		target ??= instanceFactory?.CreateInstance<Legion.ADF.Messaging.Outbox.Model.OutboxMessageProcessingLog>(
				useActivatorIfNoFactoryFound: false,
				throwIfFactoryReturnsNull: true)
			?? new Legion.ADF.Messaging.Outbox.Model.OutboxMessageProcessingLog();

		if (cache.TryGetValue(this, out var cached))
			return (Outbox.Model.OutboxMessageProcessingLog)cached;
			
		MappingConditions<Outbox.Model.OutboxMessageProcessingLog>? conds = null;
		if (conditions != null)
		{
			conds = new MappingConditions<Outbox.Model.OutboxMessageProcessingLog>();
			conditions.Invoke(conds);

			if (conds.CanMap(this, nameof(IdOutboxMessageProcessingLog)))
				target.IdOutboxMessageProcessingLog = IdOutboxMessageProcessingLog;
			if (conds.CanMap(this, nameof(IdOutboxMessage)))
				target.IdOutboxMessage = IdOutboxMessage;
			if (conds.CanMap(this, nameof(IdOutboxQueue)))
				target.IdOutboxQueue = IdOutboxQueue;
			if (conds.CanMap(this, nameof(CreatedUtc)))
				target.CreatedUtc = CreatedUtc;
			if (conds.CanMap(this, nameof(IdOutboxMessageStatus)))
				target.IdOutboxMessageStatus = IdOutboxMessageStatus;
			if (conds.CanMap(this, nameof(TraceCorrelationId)))
				target.TraceCorrelationId = TraceCorrelationId;
			if (conds.CanMap(this, nameof(IdLogMessage)))
				target.IdLogMessage = IdLogMessage;
			if (conds.CanMap(this, nameof(Code)))
				target.Code = Code;
			if (conds.CanMap(this, nameof(Detail)))
				target.Detail = Detail;
			if (conds.CanMap(this, nameof(IdOutboxInstance)))
				target.IdOutboxInstance = IdOutboxInstance;
		}
		else
		{
			target.IdOutboxMessageProcessingLog = IdOutboxMessageProcessingLog;
			target.IdOutboxMessage = IdOutboxMessage;
			target.IdOutboxQueue = IdOutboxQueue;
			target.CreatedUtc = CreatedUtc;
			target.IdOutboxMessageStatus = IdOutboxMessageStatus;
			target.TraceCorrelationId = TraceCorrelationId;
			target.IdLogMessage = IdLogMessage;
			target.Code = Code;
			target.Detail = Detail;
			target.IdOutboxInstance = IdOutboxInstance;
		}

		cache.Add(this, target);

		if (referenceModifier == ReferenceModifier.MapAllReferences)
		{
			target.OutboxInstance = OutboxInstance?.MapTo(target.OutboxInstance, referenceModifier, conds?.GetConditions(x => x.OutboxInstance), instanceFactory, cache)!;
			target.OutboxMessageStatus = OutboxMessageStatus?.MapTo(target.OutboxMessageStatus, referenceModifier, conds?.GetConditions(x => x.OutboxMessageStatus), instanceFactory, cache)!;
			target.OutboxQueue = OutboxQueue?.MapTo(target.OutboxQueue, referenceModifier, conds?.GetConditions(x => x.OutboxQueue), instanceFactory, cache)!;
		}
		else if (referenceModifier == ReferenceModifier.SetNull)
		{
			target.OutboxInstance = null!;
			target.OutboxMessageStatus = null!;
			target.OutboxQueue = null!;
		}

		return target;
	}
}
