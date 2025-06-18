using Legion;
using Legion.Model.Comparers;
using System.Diagnostics.CodeAnalysis;

namespace Legion.ADF.Logs.Model;

public sealed partial class EventCounterCategory : Logs.LogsBaseEntity, Legion.Model.IEntity
{
	public bool EqualsTo(
		Logs.Model.EventCounterCategory? obj,
		ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
		Action<ComparisonConditions<Logs.Model.EventCounterCategory>>? conditions = null)
		=> EventCounterCategoryEqualityComparer.EqualsTo(
			this,
			obj,
			comparisonOptions,
			conditions,
			cache: null);

	public sealed partial class EventCounterCategoryEqualityComparer : IEqualityComparer<EventCounterCategory>
	{
		public static bool EqualsTo(
			Logs.Model.EventCounterCategory? obj1,
			Logs.Model.EventCounterCategory? obj2,
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<Logs.Model.EventCounterCategory>>? conditions = null,
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
			
			ComparisonConditions<Logs.Model.EventCounterCategory>? conds = null;
		
			if ((ComparisonOptions.CompareProperties & comparisonOptions) == ComparisonOptions.CompareProperties)
			{
				if (conditions != null)
				{
					conds = new ComparisonConditions<Logs.Model.EventCounterCategory>();
					conditions.Invoke(conds);

					if (conds.CanCompare(obj1, nameof(obj1.IdEventCounterCategory)) && obj1.IdEventCounterCategory != obj2.IdEventCounterCategory)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.Source)) && !string.Equals(obj1.Source, obj2.Source))
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.DisplayName)) && !string.Equals(obj1.DisplayName, obj2.DisplayName))
						return false;
				}
				else
				{
					if (obj1.IdEventCounterCategory != obj2.IdEventCounterCategory)
						return false;
					if (!string.Equals(obj1.Source, obj2.Source))
						return false;
					if (!string.Equals(obj1.DisplayName, obj2.DisplayName))
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
				if (!ComparisonHelper.SequenceEqual(obj1.EventCounters, obj2.EventCounters, new EventCounter.EventCounterEqualityComparer(comparisonOptions, conds?.GetConditions(x => x.EventCounters), cache)))
					return false;
		}

			return true;
		}

		public static int GetHashCode(
			Logs.Model.EventCounterCategory? obj,
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<Logs.Model.EventCounterCategory>>? conditions = null,
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
		public Action<ComparisonConditions<Logs.Model.EventCounterCategory>>? Conditions { get; }
		public Dictionary<object, HashSet<object>>? Cache { get; }
	
		public EventCounterCategoryEqualityComparer(
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<Logs.Model.EventCounterCategory>>? conditions = null,
			Dictionary<object, HashSet<object>>? cache = null)
		{
			Cache = cache ?? [];
			ComparisonOptions = comparisonOptions;
			Conditions = conditions;
		}
	
		public bool Equals(
			Logs.Model.EventCounterCategory? obj1,
			Logs.Model.EventCounterCategory? obj2)
			=> EqualsTo(
				obj1,
				obj2,
				ComparisonOptions,
				Conditions,
				Cache);
	
		public int GetHashCode([DisallowNull] Logs.Model.EventCounterCategory? obj)
			=> GetHashCode(
				obj,
				ComparisonOptions,
				Conditions,
				cache: null);
	}
}
