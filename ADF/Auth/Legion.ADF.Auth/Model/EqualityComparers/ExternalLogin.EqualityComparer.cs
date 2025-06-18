using Legion;
using Legion.Model.Comparers;
using System.Diagnostics.CodeAnalysis;

namespace Legion.ADF.Auth.Model;

public sealed partial class ExternalLogin : Auth.AuthBaseEntity, Legion.Model.IEntity
{
	public bool EqualsTo(
		Auth.Model.ExternalLogin? obj,
		ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
		Action<ComparisonConditions<Auth.Model.ExternalLogin>>? conditions = null)
		=> ExternalLoginEqualityComparer.EqualsTo(
			this,
			obj,
			comparisonOptions,
			conditions,
			cache: null);

	public sealed partial class ExternalLoginEqualityComparer : IEqualityComparer<ExternalLogin>
	{
		public static bool EqualsTo(
			Auth.Model.ExternalLogin? obj1,
			Auth.Model.ExternalLogin? obj2,
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<Auth.Model.ExternalLogin>>? conditions = null,
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
			
			ComparisonConditions<Auth.Model.ExternalLogin>? conds = null;
		
			if ((ComparisonOptions.CompareProperties & comparisonOptions) == ComparisonOptions.CompareProperties)
			{
				if (conditions != null)
				{
					conds = new ComparisonConditions<Auth.Model.ExternalLogin>();
					conditions.Invoke(conds);

					if (conds.CanCompare(obj1, nameof(obj1.IdExternalLogin)) && obj1.IdExternalLogin != obj2.IdExternalLogin)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.IdLoginProvider)) && obj1.IdLoginProvider != obj2.IdLoginProvider)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.IdUser)) && obj1.IdUser != obj2.IdUser)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.ExternalUserIdentifier)) && !string.Equals(obj1.ExternalUserIdentifier, obj2.ExternalUserIdentifier))
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.Data)) && !string.Equals(obj1.Data, obj2.Data))
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.CreatedUtc)) && obj1.CreatedUtc != obj2.CreatedUtc)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.ValidToUtc)) && obj1.ValidToUtc != obj2.ValidToUtc)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.LastAccessUtc)) && obj1.LastAccessUtc != obj2.LastAccessUtc)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.RemoteIP)) && !string.Equals(obj1.RemoteIP, obj2.RemoteIP))
						return false;
				}
				else
				{
					if (obj1.IdExternalLogin != obj2.IdExternalLogin)
						return false;
					if (obj1.IdLoginProvider != obj2.IdLoginProvider)
						return false;
					if (obj1.IdUser != obj2.IdUser)
						return false;
					if (!string.Equals(obj1.ExternalUserIdentifier, obj2.ExternalUserIdentifier))
						return false;
					if (!string.Equals(obj1.Data, obj2.Data))
						return false;
					if (obj1.CreatedUtc != obj2.CreatedUtc)
						return false;
					if (obj1.ValidToUtc != obj2.ValidToUtc)
						return false;
					if (obj1.LastAccessUtc != obj2.LastAccessUtc)
						return false;
					if (!string.Equals(obj1.RemoteIP, obj2.RemoteIP))
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
				if (!LoginProvider.LoginProviderEqualityComparer.EqualsTo(obj1.LoginProvider, obj2.LoginProvider, comparisonOptions, conds?.GetConditions(x => x.LoginProvider), cache))
					return false;
				if (!User.UserEqualityComparer.EqualsTo(obj1.User, obj2.User, comparisonOptions, conds?.GetConditions(x => x.User), cache))
					return false;
		}

			return true;
		}

		public static int GetHashCode(
			Auth.Model.ExternalLogin? obj,
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<Auth.Model.ExternalLogin>>? conditions = null,
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
		public Action<ComparisonConditions<Auth.Model.ExternalLogin>>? Conditions { get; }
		public Dictionary<object, HashSet<object>>? Cache { get; }
	
		public ExternalLoginEqualityComparer(
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<Auth.Model.ExternalLogin>>? conditions = null,
			Dictionary<object, HashSet<object>>? cache = null)
		{
			Cache = cache ?? [];
			ComparisonOptions = comparisonOptions;
			Conditions = conditions;
		}
	
		public bool Equals(
			Auth.Model.ExternalLogin? obj1,
			Auth.Model.ExternalLogin? obj2)
			=> EqualsTo(
				obj1,
				obj2,
				ComparisonOptions,
				Conditions,
				Cache);
	
		public int GetHashCode([DisallowNull] Auth.Model.ExternalLogin? obj)
			=> GetHashCode(
				obj,
				ComparisonOptions,
				Conditions,
				cache: null);
	}
}
