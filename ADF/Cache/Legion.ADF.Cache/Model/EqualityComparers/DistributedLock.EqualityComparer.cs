using Legion;
using Legion.Model.Comparers;
using System.Diagnostics.CodeAnalysis;

namespace Legion.ADF.Cache.Model;

public sealed partial class DistributedLock : Cache.CacheBaseEntity, Legion.Model.IEntity
{
	public bool EqualsTo(
		Cache.Model.DistributedLock? obj,
		ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
		Action<ComparisonConditions<Cache.Model.DistributedLock>>? conditions = null)
		=> DistributedLockEqualityComparer.EqualsTo(
			this,
			obj,
			comparisonOptions,
			conditions,
			cache: null);

	public sealed partial class DistributedLockEqualityComparer : IEqualityComparer<DistributedLock>
	{
		public static bool EqualsTo(
			Cache.Model.DistributedLock? obj1,
			Cache.Model.DistributedLock? obj2,
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<Cache.Model.DistributedLock>>? conditions = null,
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
			
			ComparisonConditions<Cache.Model.DistributedLock>? conds = null;
		
			if ((ComparisonOptions.CompareProperties & comparisonOptions) == ComparisonOptions.CompareProperties)
			{
				if (conditions != null)
				{
					conds = new ComparisonConditions<Cache.Model.DistributedLock>();
					conditions.Invoke(conds);

					if (conds.CanCompare(obj1, nameof(obj1.KeyHash)) && !string.Equals(obj1.KeyHash, obj2.KeyHash))
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.LockKey)) && !string.Equals(obj1.LockKey, obj2.LockKey))
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.LockId)) && !string.Equals(obj1.LockId, obj2.LockId))
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.Metadata)) && !string.Equals(obj1.Metadata, obj2.Metadata))
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.CreatedUtc)) && obj1.CreatedUtc != obj2.CreatedUtc)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.ExpiresUtc)) && obj1.ExpiresUtc != obj2.ExpiresUtc)
						return false;
				}
				else
				{
					if (!string.Equals(obj1.KeyHash, obj2.KeyHash))
						return false;
					if (!string.Equals(obj1.LockKey, obj2.LockKey))
						return false;
					if (!string.Equals(obj1.LockId, obj2.LockId))
						return false;
					if (!string.Equals(obj1.Metadata, obj2.Metadata))
						return false;
					if (obj1.CreatedUtc != obj2.CreatedUtc)
						return false;
					if (obj1.ExpiresUtc != obj2.ExpiresUtc)
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
			Cache.Model.DistributedLock? obj,
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<Cache.Model.DistributedLock>>? conditions = null,
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
		public Action<ComparisonConditions<Cache.Model.DistributedLock>>? Conditions { get; }
		public Dictionary<object, HashSet<object>>? Cache { get; }
	
		public DistributedLockEqualityComparer(
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<Cache.Model.DistributedLock>>? conditions = null,
			Dictionary<object, HashSet<object>>? cache = null)
		{
			Cache = cache ?? [];
			ComparisonOptions = comparisonOptions;
			Conditions = conditions;
		}
	
		public bool Equals(
			Cache.Model.DistributedLock? obj1,
			Cache.Model.DistributedLock? obj2)
			=> EqualsTo(
				obj1,
				obj2,
				ComparisonOptions,
				Conditions,
				Cache);
	
		public int GetHashCode([DisallowNull] Cache.Model.DistributedLock? obj)
			=> GetHashCode(
				obj,
				ComparisonOptions,
				Conditions,
				cache: null);
	}
}
