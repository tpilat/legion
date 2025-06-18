using Legion;
using Legion.Model.Mappers;

namespace Legion.ADF.Messaging.DomainEvents.Model;

public sealed partial class DomainEvent : DomainEvents.DomainEventsBaseEntity, Legion.Model.IEntity
{
	public static DomainEvents.Model.DomainEvent? Map(
		DomainEvents.Model.DomainEvent source,
		DomainEvents.Model.DomainEvent? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<DomainEvents.Model.DomainEvent>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		Throw.IfArgumentNull(source);

		return source.MapTo(target, referenceModifier, conditions, instanceFactory, cache);
	}

	public DomainEvents.Model.DomainEvent? Clone(
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<DomainEvents.Model.DomainEvent>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
		=> MapTo(target: null, referenceModifier, conditions, instanceFactory, cache);

	public DomainEvents.Model.DomainEvent? MapTo(
		DomainEvents.Model.DomainEvent? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<DomainEvents.Model.DomainEvent>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		cache ??= [];
		target ??= instanceFactory?.CreateInstance<Legion.ADF.Messaging.DomainEvents.Model.DomainEvent>(
				useActivatorIfNoFactoryFound: false,
				throwIfFactoryReturnsNull: true)
			?? new Legion.ADF.Messaging.DomainEvents.Model.DomainEvent();

		if (cache.TryGetValue(this, out var cached))
			return (DomainEvents.Model.DomainEvent)cached;
			
		MappingConditions<DomainEvents.Model.DomainEvent>? conds = null;
		if (conditions != null)
		{
			conds = new MappingConditions<DomainEvents.Model.DomainEvent>();
			conditions.Invoke(conds);

			if (conds.CanMap(this, nameof(IdDomainEvent)))
				target.IdDomainEvent = IdDomainEvent;
			if (conds.CanMap(this, nameof(IdContent)))
				target.IdContent = IdContent;
			if (conds.CanMap(this, nameof(IdDomainEventProcessingStatus)))
				target.IdDomainEventProcessingStatus = IdDomainEventProcessingStatus;
			if (conds.CanMap(this, nameof(Namespace)))
				target.Namespace = Namespace;
			if (conds.CanMap(this, nameof(TraceCorrelationId)))
				target.TraceCorrelationId = TraceCorrelationId;
			if (conds.CanMap(this, nameof(Properties)))
				target.Properties = Properties;
			if (conds.CanMap(this, nameof(Publisher)))
				target.Publisher = Publisher;
			if (conds.CanMap(this, nameof(PublisherId)))
				target.PublisherId = PublisherId;
			if (conds.CanMap(this, nameof(CreatedUtc)))
				target.CreatedUtc = CreatedUtc;
			if (conds.CanMap(this, nameof(ProcessedUtc)))
				target.ProcessedUtc = ProcessedUtc;
			if (conds.CanMap(this, nameof(SuspendedUtc)))
				target.SuspendedUtc = SuspendedUtc;
			if (conds.CanMap(this, nameof(LastProcessingUtc)))
				target.LastProcessingUtc = LastProcessingUtc;
			if (conds.CanMap(this, nameof(LastProcessingTimeoutUtc)))
				target.LastProcessingTimeoutUtc = LastProcessingTimeoutUtc;
			if (conds.CanMap(this, nameof(NextProcessingUtc)))
				target.NextProcessingUtc = NextProcessingUtc;
			if (conds.CanMap(this, nameof(RetryCount)))
				target.RetryCount = RetryCount;
			if (conds.CanMap(this, nameof(Priority)))
				target.Priority = Priority;
		}
		else
		{
			target.IdDomainEvent = IdDomainEvent;
			target.IdContent = IdContent;
			target.IdDomainEventProcessingStatus = IdDomainEventProcessingStatus;
			target.Namespace = Namespace;
			target.TraceCorrelationId = TraceCorrelationId;
			target.Properties = Properties;
			target.Publisher = Publisher;
			target.PublisherId = PublisherId;
			target.CreatedUtc = CreatedUtc;
			target.ProcessedUtc = ProcessedUtc;
			target.SuspendedUtc = SuspendedUtc;
			target.LastProcessingUtc = LastProcessingUtc;
			target.LastProcessingTimeoutUtc = LastProcessingTimeoutUtc;
			target.NextProcessingUtc = NextProcessingUtc;
			target.RetryCount = RetryCount;
			target.Priority = Priority;
		}

		cache.Add(this, target);

		if (referenceModifier == ReferenceModifier.MapAllReferences)
		{
			target.Content = Content?.MapTo(target.Content, referenceModifier, conds?.GetConditions(x => x.Content), instanceFactory, cache)!;
			target.DomainEventProcessingStatus = DomainEventProcessingStatus?.MapTo(target.DomainEventProcessingStatus, referenceModifier, conds?.GetConditions(x => x.DomainEventProcessingStatus), instanceFactory, cache)!;
			target._domainEventProcessingLogs = MapperHelper.MapToList(DomainEventProcessingLogs, target._domainEventProcessingLogs, DomainEventProcessingLog.Map, referenceModifier, conds?.GetConditions(x => x.DomainEventProcessingLogs), instanceFactory, cache)!;
		}
		else if (referenceModifier == ReferenceModifier.SetNull)
		{
			target.Content = null!;
			target.DomainEventProcessingStatus = null!;
			target._domainEventProcessingLogs = [];
		}

		return target;
	}
}
