using Legion;
using Legion.Model.Comparers;
using System.Diagnostics.CodeAnalysis;

namespace Legion.ADF.Logs.Model;

public sealed partial class RemoteResponse : Logs.LogsBaseEntity, Legion.Model.IEntity
{
	public bool EqualsTo(
		Logs.Model.RemoteResponse? obj,
		ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
		Action<ComparisonConditions<Logs.Model.RemoteResponse>>? conditions = null)
		=> RemoteResponseEqualityComparer.EqualsTo(
			this,
			obj,
			comparisonOptions,
			conditions,
			cache: null);

	public sealed partial class RemoteResponseEqualityComparer : IEqualityComparer<RemoteResponse>
	{
		public static bool EqualsTo(
			Logs.Model.RemoteResponse? obj1,
			Logs.Model.RemoteResponse? obj2,
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<Logs.Model.RemoteResponse>>? conditions = null,
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
			
			ComparisonConditions<Logs.Model.RemoteResponse>? conds = null;
		
			if ((ComparisonOptions.CompareProperties & comparisonOptions) == ComparisonOptions.CompareProperties)
			{
				if (conditions != null)
				{
					conds = new ComparisonConditions<Logs.Model.RemoteResponse>();
					conditions.Invoke(conds);

					if (conds.CanCompare(obj1, nameof(obj1.IdRemoteResponse)) && obj1.IdRemoteResponse != obj2.IdRemoteResponse)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.IdRemoteRequest)) && obj1.IdRemoteRequest != obj2.IdRemoteRequest)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.CreatedUtc)) && obj1.CreatedUtc != obj2.CreatedUtc)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.CorrelationId)) && obj1.CorrelationId != obj2.CorrelationId)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.ExternalCorrelationId)) && !string.Equals(obj1.ExternalCorrelationId, obj2.ExternalCorrelationId))
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.StatusCode)) && !string.Equals(obj1.StatusCode, obj2.StatusCode))
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.Reason)) && !string.Equals(obj1.Reason, obj2.Reason))
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.Headers)) && !string.Equals(obj1.Headers, obj2.Headers))
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.ContentType)) && !string.Equals(obj1.ContentType, obj2.ContentType))
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.Error)) && !string.Equals(obj1.Error, obj2.Error))
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.ElapsedMilliseconds)) && obj1.ElapsedMilliseconds != obj2.ElapsedMilliseconds)
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
					if (obj1.IdRemoteResponse != obj2.IdRemoteResponse)
						return false;
					if (obj1.IdRemoteRequest != obj2.IdRemoteRequest)
						return false;
					if (obj1.CreatedUtc != obj2.CreatedUtc)
						return false;
					if (obj1.CorrelationId != obj2.CorrelationId)
						return false;
					if (!string.Equals(obj1.ExternalCorrelationId, obj2.ExternalCorrelationId))
						return false;
					if (!string.Equals(obj1.StatusCode, obj2.StatusCode))
						return false;
					if (!string.Equals(obj1.Reason, obj2.Reason))
						return false;
					if (!string.Equals(obj1.Headers, obj2.Headers))
						return false;
					if (!string.Equals(obj1.ContentType, obj2.ContentType))
						return false;
					if (!string.Equals(obj1.Error, obj2.Error))
						return false;
					if (obj1.ElapsedMilliseconds != obj2.ElapsedMilliseconds)
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
				if (!RemoteRequest.RemoteRequestEqualityComparer.EqualsTo(obj1.RemoteRequest, obj2.RemoteRequest, comparisonOptions, conds?.GetConditions(x => x.RemoteRequest), cache))
					return false;
				if (!ComparisonHelper.SequenceEqual(obj1.RemoteResponsePayloads, obj2.RemoteResponsePayloads, new RemoteResponsePayload.RemoteResponsePayloadEqualityComparer(comparisonOptions, conds?.GetConditions(x => x.RemoteResponsePayloads), cache)))
					return false;
		}

			return true;
		}

		public static int GetHashCode(
			Logs.Model.RemoteResponse? obj,
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<Logs.Model.RemoteResponse>>? conditions = null,
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
		public Action<ComparisonConditions<Logs.Model.RemoteResponse>>? Conditions { get; }
		public Dictionary<object, HashSet<object>>? Cache { get; }
	
		public RemoteResponseEqualityComparer(
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<Logs.Model.RemoteResponse>>? conditions = null,
			Dictionary<object, HashSet<object>>? cache = null)
		{
			Cache = cache ?? [];
			ComparisonOptions = comparisonOptions;
			Conditions = conditions;
		}
	
		public bool Equals(
			Logs.Model.RemoteResponse? obj1,
			Logs.Model.RemoteResponse? obj2)
			=> EqualsTo(
				obj1,
				obj2,
				ComparisonOptions,
				Conditions,
				Cache);
	
		public int GetHashCode([DisallowNull] Logs.Model.RemoteResponse? obj)
			=> GetHashCode(
				obj,
				ComparisonOptions,
				Conditions,
				cache: null);
	}
}
