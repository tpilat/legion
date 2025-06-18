using Legion;
using Legion.Model.Comparers;
using System.Diagnostics.CodeAnalysis;

namespace Legion.ADF.Messaging.DomainEvents.Model;

public sealed partial class DomainEventProcessingStatus : DomainEvents.DomainEventsBaseEntity, Legion.Model.IEntity
{
	public bool EqualsTo(
		DomainEvents.Model.DomainEventProcessingStatus? obj,
		ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
		Action<ComparisonConditions<DomainEvents.Model.DomainEventProcessingStatus>>? conditions = null)
		=> DomainEventProcessingStatusEqualityComparer.EqualsTo(
			this,
			obj,
			comparisonOptions,
			conditions,
			cache: null);

	public sealed partial class DomainEventProcessingStatusEqualityComparer : IEqualityComparer<DomainEventProcessingStatus>
	{
		public static bool EqualsTo(
			DomainEvents.Model.DomainEventProcessingStatus? obj1,
			DomainEvents.Model.DomainEventProcessingStatus? obj2,
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<DomainEvents.Model.DomainEventProcessingStatus>>? conditions = null,
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
			
			ComparisonConditions<DomainEvents.Model.DomainEventProcessingStatus>? conds = null;
		
			if ((ComparisonOptions.CompareProperties & comparisonOptions) == ComparisonOptions.CompareProperties)
			{
				if (conditions != null)
				{
					conds = new ComparisonConditions<DomainEvents.Model.DomainEventProcessingStatus>();
					conditions.Invoke(conds);
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
				if (!ComparisonHelper.SequenceEqual(obj1.DomainEventProcessingLogs, obj2.DomainEventProcessingLogs, new DomainEventProcessingLog.DomainEventProcessingLogEqualityComparer(comparisonOptions, conds?.GetConditions(x => x.DomainEventProcessingLogs), cache)))
					return false;
				if (!ComparisonHelper.SequenceEqual(obj1.DomainEvents, obj2.DomainEvents, new DomainEvent.DomainEventEqualityComparer(comparisonOptions, conds?.GetConditions(x => x.DomainEvents), cache)))
					return false;
		}

			return true;
		}

		public static int GetHashCode(
			DomainEvents.Model.DomainEventProcessingStatus? obj,
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<DomainEvents.Model.DomainEventProcessingStatus>>? conditions = null,
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
		public Action<ComparisonConditions<DomainEvents.Model.DomainEventProcessingStatus>>? Conditions { get; }
		public Dictionary<object, HashSet<object>>? Cache { get; }
	
		public DomainEventProcessingStatusEqualityComparer(
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<DomainEvents.Model.DomainEventProcessingStatus>>? conditions = null,
			Dictionary<object, HashSet<object>>? cache = null)
		{
			Cache = cache ?? [];
			ComparisonOptions = comparisonOptions;
			Conditions = conditions;
		}
	
		public bool Equals(
			DomainEvents.Model.DomainEventProcessingStatus? obj1,
			DomainEvents.Model.DomainEventProcessingStatus? obj2)
			=> EqualsTo(
				obj1,
				obj2,
				ComparisonOptions,
				Conditions,
				Cache);
	
		public int GetHashCode([DisallowNull] DomainEvents.Model.DomainEventProcessingStatus? obj)
			=> GetHashCode(
				obj,
				ComparisonOptions,
				Conditions,
				cache: null);
	}
}
