using Legion;
using Legion.Model.Mappers;

namespace Legion.ADF.Auth.Model;

public partial class Role : Auth.AuthBaseEntity, Legion.Model.Audit.ISelfAuditableEntity, Legion.Model.Concurrence.IConcurrent, Legion.Model.Audit.IAuditableEntity, Legion.Model.IEntity
{
	public static Auth.Model.Role? Map(
		Auth.Model.Role source,
		Auth.Model.Role? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Auth.Model.Role>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		Throw.IfArgumentNull(source);

		return source.MapTo(target, referenceModifier, conditions, instanceFactory, cache);
	}

	public Auth.Model.Role? Clone(
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Auth.Model.Role>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
		=> MapTo(target: null, referenceModifier, conditions, instanceFactory, cache);

	public Auth.Model.Role? MapTo(
		Auth.Model.Role? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Auth.Model.Role>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		cache ??= [];
		target ??= instanceFactory?.CreateInstance<Legion.ADF.Auth.Model.Role>(
				useActivatorIfNoFactoryFound: false,
				throwIfFactoryReturnsNull: true)
			?? new Legion.ADF.Auth.Model.Role();

		if (cache.TryGetValue(this, out var cached))
			return (Auth.Model.Role)cached;
			
		MappingConditions<Auth.Model.Role>? conds = null;
		if (conditions != null)
		{
			conds = new MappingConditions<Auth.Model.Role>();
			conditions.Invoke(conds);

			if (conds.CanMap(this, nameof(IdRole)))
				target.IdRole = IdRole;
			if (conds.CanMap(this, nameof(Name)))
				target.Name = Name;
			if (conds.CanMap(this, nameof(NormalizedName)))
				target.NormalizedName = NormalizedName;
			if (conds.CanMap(this, nameof(ADGroupDistinguishedName)))
				target.ADGroupDistinguishedName = ADGroupDistinguishedName;
			if (conds.CanMap(this, nameof(Data)))
				target.Data = Data;
			if (conds.CanMap(this, nameof(Description)))
				target.Description = Description;
			if (conds.CanMap(this, nameof(HasConstantPermissions)))
				target.HasConstantPermissions = HasConstantPermissions;
			if (conds.CanMap(this, nameof(HasConstantUsers)))
				target.HasConstantUsers = HasConstantUsers;
			if (conds.CanMap(this, nameof(IsSystemRole)))
				target.IsSystemRole = IsSystemRole;
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
			target.IdRole = IdRole;
			target.Name = Name;
			target.NormalizedName = NormalizedName;
			target.ADGroupDistinguishedName = ADGroupDistinguishedName;
			target.Data = Data;
			target.Description = Description;
			target.HasConstantPermissions = HasConstantPermissions;
			target.HasConstantUsers = HasConstantUsers;
			target.IsSystemRole = IsSystemRole;
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
			target._rolePermissions = MapperHelper.MapToList(RolePermissions, target._rolePermissions, RolePermission.Map, referenceModifier, conds?.GetConditions(x => x.RolePermissions), instanceFactory, cache)!;
			target._userRoles = MapperHelper.MapToList(UserRoles, target._userRoles, UserRole.Map, referenceModifier, conds?.GetConditions(x => x.UserRoles), instanceFactory, cache)!;
		}
		else if (referenceModifier == ReferenceModifier.SetNull)
		{
			target._rolePermissions = [];
			target._userRoles = [];
		}

		return target;
	}
}
