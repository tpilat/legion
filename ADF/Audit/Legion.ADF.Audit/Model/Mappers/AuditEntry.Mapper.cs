using Legion;
using Legion.Model.Mappers;

namespace Legion.ADF.Audit.Model;

public sealed partial class AuditEntry : Audit.AuditBaseEntity, Legion.Model.IEntity
{
	public static Audit.Model.AuditEntry? Map(
		Audit.Model.AuditEntry source,
		Audit.Model.AuditEntry? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Audit.Model.AuditEntry>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		Throw.IfArgumentNull(source);

		return source.MapTo(target, referenceModifier, conditions, instanceFactory, cache);
	}

	public Audit.Model.AuditEntry? Clone(
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Audit.Model.AuditEntry>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
		=> MapTo(target: null, referenceModifier, conditions, instanceFactory, cache);

	public Audit.Model.AuditEntry? MapTo(
		Audit.Model.AuditEntry? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Audit.Model.AuditEntry>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		cache ??= [];
		target ??= instanceFactory?.CreateInstance<Legion.ADF.Audit.Model.AuditEntry>(
				useActivatorIfNoFactoryFound: false,
				throwIfFactoryReturnsNull: true)
			?? new Legion.ADF.Audit.Model.AuditEntry();

		if (cache.TryGetValue(this, out var cached))
			return (Audit.Model.AuditEntry)cached;
			
		MappingConditions<Audit.Model.AuditEntry>? conds = null;
		if (conditions != null)
		{
			conds = new MappingConditions<Audit.Model.AuditEntry>();
			conditions.Invoke(conds);

			if (conds.CanMap(this, nameof(IdAuditEntry)))
				target.IdAuditEntry = IdAuditEntry;
			if (conds.CanMap(this, nameof(CreatedUtc)))
				target.CreatedUtc = CreatedUtc;
			if (conds.CanMap(this, nameof(IdAuditOperation)))
				target.IdAuditOperation = IdAuditOperation;
			if (conds.CanMap(this, nameof(TableName)))
				target.TableName = TableName;
			if (conds.CanMap(this, nameof(IdUser)))
				target.IdUser = IdUser;
			if (conds.CanMap(this, nameof(PrimaryKey)))
				target.PrimaryKey = PrimaryKey;
			if (conds.CanMap(this, nameof(OldValues)))
				target.OldValues = OldValues;
			if (conds.CanMap(this, nameof(NewValues)))
				target.NewValues = NewValues;
			if (conds.CanMap(this, nameof(AffectedColumns)))
				target.AffectedColumns = AffectedColumns;
			if (conds.CanMap(this, nameof(AuditCorrelationId)))
				target.AuditCorrelationId = AuditCorrelationId;
			if (conds.CanMap(this, nameof(TraceFrame)))
				target.TraceFrame = TraceFrame;
			if (conds.CanMap(this, nameof(CorrelationId)))
				target.CorrelationId = CorrelationId;
		}
		else
		{
			target.IdAuditEntry = IdAuditEntry;
			target.CreatedUtc = CreatedUtc;
			target.IdAuditOperation = IdAuditOperation;
			target.TableName = TableName;
			target.IdUser = IdUser;
			target.PrimaryKey = PrimaryKey;
			target.OldValues = OldValues;
			target.NewValues = NewValues;
			target.AffectedColumns = AffectedColumns;
			target.AuditCorrelationId = AuditCorrelationId;
			target.TraceFrame = TraceFrame;
			target.CorrelationId = CorrelationId;
		}

		cache.Add(this, target);

		if (referenceModifier == ReferenceModifier.MapAllReferences)
		{
			target.AuditOperation = AuditOperation?.MapTo(target.AuditOperation, referenceModifier, conds?.GetConditions(x => x.AuditOperation), instanceFactory, cache)!;
		}
		else if (referenceModifier == ReferenceModifier.SetNull)
		{
			target.AuditOperation = null!;
		}

		return target;
	}
}
