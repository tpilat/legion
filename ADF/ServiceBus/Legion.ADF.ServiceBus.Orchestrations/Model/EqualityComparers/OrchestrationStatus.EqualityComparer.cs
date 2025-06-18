using Legion;
using Legion.Model.Comparers;
using System.Diagnostics.CodeAnalysis;

namespace Legion.ADF.ServiceBus.Orchestrations.Model;

public sealed partial class OrchestrationStatus : Orchestrations.OrchestrationsBaseEntity, Legion.Model.IEntity
{
	public bool EqualsTo(
		Orchestrations.Model.OrchestrationStatus? obj,
		ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
		Action<ComparisonConditions<Orchestrations.Model.OrchestrationStatus>>? conditions = null)
		=> OrchestrationStatusEqualityComparer.EqualsTo(
			this,
			obj,
			comparisonOptions,
			conditions,
			cache: null);

	public sealed partial class OrchestrationStatusEqualityComparer : IEqualityComparer<OrchestrationStatus>
	{
		public static bool EqualsTo(
			Orchestrations.Model.OrchestrationStatus? obj1,
			Orchestrations.Model.OrchestrationStatus? obj2,
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<Orchestrations.Model.OrchestrationStatus>>? conditions = null,
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
			
			ComparisonConditions<Orchestrations.Model.OrchestrationStatus>? conds = null;
		
			if ((ComparisonOptions.CompareProperties & comparisonOptions) == ComparisonOptions.CompareProperties)
			{
				if (conditions != null)
				{
					conds = new ComparisonConditions<Orchestrations.Model.OrchestrationStatus>();
					conditions.Invoke(conds);

					if (conds.CanCompare(obj1, nameof(obj1.IdOrchestrationStatus)) && obj1.IdOrchestrationStatus != obj2.IdOrchestrationStatus)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.Code)) && !string.Equals(obj1.Code, obj2.Code))
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.Name)) && !string.Equals(obj1.Name, obj2.Name))
						return false;
				}
				else
				{
					if (obj1.IdOrchestrationStatus != obj2.IdOrchestrationStatus)
						return false;
					if (!string.Equals(obj1.Code, obj2.Code))
						return false;
					if (!string.Equals(obj1.Name, obj2.Name))
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
				if (!ComparisonHelper.SequenceEqual(obj1.OrchestrationInstances, obj2.OrchestrationInstances, new OrchestrationInstance.OrchestrationInstanceEqualityComparer(comparisonOptions, conds?.GetConditions(x => x.OrchestrationInstances), cache)))
					return false;
		}

			return true;
		}

		public static int GetHashCode(
			Orchestrations.Model.OrchestrationStatus? obj,
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<Orchestrations.Model.OrchestrationStatus>>? conditions = null,
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
		public Action<ComparisonConditions<Orchestrations.Model.OrchestrationStatus>>? Conditions { get; }
		public Dictionary<object, HashSet<object>>? Cache { get; }
	
		public OrchestrationStatusEqualityComparer(
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<Orchestrations.Model.OrchestrationStatus>>? conditions = null,
			Dictionary<object, HashSet<object>>? cache = null)
		{
			Cache = cache ?? [];
			ComparisonOptions = comparisonOptions;
			Conditions = conditions;
		}
	
		public bool Equals(
			Orchestrations.Model.OrchestrationStatus? obj1,
			Orchestrations.Model.OrchestrationStatus? obj2)
			=> EqualsTo(
				obj1,
				obj2,
				ComparisonOptions,
				Conditions,
				Cache);
	
		public int GetHashCode([DisallowNull] Orchestrations.Model.OrchestrationStatus? obj)
			=> GetHashCode(
				obj,
				ComparisonOptions,
				Conditions,
				cache: null);
	}
}
