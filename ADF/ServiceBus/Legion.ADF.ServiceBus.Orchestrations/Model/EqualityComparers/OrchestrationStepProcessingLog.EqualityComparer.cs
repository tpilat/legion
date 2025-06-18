using Legion;
using Legion.Model.Comparers;
using System.Diagnostics.CodeAnalysis;

namespace Legion.ADF.ServiceBus.Orchestrations.Model;

public sealed partial class OrchestrationStepProcessingLog : Orchestrations.OrchestrationsBaseEntity, Legion.Model.IEntity
{
	public bool EqualsTo(
		Orchestrations.Model.OrchestrationStepProcessingLog? obj,
		ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
		Action<ComparisonConditions<Orchestrations.Model.OrchestrationStepProcessingLog>>? conditions = null)
		=> OrchestrationStepProcessingLogEqualityComparer.EqualsTo(
			this,
			obj,
			comparisonOptions,
			conditions,
			cache: null);

	public sealed partial class OrchestrationStepProcessingLogEqualityComparer : IEqualityComparer<OrchestrationStepProcessingLog>
	{
		public static bool EqualsTo(
			Orchestrations.Model.OrchestrationStepProcessingLog? obj1,
			Orchestrations.Model.OrchestrationStepProcessingLog? obj2,
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<Orchestrations.Model.OrchestrationStepProcessingLog>>? conditions = null,
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
			
			ComparisonConditions<Orchestrations.Model.OrchestrationStepProcessingLog>? conds = null;
		
			if ((ComparisonOptions.CompareProperties & comparisonOptions) == ComparisonOptions.CompareProperties)
			{
				if (conditions != null)
				{
					conds = new ComparisonConditions<Orchestrations.Model.OrchestrationStepProcessingLog>();
					conditions.Invoke(conds);

					if (conds.CanCompare(obj1, nameof(obj1.IdOrchestrationStepProcessingLog)) && obj1.IdOrchestrationStepProcessingLog != obj2.IdOrchestrationStepProcessingLog)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.IdOrchestrationStepProcessing)) && obj1.IdOrchestrationStepProcessing != obj2.IdOrchestrationStepProcessing)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.IdLogLevel)) && obj1.IdLogLevel != obj2.IdLogLevel)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.CreatedUtc)) && obj1.CreatedUtc != obj2.CreatedUtc)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.IdOrchestrationStepProcessingStatus)) && obj1.IdOrchestrationStepProcessingStatus != obj2.IdOrchestrationStepProcessingStatus)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.TraceCorrelationId)) && obj1.TraceCorrelationId != obj2.TraceCorrelationId)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.IdLogMessage)) && obj1.IdLogMessage != obj2.IdLogMessage)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.Code)) && !string.Equals(obj1.Code, obj2.Code))
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.Detail)) && !string.Equals(obj1.Detail, obj2.Detail))
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.IdMessageProcessingLog)) && obj1.IdMessageProcessingLog != obj2.IdMessageProcessingLog)
						return false;
				}
				else
				{
					if (obj1.IdOrchestrationStepProcessingLog != obj2.IdOrchestrationStepProcessingLog)
						return false;
					if (obj1.IdOrchestrationStepProcessing != obj2.IdOrchestrationStepProcessing)
						return false;
					if (obj1.IdLogLevel != obj2.IdLogLevel)
						return false;
					if (obj1.CreatedUtc != obj2.CreatedUtc)
						return false;
					if (obj1.IdOrchestrationStepProcessingStatus != obj2.IdOrchestrationStepProcessingStatus)
						return false;
					if (obj1.TraceCorrelationId != obj2.TraceCorrelationId)
						return false;
					if (obj1.IdLogMessage != obj2.IdLogMessage)
						return false;
					if (!string.Equals(obj1.Code, obj2.Code))
						return false;
					if (!string.Equals(obj1.Detail, obj2.Detail))
						return false;
					if (obj1.IdMessageProcessingLog != obj2.IdMessageProcessingLog)
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
				if (!OrchestrationStepProcessingStatus.OrchestrationStepProcessingStatusEqualityComparer.EqualsTo(obj1.OrchestrationStepProcessingStatus, obj2.OrchestrationStepProcessingStatus, comparisonOptions, conds?.GetConditions(x => x.OrchestrationStepProcessingStatus), cache))
					return false;
		}

			return true;
		}

		public static int GetHashCode(
			Orchestrations.Model.OrchestrationStepProcessingLog? obj,
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<Orchestrations.Model.OrchestrationStepProcessingLog>>? conditions = null,
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
		public Action<ComparisonConditions<Orchestrations.Model.OrchestrationStepProcessingLog>>? Conditions { get; }
		public Dictionary<object, HashSet<object>>? Cache { get; }
	
		public OrchestrationStepProcessingLogEqualityComparer(
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<Orchestrations.Model.OrchestrationStepProcessingLog>>? conditions = null,
			Dictionary<object, HashSet<object>>? cache = null)
		{
			Cache = cache ?? [];
			ComparisonOptions = comparisonOptions;
			Conditions = conditions;
		}
	
		public bool Equals(
			Orchestrations.Model.OrchestrationStepProcessingLog? obj1,
			Orchestrations.Model.OrchestrationStepProcessingLog? obj2)
			=> EqualsTo(
				obj1,
				obj2,
				ComparisonOptions,
				Conditions,
				Cache);
	
		public int GetHashCode([DisallowNull] Orchestrations.Model.OrchestrationStepProcessingLog? obj)
			=> GetHashCode(
				obj,
				ComparisonOptions,
				Conditions,
				cache: null);
	}
}
