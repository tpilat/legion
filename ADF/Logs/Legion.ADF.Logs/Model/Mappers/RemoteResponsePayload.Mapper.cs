using Legion;
using Legion.Model.Mappers;

namespace Legion.ADF.Logs.Model;

public sealed partial class RemoteResponsePayload : Logs.LogsBaseEntity, Legion.Model.IEntity
{
	public static Logs.Model.RemoteResponsePayload? Map(
		Logs.Model.RemoteResponsePayload source,
		Logs.Model.RemoteResponsePayload? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Logs.Model.RemoteResponsePayload>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		Throw.IfArgumentNull(source);

		return source.MapTo(target, referenceModifier, conditions, instanceFactory, cache);
	}

	public Logs.Model.RemoteResponsePayload? Clone(
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Logs.Model.RemoteResponsePayload>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
		=> MapTo(target: null, referenceModifier, conditions, instanceFactory, cache);

	public Logs.Model.RemoteResponsePayload? MapTo(
		Logs.Model.RemoteResponsePayload? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Logs.Model.RemoteResponsePayload>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		cache ??= [];
		target ??= instanceFactory?.CreateInstance<Legion.ADF.Logs.Model.RemoteResponsePayload>(
				useActivatorIfNoFactoryFound: false,
				throwIfFactoryReturnsNull: true)
			?? new Legion.ADF.Logs.Model.RemoteResponsePayload();

		if (cache.TryGetValue(this, out var cached))
			return (Logs.Model.RemoteResponsePayload)cached;
			
		MappingConditions<Logs.Model.RemoteResponsePayload>? conds = null;
		if (conditions != null)
		{
			conds = new MappingConditions<Logs.Model.RemoteResponsePayload>();
			conditions.Invoke(conds);

			if (conds.CanMap(this, nameof(IdRemoteResponsePayload)))
				target.IdRemoteResponsePayload = IdRemoteResponsePayload;
			if (conds.CanMap(this, nameof(IdRemoteResponse)))
				target.IdRemoteResponse = IdRemoteResponse;
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
			target.IdRemoteResponsePayload = IdRemoteResponsePayload;
			target.IdRemoteResponse = IdRemoteResponse;
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
			target.RemoteResponse = RemoteResponse?.MapTo(target.RemoteResponse, referenceModifier, conds?.GetConditions(x => x.RemoteResponse), instanceFactory, cache)!;
		}
		else if (referenceModifier == ReferenceModifier.SetNull)
		{
			target.RemoteResponse = null!;
		}

		return target;
	}
}
