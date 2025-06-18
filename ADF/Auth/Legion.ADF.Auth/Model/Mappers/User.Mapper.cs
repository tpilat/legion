using Legion;
using Legion.Model.Mappers;

namespace Legion.ADF.Auth.Model;

public partial class User : Auth.AuthBaseEntity, Legion.Model.Audit.ISelfAuditableEntity, Legion.Model.Concurrence.IConcurrent, Legion.Model.Audit.IAuditableEntity, Legion.Model.IEntity
{
	public static Auth.Model.User? Map(
		Auth.Model.User source,
		Auth.Model.User? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Auth.Model.User>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		Throw.IfArgumentNull(source);

		return source.MapTo(target, referenceModifier, conditions, instanceFactory, cache);
	}

	public Auth.Model.User? Clone(
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Auth.Model.User>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
		=> MapTo(target: null, referenceModifier, conditions, instanceFactory, cache);

	public Auth.Model.User? MapTo(
		Auth.Model.User? target,
		ReferenceModifier referenceModifier = ReferenceModifier.SkipAllReferences,
		Action<MappingConditions<Auth.Model.User>>? conditions = null,
		Legion.Reflection.InstanceFactory? instanceFactory = null,
		Dictionary<object, object>? cache = null)
	{
		cache ??= [];
		target ??= instanceFactory?.CreateInstance<Legion.ADF.Auth.Model.User>(
				useActivatorIfNoFactoryFound: false,
				throwIfFactoryReturnsNull: true)
			?? new Legion.ADF.Auth.Model.User();

		if (cache.TryGetValue(this, out var cached))
			return (Auth.Model.User)cached;
			
		MappingConditions<Auth.Model.User>? conds = null;
		if (conditions != null)
		{
			conds = new MappingConditions<Auth.Model.User>();
			conditions.Invoke(conds);

			if (conds.CanMap(this, nameof(IdUser)))
				target.IdUser = IdUser;
			if (conds.CanMap(this, nameof(Login)))
				target.Login = Login;
			if (conds.CanMap(this, nameof(NormalizedLogin)))
				target.NormalizedLogin = NormalizedLogin;
			if (conds.CanMap(this, nameof(TenantIdentifier)))
				target.TenantIdentifier = TenantIdentifier;
			if (conds.CanMap(this, nameof(Email)))
				target.Email = Email;
			if (conds.CanMap(this, nameof(NormalizedEmail)))
				target.NormalizedEmail = NormalizedEmail;
			if (conds.CanMap(this, nameof(EmailConfirmed)))
				target.EmailConfirmed = EmailConfirmed;
			if (conds.CanMap(this, nameof(PasswordHash)))
				target.PasswordHash = PasswordHash;
			if (conds.CanMap(this, nameof(SecurityStamp)))
				target.SecurityStamp = SecurityStamp;
			if (conds.CanMap(this, nameof(ADDistinguishedName)))
				target.ADDistinguishedName = ADDistinguishedName;
			if (conds.CanMap(this, nameof(Data)))
				target.Data = Data;
			if (conds.CanMap(this, nameof(PhoneNumber)))
				target.PhoneNumber = PhoneNumber;
			if (conds.CanMap(this, nameof(PhoneNumberConfirmed)))
				target.PhoneNumberConfirmed = PhoneNumberConfirmed;
			if (conds.CanMap(this, nameof(MultiFactorEnabled)))
				target.MultiFactorEnabled = MultiFactorEnabled;
			if (conds.CanMap(this, nameof(LockoutEndUtc)))
				target.LockoutEndUtc = LockoutEndUtc;
			if (conds.CanMap(this, nameof(LockoutEnabled)))
				target.LockoutEnabled = LockoutEnabled;
			if (conds.CanMap(this, nameof(AccessFailedCount)))
				target.AccessFailedCount = AccessFailedCount;
			if (conds.CanMap(this, nameof(IsSystemUser)))
				target.IsSystemUser = IsSystemUser;
			if (conds.CanMap(this, nameof(ConfirmationUrlSlug)))
				target.ConfirmationUrlSlug = ConfirmationUrlSlug;
			if (conds.CanMap(this, nameof(ConfirmationUrlValidToUtc)))
				target.ConfirmationUrlValidToUtc = ConfirmationUrlValidToUtc;
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
			target.IdUser = IdUser;
			target.Login = Login;
			target.NormalizedLogin = NormalizedLogin;
			target.TenantIdentifier = TenantIdentifier;
			target.Email = Email;
			target.NormalizedEmail = NormalizedEmail;
			target.EmailConfirmed = EmailConfirmed;
			target.PasswordHash = PasswordHash;
			target.SecurityStamp = SecurityStamp;
			target.ADDistinguishedName = ADDistinguishedName;
			target.Data = Data;
			target.PhoneNumber = PhoneNumber;
			target.PhoneNumberConfirmed = PhoneNumberConfirmed;
			target.MultiFactorEnabled = MultiFactorEnabled;
			target.LockoutEndUtc = LockoutEndUtc;
			target.LockoutEnabled = LockoutEnabled;
			target.AccessFailedCount = AccessFailedCount;
			target.IsSystemUser = IsSystemUser;
			target.ConfirmationUrlSlug = ConfirmationUrlSlug;
			target.ConfirmationUrlValidToUtc = ConfirmationUrlValidToUtc;
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
			target._externalLogins = MapperHelper.MapToList(ExternalLogins, target._externalLogins, ExternalLogin.Map, referenceModifier, conds?.GetConditions(x => x.ExternalLogins), instanceFactory, cache)!;
			target._userPermissions = MapperHelper.MapToList(UserPermissions, target._userPermissions, UserPermission.Map, referenceModifier, conds?.GetConditions(x => x.UserPermissions), instanceFactory, cache)!;
			target._userRoles = MapperHelper.MapToList(UserRoles, target._userRoles, UserRole.Map, referenceModifier, conds?.GetConditions(x => x.UserRoles), instanceFactory, cache)!;
			target._userTokens = MapperHelper.MapToList(UserTokens, target._userTokens, UserToken.Map, referenceModifier, conds?.GetConditions(x => x.UserTokens), instanceFactory, cache)!;
		}
		else if (referenceModifier == ReferenceModifier.SetNull)
		{
			target._externalLogins = [];
			target._userPermissions = [];
			target._userRoles = [];
			target._userTokens = [];
		}

		return target;
	}
}
