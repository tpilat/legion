using Legion;
using Legion.Model.Comparers;
using System.Diagnostics.CodeAnalysis;

namespace Legion.ADF.ServiceBus.Orchestrations.Model;

public sealed partial class OrchestrationStep : Orchestrations.OrchestrationsBaseEntity, Legion.Model.IEntity
{
	public bool EqualsTo(
		Orchestrations.Model.OrchestrationStep? obj,
		ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
		Action<ComparisonConditions<Orchestrations.Model.OrchestrationStep>>? conditions = null)
		=> OrchestrationStepEqualityComparer.EqualsTo(
			this,
			obj,
			comparisonOptions,
			conditions,
			cache: null);

	public sealed partial class OrchestrationStepEqualityComparer : IEqualityComparer<OrchestrationStep>
	{
		public static bool EqualsTo(
			Orchestrations.Model.OrchestrationStep? obj1,
			Orchestrations.Model.OrchestrationStep? obj2,
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<Orchestrations.Model.OrchestrationStep>>? conditions = null,
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
			
			ComparisonConditions<Orchestrations.Model.OrchestrationStep>? conds = null;
		
			if ((ComparisonOptions.CompareProperties & comparisonOptions) == ComparisonOptions.CompareProperties)
			{
				if (conditions != null)
				{
					conds = new ComparisonConditions<Orchestrations.Model.OrchestrationStep>();
					conditions.Invoke(conds);

					if (conds.CanCompare(obj1, nameof(obj1.IdOrchestrationStep)) && obj1.IdOrchestrationStep != obj2.IdOrchestrationStep)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.IdOrchestration)) && obj1.IdOrchestration != obj2.IdOrchestration)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.IsMainEntry)) && obj1.IsMainEntry != obj2.IsMainEntry)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.Order)) && obj1.Order != obj2.Order)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.Name)) && !string.Equals(obj1.Name, obj2.Name))
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.Description)) && !string.Equals(obj1.Description, obj2.Description))
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.Namespace)) && !string.Equals(obj1.Namespace, obj2.Namespace))
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.Properties)) && !string.Equals(obj1.Properties, obj2.Properties))
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.TimeoutForMessageProcessingInSeconds)) && obj1.TimeoutForMessageProcessingInSeconds != obj2.TimeoutForMessageProcessingInSeconds)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.MaxMessageProcessingRetryCount)) && obj1.MaxMessageProcessingRetryCount != obj2.MaxMessageProcessingRetryCount)
						return false;
				}
				else
				{
					if (obj1.IdOrchestrationStep != obj2.IdOrchestrationStep)
						return false;
					if (obj1.IdOrchestration != obj2.IdOrchestration)
						return false;
					if (obj1.IsMainEntry != obj2.IsMainEntry)
						return false;
					if (obj1.Order != obj2.Order)
						return false;
					if (!string.Equals(obj1.Name, obj2.Name))
						return false;
					if (!string.Equals(obj1.Description, obj2.Description))
						return false;
					if (!string.Equals(obj1.Namespace, obj2.Namespace))
						return false;
					if (!string.Equals(obj1.Properties, obj2.Properties))
						return false;
					if (obj1.TimeoutForMessageProcessingInSeconds != obj2.TimeoutForMessageProcessingInSeconds)
						return false;
					if (obj1.MaxMessageProcessingRetryCount != obj2.MaxMessageProcessingRetryCount)
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
				if (!Orchestration.OrchestrationEqualityComparer.EqualsTo(obj1.Orchestration, obj2.Orchestration, comparisonOptions, conds?.GetConditions(x => x.Orchestration), cache))
					return false;
				if (!ComparisonHelper.SequenceEqual(obj1.OrchestrationStepProcessings, obj2.OrchestrationStepProcessings, new OrchestrationStepProcessing.OrchestrationStepProcessingEqualityComparer(comparisonOptions, conds?.GetConditions(x => x.OrchestrationStepProcessings), cache)))
					return false;
		}

			return true;
		}

		public static int GetHashCode(
			Orchestrations.Model.OrchestrationStep? obj,
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<Orchestrations.Model.OrchestrationStep>>? conditions = null,
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
		public Action<ComparisonConditions<Orchestrations.Model.OrchestrationStep>>? Conditions { get; }
		public Dictionary<object, HashSet<object>>? Cache { get; }
	
		public OrchestrationStepEqualityComparer(
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<Orchestrations.Model.OrchestrationStep>>? conditions = null,
			Dictionary<object, HashSet<object>>? cache = null)
		{
			Cache = cache ?? [];
			ComparisonOptions = comparisonOptions;
			Conditions = conditions;
		}
	
		public bool Equals(
			Orchestrations.Model.OrchestrationStep? obj1,
			Orchestrations.Model.OrchestrationStep? obj2)
			=> EqualsTo(
				obj1,
				obj2,
				ComparisonOptions,
				Conditions,
				Cache);
	
		public int GetHashCode([DisallowNull] Orchestrations.Model.OrchestrationStep? obj)
			=> GetHashCode(
				obj,
				ComparisonOptions,
				Conditions,
				cache: null);
	}
}
