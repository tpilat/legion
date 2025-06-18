using Legion;
using Legion.Model.Comparers;
using System.Diagnostics.CodeAnalysis;

namespace Legion.ADF.Auth.Model;

public partial class User : Auth.AuthBaseEntity, Legion.Model.Audit.ISelfAuditableEntity, Legion.Model.Concurrence.IConcurrent, Legion.Model.Audit.IAuditableEntity, Legion.Model.IEntity
{
	public bool EqualsTo(
		Auth.Model.User? obj,
		ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
		Action<ComparisonConditions<Auth.Model.User>>? conditions = null)
		=> UserEqualityComparer.EqualsTo(
			this,
			obj,
			comparisonOptions,
			conditions,
			cache: null);

	public partial class UserEqualityComparer : IEqualityComparer<User>
	{
		public static bool EqualsTo(
			Auth.Model.User? obj1,
			Auth.Model.User? obj2,
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<Auth.Model.User>>? conditions = null,
			Dictionary<object, HashSet<object>>? cache = null)
		{
			if (obj1 == null && obj2 == null)
				return true;

			if (obj1 == null || obj2 == null)
				return false;

			if (ReferenceEquals(obj1, obj2))
				return true;

			cache ??= [];

			cache.TryGetValue(obj1, out HashSet<object>? cachedHashSet);
			if (cachedHashSet?.Contains(obj2) == true)
				return true;
			
			ComparisonConditions<Auth.Model.User>? conds = null;
		
			if ((ComparisonOptions.CompareProperties & comparisonOptions) == ComparisonOptions.CompareProperties)
			{
				if (conditions != null)
				{
					conds = new ComparisonConditions<Auth.Model.User>();
					conditions.Invoke(conds);

					if (conds.CanCompare(obj1, nameof(obj1.IdUser)) && obj1.IdUser != obj2.IdUser)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.Login)) && !string.Equals(obj1.Login, obj2.Login))
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.NormalizedLogin)) && !string.Equals(obj1.NormalizedLogin, obj2.NormalizedLogin))
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.TenantIdentifier)) && obj1.TenantIdentifier != obj2.TenantIdentifier)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.Email)) && !string.Equals(obj1.Email, obj2.Email))
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.NormalizedEmail)) && !string.Equals(obj1.NormalizedEmail, obj2.NormalizedEmail))
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.EmailConfirmed)) && obj1.EmailConfirmed != obj2.EmailConfirmed)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.PasswordHash)) && !string.Equals(obj1.PasswordHash, obj2.PasswordHash))
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.SecurityStamp)) && !string.Equals(obj1.SecurityStamp, obj2.SecurityStamp))
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.ADDistinguishedName)) && !string.Equals(obj1.ADDistinguishedName, obj2.ADDistinguishedName))
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.Data)) && !string.Equals(obj1.Data, obj2.Data))
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.PhoneNumber)) && !string.Equals(obj1.PhoneNumber, obj2.PhoneNumber))
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.PhoneNumberConfirmed)) && obj1.PhoneNumberConfirmed != obj2.PhoneNumberConfirmed)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.MultiFactorEnabled)) && obj1.MultiFactorEnabled != obj2.MultiFactorEnabled)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.LockoutEndUtc)) && obj1.LockoutEndUtc != obj2.LockoutEndUtc)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.LockoutEnabled)) && obj1.LockoutEnabled != obj2.LockoutEnabled)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.AccessFailedCount)) && obj1.AccessFailedCount != obj2.AccessFailedCount)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.IsSystemUser)) && obj1.IsSystemUser != obj2.IsSystemUser)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.ConfirmationUrlSlug)) && !string.Equals(obj1.ConfirmationUrlSlug, obj2.ConfirmationUrlSlug))
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.ConfirmationUrlValidToUtc)) && obj1.ConfirmationUrlValidToUtc != obj2.ConfirmationUrlValidToUtc)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.AuditCreatedUtc)) && obj1.AuditCreatedUtc != obj2.AuditCreatedUtc)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.AuditModifiedUtc)) && obj1.AuditModifiedUtc != obj2.AuditModifiedUtc)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.IdAuditCreatedBy)) && obj1.IdAuditCreatedBy != obj2.IdAuditCreatedBy)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.IdAuditModifiedBy)) && obj1.IdAuditModifiedBy != obj2.IdAuditModifiedBy)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.ConcurrencyToken)) && obj1.ConcurrencyToken != obj2.ConcurrencyToken)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.DeletedUtc)) && obj1.DeletedUtc != obj2.DeletedUtc)
						return false;
				}
				else
				{
					if (obj1.IdUser != obj2.IdUser)
						return false;
					if (!string.Equals(obj1.Login, obj2.Login))
						return false;
					if (!string.Equals(obj1.NormalizedLogin, obj2.NormalizedLogin))
						return false;
					if (obj1.TenantIdentifier != obj2.TenantIdentifier)
						return false;
					if (!string.Equals(obj1.Email, obj2.Email))
						return false;
					if (!string.Equals(obj1.NormalizedEmail, obj2.NormalizedEmail))
						return false;
					if (obj1.EmailConfirmed != obj2.EmailConfirmed)
						return false;
					if (!string.Equals(obj1.PasswordHash, obj2.PasswordHash))
						return false;
					if (!string.Equals(obj1.SecurityStamp, obj2.SecurityStamp))
						return false;
					if (!string.Equals(obj1.ADDistinguishedName, obj2.ADDistinguishedName))
						return false;
					if (!string.Equals(obj1.Data, obj2.Data))
						return false;
					if (!string.Equals(obj1.PhoneNumber, obj2.PhoneNumber))
						return false;
					if (obj1.PhoneNumberConfirmed != obj2.PhoneNumberConfirmed)
						return false;
					if (obj1.MultiFactorEnabled != obj2.MultiFactorEnabled)
						return false;
					if (obj1.LockoutEndUtc != obj2.LockoutEndUtc)
						return false;
					if (obj1.LockoutEnabled != obj2.LockoutEnabled)
						return false;
					if (obj1.AccessFailedCount != obj2.AccessFailedCount)
						return false;
					if (obj1.IsSystemUser != obj2.IsSystemUser)
						return false;
					if (!string.Equals(obj1.ConfirmationUrlSlug, obj2.ConfirmationUrlSlug))
						return false;
					if (obj1.ConfirmationUrlValidToUtc != obj2.ConfirmationUrlValidToUtc)
						return false;
					if (obj1.AuditCreatedUtc != obj2.AuditCreatedUtc)
						return false;
					if (obj1.AuditModifiedUtc != obj2.AuditModifiedUtc)
						return false;
					if (obj1.IdAuditCreatedBy != obj2.IdAuditCreatedBy)
						return false;
					if (obj1.IdAuditModifiedBy != obj2.IdAuditModifiedBy)
						return false;
					if (obj1.ConcurrencyToken != obj2.ConcurrencyToken)
						return false;
					if (obj1.DeletedUtc != obj2.DeletedUtc)
						return false;
				}
			}

			if (cachedHashSet == null)
			{
				cachedHashSet = [];
				cache[obj1] = cachedHashSet;
			}

			cachedHashSet.Add(obj2);

			if ((ComparisonOptions.CompareReferences & comparisonOptions) == ComparisonOptions.CompareReferences)
			{
				if (!ComparisonHelper.SequenceEqual(obj1.ExternalLogins, obj2.ExternalLogins, new ExternalLogin.ExternalLoginEqualityComparer(comparisonOptions, conds?.GetConditions(x => x.ExternalLogins), cache)))
					return false;
				if (!ComparisonHelper.SequenceEqual(obj1.UserPermissions, obj2.UserPermissions, new UserPermission.UserPermissionEqualityComparer(comparisonOptions, conds?.GetConditions(x => x.UserPermissions), cache)))
					return false;
				if (!ComparisonHelper.SequenceEqual(obj1.UserRoles, obj2.UserRoles, new UserRole.UserRoleEqualityComparer(comparisonOptions, conds?.GetConditions(x => x.UserRoles), cache)))
					return false;
				if (!ComparisonHelper.SequenceEqual(obj1.UserTokens, obj2.UserTokens, new UserToken.UserTokenEqualityComparer(comparisonOptions, conds?.GetConditions(x => x.UserTokens), cache)))
					return false;
		}

			return true;
		}

		public static int GetHashCode(
			Auth.Model.User? obj,
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<Auth.Model.User>>? conditions = null,
			HashSet<object>? cache = null)
		{
			if (obj == null)
				return 0;

			cache ??= [];

			if (cache.Contains(obj))
				return 0;

				var hash = 1;
			return hash;
		}

		public ComparisonOptions ComparisonOptions { get; }
		public Action<ComparisonConditions<Auth.Model.User>>? Conditions { get; }
		public Dictionary<object, HashSet<object>>? Cache { get; }
	
		public UserEqualityComparer(
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<Auth.Model.User>>? conditions = null,
			Dictionary<object, HashSet<object>>? cache = null)
		{
			Cache = cache ?? [];
			ComparisonOptions = comparisonOptions;
			Conditions = conditions;
		}
	
		public bool Equals(
			Auth.Model.User? obj1,
			Auth.Model.User? obj2)
			=> EqualsTo(
				obj1,
				obj2,
				ComparisonOptions,
				Conditions,
				Cache);
	
		public int GetHashCode([DisallowNull] Auth.Model.User? obj)
			=> GetHashCode(
				obj,
				ComparisonOptions,
				Conditions,
				cache: null);
	}
}
