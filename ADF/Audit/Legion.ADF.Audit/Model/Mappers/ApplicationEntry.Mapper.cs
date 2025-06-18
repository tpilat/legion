using Legion;
using Legion.Model.Mappers;

namespace Legion.ADF.Audit.Model;

public sealed partial class ApplicationEntry : Audit.AuditBaseEntity, Legion.Model.IEntity
{
	public static Audit.Model.ApplicationEntry? Map(
		Audit.Model.ApplicationEntry source,
		Audit.Model.ApplicationEntry? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Audit.Model.ApplicationEntry>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		Throw.IfArgumentNull(source);

		return source.MapTo(target, referenceModifier, conditions, instanceFactory, cache);
	}

	public Audit.Model.ApplicationEntry? Clone(
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Audit.Model.ApplicationEntry>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
		=> MapTo(target: null, referenceModifier, conditions, instanceFactory, cache);

	public Audit.Model.ApplicationEntry? MapTo(
		Audit.Model.ApplicationEntry? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Audit.Model.ApplicationEntry>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		cache ??= [];
		target ??= instanceFactory?.CreateInstance<Legion.ADF.Audit.Model.ApplicationEntry>(
				useActivatorIfNoFactoryFound: false,
				throwIfFactoryReturnsNull: true)
			?? new Legion.ADF.Audit.Model.ApplicationEntry();

		if (cache.TryGetValue(this, out var cached))
			return (Audit.Model.ApplicationEntry)cached;
			
		MappingConditions<Audit.Model.ApplicationEntry>? conds = null;
		if (conditions != null)
		{
			conds = new MappingConditions<Audit.Model.ApplicationEntry>();
			conditions.Invoke(conds);

			if (conds.CanMap(this, nameof(IdApplicationEntry)))
				target.IdApplicationEntry = IdApplicationEntry;
			if (conds.CanMap(this, nameof(IdApplicationEntryToken)))
				target.IdApplicationEntryToken = IdApplicationEntryToken;
			if (conds.CanMap(this, nameof(IdAuditOperation)))
				target.IdAuditOperation = IdAuditOperation;
			if (conds.CanMap(this, nameof(RuntimeUniqueKey)))
				target.RuntimeUniqueKey = RuntimeUniqueKey;
			if (conds.CanMap(this, nameof(CreatedUtc)))
				target.CreatedUtc = CreatedUtc;
			if (conds.CanMap(this, nameof(CorrelationId)))
				target.CorrelationId = CorrelationId;
			if (conds.CanMap(this, nameof(ExternalCorrelationId)))
				target.ExternalCorrelationId = ExternalCorrelationId;
			if (conds.CanMap(this, nameof(AggregateIdentifier)))
				target.AggregateIdentifier = AggregateIdentifier;
			if (conds.CanMap(this, nameof(HttpMethod)))
				target.HttpMethod = HttpMethod;
			if (conds.CanMap(this, nameof(Uri)))
				target.Uri = Uri;
			if (conds.CanMap(this, nameof(IdUser)))
				target.IdUser = IdUser;
			if (conds.CanMap(this, nameof(TenantIdentifier)))
				target.TenantIdentifier = TenantIdentifier;
			if (conds.CanMap(this, nameof(RemoteIP)))
				target.RemoteIP = RemoteIP;
		}
		else
		{
			target.IdApplicationEntry = IdApplicationEntry;
			target.IdApplicationEntryToken = IdApplicationEntryToken;
			target.IdAuditOperation = IdAuditOperation;
			target.RuntimeUniqueKey = RuntimeUniqueKey;
			target.CreatedUtc = CreatedUtc;
			target.CorrelationId = CorrelationId;
			target.ExternalCorrelationId = ExternalCorrelationId;
			target.AggregateIdentifier = AggregateIdentifier;
			target.HttpMethod = HttpMethod;
			target.Uri = Uri;
			target.IdUser = IdUser;
			target.TenantIdentifier = TenantIdentifier;
			target.RemoteIP = RemoteIP;
		}

		cache.Add(this, target);

		if (referenceModifier == ReferenceModifier.MapAllReferences)
		{
			target.ApplicationEntryToken = ApplicationEntryToken?.MapTo(target.ApplicationEntryToken, referenceModifier, conds?.GetConditions(x => x.ApplicationEntryToken), instanceFactory, cache)!;
			target.AuditOperation = AuditOperation?.MapTo(target.AuditOperation, referenceModifier, conds?.GetConditions(x => x.AuditOperation), instanceFactory, cache)!;
			target._applicationEntryRequests = MapperHelper.MapToList(ApplicationEntryRequests, target._applicationEntryRequests, ApplicationEntryRequest.Map, referenceModifier, conds?.GetConditions(x => x.ApplicationEntryRequests), instanceFactory, cache)!;
			target._applicationEntryResponses = MapperHelper.MapToList(ApplicationEntryResponses, target._applicationEntryResponses, ApplicationEntryResponse.Map, referenceModifier, conds?.GetConditions(x => x.ApplicationEntryResponses), instanceFactory, cache)!;
		}
		else if (referenceModifier == ReferenceModifier.SetNull)
		{
			target.ApplicationEntryToken = null!;
			target.AuditOperation = null!;
			target._applicationEntryRequests = [];
			target._applicationEntryResponses = [];
		}

		return target;
	}
}
