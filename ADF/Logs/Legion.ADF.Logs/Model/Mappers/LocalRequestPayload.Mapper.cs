using Legion;
using Legion.Model.Mappers;

namespace Legion.ADF.Logs.Model;

public sealed partial class LocalRequestPayload : Logs.LogsBaseEntity, Legion.Model.IEntity
{
	public static Logs.Model.LocalRequestPayload? Map(
		Logs.Model.LocalRequestPayload source,
		Logs.Model.LocalRequestPayload? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Logs.Model.LocalRequestPayload>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		Throw.IfArgumentNull(source);

		return source.MapTo(target, referenceModifier, conditions, instanceFactory, cache);
	}

	public Logs.Model.LocalRequestPayload? Clone(
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Logs.Model.LocalRequestPayload>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
		=> MapTo(target: null, referenceModifier, conditions, instanceFactory, cache);

	public Logs.Model.LocalRequestPayload? MapTo(
		Logs.Model.LocalRequestPayload? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Logs.Model.LocalRequestPayload>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		cache ??= [];
		target ??= instanceFactory?.CreateInstance<Legion.ADF.Logs.Model.LocalRequestPayload>(
				useActivatorIfNoFactoryFound: false,
				throwIfFactoryReturnsNull: true)
			?? new Legion.ADF.Logs.Model.LocalRequestPayload();

		if (cache.TryGetValue(this, out var cached))
			return (Logs.Model.LocalRequestPayload)cached;
			
		MappingConditions<Logs.Model.LocalRequestPayload>? conds = null;
		if (conditions != null)
		{
			conds = new MappingConditions<Logs.Model.LocalRequestPayload>();
			conditions.Invoke(conds);

			if (conds.CanMap(this, nameof(IdLocalRequestPayload)))
				target.IdLocalRequestPayload = IdLocalRequestPayload;
			if (conds.CanMap(this, nameof(IdLocalRequest)))
				target.IdLocalRequest = IdLocalRequest;
			if (conds.CanMap(this, nameof(CreatedUtc)))
				target.CreatedUtc = CreatedUtc;
			if (conds.CanMap(this, nameof(RequestContentType)))
				target.RequestContentType = RequestContentType;
			if (conds.CanMap(this, nameof(ByteArrayContent)))
				target.ByteArrayContent = ByteArrayContent?.ToArray();
			if (conds.CanMap(this, nameof(JsonContent)))
				target.JsonContent = JsonContent;
			if (conds.CanMap(this, nameof(StringContent)))
				target.StringContent = StringContent;
			if (conds.CanMap(this, nameof(ContentHeaders)))
				target.ContentHeaders = ContentHeaders;
			if (conds.CanMap(this, nameof(DbOid)))
				target.DbOid = DbOid;
			if (conds.CanMap(this, nameof(FileName)))
				target.FileName = FileName;
			if (conds.CanMap(this, nameof(RelativePath)))
				target.RelativePath = RelativePath;
			if (conds.CanMap(this, nameof(Metadata)))
				target.Metadata = Metadata;
			if (conds.CanMap(this, nameof(IsCompressed)))
				target.IsCompressed = IsCompressed;
			if (conds.CanMap(this, nameof(EncryptionKey)))
				target.EncryptionKey = EncryptionKey;
			if (conds.CanMap(this, nameof(ContentEncoding)))
				target.ContentEncoding = ContentEncoding;
			if (conds.CanMap(this, nameof(MediaType)))
				target.MediaType = MediaType;
			if (conds.CanMap(this, nameof(MultipartFormDataContentName)))
				target.MultipartFormDataContentName = MultipartFormDataContentName;
			if (conds.CanMap(this, nameof(MultipartFormDataFileName)))
				target.MultipartFormDataFileName = MultipartFormDataFileName;
			if (conds.CanMap(this, nameof(JsonInputCSharpType)))
				target.JsonInputCSharpType = JsonInputCSharpType;
		}
		else
		{
			target.IdLocalRequestPayload = IdLocalRequestPayload;
			target.IdLocalRequest = IdLocalRequest;
			target.CreatedUtc = CreatedUtc;
			target.RequestContentType = RequestContentType;
			target.ByteArrayContent = ByteArrayContent?.ToArray();
			target.JsonContent = JsonContent;
			target.StringContent = StringContent;
			target.ContentHeaders = ContentHeaders;
			target.DbOid = DbOid;
			target.FileName = FileName;
			target.RelativePath = RelativePath;
			target.Metadata = Metadata;
			target.IsCompressed = IsCompressed;
			target.EncryptionKey = EncryptionKey;
			target.ContentEncoding = ContentEncoding;
			target.MediaType = MediaType;
			target.MultipartFormDataContentName = MultipartFormDataContentName;
			target.MultipartFormDataFileName = MultipartFormDataFileName;
			target.JsonInputCSharpType = JsonInputCSharpType;
		}

		cache.Add(this, target);

		if (referenceModifier == ReferenceModifier.MapAllReferences)
		{
			target.LocalRequest = LocalRequest?.MapTo(target.LocalRequest, referenceModifier, conds?.GetConditions(x => x.LocalRequest), instanceFactory, cache)!;
		}
		else if (referenceModifier == ReferenceModifier.SetNull)
		{
			target.LocalRequest = null!;
		}

		return target;
	}
}
