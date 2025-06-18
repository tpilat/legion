using Legion;
using Legion.Model.Comparers;
using System.Diagnostics.CodeAnalysis;

namespace Legion.ADF.Cache.Model;

public sealed partial class ReloadableCacheKey : Cache.CacheBaseEntity, Legion.Model.IEntity
{
	public bool EqualsTo(
		Cache.Model.ReloadableCacheKey? obj,
		ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
		Action<ComparisonConditions<Cache.Model.ReloadableCacheKey>>? conditions = null)
		=> ReloadableCacheKeyEqualityComparer.EqualsTo(
			this,
			obj,
			comparisonOptions,
			conditions,
			cache: null);

	public sealed partial class ReloadableCacheKeyEqualityComparer : IEqualityComparer<ReloadableCacheKey>
	{
		public static bool EqualsTo(
			Cache.Model.ReloadableCacheKey? obj1,
			Cache.Model.ReloadableCacheKey? obj2,
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<Cache.Model.ReloadableCacheKey>>? conditions = null,
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
			
			ComparisonConditions<Cache.Model.ReloadableCacheKey>? conds = null;
		
			if ((ComparisonOptions.CompareProperties & comparisonOptions) == ComparisonOptions.CompareProperties)
			{
				if (conditions != null)
				{
					conds = new ComparisonConditions<Cache.Model.ReloadableCacheKey>();
					conditions.Invoke(conds);

					if (conds.CanCompare(obj1, nameof(obj1.IdReloadableCacheKey)) && obj1.IdReloadableCacheKey != obj2.IdReloadableCacheKey)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.Key)) && !string.Equals(obj1.Key, obj2.Key))
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.Tags)) && (obj1.Tags != null || obj2.Tags != null) && (obj1.Tags == null || obj2.Tags == null || !obj1.Tags.SequenceEqual(obj2.Tags)))
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.CreatedUtc)) && obj1.CreatedUtc != obj2.CreatedUtc)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.ReloadAtUtc)) && obj1.ReloadAtUtc != obj2.ReloadAtUtc)
						return false;
				}
				else
				{
					if (obj1.IdReloadableCacheKey != obj2.IdReloadableCacheKey)
						return false;
					if (!string.Equals(obj1.Key, obj2.Key))
						return false;
					if ((obj1.Tags != null || obj2.Tags != null) && (obj1.Tags == null || obj2.Tags == null || !obj1.Tags.SequenceEqual(obj2.Tags)))
						return false;
					if (obj1.CreatedUtc != obj2.CreatedUtc)
						return false;
					if (obj1.ReloadAtUtc != obj2.ReloadAtUtc)
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
			Cache.Model.ReloadableCacheKey? obj,
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<Cache.Model.ReloadableCacheKey>>? conditions = null,
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
		public Action<ComparisonConditions<Cache.Model.ReloadableCacheKey>>? Conditions { get; }
		public Dictionary<object, HashSet<object>>? Cache { get; }
	
		public ReloadableCacheKeyEqualityComparer(
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<Cache.Model.ReloadableCacheKey>>? conditions = null,
			Dictionary<object, HashSet<object>>? cache = null)
		{
			Cache = cache ?? [];
			ComparisonOptions = comparisonOptions;
			Conditions = conditions;
		}
	
		public bool Equals(
			Cache.Model.ReloadableCacheKey? obj1,
			Cache.Model.ReloadableCacheKey? obj2)
			=> EqualsTo(
				obj1,
				obj2,
				ComparisonOptions,
				Conditions,
				Cache);
	
		public int GetHashCode([DisallowNull] Cache.Model.ReloadableCacheKey? obj)
			=> GetHashCode(
				obj,
				ComparisonOptions,
				Conditions,
				cache: null);
	}
}
