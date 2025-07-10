using Legion;
using Legion.Model.Comparers;
using System.Diagnostics.CodeAnalysis;

namespace Legion.ADF.ServiceBus.Model;

public sealed partial class OrchestrationStepProcessing : ServiceBus.ServiceBusBaseEntity, Legion.Model.IEntity
{
	public bool EqualsTo(
		ServiceBus.Model.OrchestrationStepProcessing? obj,
		ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
		Action<ComparisonConditions<ServiceBus.Model.OrchestrationStepProcessing>>? conditions = null)
		=> OrchestrationStepProcessingEqualityComparer.EqualsTo(
			this,
			obj,
			comparisonOptions,
			conditions,
			cache: null);

	public sealed partial class OrchestrationStepProcessingEqualityComparer : IEqualityComparer<OrchestrationStepProcessing>
	{
		public static bool EqualsTo(
			ServiceBus.Model.OrchestrationStepProcessing? obj1,
			ServiceBus.Model.OrchestrationStepProcessing? obj2,
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<ServiceBus.Model.OrchestrationStepProcessing>>? conditions = null,
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
			
			ComparisonConditions<ServiceBus.Model.OrchestrationStepProcessing>? conds = null;
		
			if ((ComparisonOptions.CompareProperties & comparisonOptions) == ComparisonOptions.CompareProperties)
			{
				if (conditions != null)
				{
					conds = new ComparisonConditions<ServiceBus.Model.OrchestrationStepProcessing>();
					conditions.Invoke(conds);

					if (conds.CanCompare(obj1, nameof(obj1.IdOrchestrationStepProcessing)) && obj1.IdOrchestrationStepProcessing != obj2.IdOrchestrationStepProcessing)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.IdOrchestrationStep)) && obj1.IdOrchestrationStep != obj2.IdOrchestrationStep)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.IdOrchestrationInstance)) && obj1.IdOrchestrationInstance != obj2.IdOrchestrationInstance)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.IdOrchestrationStepProcessingStatus)) && obj1.IdOrchestrationStepProcessingStatus != obj2.IdOrchestrationStepProcessingStatus)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.CreatedUtc)) && obj1.CreatedUtc != obj2.CreatedUtc)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.ProcessedUtc)) && obj1.ProcessedUtc != obj2.ProcessedUtc)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.SuspendedUtc)) && obj1.SuspendedUtc != obj2.SuspendedUtc)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.LastProcessingUtc)) && obj1.LastProcessingUtc != obj2.LastProcessingUtc)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.NextProcessingUtc)) && obj1.NextProcessingUtc != obj2.NextProcessingUtc)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.RetryCount)) && obj1.RetryCount != obj2.RetryCount)
						return false;
				}
				else
				{
					if (obj1.IdOrchestrationStepProcessing != obj2.IdOrchestrationStepProcessing)
						return false;
					if (obj1.IdOrchestrationStep != obj2.IdOrchestrationStep)
						return false;
					if (obj1.IdOrchestrationInstance != obj2.IdOrchestrationInstance)
						return false;
					if (obj1.IdOrchestrationStepProcessingStatus != obj2.IdOrchestrationStepProcessingStatus)
						return false;
					if (obj1.CreatedUtc != obj2.CreatedUtc)
						return false;
					if (obj1.ProcessedUtc != obj2.ProcessedUtc)
						return false;
					if (obj1.SuspendedUtc != obj2.SuspendedUtc)
						return false;
					if (obj1.LastProcessingUtc != obj2.LastProcessingUtc)
						return false;
					if (obj1.NextProcessingUtc != obj2.NextProcessingUtc)
						return false;
					if (obj1.RetryCount != obj2.RetryCount)
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
				if (!OrchestrationInstance.OrchestrationInstanceEqualityComparer.EqualsTo(obj1.OrchestrationInstance, obj2.OrchestrationInstance, comparisonOptions, conds?.GetConditions(x => x.OrchestrationInstance), cache))
					return false;
				if (!OrchestrationStep.OrchestrationStepEqualityComparer.EqualsTo(obj1.OrchestrationStep, obj2.OrchestrationStep, comparisonOptions, conds?.GetConditions(x => x.OrchestrationStep), cache))
					return false;
				if (!OrchestrationStepProcessingStatus.OrchestrationStepProcessingStatusEqualityComparer.EqualsTo(obj1.OrchestrationStepProcessingStatus, obj2.OrchestrationStepProcessingStatus, comparisonOptions, conds?.GetConditions(x => x.OrchestrationStepProcessingStatus), cache))
					return false;
				if (!ComparisonHelper.SequenceEqual(obj1.OrchestrationStepProcessingDirections, obj2.OrchestrationStepProcessingDirections, new OrchestrationStepProcessingDirection.OrchestrationStepProcessingDirectionEqualityComparer(comparisonOptions, conds?.GetConditions(x => x.OrchestrationStepProcessingDirections), cache)))
					return false;
				if (!ComparisonHelper.SequenceEqual(obj1.OrchestrationStepProcessingLogs, obj2.OrchestrationStepProcessingLogs, new OrchestrationStepProcessingLog.OrchestrationStepProcessingLogEqualityComparer(comparisonOptions, conds?.GetConditions(x => x.OrchestrationStepProcessingLogs), cache)))
					return false;
				if (!ComparisonHelper.SequenceEqual(obj1.OrchestrationStepProcessingMessages, obj2.OrchestrationStepProcessingMessages, new OrchestrationStepProcessingMessage.OrchestrationStepProcessingMessageEqualityComparer(comparisonOptions, conds?.GetConditions(x => x.OrchestrationStepProcessingMessages), cache)))
					return false;
				if (!ComparisonHelper.SequenceEqual(obj1.ToStepOrchestrationStepProcessingDirections, obj2.ToStepOrchestrationStepProcessingDirections, new OrchestrationStepProcessingDirection.OrchestrationStepProcessingDirectionEqualityComparer(comparisonOptions, conds?.GetConditions(x => x.ToStepOrchestrationStepProcessingDirections), cache)))
					return false;
		}

			return true;
		}

		public static int GetHashCode(
			ServiceBus.Model.OrchestrationStepProcessing? obj,
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<ServiceBus.Model.OrchestrationStepProcessing>>? conditions = null,
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
		public Action<ComparisonConditions<ServiceBus.Model.OrchestrationStepProcessing>>? Conditions { get; }
		public Dictionary<object, HashSet<object>>? Cache { get; }
	
		public OrchestrationStepProcessingEqualityComparer(
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<ServiceBus.Model.OrchestrationStepProcessing>>? conditions = null,
			Dictionary<object, HashSet<object>>? cache = null)
		{
			Cache = cache ?? [];
			ComparisonOptions = comparisonOptions;
			Conditions = conditions;
		}
	
		public bool Equals(
			ServiceBus.Model.OrchestrationStepProcessing? obj1,
			ServiceBus.Model.OrchestrationStepProcessing? obj2)
			=> EqualsTo(
				obj1,
				obj2,
				ComparisonOptions,
				Conditions,
				Cache);
	
		public int GetHashCode([DisallowNull] ServiceBus.Model.OrchestrationStepProcessing? obj)
			=> GetHashCode(
				obj,
				ComparisonOptions,
				Conditions,
				cache: null);
	}
}
