namespace Legion.ADF.Logs.Model;

public partial class LogLevel : Logs.LogsBaseEntity, Legion.Model.IEntity
{
	/// <summary>
	/// 00000001-0000-0000-0000-000000000000
	/// </summary>
	public static Guid Trace { get; }

	/// <summary>
	/// 00000002-0000-0000-0000-000000000000
	/// </summary>
	public static Guid Debug { get; }

	/// <summary>
	/// 00000003-0000-0000-0000-000000000000
	/// </summary>
	public static Guid Info { get; }

	/// <summary>
	/// 00000004-0000-0000-0000-000000000000
	/// </summary>
	public static Guid Warning { get; }

	/// <summary>
	/// 00000005-0000-0000-0000-000000000000
	/// </summary>
	public static Guid Error { get; }

	/// <summary>
	/// 00000006-0000-0000-0000-000000000000
	/// </summary>
	public static Guid Critical { get; }

	static LogLevel()
	{
		Trace = new Guid("00000001-0000-0000-0000-000000000000");
		Debug = new Guid("00000002-0000-0000-0000-000000000000");
		Info = new Guid("00000003-0000-0000-0000-000000000000");
		Warning = new Guid("00000004-0000-0000-0000-000000000000");
		Error = new Guid("00000005-0000-0000-0000-000000000000");
		Critical = new Guid("00000006-0000-0000-0000-000000000000");

		DefaultDBValidator = SetDBValidatorRules(new Legion.Validation.ValidatorBuilder<LogLevel>()).Build();
	}

	public static IEnumerable<Guid> AsEnumerable()
	{
		yield return Trace;
		yield return Debug;
		yield return Info;
		yield return Warning;
		yield return Error;
		yield return Critical;
	}
}
