using Legion;
using Legion.Model.Mappers;

namespace Legion.ADF.Logs.Model;

public sealed partial class LocalRequest : Logs.LogsBaseEntity, Legion.Model.IEntity
{
	public static Logs.Model.LocalRequest? Map(
		Logs.Model.LocalRequest source,
		Logs.Model.LocalRequest? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Logs.Model.LocalRequest>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		Throw.IfArgumentNull(source);

		return source.MapTo(target, referenceModifier, conditions, instanceFactory, cache);
	}

	public Logs.Model.LocalRequest? Clone(
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Logs.Model.LocalRequest>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
		=> MapTo(target: null, referenceModifier, conditions, instanceFactory, cache);

	public Logs.Model.LocalRequest? MapTo(
		Logs.Model.LocalRequest? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Logs.Model.LocalRequest>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		cache ??= [];
		target ??= instanceFactory?.CreateInstance<Legion.ADF.Logs.Model.LocalRequest>(
				useActivatorIfNoFactoryFound: false,
				throwIfFactoryReturnsNull: true)
			?? new Legion.ADF.Logs.Model.LocalRequest();

		if (cache.TryGetValue(this, out var cached))
			return (Logs.Model.LocalRequest)cached;
			
		MappingConditions<Logs.Model.LocalRequest>? conds = null;
		if (conditions != null)
		{
			conds = new MappingConditions<Logs.Model.LocalRequest>();
			conditions.Invoke(conds);

			if (conds.CanMap(this, nameof(IdLocalRequest)))
				target.IdLocalRequest = IdLocalRequest;
			if (conds.CanMap(this, nameof(IdRemoteSystem)))
				target.IdRemoteSystem = IdRemoteSystem;
			if (conds.CanMap(this, nameof(RemoteIp)))
				target.RemoteIp = RemoteIp;
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
			if (conds.CanMap(this, nameof(Path)))
				target.Path = Path;
			if (conds.CanMap(this, nameof(QueryString)))
				target.QueryString = QueryString;
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
			target.IdLocalRequest = IdLocalRequest;
			target.IdRemoteSystem = IdRemoteSystem;
			target.RemoteIp = RemoteIp;
			target.CreatedUtc = CreatedUtc;
			target.CorrelationId = CorrelationId;
			target.ExternalCorrelationId = ExternalCorrelationId;
			target.SourceClientIdentifier = SourceClientIdentifier;
			target.Url = Url;
			target.Path = Path;
			target.QueryString = QueryString;
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
			target._localRequestPayloads = MapperHelper.MapToList(LocalRequestPayloads, target._localRequestPayloads, LocalRequestPayload.Map, referenceModifier, conds?.GetConditions(x => x.LocalRequestPayloads), instanceFactory, cache)!;
			target._localResponses = MapperHelper.MapToList(LocalResponses, target._localResponses, LocalResponse.Map, referenceModifier, conds?.GetConditions(x => x.LocalResponses), instanceFactory, cache)!;
		}
		else if (referenceModifier == ReferenceModifier.SetNull)
		{
			target.RemoteSystem = null!;
			target._localRequestPayloads = [];
			target._localResponses = [];
		}

		return target;
	}
}
