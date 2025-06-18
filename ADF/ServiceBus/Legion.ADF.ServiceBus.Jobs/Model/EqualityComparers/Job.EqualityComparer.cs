using Legion;
using Legion.Model.Comparers;
using System.Diagnostics.CodeAnalysis;

namespace Legion.ADF.ServiceBus.Jobs.Model;

public sealed partial class Job : Jobs.JobsBaseEntity, Legion.Model.IEntity
{
	public bool EqualsTo(
		Jobs.Model.Job? obj,
		ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
		Action<ComparisonConditions<Jobs.Model.Job>>? conditions = null)
		=> JobEqualityComparer.EqualsTo(
			this,
			obj,
			comparisonOptions,
			conditions,
			cache: null);

	public sealed partial class JobEqualityComparer : IEqualityComparer<Job>
	{
		public static bool EqualsTo(
			Jobs.Model.Job? obj1,
			Jobs.Model.Job? obj2,
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<Jobs.Model.Job>>? conditions = null,
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
			
			ComparisonConditions<Jobs.Model.Job>? conds = null;
		
			if ((ComparisonOptions.CompareProperties & comparisonOptions) == ComparisonOptions.CompareProperties)
			{
				if (conditions != null)
				{
					conds = new ComparisonConditions<Jobs.Model.Job>();
					conditions.Invoke(conds);

					if (conds.CanCompare(obj1, nameof(obj1.IdJob)) && obj1.IdJob != obj2.IdJob)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.Name)) && !string.Equals(obj1.Name, obj2.Name))
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.Description)) && !string.Equals(obj1.Description, obj2.Description))
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.IdJobRunType)) && obj1.IdJobRunType != obj2.IdJobRunType)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.IdJobStatus)) && obj1.IdJobStatus != obj2.IdJobStatus)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.Namespace)) && !string.Equals(obj1.Namespace, obj2.Namespace))
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.Properties)) && !string.Equals(obj1.Properties, obj2.Properties))
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.DelayedStartInSeconds)) && obj1.DelayedStartInSeconds != obj2.DelayedStartInSeconds)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.IdleTimeoutInSeconds)) && obj1.IdleTimeoutInSeconds != obj2.IdleTimeoutInSeconds)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.CronExpression)) && !string.Equals(obj1.CronExpression, obj2.CronExpression))
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.CronExpressionIncludeSeconds)) && obj1.CronExpressionIncludeSeconds != obj2.CronExpressionIncludeSeconds)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.LastProcessingUtc)) && obj1.LastProcessingUtc != obj2.LastProcessingUtc)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.NextProcessinUtc)) && obj1.NextProcessinUtc != obj2.NextProcessinUtc)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.TimeoutForProcessingInSeconds)) && obj1.TimeoutForProcessingInSeconds != obj2.TimeoutForProcessingInSeconds)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.MaxProcessingRetryCount)) && obj1.MaxProcessingRetryCount != obj2.MaxProcessingRetryCount)
						return false;
				}
				else
				{
					if (obj1.IdJob != obj2.IdJob)
						return false;
					if (!string.Equals(obj1.Name, obj2.Name))
						return false;
					if (!string.Equals(obj1.Description, obj2.Description))
						return false;
					if (obj1.IdJobRunType != obj2.IdJobRunType)
						return false;
					if (obj1.IdJobStatus != obj2.IdJobStatus)
						return false;
					if (!string.Equals(obj1.Namespace, obj2.Namespace))
						return false;
					if (!string.Equals(obj1.Properties, obj2.Properties))
						return false;
					if (obj1.DelayedStartInSeconds != obj2.DelayedStartInSeconds)
						return false;
					if (obj1.IdleTimeoutInSeconds != obj2.IdleTimeoutInSeconds)
						return false;
					if (!string.Equals(obj1.CronExpression, obj2.CronExpression))
						return false;
					if (obj1.CronExpressionIncludeSeconds != obj2.CronExpressionIncludeSeconds)
						return false;
					if (obj1.LastProcessingUtc != obj2.LastProcessingUtc)
						return false;
					if (obj1.NextProcessinUtc != obj2.NextProcessinUtc)
						return false;
					if (obj1.TimeoutForProcessingInSeconds != obj2.TimeoutForProcessingInSeconds)
						return false;
					if (obj1.MaxProcessingRetryCount != obj2.MaxProcessingRetryCount)
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
				if (!JobRunType.JobRunTypeEqualityComparer.EqualsTo(obj1.JobRunType, obj2.JobRunType, comparisonOptions, conds?.GetConditions(x => x.JobRunType), cache))
					return false;
				if (!JobStatus.JobStatusEqualityComparer.EqualsTo(obj1.JobStatus, obj2.JobStatus, comparisonOptions, conds?.GetConditions(x => x.JobStatus), cache))
					return false;
				if (!ComparisonHelper.SequenceEqual(obj1.JobDatas, obj2.JobDatas, new JobData.JobDataEqualityComparer(comparisonOptions, conds?.GetConditions(x => x.JobDatas), cache)))
					return false;
				if (!ComparisonHelper.SequenceEqual(obj1.JobExecutions, obj2.JobExecutions, new JobExecution.JobExecutionEqualityComparer(comparisonOptions, conds?.GetConditions(x => x.JobExecutions), cache)))
					return false;
				if (!ComparisonHelper.SequenceEqual(obj1.JobLogs, obj2.JobLogs, new JobLog.JobLogEqualityComparer(comparisonOptions, conds?.GetConditions(x => x.JobLogs), cache)))
					return false;
				if (!ComparisonHelper.SequenceEqual(obj1.JobMessages, obj2.JobMessages, new JobMessage.JobMessageEqualityComparer(comparisonOptions, conds?.GetConditions(x => x.JobMessages), cache)))
					return false;
				if (!ComparisonHelper.SequenceEqual(obj1.JobStatistics, obj2.JobStatistics, new JobStatistics.JobStatisticsEqualityComparer(comparisonOptions, conds?.GetConditions(x => x.JobStatistics), cache)))
					return false;
		}

			return true;
		}

		public static int GetHashCode(
			Jobs.Model.Job? obj,
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<Jobs.Model.Job>>? conditions = null,
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
		public Action<ComparisonConditions<Jobs.Model.Job>>? Conditions { get; }
		public Dictionary<object, HashSet<object>>? Cache { get; }
	
		public JobEqualityComparer(
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<Jobs.Model.Job>>? conditions = null,
			Dictionary<object, HashSet<object>>? cache = null)
		{
			Cache = cache ?? [];
			ComparisonOptions = comparisonOptions;
			Conditions = conditions;
		}
	
		public bool Equals(
			Jobs.Model.Job? obj1,
			Jobs.Model.Job? obj2)
			=> EqualsTo(
				obj1,
				obj2,
				ComparisonOptions,
				Conditions,
				Cache);
	
		public int GetHashCode([DisallowNull] Jobs.Model.Job? obj)
			=> GetHashCode(
				obj,
				ComparisonOptions,
				Conditions,
				cache: null);
	}
}
