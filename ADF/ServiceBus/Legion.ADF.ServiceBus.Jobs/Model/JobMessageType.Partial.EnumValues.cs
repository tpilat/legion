namespace Legion.ADF.ServiceBus.Jobs.Model;

public partial class JobMessageType : Jobs.JobsBaseEntity, Legion.Model.IEntity, IComparable
{
	/// <summary>
	/// 00000001-0000-0000-0000-000000000000
	/// </summary>
	public static JobMessageType Published_NewObject => new(new Guid("00000001-0000-0000-0000-000000000000"), "Published", "Published");

	/// <summary>
	/// 00000002-0000-0000-0000-000000000000
	/// </summary>
	public static JobMessageType SubscribedFromQueue_NewObject => new(new Guid("00000002-0000-0000-0000-000000000000"), "SubscribedFromQueue", "SubscribedFromQueue");

	/// <summary>
	/// 00000003-0000-0000-0000-000000000000
	/// </summary>
	public static JobMessageType SubscribedFromTopic_NewObject => new(new Guid("00000003-0000-0000-0000-000000000000"), "SubscribedFromTopic", "SubscribedFromTopic");

	private JobMessageType(Guid idJobMessageType, string code, string name)
		: this()
	{
		IdJobMessageType = idJobMessageType;
		Code = code;
		Name = name;
	}

	public override bool Equals(object? obj)
	{
		if (obj is not JobMessageType otherValue)
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
		var otherEnumeration = other as JobMessageType;
		return Code?.CompareTo(otherEnumeration?.Code) ?? 1;
	}

	public static JobMessageType? FromId(Guid idJobMessageType)
	{
		return AsEnumerable_NewObjects().FirstOrDefault(x => x.IdJobMessageType == idJobMessageType);
	}

	public static JobMessageType? FromCode(string code)
	{
		return AsEnumerable_NewObjects().FirstOrDefault(v => v.Code?.Equals(code, StringComparison.Ordinal) ?? false);
	}

	public static readonly Lazy<IReadOnlyDictionary<Guid, JobMessageType>> DictionaryMap = new(() => new Dictionary<Guid, JobMessageType>
	{
			{ new Guid("00000001-0000-0000-0000-000000000000"), Published_NewObject },
			{ new Guid("00000002-0000-0000-0000-000000000000"), SubscribedFromQueue_NewObject },
			{ new Guid("00000003-0000-0000-0000-000000000000"), SubscribedFromTopic_NewObject }
	});

	public static IEnumerable<JobMessageType> AsEnumerable_NewObjects()
	{
		yield return Published_NewObject;
		yield return SubscribedFromQueue_NewObject;
		yield return SubscribedFromTopic_NewObject;
	}
}
