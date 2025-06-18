using Legion;
using Legion.Model.Comparers;
using System.Diagnostics.CodeAnalysis;

namespace Legion.ADF.Messaging.MessageBox.Model;

public sealed partial class BlockedMessageType : MessageBox.MessageBoxBaseEntity, Legion.Model.IEntity
{
	public bool EqualsTo(
		MessageBox.Model.BlockedMessageType? obj,
		ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
		Action<ComparisonConditions<MessageBox.Model.BlockedMessageType>>? conditions = null)
		=> BlockedMessageTypeEqualityComparer.EqualsTo(
			this,
			obj,
			comparisonOptions,
			conditions,
			cache: null);

	public sealed partial class BlockedMessageTypeEqualityComparer : IEqualityComparer<BlockedMessageType>
	{
		public static bool EqualsTo(
			MessageBox.Model.BlockedMessageType? obj1,
			MessageBox.Model.BlockedMessageType? obj2,
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<MessageBox.Model.BlockedMessageType>>? conditions = null,
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
			
			ComparisonConditions<MessageBox.Model.BlockedMessageType>? conds = null;
		
			if ((ComparisonOptions.CompareProperties & comparisonOptions) == ComparisonOptions.CompareProperties)
			{
				if (conditions != null)
				{
					conds = new ComparisonConditions<MessageBox.Model.BlockedMessageType>();
					conditions.Invoke(conds);

					if (conds.CanCompare(obj1, nameof(obj1.IdBlockedMessageType)) && obj1.IdBlockedMessageType != obj2.IdBlockedMessageType)
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
					if (obj1.IdBlockedMessageType != obj2.IdBlockedMessageType)
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
		}

			return true;
		}

		public static int GetHashCode(
			MessageBox.Model.BlockedMessageType? obj,
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<MessageBox.Model.BlockedMessageType>>? conditions = null,
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
		public Action<ComparisonConditions<MessageBox.Model.BlockedMessageType>>? Conditions { get; }
		public Dictionary<object, HashSet<object>>? Cache { get; }
	
		public BlockedMessageTypeEqualityComparer(
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<MessageBox.Model.BlockedMessageType>>? conditions = null,
			Dictionary<object, HashSet<object>>? cache = null)
		{
			Cache = cache ?? [];
			ComparisonOptions = comparisonOptions;
			Conditions = conditions;
		}
	
		public bool Equals(
			MessageBox.Model.BlockedMessageType? obj1,
			MessageBox.Model.BlockedMessageType? obj2)
			=> EqualsTo(
				obj1,
				obj2,
				ComparisonOptions,
				Conditions,
				Cache);
	
		public int GetHashCode([DisallowNull] MessageBox.Model.BlockedMessageType? obj)
			=> GetHashCode(
				obj,
				ComparisonOptions,
				Conditions,
				cache: null);
	}
}
