using Legion;
using Legion.Model.Mappers;

namespace Legion.ADF.Auth.Model;

public sealed partial class UserPermission : Auth.AuthBaseEntity, Legion.Model.Audit.ISelfAuditableEntity, Legion.Model.Concurrence.IConcurrent, Legion.Model.Audit.IAuditableEntity, Legion.Model.IEntity
{
	public static Auth.Model.UserPermission? Map(
		Auth.Model.UserPermission source,
		Auth.Model.UserPermission? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Auth.Model.UserPermission>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		Throw.IfArgumentNull(source);

		return source.MapTo(target, referenceModifier, conditions, instanceFactory, cache);
	}

	public Auth.Model.UserPermission? Clone(
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Auth.Model.UserPermission>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
		=> MapTo(target: null, referenceModifier, conditions, instanceFactory, cache);

	public Auth.Model.UserPermission? MapTo(
		Auth.Model.UserPermission? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Auth.Model.UserPermission>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		cache ??= [];
		target ??= instanceFactory?.CreateInstance<Legion.ADF.Auth.Model.UserPermission>(
				useActivatorIfNoFactoryFound: false,
				throwIfFactoryReturnsNull: true)
			?? new Legion.ADF.Auth.Model.UserPermission();

		if (cache.TryGetValue(this, out var cached))
			return (Auth.Model.UserPermission)cached;
			
		MappingConditions<Auth.Model.UserPermission>? conds = null;
		if (conditions != null)
		{
			conds = new MappingConditions<Auth.Model.UserPermission>();
			conditions.Invoke(conds);

			if (conds.CanMap(this, nameof(IdUserPermission)))
				target.IdUserPermission = IdUserPermission;
			if (conds.CanMap(this, nameof(IdUser)))
				target.IdUser = IdUser;
			if (conds.CanMap(this, nameof(IdPermission)))
				target.IdPermission = IdPermission;
			if (conds.CanMap(this, nameof(TenantIdentifier)))
				target.TenantIdentifier = TenantIdentifier;
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
			target.IdUserPermission = IdUserPermission;
			target.IdUser = IdUser;
			target.IdPermission = IdPermission;
			target.TenantIdentifier = TenantIdentifier;
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
			target.User = User?.MapTo(target.User, referenceModifier, conds?.GetConditions(x => x.User), instanceFactory, cache)!;
		}
		else if (referenceModifier == ReferenceModifier.SetNull)
		{
			target.Permission = null!;
			target.User = null!;
		}

		return target;
	}
}
