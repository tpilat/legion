using Legion;
using Legion.Model.Comparers;
using System.Diagnostics.CodeAnalysis;

namespace Legion.ADF.Logs.Model;

public sealed partial class EventCounter : Logs.LogsBaseEntity, Legion.Model.IEntity
{
	public bool EqualsTo(
		Logs.Model.EventCounter? obj,
		ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
		Action<ComparisonConditions<Logs.Model.EventCounter>>? conditions = null)
		=> EventCounterEqualityComparer.EqualsTo(
			this,
			obj,
			comparisonOptions,
			conditions,
			cache: null);

	public sealed partial class EventCounterEqualityComparer : IEqualityComparer<EventCounter>
	{
		public static bool EqualsTo(
			Logs.Model.EventCounter? obj1,
			Logs.Model.EventCounter? obj2,
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<Logs.Model.EventCounter>>? conditions = null,
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
			
			ComparisonConditions<Logs.Model.EventCounter>? conds = null;
		
			if ((ComparisonOptions.CompareProperties & comparisonOptions) == ComparisonOptions.CompareProperties)
			{
				if (conditions != null)
				{
					conds = new ComparisonConditions<Logs.Model.EventCounter>();
					conditions.Invoke(conds);

					if (conds.CanCompare(obj1, nameof(obj1.IdEventCounter)) && obj1.IdEventCounter != obj2.IdEventCounter)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.IdEventCounterCategory)) && obj1.IdEventCounterCategory != obj2.IdEventCounterCategory)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.Code)) && !string.Equals(obj1.Code, obj2.Code))
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.Name)) && !string.Equals(obj1.Name, obj2.Name))
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.DisplayName)) && !string.Equals(obj1.DisplayName, obj2.DisplayName))
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.CounterType)) && !string.Equals(obj1.CounterType, obj2.CounterType))
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.DisplayRateTimeScale)) && !string.Equals(obj1.DisplayRateTimeScale, obj2.DisplayRateTimeScale))
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.Metadata)) && !string.Equals(obj1.Metadata, obj2.Metadata))
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.DisplayUnits)) && !string.Equals(obj1.DisplayUnits, obj2.DisplayUnits))
						return false;
				}
				else
				{
					if (obj1.IdEventCounter != obj2.IdEventCounter)
						return false;
					if (obj1.IdEventCounterCategory != obj2.IdEventCounterCategory)
						return false;
					if (!string.Equals(obj1.Code, obj2.Code))
						return false;
					if (!string.Equals(obj1.Name, obj2.Name))
						return false;
					if (!string.Equals(obj1.DisplayName, obj2.DisplayName))
						return false;
					if (!string.Equals(obj1.CounterType, obj2.CounterType))
						return false;
					if (!string.Equals(obj1.DisplayRateTimeScale, obj2.DisplayRateTimeScale))
						return false;
					if (!string.Equals(obj1.Metadata, obj2.Metadata))
						return false;
					if (!string.Equals(obj1.DisplayUnits, obj2.DisplayUnits))
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
				if (!EventCounterCategory.EventCounterCategoryEqualityComparer.EqualsTo(obj1.EventCounterCategory, obj2.EventCounterCategory, comparisonOptions, conds?.GetConditions(x => x.EventCounterCategory), cache))
					return false;
				if (!ComparisonHelper.SequenceEqual(obj1.EventCounterDatas, obj2.EventCounterDatas, new EventCounterData.EventCounterDataEqualityComparer(comparisonOptions, conds?.GetConditions(x => x.EventCounterDatas), cache)))
					return false;
		}

			return true;
		}

		public static int GetHashCode(
			Logs.Model.EventCounter? obj,
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<Logs.Model.EventCounter>>? conditions = null,
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
		public Action<ComparisonConditions<Logs.Model.EventCounter>>? Conditions { get; }
		public Dictionary<object, HashSet<object>>? Cache { get; }
	
		public EventCounterEqualityComparer(
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<Logs.Model.EventCounter>>? conditions = null,
			Dictionary<object, HashSet<object>>? cache = null)
		{
			Cache = cache ?? [];
			ComparisonOptions = comparisonOptions;
			Conditions = conditions;
		}
	
		public bool Equals(
			Logs.Model.EventCounter? obj1,
			Logs.Model.EventCounter? obj2)
			=> EqualsTo(
				obj1,
				obj2,
				ComparisonOptions,
				Conditions,
				Cache);
	
		public int GetHashCode([DisallowNull] Logs.Model.EventCounter? obj)
			=> GetHashCode(
				obj,
				ComparisonOptions,
				Conditions,
				cache: null);
	}
}
