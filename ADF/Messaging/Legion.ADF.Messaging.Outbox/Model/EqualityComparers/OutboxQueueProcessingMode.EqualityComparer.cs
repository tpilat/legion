using Legion;
using Legion.Model.Comparers;
using System.Diagnostics.CodeAnalysis;

namespace Legion.ADF.Messaging.Outbox.Model;

public sealed partial class OutboxQueueProcessingMode : Outbox.OutboxBaseEntity, Legion.Model.IEntity
{
	public bool EqualsTo(
		Outbox.Model.OutboxQueueProcessingMode? obj,
		ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
		Action<ComparisonConditions<Outbox.Model.OutboxQueueProcessingMode>>? conditions = null)
		=> OutboxQueueProcessingModeEqualityComparer.EqualsTo(
			this,
			obj,
			comparisonOptions,
			conditions,
			cache: null);

	public sealed partial class OutboxQueueProcessingModeEqualityComparer : IEqualityComparer<OutboxQueueProcessingMode>
	{
		public static bool EqualsTo(
			Outbox.Model.OutboxQueueProcessingMode? obj1,
			Outbox.Model.OutboxQueueProcessingMode? obj2,
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<Outbox.Model.OutboxQueueProcessingMode>>? conditions = null,
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
			
			ComparisonConditions<Outbox.Model.OutboxQueueProcessingMode>? conds = null;
		
			if ((ComparisonOptions.CompareProperties & comparisonOptions) == ComparisonOptions.CompareProperties)
			{
				if (conditions != null)
				{
					conds = new ComparisonConditions<Outbox.Model.OutboxQueueProcessingMode>();
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
				if (!ComparisonHelper.SequenceEqual(obj1.OutboxQueues, obj2.OutboxQueues, new OutboxQueue.OutboxQueueEqualityComparer(comparisonOptions, conds?.GetConditions(x => x.OutboxQueues), cache)))
					return false;
				if (!ComparisonHelper.SequenceEqual(obj1.SuspendingModeOutboxQueues, obj2.SuspendingModeOutboxQueues, new OutboxQueue.OutboxQueueEqualityComparer(comparisonOptions, conds?.GetConditions(x => x.SuspendingModeOutboxQueues), cache)))
					return false;
		}

			return true;
		}

		public static int GetHashCode(
			Outbox.Model.OutboxQueueProcessingMode? obj,
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<Outbox.Model.OutboxQueueProcessingMode>>? conditions = null,
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
		public Action<ComparisonConditions<Outbox.Model.OutboxQueueProcessingMode>>? Conditions { get; }
		public Dictionary<object, HashSet<object>>? Cache { get; }
	
		public OutboxQueueProcessingModeEqualityComparer(
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<Outbox.Model.OutboxQueueProcessingMode>>? conditions = null,
			Dictionary<object, HashSet<object>>? cache = null)
		{
			Cache = cache ?? [];
			ComparisonOptions = comparisonOptions;
			Conditions = conditions;
		}
	
		public bool Equals(
			Outbox.Model.OutboxQueueProcessingMode? obj1,
			Outbox.Model.OutboxQueueProcessingMode? obj2)
			=> EqualsTo(
				obj1,
				obj2,
				ComparisonOptions,
				Conditions,
				Cache);
	
		public int GetHashCode([DisallowNull] Outbox.Model.OutboxQueueProcessingMode? obj)
			=> GetHashCode(
				obj,
				ComparisonOptions,
				Conditions,
				cache: null);
	}
}
