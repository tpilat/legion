using Legion;
using Legion.Model.Mappers;

namespace Legion.ADF.Auth.Model;

public sealed partial class Permission : Auth.AuthBaseEntity, Legion.Model.IEntity
{
	public static Auth.Model.Permission? Map(
		Auth.Model.Permission source,
		Auth.Model.Permission? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Auth.Model.Permission>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		Throw.IfArgumentNull(source);

		return source.MapTo(target, referenceModifier, conditions, instanceFactory, cache);
	}

	public Auth.Model.Permission? Clone(
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Auth.Model.Permission>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
		=> MapTo(target: null, referenceModifier, conditions, instanceFactory, cache);

	public Auth.Model.Permission? MapTo(
		Auth.Model.Permission? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Auth.Model.Permission>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		cache ??= [];
		target ??= instanceFactory?.CreateInstance<Legion.ADF.Auth.Model.Permission>(
				useActivatorIfNoFactoryFound: false,
				throwIfFactoryReturnsNull: true)
			?? new Legion.ADF.Auth.Model.Permission();

		if (cache.TryGetValue(this, out var cached))
			return (Auth.Model.Permission)cached;
			
		MappingConditions<Auth.Model.Permission>? conds = null;
		if (conditions != null)
		{
			conds = new MappingConditions<Auth.Model.Permission>();
			conditions.Invoke(conds);

			if (conds.CanMap(this, nameof(IdPermission)))
				target.IdPermission = IdPermission;
			if (conds.CanMap(this, nameof(Code)))
				target.Code = Code;
			if (conds.CanMap(this, nameof(Name)))
				target.Name = Name;
			if (conds.CanMap(this, nameof(Description)))
				target.Description = Description;
			if (conds.CanMap(this, nameof(ClaimValue)))
				target.ClaimValue = ClaimValue;
			if (conds.CanMap(this, nameof(IsSystemPermission)))
				target.IsSystemPermission = IsSystemPermission;
		}
		else
		{
			target.IdPermission = IdPermission;
			target.Code = Code;
			target.Name = Name;
			target.Description = Description;
			target.ClaimValue = ClaimValue;
			target.IsSystemPermission = IsSystemPermission;
		}

		cache.Add(this, target);

		if (referenceModifier == ReferenceModifier.MapAllReferences)
		{
			target._rolePermissions = MapperHelper.MapToList(RolePermissions, target._rolePermissions, RolePermission.Map, referenceModifier, conds?.GetConditions(x => x.RolePermissions), instanceFactory, cache)!;
			target._userPermissions = MapperHelper.MapToList(UserPermissions, target._userPermissions, UserPermission.Map, referenceModifier, conds?.GetConditions(x => x.UserPermissions), instanceFactory, cache)!;
		}
		else if (referenceModifier == ReferenceModifier.SetNull)
		{
			target._rolePermissions = [];
			target._userPermissions = [];
		}

		return target;
	}
}
