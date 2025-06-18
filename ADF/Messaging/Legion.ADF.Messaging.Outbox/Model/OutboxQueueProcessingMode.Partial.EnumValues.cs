namespace Legion.ADF.Messaging.Outbox.Model;

public partial class OutboxQueueProcessingMode : Outbox.OutboxBaseEntity, Legion.Model.IEntity, IComparable
{
	/// <summary>
	/// 00000001-0000-0000-0000-000000000000
	/// </summary>
	public static OutboxQueueProcessingMode NoAction_NewObject => new(new Guid("00000001-0000-0000-0000-000000000000"), "NoAction", "NoAction");

	/// <summary>
	/// 00000002-0000-0000-0000-000000000000
	/// </summary>
	public static OutboxQueueProcessingMode Archivate_NewObject => new(new Guid("00000002-0000-0000-0000-000000000000"), "Archivate", "Archivate");

	/// <summary>
	/// 00000003-0000-0000-0000-000000000000
	/// </summary>
	public static OutboxQueueProcessingMode Delete_NewObject => new(new Guid("00000003-0000-0000-0000-000000000000"), "Delete", "Delete");

	private OutboxQueueProcessingMode(Guid idOutboxQueueProcessingMode, string code, string name)
		: this()
	{
		IdOutboxQueueProcessingMode = idOutboxQueueProcessingMode;
		Code = code;
		Name = name;
	}

	public override bool Equals(object? obj)
	{
		if (obj is not OutboxQueueProcessingMode otherValue)
			return false;

		var typeMatches = GetType().Equals(obj.GetType());
		var valueMatches = Code?.Equals(otherValue.Code) ?? (otherValue.Code == null);

		return typeMatches && valueMatches;
	}

	public override int GetHashCode()
	{
		return Code?.GetHashCode() ?? 0;
	}

	public int CompareTo(object? other)
	{
		var otherEnumeration = other as OutboxQueueProcessingMode;
		return Code?.CompareTo(otherEnumeration?.Code) ?? 1;
	}

	public static OutboxQueueProcessingMode? FromId(Guid idOutboxQueueProcessingMode)
	{
		return AsEnumerable_NewObjects().FirstOrDefault(x => x.IdOutboxQueueProcessingMode == idOutboxQueueProcessingMode);
	}

	public static OutboxQueueProcessingMode? FromCode(string code)
	{
		return AsEnumerable_NewObjects().FirstOrDefault(v => v.Code?.Equals(code, StringComparison.Ordinal) ?? false);
	}

	public static readonly Lazy<IReadOnlyDictionary<Guid, OutboxQueueProcessingMode>> DictionaryMap = new(() => new Dictionary<Guid, OutboxQueueProcessingMode>
	{
			{ new Guid("00000001-0000-0000-0000-000000000000"), NoAction_NewObject },
			{ new Guid("00000002-0000-0000-0000-000000000000"), Archivate_NewObject },
			{ new Guid("00000003-0000-0000-0000-000000000000"), Delete_NewObject }
	});

	public static IEnumerable<OutboxQueueProcessingMode> AsEnumerable_NewObjects()
	{
		yield return NoAction_NewObject;
		yield return Archivate_NewObject;
		yield return Delete_NewObject;
	}
}
