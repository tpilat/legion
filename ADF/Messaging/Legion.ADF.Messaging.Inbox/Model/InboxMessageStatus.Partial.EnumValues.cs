namespace Legion.ADF.Messaging.Inbox.Model;

public partial class InboxMessageStatus : Inbox.InboxBaseEntity, Legion.Model.IEntity, IComparable
{
	/// <summary>
	/// 00000001-0000-0000-0000-000000000000
	/// </summary>
	public static InboxMessageStatus Created_NewObject => new(new Guid("00000001-0000-0000-0000-000000000000"), "Created", "Created");

	/// <summary>
	/// 00000002-0000-0000-0000-000000000000
	/// </summary>
	public static InboxMessageStatus Processing_NewObject => new(new Guid("00000002-0000-0000-0000-000000000000"), "Processing", "Processing");

	/// <summary>
	/// 00000003-0000-0000-0000-000000000000
	/// </summary>
	public static InboxMessageStatus Processed_NewObject => new(new Guid("00000003-0000-0000-0000-000000000000"), "Processed", "Processed");

	/// <summary>
	/// 00000004-0000-0000-0000-000000000000
	/// </summary>
	public static InboxMessageStatus Failed_NewObject => new(new Guid("00000004-0000-0000-0000-000000000000"), "Failed", "Failed");

	/// <summary>
	/// 00000005-0000-0000-0000-000000000000
	/// </summary>
	public static InboxMessageStatus Suspended_NewObject => new(new Guid("00000005-0000-0000-0000-000000000000"), "Suspended", "Suspended");

	/// <summary>
	/// 00000006-0000-0000-0000-000000000000
	/// </summary>
	public static InboxMessageStatus NoHandler_NewObject => new(new Guid("00000006-0000-0000-0000-000000000000"), "NoHandler", "NoHandler");

	/// <summary>
	/// 00000007-0000-0000-0000-000000000000
	/// </summary>
	public static InboxMessageStatus Blocked_NewObject => new(new Guid("00000007-0000-0000-0000-000000000000"), "Blocked", "Blocked");

	/// <summary>
	/// 00000008-0000-0000-0000-000000000000
	/// </summary>
	public static InboxMessageStatus UnknownType_NewObject => new(new Guid("00000008-0000-0000-0000-000000000000"), "UnknownType", "UnknownType");

	private InboxMessageStatus(Guid idInboxMessageStatus, string code, string name)
		: this()
	{
		IdInboxMessageStatus = idInboxMessageStatus;
		Code = code;
		Name = name;
	}

	public override bool Equals(object? obj)
	{
		if (obj is not InboxMessageStatus otherValue)
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
		var otherEnumeration = other as InboxMessageStatus;
		return Code?.CompareTo(otherEnumeration?.Code) ?? 1;
	}

	public static InboxMessageStatus? FromId(Guid idInboxMessageStatus)
	{
		return AsEnumerable_NewObjects().FirstOrDefault(x => x.IdInboxMessageStatus == idInboxMessageStatus);
	}

	public static InboxMessageStatus? FromCode(string code)
	{
		return AsEnumerable_NewObjects().FirstOrDefault(v => v.Code?.Equals(code, StringComparison.Ordinal) ?? false);
	}

	public static readonly Lazy<IReadOnlyDictionary<Guid, InboxMessageStatus>> DictionaryMap = new(() => new Dictionary<Guid, InboxMessageStatus>
	{
			{ new Guid("00000001-0000-0000-0000-000000000000"), Created_NewObject },
			{ new Guid("00000002-0000-0000-0000-000000000000"), Processing_NewObject },
			{ new Guid("00000003-0000-0000-0000-000000000000"), Processed_NewObject },
			{ new Guid("00000004-0000-0000-0000-000000000000"), Failed_NewObject },
			{ new Guid("00000005-0000-0000-0000-000000000000"), Suspended_NewObject },
			{ new Guid("00000006-0000-0000-0000-000000000000"), NoHandler_NewObject },
			{ new Guid("00000007-0000-0000-0000-000000000000"), Blocked_NewObject },
			{ new Guid("00000008-0000-0000-0000-000000000000"), UnknownType_NewObject }
	});

	public static IEnumerable<InboxMessageStatus> AsEnumerable_NewObjects()
	{
		yield return Created_NewObject;
		yield return Processing_NewObject;
		yield return Processed_NewObject;
		yield return Failed_NewObject;
		yield return Suspended_NewObject;
		yield return NoHandler_NewObject;
		yield return Blocked_NewObject;
		yield return UnknownType_NewObject;
	}
}
