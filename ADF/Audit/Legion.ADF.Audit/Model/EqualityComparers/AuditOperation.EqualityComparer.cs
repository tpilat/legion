using Legion;
using Legion.Model.Comparers;
using System.Diagnostics.CodeAnalysis;

namespace Legion.ADF.Audit.Model;

public sealed partial class AuditOperation : Audit.AuditBaseEntity, Legion.Model.IEntity
{
	public bool EqualsTo(
		Audit.Model.AuditOperation? obj,
		ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
		Action<ComparisonConditions<Audit.Model.AuditOperation>>? conditions = null)
		=> AuditOperationEqualityComparer.EqualsTo(
			this,
			obj,
			comparisonOptions,
			conditions,
			cache: null);

	public sealed partial class AuditOperationEqualityComparer : IEqualityComparer<AuditOperation>
	{
		public static bool EqualsTo(
			Audit.Model.AuditOperation? obj1,
			Audit.Model.AuditOperation? obj2,
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<Audit.Model.AuditOperation>>? conditions = null,
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
			
			ComparisonConditions<Audit.Model.AuditOperation>? conds = null;
		
			if ((ComparisonOptions.CompareProperties & comparisonOptions) == ComparisonOptions.CompareProperties)
			{
				if (conditions != null)
				{
					conds = new ComparisonConditions<Audit.Model.AuditOperation>();
					conditions.Invoke(conds);
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
				if (!ComparisonHelper.SequenceEqual(obj1.ApplicationEntries, obj2.ApplicationEntries, new ApplicationEntry.ApplicationEntryEqualityComparer(comparisonOptions, conds?.GetConditions(x => x.ApplicationEntries), cache)))
					return false;
				if (!ComparisonHelper.SequenceEqual(obj1.AuditEntries, obj2.AuditEntries, new AuditEntry.AuditEntryEqualityComparer(comparisonOptions, conds?.GetConditions(x => x.AuditEntries), cache)))
					return false;
		}

			return true;
		}

		public static int GetHashCode(
			Audit.Model.AuditOperation? obj,
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<Audit.Model.AuditOperation>>? conditions = null,
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
		public Action<ComparisonConditions<Audit.Model.AuditOperation>>? Conditions { get; }
		public Dictionary<object, HashSet<object>>? Cache { get; }
	
		public AuditOperationEqualityComparer(
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<Audit.Model.AuditOperation>>? conditions = null,
			Dictionary<object, HashSet<object>>? cache = null)
		{
			Cache = cache ?? [];
			ComparisonOptions = comparisonOptions;
			Conditions = conditions;
		}
	
		public bool Equals(
			Audit.Model.AuditOperation? obj1,
			Audit.Model.AuditOperation? obj2)
			=> EqualsTo(
				obj1,
				obj2,
				ComparisonOptions,
				Conditions,
				Cache);
	
		public int GetHashCode([DisallowNull] Audit.Model.AuditOperation? obj)
			=> GetHashCode(
				obj,
				ComparisonOptions,
				Conditions,
				cache: null);
	}
}
