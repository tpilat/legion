using Legion;
using Legion.Model.Comparers;
using System.Diagnostics.CodeAnalysis;

namespace Legion.ADF.ServiceBus.Model;

public sealed partial class JobActivity : ServiceBus.ServiceBusBaseEntity, Legion.Model.Concurrence.IConcurrent, Legion.Model.IEntity
{
	public bool EqualsTo(
		ServiceBus.Model.JobActivity? obj,
		ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
		Action<ComparisonConditions<ServiceBus.Model.JobActivity>>? conditions = null)
		=> JobActivityEqualityComparer.EqualsTo(
			this,
			obj,
			comparisonOptions,
			conditions,
			cache: null);

	public sealed partial class JobActivityEqualityComparer : IEqualityComparer<JobActivity>
	{
		public static bool EqualsTo(
			ServiceBus.Model.JobActivity? obj1,
			ServiceBus.Model.JobActivity? obj2,
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<ServiceBus.Model.JobActivity>>? conditions = null,
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
			
			ComparisonConditions<ServiceBus.Model.JobActivity>? conds = null;
		
			if ((ComparisonOptions.CompareProperties & comparisonOptions) == ComparisonOptions.CompareProperties)
			{
				if (conditions != null)
				{
					conds = new ComparisonConditions<ServiceBus.Model.JobActivity>();
					conditions.Invoke(conds);

					if (conds.CanCompare(obj1, nameof(obj1.IdJobActivity)) && obj1.IdJobActivity != obj2.IdJobActivity)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.IdJob)) && obj1.IdJob != obj2.IdJob)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.IdJobStatus)) && obj1.IdJobStatus != obj2.IdJobStatus)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.IdCurrentHost)) && obj1.IdCurrentHost != obj2.IdCurrentHost)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.AttachedToCurrentHostUtc)) && obj1.AttachedToCurrentHostUtc != obj2.AttachedToCurrentHostUtc)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.LastStatusChangedUtc)) && obj1.LastStatusChangedUtc != obj2.LastStatusChangedUtc)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.LastProcessingStartedUtc)) && obj1.LastProcessingStartedUtc != obj2.LastProcessingStartedUtc)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.LastProcessingFinishedUtc)) && obj1.LastProcessingFinishedUtc != obj2.LastProcessingFinishedUtc)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.DelayedToUtc)) && obj1.DelayedToUtc != obj2.DelayedToUtc)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.RowVersion)) && obj1.RowVersion != obj2.RowVersion)
						return false;
				}
				else
				{
					if (obj1.IdJobActivity != obj2.IdJobActivity)
						return false;
					if (obj1.IdJob != obj2.IdJob)
						return false;
					if (obj1.IdJobStatus != obj2.IdJobStatus)
						return false;
					if (obj1.IdCurrentHost != obj2.IdCurrentHost)
						return false;
					if (obj1.AttachedToCurrentHostUtc != obj2.AttachedToCurrentHostUtc)
						return false;
					if (obj1.LastStatusChangedUtc != obj2.LastStatusChangedUtc)
						return false;
					if (obj1.LastProcessingStartedUtc != obj2.LastProcessingStartedUtc)
						return false;
					if (obj1.LastProcessingFinishedUtc != obj2.LastProcessingFinishedUtc)
						return false;
					if (obj1.DelayedToUtc != obj2.DelayedToUtc)
						return false;
					if (obj1.RowVersion != obj2.RowVersion)
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
		}

			return true;
		}

		public static int GetHashCode(
			ServiceBus.Model.JobActivity? obj,
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<ServiceBus.Model.JobActivity>>? conditions = null,
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
		public Action<ComparisonConditions<ServiceBus.Model.JobActivity>>? Conditions { get; }
		public Dictionary<object, HashSet<object>>? Cache { get; }
	
		public JobActivityEqualityComparer(
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<ServiceBus.Model.JobActivity>>? conditions = null,
			Dictionary<object, HashSet<object>>? cache = null)
		{
			Cache = cache ?? [];
			ComparisonOptions = comparisonOptions;
			Conditions = conditions;
		}
	
		public bool Equals(
			ServiceBus.Model.JobActivity? obj1,
			ServiceBus.Model.JobActivity? obj2)
			=> EqualsTo(
				obj1,
				obj2,
				ComparisonOptions,
				Conditions,
				Cache);
	
		public int GetHashCode([DisallowNull] ServiceBus.Model.JobActivity? obj)
			=> GetHashCode(
				obj,
				ComparisonOptions,
				Conditions,
				cache: null);
	}
}
