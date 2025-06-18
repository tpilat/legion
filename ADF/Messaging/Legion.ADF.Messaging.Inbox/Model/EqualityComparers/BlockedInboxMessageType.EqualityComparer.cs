using Legion;
using Legion.Model.Comparers;
using System.Diagnostics.CodeAnalysis;

namespace Legion.ADF.Messaging.Inbox.Model;

public sealed partial class BlockedInboxMessageType : Inbox.InboxBaseEntity, Legion.Model.IEntity
{
	public bool EqualsTo(
		Inbox.Model.BlockedInboxMessageType? obj,
		ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
		Action<ComparisonConditions<Inbox.Model.BlockedInboxMessageType>>? conditions = null)
		=> BlockedInboxMessageTypeEqualityComparer.EqualsTo(
			this,
			obj,
			comparisonOptions,
			conditions,
			cache: null);

	public sealed partial class BlockedInboxMessageTypeEqualityComparer : IEqualityComparer<BlockedInboxMessageType>
	{
		public static bool EqualsTo(
			Inbox.Model.BlockedInboxMessageType? obj1,
			Inbox.Model.BlockedInboxMessageType? obj2,
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<Inbox.Model.BlockedInboxMessageType>>? conditions = null,
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
			
			ComparisonConditions<Inbox.Model.BlockedInboxMessageType>? conds = null;
		
			if ((ComparisonOptions.CompareProperties & comparisonOptions) == ComparisonOptions.CompareProperties)
			{
				if (conditions != null)
				{
					conds = new ComparisonConditions<Inbox.Model.BlockedInboxMessageType>();
					conditions.Invoke(conds);

					if (conds.CanCompare(obj1, nameof(obj1.IdBlockedInboxMessageType)) && obj1.IdBlockedInboxMessageType != obj2.IdBlockedInboxMessageType)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.Namespace)) && !string.Equals(obj1.Namespace, obj2.Namespace))
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.CreatedUtc)) && obj1.CreatedUtc != obj2.CreatedUtc)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.IdInboxInstance)) && obj1.IdInboxInstance != obj2.IdInboxInstance)
						return false;
				}
				else
				{
					if (obj1.IdBlockedInboxMessageType != obj2.IdBlockedInboxMessageType)
						return false;
					if (!string.Equals(obj1.Namespace, obj2.Namespace))
						return false;
					if (obj1.CreatedUtc != obj2.CreatedUtc)
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
		}

			return true;
		}

		public static int GetHashCode(
			Inbox.Model.BlockedInboxMessageType? obj,
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<Inbox.Model.BlockedInboxMessageType>>? conditions = null,
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
		public Action<ComparisonConditions<Inbox.Model.BlockedInboxMessageType>>? Conditions { get; }
		public Dictionary<object, HashSet<object>>? Cache { get; }
	
		public BlockedInboxMessageTypeEqualityComparer(
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<Inbox.Model.BlockedInboxMessageType>>? conditions = null,
			Dictionary<object, HashSet<object>>? cache = null)
		{
			Cache = cache ?? [];
			ComparisonOptions = comparisonOptions;
			Conditions = conditions;
		}
	
		public bool Equals(
			Inbox.Model.BlockedInboxMessageType? obj1,
			Inbox.Model.BlockedInboxMessageType? obj2)
			=> EqualsTo(
				obj1,
				obj2,
				ComparisonOptions,
				Conditions,
				Cache);
	
		public int GetHashCode([DisallowNull] Inbox.Model.BlockedInboxMessageType? obj)
			=> GetHashCode(
				obj,
				ComparisonOptions,
				Conditions,
				cache: null);
	}
}
