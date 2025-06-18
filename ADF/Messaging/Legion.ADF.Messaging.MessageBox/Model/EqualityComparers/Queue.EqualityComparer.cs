using Legion;
using Legion.Model.Comparers;
using System.Diagnostics.CodeAnalysis;

namespace Legion.ADF.Messaging.MessageBox.Model;

public sealed partial class Queue : MessageBox.MessageBoxBaseEntity, Legion.Model.IEntity
{
	public bool EqualsTo(
		MessageBox.Model.Queue? obj,
		ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
		Action<ComparisonConditions<MessageBox.Model.Queue>>? conditions = null)
		=> QueueEqualityComparer.EqualsTo(
			this,
			obj,
			comparisonOptions,
			conditions,
			cache: null);

	public sealed partial class QueueEqualityComparer : IEqualityComparer<Queue>
	{
		public static bool EqualsTo(
			MessageBox.Model.Queue? obj1,
			MessageBox.Model.Queue? obj2,
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<MessageBox.Model.Queue>>? conditions = null,
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
			
			ComparisonConditions<MessageBox.Model.Queue>? conds = null;
		
			if ((ComparisonOptions.CompareProperties & comparisonOptions) == ComparisonOptions.CompareProperties)
			{
				if (conditions != null)
				{
					conds = new ComparisonConditions<MessageBox.Model.Queue>();
					conditions.Invoke(conds);

					if (conds.CanCompare(obj1, nameof(obj1.IdQueue)) && obj1.IdQueue != obj2.IdQueue)
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
					if (conds.CanCompare(obj1, nameof(obj1.IdJob)) && obj1.IdJob != obj2.IdJob)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.IdOrchestration)) && obj1.IdOrchestration != obj2.IdOrchestration)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.IdMessageBoxInstance)) && obj1.IdMessageBoxInstance != obj2.IdMessageBoxInstance)
						return false;
				}
				else
				{
					if (obj1.IdQueue != obj2.IdQueue)
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
					if (obj1.IdJob != obj2.IdJob)
						return false;
					if (obj1.IdOrchestration != obj2.IdOrchestration)
						return false;
					if (obj1.IdMessageBoxInstance != obj2.IdMessageBoxInstance)
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
				if (!MessageBoxInstance.MessageBoxInstanceEqualityComparer.EqualsTo(obj1.MessageBoxInstance, obj2.MessageBoxInstance, comparisonOptions, conds?.GetConditions(x => x.MessageBoxInstance), cache))
					return false;
				if (!MessageType.MessageTypeEqualityComparer.EqualsTo(obj1.MessageType, obj2.MessageType, comparisonOptions, conds?.GetConditions(x => x.MessageType), cache))
					return false;
				if (!QueueProcessingMode.QueueProcessingModeEqualityComparer.EqualsTo(obj1.ProcessingMode, obj2.ProcessingMode, comparisonOptions, conds?.GetConditions(x => x.ProcessingMode), cache))
					return false;
				if (!QueueProcessingMode.QueueProcessingModeEqualityComparer.EqualsTo(obj1.SuspendingMode, obj2.SuspendingMode, comparisonOptions, conds?.GetConditions(x => x.SuspendingMode), cache))
					return false;
				if (!ComparisonHelper.SequenceEqual(obj1.MessageArchives, obj2.MessageArchives, new MessageArchive.MessageArchiveEqualityComparer(comparisonOptions, conds?.GetConditions(x => x.MessageArchives), cache)))
					return false;
				if (!ComparisonHelper.SequenceEqual(obj1.MessageBoxProcessingLogs, obj2.MessageBoxProcessingLogs, new MessageBoxProcessingLog.MessageBoxProcessingLogEqualityComparer(comparisonOptions, conds?.GetConditions(x => x.MessageBoxProcessingLogs), cache)))
					return false;
				if (!ComparisonHelper.SequenceEqual(obj1.Messages, obj2.Messages, new Message.MessageEqualityComparer(comparisonOptions, conds?.GetConditions(x => x.Messages), cache)))
					return false;
				if (!ComparisonHelper.SequenceEqual(obj1.QueuedMessages, obj2.QueuedMessages, new QueuedMessage.QueuedMessageEqualityComparer(comparisonOptions, conds?.GetConditions(x => x.QueuedMessages), cache)))
					return false;
		}

			return true;
		}

		public static int GetHashCode(
			MessageBox.Model.Queue? obj,
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<MessageBox.Model.Queue>>? conditions = null,
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
		public Action<ComparisonConditions<MessageBox.Model.Queue>>? Conditions { get; }
		public Dictionary<object, HashSet<object>>? Cache { get; }
	
		public QueueEqualityComparer(
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<MessageBox.Model.Queue>>? conditions = null,
			Dictionary<object, HashSet<object>>? cache = null)
		{
			Cache = cache ?? [];
			ComparisonOptions = comparisonOptions;
			Conditions = conditions;
		}
	
		public bool Equals(
			MessageBox.Model.Queue? obj1,
			MessageBox.Model.Queue? obj2)
			=> EqualsTo(
				obj1,
				obj2,
				ComparisonOptions,
				Conditions,
				Cache);
	
		public int GetHashCode([DisallowNull] MessageBox.Model.Queue? obj)
			=> GetHashCode(
				obj,
				ComparisonOptions,
				Conditions,
				cache: null);
	}
}
