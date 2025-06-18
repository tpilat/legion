using Legion;
using Legion.Model.Comparers;
using System.Diagnostics.CodeAnalysis;

namespace Legion.ADF.Logs.Model;

public sealed partial class LocalRequest : Logs.LogsBaseEntity, Legion.Model.IEntity
{
	public bool EqualsTo(
		Logs.Model.LocalRequest? obj,
		ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
		Action<ComparisonConditions<Logs.Model.LocalRequest>>? conditions = null)
		=> LocalRequestEqualityComparer.EqualsTo(
			this,
			obj,
			comparisonOptions,
			conditions,
			cache: null);

	public sealed partial class LocalRequestEqualityComparer : IEqualityComparer<LocalRequest>
	{
		public static bool EqualsTo(
			Logs.Model.LocalRequest? obj1,
			Logs.Model.LocalRequest? obj2,
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<Logs.Model.LocalRequest>>? conditions = null,
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
			
			ComparisonConditions<Logs.Model.LocalRequest>? conds = null;
		
			if ((ComparisonOptions.CompareProperties & comparisonOptions) == ComparisonOptions.CompareProperties)
			{
				if (conditions != null)
				{
					conds = new ComparisonConditions<Logs.Model.LocalRequest>();
					conditions.Invoke(conds);

					if (conds.CanCompare(obj1, nameof(obj1.IdLocalRequest)) && obj1.IdLocalRequest != obj2.IdLocalRequest)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.IdRemoteSystem)) && obj1.IdRemoteSystem != obj2.IdRemoteSystem)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.RemoteIp)) && !string.Equals(obj1.RemoteIp, obj2.RemoteIp))
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
					if (conds.CanCompare(obj1, nameof(obj1.Path)) && !string.Equals(obj1.Path, obj2.Path))
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.QueryString)) && !string.Equals(obj1.QueryString, obj2.QueryString))
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
					if (obj1.IdLocalRequest != obj2.IdLocalRequest)
						return false;
					if (obj1.IdRemoteSystem != obj2.IdRemoteSystem)
						return false;
					if (!string.Equals(obj1.RemoteIp, obj2.RemoteIp))
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
					if (!string.Equals(obj1.Path, obj2.Path))
						return false;
					if (!string.Equals(obj1.QueryString, obj2.QueryString))
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
				if (!ComparisonHelper.SequenceEqual(obj1.LocalRequestPayloads, obj2.LocalRequestPayloads, new LocalRequestPayload.LocalRequestPayloadEqualityComparer(comparisonOptions, conds?.GetConditions(x => x.LocalRequestPayloads), cache)))
					return false;
				if (!ComparisonHelper.SequenceEqual(obj1.LocalResponses, obj2.LocalResponses, new LocalResponse.LocalResponseEqualityComparer(comparisonOptions, conds?.GetConditions(x => x.LocalResponses), cache)))
					return false;
		}

			return true;
		}

		public static int GetHashCode(
			Logs.Model.LocalRequest? obj,
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<Logs.Model.LocalRequest>>? conditions = null,
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
		public Action<ComparisonConditions<Logs.Model.LocalRequest>>? Conditions { get; }
		public Dictionary<object, HashSet<object>>? Cache { get; }
	
		public LocalRequestEqualityComparer(
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<Logs.Model.LocalRequest>>? conditions = null,
			Dictionary<object, HashSet<object>>? cache = null)
		{
			Cache = cache ?? [];
			ComparisonOptions = comparisonOptions;
			Conditions = conditions;
		}
	
		public bool Equals(
			Logs.Model.LocalRequest? obj1,
			Logs.Model.LocalRequest? obj2)
			=> EqualsTo(
				obj1,
				obj2,
				ComparisonOptions,
				Conditions,
				Cache);
	
		public int GetHashCode([DisallowNull] Logs.Model.LocalRequest? obj)
			=> GetHashCode(
				obj,
				ComparisonOptions,
				Conditions,
				cache: null);
	}
}
