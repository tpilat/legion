using Legion;
using Legion.Model.Comparers;
using System.Diagnostics.CodeAnalysis;

namespace Legion.ADF.Messaging.Inbox.Model;

public sealed partial class InboxMessage : Inbox.InboxBaseEntity, Legion.Model.IEntity
{
	public bool EqualsTo(
		Inbox.Model.InboxMessage? obj,
		ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
		Action<ComparisonConditions<Inbox.Model.InboxMessage>>? conditions = null)
		=> InboxMessageEqualityComparer.EqualsTo(
			this,
			obj,
			comparisonOptions,
			conditions,
			cache: null);

	public sealed partial class InboxMessageEqualityComparer : IEqualityComparer<InboxMessage>
	{
		public static bool EqualsTo(
			Inbox.Model.InboxMessage? obj1,
			Inbox.Model.InboxMessage? obj2,
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<Inbox.Model.InboxMessage>>? conditions = null,
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
			
			ComparisonConditions<Inbox.Model.InboxMessage>? conds = null;
		
			if ((ComparisonOptions.CompareProperties & comparisonOptions) == ComparisonOptions.CompareProperties)
			{
				if (conditions != null)
				{
					conds = new ComparisonConditions<Inbox.Model.InboxMessage>();
					conditions.Invoke(conds);

					if (conds.CanCompare(obj1, nameof(obj1.IdInboxMessage)) && obj1.IdInboxMessage != obj2.IdInboxMessage)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.IdMessageType)) && obj1.IdMessageType != obj2.IdMessageType)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.IdInboxMessageStatus)) && obj1.IdInboxMessageStatus != obj2.IdInboxMessageStatus)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.IdMessageContent)) && obj1.IdMessageContent != obj2.IdMessageContent)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.IdInboxQueue)) && obj1.IdInboxQueue != obj2.IdInboxQueue)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.MessageId)) && !string.Equals(obj1.MessageId, obj2.MessageId))
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.BusinessId)) && !string.Equals(obj1.BusinessId, obj2.BusinessId))
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.CorrelationId)) && !string.Equals(obj1.CorrelationId, obj2.CorrelationId))
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.SessionId)) && obj1.SessionId != obj2.SessionId)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.SessionMessagePartId)) && obj1.SessionMessagePartId != obj2.SessionMessagePartId)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.TraceCorrelationId)) && obj1.TraceCorrelationId != obj2.TraceCorrelationId)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.Properties)) && !string.Equals(obj1.Properties, obj2.Properties))
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.Publisher)) && !string.Equals(obj1.Publisher, obj2.Publisher))
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.PublisherId)) && !string.Equals(obj1.PublisherId, obj2.PublisherId))
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.CreatedUtc)) && obj1.CreatedUtc != obj2.CreatedUtc)
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
					if (conds.CanCompare(obj1, nameof(obj1.TargetTopic)) && !string.Equals(obj1.TargetTopic, obj2.TargetTopic))
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.TargetQueueName)) && !string.Equals(obj1.TargetQueueName, obj2.TargetQueueName))
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.IdInboxInstance)) && obj1.IdInboxInstance != obj2.IdInboxInstance)
						return false;
				}
				else
				{
					if (obj1.IdInboxMessage != obj2.IdInboxMessage)
						return false;
					if (obj1.IdMessageType != obj2.IdMessageType)
						return false;
					if (obj1.IdInboxMessageStatus != obj2.IdInboxMessageStatus)
						return false;
					if (obj1.IdMessageContent != obj2.IdMessageContent)
						return false;
					if (obj1.IdInboxQueue != obj2.IdInboxQueue)
						return false;
					if (!string.Equals(obj1.MessageId, obj2.MessageId))
						return false;
					if (!string.Equals(obj1.BusinessId, obj2.BusinessId))
						return false;
					if (!string.Equals(obj1.CorrelationId, obj2.CorrelationId))
						return false;
					if (obj1.SessionId != obj2.SessionId)
						return false;
					if (obj1.SessionMessagePartId != obj2.SessionMessagePartId)
						return false;
					if (obj1.TraceCorrelationId != obj2.TraceCorrelationId)
						return false;
					if (!string.Equals(obj1.Properties, obj2.Properties))
						return false;
					if (!string.Equals(obj1.Publisher, obj2.Publisher))
						return false;
					if (!string.Equals(obj1.PublisherId, obj2.PublisherId))
						return false;
					if (obj1.CreatedUtc != obj2.CreatedUtc)
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
					if (!string.Equals(obj1.TargetTopic, obj2.TargetTopic))
						return false;
					if (!string.Equals(obj1.TargetQueueName, obj2.TargetQueueName))
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
				if (!InboxMessageContent.InboxMessageContentEqualityComparer.EqualsTo(obj1.MessageContent, obj2.MessageContent, comparisonOptions, conds?.GetConditions(x => x.MessageContent), cache))
					return false;
				if (!InboxMessageType.InboxMessageTypeEqualityComparer.EqualsTo(obj1.MessageType, obj2.MessageType, comparisonOptions, conds?.GetConditions(x => x.MessageType), cache))
					return false;
		}

			return true;
		}

		public static int GetHashCode(
			Inbox.Model.InboxMessage? obj,
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<Inbox.Model.InboxMessage>>? conditions = null,
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
		public Action<ComparisonConditions<Inbox.Model.InboxMessage>>? Conditions { get; }
		public Dictionary<object, HashSet<object>>? Cache { get; }
	
		public InboxMessageEqualityComparer(
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<Inbox.Model.InboxMessage>>? conditions = null,
			Dictionary<object, HashSet<object>>? cache = null)
		{
			Cache = cache ?? [];
			ComparisonOptions = comparisonOptions;
			Conditions = conditions;
		}
	
		public bool Equals(
			Inbox.Model.InboxMessage? obj1,
			Inbox.Model.InboxMessage? obj2)
			=> EqualsTo(
				obj1,
				obj2,
				ComparisonOptions,
				Conditions,
				Cache);
	
		public int GetHashCode([DisallowNull] Inbox.Model.InboxMessage? obj)
			=> GetHashCode(
				obj,
				ComparisonOptions,
				Conditions,
				cache: null);
	}
}
