using Legion;
using Legion.Model.Comparers;
using System.Diagnostics.CodeAnalysis;

namespace Legion.ADF.Messaging.Outbox.Model;

public sealed partial class OutboxMessageType : Outbox.OutboxBaseEntity, Legion.Model.IEntity
{
	public bool EqualsTo(
		Outbox.Model.OutboxMessageType? obj,
		ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
		Action<ComparisonConditions<Outbox.Model.OutboxMessageType>>? conditions = null)
		=> OutboxMessageTypeEqualityComparer.EqualsTo(
			this,
			obj,
			comparisonOptions,
			conditions,
			cache: null);

	public sealed partial class OutboxMessageTypeEqualityComparer : IEqualityComparer<OutboxMessageType>
	{
		public static bool EqualsTo(
			Outbox.Model.OutboxMessageType? obj1,
			Outbox.Model.OutboxMessageType? obj2,
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<Outbox.Model.OutboxMessageType>>? conditions = null,
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
			
			ComparisonConditions<Outbox.Model.OutboxMessageType>? conds = null;
		
			if ((ComparisonOptions.CompareProperties & comparisonOptions) == ComparisonOptions.CompareProperties)
			{
				if (conditions != null)
				{
					conds = new ComparisonConditions<Outbox.Model.OutboxMessageType>();
					conditions.Invoke(conds);

					if (conds.CanCompare(obj1, nameof(obj1.IdOutboxMessageType)) && obj1.IdOutboxMessageType != obj2.IdOutboxMessageType)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.Code)) && !string.Equals(obj1.Code, obj2.Code))
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.Name)) && !string.Equals(obj1.Name, obj2.Name))
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.Namespace)) && !string.Equals(obj1.Namespace, obj2.Namespace))
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.CreatedUtc)) && obj1.CreatedUtc != obj2.CreatedUtc)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.IdOutboxInstance)) && obj1.IdOutboxInstance != obj2.IdOutboxInstance)
						return false;
				}
				else
				{
					if (obj1.IdOutboxMessageType != obj2.IdOutboxMessageType)
						return false;
					if (!string.Equals(obj1.Code, obj2.Code))
						return false;
					if (!string.Equals(obj1.Name, obj2.Name))
						return false;
					if (!string.Equals(obj1.Namespace, obj2.Namespace))
						return false;
					if (obj1.CreatedUtc != obj2.CreatedUtc)
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
				if (!OutboxInstance.OutboxInstanceEqualityComparer.EqualsTo(obj1.OutboxInstance, obj2.OutboxInstance, comparisonOptions, conds?.GetConditions(x => x.OutboxInstance), cache))
					return false;
				if (!ComparisonHelper.SequenceEqual(obj1.OutboxMessageArchives, obj2.OutboxMessageArchives, new OutboxMessageArchive.OutboxMessageArchiveEqualityComparer(comparisonOptions, conds?.GetConditions(x => x.OutboxMessageArchives), cache)))
					return false;
				if (!ComparisonHelper.SequenceEqual(obj1.OutboxMessages, obj2.OutboxMessages, new OutboxMessage.OutboxMessageEqualityComparer(comparisonOptions, conds?.GetConditions(x => x.OutboxMessages), cache)))
					return false;
				if (!ComparisonHelper.SequenceEqual(obj1.OutboxQueues, obj2.OutboxQueues, new OutboxQueue.OutboxQueueEqualityComparer(comparisonOptions, conds?.GetConditions(x => x.OutboxQueues), cache)))
					return false;
		}

			return true;
		}

		public static int GetHashCode(
			Outbox.Model.OutboxMessageType? obj,
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<Outbox.Model.OutboxMessageType>>? conditions = null,
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
		public Action<ComparisonConditions<Outbox.Model.OutboxMessageType>>? Conditions { get; }
		public Dictionary<object, HashSet<object>>? Cache { get; }
	
		public OutboxMessageTypeEqualityComparer(
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<Outbox.Model.OutboxMessageType>>? conditions = null,
			Dictionary<object, HashSet<object>>? cache = null)
		{
			Cache = cache ?? [];
			ComparisonOptions = comparisonOptions;
			Conditions = conditions;
		}
	
		public bool Equals(
			Outbox.Model.OutboxMessageType? obj1,
			Outbox.Model.OutboxMessageType? obj2)
			=> EqualsTo(
				obj1,
				obj2,
				ComparisonOptions,
				Conditions,
				Cache);
	
		public int GetHashCode([DisallowNull] Outbox.Model.OutboxMessageType? obj)
			=> GetHashCode(
				obj,
				ComparisonOptions,
				Conditions,
				cache: null);
	}
}
