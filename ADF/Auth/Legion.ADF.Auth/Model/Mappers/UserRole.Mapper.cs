using Legion;
using Legion.Model.Mappers;

namespace Legion.ADF.Auth.Model;

public sealed partial class UserRole : Auth.AuthBaseEntity, Legion.Model.Audit.ISelfAuditableEntity, Legion.Model.Concurrence.IConcurrent, Legion.Model.Audit.IAuditableEntity, Legion.Model.IEntity
{
	public static Auth.Model.UserRole? Map(
		Auth.Model.UserRole source,
		Auth.Model.UserRole? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Auth.Model.UserRole>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		Throw.IfArgumentNull(source);

		return source.MapTo(target, referenceModifier, conditions, instanceFactory, cache);
	}

	public Auth.Model.UserRole? Clone(
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Auth.Model.UserRole>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
		=> MapTo(target: null, referenceModifier, conditions, instanceFactory, cache);

	public Auth.Model.UserRole? MapTo(
		Auth.Model.UserRole? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Auth.Model.UserRole>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		cache ??= [];
		target ??= instanceFactory?.CreateInstance<Legion.ADF.Auth.Model.UserRole>(
				useActivatorIfNoFactoryFound: false,
				throwIfFactoryReturnsNull: true)
			?? new Legion.ADF.Auth.Model.UserRole();

		if (cache.TryGetValue(this, out var cached))
			return (Auth.Model.UserRole)cached;
			
		MappingConditions<Auth.Model.UserRole>? conds = null;
		if (conditions != null)
		{
			conds = new MappingConditions<Auth.Model.UserRole>();
			conditions.Invoke(conds);

			if (conds.CanMap(this, nameof(IdUserRole)))
				target.IdUserRole = IdUserRole;
			if (conds.CanMap(this, nameof(IdUser)))
				target.IdUser = IdUser;
			if (conds.CanMap(this, nameof(IdRole)))
				target.IdRole = IdRole;
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
			target.IdUserRole = IdUserRole;
			target.IdUser = IdUser;
			target.IdRole = IdRole;
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
			target.Role = Role?.MapTo(target.Role, referenceModifier, conds?.GetConditions(x => x.Role), instanceFactory, cache)!;
			target.User = User?.MapTo(target.User, referenceModifier, conds?.GetConditions(x => x.User), instanceFactory, cache)!;
		}
		else if (referenceModifier == ReferenceModifier.SetNull)
		{
			target.Role = null!;
			target.User = null!;
		}

		return target;
	}
}
