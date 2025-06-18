using Legion;
using Legion.Model.Comparers;
using System.Diagnostics.CodeAnalysis;

namespace Legion.ADF.Messaging.Inbox.Model;

public sealed partial class InboxMessageProcessingLog : Inbox.InboxBaseEntity, Legion.Model.IEntity
{
	public bool EqualsTo(
		Inbox.Model.InboxMessageProcessingLog? obj,
		ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
		Action<ComparisonConditions<Inbox.Model.InboxMessageProcessingLog>>? conditions = null)
		=> InboxMessageProcessingLogEqualityComparer.EqualsTo(
			this,
			obj,
			comparisonOptions,
			conditions,
			cache: null);

	public sealed partial class InboxMessageProcessingLogEqualityComparer : IEqualityComparer<InboxMessageProcessingLog>
	{
		public static bool EqualsTo(
			Inbox.Model.InboxMessageProcessingLog? obj1,
			Inbox.Model.InboxMessageProcessingLog? obj2,
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<Inbox.Model.InboxMessageProcessingLog>>? conditions = null,
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
			
			ComparisonConditions<Inbox.Model.InboxMessageProcessingLog>? conds = null;
		
			if ((ComparisonOptions.CompareProperties & comparisonOptions) == ComparisonOptions.CompareProperties)
			{
				if (conditions != null)
				{
					conds = new ComparisonConditions<Inbox.Model.InboxMessageProcessingLog>();
					conditions.Invoke(conds);

					if (conds.CanCompare(obj1, nameof(obj1.IdInboxMessageProcessingLog)) && obj1.IdInboxMessageProcessingLog != obj2.IdInboxMessageProcessingLog)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.IdInboxMessage)) && obj1.IdInboxMessage != obj2.IdInboxMessage)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.IdInboxQueue)) && obj1.IdInboxQueue != obj2.IdInboxQueue)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.CreatedUtc)) && obj1.CreatedUtc != obj2.CreatedUtc)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.IdInboxMessageStatus)) && obj1.IdInboxMessageStatus != obj2.IdInboxMessageStatus)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.TraceCorrelationId)) && obj1.TraceCorrelationId != obj2.TraceCorrelationId)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.IdLogMessage)) && obj1.IdLogMessage != obj2.IdLogMessage)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.Code)) && !string.Equals(obj1.Code, obj2.Code))
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.Detail)) && !string.Equals(obj1.Detail, obj2.Detail))
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.IdInboxInstance)) && obj1.IdInboxInstance != obj2.IdInboxInstance)
						return false;
				}
				else
				{
					if (obj1.IdInboxMessageProcessingLog != obj2.IdInboxMessageProcessingLog)
						return false;
					if (obj1.IdInboxMessage != obj2.IdInboxMessage)
						return false;
					if (obj1.IdInboxQueue != obj2.IdInboxQueue)
						return false;
					if (obj1.CreatedUtc != obj2.CreatedUtc)
						return false;
					if (obj1.IdInboxMessageStatus != obj2.IdInboxMessageStatus)
						return false;
					if (obj1.TraceCorrelationId != obj2.TraceCorrelationId)
						return false;
					if (obj1.IdLogMessage != obj2.IdLogMessage)
						return false;
					if (!string.Equals(obj1.Code, obj2.Code))
						return false;
					if (!string.Equals(obj1.Detail, obj2.Detail))
						return false;
					if (obj1.IdInboxInstance != obj2.IdInboxInstance)
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
				if (!InboxInstance.InboxInstanceEqualityComparer.EqualsTo(obj1.InboxInstance, obj2.InboxInstance, comparisonOptions, conds?.GetConditions(x => x.InboxInstance), cache))
					return false;
				if (!InboxMessageStatus.InboxMessageStatusEqualityComparer.EqualsTo(obj1.InboxMessageStatus, obj2.InboxMessageStatus, comparisonOptions, conds?.GetConditions(x => x.InboxMessageStatus), cache))
					return false;
				if (!InboxQueue.InboxQueueEqualityComparer.EqualsTo(obj1.InboxQueue, obj2.InboxQueue, comparisonOptions, conds?.GetConditions(x => x.InboxQueue), cache))
					return false;
		}

			return true;
		}

		public static int GetHashCode(
			Inbox.Model.InboxMessageProcessingLog? obj,
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<Inbox.Model.InboxMessageProcessingLog>>? conditions = null,
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
		public Action<ComparisonConditions<Inbox.Model.InboxMessageProcessingLog>>? Conditions { get; }
		public Dictionary<object, HashSet<object>>? Cache { get; }
	
		public InboxMessageProcessingLogEqualityComparer(
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<Inbox.Model.InboxMessageProcessingLog>>? conditions = null,
			Dictionary<object, HashSet<object>>? cache = null)
		{
			Cache = cache ?? [];
			ComparisonOptions = comparisonOptions;
			Conditions = conditions;
		}
	
		public bool Equals(
			Inbox.Model.InboxMessageProcessingLog? obj1,
			Inbox.Model.InboxMessageProcessingLog? obj2)
			=> EqualsTo(
				obj1,
				obj2,
				ComparisonOptions,
				Conditions,
				Cache);
	
		public int GetHashCode([DisallowNull] Inbox.Model.InboxMessageProcessingLog? obj)
			=> GetHashCode(
				obj,
				ComparisonOptions,
				Conditions,
				cache: null);
	}
}
