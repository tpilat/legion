using Legion;
using Legion.Model.Comparers;
using System.Diagnostics.CodeAnalysis;

namespace Legion.ADF.Logs.Model;

public sealed partial class RemoteRequest : Logs.LogsBaseEntity, Legion.Model.IEntity
{
	public bool EqualsTo(
		Logs.Model.RemoteRequest? obj,
		ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
		Action<ComparisonConditions<Logs.Model.RemoteRequest>>? conditions = null)
		=> RemoteRequestEqualityComparer.EqualsTo(
			this,
			obj,
			comparisonOptions,
			conditions,
			cache: null);

	public sealed partial class RemoteRequestEqualityComparer : IEqualityComparer<RemoteRequest>
	{
		public static bool EqualsTo(
			Logs.Model.RemoteRequest? obj1,
			Logs.Model.RemoteRequest? obj2,
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<Logs.Model.RemoteRequest>>? conditions = null,
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
			
			ComparisonConditions<Logs.Model.RemoteRequest>? conds = null;
		
			if ((ComparisonOptions.CompareProperties & comparisonOptions) == ComparisonOptions.CompareProperties)
			{
				if (conditions != null)
				{
					conds = new ComparisonConditions<Logs.Model.RemoteRequest>();
					conditions.Invoke(conds);

					if (conds.CanCompare(obj1, nameof(obj1.IdRemoteRequest)) && obj1.IdRemoteRequest != obj2.IdRemoteRequest)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.IdRemoteSystem)) && obj1.IdRemoteSystem != obj2.IdRemoteSystem)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.CreatedUtc)) && obj1.CreatedUtc != obj2.CreatedUtc)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.CorrelationId)) && obj1.CorrelationId != obj2.CorrelationId)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.ExternalCorrelationId)) && !string.Equals(obj1.ExternalCorrelationId, obj2.ExternalCorrelationId))
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.SourceClientIdentifier)) && !string.Equals(obj1.SourceClientIdentifier, obj2.SourceClientIdentifier))
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.Url)) && !string.Equals(obj1.Url, obj2.Url))
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.Method)) && !string.Equals(obj1.Method, obj2.Method))
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.Headers)) && !string.Equals(obj1.Headers, obj2.Headers))
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.ContentType)) && !string.Equals(obj1.ContentType, obj2.ContentType))
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.Metadata)) && !string.Equals(obj1.Metadata, obj2.Metadata))
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.CustomCorrelationId)) && !string.Equals(obj1.CustomCorrelationId, obj2.CustomCorrelationId))
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.RuntimeUniqueKey)) && obj1.RuntimeUniqueKey != obj2.RuntimeUniqueKey)
						return false;
				}
				else
				{
					if (obj1.IdRemoteRequest != obj2.IdRemoteRequest)
						return false;
					if (obj1.IdRemoteSystem != obj2.IdRemoteSystem)
						return false;
					if (obj1.CreatedUtc != obj2.CreatedUtc)
						return false;
					if (obj1.CorrelationId != obj2.CorrelationId)
						return false;
					if (!string.Equals(obj1.ExternalCorrelationId, obj2.ExternalCorrelationId))
						return false;
					if (!string.Equals(obj1.SourceClientIdentifier, obj2.SourceClientIdentifier))
						return false;
					if (!string.Equals(obj1.Url, obj2.Url))
						return false;
					if (!string.Equals(obj1.Method, obj2.Method))
						return false;
					if (!string.Equals(obj1.Headers, obj2.Headers))
						return false;
					if (!string.Equals(obj1.ContentType, obj2.ContentType))
						return false;
					if (!string.Equals(obj1.Metadata, obj2.Metadata))
						return false;
					if (!string.Equals(obj1.CustomCorrelationId, obj2.CustomCorrelationId))
						return false;
					if (obj1.RuntimeUniqueKey != obj2.RuntimeUniqueKey)
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
				if (!RemoteSystem.RemoteSystemEqualityComparer.EqualsTo(obj1.RemoteSystem, obj2.RemoteSystem, comparisonOptions, conds?.GetConditions(x => x.RemoteSystem), cache))
					return false;
				if (!ComparisonHelper.SequenceEqual(obj1.RemoteRequestPayloads, obj2.RemoteRequestPayloads, new RemoteRequestPayload.RemoteRequestPayloadEqualityComparer(comparisonOptions, conds?.GetConditions(x => x.RemoteRequestPayloads), cache)))
					return false;
				if (!ComparisonHelper.SequenceEqual(obj1.RemoteResponses, obj2.RemoteResponses, new RemoteResponse.RemoteResponseEqualityComparer(comparisonOptions, conds?.GetConditions(x => x.RemoteResponses), cache)))
					return false;
		}

			return true;
		}

		public static int GetHashCode(
			Logs.Model.RemoteRequest? obj,
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<Logs.Model.RemoteRequest>>? conditions = null,
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
		public Action<ComparisonConditions<Logs.Model.RemoteRequest>>? Conditions { get; }
		public Dictionary<object, HashSet<object>>? Cache { get; }
	
		public RemoteRequestEqualityComparer(
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<Logs.Model.RemoteRequest>>? conditions = null,
			Dictionary<object, HashSet<object>>? cache = null)
		{
			Cache = cache ?? [];
			ComparisonOptions = comparisonOptions;
			Conditions = conditions;
		}
	
		public bool Equals(
			Logs.Model.RemoteRequest? obj1,
			Logs.Model.RemoteRequest? obj2)
			=> EqualsTo(
				obj1,
				obj2,
				ComparisonOptions,
				Conditions,
				Cache);
	
		public int GetHashCode([DisallowNull] Logs.Model.RemoteRequest? obj)
			=> GetHashCode(
				obj,
				ComparisonOptions,
				Conditions,
				cache: null);
	}
}
