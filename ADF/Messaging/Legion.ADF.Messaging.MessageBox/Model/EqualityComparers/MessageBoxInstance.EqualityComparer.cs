using Legion;
using Legion.Model.Comparers;
using System.Diagnostics.CodeAnalysis;

namespace Legion.ADF.Messaging.MessageBox.Model;

public sealed partial class MessageBoxInstance : MessageBox.MessageBoxBaseEntity, Legion.Model.IEntity
{
	public bool EqualsTo(
		MessageBox.Model.MessageBoxInstance? obj,
		ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
		Action<ComparisonConditions<MessageBox.Model.MessageBoxInstance>>? conditions = null)
		=> MessageBoxInstanceEqualityComparer.EqualsTo(
			this,
			obj,
			comparisonOptions,
			conditions,
			cache: null);

	public sealed partial class MessageBoxInstanceEqualityComparer : IEqualityComparer<MessageBoxInstance>
	{
		public static bool EqualsTo(
			MessageBox.Model.MessageBoxInstance? obj1,
			MessageBox.Model.MessageBoxInstance? obj2,
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<MessageBox.Model.MessageBoxInstance>>? conditions = null,
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
			
			ComparisonConditions<MessageBox.Model.MessageBoxInstance>? conds = null;
		
			if ((ComparisonOptions.CompareProperties & comparisonOptions) == ComparisonOptions.CompareProperties)
			{
				if (conditions != null)
				{
					conds = new ComparisonConditions<MessageBox.Model.MessageBoxInstance>();
					conditions.Invoke(conds);

					if (conds.CanCompare(obj1, nameof(obj1.IdMessageBoxInstance)) && obj1.IdMessageBoxInstance != obj2.IdMessageBoxInstance)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.CreatedUtc)) && obj1.CreatedUtc != obj2.CreatedUtc)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.Name)) && !string.Equals(obj1.Name, obj2.Name))
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.Version)) && !string.Equals(obj1.Version, obj2.Version))
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.MaxDegreeOfQueueParallelism)) && obj1.MaxDegreeOfQueueParallelism != obj2.MaxDegreeOfQueueParallelism)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.MaxDegreeOfTopicParallelism)) && obj1.MaxDegreeOfTopicParallelism != obj2.MaxDegreeOfTopicParallelism)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.IdLogLevel)) && obj1.IdLogLevel != obj2.IdLogLevel)
						return false;
				}
				else
				{
					if (obj1.IdMessageBoxInstance != obj2.IdMessageBoxInstance)
						return false;
					if (obj1.CreatedUtc != obj2.CreatedUtc)
						return false;
					if (!string.Equals(obj1.Name, obj2.Name))
						return false;
					if (!string.Equals(obj1.Version, obj2.Version))
						return false;
					if (obj1.MaxDegreeOfQueueParallelism != obj2.MaxDegreeOfQueueParallelism)
						return false;
					if (obj1.MaxDegreeOfTopicParallelism != obj2.MaxDegreeOfTopicParallelism)
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
				if (!ComparisonHelper.SequenceEqual(obj1.BlockedMessageTypes, obj2.BlockedMessageTypes, new BlockedMessageType.BlockedMessageTypeEqualityComparer(comparisonOptions, conds?.GetConditions(x => x.BlockedMessageTypes), cache)))
					return false;
				if (!ComparisonHelper.SequenceEqual(obj1.MessageArchives, obj2.MessageArchives, new MessageArchive.MessageArchiveEqualityComparer(comparisonOptions, conds?.GetConditions(x => x.MessageArchives), cache)))
					return false;
				if (!ComparisonHelper.SequenceEqual(obj1.MessageBoxProcessingLogs, obj2.MessageBoxProcessingLogs, new MessageBoxProcessingLog.MessageBoxProcessingLogEqualityComparer(comparisonOptions, conds?.GetConditions(x => x.MessageBoxProcessingLogs), cache)))
					return false;
				if (!ComparisonHelper.SequenceEqual(obj1.MessageProcessingLogs, obj2.MessageProcessingLogs, new MessageProcessingLog.MessageProcessingLogEqualityComparer(comparisonOptions, conds?.GetConditions(x => x.MessageProcessingLogs), cache)))
					return false;
				if (!ComparisonHelper.SequenceEqual(obj1.Messages, obj2.Messages, new Message.MessageEqualityComparer(comparisonOptions, conds?.GetConditions(x => x.Messages), cache)))
					return false;
				if (!ComparisonHelper.SequenceEqual(obj1.MessageTypes, obj2.MessageTypes, new MessageType.MessageTypeEqualityComparer(comparisonOptions, conds?.GetConditions(x => x.MessageTypes), cache)))
					return false;
				if (!ComparisonHelper.SequenceEqual(obj1.QueuedMessages, obj2.QueuedMessages, new QueuedMessage.QueuedMessageEqualityComparer(comparisonOptions, conds?.GetConditions(x => x.QueuedMessages), cache)))
					return false;
				if (!ComparisonHelper.SequenceEqual(obj1.Queues, obj2.Queues, new Queue.QueueEqualityComparer(comparisonOptions, conds?.GetConditions(x => x.Queues), cache)))
					return false;
				if (!ComparisonHelper.SequenceEqual(obj1.SubscribedMessages, obj2.SubscribedMessages, new SubscribedMessage.SubscribedMessageEqualityComparer(comparisonOptions, conds?.GetConditions(x => x.SubscribedMessages), cache)))
					return false;
				if (!ComparisonHelper.SequenceEqual(obj1.Topics, obj2.Topics, new Topic.TopicEqualityComparer(comparisonOptions, conds?.GetConditions(x => x.Topics), cache)))
					return false;
				if (!ComparisonHelper.SequenceEqual(obj1.TopicSubscriptions, obj2.TopicSubscriptions, new TopicSubscription.TopicSubscriptionEqualityComparer(comparisonOptions, conds?.GetConditions(x => x.TopicSubscriptions), cache)))
					return false;
		}

			return true;
		}

		public static int GetHashCode(
			MessageBox.Model.MessageBoxInstance? obj,
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<MessageBox.Model.MessageBoxInstance>>? conditions = null,
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
		public Action<ComparisonConditions<MessageBox.Model.MessageBoxInstance>>? Conditions { get; }
		public Dictionary<object, HashSet<object>>? Cache { get; }
	
		public MessageBoxInstanceEqualityComparer(
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<MessageBox.Model.MessageBoxInstance>>? conditions = null,
			Dictionary<object, HashSet<object>>? cache = null)
		{
			Cache = cache ?? [];
			ComparisonOptions = comparisonOptions;
			Conditions = conditions;
		}
	
		public bool Equals(
			MessageBox.Model.MessageBoxInstance? obj1,
			MessageBox.Model.MessageBoxInstance? obj2)
			=> EqualsTo(
				obj1,
				obj2,
				ComparisonOptions,
				Conditions,
				Cache);
	
		public int GetHashCode([DisallowNull] MessageBox.Model.MessageBoxInstance? obj)
			=> GetHashCode(
				obj,
				ComparisonOptions,
				Conditions,
				cache: null);
	}
}
