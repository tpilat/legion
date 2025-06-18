using Legion;
using Legion.Model.Comparers;
using System.Diagnostics.CodeAnalysis;

namespace Legion.ADF.ServiceBus.Orchestrations.Model;

public sealed partial class Orchestration : Orchestrations.OrchestrationsBaseEntity, Legion.Model.IEntity
{
	public bool EqualsTo(
		Orchestrations.Model.Orchestration? obj,
		ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
		Action<ComparisonConditions<Orchestrations.Model.Orchestration>>? conditions = null)
		=> OrchestrationEqualityComparer.EqualsTo(
			this,
			obj,
			comparisonOptions,
			conditions,
			cache: null);

	public sealed partial class OrchestrationEqualityComparer : IEqualityComparer<Orchestration>
	{
		public static bool EqualsTo(
			Orchestrations.Model.Orchestration? obj1,
			Orchestrations.Model.Orchestration? obj2,
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<Orchestrations.Model.Orchestration>>? conditions = null,
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
			
			ComparisonConditions<Orchestrations.Model.Orchestration>? conds = null;
		
			if ((ComparisonOptions.CompareProperties & comparisonOptions) == ComparisonOptions.CompareProperties)
			{
				if (conditions != null)
				{
					conds = new ComparisonConditions<Orchestrations.Model.Orchestration>();
					conditions.Invoke(conds);

					if (conds.CanCompare(obj1, nameof(obj1.IdOrchestration)) && obj1.IdOrchestration != obj2.IdOrchestration)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.Name)) && !string.Equals(obj1.Name, obj2.Name))
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.Description)) && !string.Equals(obj1.Description, obj2.Description))
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.IsSingleton)) && obj1.IsSingleton != obj2.IsSingleton)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.Namespace)) && !string.Equals(obj1.Namespace, obj2.Namespace))
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.Version)) && !string.Equals(obj1.Version, obj2.Version))
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.Properties)) && !string.Equals(obj1.Properties, obj2.Properties))
						return false;
				}
				else
				{
					if (obj1.IdOrchestration != obj2.IdOrchestration)
						return false;
					if (!string.Equals(obj1.Name, obj2.Name))
						return false;
					if (!string.Equals(obj1.Description, obj2.Description))
						return false;
					if (obj1.IsSingleton != obj2.IsSingleton)
						return false;
					if (!string.Equals(obj1.Namespace, obj2.Namespace))
						return false;
					if (!string.Equals(obj1.Version, obj2.Version))
						return false;
					if (!string.Equals(obj1.Properties, obj2.Properties))
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
				if (!ComparisonHelper.SequenceEqual(obj1.OrchestrationSteps, obj2.OrchestrationSteps, new OrchestrationStep.OrchestrationStepEqualityComparer(comparisonOptions, conds?.GetConditions(x => x.OrchestrationSteps), cache)))
					return false;
		}

			return true;
		}

		public static int GetHashCode(
			Orchestrations.Model.Orchestration? obj,
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<Orchestrations.Model.Orchestration>>? conditions = null,
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
		public Action<ComparisonConditions<Orchestrations.Model.Orchestration>>? Conditions { get; }
		public Dictionary<object, HashSet<object>>? Cache { get; }
	
		public OrchestrationEqualityComparer(
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<Orchestrations.Model.Orchestration>>? conditions = null,
			Dictionary<object, HashSet<object>>? cache = null)
		{
			Cache = cache ?? [];
			ComparisonOptions = comparisonOptions;
			Conditions = conditions;
		}
	
		public bool Equals(
			Orchestrations.Model.Orchestration? obj1,
			Orchestrations.Model.Orchestration? obj2)
			=> EqualsTo(
				obj1,
				obj2,
				ComparisonOptions,
				Conditions,
				Cache);
	
		public int GetHashCode([DisallowNull] Orchestrations.Model.Orchestration? obj)
			=> GetHashCode(
				obj,
				ComparisonOptions,
				Conditions,
				cache: null);
	}
}
