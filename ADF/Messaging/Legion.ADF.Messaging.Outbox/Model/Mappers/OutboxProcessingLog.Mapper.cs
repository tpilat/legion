using Legion;
using Legion.Model.Mappers;

namespace Legion.ADF.Messaging.Outbox.Model;

public sealed partial class OutboxProcessingLog : Outbox.OutboxBaseEntity, Legion.Model.IEntity
{
	public static Outbox.Model.OutboxProcessingLog? Map(
		Outbox.Model.OutboxProcessingLog source,
		Outbox.Model.OutboxProcessingLog? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Outbox.Model.OutboxProcessingLog>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		Throw.IfArgumentNull(source);

		return source.MapTo(target, referenceModifier, conditions, instanceFactory, cache);
	}

	public Outbox.Model.OutboxProcessingLog? Clone(
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Outbox.Model.OutboxProcessingLog>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
		=> MapTo(target: null, referenceModifier, conditions, instanceFactory, cache);

	public Outbox.Model.OutboxProcessingLog? MapTo(
		Outbox.Model.OutboxProcessingLog? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Outbox.Model.OutboxProcessingLog>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		cache ??= [];
		target ??= instanceFactory?.CreateInstance<Legion.ADF.Messaging.Outbox.Model.OutboxProcessingLog>(
				useActivatorIfNoFactoryFound: false,
				throwIfFactoryReturnsNull: true)
			?? new Legion.ADF.Messaging.Outbox.Model.OutboxProcessingLog();

		if (cache.TryGetValue(this, out var cached))
			return (Outbox.Model.OutboxProcessingLog)cached;
			
		MappingConditions<Outbox.Model.OutboxProcessingLog>? conds = null;
		if (conditions != null)
		{
			conds = new MappingConditions<Outbox.Model.OutboxProcessingLog>();
			conditions.Invoke(conds);

			if (conds.CanMap(this, nameof(IdOutboxProcessingLog)))
				target.IdOutboxProcessingLog = IdOutboxProcessingLog;
			if (conds.CanMap(this, nameof(IdOutboxInstance)))
				target.IdOutboxInstance = IdOutboxInstance;
			if (conds.CanMap(this, nameof(IdOutboxQueue)))
				target.IdOutboxQueue = IdOutboxQueue;
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
			target.IdOutboxProcessingLog = IdOutboxProcessingLog;
			target.IdOutboxInstance = IdOutboxInstance;
			target.IdOutboxQueue = IdOutboxQueue;
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
			target.OutboxInstance = OutboxInstance?.MapTo(target.OutboxInstance, referenceModifier, conds?.GetConditions(x => x.OutboxInstance), instanceFactory, cache)!;
			target.OutboxQueue = OutboxQueue?.MapTo(target.OutboxQueue, referenceModifier, conds?.GetConditions(x => x.OutboxQueue), instanceFactory, cache)!;
		}
		else if (referenceModifier == ReferenceModifier.SetNull)
		{
			target.OutboxInstance = null!;
			target.OutboxQueue = null!;
		}

		return target;
	}
}
