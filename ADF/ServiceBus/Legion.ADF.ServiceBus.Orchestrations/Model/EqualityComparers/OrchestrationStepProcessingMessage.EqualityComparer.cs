using Legion;
using Legion.Model.Comparers;
using System.Diagnostics.CodeAnalysis;

namespace Legion.ADF.ServiceBus.Orchestrations.Model;

public sealed partial class OrchestrationStepProcessingMessage : Orchestrations.OrchestrationsBaseEntity, Legion.Model.IEntity
{
	public bool EqualsTo(
		Orchestrations.Model.OrchestrationStepProcessingMessage? obj,
		ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
		Action<ComparisonConditions<Orchestrations.Model.OrchestrationStepProcessingMessage>>? conditions = null)
		=> OrchestrationStepProcessingMessageEqualityComparer.EqualsTo(
			this,
			obj,
			comparisonOptions,
			conditions,
			cache: null);

	public sealed partial class OrchestrationStepProcessingMessageEqualityComparer : IEqualityComparer<OrchestrationStepProcessingMessage>
	{
		public static bool EqualsTo(
			Orchestrations.Model.OrchestrationStepProcessingMessage? obj1,
			Orchestrations.Model.OrchestrationStepProcessingMessage? obj2,
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<Orchestrations.Model.OrchestrationStepProcessingMessage>>? conditions = null,
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
			
			ComparisonConditions<Orchestrations.Model.OrchestrationStepProcessingMessage>? conds = null;
		
			if ((ComparisonOptions.CompareProperties & comparisonOptions) == ComparisonOptions.CompareProperties)
			{
				if (conditions != null)
				{
					conds = new ComparisonConditions<Orchestrations.Model.OrchestrationStepProcessingMessage>();
					conditions.Invoke(conds);

					if (conds.CanCompare(obj1, nameof(obj1.IdOrchestrationStepProcessingMessage)) && obj1.IdOrchestrationStepProcessingMessage != obj2.IdOrchestrationStepProcessingMessage)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.IdOrchestrationStepProcessing)) && obj1.IdOrchestrationStepProcessing != obj2.IdOrchestrationStepProcessing)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.IdMessage)) && obj1.IdMessage != obj2.IdMessage)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.IdOrchestrationStepProcessingMessageType)) && obj1.IdOrchestrationStepProcessingMessageType != obj2.IdOrchestrationStepProcessingMessageType)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.CreatedUtc)) && obj1.CreatedUtc != obj2.CreatedUtc)
						return false;
				}
				else
				{
					if (obj1.IdOrchestrationStepProcessingMessage != obj2.IdOrchestrationStepProcessingMessage)
						return false;
					if (obj1.IdOrchestrationStepProcessing != obj2.IdOrchestrationStepProcessing)
						return false;
					if (obj1.IdMessage != obj2.IdMessage)
						return false;
					if (obj1.IdOrchestrationStepProcessingMessageType != obj2.IdOrchestrationStepProcessingMessageType)
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
				if (!OrchestrationStepProcessing.OrchestrationStepProcessingEqualityComparer.EqualsTo(obj1.OrchestrationStepProcessing, obj2.OrchestrationStepProcessing, comparisonOptions, conds?.GetConditions(x => x.OrchestrationStepProcessing), cache))
					return false;
				if (!OrchestrationStepProcessingMessageType.OrchestrationStepProcessingMessageTypeEqualityComparer.EqualsTo(obj1.OrchestrationStepProcessingMessageType, obj2.OrchestrationStepProcessingMessageType, comparisonOptions, conds?.GetConditions(x => x.OrchestrationStepProcessingMessageType), cache))
					return false;
		}

			return true;
		}

		public static int GetHashCode(
			Orchestrations.Model.OrchestrationStepProcessingMessage? obj,
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<Orchestrations.Model.OrchestrationStepProcessingMessage>>? conditions = null,
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
		public Action<ComparisonConditions<Orchestrations.Model.OrchestrationStepProcessingMessage>>? Conditions { get; }
		public Dictionary<object, HashSet<object>>? Cache { get; }
	
		public OrchestrationStepProcessingMessageEqualityComparer(
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<Orchestrations.Model.OrchestrationStepProcessingMessage>>? conditions = null,
			Dictionary<object, HashSet<object>>? cache = null)
		{
			Cache = cache ?? [];
			ComparisonOptions = comparisonOptions;
			Conditions = conditions;
		}
	
		public bool Equals(
			Orchestrations.Model.OrchestrationStepProcessingMessage? obj1,
			Orchestrations.Model.OrchestrationStepProcessingMessage? obj2)
			=> EqualsTo(
				obj1,
				obj2,
				ComparisonOptions,
				Conditions,
				Cache);
	
		public int GetHashCode([DisallowNull] Orchestrations.Model.OrchestrationStepProcessingMessage? obj)
			=> GetHashCode(
				obj,
				ComparisonOptions,
				Conditions,
				cache: null);
	}
}
