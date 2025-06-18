using Legion;
using Legion.Model.Comparers;
using System.Diagnostics.CodeAnalysis;

namespace Legion.ADF.Messaging.Outbox.Model;

public sealed partial class OutboxInstance : Outbox.OutboxBaseEntity, Legion.Model.IEntity
{
	public bool EqualsTo(
		Outbox.Model.OutboxInstance? obj,
		ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
		Action<ComparisonConditions<Outbox.Model.OutboxInstance>>? conditions = null)
		=> OutboxInstanceEqualityComparer.EqualsTo(
			this,
			obj,
			comparisonOptions,
			conditions,
			cache: null);

	public sealed partial class OutboxInstanceEqualityComparer : IEqualityComparer<OutboxInstance>
	{
		public static bool EqualsTo(
			Outbox.Model.OutboxInstance? obj1,
			Outbox.Model.OutboxInstance? obj2,
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<Outbox.Model.OutboxInstance>>? conditions = null,
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
			
			ComparisonConditions<Outbox.Model.OutboxInstance>? conds = null;
		
			if ((ComparisonOptions.CompareProperties & comparisonOptions) == ComparisonOptions.CompareProperties)
			{
				if (conditions != null)
				{
					conds = new ComparisonConditions<Outbox.Model.OutboxInstance>();
					conditions.Invoke(conds);

					if (conds.CanCompare(obj1, nameof(obj1.IdOutboxInstance)) && obj1.IdOutboxInstance != obj2.IdOutboxInstance)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.CreatedUtc)) && obj1.CreatedUtc != obj2.CreatedUtc)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.Name)) && !string.Equals(obj1.Name, obj2.Name))
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.Version)) && !string.Equals(obj1.Version, obj2.Version))
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.MaxDegreeOfQueueParallelism)) && obj1.MaxDegreeOfQueueParallelism != obj2.MaxDegreeOfQueueParallelism)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.IdLogLevel)) && obj1.IdLogLevel != obj2.IdLogLevel)
						return false;
				}
				else
				{
					if (obj1.IdOutboxInstance != obj2.IdOutboxInstance)
						return false;
					if (obj1.CreatedUtc != obj2.CreatedUtc)
						return false;
					if (!string.Equals(obj1.Name, obj2.Name))
						return false;
					if (!string.Equals(obj1.Version, obj2.Version))
						return false;
					if (obj1.MaxDegreeOfQueueParallelism != obj2.MaxDegreeOfQueueParallelism)
						return false;
					if (obj1.IdLogLevel != obj2.IdLogLevel)
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
				if (!ComparisonHelper.SequenceEqual(obj1.BlockedOutboxMessageTypes, obj2.BlockedOutboxMessageTypes, new BlockedOutboxMessageType.BlockedOutboxMessageTypeEqualityComparer(comparisonOptions, conds?.GetConditions(x => x.BlockedOutboxMessageTypes), cache)))
					return false;
				if (!ComparisonHelper.SequenceEqual(obj1.OutboxMessageArchives, obj2.OutboxMessageArchives, new OutboxMessageArchive.OutboxMessageArchiveEqualityComparer(comparisonOptions, conds?.GetConditions(x => x.OutboxMessageArchives), cache)))
					return false;
				if (!ComparisonHelper.SequenceEqual(obj1.OutboxMessageProcessingLogs, obj2.OutboxMessageProcessingLogs, new OutboxMessageProcessingLog.OutboxMessageProcessingLogEqualityComparer(comparisonOptions, conds?.GetConditions(x => x.OutboxMessageProcessingLogs), cache)))
					return false;
				if (!ComparisonHelper.SequenceEqual(obj1.OutboxMessages, obj2.OutboxMessages, new OutboxMessage.OutboxMessageEqualityComparer(comparisonOptions, conds?.GetConditions(x => x.OutboxMessages), cache)))
					return false;
				if (!ComparisonHelper.SequenceEqual(obj1.OutboxMessageTypes, obj2.OutboxMessageTypes, new OutboxMessageType.OutboxMessageTypeEqualityComparer(comparisonOptions, conds?.GetConditions(x => x.OutboxMessageTypes), cache)))
					return false;
				if (!ComparisonHelper.SequenceEqual(obj1.OutboxProcessingLogs, obj2.OutboxProcessingLogs, new OutboxProcessingLog.OutboxProcessingLogEqualityComparer(comparisonOptions, conds?.GetConditions(x => x.OutboxProcessingLogs), cache)))
					return false;
				if (!ComparisonHelper.SequenceEqual(obj1.OutboxQueues, obj2.OutboxQueues, new OutboxQueue.OutboxQueueEqualityComparer(comparisonOptions, conds?.GetConditions(x => x.OutboxQueues), cache)))
					return false;
		}

			return true;
		}

		public static int GetHashCode(
			Outbox.Model.OutboxInstance? obj,
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<Outbox.Model.OutboxInstance>>? conditions = null,
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
		public Action<ComparisonConditions<Outbox.Model.OutboxInstance>>? Conditions { get; }
		public Dictionary<object, HashSet<object>>? Cache { get; }
	
		public OutboxInstanceEqualityComparer(
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<Outbox.Model.OutboxInstance>>? conditions = null,
			Dictionary<object, HashSet<object>>? cache = null)
		{
			Cache = cache ?? [];
			ComparisonOptions = comparisonOptions;
			Conditions = conditions;
		}
	
		public bool Equals(
			Outbox.Model.OutboxInstance? obj1,
			Outbox.Model.OutboxInstance? obj2)
			=> EqualsTo(
				obj1,
				obj2,
				ComparisonOptions,
				Conditions,
				Cache);
	
		public int GetHashCode([DisallowNull] Outbox.Model.OutboxInstance? obj)
			=> GetHashCode(
				obj,
				ComparisonOptions,
				Conditions,
				cache: null);
	}
}
