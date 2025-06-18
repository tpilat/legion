using Legion;
using Legion.Model.Comparers;
using System.Diagnostics.CodeAnalysis;

namespace Legion.ADF.Messaging.Outbox.Model;

public sealed partial class OutboxMessageStatus : Outbox.OutboxBaseEntity, Legion.Model.IEntity
{
	public bool EqualsTo(
		Outbox.Model.OutboxMessageStatus? obj,
		ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
		Action<ComparisonConditions<Outbox.Model.OutboxMessageStatus>>? conditions = null)
		=> OutboxMessageStatusEqualityComparer.EqualsTo(
			this,
			obj,
			comparisonOptions,
			conditions,
			cache: null);

	public sealed partial class OutboxMessageStatusEqualityComparer : IEqualityComparer<OutboxMessageStatus>
	{
		public static bool EqualsTo(
			Outbox.Model.OutboxMessageStatus? obj1,
			Outbox.Model.OutboxMessageStatus? obj2,
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<Outbox.Model.OutboxMessageStatus>>? conditions = null,
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
			
			ComparisonConditions<Outbox.Model.OutboxMessageStatus>? conds = null;
		
			if ((ComparisonOptions.CompareProperties & comparisonOptions) == ComparisonOptions.CompareProperties)
			{
				if (conditions != null)
				{
					conds = new ComparisonConditions<Outbox.Model.OutboxMessageStatus>();
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
				if (!ComparisonHelper.SequenceEqual(obj1.OutboxMessageArchives, obj2.OutboxMessageArchives, new OutboxMessageArchive.OutboxMessageArchiveEqualityComparer(comparisonOptions, conds?.GetConditions(x => x.OutboxMessageArchives), cache)))
					return false;
				if (!ComparisonHelper.SequenceEqual(obj1.OutboxMessageProcessingLogs, obj2.OutboxMessageProcessingLogs, new OutboxMessageProcessingLog.OutboxMessageProcessingLogEqualityComparer(comparisonOptions, conds?.GetConditions(x => x.OutboxMessageProcessingLogs), cache)))
					return false;
				if (!ComparisonHelper.SequenceEqual(obj1.OutboxMessages, obj2.OutboxMessages, new OutboxMessage.OutboxMessageEqualityComparer(comparisonOptions, conds?.GetConditions(x => x.OutboxMessages), cache)))
					return false;
		}

			return true;
		}

		public static int GetHashCode(
			Outbox.Model.OutboxMessageStatus? obj,
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<Outbox.Model.OutboxMessageStatus>>? conditions = null,
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
		public Action<ComparisonConditions<Outbox.Model.OutboxMessageStatus>>? Conditions { get; }
		public Dictionary<object, HashSet<object>>? Cache { get; }
	
		public OutboxMessageStatusEqualityComparer(
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<Outbox.Model.OutboxMessageStatus>>? conditions = null,
			Dictionary<object, HashSet<object>>? cache = null)
		{
			Cache = cache ?? [];
			ComparisonOptions = comparisonOptions;
			Conditions = conditions;
		}
	
		public bool Equals(
			Outbox.Model.OutboxMessageStatus? obj1,
			Outbox.Model.OutboxMessageStatus? obj2)
			=> EqualsTo(
				obj1,
				obj2,
				ComparisonOptions,
				Conditions,
				Cache);
	
		public int GetHashCode([DisallowNull] Outbox.Model.OutboxMessageStatus? obj)
			=> GetHashCode(
				obj,
				ComparisonOptions,
				Conditions,
				cache: null);
	}
}
