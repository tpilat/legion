using Legion;
using Legion.Model.Mappers;

namespace Legion.ADF.Logs.Model;

public sealed partial class RemoteRequestPayload : Logs.LogsBaseEntity, Legion.Model.IEntity
{
	public static Logs.Model.RemoteRequestPayload? Map(
		Logs.Model.RemoteRequestPayload source,
		Logs.Model.RemoteRequestPayload? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Logs.Model.RemoteRequestPayload>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		Throw.IfArgumentNull(source);

		return source.MapTo(target, referenceModifier, conditions, instanceFactory, cache);
	}

	public Logs.Model.RemoteRequestPayload? Clone(
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Logs.Model.RemoteRequestPayload>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
		=> MapTo(target: null, referenceModifier, conditions, instanceFactory, cache);

	public Logs.Model.RemoteRequestPayload? MapTo(
		Logs.Model.RemoteRequestPayload? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Logs.Model.RemoteRequestPayload>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		cache ??= [];
		target ??= instanceFactory?.CreateInstance<Legion.ADF.Logs.Model.RemoteRequestPayload>(
				useActivatorIfNoFactoryFound: false,
				throwIfFactoryReturnsNull: true)
			?? new Legion.ADF.Logs.Model.RemoteRequestPayload();

		if (cache.TryGetValue(this, out var cached))
			return (Logs.Model.RemoteRequestPayload)cached;
			
		MappingConditions<Logs.Model.RemoteRequestPayload>? conds = null;
		if (conditions != null)
		{
			conds = new MappingConditions<Logs.Model.RemoteRequestPayload>();
			conditions.Invoke(conds);

			if (conds.CanMap(this, nameof(IdRemoteRequestPayload)))
				target.IdRemoteRequestPayload = IdRemoteRequestPayload;
			if (conds.CanMap(this, nameof(IdRemoteRequest)))
				target.IdRemoteRequest = IdRemoteRequest;
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
			target.IdRemoteRequestPayload = IdRemoteRequestPayload;
			target.IdRemoteRequest = IdRemoteRequest;
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
			target.RemoteRequest = RemoteRequest?.MapTo(target.RemoteRequest, referenceModifier, conds?.GetConditions(x => x.RemoteRequest), instanceFactory, cache)!;
		}
		else if (referenceModifier == ReferenceModifier.SetNull)
		{
			target.RemoteRequest = null!;
		}

		return target;
	}
}
