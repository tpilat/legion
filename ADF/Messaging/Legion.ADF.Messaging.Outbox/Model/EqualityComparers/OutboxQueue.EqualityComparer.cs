using Legion;
using Legion.Model.Comparers;
using System.Diagnostics.CodeAnalysis;

namespace Legion.ADF.Messaging.Outbox.Model;

public sealed partial class OutboxQueue : Outbox.OutboxBaseEntity, Legion.Model.IEntity
{
	public bool EqualsTo(
		Outbox.Model.OutboxQueue? obj,
		ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
		Action<ComparisonConditions<Outbox.Model.OutboxQueue>>? conditions = null)
		=> OutboxQueueEqualityComparer.EqualsTo(
			this,
			obj,
			comparisonOptions,
			conditions,
			cache: null);

	public sealed partial class OutboxQueueEqualityComparer : IEqualityComparer<OutboxQueue>
	{
		public static bool EqualsTo(
			Outbox.Model.OutboxQueue? obj1,
			Outbox.Model.OutboxQueue? obj2,
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<Outbox.Model.OutboxQueue>>? conditions = null,
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
			
			ComparisonConditions<Outbox.Model.OutboxQueue>? conds = null;
		
			if ((ComparisonOptions.CompareProperties & comparisonOptions) == ComparisonOptions.CompareProperties)
			{
				if (conditions != null)
				{
					conds = new ComparisonConditions<Outbox.Model.OutboxQueue>();
					conditions.Invoke(conds);

					if (conds.CanCompare(obj1, nameof(obj1.IdOutboxQueue)) && obj1.IdOutboxQueue != obj2.IdOutboxQueue)
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
					if (conds.CanCompare(obj1, nameof(obj1.IdOutboxInstance)) && obj1.IdOutboxInstance != obj2.IdOutboxInstance)
						return false;
				}
				else
				{
					if (obj1.IdOutboxQueue != obj2.IdOutboxQueue)
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
				if (!OutboxMessageType.OutboxMessageTypeEqualityComparer.EqualsTo(obj1.MessageType, obj2.MessageType, comparisonOptions, conds?.GetConditions(x => x.MessageType), cache))
					return false;
				if (!OutboxInstance.OutboxInstanceEqualityComparer.EqualsTo(obj1.OutboxInstance, obj2.OutboxInstance, comparisonOptions, conds?.GetConditions(x => x.OutboxInstance), cache))
					return false;
				if (!OutboxQueueProcessingMode.OutboxQueueProcessingModeEqualityComparer.EqualsTo(obj1.ProcessingMode, obj2.ProcessingMode, comparisonOptions, conds?.GetConditions(x => x.ProcessingMode), cache))
					return false;
				if (!OutboxQueueProcessingMode.OutboxQueueProcessingModeEqualityComparer.EqualsTo(obj1.SuspendingMode, obj2.SuspendingMode, comparisonOptions, conds?.GetConditions(x => x.SuspendingMode), cache))
					return false;
				if (!ComparisonHelper.SequenceEqual(obj1.OutboxMessageArchives, obj2.OutboxMessageArchives, new OutboxMessageArchive.OutboxMessageArchiveEqualityComparer(comparisonOptions, conds?.GetConditions(x => x.OutboxMessageArchives), cache)))
					return false;
				if (!ComparisonHelper.SequenceEqual(obj1.OutboxMessageProcessingLogs, obj2.OutboxMessageProcessingLogs, new OutboxMessageProcessingLog.OutboxMessageProcessingLogEqualityComparer(comparisonOptions, conds?.GetConditions(x => x.OutboxMessageProcessingLogs), cache)))
					return false;
				if (!ComparisonHelper.SequenceEqual(obj1.OutboxMessages, obj2.OutboxMessages, new OutboxMessage.OutboxMessageEqualityComparer(comparisonOptions, conds?.GetConditions(x => x.OutboxMessages), cache)))
					return false;
				if (!ComparisonHelper.SequenceEqual(obj1.OutboxProcessingLogs, obj2.OutboxProcessingLogs, new OutboxProcessingLog.OutboxProcessingLogEqualityComparer(comparisonOptions, conds?.GetConditions(x => x.OutboxProcessingLogs), cache)))
					return false;
		}

			return true;
		}

		public static int GetHashCode(
			Outbox.Model.OutboxQueue? obj,
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<Outbox.Model.OutboxQueue>>? conditions = null,
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
		public Action<ComparisonConditions<Outbox.Model.OutboxQueue>>? Conditions { get; }
		public Dictionary<object, HashSet<object>>? Cache { get; }
	
		public OutboxQueueEqualityComparer(
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<Outbox.Model.OutboxQueue>>? conditions = null,
			Dictionary<object, HashSet<object>>? cache = null)
		{
			Cache = cache ?? [];
			ComparisonOptions = comparisonOptions;
			Conditions = conditions;
		}
	
		public bool Equals(
			Outbox.Model.OutboxQueue? obj1,
			Outbox.Model.OutboxQueue? obj2)
			=> EqualsTo(
				obj1,
				obj2,
				ComparisonOptions,
				Conditions,
				Cache);
	
		public int GetHashCode([DisallowNull] Outbox.Model.OutboxQueue? obj)
			=> GetHashCode(
				obj,
				ComparisonOptions,
				Conditions,
				cache: null);
	}
}
