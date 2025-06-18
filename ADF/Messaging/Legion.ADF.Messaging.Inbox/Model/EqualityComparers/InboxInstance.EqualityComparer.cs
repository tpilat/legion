using Legion;
using Legion.Model.Comparers;
using System.Diagnostics.CodeAnalysis;

namespace Legion.ADF.Messaging.Inbox.Model;

public sealed partial class InboxInstance : Inbox.InboxBaseEntity, Legion.Model.IEntity
{
	public bool EqualsTo(
		Inbox.Model.InboxInstance? obj,
		ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
		Action<ComparisonConditions<Inbox.Model.InboxInstance>>? conditions = null)
		=> InboxInstanceEqualityComparer.EqualsTo(
			this,
			obj,
			comparisonOptions,
			conditions,
			cache: null);

	public sealed partial class InboxInstanceEqualityComparer : IEqualityComparer<InboxInstance>
	{
		public static bool EqualsTo(
			Inbox.Model.InboxInstance? obj1,
			Inbox.Model.InboxInstance? obj2,
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<Inbox.Model.InboxInstance>>? conditions = null,
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
			
			ComparisonConditions<Inbox.Model.InboxInstance>? conds = null;
		
			if ((ComparisonOptions.CompareProperties & comparisonOptions) == ComparisonOptions.CompareProperties)
			{
				if (conditions != null)
				{
					conds = new ComparisonConditions<Inbox.Model.InboxInstance>();
					conditions.Invoke(conds);

					if (conds.CanCompare(obj1, nameof(obj1.IdInboxInstance)) && obj1.IdInboxInstance != obj2.IdInboxInstance)
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
					if (obj1.IdInboxInstance != obj2.IdInboxInstance)
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
				if (!ComparisonHelper.SequenceEqual(obj1.BlockedInboxMessageTypes, obj2.BlockedInboxMessageTypes, new BlockedInboxMessageType.BlockedInboxMessageTypeEqualityComparer(comparisonOptions, conds?.GetConditions(x => x.BlockedInboxMessageTypes), cache)))
					return false;
				if (!ComparisonHelper.SequenceEqual(obj1.InboxMessageArchives, obj2.InboxMessageArchives, new InboxMessageArchive.InboxMessageArchiveEqualityComparer(comparisonOptions, conds?.GetConditions(x => x.InboxMessageArchives), cache)))
					return false;
				if (!ComparisonHelper.SequenceEqual(obj1.InboxMessageProcessingLogs, obj2.InboxMessageProcessingLogs, new InboxMessageProcessingLog.InboxMessageProcessingLogEqualityComparer(comparisonOptions, conds?.GetConditions(x => x.InboxMessageProcessingLogs), cache)))
					return false;
				if (!ComparisonHelper.SequenceEqual(obj1.InboxMessages, obj2.InboxMessages, new InboxMessage.InboxMessageEqualityComparer(comparisonOptions, conds?.GetConditions(x => x.InboxMessages), cache)))
					return false;
				if (!ComparisonHelper.SequenceEqual(obj1.InboxMessageTypes, obj2.InboxMessageTypes, new InboxMessageType.InboxMessageTypeEqualityComparer(comparisonOptions, conds?.GetConditions(x => x.InboxMessageTypes), cache)))
					return false;
				if (!ComparisonHelper.SequenceEqual(obj1.InboxProcessingLogs, obj2.InboxProcessingLogs, new InboxProcessingLog.InboxProcessingLogEqualityComparer(comparisonOptions, conds?.GetConditions(x => x.InboxProcessingLogs), cache)))
					return false;
				if (!ComparisonHelper.SequenceEqual(obj1.InboxQueues, obj2.InboxQueues, new InboxQueue.InboxQueueEqualityComparer(comparisonOptions, conds?.GetConditions(x => x.InboxQueues), cache)))
					return false;
		}

			return true;
		}

		public static int GetHashCode(
			Inbox.Model.InboxInstance? obj,
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<Inbox.Model.InboxInstance>>? conditions = null,
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
		public Action<ComparisonConditions<Inbox.Model.InboxInstance>>? Conditions { get; }
		public Dictionary<object, HashSet<object>>? Cache { get; }
	
		public InboxInstanceEqualityComparer(
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<Inbox.Model.InboxInstance>>? conditions = null,
			Dictionary<object, HashSet<object>>? cache = null)
		{
			Cache = cache ?? [];
			ComparisonOptions = comparisonOptions;
			Conditions = conditions;
		}
	
		public bool Equals(
			Inbox.Model.InboxInstance? obj1,
			Inbox.Model.InboxInstance? obj2)
			=> EqualsTo(
				obj1,
				obj2,
				ComparisonOptions,
				Conditions,
				Cache);
	
		public int GetHashCode([DisallowNull] Inbox.Model.InboxInstance? obj)
			=> GetHashCode(
				obj,
				ComparisonOptions,
				Conditions,
				cache: null);
	}
}
