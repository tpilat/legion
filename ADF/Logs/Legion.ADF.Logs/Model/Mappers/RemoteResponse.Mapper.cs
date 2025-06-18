using Legion;
using Legion.Model.Mappers;

namespace Legion.ADF.Logs.Model;

public sealed partial class RemoteResponse : Logs.LogsBaseEntity, Legion.Model.IEntity
{
	public static Logs.Model.RemoteResponse? Map(
		Logs.Model.RemoteResponse source,
		Logs.Model.RemoteResponse? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Logs.Model.RemoteResponse>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		Throw.IfArgumentNull(source);

		return source.MapTo(target, referenceModifier, conditions, instanceFactory, cache);
	}

	public Logs.Model.RemoteResponse? Clone(
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Logs.Model.RemoteResponse>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
		=> MapTo(target: null, referenceModifier, conditions, instanceFactory, cache);

	public Logs.Model.RemoteResponse? MapTo(
		Logs.Model.RemoteResponse? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Logs.Model.RemoteResponse>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		cache ??= [];
		target ??= instanceFactory?.CreateInstance<Legion.ADF.Logs.Model.RemoteResponse>(
				useActivatorIfNoFactoryFound: false,
				throwIfFactoryReturnsNull: true)
			?? new Legion.ADF.Logs.Model.RemoteResponse();

		if (cache.TryGetValue(this, out var cached))
			return (Logs.Model.RemoteResponse)cached;
			
		MappingConditions<Logs.Model.RemoteResponse>? conds = null;
		if (conditions != null)
		{
			conds = new MappingConditions<Logs.Model.RemoteResponse>();
			conditions.Invoke(conds);

			if (conds.CanMap(this, nameof(IdRemoteResponse)))
				target.IdRemoteResponse = IdRemoteResponse;
			if (conds.CanMap(this, nameof(IdRemoteRequest)))
				target.IdRemoteRequest = IdRemoteRequest;
			if (conds.CanMap(this, nameof(CreatedUtc)))
				target.CreatedUtc = CreatedUtc;
			if (conds.CanMap(this, nameof(CorrelationId)))
				target.CorrelationId = CorrelationId;
			if (conds.CanMap(this, nameof(ExternalCorrelationId)))
				target.ExternalCorrelationId = ExternalCorrelationId;
			if (conds.CanMap(this, nameof(StatusCode)))
				target.StatusCode = StatusCode;
			if (conds.CanMap(this, nameof(Reason)))
				target.Reason = Reason;
			if (conds.CanMap(this, nameof(Headers)))
				target.Headers = Headers;
			if (conds.CanMap(this, nameof(ContentType)))
				target.ContentType = ContentType;
			if (conds.CanMap(this, nameof(Error)))
				target.Error = Error;
			if (conds.CanMap(this, nameof(ElapsedMilliseconds)))
				target.ElapsedMilliseconds = ElapsedMilliseconds;
			if (conds.CanMap(this, nameof(Metadata)))
				target.Metadata = Metadata;
			if (conds.CanMap(this, nameof(CustomCorrelationId)))
				target.CustomCorrelationId = CustomCorrelationId;
			if (conds.CanMap(this, nameof(RuntimeUniqueKey)))
				target.RuntimeUniqueKey = RuntimeUniqueKey;
		}
		else
		{
			target.IdRemoteResponse = IdRemoteResponse;
			target.IdRemoteRequest = IdRemoteRequest;
			target.CreatedUtc = CreatedUtc;
			target.CorrelationId = CorrelationId;
			target.ExternalCorrelationId = ExternalCorrelationId;
			target.StatusCode = StatusCode;
			target.Reason = Reason;
			target.Headers = Headers;
			target.ContentType = ContentType;
			target.Error = Error;
			target.ElapsedMilliseconds = ElapsedMilliseconds;
			target.Metadata = Metadata;
			target.CustomCorrelationId = CustomCorrelationId;
			target.RuntimeUniqueKey = RuntimeUniqueKey;
		}

		cache.Add(this, target);

		if (referenceModifier == ReferenceModifier.MapAllReferences)
		{
			target.RemoteRequest = RemoteRequest?.MapTo(target.RemoteRequest, referenceModifier, conds?.GetConditions(x => x.RemoteRequest), instanceFactory, cache)!;
			target._remoteResponsePayloads = MapperHelper.MapToList(RemoteResponsePayloads, target._remoteResponsePayloads, RemoteResponsePayload.Map, referenceModifier, conds?.GetConditions(x => x.RemoteResponsePayloads), instanceFactory, cache)!;
		}
		else if (referenceModifier == ReferenceModifier.SetNull)
		{
			target.RemoteRequest = null!;
			target._remoteResponsePayloads = [];
		}

		return target;
	}
}
