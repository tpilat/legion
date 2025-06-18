namespace Legion.ADF.ESB.MBox.Model;

public partial class MessageStatus : MBox.MBoxBaseEntity, Legion.Model.IEntity, IComparable
{
	/// <summary>
	/// 00000001-0000-0000-0000-000000000000
	/// </summary>
	public static MessageStatus Published_NewObject => new(new Guid("00000001-0000-0000-0000-000000000000"), "Published", "Published");

	/// <summary>
	/// 00000002-0000-0000-0000-000000000000
	/// </summary>
	public static MessageStatus Delivered_NewObject => new(new Guid("00000002-0000-0000-0000-000000000000"), "Delivered", "Delivered");

	/// <summary>
	/// 00000003-0000-0000-0000-000000000000
	/// </summary>
	public static MessageStatus CannotDeliver_NewObject => new(new Guid("00000003-0000-0000-0000-000000000000"), "CannotDeliver", "CannotDeliver");

	/// <summary>
	/// 00000004-0000-0000-0000-000000000000
	/// </summary>
	public static MessageStatus Dropped_NewObject => new(new Guid("00000004-0000-0000-0000-000000000000"), "Dropped", "Dropped");

	private MessageStatus(Guid idMessageStatus, string code, string name)
		: this()
	{
		IdMessageStatus = idMessageStatus;
		Code = code;
		Name = name;
	}

	public override bool Equals(object? obj)
	{
		if (obj is not MessageStatus otherValue)
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
		var otherEnumeration = other as MessageStatus;
		return Code?.CompareTo(otherEnumeration?.Code) ?? 1;
	}

	public static MessageStatus? FromId(Guid idMessageStatus)
	{
		return AsEnumerable_NewObjects().FirstOrDefault(x => x.IdMessageStatus == idMessageStatus);
	}

	public static MessageStatus? FromCode(string code)
	{
		return AsEnumerable_NewObjects().FirstOrDefault(v => v.Code?.Equals(code, StringComparison.Ordinal) ?? false);
	}

	public static readonly Lazy<IReadOnlyDictionary<Guid, MessageStatus>> DictionaryMap = new(() => new Dictionary<Guid, MessageStatus>
	{
			{ new Guid("00000001-0000-0000-0000-000000000000"), Published_NewObject },
			{ new Guid("00000002-0000-0000-0000-000000000000"), Delivered_NewObject },
			{ new Guid("00000003-0000-0000-0000-000000000000"), CannotDeliver_NewObject },
			{ new Guid("00000004-0000-0000-0000-000000000000"), Dropped_NewObject }
	});

	public static IEnumerable<MessageStatus> AsEnumerable_NewObjects()
	{
		yield return Published_NewObject;
		yield return Delivered_NewObject;
		yield return CannotDeliver_NewObject;
		yield return Dropped_NewObject;
	}
}
