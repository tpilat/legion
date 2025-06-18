using Legion;
using Legion.Model.Comparers;
using System.Diagnostics.CodeAnalysis;

namespace Legion.ADF.Auth.Model;

public partial class Role : Auth.AuthBaseEntity, Legion.Model.Audit.ISelfAuditableEntity, Legion.Model.Concurrence.IConcurrent, Legion.Model.Audit.IAuditableEntity, Legion.Model.IEntity
{
	public bool EqualsTo(
		Auth.Model.Role? obj,
		ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
		Action<ComparisonConditions<Auth.Model.Role>>? conditions = null)
		=> RoleEqualityComparer.EqualsTo(
			this,
			obj,
			comparisonOptions,
			conditions,
			cache: null);

	public partial class RoleEqualityComparer : IEqualityComparer<Role>
	{
		public static bool EqualsTo(
			Auth.Model.Role? obj1,
			Auth.Model.Role? obj2,
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<Auth.Model.Role>>? conditions = null,
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
			
			ComparisonConditions<Auth.Model.Role>? conds = null;
		
			if ((ComparisonOptions.CompareProperties & comparisonOptions) == ComparisonOptions.CompareProperties)
			{
				if (conditions != null)
				{
					conds = new ComparisonConditions<Auth.Model.Role>();
					conditions.Invoke(conds);

					if (conds.CanCompare(obj1, nameof(obj1.IdRole)) && obj1.IdRole != obj2.IdRole)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.Name)) && !string.Equals(obj1.Name, obj2.Name))
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.NormalizedName)) && !string.Equals(obj1.NormalizedName, obj2.NormalizedName))
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.ADGroupDistinguishedName)) && !string.Equals(obj1.ADGroupDistinguishedName, obj2.ADGroupDistinguishedName))
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.Data)) && !string.Equals(obj1.Data, obj2.Data))
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.Description)) && !string.Equals(obj1.Description, obj2.Description))
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.HasConstantPermissions)) && obj1.HasConstantPermissions != obj2.HasConstantPermissions)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.HasConstantUsers)) && obj1.HasConstantUsers != obj2.HasConstantUsers)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.IsSystemRole)) && obj1.IsSystemRole != obj2.IsSystemRole)
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
					if (obj1.IdRole != obj2.IdRole)
						return false;
					if (!string.Equals(obj1.Name, obj2.Name))
						return false;
					if (!string.Equals(obj1.NormalizedName, obj2.NormalizedName))
						return false;
					if (!string.Equals(obj1.ADGroupDistinguishedName, obj2.ADGroupDistinguishedName))
						return false;
					if (!string.Equals(obj1.Data, obj2.Data))
						return false;
					if (!string.Equals(obj1.Description, obj2.Description))
						return false;
					if (obj1.HasConstantPermissions != obj2.HasConstantPermissions)
						return false;
					if (obj1.HasConstantUsers != obj2.HasConstantUsers)
						return false;
					if (obj1.IsSystemRole != obj2.IsSystemRole)
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
				if (!ComparisonHelper.SequenceEqual(obj1.RolePermissions, obj2.RolePermissions, new RolePermission.RolePermissionEqualityComparer(comparisonOptions, conds?.GetConditions(x => x.RolePermissions), cache)))
					return false;
				if (!ComparisonHelper.SequenceEqual(obj1.UserRoles, obj2.UserRoles, new UserRole.UserRoleEqualityComparer(comparisonOptions, conds?.GetConditions(x => x.UserRoles), cache)))
					return false;
		}

			return true;
		}

		public static int GetHashCode(
			Auth.Model.Role? obj,
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<Auth.Model.Role>>? conditions = null,
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
		public Action<ComparisonConditions<Auth.Model.Role>>? Conditions { get; }
		public Dictionary<object, HashSet<object>>? Cache { get; }
	
		public RoleEqualityComparer(
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<Auth.Model.Role>>? conditions = null,
			Dictionary<object, HashSet<object>>? cache = null)
		{
			Cache = cache ?? [];
			ComparisonOptions = comparisonOptions;
			Conditions = conditions;
		}
	
		public bool Equals(
			Auth.Model.Role? obj1,
			Auth.Model.Role? obj2)
			=> EqualsTo(
				obj1,
				obj2,
				ComparisonOptions,
				Conditions,
				Cache);
	
		public int GetHashCode([DisallowNull] Auth.Model.Role? obj)
			=> GetHashCode(
				obj,
				ComparisonOptions,
				Conditions,
				cache: null);
	}
}
