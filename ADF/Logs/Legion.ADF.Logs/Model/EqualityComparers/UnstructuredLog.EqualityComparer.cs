using Legion;
using Legion.Model.Comparers;
using System.Diagnostics.CodeAnalysis;

namespace Legion.ADF.Logs.Model;

public sealed partial class UnstructuredLog : Logs.LogsBaseEntity, Legion.Model.IEntity
{
	public bool EqualsTo(
		Logs.Model.UnstructuredLog? obj,
		ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
		Action<ComparisonConditions<Logs.Model.UnstructuredLog>>? conditions = null)
		=> UnstructuredLogEqualityComparer.EqualsTo(
			this,
			obj,
			comparisonOptions,
			conditions,
			cache: null);

	public sealed partial class UnstructuredLogEqualityComparer : IEqualityComparer<UnstructuredLog>
	{
		public static bool EqualsTo(
			Logs.Model.UnstructuredLog? obj1,
			Logs.Model.UnstructuredLog? obj2,
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<Logs.Model.UnstructuredLog>>? conditions = null,
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
			
			ComparisonConditions<Logs.Model.UnstructuredLog>? conds = null;
		
			if ((ComparisonOptions.CompareProperties & comparisonOptions) == ComparisonOptions.CompareProperties)
			{
				if (conditions != null)
				{
					conds = new ComparisonConditions<Logs.Model.UnstructuredLog>();
					conditions.Invoke(conds);

					if (conds.CanCompare(obj1, nameof(obj1.IdUnstructuredLog)) && obj1.IdUnstructuredLog != obj2.IdUnstructuredLog)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.CreatedUtc)) && obj1.CreatedUtc != obj2.CreatedUtc)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.IdLogLevel)) && obj1.IdLogLevel != obj2.IdLogLevel)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.Message)) && !string.Equals(obj1.Message, obj2.Message))
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.StackTrace)) && !string.Equals(obj1.StackTrace, obj2.StackTrace))
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.SourceContext)) && !string.Equals(obj1.SourceContext, obj2.SourceContext))
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.RuntimeUniqueKey)) && obj1.RuntimeUniqueKey != obj2.RuntimeUniqueKey)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.EventName)) && !string.Equals(obj1.EventName, obj2.EventName))
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.EventId)) && obj1.EventId != obj2.EventId)
						return false;
				}
				else
				{
					if (obj1.IdUnstructuredLog != obj2.IdUnstructuredLog)
						return false;
					if (obj1.CreatedUtc != obj2.CreatedUtc)
						return false;
					if (obj1.IdLogLevel != obj2.IdLogLevel)
						return false;
					if (!string.Equals(obj1.Message, obj2.Message))
						return false;
					if (!string.Equals(obj1.StackTrace, obj2.StackTrace))
						return false;
					if (!string.Equals(obj1.SourceContext, obj2.SourceContext))
						return false;
					if (obj1.RuntimeUniqueKey != obj2.RuntimeUniqueKey)
						return false;
					if (!string.Equals(obj1.EventName, obj2.EventName))
						return false;
					if (obj1.EventId != obj2.EventId)
						return false;
				}
			}

			if (cachedHashSet == null)
			{
				cachedHashSet = [];
				cache[obj1] = cachedHashSet;
			}

			cachedHashSet.Add(obj2);

			return true;
		}

		public static int GetHashCode(
			Logs.Model.UnstructuredLog? obj,
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<Logs.Model.UnstructuredLog>>? conditions = null,
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
		public Action<ComparisonConditions<Logs.Model.UnstructuredLog>>? Conditions { get; }
		public Dictionary<object, HashSet<object>>? Cache { get; }
	
		public UnstructuredLogEqualityComparer(
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<Logs.Model.UnstructuredLog>>? conditions = null,
			Dictionary<object, HashSet<object>>? cache = null)
		{
			Cache = cache ?? [];
			ComparisonOptions = comparisonOptions;
			Conditions = conditions;
		}
	
		public bool Equals(
			Logs.Model.UnstructuredLog? obj1,
			Logs.Model.UnstructuredLog? obj2)
			=> EqualsTo(
				obj1,
				obj2,
				ComparisonOptions,
				Conditions,
				Cache);
	
		public int GetHashCode([DisallowNull] Logs.Model.UnstructuredLog? obj)
			=> GetHashCode(
				obj,
				ComparisonOptions,
				Conditions,
				cache: null);
	}
}
