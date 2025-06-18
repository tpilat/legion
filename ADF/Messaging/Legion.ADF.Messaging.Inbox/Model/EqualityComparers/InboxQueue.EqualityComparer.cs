using Legion;
using Legion.Model.Comparers;
using System.Diagnostics.CodeAnalysis;

namespace Legion.ADF.Messaging.Inbox.Model;

public sealed partial class InboxQueue : Inbox.InboxBaseEntity, Legion.Model.IEntity
{
	public bool EqualsTo(
		Inbox.Model.InboxQueue? obj,
		ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
		Action<ComparisonConditions<Inbox.Model.InboxQueue>>? conditions = null)
		=> InboxQueueEqualityComparer.EqualsTo(
			this,
			obj,
			comparisonOptions,
			conditions,
			cache: null);

	public sealed partial class InboxQueueEqualityComparer : IEqualityComparer<InboxQueue>
	{
		public static bool EqualsTo(
			Inbox.Model.InboxQueue? obj1,
			Inbox.Model.InboxQueue? obj2,
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<Inbox.Model.InboxQueue>>? conditions = null,
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
			
			ComparisonConditions<Inbox.Model.InboxQueue>? conds = null;
		
			if ((ComparisonOptions.CompareProperties & comparisonOptions) == ComparisonOptions.CompareProperties)
			{
				if (conditions != null)
				{
					conds = new ComparisonConditions<Inbox.Model.InboxQueue>();
					conditions.Invoke(conds);

					if (conds.CanCompare(obj1, nameof(obj1.IdInboxQueue)) && obj1.IdInboxQueue != obj2.IdInboxQueue)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.Name)) && !string.Equals(obj1.Name, obj2.Name))
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.ReceivedEventNamespace)) && !string.Equals(obj1.ReceivedEventNamespace, obj2.ReceivedEventNamespace))
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.IdMessageType)) && obj1.IdMessageType != obj2.IdMessageType)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.IsActive)) && obj1.IsActive != obj2.IsActive)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.IsSequentialFIFO)) && obj1.IsSequentialFIFO != obj2.IsSequentialFIFO)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.MessagesBatchCount)) && obj1.MessagesBatchCount != obj2.MessagesBatchCount)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.MaxDegreeOfParallelism)) && obj1.MaxDegreeOfParallelism != obj2.MaxDegreeOfParallelism)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.TimeoutForMessageProcessing)) && obj1.TimeoutForMessageProcessing != obj2.TimeoutForMessageProcessing)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.MaxMessageProcessingRetryCount)) && obj1.MaxMessageProcessingRetryCount != obj2.MaxMessageProcessingRetryCount)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.Properties)) && !string.Equals(obj1.Properties, obj2.Properties))
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.IdProcessingMode)) && obj1.IdProcessingMode != obj2.IdProcessingMode)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.IdSuspendingMode)) && obj1.IdSuspendingMode != obj2.IdSuspendingMode)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.IdInboxInstance)) && obj1.IdInboxInstance != obj2.IdInboxInstance)
						return false;
				}
				else
				{
					if (obj1.IdInboxQueue != obj2.IdInboxQueue)
						return false;
					if (!string.Equals(obj1.Name, obj2.Name))
						return false;
					if (!string.Equals(obj1.ReceivedEventNamespace, obj2.ReceivedEventNamespace))
						return false;
					if (obj1.IdMessageType != obj2.IdMessageType)
						return false;
					if (obj1.IsActive != obj2.IsActive)
						return false;
					if (obj1.IsSequentialFIFO != obj2.IsSequentialFIFO)
						return false;
					if (obj1.MessagesBatchCount != obj2.MessagesBatchCount)
						return false;
					if (obj1.MaxDegreeOfParallelism != obj2.MaxDegreeOfParallelism)
						return false;
					if (obj1.TimeoutForMessageProcessing != obj2.TimeoutForMessageProcessing)
						return false;
					if (obj1.MaxMessageProcessingRetryCount != obj2.MaxMessageProcessingRetryCount)
						return false;
					if (!string.Equals(obj1.Properties, obj2.Properties))
						return false;
					if (obj1.IdProcessingMode != obj2.IdProcessingMode)
						return false;
					if (obj1.IdSuspendingMode != obj2.IdSuspendingMode)
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
				if (!InboxMessageType.InboxMessageTypeEqualityComparer.EqualsTo(obj1.MessageType, obj2.MessageType, comparisonOptions, conds?.GetConditions(x => x.MessageType), cache))
					return false;
				if (!InboxQueueProcessingMode.InboxQueueProcessingModeEqualityComparer.EqualsTo(obj1.ProcessingMode, obj2.ProcessingMode, comparisonOptions, conds?.GetConditions(x => x.ProcessingMode), cache))
					return false;
				if (!InboxQueueProcessingMode.InboxQueueProcessingModeEqualityComparer.EqualsTo(obj1.SuspendingMode, obj2.SuspendingMode, comparisonOptions, conds?.GetConditions(x => x.SuspendingMode), cache))
					return false;
				if (!ComparisonHelper.SequenceEqual(obj1.InboxMessageArchives, obj2.InboxMessageArchives, new InboxMessageArchive.InboxMessageArchiveEqualityComparer(comparisonOptions, conds?.GetConditions(x => x.InboxMessageArchives), cache)))
					return false;
				if (!ComparisonHelper.SequenceEqual(obj1.InboxMessageProcessingLogs, obj2.InboxMessageProcessingLogs, new InboxMessageProcessingLog.InboxMessageProcessingLogEqualityComparer(comparisonOptions, conds?.GetConditions(x => x.InboxMessageProcessingLogs), cache)))
					return false;
				if (!ComparisonHelper.SequenceEqual(obj1.InboxMessages, obj2.InboxMessages, new InboxMessage.InboxMessageEqualityComparer(comparisonOptions, conds?.GetConditions(x => x.InboxMessages), cache)))
					return false;
				if (!ComparisonHelper.SequenceEqual(obj1.InboxProcessingLogs, obj2.InboxProcessingLogs, new InboxProcessingLog.InboxProcessingLogEqualityComparer(comparisonOptions, conds?.GetConditions(x => x.InboxProcessingLogs), cache)))
					return false;
		}

			return true;
		}

		public static int GetHashCode(
			Inbox.Model.InboxQueue? obj,
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<Inbox.Model.InboxQueue>>? conditions = null,
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
		public Action<ComparisonConditions<Inbox.Model.InboxQueue>>? Conditions { get; }
		public Dictionary<object, HashSet<object>>? Cache { get; }
	
		public InboxQueueEqualityComparer(
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<Inbox.Model.InboxQueue>>? conditions = null,
			Dictionary<object, HashSet<object>>? cache = null)
		{
			Cache = cache ?? [];
			ComparisonOptions = comparisonOptions;
			Conditions = conditions;
		}
	
		public bool Equals(
			Inbox.Model.InboxQueue? obj1,
			Inbox.Model.InboxQueue? obj2)
			=> EqualsTo(
				obj1,
				obj2,
				ComparisonOptions,
				Conditions,
				Cache);
	
		public int GetHashCode([DisallowNull] Inbox.Model.InboxQueue? obj)
			=> GetHashCode(
				obj,
				ComparisonOptions,
				Conditions,
				cache: null);
	}
}
