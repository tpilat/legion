using Legion;
using Legion.Model.Mappers;

namespace Legion.ADF.Messaging.MessageBox.Model;

public sealed partial class MessageContent : MessageBox.MessageBoxBaseEntity, Legion.Model.IEntity
{
	public static MessageBox.Model.MessageContent? Map(
		MessageBox.Model.MessageContent source,
		MessageBox.Model.MessageContent? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<MessageBox.Model.MessageContent>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		Throw.IfArgumentNull(source);

		return source.MapTo(target, referenceModifier, conditions, instanceFactory, cache);
	}

	public MessageBox.Model.MessageContent? Clone(
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<MessageBox.Model.MessageContent>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
		=> MapTo(target: null, referenceModifier, conditions, instanceFactory, cache);

	public MessageBox.Model.MessageContent? MapTo(
		MessageBox.Model.MessageContent? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<MessageBox.Model.MessageContent>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		cache ??= [];
		target ??= instanceFactory?.CreateInstance<Legion.ADF.Messaging.MessageBox.Model.MessageContent>(
				useActivatorIfNoFactoryFound: false,
				throwIfFactoryReturnsNull: true)
			?? new Legion.ADF.Messaging.MessageBox.Model.MessageContent();

		if (cache.TryGetValue(this, out var cached))
			return (MessageBox.Model.MessageContent)cached;
			
		MappingConditions<MessageBox.Model.MessageContent>? conds = null;
		if (conditions != null)
		{
			conds = new MappingConditions<MessageBox.Model.MessageContent>();
			conditions.Invoke(conds);

			if (conds.CanMap(this, nameof(IdMessageContent)))
				target.IdMessageContent = IdMessageContent;
			if (conds.CanMap(this, nameof(MimeType)))
				target.MimeType = MimeType;
			if (conds.CanMap(this, nameof(ContentEncoding)))
				target.ContentEncoding = ContentEncoding;
			if (conds.CanMap(this, nameof(ByteArrayContent)))
				target.ByteArrayContent = ByteArrayContent?.ToArray();
			if (conds.CanMap(this, nameof(JsonContent)))
				target.JsonContent = JsonContent;
			if (conds.CanMap(this, nameof(StringContent)))
				target.StringContent = StringContent;
			if (conds.CanMap(this, nameof(DbOid)))
				target.DbOid = DbOid;
			if (conds.CanMap(this, nameof(Name)))
				target.Name = Name;
			if (conds.CanMap(this, nameof(RelativePath)))
				target.RelativePath = RelativePath;
			if (conds.CanMap(this, nameof(Metadata)))
				target.Metadata = Metadata;
			if (conds.CanMap(this, nameof(IsCompressed)))
				target.IsCompressed = IsCompressed;
			if (conds.CanMap(this, nameof(EncryptionKey)))
				target.EncryptionKey = EncryptionKey;
		}
		else
		{
			target.IdMessageContent = IdMessageContent;
			target.MimeType = MimeType;
			target.ContentEncoding = ContentEncoding;
			target.ByteArrayContent = ByteArrayContent?.ToArray();
			target.JsonContent = JsonContent;
			target.StringContent = StringContent;
			target.DbOid = DbOid;
			target.Name = Name;
			target.RelativePath = RelativePath;
			target.Metadata = Metadata;
			target.IsCompressed = IsCompressed;
			target.EncryptionKey = EncryptionKey;
		}

		cache.Add(this, target);

		if (referenceModifier == ReferenceModifier.MapAllReferences)
		{
			target.Message = Message?.MapTo(target.Message, referenceModifier, conds?.GetConditions(x => x.Message), instanceFactory, cache)!;
			target.MessageArchive = MessageArchive?.MapTo(target.MessageArchive, referenceModifier, conds?.GetConditions(x => x.MessageArchive), instanceFactory, cache)!;
		}
		else if (referenceModifier == ReferenceModifier.SetNull)
		{
			target.Message = null!;
			target.MessageArchive = null!;
		}

		return target;
	}
}
