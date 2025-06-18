using Legion;
using Legion.Model.Mappers;

namespace Legion.ADF.Messaging.Outbox.Model;

public sealed partial class OutboxMessageType : Outbox.OutboxBaseEntity, Legion.Model.IEntity
{
	public static Outbox.Model.OutboxMessageType? Map(
		Outbox.Model.OutboxMessageType source,
		Outbox.Model.OutboxMessageType? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Outbox.Model.OutboxMessageType>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		Throw.IfArgumentNull(source);

		return source.MapTo(target, referenceModifier, conditions, instanceFactory, cache);
	}

	public Outbox.Model.OutboxMessageType? Clone(
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Outbox.Model.OutboxMessageType>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
		=> MapTo(target: null, referenceModifier, conditions, instanceFactory, cache);

	public Outbox.Model.OutboxMessageType? MapTo(
		Outbox.Model.OutboxMessageType? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Outbox.Model.OutboxMessageType>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		cache ??= [];
		target ??= instanceFactory?.CreateInstance<Legion.ADF.Messaging.Outbox.Model.OutboxMessageType>(
				useActivatorIfNoFactoryFound: false,
				throwIfFactoryReturnsNull: true)
			?? new Legion.ADF.Messaging.Outbox.Model.OutboxMessageType();

		if (cache.TryGetValue(this, out var cached))
			return (Outbox.Model.OutboxMessageType)cached;
			
		MappingConditions<Outbox.Model.OutboxMessageType>? conds = null;
		if (conditions != null)
		{
			conds = new MappingConditions<Outbox.Model.OutboxMessageType>();
			conditions.Invoke(conds);

			if (conds.CanMap(this, nameof(IdOutboxMessageType)))
				target.IdOutboxMessageType = IdOutboxMessageType;
			if (conds.CanMap(this, nameof(Code)))
				target.Code = Code;
			if (conds.CanMap(this, nameof(Name)))
				target.Name = Name;
			if (conds.CanMap(this, nameof(Namespace)))
				target.Namespace = Namespace;
			if (conds.CanMap(this, nameof(CreatedUtc)))
				target.CreatedUtc = CreatedUtc;
			if (conds.CanMap(this, nameof(IdOutboxInstance)))
				target.IdOutboxInstance = IdOutboxInstance;
		}
		else
		{
			target.IdOutboxMessageType = IdOutboxMessageType;
			target.Code = Code;
			target.Name = Name;
			target.Namespace = Namespace;
			target.CreatedUtc = CreatedUtc;
			target.IdOutboxInstance = IdOutboxInstance;
		}

		cache.Add(this, target);

		if (referenceModifier == ReferenceModifier.MapAllReferences)
		{
			target.OutboxInstance = OutboxInstance?.MapTo(target.OutboxInstance, referenceModifier, conds?.GetConditions(x => x.OutboxInstance), instanceFactory, cache)!;
			target._outboxMessageArchives = MapperHelper.MapToList(OutboxMessageArchives, target._outboxMessageArchives, OutboxMessageArchive.Map, referenceModifier, conds?.GetConditions(x => x.OutboxMessageArchives), instanceFactory, cache)!;
			target._outboxMessages = MapperHelper.MapToList(OutboxMessages, target._outboxMessages, OutboxMessage.Map, referenceModifier, conds?.GetConditions(x => x.OutboxMessages), instanceFactory, cache)!;
			target._outboxQueues = MapperHelper.MapToList(OutboxQueues, target._outboxQueues, OutboxQueue.Map, referenceModifier, conds?.GetConditions(x => x.OutboxQueues), instanceFactory, cache)!;
		}
		else if (referenceModifier == ReferenceModifier.SetNull)
		{
			target.OutboxInstance = null!;
			target._outboxMessageArchives = [];
			target._outboxMessages = [];
			target._outboxQueues = [];
		}

		return target;
	}
}
