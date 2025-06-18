namespace Legion.ADF.ServiceBus.Jobs.Model;

public partial class JobStatus : Jobs.JobsBaseEntity, Legion.Model.IEntity
{
	/// <summary>
	/// 00000001-0000-0000-0000-000000000000
	/// </summary>
	public static Guid Disabled { get; }

	/// <summary>
	/// 00000002-0000-0000-0000-000000000000
	/// </summary>
	public static Guid Started { get; }

	/// <summary>
	/// 00000003-0000-0000-0000-000000000000
	/// </summary>
	public static Guid Idle { get; }

	/// <summary>
	/// 00000004-0000-0000-0000-000000000000
	/// </summary>
	public static Guid Running { get; }

	/// <summary>
	/// 00000005-0000-0000-0000-000000000000
	/// </summary>
	public static Guid Error { get; }

	/// <summary>
	/// 00000006-0000-0000-0000-000000000000
	/// </summary>
	public static Guid Suspended { get; }

	/// <summary>
	/// 00000007-0000-0000-0000-000000000000
	/// </summary>
	public static Guid Stopped { get; }

	static JobStatus()
	{
		Disabled = new Guid("00000001-0000-0000-0000-000000000000");
		Started = new Guid("00000002-0000-0000-0000-000000000000");
		Idle = new Guid("00000003-0000-0000-0000-000000000000");
		Running = new Guid("00000004-0000-0000-0000-000000000000");
		Error = new Guid("00000005-0000-0000-0000-000000000000");
		Suspended = new Guid("00000006-0000-0000-0000-000000000000");
		Stopped = new Guid("00000007-0000-0000-0000-000000000000");

		DefaultDBValidator = SetDBValidatorRules(new Legion.Validation.ValidatorBuilder<JobStatus>()).Build();
	}

	public static IEnumerable<Guid> AsEnumerable()
	{
		yield return Disabled;
		yield return Started;
		yield return Idle;
		yield return Running;
		yield return Error;
		yield return Suspended;
		yield return Stopped;
	}
}
