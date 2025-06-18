using Legion;
using Legion.Model.Comparers;
using System.Diagnostics.CodeAnalysis;

namespace Legion.ADF.Messaging.MessageBox.Model;

public sealed partial class QueueProcessingMode : MessageBox.MessageBoxBaseEntity, Legion.Model.IEntity
{
	public bool EqualsTo(
		MessageBox.Model.QueueProcessingMode? obj,
		ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
		Action<ComparisonConditions<MessageBox.Model.QueueProcessingMode>>? conditions = null)
		=> QueueProcessingModeEqualityComparer.EqualsTo(
			this,
			obj,
			comparisonOptions,
			conditions,
			cache: null);

	public sealed partial class QueueProcessingModeEqualityComparer : IEqualityComparer<QueueProcessingMode>
	{
		public static bool EqualsTo(
			MessageBox.Model.QueueProcessingMode? obj1,
			MessageBox.Model.QueueProcessingMode? obj2,
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<MessageBox.Model.QueueProcessingMode>>? conditions = null,
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
			
			ComparisonConditions<MessageBox.Model.QueueProcessingMode>? conds = null;
		
			if ((ComparisonOptions.CompareProperties & comparisonOptions) == ComparisonOptions.CompareProperties)
			{
				if (conditions != null)
				{
					conds = new ComparisonConditions<MessageBox.Model.QueueProcessingMode>();
					conditions.Invoke(conds);
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
				if (!ComparisonHelper.SequenceEqual(obj1.Queues, obj2.Queues, new Queue.QueueEqualityComparer(comparisonOptions, conds?.GetConditions(x => x.Queues), cache)))
					return false;
				if (!ComparisonHelper.SequenceEqual(obj1.SuspendingModeQueues, obj2.SuspendingModeQueues, new Queue.QueueEqualityComparer(comparisonOptions, conds?.GetConditions(x => x.SuspendingModeQueues), cache)))
					return false;
				if (!ComparisonHelper.SequenceEqual(obj1.SuspendingModeTopics, obj2.SuspendingModeTopics, new Topic.TopicEqualityComparer(comparisonOptions, conds?.GetConditions(x => x.SuspendingModeTopics), cache)))
					return false;
				if (!ComparisonHelper.SequenceEqual(obj1.SuspendingModeTopicSubscriptions, obj2.SuspendingModeTopicSubscriptions, new TopicSubscription.TopicSubscriptionEqualityComparer(comparisonOptions, conds?.GetConditions(x => x.SuspendingModeTopicSubscriptions), cache)))
					return false;
				if (!ComparisonHelper.SequenceEqual(obj1.Topics, obj2.Topics, new Topic.TopicEqualityComparer(comparisonOptions, conds?.GetConditions(x => x.Topics), cache)))
					return false;
				if (!ComparisonHelper.SequenceEqual(obj1.TopicSubscriptions, obj2.TopicSubscriptions, new TopicSubscription.TopicSubscriptionEqualityComparer(comparisonOptions, conds?.GetConditions(x => x.TopicSubscriptions), cache)))
					return false;
		}

			return true;
		}

		public static int GetHashCode(
			MessageBox.Model.QueueProcessingMode? obj,
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<MessageBox.Model.QueueProcessingMode>>? conditions = null,
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
		public Action<ComparisonConditions<MessageBox.Model.QueueProcessingMode>>? Conditions { get; }
		public Dictionary<object, HashSet<object>>? Cache { get; }
	
		public QueueProcessingModeEqualityComparer(
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<MessageBox.Model.QueueProcessingMode>>? conditions = null,
			Dictionary<object, HashSet<object>>? cache = null)
		{
			Cache = cache ?? [];
			ComparisonOptions = comparisonOptions;
			Conditions = conditions;
		}
	
		public bool Equals(
			MessageBox.Model.QueueProcessingMode? obj1,
			MessageBox.Model.QueueProcessingMode? obj2)
			=> EqualsTo(
				obj1,
				obj2,
				ComparisonOptions,
				Conditions,
				Cache);
	
		public int GetHashCode([DisallowNull] MessageBox.Model.QueueProcessingMode? obj)
			=> GetHashCode(
				obj,
				ComparisonOptions,
				Conditions,
				cache: null);
	}
}
