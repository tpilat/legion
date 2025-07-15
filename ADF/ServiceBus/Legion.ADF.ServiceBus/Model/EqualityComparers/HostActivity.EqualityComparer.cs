using Legion;
using Legion.Model.Comparers;
using System.Diagnostics.CodeAnalysis;

namespace Legion.ADF.ServiceBus.Model;

public sealed partial class HostActivity : ServiceBus.ServiceBusBaseEntity, Legion.Model.Concurrence.IConcurrent, Legion.Model.IEntity
{
	public bool EqualsTo(
		ServiceBus.Model.HostActivity? obj,
		ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
		Action<ComparisonConditions<ServiceBus.Model.HostActivity>>? conditions = null)
		=> HostActivityEqualityComparer.EqualsTo(
			this,
			obj,
			comparisonOptions,
			conditions,
			cache: null);

	public sealed partial class HostActivityEqualityComparer : IEqualityComparer<HostActivity>
	{
		public static bool EqualsTo(
			ServiceBus.Model.HostActivity? obj1,
			ServiceBus.Model.HostActivity? obj2,
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<ServiceBus.Model.HostActivity>>? conditions = null,
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
			
			ComparisonConditions<ServiceBus.Model.HostActivity>? conds = null;
		
			if ((ComparisonOptions.CompareProperties & comparisonOptions) == ComparisonOptions.CompareProperties)
			{
				if (conditions != null)
				{
					conds = new ComparisonConditions<ServiceBus.Model.HostActivity>();
					conditions.Invoke(conds);

					if (conds.CanCompare(obj1, nameof(obj1.IdHostActivity)) && obj1.IdHostActivity != obj2.IdHostActivity)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.IdHost)) && obj1.IdHost != obj2.IdHost)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.StartedUtc)) && obj1.StartedUtc != obj2.StartedUtc)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.LastActivityUtc)) && obj1.LastActivityUtc != obj2.LastActivityUtc)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.StoppedUtc)) && obj1.StoppedUtc != obj2.StoppedUtc)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.IsDistributedManagerAvailable)) && obj1.IsDistributedManagerAvailable != obj2.IsDistributedManagerAvailable)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.RowVersion)) && obj1.RowVersion != obj2.RowVersion)
						return false;
				}
				else
				{
					if (obj1.IdHostActivity != obj2.IdHostActivity)
						return false;
					if (obj1.IdHost != obj2.IdHost)
						return false;
					if (obj1.StartedUtc != obj2.StartedUtc)
						return false;
					if (obj1.LastActivityUtc != obj2.LastActivityUtc)
						return false;
					if (obj1.StoppedUtc != obj2.StoppedUtc)
						return false;
					if (obj1.IsDistributedManagerAvailable != obj2.IsDistributedManagerAvailable)
						return false;
					if (obj1.RowVersion != obj2.RowVersion)
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
				if (!Host.HostEqualityComparer.EqualsTo(obj1.Host, obj2.Host, comparisonOptions, conds?.GetConditions(x => x.Host), cache))
					return false;
		}

			return true;
		}

		public static int GetHashCode(
			ServiceBus.Model.HostActivity? obj,
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<ServiceBus.Model.HostActivity>>? conditions = null,
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
		public Action<ComparisonConditions<ServiceBus.Model.HostActivity>>? Conditions { get; }
		public Dictionary<object, HashSet<object>>? Cache { get; }
	
		public HostActivityEqualityComparer(
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<ServiceBus.Model.HostActivity>>? conditions = null,
			Dictionary<object, HashSet<object>>? cache = null)
		{
			Cache = cache ?? [];
			ComparisonOptions = comparisonOptions;
			Conditions = conditions;
		}
	
		public bool Equals(
			ServiceBus.Model.HostActivity? obj1,
			ServiceBus.Model.HostActivity? obj2)
			=> EqualsTo(
				obj1,
				obj2,
				ComparisonOptions,
				Conditions,
				Cache);
	
		public int GetHashCode([DisallowNull] ServiceBus.Model.HostActivity? obj)
			=> GetHashCode(
				obj,
				ComparisonOptions,
				Conditions,
				cache: null);
	}
}
