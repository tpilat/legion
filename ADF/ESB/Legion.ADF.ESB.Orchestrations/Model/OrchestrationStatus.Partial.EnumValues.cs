namespace Legion.ADF.ESB.Orchestrations.Model;

public partial class OrchestrationStatus : Orchestrations.OrchestrationsBaseEntity, Legion.Model.IEntity, IComparable
{
	/// <summary>
	/// 00000001-0000-0000-0000-000000000000
	/// </summary>
	public static OrchestrationStatus Disabled_NewObject => new(new Guid("00000001-0000-0000-0000-000000000000"), "Disabled", "Disabled");

	/// <summary>
	/// 00000002-0000-0000-0000-000000000000
	/// </summary>
	public static OrchestrationStatus Offline_NewObject => new(new Guid("00000002-0000-0000-0000-000000000000"), "Offline", "Offline");

	/// <summary>
	/// 00000003-0000-0000-0000-000000000000
	/// </summary>
	public static OrchestrationStatus Running_NewObject => new(new Guid("00000003-0000-0000-0000-000000000000"), "Running", "Running");

	/// <summary>
	/// 00000004-0000-0000-0000-000000000000
	/// </summary>
	public static OrchestrationStatus Error_NewObject => new(new Guid("00000004-0000-0000-0000-000000000000"), "Error", "Error");

	/// <summary>
	/// 00000005-0000-0000-0000-000000000000
	/// </summary>
	public static OrchestrationStatus Succeeded_NewObject => new(new Guid("00000005-0000-0000-0000-000000000000"), "Succeeded", "Succeeded");

	/// <summary>
	/// 00000006-0000-0000-0000-000000000000
	/// </summary>
	public static OrchestrationStatus Suspended_NewObject => new(new Guid("00000006-0000-0000-0000-000000000000"), "Suspended", "Suspended");

	private OrchestrationStatus(Guid idOrchestrationStatus, string code, string name)
		: this()
	{
		IdOrchestrationStatus = idOrchestrationStatus;
		Code = code;
		Name = name;
	}

	public override bool Equals(object? obj)
	{
		if (obj is not OrchestrationStatus otherValue)
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
		var otherEnumeration = other as OrchestrationStatus;
		return Code?.CompareTo(otherEnumeration?.Code) ?? 1;
	}

	public static OrchestrationStatus? FromId(Guid idOrchestrationStatus)
	{
		return AsEnumerable_NewObjects().FirstOrDefault(x => x.IdOrchestrationStatus == idOrchestrationStatus);
	}

	public static OrchestrationStatus? FromCode(string code)
	{
		return AsEnumerable_NewObjects().FirstOrDefault(v => v.Code?.Equals(code, StringComparison.Ordinal) ?? false);
	}

	public static readonly Lazy<IReadOnlyDictionary<Guid, OrchestrationStatus>> DictionaryMap = new(() => new Dictionary<Guid, OrchestrationStatus>
	{
			{ new Guid("00000001-0000-0000-0000-000000000000"), Disabled_NewObject },
			{ new Guid("00000002-0000-0000-0000-000000000000"), Offline_NewObject },
			{ new Guid("00000003-0000-0000-0000-000000000000"), Running_NewObject },
			{ new Guid("00000004-0000-0000-0000-000000000000"), Error_NewObject },
			{ new Guid("00000005-0000-0000-0000-000000000000"), Succeeded_NewObject },
			{ new Guid("00000006-0000-0000-0000-000000000000"), Suspended_NewObject }
	});

	public static IEnumerable<OrchestrationStatus> AsEnumerable_NewObjects()
	{
		yield return Disabled_NewObject;
		yield return Offline_NewObject;
		yield return Running_NewObject;
		yield return Error_NewObject;
		yield return Succeeded_NewObject;
		yield return Suspended_NewObject;
	}
}
