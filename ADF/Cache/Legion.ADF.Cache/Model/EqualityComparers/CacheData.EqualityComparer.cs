using Legion;
using Legion.Model.Comparers;
using System.Diagnostics.CodeAnalysis;

namespace Legion.ADF.Cache.Model;

public sealed partial class CacheData : Cache.CacheBaseEntity, Legion.Model.IEntity
{
	public bool EqualsTo(
		Cache.Model.CacheData? obj,
		ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
		Action<ComparisonConditions<Cache.Model.CacheData>>? conditions = null)
		=> CacheDataEqualityComparer.EqualsTo(
			this,
			obj,
			comparisonOptions,
			conditions,
			cache: null);

	public sealed partial class CacheDataEqualityComparer : IEqualityComparer<CacheData>
	{
		public static bool EqualsTo(
			Cache.Model.CacheData? obj1,
			Cache.Model.CacheData? obj2,
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<Cache.Model.CacheData>>? conditions = null,
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
			
			ComparisonConditions<Cache.Model.CacheData>? conds = null;
		
			if ((ComparisonOptions.CompareProperties & comparisonOptions) == ComparisonOptions.CompareProperties)
			{
				if (conditions != null)
				{
					conds = new ComparisonConditions<Cache.Model.CacheData>();
					conditions.Invoke(conds);

					if (conds.CanCompare(obj1, nameof(obj1.KeyHash)) && !string.Equals(obj1.KeyHash, obj2.KeyHash))
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.ValueHash)) && !string.Equals(obj1.ValueHash, obj2.ValueHash))
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.Key)) && !string.Equals(obj1.Key, obj2.Key))
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.Value)) && !string.Equals(obj1.Value, obj2.Value))
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.KeyPrefix450)) && !string.Equals(obj1.KeyPrefix450, obj2.KeyPrefix450))
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.ExpiresUtc)) && obj1.ExpiresUtc != obj2.ExpiresUtc)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.SlidingTime)) && obj1.SlidingTime != obj2.SlidingTime)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.LastAccessedUtc)) && obj1.LastAccessedUtc != obj2.LastAccessedUtc)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.RowVersion)) && obj1.RowVersion != obj2.RowVersion)
						return false;
				}
				else
				{
					if (!string.Equals(obj1.KeyHash, obj2.KeyHash))
						return false;
					if (!string.Equals(obj1.ValueHash, obj2.ValueHash))
						return false;
					if (!string.Equals(obj1.Key, obj2.Key))
						return false;
					if (!string.Equals(obj1.Value, obj2.Value))
						return false;
					if (!string.Equals(obj1.KeyPrefix450, obj2.KeyPrefix450))
						return false;
					if (obj1.ExpiresUtc != obj2.ExpiresUtc)
						return false;
					if (obj1.SlidingTime != obj2.SlidingTime)
						return false;
					if (obj1.LastAccessedUtc != obj2.LastAccessedUtc)
						return false;
					if (obj1.RowVersion != obj2.RowVersion)
						return false;
				}
			}

			if (cachedHashSet == null)
			{
				cachedHashSet = [];
				cache[obj1] = cachedHashSet;
			}

			cachedHashSet.Add(obj2);

			return true;
		}

		public static int GetHashCode(
			Cache.Model.CacheData? obj,
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<Cache.Model.CacheData>>? conditions = null,
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
		public Action<ComparisonConditions<Cache.Model.CacheData>>? Conditions { get; }
		public Dictionary<object, HashSet<object>>? Cache { get; }
	
		public CacheDataEqualityComparer(
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<Cache.Model.CacheData>>? conditions = null,
			Dictionary<object, HashSet<object>>? cache = null)
		{
			Cache = cache ?? [];
			ComparisonOptions = comparisonOptions;
			Conditions = conditions;
		}
	
		public bool Equals(
			Cache.Model.CacheData? obj1,
			Cache.Model.CacheData? obj2)
			=> EqualsTo(
				obj1,
				obj2,
				ComparisonOptions,
				Conditions,
				Cache);
	
		public int GetHashCode([DisallowNull] Cache.Model.CacheData? obj)
			=> GetHashCode(
				obj,
				ComparisonOptions,
				Conditions,
				cache: null);
	}
}
