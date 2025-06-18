using Legion;
using Legion.Model.Mappers;

namespace Legion.ADF.Audit.Model;

public sealed partial class AuditOperation : Audit.AuditBaseEntity, Legion.Model.IEntity
{
	public static Audit.Model.AuditOperation? Map(
		Audit.Model.AuditOperation source,
		Audit.Model.AuditOperation? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Audit.Model.AuditOperation>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		Throw.IfArgumentNull(source);

		return source.MapTo(target, referenceModifier, conditions, instanceFactory, cache);
	}

	public Audit.Model.AuditOperation? Clone(
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Audit.Model.AuditOperation>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
		=> MapTo(target: null, referenceModifier, conditions, instanceFactory, cache);

	public Audit.Model.AuditOperation? MapTo(
		Audit.Model.AuditOperation? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Audit.Model.AuditOperation>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		cache ??= [];
		target ??= Legion.ADF.Audit.Model.AuditOperation.DictionaryMap.Value[IdAuditOperation];

		if (cache.TryGetValue(this, out var cached))
			return (Audit.Model.AuditOperation)cached;
			
		MappingConditions<Audit.Model.AuditOperation>? conds = null;
		if (conditions != null)
		{
			conds = new MappingConditions<Audit.Model.AuditOperation>();
			conditions.Invoke(conds);
		}

		cache.Add(this, target);

		if (referenceModifier == ReferenceModifier.MapAllReferences)
		{
			target._applicationEntries = MapperHelper.MapToList(ApplicationEntries, target._applicationEntries, ApplicationEntry.Map, referenceModifier, conds?.GetConditions(x => x.ApplicationEntries), instanceFactory, cache)!;
			target._auditEntries = MapperHelper.MapToList(AuditEntries, target._auditEntries, AuditEntry.Map, referenceModifier, conds?.GetConditions(x => x.AuditEntries), instanceFactory, cache)!;
		}
		else if (referenceModifier == ReferenceModifier.SetNull)
		{
			target._applicationEntries = [];
			target._auditEntries = [];
		}

		return target;
	}
}
