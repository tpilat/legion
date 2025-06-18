using Legion;
using Legion.Model.Comparers;
using System.Diagnostics.CodeAnalysis;

namespace Legion.ADF.Auth.Model;

public sealed partial class Permission : Auth.AuthBaseEntity, Legion.Model.IEntity
{
	public bool EqualsTo(
		Auth.Model.Permission? obj,
		ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
		Action<ComparisonConditions<Auth.Model.Permission>>? conditions = null)
		=> PermissionEqualityComparer.EqualsTo(
			this,
			obj,
			comparisonOptions,
			conditions,
			cache: null);

	public sealed partial class PermissionEqualityComparer : IEqualityComparer<Permission>
	{
		public static bool EqualsTo(
			Auth.Model.Permission? obj1,
			Auth.Model.Permission? obj2,
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<Auth.Model.Permission>>? conditions = null,
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
			
			ComparisonConditions<Auth.Model.Permission>? conds = null;
		
			if ((ComparisonOptions.CompareProperties & comparisonOptions) == ComparisonOptions.CompareProperties)
			{
				if (conditions != null)
				{
					conds = new ComparisonConditions<Auth.Model.Permission>();
					conditions.Invoke(conds);

					if (conds.CanCompare(obj1, nameof(obj1.IdPermission)) && obj1.IdPermission != obj2.IdPermission)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.Code)) && !string.Equals(obj1.Code, obj2.Code))
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.Name)) && !string.Equals(obj1.Name, obj2.Name))
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.Description)) && !string.Equals(obj1.Description, obj2.Description))
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.ClaimValue)) && !string.Equals(obj1.ClaimValue, obj2.ClaimValue))
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.IsSystemPermission)) && obj1.IsSystemPermission != obj2.IsSystemPermission)
						return false;
				}
				else
				{
					if (obj1.IdPermission != obj2.IdPermission)
						return false;
					if (!string.Equals(obj1.Code, obj2.Code))
						return false;
					if (!string.Equals(obj1.Name, obj2.Name))
						return false;
					if (!string.Equals(obj1.Description, obj2.Description))
						return false;
					if (!string.Equals(obj1.ClaimValue, obj2.ClaimValue))
						return false;
					if (obj1.IsSystemPermission != obj2.IsSystemPermission)
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
				if (!ComparisonHelper.SequenceEqual(obj1.UserPermissions, obj2.UserPermissions, new UserPermission.UserPermissionEqualityComparer(comparisonOptions, conds?.GetConditions(x => x.UserPermissions), cache)))
					return false;
		}

			return true;
		}

		public static int GetHashCode(
			Auth.Model.Permission? obj,
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<Auth.Model.Permission>>? conditions = null,
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
		public Action<ComparisonConditions<Auth.Model.Permission>>? Conditions { get; }
		public Dictionary<object, HashSet<object>>? Cache { get; }
	
		public PermissionEqualityComparer(
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<Auth.Model.Permission>>? conditions = null,
			Dictionary<object, HashSet<object>>? cache = null)
		{
			Cache = cache ?? [];
			ComparisonOptions = comparisonOptions;
			Conditions = conditions;
		}
	
		public bool Equals(
			Auth.Model.Permission? obj1,
			Auth.Model.Permission? obj2)
			=> EqualsTo(
				obj1,
				obj2,
				ComparisonOptions,
				Conditions,
				Cache);
	
		public int GetHashCode([DisallowNull] Auth.Model.Permission? obj)
			=> GetHashCode(
				obj,
				ComparisonOptions,
				Conditions,
				cache: null);
	}
}
