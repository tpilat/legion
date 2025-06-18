using Legion;
using Legion.Model.Comparers;
using System.Diagnostics.CodeAnalysis;

namespace Legion.ADF.Auth.Model;

public sealed partial class UserRole : Auth.AuthBaseEntity, Legion.Model.Audit.ISelfAuditableEntity, Legion.Model.Concurrence.IConcurrent, Legion.Model.Audit.IAuditableEntity, Legion.Model.IEntity
{
	public bool EqualsTo(
		Auth.Model.UserRole? obj,
		ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
		Action<ComparisonConditions<Auth.Model.UserRole>>? conditions = null)
		=> UserRoleEqualityComparer.EqualsTo(
			this,
			obj,
			comparisonOptions,
			conditions,
			cache: null);

	public sealed partial class UserRoleEqualityComparer : IEqualityComparer<UserRole>
	{
		public static bool EqualsTo(
			Auth.Model.UserRole? obj1,
			Auth.Model.UserRole? obj2,
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<Auth.Model.UserRole>>? conditions = null,
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
			
			ComparisonConditions<Auth.Model.UserRole>? conds = null;
		
			if ((ComparisonOptions.CompareProperties & comparisonOptions) == ComparisonOptions.CompareProperties)
			{
				if (conditions != null)
				{
					conds = new ComparisonConditions<Auth.Model.UserRole>();
					conditions.Invoke(conds);

					if (conds.CanCompare(obj1, nameof(obj1.IdUserRole)) && obj1.IdUserRole != obj2.IdUserRole)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.IdUser)) && obj1.IdUser != obj2.IdUser)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.IdRole)) && obj1.IdRole != obj2.IdRole)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.TenantIdentifier)) && obj1.TenantIdentifier != obj2.TenantIdentifier)
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
					if (obj1.IdUserRole != obj2.IdUserRole)
						return false;
					if (obj1.IdUser != obj2.IdUser)
						return false;
					if (obj1.IdRole != obj2.IdRole)
						return false;
					if (obj1.TenantIdentifier != obj2.TenantIdentifier)
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
				if (!Role.RoleEqualityComparer.EqualsTo(obj1.Role, obj2.Role, comparisonOptions, conds?.GetConditions(x => x.Role), cache))
					return false;
				if (!User.UserEqualityComparer.EqualsTo(obj1.User, obj2.User, comparisonOptions, conds?.GetConditions(x => x.User), cache))
					return false;
		}

			return true;
		}

		public static int GetHashCode(
			Auth.Model.UserRole? obj,
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<Auth.Model.UserRole>>? conditions = null,
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
		public Action<ComparisonConditions<Auth.Model.UserRole>>? Conditions { get; }
		public Dictionary<object, HashSet<object>>? Cache { get; }
	
		public UserRoleEqualityComparer(
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<Auth.Model.UserRole>>? conditions = null,
			Dictionary<object, HashSet<object>>? cache = null)
		{
			Cache = cache ?? [];
			ComparisonOptions = comparisonOptions;
			Conditions = conditions;
		}
	
		public bool Equals(
			Auth.Model.UserRole? obj1,
			Auth.Model.UserRole? obj2)
			=> EqualsTo(
				obj1,
				obj2,
				ComparisonOptions,
				Conditions,
				Cache);
	
		public int GetHashCode([DisallowNull] Auth.Model.UserRole? obj)
			=> GetHashCode(
				obj,
				ComparisonOptions,
				Conditions,
				cache: null);
	}
}
