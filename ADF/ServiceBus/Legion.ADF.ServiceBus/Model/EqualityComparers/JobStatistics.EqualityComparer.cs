using Legion;
using Legion.Model.Comparers;
using System.Diagnostics.CodeAnalysis;

namespace Legion.ADF.ServiceBus.Model;

public sealed partial class JobStatistics : ServiceBus.ServiceBusBaseEntity, Legion.Model.IEntity
{
	public bool EqualsTo(
		ServiceBus.Model.JobStatistics? obj,
		ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
		Action<ComparisonConditions<ServiceBus.Model.JobStatistics>>? conditions = null)
		=> JobStatisticsEqualityComparer.EqualsTo(
			this,
			obj,
			comparisonOptions,
			conditions,
			cache: null);

	public sealed partial class JobStatisticsEqualityComparer : IEqualityComparer<JobStatistics>
	{
		public static bool EqualsTo(
			ServiceBus.Model.JobStatistics? obj1,
			ServiceBus.Model.JobStatistics? obj2,
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<ServiceBus.Model.JobStatistics>>? conditions = null,
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
			
			ComparisonConditions<ServiceBus.Model.JobStatistics>? conds = null;
		
			if ((ComparisonOptions.CompareProperties & comparisonOptions) == ComparisonOptions.CompareProperties)
			{
				if (conditions != null)
				{
					conds = new ComparisonConditions<ServiceBus.Model.JobStatistics>();
					conditions.Invoke(conds);

					if (conds.CanCompare(obj1, nameof(obj1.IdJobStatistics)) && obj1.IdJobStatistics != obj2.IdJobStatistics)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.IdJob)) && obj1.IdJob != obj2.IdJob)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.StartHourUtc)) && obj1.StartHourUtc != obj2.StartHourUtc)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.ExecutionCount)) && obj1.ExecutionCount != obj2.ExecutionCount)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.ErrorCount)) && obj1.ErrorCount != obj2.ErrorCount)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.DurationSumInSeconds)) && obj1.DurationSumInSeconds != obj2.DurationSumInSeconds)
						return false;
				}
				else
				{
					if (obj1.IdJobStatistics != obj2.IdJobStatistics)
						return false;
					if (obj1.IdJob != obj2.IdJob)
						return false;
					if (obj1.StartHourUtc != obj2.StartHourUtc)
						return false;
					if (obj1.ExecutionCount != obj2.ExecutionCount)
						return false;
					if (obj1.ErrorCount != obj2.ErrorCount)
						return false;
					if (obj1.DurationSumInSeconds != obj2.DurationSumInSeconds)
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
				if (!Job.JobEqualityComparer.EqualsTo(obj1.Job, obj2.Job, comparisonOptions, conds?.GetConditions(x => x.Job), cache))
					return false;
		}

			return true;
		}

		public static int GetHashCode(
			ServiceBus.Model.JobStatistics? obj,
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<ServiceBus.Model.JobStatistics>>? conditions = null,
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
		public Action<ComparisonConditions<ServiceBus.Model.JobStatistics>>? Conditions { get; }
		public Dictionary<object, HashSet<object>>? Cache { get; }
	
		public JobStatisticsEqualityComparer(
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<ServiceBus.Model.JobStatistics>>? conditions = null,
			Dictionary<object, HashSet<object>>? cache = null)
		{
			Cache = cache ?? [];
			ComparisonOptions = comparisonOptions;
			Conditions = conditions;
		}
	
		public bool Equals(
			ServiceBus.Model.JobStatistics? obj1,
			ServiceBus.Model.JobStatistics? obj2)
			=> EqualsTo(
				obj1,
				obj2,
				ComparisonOptions,
				Conditions,
				Cache);
	
		public int GetHashCode([DisallowNull] ServiceBus.Model.JobStatistics? obj)
			=> GetHashCode(
				obj,
				ComparisonOptions,
				Conditions,
				cache: null);
	}
}
