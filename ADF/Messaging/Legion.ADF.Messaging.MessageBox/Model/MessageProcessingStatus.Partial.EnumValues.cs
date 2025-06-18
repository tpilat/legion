namespace Legion.ADF.Messaging.MessageBox.Model;

public partial class MessageProcessingStatus : MessageBox.MessageBoxBaseEntity, Legion.Model.IEntity, IComparable
{
	/// <summary>
	/// 00000001-0000-0000-0000-000000000000
	/// </summary>
	public static MessageProcessingStatus Created_NewObject => new(new Guid("00000001-0000-0000-0000-000000000000"), "Created", "Created");

	/// <summary>
	/// 00000002-0000-0000-0000-000000000000
	/// </summary>
	public static MessageProcessingStatus Processing_NewObject => new(new Guid("00000002-0000-0000-0000-000000000000"), "Processing", "Processing");

	/// <summary>
	/// 00000003-0000-0000-0000-000000000000
	/// </summary>
	public static MessageProcessingStatus Processed_NewObject => new(new Guid("00000003-0000-0000-0000-000000000000"), "Processed", "Processed");

	/// <summary>
	/// 00000004-0000-0000-0000-000000000000
	/// </summary>
	public static MessageProcessingStatus Failed_NewObject => new(new Guid("00000004-0000-0000-0000-000000000000"), "Failed", "Failed");

	/// <summary>
	/// 00000005-0000-0000-0000-000000000000
	/// </summary>
	public static MessageProcessingStatus Suspended_NewObject => new(new Guid("00000005-0000-0000-0000-000000000000"), "Suspended", "Suspended");

	/// <summary>
	/// 00000006-0000-0000-0000-000000000000
	/// </summary>
	public static MessageProcessingStatus NoHandler_NewObject => new(new Guid("00000006-0000-0000-0000-000000000000"), "NoHandler", "NoHandler");

	/// <summary>
	/// 00000007-0000-0000-0000-000000000000
	/// </summary>
	public static MessageProcessingStatus Blocked_NewObject => new(new Guid("00000007-0000-0000-0000-000000000000"), "Blocked", "Blocked");

	/// <summary>
	/// 00000008-0000-0000-0000-000000000000
	/// </summary>
	public static MessageProcessingStatus UnknownType_NewObject => new(new Guid("00000008-0000-0000-0000-000000000000"), "UnknownType", "UnknownType");

	private MessageProcessingStatus(Guid idMessageProcessingStatus, string code, string name)
		: this()
	{
		IdMessageProcessingStatus = idMessageProcessingStatus;
		Code = code;
		Name = name;
	}

	public override bool Equals(object? obj)
	{
		if (obj is not MessageProcessingStatus otherValue)
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
		var otherEnumeration = other as MessageProcessingStatus;
		return Code?.CompareTo(otherEnumeration?.Code) ?? 1;
	}

	public static MessageProcessingStatus? FromId(Guid idMessageProcessingStatus)
	{
		return AsEnumerable_NewObjects().FirstOrDefault(x => x.IdMessageProcessingStatus == idMessageProcessingStatus);
	}

	public static MessageProcessingStatus? FromCode(string code)
	{
		return AsEnumerable_NewObjects().FirstOrDefault(v => v.Code?.Equals(code, StringComparison.Ordinal) ?? false);
	}

	public static readonly Lazy<IReadOnlyDictionary<Guid, MessageProcessingStatus>> DictionaryMap = new(() => new Dictionary<Guid, MessageProcessingStatus>
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

	public static IEnumerable<MessageProcessingStatus> AsEnumerable_NewObjects()
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
