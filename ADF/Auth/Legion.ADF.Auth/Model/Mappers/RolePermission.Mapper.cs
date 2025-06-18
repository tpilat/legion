using Legion;
using Legion.Model.Mappers;

namespace Legion.ADF.Auth.Model;

public sealed partial class RolePermission : Auth.AuthBaseEntity, Legion.Model.Audit.ISelfAuditableEntity, Legion.Model.Concurrence.IConcurrent, Legion.Model.Audit.IAuditableEntity, Legion.Model.IEntity
{
	public static Auth.Model.RolePermission? Map(
		Auth.Model.RolePermission source,
		Auth.Model.RolePermission? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Auth.Model.RolePermission>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		Throw.IfArgumentNull(source);

		return source.MapTo(target, referenceModifier, conditions, instanceFactory, cache);
	}

	public Auth.Model.RolePermission? Clone(
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Auth.Model.RolePermission>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
		=> MapTo(target: null, referenceModifier, conditions, instanceFactory, cache);

	public Auth.Model.RolePermission? MapTo(
		Auth.Model.RolePermission? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Auth.Model.RolePermission>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		cache ??= [];
		target ??= instanceFactory?.CreateInstance<Legion.ADF.Auth.Model.RolePermission>(
				useActivatorIfNoFactoryFound: false,
				throwIfFactoryReturnsNull: true)
			?? new Legion.ADF.Auth.Model.RolePermission();

		if (cache.TryGetValue(this, out var cached))
			return (Auth.Model.RolePermission)cached;
			
		MappingConditions<Auth.Model.RolePermission>? conds = null;
		if (conditions != null)
		{
			conds = new MappingConditions<Auth.Model.RolePermission>();
			conditions.Invoke(conds);

			if (conds.CanMap(this, nameof(IdRolePermission)))
				target.IdRolePermission = IdRolePermission;
			if (conds.CanMap(this, nameof(IdRole)))
				target.IdRole = IdRole;
			if (conds.CanMap(this, nameof(IdPermission)))
				target.IdPermission = IdPermission;
			if (conds.CanMap(this, nameof(AuditCreatedUtc)))
				target.AuditCreatedUtc = AuditCreatedUtc;
			if (conds.CanMap(this, nameof(AuditModifiedUtc)))
				target.AuditModifiedUtc = AuditModifiedUtc;
			if (conds.CanMap(this, nameof(IdAuditCreatedBy)))
				target.IdAuditCreatedBy = IdAuditCreatedBy;
			if (conds.CanMap(this, nameof(IdAuditModifiedBy)))
				target.IdAuditModifiedBy = IdAuditModifiedBy;
			if (conds.CanMap(this, nameof(ConcurrencyToken)))
				target.ConcurrencyToken = ConcurrencyToken;
			if (conds.CanMap(this, nameof(DeletedUtc)))
				target.DeletedUtc = DeletedUtc;
		}
		else
		{
			target.IdRolePermission = IdRolePermission;
			target.IdRole = IdRole;
			target.IdPermission = IdPermission;
			target.AuditCreatedUtc = AuditCreatedUtc;
			target.AuditModifiedUtc = AuditModifiedUtc;
			target.IdAuditCreatedBy = IdAuditCreatedBy;
			target.IdAuditModifiedBy = IdAuditModifiedBy;
			target.ConcurrencyToken = ConcurrencyToken;
			target.DeletedUtc = DeletedUtc;
		}

		cache.Add(this, target);

		if (referenceModifier == ReferenceModifier.MapAllReferences)
		{
			target.Permission = Permission?.MapTo(target.Permission, referenceModifier, conds?.GetConditions(x => x.Permission), instanceFactory, cache)!;
			target.Role = Role?.MapTo(target.Role, referenceModifier, conds?.GetConditions(x => x.Role), instanceFactory, cache)!;
		}
		else if (referenceModifier == ReferenceModifier.SetNull)
		{
			target.Permission = null!;
			target.Role = null!;
		}

		return target;
	}
}
