namespace Legion.ADF.ESB.MBox.Model;

public partial class MessageProcessingStatus : MBox.MBoxBaseEntity, Legion.Model.IEntity, IComparable
{
	/// <summary>
	/// 00000001-0000-0000-0000-000000000000
	/// </summary>
	public static MessageProcessingStatus Delivered_NewObject => new(new Guid("00000001-0000-0000-0000-000000000000"), "Delivered", "Delivered");

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
	public static MessageProcessingStatus Terminated_NewObject => new(new Guid("00000004-0000-0000-0000-000000000000"), "Terminated", "Terminated");

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
			{ new Guid("00000001-0000-0000-0000-000000000000"), Delivered_NewObject },
			{ new Guid("00000002-0000-0000-0000-000000000000"), Processing_NewObject },
			{ new Guid("00000003-0000-0000-0000-000000000000"), Processed_NewObject },
			{ new Guid("00000004-0000-0000-0000-000000000000"), Terminated_NewObject }
	});

	public static IEnumerable<MessageProcessingStatus> AsEnumerable_NewObjects()
	{
		yield return Delivered_NewObject;
		yield return Processing_NewObject;
		yield return Processed_NewObject;
		yield return Terminated_NewObject;
	}
}
