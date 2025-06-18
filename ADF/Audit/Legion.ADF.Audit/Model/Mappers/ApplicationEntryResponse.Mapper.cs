using Legion;
using Legion.Model.Mappers;

namespace Legion.ADF.Audit.Model;

public sealed partial class ApplicationEntryResponse : Audit.AuditBaseEntity, Legion.Model.IEntity
{
	public static Audit.Model.ApplicationEntryResponse? Map(
		Audit.Model.ApplicationEntryResponse source,
		Audit.Model.ApplicationEntryResponse? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Audit.Model.ApplicationEntryResponse>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		Throw.IfArgumentNull(source);

		return source.MapTo(target, referenceModifier, conditions, instanceFactory, cache);
	}

	public Audit.Model.ApplicationEntryResponse? Clone(
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Audit.Model.ApplicationEntryResponse>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
		=> MapTo(target: null, referenceModifier, conditions, instanceFactory, cache);

	public Audit.Model.ApplicationEntryResponse? MapTo(
		Audit.Model.ApplicationEntryResponse? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Audit.Model.ApplicationEntryResponse>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		cache ??= [];
		target ??= instanceFactory?.CreateInstance<Legion.ADF.Audit.Model.ApplicationEntryResponse>(
				useActivatorIfNoFactoryFound: false,
				throwIfFactoryReturnsNull: true)
			?? new Legion.ADF.Audit.Model.ApplicationEntryResponse();

		if (cache.TryGetValue(this, out var cached))
			return (Audit.Model.ApplicationEntryResponse)cached;
			
		MappingConditions<Audit.Model.ApplicationEntryResponse>? conds = null;
		if (conditions != null)
		{
			conds = new MappingConditions<Audit.Model.ApplicationEntryResponse>();
			conditions.Invoke(conds);

			if (conds.CanMap(this, nameof(IdApplicationEntryResponse)))
				target.IdApplicationEntryResponse = IdApplicationEntryResponse;
			if (conds.CanMap(this, nameof(IdApplicationEntry)))
				target.IdApplicationEntry = IdApplicationEntry;
			if (conds.CanMap(this, nameof(CreatedUtc)))
				target.CreatedUtc = CreatedUtc;
			if (conds.CanMap(this, nameof(ElapsedMilliseconds)))
				target.ElapsedMilliseconds = ElapsedMilliseconds;
			if (conds.CanMap(this, nameof(StatusCode)))
				target.StatusCode = StatusCode;
			if (conds.CanMap(this, nameof(Metadata)))
				target.Metadata = Metadata;
			if (conds.CanMap(this, nameof(Error)))
				target.Error = Error;
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
			target.IdApplicationEntryResponse = IdApplicationEntryResponse;
			target.IdApplicationEntry = IdApplicationEntry;
			target.CreatedUtc = CreatedUtc;
			target.ElapsedMilliseconds = ElapsedMilliseconds;
			target.StatusCode = StatusCode;
			target.Metadata = Metadata;
			target.Error = Error;
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
