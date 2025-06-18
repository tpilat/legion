using Legion;
using Legion.Model.Comparers;
using System.Diagnostics.CodeAnalysis;

namespace Legion.ADF.Messaging.Inbox.Model;

public sealed partial class InboxMessageStatus : Inbox.InboxBaseEntity, Legion.Model.IEntity
{
	public bool EqualsTo(
		Inbox.Model.InboxMessageStatus? obj,
		ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
		Action<ComparisonConditions<Inbox.Model.InboxMessageStatus>>? conditions = null)
		=> InboxMessageStatusEqualityComparer.EqualsTo(
			this,
			obj,
			comparisonOptions,
			conditions,
			cache: null);

	public sealed partial class InboxMessageStatusEqualityComparer : IEqualityComparer<InboxMessageStatus>
	{
		public static bool EqualsTo(
			Inbox.Model.InboxMessageStatus? obj1,
			Inbox.Model.InboxMessageStatus? obj2,
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<Inbox.Model.InboxMessageStatus>>? conditions = null,
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
			
			ComparisonConditions<Inbox.Model.InboxMessageStatus>? conds = null;
		
			if ((ComparisonOptions.CompareProperties & comparisonOptions) == ComparisonOptions.CompareProperties)
			{
				if (conditions != null)
				{
					conds = new ComparisonConditions<Inbox.Model.InboxMessageStatus>();
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
				if (!ComparisonHelper.SequenceEqual(obj1.InboxMessageArchives, obj2.InboxMessageArchives, new InboxMessageArchive.InboxMessageArchiveEqualityComparer(comparisonOptions, conds?.GetConditions(x => x.InboxMessageArchives), cache)))
					return false;
				if (!ComparisonHelper.SequenceEqual(obj1.InboxMessageProcessingLogs, obj2.InboxMessageProcessingLogs, new InboxMessageProcessingLog.InboxMessageProcessingLogEqualityComparer(comparisonOptions, conds?.GetConditions(x => x.InboxMessageProcessingLogs), cache)))
					return false;
				if (!ComparisonHelper.SequenceEqual(obj1.InboxMessages, obj2.InboxMessages, new InboxMessage.InboxMessageEqualityComparer(comparisonOptions, conds?.GetConditions(x => x.InboxMessages), cache)))
					return false;
		}

			return true;
		}

		public static int GetHashCode(
			Inbox.Model.InboxMessageStatus? obj,
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<Inbox.Model.InboxMessageStatus>>? conditions = null,
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
		public Action<ComparisonConditions<Inbox.Model.InboxMessageStatus>>? Conditions { get; }
		public Dictionary<object, HashSet<object>>? Cache { get; }
	
		public InboxMessageStatusEqualityComparer(
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<Inbox.Model.InboxMessageStatus>>? conditions = null,
			Dictionary<object, HashSet<object>>? cache = null)
		{
			Cache = cache ?? [];
			ComparisonOptions = comparisonOptions;
			Conditions = conditions;
		}
	
		public bool Equals(
			Inbox.Model.InboxMessageStatus? obj1,
			Inbox.Model.InboxMessageStatus? obj2)
			=> EqualsTo(
				obj1,
				obj2,
				ComparisonOptions,
				Conditions,
				Cache);
	
		public int GetHashCode([DisallowNull] Inbox.Model.InboxMessageStatus? obj)
			=> GetHashCode(
				obj,
				ComparisonOptions,
				Conditions,
				cache: null);
	}
}
