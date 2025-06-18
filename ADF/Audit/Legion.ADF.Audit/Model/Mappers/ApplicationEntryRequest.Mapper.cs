using Legion;
using Legion.Model.Mappers;

namespace Legion.ADF.Audit.Model;

public sealed partial class ApplicationEntryRequest : Audit.AuditBaseEntity, Legion.Model.IEntity
{
	public static Audit.Model.ApplicationEntryRequest? Map(
		Audit.Model.ApplicationEntryRequest source,
		Audit.Model.ApplicationEntryRequest? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Audit.Model.ApplicationEntryRequest>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		Throw.IfArgumentNull(source);

		return source.MapTo(target, referenceModifier, conditions, instanceFactory, cache);
	}

	public Audit.Model.ApplicationEntryRequest? Clone(
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Audit.Model.ApplicationEntryRequest>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
		=> MapTo(target: null, referenceModifier, conditions, instanceFactory, cache);

	public Audit.Model.ApplicationEntryRequest? MapTo(
		Audit.Model.ApplicationEntryRequest? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Audit.Model.ApplicationEntryRequest>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		cache ??= [];
		target ??= instanceFactory?.CreateInstance<Legion.ADF.Audit.Model.ApplicationEntryRequest>(
				useActivatorIfNoFactoryFound: false,
				throwIfFactoryReturnsNull: true)
			?? new Legion.ADF.Audit.Model.ApplicationEntryRequest();

		if (cache.TryGetValue(this, out var cached))
			return (Audit.Model.ApplicationEntryRequest)cached;
			
		MappingConditions<Audit.Model.ApplicationEntryRequest>? conds = null;
		if (conditions != null)
		{
			conds = new MappingConditions<Audit.Model.ApplicationEntryRequest>();
			conditions.Invoke(conds);

			if (conds.CanMap(this, nameof(IdApplicationEntryRequest)))
				target.IdApplicationEntryRequest = IdApplicationEntryRequest;
			if (conds.CanMap(this, nameof(IdApplicationEntry)))
				target.IdApplicationEntry = IdApplicationEntry;
			if (conds.CanMap(this, nameof(CreatedUtc)))
				target.CreatedUtc = CreatedUtc;
			if (conds.CanMap(this, nameof(Metadata)))
				target.Metadata = Metadata;
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
			if (conds.CanMap(this, nameof(IsCompressed)))
				target.IsCompressed = IsCompressed;
			if (conds.CanMap(this, nameof(EncryptionKey)))
				target.EncryptionKey = EncryptionKey;
		}
		else
		{
			target.IdApplicationEntryRequest = IdApplicationEntryRequest;
			target.IdApplicationEntry = IdApplicationEntry;
			target.CreatedUtc = CreatedUtc;
			target.Metadata = Metadata;
			target.MimeType = MimeType;
			target.ContentEncoding = ContentEncoding;
			target.ByteArrayContent = ByteArrayContent?.ToArray();
			target.JsonContent = JsonContent;
			target.StringContent = StringContent;
			target.DbOid = DbOid;
			target.Name = Name;
			target.RelativePath = RelativePath;
			target.IsCompressed = IsCompressed;
			target.EncryptionKey = EncryptionKey;
		}

		cache.Add(this, target);

		if (referenceModifier == ReferenceModifier.MapAllReferences)
		{
			target.ApplicationEntry = ApplicationEntry?.MapTo(target.ApplicationEntry, referenceModifier, conds?.GetConditions(x => x.ApplicationEntry), instanceFactory, cache)!;
		}
		else if (referenceModifier == ReferenceModifier.SetNull)
		{
			target.ApplicationEntry = null!;
		}

		return target;
	}
}
