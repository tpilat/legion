using Legion;
using Legion.Model.Comparers;
using System.Diagnostics.CodeAnalysis;

namespace Legion.ADF.Messaging.MessageBox.Model;

public sealed partial class MessageBoxProcessingLog : MessageBox.MessageBoxBaseEntity, Legion.Model.IEntity
{
	public bool EqualsTo(
		MessageBox.Model.MessageBoxProcessingLog? obj,
		ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
		Action<ComparisonConditions<MessageBox.Model.MessageBoxProcessingLog>>? conditions = null)
		=> MessageBoxProcessingLogEqualityComparer.EqualsTo(
			this,
			obj,
			comparisonOptions,
			conditions,
			cache: null);

	public sealed partial class MessageBoxProcessingLogEqualityComparer : IEqualityComparer<MessageBoxProcessingLog>
	{
		public static bool EqualsTo(
			MessageBox.Model.MessageBoxProcessingLog? obj1,
			MessageBox.Model.MessageBoxProcessingLog? obj2,
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<MessageBox.Model.MessageBoxProcessingLog>>? conditions = null,
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
			
			ComparisonConditions<MessageBox.Model.MessageBoxProcessingLog>? conds = null;
		
			if ((ComparisonOptions.CompareProperties & comparisonOptions) == ComparisonOptions.CompareProperties)
			{
				if (conditions != null)
				{
					conds = new ComparisonConditions<MessageBox.Model.MessageBoxProcessingLog>();
					conditions.Invoke(conds);

					if (conds.CanCompare(obj1, nameof(obj1.IdMessageBoxProcessingLog)) && obj1.IdMessageBoxProcessingLog != obj2.IdMessageBoxProcessingLog)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.IdMessageBoxInstance)) && obj1.IdMessageBoxInstance != obj2.IdMessageBoxInstance)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.IdQueue)) && obj1.IdQueue != obj2.IdQueue)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.IdTopic)) && obj1.IdTopic != obj2.IdTopic)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.IdTopicSubscription)) && obj1.IdTopicSubscription != obj2.IdTopicSubscription)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.CreatedUtc)) && obj1.CreatedUtc != obj2.CreatedUtc)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.IdLogLevel)) && obj1.IdLogLevel != obj2.IdLogLevel)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.TraceCorrelationId)) && obj1.TraceCorrelationId != obj2.TraceCorrelationId)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.IdLogMessage)) && obj1.IdLogMessage != obj2.IdLogMessage)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.Code)) && !string.Equals(obj1.Code, obj2.Code))
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.Detail)) && !string.Equals(obj1.Detail, obj2.Detail))
						return false;
				}
				else
				{
					if (obj1.IdMessageBoxProcessingLog != obj2.IdMessageBoxProcessingLog)
						return false;
					if (obj1.IdMessageBoxInstance != obj2.IdMessageBoxInstance)
						return false;
					if (obj1.IdQueue != obj2.IdQueue)
						return false;
					if (obj1.IdTopic != obj2.IdTopic)
						return false;
					if (obj1.IdTopicSubscription != obj2.IdTopicSubscription)
						return false;
					if (obj1.CreatedUtc != obj2.CreatedUtc)
						return false;
					if (obj1.IdLogLevel != obj2.IdLogLevel)
						return false;
					if (obj1.TraceCorrelationId != obj2.TraceCorrelationId)
						return false;
					if (obj1.IdLogMessage != obj2.IdLogMessage)
						return false;
					if (!string.Equals(obj1.Code, obj2.Code))
						return false;
					if (!string.Equals(obj1.Detail, obj2.Detail))
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
				if (!Queue.QueueEqualityComparer.EqualsTo(obj1.Queue, obj2.Queue, comparisonOptions, conds?.GetConditions(x => x.Queue), cache))
					return false;
				if (!Topic.TopicEqualityComparer.EqualsTo(obj1.Topic, obj2.Topic, comparisonOptions, conds?.GetConditions(x => x.Topic), cache))
					return false;
				if (!TopicSubscription.TopicSubscriptionEqualityComparer.EqualsTo(obj1.TopicSubscription, obj2.TopicSubscription, comparisonOptions, conds?.GetConditions(x => x.TopicSubscription), cache))
					return false;
		}

			return true;
		}

		public static int GetHashCode(
			MessageBox.Model.MessageBoxProcessingLog? obj,
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<MessageBox.Model.MessageBoxProcessingLog>>? conditions = null,
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
		public Action<ComparisonConditions<MessageBox.Model.MessageBoxProcessingLog>>? Conditions { get; }
		public Dictionary<object, HashSet<object>>? Cache { get; }
	
		public MessageBoxProcessingLogEqualityComparer(
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<MessageBox.Model.MessageBoxProcessingLog>>? conditions = null,
			Dictionary<object, HashSet<object>>? cache = null)
		{
			Cache = cache ?? [];
			ComparisonOptions = comparisonOptions;
			Conditions = conditions;
		}
	
		public bool Equals(
			MessageBox.Model.MessageBoxProcessingLog? obj1,
			MessageBox.Model.MessageBoxProcessingLog? obj2)
			=> EqualsTo(
				obj1,
				obj2,
				ComparisonOptions,
				Conditions,
				Cache);
	
		public int GetHashCode([DisallowNull] MessageBox.Model.MessageBoxProcessingLog? obj)
			=> GetHashCode(
				obj,
				ComparisonOptions,
				Conditions,
				cache: null);
	}
}
