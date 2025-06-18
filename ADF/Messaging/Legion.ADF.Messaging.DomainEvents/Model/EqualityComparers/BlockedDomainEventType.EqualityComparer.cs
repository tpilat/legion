using Legion;
using Legion.Model.Comparers;
using System.Diagnostics.CodeAnalysis;

namespace Legion.ADF.Messaging.DomainEvents.Model;

public sealed partial class BlockedDomainEventType : DomainEvents.DomainEventsBaseEntity, Legion.Model.IEntity
{
	public bool EqualsTo(
		DomainEvents.Model.BlockedDomainEventType? obj,
		ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
		Action<ComparisonConditions<DomainEvents.Model.BlockedDomainEventType>>? conditions = null)
		=> BlockedDomainEventTypeEqualityComparer.EqualsTo(
			this,
			obj,
			comparisonOptions,
			conditions,
			cache: null);

	public sealed partial class BlockedDomainEventTypeEqualityComparer : IEqualityComparer<BlockedDomainEventType>
	{
		public static bool EqualsTo(
			DomainEvents.Model.BlockedDomainEventType? obj1,
			DomainEvents.Model.BlockedDomainEventType? obj2,
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<DomainEvents.Model.BlockedDomainEventType>>? conditions = null,
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
			
			ComparisonConditions<DomainEvents.Model.BlockedDomainEventType>? conds = null;
		
			if ((ComparisonOptions.CompareProperties & comparisonOptions) == ComparisonOptions.CompareProperties)
			{
				if (conditions != null)
				{
					conds = new ComparisonConditions<DomainEvents.Model.BlockedDomainEventType>();
					conditions.Invoke(conds);

					if (conds.CanCompare(obj1, nameof(obj1.IdBlockedDomainEventType)) && obj1.IdBlockedDomainEventType != obj2.IdBlockedDomainEventType)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.Namespace)) && !string.Equals(obj1.Namespace, obj2.Namespace))
						return false;
				}
				else
				{
					if (obj1.IdBlockedDomainEventType != obj2.IdBlockedDomainEventType)
						return false;
					if (!string.Equals(obj1.Namespace, obj2.Namespace))
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
			DomainEvents.Model.BlockedDomainEventType? obj,
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<DomainEvents.Model.BlockedDomainEventType>>? conditions = null,
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
		public Action<ComparisonConditions<DomainEvents.Model.BlockedDomainEventType>>? Conditions { get; }
		public Dictionary<object, HashSet<object>>? Cache { get; }
	
		public BlockedDomainEventTypeEqualityComparer(
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<DomainEvents.Model.BlockedDomainEventType>>? conditions = null,
			Dictionary<object, HashSet<object>>? cache = null)
		{
			Cache = cache ?? [];
			ComparisonOptions = comparisonOptions;
			Conditions = conditions;
		}
	
		public bool Equals(
			DomainEvents.Model.BlockedDomainEventType? obj1,
			DomainEvents.Model.BlockedDomainEventType? obj2)
			=> EqualsTo(
				obj1,
				obj2,
				ComparisonOptions,
				Conditions,
				Cache);
	
		public int GetHashCode([DisallowNull] DomainEvents.Model.BlockedDomainEventType? obj)
			=> GetHashCode(
				obj,
				ComparisonOptions,
				Conditions,
				cache: null);
	}
}
