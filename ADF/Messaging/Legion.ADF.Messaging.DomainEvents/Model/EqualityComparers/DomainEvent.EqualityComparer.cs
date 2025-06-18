using Legion;
using Legion.Model.Comparers;
using System.Diagnostics.CodeAnalysis;

namespace Legion.ADF.Messaging.DomainEvents.Model;

public sealed partial class DomainEvent : DomainEvents.DomainEventsBaseEntity, Legion.Model.IEntity
{
	public bool EqualsTo(
		DomainEvents.Model.DomainEvent? obj,
		ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
		Action<ComparisonConditions<DomainEvents.Model.DomainEvent>>? conditions = null)
		=> DomainEventEqualityComparer.EqualsTo(
			this,
			obj,
			comparisonOptions,
			conditions,
			cache: null);

	public sealed partial class DomainEventEqualityComparer : IEqualityComparer<DomainEvent>
	{
		public static bool EqualsTo(
			DomainEvents.Model.DomainEvent? obj1,
			DomainEvents.Model.DomainEvent? obj2,
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<DomainEvents.Model.DomainEvent>>? conditions = null,
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
			
			ComparisonConditions<DomainEvents.Model.DomainEvent>? conds = null;
		
			if ((ComparisonOptions.CompareProperties & comparisonOptions) == ComparisonOptions.CompareProperties)
			{
				if (conditions != null)
				{
					conds = new ComparisonConditions<DomainEvents.Model.DomainEvent>();
					conditions.Invoke(conds);

					if (conds.CanCompare(obj1, nameof(obj1.IdDomainEvent)) && obj1.IdDomainEvent != obj2.IdDomainEvent)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.IdContent)) && obj1.IdContent != obj2.IdContent)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.IdDomainEventProcessingStatus)) && obj1.IdDomainEventProcessingStatus != obj2.IdDomainEventProcessingStatus)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.Namespace)) && !string.Equals(obj1.Namespace, obj2.Namespace))
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.TraceCorrelationId)) && obj1.TraceCorrelationId != obj2.TraceCorrelationId)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.Properties)) && !string.Equals(obj1.Properties, obj2.Properties))
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.Publisher)) && !string.Equals(obj1.Publisher, obj2.Publisher))
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.PublisherId)) && !string.Equals(obj1.PublisherId, obj2.PublisherId))
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.CreatedUtc)) && obj1.CreatedUtc != obj2.CreatedUtc)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.ProcessedUtc)) && obj1.ProcessedUtc != obj2.ProcessedUtc)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.SuspendedUtc)) && obj1.SuspendedUtc != obj2.SuspendedUtc)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.LastProcessingUtc)) && obj1.LastProcessingUtc != obj2.LastProcessingUtc)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.LastProcessingTimeoutUtc)) && obj1.LastProcessingTimeoutUtc != obj2.LastProcessingTimeoutUtc)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.NextProcessingUtc)) && obj1.NextProcessingUtc != obj2.NextProcessingUtc)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.RetryCount)) && obj1.RetryCount != obj2.RetryCount)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.Priority)) && obj1.Priority != obj2.Priority)
						return false;
				}
				else
				{
					if (obj1.IdDomainEvent != obj2.IdDomainEvent)
						return false;
					if (obj1.IdContent != obj2.IdContent)
						return false;
					if (obj1.IdDomainEventProcessingStatus != obj2.IdDomainEventProcessingStatus)
						return false;
					if (!string.Equals(obj1.Namespace, obj2.Namespace))
						return false;
					if (obj1.TraceCorrelationId != obj2.TraceCorrelationId)
						return false;
					if (!string.Equals(obj1.Properties, obj2.Properties))
						return false;
					if (!string.Equals(obj1.Publisher, obj2.Publisher))
						return false;
					if (!string.Equals(obj1.PublisherId, obj2.PublisherId))
						return false;
					if (obj1.CreatedUtc != obj2.CreatedUtc)
						return false;
					if (obj1.ProcessedUtc != obj2.ProcessedUtc)
						return false;
					if (obj1.SuspendedUtc != obj2.SuspendedUtc)
						return false;
					if (obj1.LastProcessingUtc != obj2.LastProcessingUtc)
						return false;
					if (obj1.LastProcessingTimeoutUtc != obj2.LastProcessingTimeoutUtc)
						return false;
					if (obj1.NextProcessingUtc != obj2.NextProcessingUtc)
						return false;
					if (obj1.RetryCount != obj2.RetryCount)
						return false;
					if (obj1.Priority != obj2.Priority)
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
				if (!DomainEventContent.DomainEventContentEqualityComparer.EqualsTo(obj1.Content, obj2.Content, comparisonOptions, conds?.GetConditions(x => x.Content), cache))
					return false;
				if (!DomainEventProcessingStatus.DomainEventProcessingStatusEqualityComparer.EqualsTo(obj1.DomainEventProcessingStatus, obj2.DomainEventProcessingStatus, comparisonOptions, conds?.GetConditions(x => x.DomainEventProcessingStatus), cache))
					return false;
				if (!ComparisonHelper.SequenceEqual(obj1.DomainEventProcessingLogs, obj2.DomainEventProcessingLogs, new DomainEventProcessingLog.DomainEventProcessingLogEqualityComparer(comparisonOptions, conds?.GetConditions(x => x.DomainEventProcessingLogs), cache)))
					return false;
		}

			return true;
		}

		public static int GetHashCode(
			DomainEvents.Model.DomainEvent? obj,
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<DomainEvents.Model.DomainEvent>>? conditions = null,
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
		public Action<ComparisonConditions<DomainEvents.Model.DomainEvent>>? Conditions { get; }
		public Dictionary<object, HashSet<object>>? Cache { get; }
	
		public DomainEventEqualityComparer(
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<DomainEvents.Model.DomainEvent>>? conditions = null,
			Dictionary<object, HashSet<object>>? cache = null)
		{
			Cache = cache ?? [];
			ComparisonOptions = comparisonOptions;
			Conditions = conditions;
		}
	
		public bool Equals(
			DomainEvents.Model.DomainEvent? obj1,
			DomainEvents.Model.DomainEvent? obj2)
			=> EqualsTo(
				obj1,
				obj2,
				ComparisonOptions,
				Conditions,
				Cache);
	
		public int GetHashCode([DisallowNull] DomainEvents.Model.DomainEvent? obj)
			=> GetHashCode(
				obj,
				ComparisonOptions,
				Conditions,
				cache: null);
	}
}
