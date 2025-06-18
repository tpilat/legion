using Legion;
using Legion.Model.Mappers;

namespace Legion.ADF.Logs.Model;

public sealed partial class LocalResponse : Logs.LogsBaseEntity, Legion.Model.IEntity
{
	public static Logs.Model.LocalResponse? Map(
		Logs.Model.LocalResponse source,
		Logs.Model.LocalResponse? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Logs.Model.LocalResponse>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		Throw.IfArgumentNull(source);

		return source.MapTo(target, referenceModifier, conditions, instanceFactory, cache);
	}

	public Logs.Model.LocalResponse? Clone(
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Logs.Model.LocalResponse>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
		=> MapTo(target: null, referenceModifier, conditions, instanceFactory, cache);

	public Logs.Model.LocalResponse? MapTo(
		Logs.Model.LocalResponse? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Logs.Model.LocalResponse>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		cache ??= [];
		target ??= instanceFactory?.CreateInstance<Legion.ADF.Logs.Model.LocalResponse>(
				useActivatorIfNoFactoryFound: false,
				throwIfFactoryReturnsNull: true)
			?? new Legion.ADF.Logs.Model.LocalResponse();

		if (cache.TryGetValue(this, out var cached))
			return (Logs.Model.LocalResponse)cached;
			
		MappingConditions<Logs.Model.LocalResponse>? conds = null;
		if (conditions != null)
		{
			conds = new MappingConditions<Logs.Model.LocalResponse>();
			conditions.Invoke(conds);

			if (conds.CanMap(this, nameof(IdLocalResponse)))
				target.IdLocalResponse = IdLocalResponse;
			if (conds.CanMap(this, nameof(IdLocalRequest)))
				target.IdLocalRequest = IdLocalRequest;
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
			target.IdLocalResponse = IdLocalResponse;
			target.IdLocalRequest = IdLocalRequest;
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
			target.LocalRequest = LocalRequest?.MapTo(target.LocalRequest, referenceModifier, conds?.GetConditions(x => x.LocalRequest), instanceFactory, cache)!;
			target._localResponsePayloads = MapperHelper.MapToList(LocalResponsePayloads, target._localResponsePayloads, LocalResponsePayload.Map, referenceModifier, conds?.GetConditions(x => x.LocalResponsePayloads), instanceFactory, cache)!;
		}
		else if (referenceModifier == ReferenceModifier.SetNull)
		{
			target.LocalRequest = null!;
			target._localResponsePayloads = [];
		}

		return target;
	}
}
