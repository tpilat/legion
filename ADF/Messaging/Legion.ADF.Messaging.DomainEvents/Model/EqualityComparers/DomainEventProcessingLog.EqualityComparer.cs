using Legion;
using Legion.Model.Comparers;
using System.Diagnostics.CodeAnalysis;

namespace Legion.ADF.Messaging.DomainEvents.Model;

public sealed partial class DomainEventProcessingLog : DomainEvents.DomainEventsBaseEntity, Legion.Model.IEntity
{
	public bool EqualsTo(
		DomainEvents.Model.DomainEventProcessingLog? obj,
		ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
		Action<ComparisonConditions<DomainEvents.Model.DomainEventProcessingLog>>? conditions = null)
		=> DomainEventProcessingLogEqualityComparer.EqualsTo(
			this,
			obj,
			comparisonOptions,
			conditions,
			cache: null);

	public sealed partial class DomainEventProcessingLogEqualityComparer : IEqualityComparer<DomainEventProcessingLog>
	{
		public static bool EqualsTo(
			DomainEvents.Model.DomainEventProcessingLog? obj1,
			DomainEvents.Model.DomainEventProcessingLog? obj2,
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<DomainEvents.Model.DomainEventProcessingLog>>? conditions = null,
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
			
			ComparisonConditions<DomainEvents.Model.DomainEventProcessingLog>? conds = null;
		
			if ((ComparisonOptions.CompareProperties & comparisonOptions) == ComparisonOptions.CompareProperties)
			{
				if (conditions != null)
				{
					conds = new ComparisonConditions<DomainEvents.Model.DomainEventProcessingLog>();
					conditions.Invoke(conds);

					if (conds.CanCompare(obj1, nameof(obj1.IdDomainEventProcessingLog)) && obj1.IdDomainEventProcessingLog != obj2.IdDomainEventProcessingLog)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.IdDomainEvent)) && obj1.IdDomainEvent != obj2.IdDomainEvent)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.CreatedUtc)) && obj1.CreatedUtc != obj2.CreatedUtc)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.IdDomainEventProcessingStatus)) && obj1.IdDomainEventProcessingStatus != obj2.IdDomainEventProcessingStatus)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.TraceCorrelationId)) && obj1.TraceCorrelationId != obj2.TraceCorrelationId)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.IdLogMessage)) && obj1.IdLogMessage != obj2.IdLogMessage)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.Code)) && !string.Equals(obj1.Code, obj2.Code))
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.Detail)) && !string.Equals(obj1.Detail, obj2.Detail))
						return false;
				}
				else
				{
					if (obj1.IdDomainEventProcessingLog != obj2.IdDomainEventProcessingLog)
						return false;
					if (obj1.IdDomainEvent != obj2.IdDomainEvent)
						return false;
					if (obj1.CreatedUtc != obj2.CreatedUtc)
						return false;
					if (obj1.IdDomainEventProcessingStatus != obj2.IdDomainEventProcessingStatus)
						return false;
					if (obj1.TraceCorrelationId != obj2.TraceCorrelationId)
						return false;
					if (obj1.IdLogMessage != obj2.IdLogMessage)
						return false;
					if (!string.Equals(obj1.Code, obj2.Code))
						return false;
					if (!string.Equals(obj1.Detail, obj2.Detail))
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
				if (!DomainEventProcessingStatus.DomainEventProcessingStatusEqualityComparer.EqualsTo(obj1.DomainEventProcessingStatus, obj2.DomainEventProcessingStatus, comparisonOptions, conds?.GetConditions(x => x.DomainEventProcessingStatus), cache))
					return false;
		}

			return true;
		}

		public static int GetHashCode(
			DomainEvents.Model.DomainEventProcessingLog? obj,
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<DomainEvents.Model.DomainEventProcessingLog>>? conditions = null,
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
		public Action<ComparisonConditions<DomainEvents.Model.DomainEventProcessingLog>>? Conditions { get; }
		public Dictionary<object, HashSet<object>>? Cache { get; }
	
		public DomainEventProcessingLogEqualityComparer(
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<DomainEvents.Model.DomainEventProcessingLog>>? conditions = null,
			Dictionary<object, HashSet<object>>? cache = null)
		{
			Cache = cache ?? [];
			ComparisonOptions = comparisonOptions;
			Conditions = conditions;
		}
	
		public bool Equals(
			DomainEvents.Model.DomainEventProcessingLog? obj1,
			DomainEvents.Model.DomainEventProcessingLog? obj2)
			=> EqualsTo(
				obj1,
				obj2,
				ComparisonOptions,
				Conditions,
				Cache);
	
		public int GetHashCode([DisallowNull] DomainEvents.Model.DomainEventProcessingLog? obj)
			=> GetHashCode(
				obj,
				ComparisonOptions,
				Conditions,
				cache: null);
	}
}
