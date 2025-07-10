using Legion;
using Legion.Model.Comparers;
using System.Diagnostics.CodeAnalysis;

namespace Legion.ADF.ServiceBus.Hosts.Model;

public sealed partial class Host : Hosts.HostsBaseEntity, Legion.Model.IEntity
{
	public bool EqualsTo(
		Hosts.Model.Host? obj,
		ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
		Action<ComparisonConditions<Hosts.Model.Host>>? conditions = null)
		=> HostEqualityComparer.EqualsTo(
			this,
			obj,
			comparisonOptions,
			conditions,
			cache: null);

	public sealed partial class HostEqualityComparer : IEqualityComparer<Host>
	{
		public static bool EqualsTo(
			Hosts.Model.Host? obj1,
			Hosts.Model.Host? obj2,
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<Hosts.Model.Host>>? conditions = null,
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
			
			ComparisonConditions<Hosts.Model.Host>? conds = null;
		
			if ((ComparisonOptions.CompareProperties & comparisonOptions) == ComparisonOptions.CompareProperties)
			{
				if (conditions != null)
				{
					conds = new ComparisonConditions<Hosts.Model.Host>();
					conditions.Invoke(conds);

					if (conds.CanCompare(obj1, nameof(obj1.IdHost)) && obj1.IdHost != obj2.IdHost)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.Name)) && !string.Equals(obj1.Name, obj2.Name))
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.Description)) && !string.Equals(obj1.Description, obj2.Description))
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.CreatedUtc)) && obj1.CreatedUtc != obj2.CreatedUtc)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.IsEnabled)) && obj1.IsEnabled != obj2.IsEnabled)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.StartedUtc)) && obj1.StartedUtc != obj2.StartedUtc)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.LastActivityUtc)) && obj1.LastActivityUtc != obj2.LastActivityUtc)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.StoppedUtc)) && obj1.StoppedUtc != obj2.StoppedUtc)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.Configuration)) && !string.Equals(obj1.Configuration, obj2.Configuration))
						return false;
				}
				else
				{
					if (obj1.IdHost != obj2.IdHost)
						return false;
					if (!string.Equals(obj1.Name, obj2.Name))
						return false;
					if (!string.Equals(obj1.Description, obj2.Description))
						return false;
					if (obj1.CreatedUtc != obj2.CreatedUtc)
						return false;
					if (obj1.IsEnabled != obj2.IsEnabled)
						return false;
					if (obj1.StartedUtc != obj2.StartedUtc)
						return false;
					if (obj1.LastActivityUtc != obj2.LastActivityUtc)
						return false;
					if (obj1.StoppedUtc != obj2.StoppedUtc)
						return false;
					if (!string.Equals(obj1.Configuration, obj2.Configuration))
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
				if (!ComparisonHelper.SequenceEqual(obj1.HostLogs, obj2.HostLogs, new HostLog.HostLogEqualityComparer(comparisonOptions, conds?.GetConditions(x => x.HostLogs), cache)))
					return false;
		}

			return true;
		}

		public static int GetHashCode(
			Hosts.Model.Host? obj,
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<Hosts.Model.Host>>? conditions = null,
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
		public Action<ComparisonConditions<Hosts.Model.Host>>? Conditions { get; }
		public Dictionary<object, HashSet<object>>? Cache { get; }
	
		public HostEqualityComparer(
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<Hosts.Model.Host>>? conditions = null,
			Dictionary<object, HashSet<object>>? cache = null)
		{
			Cache = cache ?? [];
			ComparisonOptions = comparisonOptions;
			Conditions = conditions;
		}
	
		public bool Equals(
			Hosts.Model.Host? obj1,
			Hosts.Model.Host? obj2)
			=> EqualsTo(
				obj1,
				obj2,
				ComparisonOptions,
				Conditions,
				Cache);
	
		public int GetHashCode([DisallowNull] Hosts.Model.Host? obj)
			=> GetHashCode(
				obj,
				ComparisonOptions,
				Conditions,
				cache: null);
	}
}
