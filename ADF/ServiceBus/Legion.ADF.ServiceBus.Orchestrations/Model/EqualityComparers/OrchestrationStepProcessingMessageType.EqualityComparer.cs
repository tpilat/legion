using Legion;
using Legion.Model.Comparers;
using System.Diagnostics.CodeAnalysis;

namespace Legion.ADF.ServiceBus.Orchestrations.Model;

public sealed partial class OrchestrationStepProcessingMessageType : Orchestrations.OrchestrationsBaseEntity, Legion.Model.IEntity
{
	public bool EqualsTo(
		Orchestrations.Model.OrchestrationStepProcessingMessageType? obj,
		ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
		Action<ComparisonConditions<Orchestrations.Model.OrchestrationStepProcessingMessageType>>? conditions = null)
		=> OrchestrationStepProcessingMessageTypeEqualityComparer.EqualsTo(
			this,
			obj,
			comparisonOptions,
			conditions,
			cache: null);

	public sealed partial class OrchestrationStepProcessingMessageTypeEqualityComparer : IEqualityComparer<OrchestrationStepProcessingMessageType>
	{
		public static bool EqualsTo(
			Orchestrations.Model.OrchestrationStepProcessingMessageType? obj1,
			Orchestrations.Model.OrchestrationStepProcessingMessageType? obj2,
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<Orchestrations.Model.OrchestrationStepProcessingMessageType>>? conditions = null,
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
			
			ComparisonConditions<Orchestrations.Model.OrchestrationStepProcessingMessageType>? conds = null;
		
			if ((ComparisonOptions.CompareProperties & comparisonOptions) == ComparisonOptions.CompareProperties)
			{
				if (conditions != null)
				{
					conds = new ComparisonConditions<Orchestrations.Model.OrchestrationStepProcessingMessageType>();
					conditions.Invoke(conds);

					if (conds.CanCompare(obj1, nameof(obj1.IdOrchestrationStepProcessingMessageType)) && obj1.IdOrchestrationStepProcessingMessageType != obj2.IdOrchestrationStepProcessingMessageType)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.Code)) && !string.Equals(obj1.Code, obj2.Code))
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.Name)) && !string.Equals(obj1.Name, obj2.Name))
						return false;
				}
				else
				{
					if (obj1.IdOrchestrationStepProcessingMessageType != obj2.IdOrchestrationStepProcessingMessageType)
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
				if (!ComparisonHelper.SequenceEqual(obj1.OrchestrationStepProcessingMessages, obj2.OrchestrationStepProcessingMessages, new OrchestrationStepProcessingMessage.OrchestrationStepProcessingMessageEqualityComparer(comparisonOptions, conds?.GetConditions(x => x.OrchestrationStepProcessingMessages), cache)))
					return false;
		}

			return true;
		}

		public static int GetHashCode(
			Orchestrations.Model.OrchestrationStepProcessingMessageType? obj,
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<Orchestrations.Model.OrchestrationStepProcessingMessageType>>? conditions = null,
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
		public Action<ComparisonConditions<Orchestrations.Model.OrchestrationStepProcessingMessageType>>? Conditions { get; }
		public Dictionary<object, HashSet<object>>? Cache { get; }
	
		public OrchestrationStepProcessingMessageTypeEqualityComparer(
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<Orchestrations.Model.OrchestrationStepProcessingMessageType>>? conditions = null,
			Dictionary<object, HashSet<object>>? cache = null)
		{
			Cache = cache ?? [];
			ComparisonOptions = comparisonOptions;
			Conditions = conditions;
		}
	
		public bool Equals(
			Orchestrations.Model.OrchestrationStepProcessingMessageType? obj1,
			Orchestrations.Model.OrchestrationStepProcessingMessageType? obj2)
			=> EqualsTo(
				obj1,
				obj2,
				ComparisonOptions,
				Conditions,
				Cache);
	
		public int GetHashCode([DisallowNull] Orchestrations.Model.OrchestrationStepProcessingMessageType? obj)
			=> GetHashCode(
				obj,
				ComparisonOptions,
				Conditions,
				cache: null);
	}
}
