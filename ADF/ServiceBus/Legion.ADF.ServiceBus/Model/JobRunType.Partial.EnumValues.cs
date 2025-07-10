namespace Legion.ADF.ServiceBus.Model;

public partial class JobRunType : ServiceBus.ServiceBusBaseEntity, Legion.Model.IEntity, IComparable
{
	/// <summary>
	/// 00000001-0000-0000-0000-000000000000
	/// </summary>
	public static JobRunType SequentialTimer_NewObject => new(new Guid("00000001-0000-0000-0000-000000000000"), "SequentialTimer", "SequentialTimer");

	/// <summary>
	/// 00000002-0000-0000-0000-000000000000
	/// </summary>
	public static JobRunType PeriodicTimer_NewObject => new(new Guid("00000002-0000-0000-0000-000000000000"), "PeriodicTimer", "PeriodicTimer");

	/// <summary>
	/// 00000003-0000-0000-0000-000000000000
	/// </summary>
	public static JobRunType Cron_NewObject => new(new Guid("00000003-0000-0000-0000-000000000000"), "Cron", "Cron");

	private JobRunType(Guid idJobRunType, string code, string name)
		: this()
	{
		IdJobRunType = idJobRunType;
		Code = code;
		Name = name;
	}

	public override bool Equals(object? obj)
	{
		if (obj is not JobRunType otherValue)
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
		var otherEnumeration = other as JobRunType;
		return Code?.CompareTo(otherEnumeration?.Code) ?? 1;
	}

	public static JobRunType? FromId(Guid idJobRunType)
	{
		return AsEnumerable_NewObjects().FirstOrDefault(x => x.IdJobRunType == idJobRunType);
	}

	public static JobRunType? FromCode(string code)
	{
		return AsEnumerable_NewObjects().FirstOrDefault(v => v.Code?.Equals(code, StringComparison.Ordinal) ?? false);
	}

	public static readonly Lazy<IReadOnlyDictionary<Guid, JobRunType>> DictionaryMap = new(() => new Dictionary<Guid, JobRunType>
	{
			{ new Guid("00000001-0000-0000-0000-000000000000"), SequentialTimer_NewObject },
			{ new Guid("00000002-0000-0000-0000-000000000000"), PeriodicTimer_NewObject },
			{ new Guid("00000003-0000-0000-0000-000000000000"), Cron_NewObject }
	});

	public static IEnumerable<JobRunType> AsEnumerable_NewObjects()
	{
		yield return SequentialTimer_NewObject;
		yield return PeriodicTimer_NewObject;
		yield return Cron_NewObject;
	}
}
