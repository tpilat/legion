using Legion;
using Legion.Model.Mappers;

namespace Legion.ADF.Messaging.MessageBox.Model;

public sealed partial class MessageStatus : MessageBox.MessageBoxBaseEntity, Legion.Model.IEntity
{
	public static MessageBox.Model.MessageStatus? Map(
		MessageBox.Model.MessageStatus source,
		MessageBox.Model.MessageStatus? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<MessageBox.Model.MessageStatus>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		Throw.IfArgumentNull(source);

		return source.MapTo(target, referenceModifier, conditions, instanceFactory, cache);
	}

	public MessageBox.Model.MessageStatus? Clone(
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<MessageBox.Model.MessageStatus>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
		=> MapTo(target: null, referenceModifier, conditions, instanceFactory, cache);

	public MessageBox.Model.MessageStatus? MapTo(
		MessageBox.Model.MessageStatus? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<MessageBox.Model.MessageStatus>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		cache ??= [];
		target ??= Legion.ADF.Messaging.MessageBox.Model.MessageStatus.DictionaryMap.Value[IdMessageStatus];

		if (cache.TryGetValue(this, out var cached))
			return (MessageBox.Model.MessageStatus)cached;
			
		MappingConditions<MessageBox.Model.MessageStatus>? conds = null;
		if (conditions != null)
		{
			conds = new MappingConditions<MessageBox.Model.MessageStatus>();
			conditions.Invoke(conds);
		}

		cache.Add(this, target);

		if (referenceModifier == ReferenceModifier.MapAllReferences)
		{
			target._messageArchives = MapperHelper.MapToList(MessageArchives, target._messageArchives, MessageArchive.Map, referenceModifier, conds?.GetConditions(x => x.MessageArchives), instanceFactory, cache)!;
			target._messages = MapperHelper.MapToList(Messages, target._messages, Message.Map, referenceModifier, conds?.GetConditions(x => x.Messages), instanceFactory, cache)!;
		}
		else if (referenceModifier == ReferenceModifier.SetNull)
		{
			target._messageArchives = [];
			target._messages = [];
		}

		return target;
	}
}
