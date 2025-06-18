using Legion;
using Legion.Model.Mappers;

namespace Legion.ADF.Messaging.Inbox.Model;

public sealed partial class InboxMessageContent : Inbox.InboxBaseEntity, Legion.Model.IEntity
{
	public static Inbox.Model.InboxMessageContent? Map(
		Inbox.Model.InboxMessageContent source,
		Inbox.Model.InboxMessageContent? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Inbox.Model.InboxMessageContent>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		Throw.IfArgumentNull(source);

		return source.MapTo(target, referenceModifier, conditions, instanceFactory, cache);
	}

	public Inbox.Model.InboxMessageContent? Clone(
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Inbox.Model.InboxMessageContent>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
		=> MapTo(target: null, referenceModifier, conditions, instanceFactory, cache);

	public Inbox.Model.InboxMessageContent? MapTo(
		Inbox.Model.InboxMessageContent? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Inbox.Model.InboxMessageContent>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		cache ??= [];
		target ??= instanceFactory?.CreateInstance<Legion.ADF.Messaging.Inbox.Model.InboxMessageContent>(
				useActivatorIfNoFactoryFound: false,
				throwIfFactoryReturnsNull: true)
			?? new Legion.ADF.Messaging.Inbox.Model.InboxMessageContent();

		if (cache.TryGetValue(this, out var cached))
			return (Inbox.Model.InboxMessageContent)cached;
			
		MappingConditions<Inbox.Model.InboxMessageContent>? conds = null;
		if (conditions != null)
		{
			conds = new MappingConditions<Inbox.Model.InboxMessageContent>();
			conditions.Invoke(conds);

			if (conds.CanMap(this, nameof(IdInboxMessageContent)))
				target.IdInboxMessageContent = IdInboxMessageContent;
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
			target.IdInboxMessageContent = IdInboxMessageContent;
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
			target.InboxMessage = InboxMessage?.MapTo(target.InboxMessage, referenceModifier, conds?.GetConditions(x => x.InboxMessage), instanceFactory, cache)!;
			target.InboxMessageArchive = InboxMessageArchive?.MapTo(target.InboxMessageArchive, referenceModifier, conds?.GetConditions(x => x.InboxMessageArchive), instanceFactory, cache)!;
		}
		else if (referenceModifier == ReferenceModifier.SetNull)
		{
			target.InboxMessage = null!;
			target.InboxMessageArchive = null!;
		}

		return target;
	}
}
