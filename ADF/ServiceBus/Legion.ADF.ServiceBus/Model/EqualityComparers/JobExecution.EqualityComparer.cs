using Legion;
using Legion.Model.Comparers;
using System.Diagnostics.CodeAnalysis;

namespace Legion.ADF.ServiceBus.Model;

public sealed partial class JobExecution : ServiceBus.ServiceBusBaseEntity, Legion.Model.IEntity
{
	public bool EqualsTo(
		ServiceBus.Model.JobExecution? obj,
		ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
		Action<ComparisonConditions<ServiceBus.Model.JobExecution>>? conditions = null)
		=> JobExecutionEqualityComparer.EqualsTo(
			this,
			obj,
			comparisonOptions,
			conditions,
			cache: null);

	public sealed partial class JobExecutionEqualityComparer : IEqualityComparer<JobExecution>
	{
		public static bool EqualsTo(
			ServiceBus.Model.JobExecution? obj1,
			ServiceBus.Model.JobExecution? obj2,
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<ServiceBus.Model.JobExecution>>? conditions = null,
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
			
			ComparisonConditions<ServiceBus.Model.JobExecution>? conds = null;
		
			if ((ComparisonOptions.CompareProperties & comparisonOptions) == ComparisonOptions.CompareProperties)
			{
				if (conditions != null)
				{
					conds = new ComparisonConditions<ServiceBus.Model.JobExecution>();
					conditions.Invoke(conds);

					if (conds.CanCompare(obj1, nameof(obj1.IdJobExecution)) && obj1.IdJobExecution != obj2.IdJobExecution)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.IdJob)) && obj1.IdJob != obj2.IdJob)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.TraceCorrelationId)) && obj1.TraceCorrelationId != obj2.TraceCorrelationId)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.StartUtc)) && obj1.StartUtc != obj2.StartUtc)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.EndUtc)) && obj1.EndUtc != obj2.EndUtc)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.IdJobStatus)) && obj1.IdJobStatus != obj2.IdJobStatus)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.StatisticsStartHourUtc)) && obj1.StatisticsStartHourUtc != obj2.StatisticsStartHourUtc)
						return false;
				}
				else
				{
					if (obj1.IdJobExecution != obj2.IdJobExecution)
						return false;
					if (obj1.IdJob != obj2.IdJob)
						return false;
					if (obj1.TraceCorrelationId != obj2.TraceCorrelationId)
						return false;
					if (obj1.StartUtc != obj2.StartUtc)
						return false;
					if (obj1.EndUtc != obj2.EndUtc)
						return false;
					if (obj1.IdJobStatus != obj2.IdJobStatus)
						return false;
					if (obj1.StatisticsStartHourUtc != obj2.StatisticsStartHourUtc)
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
				if (!JobStatus.JobStatusEqualityComparer.EqualsTo(obj1.JobStatus, obj2.JobStatus, comparisonOptions, conds?.GetConditions(x => x.JobStatus), cache))
					return false;
				if (!ComparisonHelper.SequenceEqual(obj1.JobLogs, obj2.JobLogs, new JobLog.JobLogEqualityComparer(comparisonOptions, conds?.GetConditions(x => x.JobLogs), cache)))
					return false;
		}

			return true;
		}

		public static int GetHashCode(
			ServiceBus.Model.JobExecution? obj,
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<ServiceBus.Model.JobExecution>>? conditions = null,
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
		public Action<ComparisonConditions<ServiceBus.Model.JobExecution>>? Conditions { get; }
		public Dictionary<object, HashSet<object>>? Cache { get; }
	
		public JobExecutionEqualityComparer(
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<ServiceBus.Model.JobExecution>>? conditions = null,
			Dictionary<object, HashSet<object>>? cache = null)
		{
			Cache = cache ?? [];
			ComparisonOptions = comparisonOptions;
			Conditions = conditions;
		}
	
		public bool Equals(
			ServiceBus.Model.JobExecution? obj1,
			ServiceBus.Model.JobExecution? obj2)
			=> EqualsTo(
				obj1,
				obj2,
				ComparisonOptions,
				Conditions,
				Cache);
	
		public int GetHashCode([DisallowNull] ServiceBus.Model.JobExecution? obj)
			=> GetHashCode(
				obj,
				ComparisonOptions,
				Conditions,
				cache: null);
	}
}
