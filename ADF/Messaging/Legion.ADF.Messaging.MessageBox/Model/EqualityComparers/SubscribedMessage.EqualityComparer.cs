using Legion;
using Legion.Model.Comparers;
using System.Diagnostics.CodeAnalysis;

namespace Legion.ADF.Messaging.MessageBox.Model;

public sealed partial class SubscribedMessage : MessageBox.MessageBoxBaseEntity, Legion.Model.IEntity
{
	public bool EqualsTo(
		MessageBox.Model.SubscribedMessage? obj,
		ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
		Action<ComparisonConditions<MessageBox.Model.SubscribedMessage>>? conditions = null)
		=> SubscribedMessageEqualityComparer.EqualsTo(
			this,
			obj,
			comparisonOptions,
			conditions,
			cache: null);

	public sealed partial class SubscribedMessageEqualityComparer : IEqualityComparer<SubscribedMessage>
	{
		public static bool EqualsTo(
			MessageBox.Model.SubscribedMessage? obj1,
			MessageBox.Model.SubscribedMessage? obj2,
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<MessageBox.Model.SubscribedMessage>>? conditions = null,
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
			
			ComparisonConditions<MessageBox.Model.SubscribedMessage>? conds = null;
		
			if ((ComparisonOptions.CompareProperties & comparisonOptions) == ComparisonOptions.CompareProperties)
			{
				if (conditions != null)
				{
					conds = new ComparisonConditions<MessageBox.Model.SubscribedMessage>();
					conditions.Invoke(conds);

					if (conds.CanCompare(obj1, nameof(obj1.IdSubscribedMessage)) && obj1.IdSubscribedMessage != obj2.IdSubscribedMessage)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.IdTopicSubscription)) && obj1.IdTopicSubscription != obj2.IdTopicSubscription)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.IdMessage)) && obj1.IdMessage != obj2.IdMessage)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.IdMessageProcessingStatus)) && obj1.IdMessageProcessingStatus != obj2.IdMessageProcessingStatus)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.AssignedUtc)) && obj1.AssignedUtc != obj2.AssignedUtc)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.ProcessedUtc)) && obj1.ProcessedUtc != obj2.ProcessedUtc)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.SuspendedUtc)) && obj1.SuspendedUtc != obj2.SuspendedUtc)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.LastProcessingUtc)) && obj1.LastProcessingUtc != obj2.LastProcessingUtc)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.LastProcessingTimeoutUtc)) && obj1.LastProcessingTimeoutUtc != obj2.LastProcessingTimeoutUtc)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.NextProcessingUtc)) && obj1.NextProcessingUtc != obj2.NextProcessingUtc)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.RetryCount)) && obj1.RetryCount != obj2.RetryCount)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.IdMessageBoxInstance)) && obj1.IdMessageBoxInstance != obj2.IdMessageBoxInstance)
						return false;
				}
				else
				{
					if (obj1.IdSubscribedMessage != obj2.IdSubscribedMessage)
						return false;
					if (obj1.IdTopicSubscription != obj2.IdTopicSubscription)
						return false;
					if (obj1.IdMessage != obj2.IdMessage)
						return false;
					if (obj1.IdMessageProcessingStatus != obj2.IdMessageProcessingStatus)
						return false;
					if (obj1.AssignedUtc != obj2.AssignedUtc)
						return false;
					if (obj1.ProcessedUtc != obj2.ProcessedUtc)
						return false;
					if (obj1.SuspendedUtc != obj2.SuspendedUtc)
						return false;
					if (obj1.LastProcessingUtc != obj2.LastProcessingUtc)
						return false;
					if (obj1.LastProcessingTimeoutUtc != obj2.LastProcessingTimeoutUtc)
						return false;
					if (obj1.NextProcessingUtc != obj2.NextProcessingUtc)
						return false;
					if (obj1.RetryCount != obj2.RetryCount)
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
				if (!MessageProcessingStatus.MessageProcessingStatusEqualityComparer.EqualsTo(obj1.MessageProcessingStatus, obj2.MessageProcessingStatus, comparisonOptions, conds?.GetConditions(x => x.MessageProcessingStatus), cache))
					return false;
				if (!TopicSubscription.TopicSubscriptionEqualityComparer.EqualsTo(obj1.TopicSubscription, obj2.TopicSubscription, comparisonOptions, conds?.GetConditions(x => x.TopicSubscription), cache))
					return false;
				if (!ComparisonHelper.SequenceEqual(obj1.MessageProcessingLogs, obj2.MessageProcessingLogs, new MessageProcessingLog.MessageProcessingLogEqualityComparer(comparisonOptions, conds?.GetConditions(x => x.MessageProcessingLogs), cache)))
					return false;
		}

			return true;
		}

		public static int GetHashCode(
			MessageBox.Model.SubscribedMessage? obj,
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<MessageBox.Model.SubscribedMessage>>? conditions = null,
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
		public Action<ComparisonConditions<MessageBox.Model.SubscribedMessage>>? Conditions { get; }
		public Dictionary<object, HashSet<object>>? Cache { get; }
	
		public SubscribedMessageEqualityComparer(
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<MessageBox.Model.SubscribedMessage>>? conditions = null,
			Dictionary<object, HashSet<object>>? cache = null)
		{
			Cache = cache ?? [];
			ComparisonOptions = comparisonOptions;
			Conditions = conditions;
		}
	
		public bool Equals(
			MessageBox.Model.SubscribedMessage? obj1,
			MessageBox.Model.SubscribedMessage? obj2)
			=> EqualsTo(
				obj1,
				obj2,
				ComparisonOptions,
				Conditions,
				Cache);
	
		public int GetHashCode([DisallowNull] MessageBox.Model.SubscribedMessage? obj)
			=> GetHashCode(
				obj,
				ComparisonOptions,
				Conditions,
				cache: null);
	}
}
