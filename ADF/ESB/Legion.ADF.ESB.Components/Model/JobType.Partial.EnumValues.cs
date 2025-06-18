namespace Legion.ADF.ESB.Components.Model;

public partial class JobType : Components.ComponentsBaseEntity, Legion.Model.IEntity, IComparable
{
	/// <summary>
	/// 00000001-0000-0000-0000-000000000000
	/// </summary>
	public static JobType SequentialIntervalTimer_NewObject => new(new Guid("00000001-0000-0000-0000-000000000000"), "SequentialIntervalTimer", "SequentialIntervalTimer");

	/// <summary>
	/// 00000002-0000-0000-0000-000000000000
	/// </summary>
	public static JobType ExactPeriodicTimer_NewObject => new(new Guid("00000002-0000-0000-0000-000000000000"), "ExactPeriodicTimer", "ExactPeriodicTimer");

	/// <summary>
	/// 00000003-0000-0000-0000-000000000000
	/// </summary>
	public static JobType CronTimer_NewObject => new(new Guid("00000003-0000-0000-0000-000000000000"), "CronTimer", "CronTimer");

	private JobType(Guid idJobType, string code, string name)
		: this()
	{
		IdJobType = idJobType;
		Code = code;
		Name = name;
	}

	public override bool Equals(object? obj)
	{
		if (obj is not JobType otherValue)
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
		var otherEnumeration = other as JobType;
		return Code?.CompareTo(otherEnumeration?.Code) ?? 1;
	}

	public static JobType? FromId(Guid idJobType)
	{
		return AsEnumerable_NewObjects().FirstOrDefault(x => x.IdJobType == idJobType);
	}

	public static JobType? FromCode(string code)
	{
		return AsEnumerable_NewObjects().FirstOrDefault(v => v.Code?.Equals(code, StringComparison.Ordinal) ?? false);
	}

	public static readonly Lazy<IReadOnlyDictionary<Guid, JobType>> DictionaryMap = new(() => new Dictionary<Guid, JobType>
	{
			{ new Guid("00000001-0000-0000-0000-000000000000"), SequentialIntervalTimer_NewObject },
			{ new Guid("00000002-0000-0000-0000-000000000000"), ExactPeriodicTimer_NewObject },
			{ new Guid("00000003-0000-0000-0000-000000000000"), CronTimer_NewObject }
	});

	public static IEnumerable<JobType> AsEnumerable_NewObjects()
	{
		yield return SequentialIntervalTimer_NewObject;
		yield return ExactPeriodicTimer_NewObject;
		yield return CronTimer_NewObject;
	}
}
