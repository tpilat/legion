using Legion;
using Legion.Model.Comparers;
using System.Diagnostics.CodeAnalysis;

namespace Legion.ADF.Logs.Model;

public sealed partial class RemoteResponsePayload : Logs.LogsBaseEntity, Legion.Model.IEntity
{
	public bool EqualsTo(
		Logs.Model.RemoteResponsePayload? obj,
		ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
		Action<ComparisonConditions<Logs.Model.RemoteResponsePayload>>? conditions = null)
		=> RemoteResponsePayloadEqualityComparer.EqualsTo(
			this,
			obj,
			comparisonOptions,
			conditions,
			cache: null);

	public sealed partial class RemoteResponsePayloadEqualityComparer : IEqualityComparer<RemoteResponsePayload>
	{
		public static bool EqualsTo(
			Logs.Model.RemoteResponsePayload? obj1,
			Logs.Model.RemoteResponsePayload? obj2,
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<Logs.Model.RemoteResponsePayload>>? conditions = null,
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
			
			ComparisonConditions<Logs.Model.RemoteResponsePayload>? conds = null;
		
			if ((ComparisonOptions.CompareProperties & comparisonOptions) == ComparisonOptions.CompareProperties)
			{
				if (conditions != null)
				{
					conds = new ComparisonConditions<Logs.Model.RemoteResponsePayload>();
					conditions.Invoke(conds);

					if (conds.CanCompare(obj1, nameof(obj1.IdRemoteResponsePayload)) && obj1.IdRemoteResponsePayload != obj2.IdRemoteResponsePayload)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.IdRemoteResponse)) && obj1.IdRemoteResponse != obj2.IdRemoteResponse)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.CreatedUtc)) && obj1.CreatedUtc != obj2.CreatedUtc)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.ResponseContentType)) && !string.Equals(obj1.ResponseContentType, obj2.ResponseContentType))
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.ByteArrayContent)) && (obj1.ByteArrayContent != null || obj2.ByteArrayContent != null) && (obj1.ByteArrayContent == null || obj2.ByteArrayContent == null || !obj1.ByteArrayContent.SequenceEqual(obj2.ByteArrayContent)))
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.JsonContent)) && !string.Equals(obj1.JsonContent, obj2.JsonContent))
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.StringContent)) && !string.Equals(obj1.StringContent, obj2.StringContent))
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.ContentHeaders)) && !string.Equals(obj1.ContentHeaders, obj2.ContentHeaders))
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.DbOid)) && obj1.DbOid != obj2.DbOid)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.FileName)) && !string.Equals(obj1.FileName, obj2.FileName))
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.RelativePath)) && !string.Equals(obj1.RelativePath, obj2.RelativePath))
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.Metadata)) && !string.Equals(obj1.Metadata, obj2.Metadata))
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.IsCompressed)) && obj1.IsCompressed != obj2.IsCompressed)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.EncryptionKey)) && !string.Equals(obj1.EncryptionKey, obj2.EncryptionKey))
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.ContentEncoding)) && !string.Equals(obj1.ContentEncoding, obj2.ContentEncoding))
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.MediaType)) && !string.Equals(obj1.MediaType, obj2.MediaType))
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.MultipartFormDataContentName)) && !string.Equals(obj1.MultipartFormDataContentName, obj2.MultipartFormDataContentName))
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.MultipartFormDataFileName)) && !string.Equals(obj1.MultipartFormDataFileName, obj2.MultipartFormDataFileName))
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.JsonInputCSharpType)) && !string.Equals(obj1.JsonInputCSharpType, obj2.JsonInputCSharpType))
						return false;
				}
				else
				{
					if (obj1.IdRemoteResponsePayload != obj2.IdRemoteResponsePayload)
						return false;
					if (obj1.IdRemoteResponse != obj2.IdRemoteResponse)
						return false;
					if (obj1.CreatedUtc != obj2.CreatedUtc)
						return false;
					if (!string.Equals(obj1.ResponseContentType, obj2.ResponseContentType))
						return false;
					if ((obj1.ByteArrayContent != null || obj2.ByteArrayContent != null) && (obj1.ByteArrayContent == null || obj2.ByteArrayContent == null || !obj1.ByteArrayContent.SequenceEqual(obj2.ByteArrayContent)))
						return false;
					if (!string.Equals(obj1.JsonContent, obj2.JsonContent))
						return false;
					if (!string.Equals(obj1.StringContent, obj2.StringContent))
						return false;
					if (!string.Equals(obj1.ContentHeaders, obj2.ContentHeaders))
						return false;
					if (obj1.DbOid != obj2.DbOid)
						return false;
					if (!string.Equals(obj1.FileName, obj2.FileName))
						return false;
					if (!string.Equals(obj1.RelativePath, obj2.RelativePath))
						return false;
					if (!string.Equals(obj1.Metadata, obj2.Metadata))
						return false;
					if (obj1.IsCompressed != obj2.IsCompressed)
						return false;
					if (!string.Equals(obj1.EncryptionKey, obj2.EncryptionKey))
						return false;
					if (!string.Equals(obj1.ContentEncoding, obj2.ContentEncoding))
						return false;
					if (!string.Equals(obj1.MediaType, obj2.MediaType))
						return false;
					if (!string.Equals(obj1.MultipartFormDataContentName, obj2.MultipartFormDataContentName))
						return false;
					if (!string.Equals(obj1.MultipartFormDataFileName, obj2.MultipartFormDataFileName))
						return false;
					if (!string.Equals(obj1.JsonInputCSharpType, obj2.JsonInputCSharpType))
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
				if (!RemoteResponse.RemoteResponseEqualityComparer.EqualsTo(obj1.RemoteResponse, obj2.RemoteResponse, comparisonOptions, conds?.GetConditions(x => x.RemoteResponse), cache))
					return false;
		}

			return true;
		}

		public static int GetHashCode(
			Logs.Model.RemoteResponsePayload? obj,
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<Logs.Model.RemoteResponsePayload>>? conditions = null,
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
		public Action<ComparisonConditions<Logs.Model.RemoteResponsePayload>>? Conditions { get; }
		public Dictionary<object, HashSet<object>>? Cache { get; }
	
		public RemoteResponsePayloadEqualityComparer(
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<Logs.Model.RemoteResponsePayload>>? conditions = null,
			Dictionary<object, HashSet<object>>? cache = null)
		{
			Cache = cache ?? [];
			ComparisonOptions = comparisonOptions;
			Conditions = conditions;
		}
	
		public bool Equals(
			Logs.Model.RemoteResponsePayload? obj1,
			Logs.Model.RemoteResponsePayload? obj2)
			=> EqualsTo(
				obj1,
				obj2,
				ComparisonOptions,
				Conditions,
				Cache);
	
		public int GetHashCode([DisallowNull] Logs.Model.RemoteResponsePayload? obj)
			=> GetHashCode(
				obj,
				ComparisonOptions,
				Conditions,
				cache: null);
	}
}
