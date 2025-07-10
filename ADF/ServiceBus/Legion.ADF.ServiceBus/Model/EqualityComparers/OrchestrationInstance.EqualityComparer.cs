using Legion;
using Legion.Model.Comparers;
using System.Diagnostics.CodeAnalysis;

namespace Legion.ADF.ServiceBus.Model;

public sealed partial class OrchestrationInstance : ServiceBus.ServiceBusBaseEntity, Legion.Model.IEntity
{
	public bool EqualsTo(
		ServiceBus.Model.OrchestrationInstance? obj,
		ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
		Action<ComparisonConditions<ServiceBus.Model.OrchestrationInstance>>? conditions = null)
		=> OrchestrationInstanceEqualityComparer.EqualsTo(
			this,
			obj,
			comparisonOptions,
			conditions,
			cache: null);

	public sealed partial class OrchestrationInstanceEqualityComparer : IEqualityComparer<OrchestrationInstance>
	{
		public static bool EqualsTo(
			ServiceBus.Model.OrchestrationInstance? obj1,
			ServiceBus.Model.OrchestrationInstance? obj2,
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<ServiceBus.Model.OrchestrationInstance>>? conditions = null,
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
			
			ComparisonConditions<ServiceBus.Model.OrchestrationInstance>? conds = null;
		
			if ((ComparisonOptions.CompareProperties & comparisonOptions) == ComparisonOptions.CompareProperties)
			{
				if (conditions != null)
				{
					conds = new ComparisonConditions<ServiceBus.Model.OrchestrationInstance>();
					conditions.Invoke(conds);

					if (conds.CanCompare(obj1, nameof(obj1.IdOrchestrationInstance)) && obj1.IdOrchestrationInstance != obj2.IdOrchestrationInstance)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.IdOrchestration)) && obj1.IdOrchestration != obj2.IdOrchestration)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.IdOrchestrationStatus)) && obj1.IdOrchestrationStatus != obj2.IdOrchestrationStatus)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.CreatedUtc)) && obj1.CreatedUtc != obj2.CreatedUtc)
						return false;
				}
				else
				{
					if (obj1.IdOrchestrationInstance != obj2.IdOrchestrationInstance)
						return false;
					if (obj1.IdOrchestration != obj2.IdOrchestration)
						return false;
					if (obj1.IdOrchestrationStatus != obj2.IdOrchestrationStatus)
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
				if (!Orchestration.OrchestrationEqualityComparer.EqualsTo(obj1.Orchestration, obj2.Orchestration, comparisonOptions, conds?.GetConditions(x => x.Orchestration), cache))
					return false;
				if (!OrchestrationStatus.OrchestrationStatusEqualityComparer.EqualsTo(obj1.OrchestrationStatus, obj2.OrchestrationStatus, comparisonOptions, conds?.GetConditions(x => x.OrchestrationStatus), cache))
					return false;
				if (!ComparisonHelper.SequenceEqual(obj1.OrchestrationStepProcessings, obj2.OrchestrationStepProcessings, new OrchestrationStepProcessing.OrchestrationStepProcessingEqualityComparer(comparisonOptions, conds?.GetConditions(x => x.OrchestrationStepProcessings), cache)))
					return false;
		}

			return true;
		}

		public static int GetHashCode(
			ServiceBus.Model.OrchestrationInstance? obj,
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<ServiceBus.Model.OrchestrationInstance>>? conditions = null,
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
		public Action<ComparisonConditions<ServiceBus.Model.OrchestrationInstance>>? Conditions { get; }
		public Dictionary<object, HashSet<object>>? Cache { get; }
	
		public OrchestrationInstanceEqualityComparer(
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<ServiceBus.Model.OrchestrationInstance>>? conditions = null,
			Dictionary<object, HashSet<object>>? cache = null)
		{
			Cache = cache ?? [];
			ComparisonOptions = comparisonOptions;
			Conditions = conditions;
		}
	
		public bool Equals(
			ServiceBus.Model.OrchestrationInstance? obj1,
			ServiceBus.Model.OrchestrationInstance? obj2)
			=> EqualsTo(
				obj1,
				obj2,
				ComparisonOptions,
				Conditions,
				Cache);
	
		public int GetHashCode([DisallowNull] ServiceBus.Model.OrchestrationInstance? obj)
			=> GetHashCode(
				obj,
				ComparisonOptions,
				Conditions,
				cache: null);
	}
}
