using Legion;
using Legion.Model.Mappers;

namespace Legion.ADF.Messaging.DomainEvents.Model;

public sealed partial class DomainEventProcessingLog : DomainEvents.DomainEventsBaseEntity, Legion.Model.IEntity
{
	public static DomainEvents.Model.DomainEventProcessingLog? Map(
		DomainEvents.Model.DomainEventProcessingLog source,
		DomainEvents.Model.DomainEventProcessingLog? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<DomainEvents.Model.DomainEventProcessingLog>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		Throw.IfArgumentNull(source);

		return source.MapTo(target, referenceModifier, conditions, instanceFactory, cache);
	}

	public DomainEvents.Model.DomainEventProcessingLog? Clone(
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<DomainEvents.Model.DomainEventProcessingLog>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
		=> MapTo(target: null, referenceModifier, conditions, instanceFactory, cache);

	public DomainEvents.Model.DomainEventProcessingLog? MapTo(
		DomainEvents.Model.DomainEventProcessingLog? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<DomainEvents.Model.DomainEventProcessingLog>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		cache ??= [];
		target ??= instanceFactory?.CreateInstance<Legion.ADF.Messaging.DomainEvents.Model.DomainEventProcessingLog>(
				useActivatorIfNoFactoryFound: false,
				throwIfFactoryReturnsNull: true)
			?? new Legion.ADF.Messaging.DomainEvents.Model.DomainEventProcessingLog();

		if (cache.TryGetValue(this, out var cached))
			return (DomainEvents.Model.DomainEventProcessingLog)cached;
			
		MappingConditions<DomainEvents.Model.DomainEventProcessingLog>? conds = null;
		if (conditions != null)
		{
			conds = new MappingConditions<DomainEvents.Model.DomainEventProcessingLog>();
			conditions.Invoke(conds);

			if (conds.CanMap(this, nameof(IdDomainEventProcessingLog)))
				target.IdDomainEventProcessingLog = IdDomainEventProcessingLog;
			if (conds.CanMap(this, nameof(IdDomainEvent)))
				target.IdDomainEvent = IdDomainEvent;
			if (conds.CanMap(this, nameof(CreatedUtc)))
				target.CreatedUtc = CreatedUtc;
			if (conds.CanMap(this, nameof(IdDomainEventProcessingStatus)))
				target.IdDomainEventProcessingStatus = IdDomainEventProcessingStatus;
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
			target.IdDomainEventProcessingLog = IdDomainEventProcessingLog;
			target.IdDomainEvent = IdDomainEvent;
			target.CreatedUtc = CreatedUtc;
			target.IdDomainEventProcessingStatus = IdDomainEventProcessingStatus;
			target.TraceCorrelationId = TraceCorrelationId;
			target.IdLogMessage = IdLogMessage;
			target.Code = Code;
			target.Detail = Detail;
		}

		cache.Add(this, target);

		if (referenceModifier == ReferenceModifier.MapAllReferences)
		{
			target.DomainEvent = DomainEvent?.MapTo(target.DomainEvent, referenceModifier, conds?.GetConditions(x => x.DomainEvent), instanceFactory, cache)!;
			target.DomainEventProcessingStatus = DomainEventProcessingStatus?.MapTo(target.DomainEventProcessingStatus, referenceModifier, conds?.GetConditions(x => x.DomainEventProcessingStatus), instanceFactory, cache)!;
		}
		else if (referenceModifier == ReferenceModifier.SetNull)
		{
			target.DomainEvent = null!;
			target.DomainEventProcessingStatus = null!;
		}

		return target;
	}
}
