using Legion;
using Legion.Model.Comparers;
using System.Diagnostics.CodeAnalysis;

namespace Legion.ADF.Audit.Model;

public sealed partial class AuditEntry : Audit.AuditBaseEntity, Legion.Model.IEntity
{
	public bool EqualsTo(
		Audit.Model.AuditEntry? obj,
		ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
		Action<ComparisonConditions<Audit.Model.AuditEntry>>? conditions = null)
		=> AuditEntryEqualityComparer.EqualsTo(
			this,
			obj,
			comparisonOptions,
			conditions,
			cache: null);

	public sealed partial class AuditEntryEqualityComparer : IEqualityComparer<AuditEntry>
	{
		public static bool EqualsTo(
			Audit.Model.AuditEntry? obj1,
			Audit.Model.AuditEntry? obj2,
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<Audit.Model.AuditEntry>>? conditions = null,
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
			
			ComparisonConditions<Audit.Model.AuditEntry>? conds = null;
		
			if ((ComparisonOptions.CompareProperties & comparisonOptions) == ComparisonOptions.CompareProperties)
			{
				if (conditions != null)
				{
					conds = new ComparisonConditions<Audit.Model.AuditEntry>();
					conditions.Invoke(conds);

					if (conds.CanCompare(obj1, nameof(obj1.IdAuditEntry)) && obj1.IdAuditEntry != obj2.IdAuditEntry)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.CreatedUtc)) && obj1.CreatedUtc != obj2.CreatedUtc)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.IdAuditOperation)) && obj1.IdAuditOperation != obj2.IdAuditOperation)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.TableName)) && !string.Equals(obj1.TableName, obj2.TableName))
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.IdUser)) && obj1.IdUser != obj2.IdUser)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.PrimaryKey)) && !string.Equals(obj1.PrimaryKey, obj2.PrimaryKey))
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.OldValues)) && !string.Equals(obj1.OldValues, obj2.OldValues))
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.NewValues)) && !string.Equals(obj1.NewValues, obj2.NewValues))
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.AffectedColumns)) && !string.Equals(obj1.AffectedColumns, obj2.AffectedColumns))
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.AuditCorrelationId)) && obj1.AuditCorrelationId != obj2.AuditCorrelationId)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.TraceFrame)) && !string.Equals(obj1.TraceFrame, obj2.TraceFrame))
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.CorrelationId)) && obj1.CorrelationId != obj2.CorrelationId)
						return false;
				}
				else
				{
					if (obj1.IdAuditEntry != obj2.IdAuditEntry)
						return false;
					if (obj1.CreatedUtc != obj2.CreatedUtc)
						return false;
					if (obj1.IdAuditOperation != obj2.IdAuditOperation)
						return false;
					if (!string.Equals(obj1.TableName, obj2.TableName))
						return false;
					if (obj1.IdUser != obj2.IdUser)
						return false;
					if (!string.Equals(obj1.PrimaryKey, obj2.PrimaryKey))
						return false;
					if (!string.Equals(obj1.OldValues, obj2.OldValues))
						return false;
					if (!string.Equals(obj1.NewValues, obj2.NewValues))
						return false;
					if (!string.Equals(obj1.AffectedColumns, obj2.AffectedColumns))
						return false;
					if (obj1.AuditCorrelationId != obj2.AuditCorrelationId)
						return false;
					if (!string.Equals(obj1.TraceFrame, obj2.TraceFrame))
						return false;
					if (obj1.CorrelationId != obj2.CorrelationId)
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
				if (!AuditOperation.AuditOperationEqualityComparer.EqualsTo(obj1.AuditOperation, obj2.AuditOperation, comparisonOptions, conds?.GetConditions(x => x.AuditOperation), cache))
					return false;
		}

			return true;
		}

		public static int GetHashCode(
			Audit.Model.AuditEntry? obj,
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<Audit.Model.AuditEntry>>? conditions = null,
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
		public Action<ComparisonConditions<Audit.Model.AuditEntry>>? Conditions { get; }
		public Dictionary<object, HashSet<object>>? Cache { get; }
	
		public AuditEntryEqualityComparer(
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<Audit.Model.AuditEntry>>? conditions = null,
			Dictionary<object, HashSet<object>>? cache = null)
		{
			Cache = cache ?? [];
			ComparisonOptions = comparisonOptions;
			Conditions = conditions;
		}
	
		public bool Equals(
			Audit.Model.AuditEntry? obj1,
			Audit.Model.AuditEntry? obj2)
			=> EqualsTo(
				obj1,
				obj2,
				ComparisonOptions,
				Conditions,
				Cache);
	
		public int GetHashCode([DisallowNull] Audit.Model.AuditEntry? obj)
			=> GetHashCode(
				obj,
				ComparisonOptions,
				Conditions,
				cache: null);
	}
}
