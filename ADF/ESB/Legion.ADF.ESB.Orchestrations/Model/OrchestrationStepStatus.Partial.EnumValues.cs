namespace Legion.ADF.ESB.Orchestrations.Model;

public partial class OrchestrationStepStatus : Orchestrations.OrchestrationsBaseEntity, Legion.Model.IEntity, IComparable
{
	/// <summary>
	/// 00000001-0000-0000-0000-000000000000
	/// </summary>
	public static OrchestrationStepStatus Idle_NewObject => new(new Guid("00000001-0000-0000-0000-000000000000"), "Idle", "Idle");

	/// <summary>
	/// 00000002-0000-0000-0000-000000000000
	/// </summary>
	public static OrchestrationStepStatus Running_NewObject => new(new Guid("00000002-0000-0000-0000-000000000000"), "Running", "Running");

	/// <summary>
	/// 00000003-0000-0000-0000-000000000000
	/// </summary>
	public static OrchestrationStepStatus Error_NewObject => new(new Guid("00000003-0000-0000-0000-000000000000"), "Error", "Error");

	/// <summary>
	/// 00000004-0000-0000-0000-000000000000
	/// </summary>
	public static OrchestrationStepStatus Succeeded_NewObject => new(new Guid("00000004-0000-0000-0000-000000000000"), "Succeeded", "Succeeded");

	/// <summary>
	/// 00000005-0000-0000-0000-000000000000
	/// </summary>
	public static OrchestrationStepStatus Suspended_NewObject => new(new Guid("00000005-0000-0000-0000-000000000000"), "Suspended", "Suspended");

	/// <summary>
	/// 00000006-0000-0000-0000-000000000000
	/// </summary>
	public static OrchestrationStepStatus Skipped_NewObject => new(new Guid("00000006-0000-0000-0000-000000000000"), "Skipped", "Skipped");

	private OrchestrationStepStatus(Guid idOrchestrationStepStatus, string code, string name)
		: this()
	{
		IdOrchestrationStepStatus = idOrchestrationStepStatus;
		Code = code;
		Name = name;
	}

	public override bool Equals(object? obj)
	{
		if (obj is not OrchestrationStepStatus otherValue)
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
		var otherEnumeration = other as OrchestrationStepStatus;
		return Code?.CompareTo(otherEnumeration?.Code) ?? 1;
	}

	public static OrchestrationStepStatus? FromId(Guid idOrchestrationStepStatus)
	{
		return AsEnumerable_NewObjects().FirstOrDefault(x => x.IdOrchestrationStepStatus == idOrchestrationStepStatus);
	}

	public static OrchestrationStepStatus? FromCode(string code)
	{
		return AsEnumerable_NewObjects().FirstOrDefault(v => v.Code?.Equals(code, StringComparison.Ordinal) ?? false);
	}

	public static readonly Lazy<IReadOnlyDictionary<Guid, OrchestrationStepStatus>> DictionaryMap = new(() => new Dictionary<Guid, OrchestrationStepStatus>
	{
			{ new Guid("00000001-0000-0000-0000-000000000000"), Idle_NewObject },
			{ new Guid("00000002-0000-0000-0000-000000000000"), Running_NewObject },
			{ new Guid("00000003-0000-0000-0000-000000000000"), Error_NewObject },
			{ new Guid("00000004-0000-0000-0000-000000000000"), Succeeded_NewObject },
			{ new Guid("00000005-0000-0000-0000-000000000000"), Suspended_NewObject },
			{ new Guid("00000006-0000-0000-0000-000000000000"), Skipped_NewObject }
	});

	public static IEnumerable<OrchestrationStepStatus> AsEnumerable_NewObjects()
	{
		yield return Idle_NewObject;
		yield return Running_NewObject;
		yield return Error_NewObject;
		yield return Succeeded_NewObject;
		yield return Suspended_NewObject;
		yield return Skipped_NewObject;
	}
}
