namespace Legion.ADF.Messaging.DomainEvents.Model;

public partial class DomainEventProcessingStatus : DomainEvents.DomainEventsBaseEntity, Legion.Model.IEntity, IComparable
{
	/// <summary>
	/// 00000001-0000-0000-0000-000000000000
	/// </summary>
	public static DomainEventProcessingStatus Created_NewObject => new(new Guid("00000001-0000-0000-0000-000000000000"), "Created", "Created");

	/// <summary>
	/// 00000002-0000-0000-0000-000000000000
	/// </summary>
	public static DomainEventProcessingStatus Processing_NewObject => new(new Guid("00000002-0000-0000-0000-000000000000"), "Processing", "Processing");

	/// <summary>
	/// 00000003-0000-0000-0000-000000000000
	/// </summary>
	public static DomainEventProcessingStatus Processed_NewObject => new(new Guid("00000003-0000-0000-0000-000000000000"), "Processed", "Processed");

	/// <summary>
	/// 00000004-0000-0000-0000-000000000000
	/// </summary>
	public static DomainEventProcessingStatus Failed_NewObject => new(new Guid("00000004-0000-0000-0000-000000000000"), "Failed", "Failed");

	/// <summary>
	/// 00000005-0000-0000-0000-000000000000
	/// </summary>
	public static DomainEventProcessingStatus Suspended_NewObject => new(new Guid("00000005-0000-0000-0000-000000000000"), "Suspended", "Suspended");

	/// <summary>
	/// 00000006-0000-0000-0000-000000000000
	/// </summary>
	public static DomainEventProcessingStatus NoHandler_NewObject => new(new Guid("00000006-0000-0000-0000-000000000000"), "NoHandler", "NoHandler");

	/// <summary>
	/// 00000007-0000-0000-0000-000000000000
	/// </summary>
	public static DomainEventProcessingStatus Blocked_NewObject => new(new Guid("00000007-0000-0000-0000-000000000000"), "Blocked", "Blocked");

	private DomainEventProcessingStatus(Guid idDomainEventProcessingStatus, string code, string name)
		: this()
	{
		IdDomainEventProcessingStatus = idDomainEventProcessingStatus;
		Code = code;
		Name = name;
	}

	public override bool Equals(object? obj)
	{
		if (obj is not DomainEventProcessingStatus otherValue)
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
		var otherEnumeration = other as DomainEventProcessingStatus;
		return Code?.CompareTo(otherEnumeration?.Code) ?? 1;
	}

	public static DomainEventProcessingStatus? FromId(Guid idDomainEventProcessingStatus)
	{
		return AsEnumerable_NewObjects().FirstOrDefault(x => x.IdDomainEventProcessingStatus == idDomainEventProcessingStatus);
	}

	public static DomainEventProcessingStatus? FromCode(string code)
	{
		return AsEnumerable_NewObjects().FirstOrDefault(v => v.Code?.Equals(code, StringComparison.Ordinal) ?? false);
	}

	public static readonly Lazy<IReadOnlyDictionary<Guid, DomainEventProcessingStatus>> DictionaryMap = new(() => new Dictionary<Guid, DomainEventProcessingStatus>
	{
			{ new Guid("00000001-0000-0000-0000-000000000000"), Created_NewObject },
			{ new Guid("00000002-0000-0000-0000-000000000000"), Processing_NewObject },
			{ new Guid("00000003-0000-0000-0000-000000000000"), Processed_NewObject },
			{ new Guid("00000004-0000-0000-0000-000000000000"), Failed_NewObject },
			{ new Guid("00000005-0000-0000-0000-000000000000"), Suspended_NewObject },
			{ new Guid("00000006-0000-0000-0000-000000000000"), NoHandler_NewObject },
			{ new Guid("00000007-0000-0000-0000-000000000000"), Blocked_NewObject }
	});

	public static IEnumerable<DomainEventProcessingStatus> AsEnumerable_NewObjects()
	{
		yield return Created_NewObject;
		yield return Processing_NewObject;
		yield return Processed_NewObject;
		yield return Failed_NewObject;
		yield return Suspended_NewObject;
		yield return NoHandler_NewObject;
		yield return Blocked_NewObject;
	}
}
