using Legion;
using Legion.Model.Comparers;
using System.Diagnostics.CodeAnalysis;

namespace Legion.ADF.Messaging.Outbox.Model;

public sealed partial class OutboxMessageProcessingLog : Outbox.OutboxBaseEntity, Legion.Model.IEntity
{
	public bool EqualsTo(
		Outbox.Model.OutboxMessageProcessingLog? obj,
		ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
		Action<ComparisonConditions<Outbox.Model.OutboxMessageProcessingLog>>? conditions = null)
		=> OutboxMessageProcessingLogEqualityComparer.EqualsTo(
			this,
			obj,
			comparisonOptions,
			conditions,
			cache: null);

	public sealed partial class OutboxMessageProcessingLogEqualityComparer : IEqualityComparer<OutboxMessageProcessingLog>
	{
		public static bool EqualsTo(
			Outbox.Model.OutboxMessageProcessingLog? obj1,
			Outbox.Model.OutboxMessageProcessingLog? obj2,
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<Outbox.Model.OutboxMessageProcessingLog>>? conditions = null,
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
			
			ComparisonConditions<Outbox.Model.OutboxMessageProcessingLog>? conds = null;
		
			if ((ComparisonOptions.CompareProperties & comparisonOptions) == ComparisonOptions.CompareProperties)
			{
				if (conditions != null)
				{
					conds = new ComparisonConditions<Outbox.Model.OutboxMessageProcessingLog>();
					conditions.Invoke(conds);

					if (conds.CanCompare(obj1, nameof(obj1.IdOutboxMessageProcessingLog)) && obj1.IdOutboxMessageProcessingLog != obj2.IdOutboxMessageProcessingLog)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.IdOutboxMessage)) && obj1.IdOutboxMessage != obj2.IdOutboxMessage)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.IdOutboxQueue)) && obj1.IdOutboxQueue != obj2.IdOutboxQueue)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.CreatedUtc)) && obj1.CreatedUtc != obj2.CreatedUtc)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.IdOutboxMessageStatus)) && obj1.IdOutboxMessageStatus != obj2.IdOutboxMessageStatus)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.TraceCorrelationId)) && obj1.TraceCorrelationId != obj2.TraceCorrelationId)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.IdLogMessage)) && obj1.IdLogMessage != obj2.IdLogMessage)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.Code)) && !string.Equals(obj1.Code, obj2.Code))
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.Detail)) && !string.Equals(obj1.Detail, obj2.Detail))
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.IdOutboxInstance)) && obj1.IdOutboxInstance != obj2.IdOutboxInstance)
						return false;
				}
				else
				{
					if (obj1.IdOutboxMessageProcessingLog != obj2.IdOutboxMessageProcessingLog)
						return false;
					if (obj1.IdOutboxMessage != obj2.IdOutboxMessage)
						return false;
					if (obj1.IdOutboxQueue != obj2.IdOutboxQueue)
						return false;
					if (obj1.CreatedUtc != obj2.CreatedUtc)
						return false;
					if (obj1.IdOutboxMessageStatus != obj2.IdOutboxMessageStatus)
						return false;
					if (obj1.TraceCorrelationId != obj2.TraceCorrelationId)
						return false;
					if (obj1.IdLogMessage != obj2.IdLogMessage)
						return false;
					if (!string.Equals(obj1.Code, obj2.Code))
						return false;
					if (!string.Equals(obj1.Detail, obj2.Detail))
						return false;
					if (obj1.IdOutboxInstance != obj2.IdOutboxInstance)
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
				if (!OutboxInstance.OutboxInstanceEqualityComparer.EqualsTo(obj1.OutboxInstance, obj2.OutboxInstance, comparisonOptions, conds?.GetConditions(x => x.OutboxInstance), cache))
					return false;
				if (!OutboxMessageStatus.OutboxMessageStatusEqualityComparer.EqualsTo(obj1.OutboxMessageStatus, obj2.OutboxMessageStatus, comparisonOptions, conds?.GetConditions(x => x.OutboxMessageStatus), cache))
					return false;
				if (!OutboxQueue.OutboxQueueEqualityComparer.EqualsTo(obj1.OutboxQueue, obj2.OutboxQueue, comparisonOptions, conds?.GetConditions(x => x.OutboxQueue), cache))
					return false;
		}

			return true;
		}

		public static int GetHashCode(
			Outbox.Model.OutboxMessageProcessingLog? obj,
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<Outbox.Model.OutboxMessageProcessingLog>>? conditions = null,
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
		public Action<ComparisonConditions<Outbox.Model.OutboxMessageProcessingLog>>? Conditions { get; }
		public Dictionary<object, HashSet<object>>? Cache { get; }
	
		public OutboxMessageProcessingLogEqualityComparer(
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<Outbox.Model.OutboxMessageProcessingLog>>? conditions = null,
			Dictionary<object, HashSet<object>>? cache = null)
		{
			Cache = cache ?? [];
			ComparisonOptions = comparisonOptions;
			Conditions = conditions;
		}
	
		public bool Equals(
			Outbox.Model.OutboxMessageProcessingLog? obj1,
			Outbox.Model.OutboxMessageProcessingLog? obj2)
			=> EqualsTo(
				obj1,
				obj2,
				ComparisonOptions,
				Conditions,
				Cache);
	
		public int GetHashCode([DisallowNull] Outbox.Model.OutboxMessageProcessingLog? obj)
			=> GetHashCode(
				obj,
				ComparisonOptions,
				Conditions,
				cache: null);
	}
}
