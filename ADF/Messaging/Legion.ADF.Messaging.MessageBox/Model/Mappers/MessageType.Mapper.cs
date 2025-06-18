using Legion;
using Legion.Model.Mappers;

namespace Legion.ADF.Messaging.MessageBox.Model;

public sealed partial class MessageType : MessageBox.MessageBoxBaseEntity, Legion.Model.IEntity
{
	public static MessageBox.Model.MessageType? Map(
		MessageBox.Model.MessageType source,
		MessageBox.Model.MessageType? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<MessageBox.Model.MessageType>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		Throw.IfArgumentNull(source);

		return source.MapTo(target, referenceModifier, conditions, instanceFactory, cache);
	}

	public MessageBox.Model.MessageType? Clone(
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<MessageBox.Model.MessageType>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
		=> MapTo(target: null, referenceModifier, conditions, instanceFactory, cache);

	public MessageBox.Model.MessageType? MapTo(
		MessageBox.Model.MessageType? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<MessageBox.Model.MessageType>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		cache ??= [];
		target ??= instanceFactory?.CreateInstance<Legion.ADF.Messaging.MessageBox.Model.MessageType>(
				useActivatorIfNoFactoryFound: false,
				throwIfFactoryReturnsNull: true)
			?? new Legion.ADF.Messaging.MessageBox.Model.MessageType();

		if (cache.TryGetValue(this, out var cached))
			return (MessageBox.Model.MessageType)cached;
			
		MappingConditions<MessageBox.Model.MessageType>? conds = null;
		if (conditions != null)
		{
			conds = new MappingConditions<MessageBox.Model.MessageType>();
			conditions.Invoke(conds);

			if (conds.CanMap(this, nameof(IdMessageType)))
				target.IdMessageType = IdMessageType;
			if (conds.CanMap(this, nameof(Code)))
				target.Code = Code;
			if (conds.CanMap(this, nameof(Name)))
				target.Name = Name;
			if (conds.CanMap(this, nameof(Namespace)))
				target.Namespace = Namespace;
			if (conds.CanMap(this, nameof(CreatedUtc)))
				target.CreatedUtc = CreatedUtc;
			if (conds.CanMap(this, nameof(IdMessageBoxInstance)))
				target.IdMessageBoxInstance = IdMessageBoxInstance;
		}
		else
		{
			target.IdMessageType = IdMessageType;
			target.Code = Code;
			target.Name = Name;
			target.Namespace = Namespace;
			target.CreatedUtc = CreatedUtc;
			target.IdMessageBoxInstance = IdMessageBoxInstance;
		}

		cache.Add(this, target);

		if (referenceModifier == ReferenceModifier.MapAllReferences)
		{
			target.MessageBoxInstance = MessageBoxInstance?.MapTo(target.MessageBoxInstance, referenceModifier, conds?.GetConditions(x => x.MessageBoxInstance), instanceFactory, cache)!;
			target._messageArchives = MapperHelper.MapToList(MessageArchives, target._messageArchives, MessageArchive.Map, referenceModifier, conds?.GetConditions(x => x.MessageArchives), instanceFactory, cache)!;
			target._messages = MapperHelper.MapToList(Messages, target._messages, Message.Map, referenceModifier, conds?.GetConditions(x => x.Messages), instanceFactory, cache)!;
			target._queues = MapperHelper.MapToList(Queues, target._queues, Queue.Map, referenceModifier, conds?.GetConditions(x => x.Queues), instanceFactory, cache)!;
		}
		else if (referenceModifier == ReferenceModifier.SetNull)
		{
			target.MessageBoxInstance = null!;
			target._messageArchives = [];
			target._messages = [];
			target._queues = [];
		}

		return target;
	}
}
