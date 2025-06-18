using Legion;
using Legion.Model.Mappers;

namespace Legion.ADF.Messaging.Inbox.Model;

public sealed partial class InboxProcessingLog : Inbox.InboxBaseEntity, Legion.Model.IEntity
{
	public static Inbox.Model.InboxProcessingLog? Map(
		Inbox.Model.InboxProcessingLog source,
		Inbox.Model.InboxProcessingLog? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Inbox.Model.InboxProcessingLog>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		Throw.IfArgumentNull(source);

		return source.MapTo(target, referenceModifier, conditions, instanceFactory, cache);
	}

	public Inbox.Model.InboxProcessingLog? Clone(
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Inbox.Model.InboxProcessingLog>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
		=> MapTo(target: null, referenceModifier, conditions, instanceFactory, cache);

	public Inbox.Model.InboxProcessingLog? MapTo(
		Inbox.Model.InboxProcessingLog? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Inbox.Model.InboxProcessingLog>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		cache ??= [];
		target ??= instanceFactory?.CreateInstance<Legion.ADF.Messaging.Inbox.Model.InboxProcessingLog>(
				useActivatorIfNoFactoryFound: false,
				throwIfFactoryReturnsNull: true)
			?? new Legion.ADF.Messaging.Inbox.Model.InboxProcessingLog();

		if (cache.TryGetValue(this, out var cached))
			return (Inbox.Model.InboxProcessingLog)cached;
			
		MappingConditions<Inbox.Model.InboxProcessingLog>? conds = null;
		if (conditions != null)
		{
			conds = new MappingConditions<Inbox.Model.InboxProcessingLog>();
			conditions.Invoke(conds);

			if (conds.CanMap(this, nameof(IdInboxProcessingLog)))
				target.IdInboxProcessingLog = IdInboxProcessingLog;
			if (conds.CanMap(this, nameof(IdInboxInstance)))
				target.IdInboxInstance = IdInboxInstance;
			if (conds.CanMap(this, nameof(IdInboxQueue)))
				target.IdInboxQueue = IdInboxQueue;
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
			target.IdInboxProcessingLog = IdInboxProcessingLog;
			target.IdInboxInstance = IdInboxInstance;
			target.IdInboxQueue = IdInboxQueue;
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
			target.InboxInstance = InboxInstance?.MapTo(target.InboxInstance, referenceModifier, conds?.GetConditions(x => x.InboxInstance), instanceFactory, cache)!;
			target.InboxQueue = InboxQueue?.MapTo(target.InboxQueue, referenceModifier, conds?.GetConditions(x => x.InboxQueue), instanceFactory, cache)!;
		}
		else if (referenceModifier == ReferenceModifier.SetNull)
		{
			target.InboxInstance = null!;
			target.InboxQueue = null!;
		}

		return target;
	}
}
