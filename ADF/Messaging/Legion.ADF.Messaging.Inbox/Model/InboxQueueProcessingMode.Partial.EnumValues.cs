namespace Legion.ADF.Messaging.Inbox.Model;

public partial class InboxQueueProcessingMode : Inbox.InboxBaseEntity, Legion.Model.IEntity, IComparable
{
	/// <summary>
	/// 00000001-0000-0000-0000-000000000000
	/// </summary>
	public static InboxQueueProcessingMode NoAction_NewObject => new(new Guid("00000001-0000-0000-0000-000000000000"), "NoAction", "NoAction");

	/// <summary>
	/// 00000002-0000-0000-0000-000000000000
	/// </summary>
	public static InboxQueueProcessingMode Archivate_NewObject => new(new Guid("00000002-0000-0000-0000-000000000000"), "Archivate", "Archivate");

	/// <summary>
	/// 00000003-0000-0000-0000-000000000000
	/// </summary>
	public static InboxQueueProcessingMode Delete_NewObject => new(new Guid("00000003-0000-0000-0000-000000000000"), "Delete", "Delete");

	private InboxQueueProcessingMode(Guid idInboxQueueProcessingMode, string code, string name)
		: this()
	{
		IdInboxQueueProcessingMode = idInboxQueueProcessingMode;
		Code = code;
		Name = name;
	}

	public override bool Equals(object? obj)
	{
		if (obj is not InboxQueueProcessingMode otherValue)
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
		var otherEnumeration = other as InboxQueueProcessingMode;
		return Code?.CompareTo(otherEnumeration?.Code) ?? 1;
	}

	public static InboxQueueProcessingMode? FromId(Guid idInboxQueueProcessingMode)
	{
		return AsEnumerable_NewObjects().FirstOrDefault(x => x.IdInboxQueueProcessingMode == idInboxQueueProcessingMode);
	}

	public static InboxQueueProcessingMode? FromCode(string code)
	{
		return AsEnumerable_NewObjects().FirstOrDefault(v => v.Code?.Equals(code, StringComparison.Ordinal) ?? false);
	}

	public static readonly Lazy<IReadOnlyDictionary<Guid, InboxQueueProcessingMode>> DictionaryMap = new(() => new Dictionary<Guid, InboxQueueProcessingMode>
	{
			{ new Guid("00000001-0000-0000-0000-000000000000"), NoAction_NewObject },
			{ new Guid("00000002-0000-0000-0000-000000000000"), Archivate_NewObject },
			{ new Guid("00000003-0000-0000-0000-000000000000"), Delete_NewObject }
	});

	public static IEnumerable<InboxQueueProcessingMode> AsEnumerable_NewObjects()
	{
		yield return NoAction_NewObject;
		yield return Archivate_NewObject;
		yield return Delete_NewObject;
	}
}
