namespace Legion.ADF.ServiceBus.Model;

public partial class JobStatus : ServiceBus.ServiceBusBaseEntity, Legion.Model.IEntity, IComparable
{
	/// <summary>
	/// 00000001-0000-0000-0000-000000000000
	/// </summary>
	public static JobStatus Disabled_NewObject => new(new Guid("00000001-0000-0000-0000-000000000000"), "Disabled", "Disabled");

	/// <summary>
	/// 00000002-0000-0000-0000-000000000000
	/// </summary>
	public static JobStatus Started_NewObject => new(new Guid("00000002-0000-0000-0000-000000000000"), "Started", "Started");

	/// <summary>
	/// 00000003-0000-0000-0000-000000000000
	/// </summary>
	public static JobStatus Idle_NewObject => new(new Guid("00000003-0000-0000-0000-000000000000"), "Idle", "Idle");

	/// <summary>
	/// 00000004-0000-0000-0000-000000000000
	/// </summary>
	public static JobStatus Running_NewObject => new(new Guid("00000004-0000-0000-0000-000000000000"), "Running", "Running");

	/// <summary>
	/// 00000005-0000-0000-0000-000000000000
	/// </summary>
	public static JobStatus Error_NewObject => new(new Guid("00000005-0000-0000-0000-000000000000"), "Error", "Error");

	/// <summary>
	/// 00000006-0000-0000-0000-000000000000
	/// </summary>
	public static JobStatus Suspended_NewObject => new(new Guid("00000006-0000-0000-0000-000000000000"), "Suspended", "Suspended");

	/// <summary>
	/// 00000007-0000-0000-0000-000000000000
	/// </summary>
	public static JobStatus Stopped_NewObject => new(new Guid("00000007-0000-0000-0000-000000000000"), "Stopped", "Stopped");

	private JobStatus(Guid idJobStatus, string code, string name)
		: this()
	{
		IdJobStatus = idJobStatus;
		Code = code;
		Name = name;
	}

	public override bool Equals(object? obj)
	{
		if (obj is not JobStatus otherValue)
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
		var otherEnumeration = other as JobStatus;
		return Code?.CompareTo(otherEnumeration?.Code) ?? 1;
	}

	public static JobStatus? FromId(Guid idJobStatus)
	{
		return AsEnumerable_NewObjects().FirstOrDefault(x => x.IdJobStatus == idJobStatus);
	}

	public static JobStatus? FromCode(string code)
	{
		return AsEnumerable_NewObjects().FirstOrDefault(v => v.Code?.Equals(code, StringComparison.Ordinal) ?? false);
	}

	public static readonly Lazy<IReadOnlyDictionary<Guid, JobStatus>> DictionaryMap = new(() => new Dictionary<Guid, JobStatus>
	{
			{ new Guid("00000001-0000-0000-0000-000000000000"), Disabled_NewObject },
			{ new Guid("00000002-0000-0000-0000-000000000000"), Started_NewObject },
			{ new Guid("00000003-0000-0000-0000-000000000000"), Idle_NewObject },
			{ new Guid("00000004-0000-0000-0000-000000000000"), Running_NewObject },
			{ new Guid("00000005-0000-0000-0000-000000000000"), Error_NewObject },
			{ new Guid("00000006-0000-0000-0000-000000000000"), Suspended_NewObject },
			{ new Guid("00000007-0000-0000-0000-000000000000"), Stopped_NewObject }
	});

	public static IEnumerable<JobStatus> AsEnumerable_NewObjects()
	{
		yield return Disabled_NewObject;
		yield return Started_NewObject;
		yield return Idle_NewObject;
		yield return Running_NewObject;
		yield return Error_NewObject;
		yield return Suspended_NewObject;
		yield return Stopped_NewObject;
	}
}
