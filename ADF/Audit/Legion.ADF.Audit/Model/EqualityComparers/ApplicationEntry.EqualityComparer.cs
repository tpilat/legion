using Legion;
using Legion.Model.Comparers;
using System.Diagnostics.CodeAnalysis;

namespace Legion.ADF.Audit.Model;

public sealed partial class ApplicationEntry : Audit.AuditBaseEntity, Legion.Model.IEntity
{
	public bool EqualsTo(
		Audit.Model.ApplicationEntry? obj,
		ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
		Action<ComparisonConditions<Audit.Model.ApplicationEntry>>? conditions = null)
		=> ApplicationEntryEqualityComparer.EqualsTo(
			this,
			obj,
			comparisonOptions,
			conditions,
			cache: null);

	public sealed partial class ApplicationEntryEqualityComparer : IEqualityComparer<ApplicationEntry>
	{
		public static bool EqualsTo(
			Audit.Model.ApplicationEntry? obj1,
			Audit.Model.ApplicationEntry? obj2,
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<Audit.Model.ApplicationEntry>>? conditions = null,
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
			
			ComparisonConditions<Audit.Model.ApplicationEntry>? conds = null;
		
			if ((ComparisonOptions.CompareProperties & comparisonOptions) == ComparisonOptions.CompareProperties)
			{
				if (conditions != null)
				{
					conds = new ComparisonConditions<Audit.Model.ApplicationEntry>();
					conditions.Invoke(conds);

					if (conds.CanCompare(obj1, nameof(obj1.IdApplicationEntry)) && obj1.IdApplicationEntry != obj2.IdApplicationEntry)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.IdApplicationEntryToken)) && obj1.IdApplicationEntryToken != obj2.IdApplicationEntryToken)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.IdAuditOperation)) && obj1.IdAuditOperation != obj2.IdAuditOperation)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.RuntimeUniqueKey)) && obj1.RuntimeUniqueKey != obj2.RuntimeUniqueKey)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.CreatedUtc)) && obj1.CreatedUtc != obj2.CreatedUtc)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.CorrelationId)) && obj1.CorrelationId != obj2.CorrelationId)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.ExternalCorrelationId)) && !string.Equals(obj1.ExternalCorrelationId, obj2.ExternalCorrelationId))
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.AggregateIdentifier)) && !string.Equals(obj1.AggregateIdentifier, obj2.AggregateIdentifier))
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.HttpMethod)) && !string.Equals(obj1.HttpMethod, obj2.HttpMethod))
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.Uri)) && !string.Equals(obj1.Uri, obj2.Uri))
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.IdUser)) && obj1.IdUser != obj2.IdUser)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.TenantIdentifier)) && obj1.TenantIdentifier != obj2.TenantIdentifier)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.RemoteIP)) && !string.Equals(obj1.RemoteIP, obj2.RemoteIP))
						return false;
				}
				else
				{
					if (obj1.IdApplicationEntry != obj2.IdApplicationEntry)
						return false;
					if (obj1.IdApplicationEntryToken != obj2.IdApplicationEntryToken)
						return false;
					if (obj1.IdAuditOperation != obj2.IdAuditOperation)
						return false;
					if (obj1.RuntimeUniqueKey != obj2.RuntimeUniqueKey)
						return false;
					if (obj1.CreatedUtc != obj2.CreatedUtc)
						return false;
					if (obj1.CorrelationId != obj2.CorrelationId)
						return false;
					if (!string.Equals(obj1.ExternalCorrelationId, obj2.ExternalCorrelationId))
						return false;
					if (!string.Equals(obj1.AggregateIdentifier, obj2.AggregateIdentifier))
						return false;
					if (!string.Equals(obj1.HttpMethod, obj2.HttpMethod))
						return false;
					if (!string.Equals(obj1.Uri, obj2.Uri))
						return false;
					if (obj1.IdUser != obj2.IdUser)
						return false;
					if (obj1.TenantIdentifier != obj2.TenantIdentifier)
						return false;
					if (!string.Equals(obj1.RemoteIP, obj2.RemoteIP))
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
				if (!ApplicationEntryToken.ApplicationEntryTokenEqualityComparer.EqualsTo(obj1.ApplicationEntryToken, obj2.ApplicationEntryToken, comparisonOptions, conds?.GetConditions(x => x.ApplicationEntryToken), cache))
					return false;
				if (!AuditOperation.AuditOperationEqualityComparer.EqualsTo(obj1.AuditOperation, obj2.AuditOperation, comparisonOptions, conds?.GetConditions(x => x.AuditOperation), cache))
					return false;
				if (!ComparisonHelper.SequenceEqual(obj1.ApplicationEntryRequests, obj2.ApplicationEntryRequests, new ApplicationEntryRequest.ApplicationEntryRequestEqualityComparer(comparisonOptions, conds?.GetConditions(x => x.ApplicationEntryRequests), cache)))
					return false;
				if (!ComparisonHelper.SequenceEqual(obj1.ApplicationEntryResponses, obj2.ApplicationEntryResponses, new ApplicationEntryResponse.ApplicationEntryResponseEqualityComparer(comparisonOptions, conds?.GetConditions(x => x.ApplicationEntryResponses), cache)))
					return false;
		}

			return true;
		}

		public static int GetHashCode(
			Audit.Model.ApplicationEntry? obj,
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<Audit.Model.ApplicationEntry>>? conditions = null,
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
		public Action<ComparisonConditions<Audit.Model.ApplicationEntry>>? Conditions { get; }
		public Dictionary<object, HashSet<object>>? Cache { get; }
	
		public ApplicationEntryEqualityComparer(
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<Audit.Model.ApplicationEntry>>? conditions = null,
			Dictionary<object, HashSet<object>>? cache = null)
		{
			Cache = cache ?? [];
			ComparisonOptions = comparisonOptions;
			Conditions = conditions;
		}
	
		public bool Equals(
			Audit.Model.ApplicationEntry? obj1,
			Audit.Model.ApplicationEntry? obj2)
			=> EqualsTo(
				obj1,
				obj2,
				ComparisonOptions,
				Conditions,
				Cache);
	
		public int GetHashCode([DisallowNull] Audit.Model.ApplicationEntry? obj)
			=> GetHashCode(
				obj,
				ComparisonOptions,
				Conditions,
				cache: null);
	}
}
