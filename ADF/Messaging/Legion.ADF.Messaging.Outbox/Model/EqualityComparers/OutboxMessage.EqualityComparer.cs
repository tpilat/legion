using Legion;
using Legion.Model.Comparers;
using System.Diagnostics.CodeAnalysis;

namespace Legion.ADF.Messaging.Outbox.Model;

public sealed partial class OutboxMessage : Outbox.OutboxBaseEntity, Legion.Model.IEntity
{
	public bool EqualsTo(
		Outbox.Model.OutboxMessage? obj,
		ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
		Action<ComparisonConditions<Outbox.Model.OutboxMessage>>? conditions = null)
		=> OutboxMessageEqualityComparer.EqualsTo(
			this,
			obj,
			comparisonOptions,
			conditions,
			cache: null);

	public sealed partial class OutboxMessageEqualityComparer : IEqualityComparer<OutboxMessage>
	{
		public static bool EqualsTo(
			Outbox.Model.OutboxMessage? obj1,
			Outbox.Model.OutboxMessage? obj2,
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<Outbox.Model.OutboxMessage>>? conditions = null,
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
			
			ComparisonConditions<Outbox.Model.OutboxMessage>? conds = null;
		
			if ((ComparisonOptions.CompareProperties & comparisonOptions) == ComparisonOptions.CompareProperties)
			{
				if (conditions != null)
				{
					conds = new ComparisonConditions<Outbox.Model.OutboxMessage>();
					conditions.Invoke(conds);

					if (conds.CanCompare(obj1, nameof(obj1.IdOutboxMessage)) && obj1.IdOutboxMessage != obj2.IdOutboxMessage)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.IdMessageType)) && obj1.IdMessageType != obj2.IdMessageType)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.IdOutboxMessageStatus)) && obj1.IdOutboxMessageStatus != obj2.IdOutboxMessageStatus)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.IdMessageContent)) && obj1.IdMessageContent != obj2.IdMessageContent)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.IdOutboxQueue)) && obj1.IdOutboxQueue != obj2.IdOutboxQueue)
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
					if (conds.CanCompare(obj1, nameof(obj1.IdOutboxInstance)) && obj1.IdOutboxInstance != obj2.IdOutboxInstance)
						return false;
				}
				else
				{
					if (obj1.IdOutboxMessage != obj2.IdOutboxMessage)
						return false;
					if (obj1.IdMessageType != obj2.IdMessageType)
						return false;
					if (obj1.IdOutboxMessageStatus != obj2.IdOutboxMessageStatus)
						return false;
					if (obj1.IdMessageContent != obj2.IdMessageContent)
						return false;
					if (obj1.IdOutboxQueue != obj2.IdOutboxQueue)
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
				if (!OutboxMessageContent.OutboxMessageContentEqualityComparer.EqualsTo(obj1.MessageContent, obj2.MessageContent, comparisonOptions, conds?.GetConditions(x => x.MessageContent), cache))
					return false;
				if (!OutboxMessageType.OutboxMessageTypeEqualityComparer.EqualsTo(obj1.MessageType, obj2.MessageType, comparisonOptions, conds?.GetConditions(x => x.MessageType), cache))
					return false;
				if (!OutboxInstance.OutboxInstanceEqualityComparer.EqualsTo(obj1.OutboxInstance, obj2.OutboxInstance, comparisonOptions, conds?.GetConditions(x => x.OutboxInstance), cache))
					return false;
				if (!OutboxMessageStatus.OutboxMessageStatusEqualityComparer.EqualsTo(obj1.OutboxMessageStatus, obj2.OutboxMessageStatus, comparisonOptions, conds?.GetConditions(x => x.OutboxMessageStatus), cache))
					return false;
				if (!OutboxQueue.OutboxQueueEqualityComparer.EqualsTo(obj1.OutboxQueue, obj2.OutboxQueue, comparisonOptions, conds?.GetConditions(x => x.OutboxQueue), cache))
					return false;
		}

			return true;
		}

		public static int GetHashCode(
			Outbox.Model.OutboxMessage? obj,
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<Outbox.Model.OutboxMessage>>? conditions = null,
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
		public Action<ComparisonConditions<Outbox.Model.OutboxMessage>>? Conditions { get; }
		public Dictionary<object, HashSet<object>>? Cache { get; }
	
		public OutboxMessageEqualityComparer(
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<Outbox.Model.OutboxMessage>>? conditions = null,
			Dictionary<object, HashSet<object>>? cache = null)
		{
			Cache = cache ?? [];
			ComparisonOptions = comparisonOptions;
			Conditions = conditions;
		}
	
		public bool Equals(
			Outbox.Model.OutboxMessage? obj1,
			Outbox.Model.OutboxMessage? obj2)
			=> EqualsTo(
				obj1,
				obj2,
				ComparisonOptions,
				Conditions,
				Cache);
	
		public int GetHashCode([DisallowNull] Outbox.Model.OutboxMessage? obj)
			=> GetHashCode(
				obj,
				ComparisonOptions,
				Conditions,
				cache: null);
	}
}
