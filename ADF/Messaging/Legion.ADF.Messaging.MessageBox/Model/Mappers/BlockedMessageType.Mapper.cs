using Legion;
using Legion.Model.Mappers;

namespace Legion.ADF.Messaging.MessageBox.Model;

public sealed partial class BlockedMessageType : MessageBox.MessageBoxBaseEntity, Legion.Model.IEntity
{
	public static MessageBox.Model.BlockedMessageType? Map(
		MessageBox.Model.BlockedMessageType source,
		MessageBox.Model.BlockedMessageType? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<MessageBox.Model.BlockedMessageType>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		Throw.IfArgumentNull(source);

		return source.MapTo(target, referenceModifier, conditions, instanceFactory, cache);
	}

	public MessageBox.Model.BlockedMessageType? Clone(
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<MessageBox.Model.BlockedMessageType>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
		=> MapTo(target: null, referenceModifier, conditions, instanceFactory, cache);

	public MessageBox.Model.BlockedMessageType? MapTo(
		MessageBox.Model.BlockedMessageType? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<MessageBox.Model.BlockedMessageType>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		cache ??= [];
		target ??= instanceFactory?.CreateInstance<Legion.ADF.Messaging.MessageBox.Model.BlockedMessageType>(
				useActivatorIfNoFactoryFound: false,
				throwIfFactoryReturnsNull: true)
			?? new Legion.ADF.Messaging.MessageBox.Model.BlockedMessageType();

		if (cache.TryGetValue(this, out var cached))
			return (MessageBox.Model.BlockedMessageType)cached;
			
		MappingConditions<MessageBox.Model.BlockedMessageType>? conds = null;
		if (conditions != null)
		{
			conds = new MappingConditions<MessageBox.Model.BlockedMessageType>();
			conditions.Invoke(conds);

			if (conds.CanMap(this, nameof(IdBlockedMessageType)))
				target.IdBlockedMessageType = IdBlockedMessageType;
			if (conds.CanMap(this, nameof(Namespace)))
				target.Namespace = Namespace;
			if (conds.CanMap(this, nameof(CreatedUtc)))
				target.CreatedUtc = CreatedUtc;
			if (conds.CanMap(this, nameof(IdMessageBoxInstance)))
				target.IdMessageBoxInstance = IdMessageBoxInstance;
		}
		else
		{
			target.IdBlockedMessageType = IdBlockedMessageType;
			target.Namespace = Namespace;
			target.CreatedUtc = CreatedUtc;
			target.IdMessageBoxInstance = IdMessageBoxInstance;
		}

		cache.Add(this, target);

		if (referenceModifier == ReferenceModifier.MapAllReferences)
		{
			target.MessageBoxInstance = MessageBoxInstance?.MapTo(target.MessageBoxInstance, referenceModifier, conds?.GetConditions(x => x.MessageBoxInstance), instanceFactory, cache)!;
		}
		else if (referenceModifier == ReferenceModifier.SetNull)
		{
			target.MessageBoxInstance = null!;
		}

		return target;
	}
}
