using Legion;
using Legion.Model.Mappers;

namespace Legion.ADF.Messaging.Inbox.Model;

public sealed partial class InboxMessageProcessingLog : Inbox.InboxBaseEntity, Legion.Model.IEntity
{
	public static Inbox.Model.InboxMessageProcessingLog? Map(
		Inbox.Model.InboxMessageProcessingLog source,
		Inbox.Model.InboxMessageProcessingLog? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Inbox.Model.InboxMessageProcessingLog>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		Throw.IfArgumentNull(source);

		return source.MapTo(target, referenceModifier, conditions, instanceFactory, cache);
	}

	public Inbox.Model.InboxMessageProcessingLog? Clone(
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Inbox.Model.InboxMessageProcessingLog>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
		=> MapTo(target: null, referenceModifier, conditions, instanceFactory, cache);

	public Inbox.Model.InboxMessageProcessingLog? MapTo(
		Inbox.Model.InboxMessageProcessingLog? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Inbox.Model.InboxMessageProcessingLog>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		cache ??= [];
		target ??= instanceFactory?.CreateInstance<Legion.ADF.Messaging.Inbox.Model.InboxMessageProcessingLog>(
				useActivatorIfNoFactoryFound: false,
				throwIfFactoryReturnsNull: true)
			?? new Legion.ADF.Messaging.Inbox.Model.InboxMessageProcessingLog();

		if (cache.TryGetValue(this, out var cached))
			return (Inbox.Model.InboxMessageProcessingLog)cached;
			
		MappingConditions<Inbox.Model.InboxMessageProcessingLog>? conds = null;
		if (conditions != null)
		{
			conds = new MappingConditions<Inbox.Model.InboxMessageProcessingLog>();
			conditions.Invoke(conds);

			if (conds.CanMap(this, nameof(IdInboxMessageProcessingLog)))
				target.IdInboxMessageProcessingLog = IdInboxMessageProcessingLog;
			if (conds.CanMap(this, nameof(IdInboxMessage)))
				target.IdInboxMessage = IdInboxMessage;
			if (conds.CanMap(this, nameof(IdInboxQueue)))
				target.IdInboxQueue = IdInboxQueue;
			if (conds.CanMap(this, nameof(CreatedUtc)))
				target.CreatedUtc = CreatedUtc;
			if (conds.CanMap(this, nameof(IdInboxMessageStatus)))
				target.IdInboxMessageStatus = IdInboxMessageStatus;
			if (conds.CanMap(this, nameof(TraceCorrelationId)))
				target.TraceCorrelationId = TraceCorrelationId;
			if (conds.CanMap(this, nameof(IdLogMessage)))
				target.IdLogMessage = IdLogMessage;
			if (conds.CanMap(this, nameof(Code)))
				target.Code = Code;
			if (conds.CanMap(this, nameof(Detail)))
				target.Detail = Detail;
			if (conds.CanMap(this, nameof(IdInboxInstance)))
				target.IdInboxInstance = IdInboxInstance;
		}
		else
		{
			target.IdInboxMessageProcessingLog = IdInboxMessageProcessingLog;
			target.IdInboxMessage = IdInboxMessage;
			target.IdInboxQueue = IdInboxQueue;
			target.CreatedUtc = CreatedUtc;
			target.IdInboxMessageStatus = IdInboxMessageStatus;
			target.TraceCorrelationId = TraceCorrelationId;
			target.IdLogMessage = IdLogMessage;
			target.Code = Code;
			target.Detail = Detail;
			target.IdInboxInstance = IdInboxInstance;
		}

		cache.Add(this, target);

		if (referenceModifier == ReferenceModifier.MapAllReferences)
		{
			target.InboxInstance = InboxInstance?.MapTo(target.InboxInstance, referenceModifier, conds?.GetConditions(x => x.InboxInstance), instanceFactory, cache)!;
			target.InboxMessageStatus = InboxMessageStatus?.MapTo(target.InboxMessageStatus, referenceModifier, conds?.GetConditions(x => x.InboxMessageStatus), instanceFactory, cache)!;
			target.InboxQueue = InboxQueue?.MapTo(target.InboxQueue, referenceModifier, conds?.GetConditions(x => x.InboxQueue), instanceFactory, cache)!;
		}
		else if (referenceModifier == ReferenceModifier.SetNull)
		{
			target.InboxInstance = null!;
			target.InboxMessageStatus = null!;
			target.InboxQueue = null!;
		}

		return target;
	}
}
