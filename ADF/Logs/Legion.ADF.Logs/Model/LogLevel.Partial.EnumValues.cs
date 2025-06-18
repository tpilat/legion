namespace Legion.ADF.Logs.Model;

public partial class LogLevel : Logs.LogsBaseEntity, Legion.Model.IEntity, IComparable
{
	/// <summary>
	/// 00000001-0000-0000-0000-000000000000
	/// </summary>
	public static LogLevel Trace_NewObject => new(new Guid("00000001-0000-0000-0000-000000000000"), "Trace", "Trace", 0);

	/// <summary>
	/// 00000002-0000-0000-0000-000000000000
	/// </summary>
	public static LogLevel Debug_NewObject => new(new Guid("00000002-0000-0000-0000-000000000000"), "Debug", "Debug", 1);

	/// <summary>
	/// 00000003-0000-0000-0000-000000000000
	/// </summary>
	public static LogLevel Info_NewObject => new(new Guid("00000003-0000-0000-0000-000000000000"), "Info", "Info", 2);

	/// <summary>
	/// 00000004-0000-0000-0000-000000000000
	/// </summary>
	public static LogLevel Warning_NewObject => new(new Guid("00000004-0000-0000-0000-000000000000"), "Warning", "Warning", 3);

	/// <summary>
	/// 00000005-0000-0000-0000-000000000000
	/// </summary>
	public static LogLevel Error_NewObject => new(new Guid("00000005-0000-0000-0000-000000000000"), "Error", "Error", 4);

	/// <summary>
	/// 00000006-0000-0000-0000-000000000000
	/// </summary>
	public static LogLevel Critical_NewObject => new(new Guid("00000006-0000-0000-0000-000000000000"), "Critical", "Critical", 5);

	private LogLevel(Guid idLogLevel, string code, string name, int itemCode)
		: this()
	{
		IdLogLevel = idLogLevel;
		Code = code;
		Name = name;
		ItemCode = itemCode;
	}

	public override bool Equals(object? obj)
	{
		if (obj is not LogLevel otherValue)
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
		var otherEnumeration = other as LogLevel;
		return Code?.CompareTo(otherEnumeration?.Code) ?? 1;
	}

	public static LogLevel? FromId(Guid idLogLevel)
	{
		return AsEnumerable_NewObjects().FirstOrDefault(x => x.IdLogLevel == idLogLevel);
	}

	public static LogLevel? FromCode(string code)
	{
		return AsEnumerable_NewObjects().FirstOrDefault(v => v.Code?.Equals(code, StringComparison.Ordinal) ?? false);
	}

	public static readonly Lazy<IReadOnlyDictionary<Guid, LogLevel>> DictionaryMap = new(() => new Dictionary<Guid, LogLevel>
	{
			{ new Guid("00000001-0000-0000-0000-000000000000"), Trace_NewObject },
			{ new Guid("00000002-0000-0000-0000-000000000000"), Debug_NewObject },
			{ new Guid("00000003-0000-0000-0000-000000000000"), Info_NewObject },
			{ new Guid("00000004-0000-0000-0000-000000000000"), Warning_NewObject },
			{ new Guid("00000005-0000-0000-0000-000000000000"), Error_NewObject },
			{ new Guid("00000006-0000-0000-0000-000000000000"), Critical_NewObject }
	});

	public static IEnumerable<LogLevel> AsEnumerable_NewObjects()
	{
		yield return Trace_NewObject;
		yield return Debug_NewObject;
		yield return Info_NewObject;
		yield return Warning_NewObject;
		yield return Error_NewObject;
		yield return Critical_NewObject;
	}
}
