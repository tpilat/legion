using Legion;
using Legion.Model.Comparers;
using System.Diagnostics.CodeAnalysis;

namespace Legion.ADF.Messaging.DomainEvents.Model;

public sealed partial class DomainEventContent : DomainEvents.DomainEventsBaseEntity, Legion.Model.IEntity
{
	public bool EqualsTo(
		DomainEvents.Model.DomainEventContent? obj,
		ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
		Action<ComparisonConditions<DomainEvents.Model.DomainEventContent>>? conditions = null)
		=> DomainEventContentEqualityComparer.EqualsTo(
			this,
			obj,
			comparisonOptions,
			conditions,
			cache: null);

	public sealed partial class DomainEventContentEqualityComparer : IEqualityComparer<DomainEventContent>
	{
		public static bool EqualsTo(
			DomainEvents.Model.DomainEventContent? obj1,
			DomainEvents.Model.DomainEventContent? obj2,
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<DomainEvents.Model.DomainEventContent>>? conditions = null,
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
			
			ComparisonConditions<DomainEvents.Model.DomainEventContent>? conds = null;
		
			if ((ComparisonOptions.CompareProperties & comparisonOptions) == ComparisonOptions.CompareProperties)
			{
				if (conditions != null)
				{
					conds = new ComparisonConditions<DomainEvents.Model.DomainEventContent>();
					conditions.Invoke(conds);

					if (conds.CanCompare(obj1, nameof(obj1.IdDomainEventContent)) && obj1.IdDomainEventContent != obj2.IdDomainEventContent)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.Content)) && !string.Equals(obj1.Content, obj2.Content))
						return false;
				}
				else
				{
					if (obj1.IdDomainEventContent != obj2.IdDomainEventContent)
						return false;
					if (!string.Equals(obj1.Content, obj2.Content))
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
				if (!DomainEvent.DomainEventEqualityComparer.EqualsTo(obj1.DomainEvent, obj2.DomainEvent, comparisonOptions, conds?.GetConditions(x => x.DomainEvent), cache))
					return false;
		}

			return true;
		}

		public static int GetHashCode(
			DomainEvents.Model.DomainEventContent? obj,
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<DomainEvents.Model.DomainEventContent>>? conditions = null,
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
		public Action<ComparisonConditions<DomainEvents.Model.DomainEventContent>>? Conditions { get; }
		public Dictionary<object, HashSet<object>>? Cache { get; }
	
		public DomainEventContentEqualityComparer(
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<DomainEvents.Model.DomainEventContent>>? conditions = null,
			Dictionary<object, HashSet<object>>? cache = null)
		{
			Cache = cache ?? [];
			ComparisonOptions = comparisonOptions;
			Conditions = conditions;
		}
	
		public bool Equals(
			DomainEvents.Model.DomainEventContent? obj1,
			DomainEvents.Model.DomainEventContent? obj2)
			=> EqualsTo(
				obj1,
				obj2,
				ComparisonOptions,
				Conditions,
				Cache);
	
		public int GetHashCode([DisallowNull] DomainEvents.Model.DomainEventContent? obj)
			=> GetHashCode(
				obj,
				ComparisonOptions,
				Conditions,
				cache: null);
	}
}
