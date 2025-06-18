using Legion;
using Legion.Model.Comparers;
using System.Diagnostics.CodeAnalysis;

namespace Legion.ADF.Logs.Model;

public sealed partial class EventCounterData : Logs.LogsBaseEntity, Legion.Model.IEntity
{
	public bool EqualsTo(
		Logs.Model.EventCounterData? obj,
		ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
		Action<ComparisonConditions<Logs.Model.EventCounterData>>? conditions = null)
		=> EventCounterDataEqualityComparer.EqualsTo(
			this,
			obj,
			comparisonOptions,
			conditions,
			cache: null);

	public sealed partial class EventCounterDataEqualityComparer : IEqualityComparer<EventCounterData>
	{
		public static bool EqualsTo(
			Logs.Model.EventCounterData? obj1,
			Logs.Model.EventCounterData? obj2,
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<Logs.Model.EventCounterData>>? conditions = null,
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
			
			ComparisonConditions<Logs.Model.EventCounterData>? conds = null;
		
			if ((ComparisonOptions.CompareProperties & comparisonOptions) == ComparisonOptions.CompareProperties)
			{
				if (conditions != null)
				{
					conds = new ComparisonConditions<Logs.Model.EventCounterData>();
					conditions.Invoke(conds);

					if (conds.CanCompare(obj1, nameof(obj1.IdEventCounterData)) && obj1.IdEventCounterData != obj2.IdEventCounterData)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.IdEventCounter)) && obj1.IdEventCounter != obj2.IdEventCounter)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.CreatedUtc)) && obj1.CreatedUtc != obj2.CreatedUtc)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.RuntimeUniqueKey)) && obj1.RuntimeUniqueKey != obj2.RuntimeUniqueKey)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.Increment)) && obj1.Increment != obj2.Increment)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.Mean)) && obj1.Mean != obj2.Mean)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.Count)) && obj1.Count != obj2.Count)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.Min)) && obj1.Min != obj2.Min)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.Max)) && obj1.Max != obj2.Max)
						return false;
				}
				else
				{
					if (obj1.IdEventCounterData != obj2.IdEventCounterData)
						return false;
					if (obj1.IdEventCounter != obj2.IdEventCounter)
						return false;
					if (obj1.CreatedUtc != obj2.CreatedUtc)
						return false;
					if (obj1.RuntimeUniqueKey != obj2.RuntimeUniqueKey)
						return false;
					if (obj1.Increment != obj2.Increment)
						return false;
					if (obj1.Mean != obj2.Mean)
						return false;
					if (obj1.Count != obj2.Count)
						return false;
					if (obj1.Min != obj2.Min)
						return false;
					if (obj1.Max != obj2.Max)
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
				if (!EventCounter.EventCounterEqualityComparer.EqualsTo(obj1.EventCounter, obj2.EventCounter, comparisonOptions, conds?.GetConditions(x => x.EventCounter), cache))
					return false;
		}

			return true;
		}

		public static int GetHashCode(
			Logs.Model.EventCounterData? obj,
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<Logs.Model.EventCounterData>>? conditions = null,
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
		public Action<ComparisonConditions<Logs.Model.EventCounterData>>? Conditions { get; }
		public Dictionary<object, HashSet<object>>? Cache { get; }
	
		public EventCounterDataEqualityComparer(
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<Logs.Model.EventCounterData>>? conditions = null,
			Dictionary<object, HashSet<object>>? cache = null)
		{
			Cache = cache ?? [];
			ComparisonOptions = comparisonOptions;
			Conditions = conditions;
		}
	
		public bool Equals(
			Logs.Model.EventCounterData? obj1,
			Logs.Model.EventCounterData? obj2)
			=> EqualsTo(
				obj1,
				obj2,
				ComparisonOptions,
				Conditions,
				Cache);
	
		public int GetHashCode([DisallowNull] Logs.Model.EventCounterData? obj)
			=> GetHashCode(
				obj,
				ComparisonOptions,
				Conditions,
				cache: null);
	}
}
