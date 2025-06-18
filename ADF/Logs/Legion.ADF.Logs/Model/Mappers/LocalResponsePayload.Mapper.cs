using Legion;
using Legion.Model.Mappers;

namespace Legion.ADF.Logs.Model;

public sealed partial class LocalResponsePayload : Logs.LogsBaseEntity, Legion.Model.IEntity
{
	public static Logs.Model.LocalResponsePayload? Map(
		Logs.Model.LocalResponsePayload source,
		Logs.Model.LocalResponsePayload? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Logs.Model.LocalResponsePayload>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		Throw.IfArgumentNull(source);

		return source.MapTo(target, referenceModifier, conditions, instanceFactory, cache);
	}

	public Logs.Model.LocalResponsePayload? Clone(
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Logs.Model.LocalResponsePayload>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
		=> MapTo(target: null, referenceModifier, conditions, instanceFactory, cache);

	public Logs.Model.LocalResponsePayload? MapTo(
		Logs.Model.LocalResponsePayload? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Logs.Model.LocalResponsePayload>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		cache ??= [];
		target ??= instanceFactory?.CreateInstance<Legion.ADF.Logs.Model.LocalResponsePayload>(
				useActivatorIfNoFactoryFound: false,
				throwIfFactoryReturnsNull: true)
			?? new Legion.ADF.Logs.Model.LocalResponsePayload();

		if (cache.TryGetValue(this, out var cached))
			return (Logs.Model.LocalResponsePayload)cached;
			
		MappingConditions<Logs.Model.LocalResponsePayload>? conds = null;
		if (conditions != null)
		{
			conds = new MappingConditions<Logs.Model.LocalResponsePayload>();
			conditions.Invoke(conds);

			if (conds.CanMap(this, nameof(IdLocalResponsePayload)))
				target.IdLocalResponsePayload = IdLocalResponsePayload;
			if (conds.CanMap(this, nameof(IdLocalResponse)))
				target.IdLocalResponse = IdLocalResponse;
			if (conds.CanMap(this, nameof(CreatedUtc)))
				target.CreatedUtc = CreatedUtc;
			if (conds.CanMap(this, nameof(ResponseContentType)))
				target.ResponseContentType = ResponseContentType;
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
			target.IdLocalResponsePayload = IdLocalResponsePayload;
			target.IdLocalResponse = IdLocalResponse;
			target.CreatedUtc = CreatedUtc;
			target.ResponseContentType = ResponseContentType;
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
			target.LocalResponse = LocalResponse?.MapTo(target.LocalResponse, referenceModifier, conds?.GetConditions(x => x.LocalResponse), instanceFactory, cache)!;
		}
		else if (referenceModifier == ReferenceModifier.SetNull)
		{
			target.LocalResponse = null!;
		}

		return target;
	}
}
