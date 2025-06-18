using Legion;
using Legion.Model.Comparers;
using System.Diagnostics.CodeAnalysis;

namespace Legion.ADF.Messaging.Outbox.Model;

public sealed partial class OutboxMessageContent : Outbox.OutboxBaseEntity, Legion.Model.IEntity
{
	public bool EqualsTo(
		Outbox.Model.OutboxMessageContent? obj,
		ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
		Action<ComparisonConditions<Outbox.Model.OutboxMessageContent>>? conditions = null)
		=> OutboxMessageContentEqualityComparer.EqualsTo(
			this,
			obj,
			comparisonOptions,
			conditions,
			cache: null);

	public sealed partial class OutboxMessageContentEqualityComparer : IEqualityComparer<OutboxMessageContent>
	{
		public static bool EqualsTo(
			Outbox.Model.OutboxMessageContent? obj1,
			Outbox.Model.OutboxMessageContent? obj2,
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<Outbox.Model.OutboxMessageContent>>? conditions = null,
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
			
			ComparisonConditions<Outbox.Model.OutboxMessageContent>? conds = null;
		
			if ((ComparisonOptions.CompareProperties & comparisonOptions) == ComparisonOptions.CompareProperties)
			{
				if (conditions != null)
				{
					conds = new ComparisonConditions<Outbox.Model.OutboxMessageContent>();
					conditions.Invoke(conds);

					if (conds.CanCompare(obj1, nameof(obj1.IdOutboxMessageContent)) && obj1.IdOutboxMessageContent != obj2.IdOutboxMessageContent)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.MimeType)) && !string.Equals(obj1.MimeType, obj2.MimeType))
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.ContentEncoding)) && !string.Equals(obj1.ContentEncoding, obj2.ContentEncoding))
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.ByteArrayContent)) && (obj1.ByteArrayContent != null || obj2.ByteArrayContent != null) && (obj1.ByteArrayContent == null || obj2.ByteArrayContent == null || !obj1.ByteArrayContent.SequenceEqual(obj2.ByteArrayContent)))
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.JsonContent)) && !string.Equals(obj1.JsonContent, obj2.JsonContent))
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.StringContent)) && !string.Equals(obj1.StringContent, obj2.StringContent))
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.DbOid)) && obj1.DbOid != obj2.DbOid)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.Name)) && !string.Equals(obj1.Name, obj2.Name))
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.RelativePath)) && !string.Equals(obj1.RelativePath, obj2.RelativePath))
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.Metadata)) && !string.Equals(obj1.Metadata, obj2.Metadata))
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.IsCompressed)) && obj1.IsCompressed != obj2.IsCompressed)
						return false;
					if (conds.CanCompare(obj1, nameof(obj1.EncryptionKey)) && !string.Equals(obj1.EncryptionKey, obj2.EncryptionKey))
						return false;
				}
				else
				{
					if (obj1.IdOutboxMessageContent != obj2.IdOutboxMessageContent)
						return false;
					if (!string.Equals(obj1.MimeType, obj2.MimeType))
						return false;
					if (!string.Equals(obj1.ContentEncoding, obj2.ContentEncoding))
						return false;
					if ((obj1.ByteArrayContent != null || obj2.ByteArrayContent != null) && (obj1.ByteArrayContent == null || obj2.ByteArrayContent == null || !obj1.ByteArrayContent.SequenceEqual(obj2.ByteArrayContent)))
						return false;
					if (!string.Equals(obj1.JsonContent, obj2.JsonContent))
						return false;
					if (!string.Equals(obj1.StringContent, obj2.StringContent))
						return false;
					if (obj1.DbOid != obj2.DbOid)
						return false;
					if (!string.Equals(obj1.Name, obj2.Name))
						return false;
					if (!string.Equals(obj1.RelativePath, obj2.RelativePath))
						return false;
					if (!string.Equals(obj1.Metadata, obj2.Metadata))
						return false;
					if (obj1.IsCompressed != obj2.IsCompressed)
						return false;
					if (!string.Equals(obj1.EncryptionKey, obj2.EncryptionKey))
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
				if (!OutboxMessage.OutboxMessageEqualityComparer.EqualsTo(obj1.OutboxMessage, obj2.OutboxMessage, comparisonOptions, conds?.GetConditions(x => x.OutboxMessage), cache))
					return false;
				if (!OutboxMessageArchive.OutboxMessageArchiveEqualityComparer.EqualsTo(obj1.OutboxMessageArchive, obj2.OutboxMessageArchive, comparisonOptions, conds?.GetConditions(x => x.OutboxMessageArchive), cache))
					return false;
		}

			return true;
		}

		public static int GetHashCode(
			Outbox.Model.OutboxMessageContent? obj,
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<Outbox.Model.OutboxMessageContent>>? conditions = null,
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
		public Action<ComparisonConditions<Outbox.Model.OutboxMessageContent>>? Conditions { get; }
		public Dictionary<object, HashSet<object>>? Cache { get; }
	
		public OutboxMessageContentEqualityComparer(
			ComparisonOptions comparisonOptions = ComparisonOptions.CompareAll,
			Action<ComparisonConditions<Outbox.Model.OutboxMessageContent>>? conditions = null,
			Dictionary<object, HashSet<object>>? cache = null)
		{
			Cache = cache ?? [];
			ComparisonOptions = comparisonOptions;
			Conditions = conditions;
		}
	
		public bool Equals(
			Outbox.Model.OutboxMessageContent? obj1,
			Outbox.Model.OutboxMessageContent? obj2)
			=> EqualsTo(
				obj1,
				obj2,
				ComparisonOptions,
				Conditions,
				Cache);
	
		public int GetHashCode([DisallowNull] Outbox.Model.OutboxMessageContent? obj)
			=> GetHashCode(
				obj,
				ComparisonOptions,
				Conditions,
				cache: null);
	}
}
