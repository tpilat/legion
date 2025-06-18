using Legion;
using Legion.Model.Comparers;
using System.Diagnostics.CodeAnalysis;

namespace Legion.ADF.Audit.Model;

public sealed partial class ApplicationEntryToken : Audit.AuditBaseEntity, Legion.Model.IEntity
{
	public bool EqualsTo(
		Audit.Model.ApplicationEntryToken? obj,
		ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
		Action<ComparisonConditions<Audit.Model.ApplicationEntryToken>>? conditions = null)
		=> ApplicationEntryTokenEqualityComparer.EqualsTo(
			this,
			obj,
			comparisonOptions,
			conditions,
			cache: null);

	public sealed partial class ApplicationEntryTokenEqualityComparer : IEqualityComparer<ApplicationEntryToken>
	{
		public static bool EqualsTo(
			Audit.Model.ApplicationEntryToken? obj1,
			Audit.Model.ApplicationEntryToken? obj2,
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<Audit.Model.ApplicationEntryToken>>? conditions = null,
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
			
			ComparisonConditions<Audit.Model.ApplicationEntryToken>? conds = null;
		
			if ((ComparisonOptions.CompareProperties & comparisonOptions) == ComparisonOptions.CompareProperties)
			{
				if (conditions != null)
				{
					conds = new ComparisonConditions<Audit.Model.ApplicationEntryToken>();
					conditions.Invoke(conds);

					if (conds.CanCompare(obj1, nameof(obj1.IdApplicationEntryToken)) && obj1.IdApplicationEntryToken != obj2.IdApplicationEntryToken)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.Token)) && !string.Equals(obj1.Token, obj2.Token))
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.SourceFilePath)) && !string.Equals(obj1.SourceFilePath, obj2.SourceFilePath))
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.MethodInfo)) && !string.Equals(obj1.MethodInfo, obj2.MethodInfo))
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.AggregateName)) && !string.Equals(obj1.AggregateName, obj2.AggregateName))
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.Description)) && !string.Equals(obj1.Description, obj2.Description))
						return false;
				}
				else
				{
					if (obj1.IdApplicationEntryToken != obj2.IdApplicationEntryToken)
						return false;
					if (!string.Equals(obj1.Token, obj2.Token))
						return false;
					if (!string.Equals(obj1.SourceFilePath, obj2.SourceFilePath))
						return false;
					if (!string.Equals(obj1.MethodInfo, obj2.MethodInfo))
						return false;
					if (!string.Equals(obj1.AggregateName, obj2.AggregateName))
						return false;
					if (!string.Equals(obj1.Description, obj2.Description))
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
				if (!ComparisonHelper.SequenceEqual(obj1.ApplicationEntries, obj2.ApplicationEntries, new ApplicationEntry.ApplicationEntryEqualityComparer(comparisonOptions, conds?.GetConditions(x => x.ApplicationEntries), cache)))
					return false;
		}

			return true;
		}

		public static int GetHashCode(
			Audit.Model.ApplicationEntryToken? obj,
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<Audit.Model.ApplicationEntryToken>>? conditions = null,
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
		public Action<ComparisonConditions<Audit.Model.ApplicationEntryToken>>? Conditions { get; }
		public Dictionary<object, HashSet<object>>? Cache { get; }
	
		public ApplicationEntryTokenEqualityComparer(
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<Audit.Model.ApplicationEntryToken>>? conditions = null,
			Dictionary<object, HashSet<object>>? cache = null)
		{
			Cache = cache ?? [];
			ComparisonOptions = comparisonOptions;
			Conditions = conditions;
		}
	
		public bool Equals(
			Audit.Model.ApplicationEntryToken? obj1,
			Audit.Model.ApplicationEntryToken? obj2)
			=> EqualsTo(
				obj1,
				obj2,
				ComparisonOptions,
				Conditions,
				Cache);
	
		public int GetHashCode([DisallowNull] Audit.Model.ApplicationEntryToken? obj)
			=> GetHashCode(
				obj,
				ComparisonOptions,
				Conditions,
				cache: null);
	}
}
