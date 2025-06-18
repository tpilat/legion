using Legion;
using Legion.Model.Comparers;
using System.Diagnostics.CodeAnalysis;

namespace Legion.ADF.Auth.Model;

public sealed partial class LoginProvider : Auth.AuthBaseEntity, Legion.Model.IEntity
{
	public bool EqualsTo(
		Auth.Model.LoginProvider? obj,
		ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
		Action<ComparisonConditions<Auth.Model.LoginProvider>>? conditions = null)
		=> LoginProviderEqualityComparer.EqualsTo(
			this,
			obj,
			comparisonOptions,
			conditions,
			cache: null);

	public sealed partial class LoginProviderEqualityComparer : IEqualityComparer<LoginProvider>
	{
		public static bool EqualsTo(
			Auth.Model.LoginProvider? obj1,
			Auth.Model.LoginProvider? obj2,
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<Auth.Model.LoginProvider>>? conditions = null,
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
			
			ComparisonConditions<Auth.Model.LoginProvider>? conds = null;
		
			if ((ComparisonOptions.CompareProperties & comparisonOptions) == ComparisonOptions.CompareProperties)
			{
				if (conditions != null)
				{
					conds = new ComparisonConditions<Auth.Model.LoginProvider>();
					conditions.Invoke(conds);

					if (conds.CanCompare(obj1, nameof(obj1.IdLoginProvider)) && obj1.IdLoginProvider != obj2.IdLoginProvider)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.Code)) && !string.Equals(obj1.Code, obj2.Code))
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.Name)) && !string.Equals(obj1.Name, obj2.Name))
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.DisabledUtc)) && obj1.DisabledUtc != obj2.DisabledUtc)
						return false;
				}
				else
				{
					if (obj1.IdLoginProvider != obj2.IdLoginProvider)
						return false;
					if (!string.Equals(obj1.Code, obj2.Code))
						return false;
					if (!string.Equals(obj1.Name, obj2.Name))
						return false;
					if (obj1.DisabledUtc != obj2.DisabledUtc)
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
				if (!ComparisonHelper.SequenceEqual(obj1.UserTokens, obj2.UserTokens, new UserToken.UserTokenEqualityComparer(comparisonOptions, conds?.GetConditions(x => x.UserTokens), cache)))
					return false;
		}

			return true;
		}

		public static int GetHashCode(
			Auth.Model.LoginProvider? obj,
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<Auth.Model.LoginProvider>>? conditions = null,
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
		public Action<ComparisonConditions<Auth.Model.LoginProvider>>? Conditions { get; }
		public Dictionary<object, HashSet<object>>? Cache { get; }
	
		public LoginProviderEqualityComparer(
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<Auth.Model.LoginProvider>>? conditions = null,
			Dictionary<object, HashSet<object>>? cache = null)
		{
			Cache = cache ?? [];
			ComparisonOptions = comparisonOptions;
			Conditions = conditions;
		}
	
		public bool Equals(
			Auth.Model.LoginProvider? obj1,
			Auth.Model.LoginProvider? obj2)
			=> EqualsTo(
				obj1,
				obj2,
				ComparisonOptions,
				Conditions,
				Cache);
	
		public int GetHashCode([DisallowNull] Auth.Model.LoginProvider? obj)
			=> GetHashCode(
				obj,
				ComparisonOptions,
				Conditions,
				cache: null);
	}
}
