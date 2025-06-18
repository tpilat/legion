using Legion;
using Legion.Model.Comparers;
using System.Diagnostics.CodeAnalysis;

namespace Legion.ADF.Messaging.MessageBox.Model;

public sealed partial class MessageType : MessageBox.MessageBoxBaseEntity, Legion.Model.IEntity
{
	public bool EqualsTo(
		MessageBox.Model.MessageType? obj,
		ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
		Action<ComparisonConditions<MessageBox.Model.MessageType>>? conditions = null)
		=> MessageTypeEqualityComparer.EqualsTo(
			this,
			obj,
			comparisonOptions,
			conditions,
			cache: null);

	public sealed partial class MessageTypeEqualityComparer : IEqualityComparer<MessageType>
	{
		public static bool EqualsTo(
			MessageBox.Model.MessageType? obj1,
			MessageBox.Model.MessageType? obj2,
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<MessageBox.Model.MessageType>>? conditions = null,
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
			
			ComparisonConditions<MessageBox.Model.MessageType>? conds = null;
		
			if ((ComparisonOptions.CompareProperties & comparisonOptions) == ComparisonOptions.CompareProperties)
			{
				if (conditions != null)
				{
					conds = new ComparisonConditions<MessageBox.Model.MessageType>();
					conditions.Invoke(conds);

					if (conds.CanCompare(obj1, nameof(obj1.IdMessageType)) && obj1.IdMessageType != obj2.IdMessageType)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.Code)) && !string.Equals(obj1.Code, obj2.Code))
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.Name)) && !string.Equals(obj1.Name, obj2.Name))
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.Namespace)) && !string.Equals(obj1.Namespace, obj2.Namespace))
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.CreatedUtc)) && obj1.CreatedUtc != obj2.CreatedUtc)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.IdMessageBoxInstance)) && obj1.IdMessageBoxInstance != obj2.IdMessageBoxInstance)
						return false;
				}
				else
				{
					if (obj1.IdMessageType != obj2.IdMessageType)
						return false;
					if (!string.Equals(obj1.Code, obj2.Code))
						return false;
					if (!string.Equals(obj1.Name, obj2.Name))
						return false;
					if (!string.Equals(obj1.Namespace, obj2.Namespace))
						return false;
					if (obj1.CreatedUtc != obj2.CreatedUtc)
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
				if (!ComparisonHelper.SequenceEqual(obj1.MessageArchives, obj2.MessageArchives, new MessageArchive.MessageArchiveEqualityComparer(comparisonOptions, conds?.GetConditions(x => x.MessageArchives), cache)))
					return false;
				if (!ComparisonHelper.SequenceEqual(obj1.Messages, obj2.Messages, new Message.MessageEqualityComparer(comparisonOptions, conds?.GetConditions(x => x.Messages), cache)))
					return false;
				if (!ComparisonHelper.SequenceEqual(obj1.Queues, obj2.Queues, new Queue.QueueEqualityComparer(comparisonOptions, conds?.GetConditions(x => x.Queues), cache)))
					return false;
		}

			return true;
		}

		public static int GetHashCode(
			MessageBox.Model.MessageType? obj,
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<MessageBox.Model.MessageType>>? conditions = null,
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
		public Action<ComparisonConditions<MessageBox.Model.MessageType>>? Conditions { get; }
		public Dictionary<object, HashSet<object>>? Cache { get; }
	
		public MessageTypeEqualityComparer(
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<MessageBox.Model.MessageType>>? conditions = null,
			Dictionary<object, HashSet<object>>? cache = null)
		{
			Cache = cache ?? [];
			ComparisonOptions = comparisonOptions;
			Conditions = conditions;
		}
	
		public bool Equals(
			MessageBox.Model.MessageType? obj1,
			MessageBox.Model.MessageType? obj2)
			=> EqualsTo(
				obj1,
				obj2,
				ComparisonOptions,
				Conditions,
				Cache);
	
		public int GetHashCode([DisallowNull] MessageBox.Model.MessageType? obj)
			=> GetHashCode(
				obj,
				ComparisonOptions,
				Conditions,
				cache: null);
	}
}
