using Legion;
using Legion.Model.Comparers;
using System.Diagnostics.CodeAnalysis;

namespace Legion.ADF.ServiceBus.Model;

public sealed partial class OrchestrationStepProcessingDirection : ServiceBus.ServiceBusBaseEntity, Legion.Model.IEntity
{
	public bool EqualsTo(
		ServiceBus.Model.OrchestrationStepProcessingDirection? obj,
		ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
		Action<ComparisonConditions<ServiceBus.Model.OrchestrationStepProcessingDirection>>? conditions = null)
		=> OrchestrationStepProcessingDirectionEqualityComparer.EqualsTo(
			this,
			obj,
			comparisonOptions,
			conditions,
			cache: null);

	public sealed partial class OrchestrationStepProcessingDirectionEqualityComparer : IEqualityComparer<OrchestrationStepProcessingDirection>
	{
		public static bool EqualsTo(
			ServiceBus.Model.OrchestrationStepProcessingDirection? obj1,
			ServiceBus.Model.OrchestrationStepProcessingDirection? obj2,
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<ServiceBus.Model.OrchestrationStepProcessingDirection>>? conditions = null,
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
			
			ComparisonConditions<ServiceBus.Model.OrchestrationStepProcessingDirection>? conds = null;
		
			if ((ComparisonOptions.CompareProperties & comparisonOptions) == ComparisonOptions.CompareProperties)
			{
				if (conditions != null)
				{
					conds = new ComparisonConditions<ServiceBus.Model.OrchestrationStepProcessingDirection>();
					conditions.Invoke(conds);

					if (conds.CanCompare(obj1, nameof(obj1.IdOrchestrationStepProcessingDirection)) && obj1.IdOrchestrationStepProcessingDirection != obj2.IdOrchestrationStepProcessingDirection)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.IdFromStep)) && obj1.IdFromStep != obj2.IdFromStep)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.IdToStep)) && obj1.IdToStep != obj2.IdToStep)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.CreatedUtc)) && obj1.CreatedUtc != obj2.CreatedUtc)
						return false;
				}
				else
				{
					if (obj1.IdOrchestrationStepProcessingDirection != obj2.IdOrchestrationStepProcessingDirection)
						return false;
					if (obj1.IdFromStep != obj2.IdFromStep)
						return false;
					if (obj1.IdToStep != obj2.IdToStep)
						return false;
					if (obj1.CreatedUtc != obj2.CreatedUtc)
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
				if (!OrchestrationStepProcessing.OrchestrationStepProcessingEqualityComparer.EqualsTo(obj1.FromStep, obj2.FromStep, comparisonOptions, conds?.GetConditions(x => x.FromStep), cache))
					return false;
				if (!OrchestrationStepProcessing.OrchestrationStepProcessingEqualityComparer.EqualsTo(obj1.ToStep, obj2.ToStep, comparisonOptions, conds?.GetConditions(x => x.ToStep), cache))
					return false;
		}

			return true;
		}

		public static int GetHashCode(
			ServiceBus.Model.OrchestrationStepProcessingDirection? obj,
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<ServiceBus.Model.OrchestrationStepProcessingDirection>>? conditions = null,
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
		public Action<ComparisonConditions<ServiceBus.Model.OrchestrationStepProcessingDirection>>? Conditions { get; }
		public Dictionary<object, HashSet<object>>? Cache { get; }
	
		public OrchestrationStepProcessingDirectionEqualityComparer(
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<ServiceBus.Model.OrchestrationStepProcessingDirection>>? conditions = null,
			Dictionary<object, HashSet<object>>? cache = null)
		{
			Cache = cache ?? [];
			ComparisonOptions = comparisonOptions;
			Conditions = conditions;
		}
	
		public bool Equals(
			ServiceBus.Model.OrchestrationStepProcessingDirection? obj1,
			ServiceBus.Model.OrchestrationStepProcessingDirection? obj2)
			=> EqualsTo(
				obj1,
				obj2,
				ComparisonOptions,
				Conditions,
				Cache);
	
		public int GetHashCode([DisallowNull] ServiceBus.Model.OrchestrationStepProcessingDirection? obj)
			=> GetHashCode(
				obj,
				ComparisonOptions,
				Conditions,
				cache: null);
	}
}
