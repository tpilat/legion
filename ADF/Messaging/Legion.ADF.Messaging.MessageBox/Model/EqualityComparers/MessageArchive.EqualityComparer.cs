using Legion;
using Legion.Model.Comparers;
using System.Diagnostics.CodeAnalysis;

namespace Legion.ADF.Messaging.MessageBox.Model;

public sealed partial class MessageArchive : MessageBox.MessageBoxBaseEntity, Legion.Model.IEntity
{
	public bool EqualsTo(
		MessageBox.Model.MessageArchive? obj,
		ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
		Action<ComparisonConditions<MessageBox.Model.MessageArchive>>? conditions = null)
		=> MessageArchiveEqualityComparer.EqualsTo(
			this,
			obj,
			comparisonOptions,
			conditions,
			cache: null);

	public sealed partial class MessageArchiveEqualityComparer : IEqualityComparer<MessageArchive>
	{
		public static bool EqualsTo(
			MessageBox.Model.MessageArchive? obj1,
			MessageBox.Model.MessageArchive? obj2,
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<MessageBox.Model.MessageArchive>>? conditions = null,
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
			
			ComparisonConditions<MessageBox.Model.MessageArchive>? conds = null;
		
			if ((ComparisonOptions.CompareProperties & comparisonOptions) == ComparisonOptions.CompareProperties)
			{
				if (conditions != null)
				{
					conds = new ComparisonConditions<MessageBox.Model.MessageArchive>();
					conditions.Invoke(conds);

					if (conds.CanCompare(obj1, nameof(obj1.IdMessage)) && obj1.IdMessage != obj2.IdMessage)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.IdMessageType)) && obj1.IdMessageType != obj2.IdMessageType)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.IdMessageStatus)) && obj1.IdMessageStatus != obj2.IdMessageStatus)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.IdMessageContent)) && obj1.IdMessageContent != obj2.IdMessageContent)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.IdQueue)) && obj1.IdQueue != obj2.IdQueue)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.IdTopic)) && obj1.IdTopic != obj2.IdTopic)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.CreatedUtc)) && obj1.CreatedUtc != obj2.CreatedUtc)
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
					if (conds.CanCompare(obj1, nameof(obj1.ValidToUtc)) && obj1.ValidToUtc != obj2.ValidToUtc)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.Priority)) && obj1.Priority != obj2.Priority)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.IdMessageBoxInstance)) && obj1.IdMessageBoxInstance != obj2.IdMessageBoxInstance)
						return false;
				}
				else
				{
					if (obj1.IdMessage != obj2.IdMessage)
						return false;
					if (obj1.IdMessageType != obj2.IdMessageType)
						return false;
					if (obj1.IdMessageStatus != obj2.IdMessageStatus)
						return false;
					if (obj1.IdMessageContent != obj2.IdMessageContent)
						return false;
					if (obj1.IdQueue != obj2.IdQueue)
						return false;
					if (obj1.IdTopic != obj2.IdTopic)
						return false;
					if (obj1.CreatedUtc != obj2.CreatedUtc)
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
					if (obj1.ValidToUtc != obj2.ValidToUtc)
						return false;
					if (obj1.Priority != obj2.Priority)
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
				if (!MessageContent.MessageContentEqualityComparer.EqualsTo(obj1.MessageContent, obj2.MessageContent, comparisonOptions, conds?.GetConditions(x => x.MessageContent), cache))
					return false;
				if (!MessageStatus.MessageStatusEqualityComparer.EqualsTo(obj1.MessageStatus, obj2.MessageStatus, comparisonOptions, conds?.GetConditions(x => x.MessageStatus), cache))
					return false;
				if (!MessageType.MessageTypeEqualityComparer.EqualsTo(obj1.MessageType, obj2.MessageType, comparisonOptions, conds?.GetConditions(x => x.MessageType), cache))
					return false;
				if (!Queue.QueueEqualityComparer.EqualsTo(obj1.Queue, obj2.Queue, comparisonOptions, conds?.GetConditions(x => x.Queue), cache))
					return false;
				if (!Topic.TopicEqualityComparer.EqualsTo(obj1.Topic, obj2.Topic, comparisonOptions, conds?.GetConditions(x => x.Topic), cache))
					return false;
		}

			return true;
		}

		public static int GetHashCode(
			MessageBox.Model.MessageArchive? obj,
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<MessageBox.Model.MessageArchive>>? conditions = null,
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
		public Action<ComparisonConditions<MessageBox.Model.MessageArchive>>? Conditions { get; }
		public Dictionary<object, HashSet<object>>? Cache { get; }
	
		public MessageArchiveEqualityComparer(
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<MessageBox.Model.MessageArchive>>? conditions = null,
			Dictionary<object, HashSet<object>>? cache = null)
		{
			Cache = cache ?? [];
			ComparisonOptions = comparisonOptions;
			Conditions = conditions;
		}
	
		public bool Equals(
			MessageBox.Model.MessageArchive? obj1,
			MessageBox.Model.MessageArchive? obj2)
			=> EqualsTo(
				obj1,
				obj2,
				ComparisonOptions,
				Conditions,
				Cache);
	
		public int GetHashCode([DisallowNull] MessageBox.Model.MessageArchive? obj)
			=> GetHashCode(
				obj,
				ComparisonOptions,
				Conditions,
				cache: null);
	}
}
