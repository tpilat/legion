using Legion;
using Legion.Model.Mappers;

namespace Legion.ADF.Logs.Model;

public sealed partial class RemoteRequest : Logs.LogsBaseEntity, Legion.Model.IEntity
{
	public static Logs.Model.RemoteRequest? Map(
		Logs.Model.RemoteRequest source,
		Logs.Model.RemoteRequest? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Logs.Model.RemoteRequest>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		Throw.IfArgumentNull(source);

		return source.MapTo(target, referenceModifier, conditions, instanceFactory, cache);
	}

	public Logs.Model.RemoteRequest? Clone(
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Logs.Model.RemoteRequest>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
		=> MapTo(target: null, referenceModifier, conditions, instanceFactory, cache);

	public Logs.Model.RemoteRequest? MapTo(
		Logs.Model.RemoteRequest? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Logs.Model.RemoteRequest>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		cache ??= [];
		target ??= instanceFactory?.CreateInstance<Legion.ADF.Logs.Model.RemoteRequest>(
				useActivatorIfNoFactoryFound: false,
				throwIfFactoryReturnsNull: true)
			?? new Legion.ADF.Logs.Model.RemoteRequest();

		if (cache.TryGetValue(this, out var cached))
			return (Logs.Model.RemoteRequest)cached;
			
		MappingConditions<Logs.Model.RemoteRequest>? conds = null;
		if (conditions != null)
		{
			conds = new MappingConditions<Logs.Model.RemoteRequest>();
			conditions.Invoke(conds);

			if (conds.CanMap(this, nameof(IdRemoteRequest)))
				target.IdRemoteRequest = IdRemoteRequest;
			if (conds.CanMap(this, nameof(IdRemoteSystem)))
				target.IdRemoteSystem = IdRemoteSystem;
			if (conds.CanMap(this, nameof(CreatedUtc)))
				target.CreatedUtc = CreatedUtc;
			if (conds.CanMap(this, nameof(CorrelationId)))
				target.CorrelationId = CorrelationId;
			if (conds.CanMap(this, nameof(ExternalCorrelationId)))
				target.ExternalCorrelationId = ExternalCorrelationId;
			if (conds.CanMap(this, nameof(SourceClientIdentifier)))
				target.SourceClientIdentifier = SourceClientIdentifier;
			if (conds.CanMap(this, nameof(Url)))
				target.Url = Url;
			if (conds.CanMap(this, nameof(Method)))
				target.Method = Method;
			if (conds.CanMap(this, nameof(Headers)))
				target.Headers = Headers;
			if (conds.CanMap(this, nameof(ContentType)))
				target.ContentType = ContentType;
			if (conds.CanMap(this, nameof(Metadata)))
				target.Metadata = Metadata;
			if (conds.CanMap(this, nameof(CustomCorrelationId)))
				target.CustomCorrelationId = CustomCorrelationId;
			if (conds.CanMap(this, nameof(RuntimeUniqueKey)))
				target.RuntimeUniqueKey = RuntimeUniqueKey;
		}
		else
		{
			target.IdRemoteRequest = IdRemoteRequest;
			target.IdRemoteSystem = IdRemoteSystem;
			target.CreatedUtc = CreatedUtc;
			target.CorrelationId = CorrelationId;
			target.ExternalCorrelationId = ExternalCorrelationId;
			target.SourceClientIdentifier = SourceClientIdentifier;
			target.Url = Url;
			target.Method = Method;
			target.Headers = Headers;
			target.ContentType = ContentType;
			target.Metadata = Metadata;
			target.CustomCorrelationId = CustomCorrelationId;
			target.RuntimeUniqueKey = RuntimeUniqueKey;
		}

		cache.Add(this, target);

		if (referenceModifier == ReferenceModifier.MapAllReferences)
		{
			target.RemoteSystem = RemoteSystem?.MapTo(target.RemoteSystem, referenceModifier, conds?.GetConditions(x => x.RemoteSystem), instanceFactory, cache)!;
			target._remoteRequestPayloads = MapperHelper.MapToList(RemoteRequestPayloads, target._remoteRequestPayloads, RemoteRequestPayload.Map, referenceModifier, conds?.GetConditions(x => x.RemoteRequestPayloads), instanceFactory, cache)!;
			target._remoteResponses = MapperHelper.MapToList(RemoteResponses, target._remoteResponses, RemoteResponse.Map, referenceModifier, conds?.GetConditions(x => x.RemoteResponses), instanceFactory, cache)!;
		}
		else if (referenceModifier == ReferenceModifier.SetNull)
		{
			target.RemoteSystem = null!;
			target._remoteRequestPayloads = [];
			target._remoteResponses = [];
		}

		return target;
	}
}
