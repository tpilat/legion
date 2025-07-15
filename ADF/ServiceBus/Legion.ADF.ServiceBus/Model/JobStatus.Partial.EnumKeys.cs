namespace Legion.ADF.ServiceBus.Model;

public partial class JobStatus : ServiceBus.ServiceBusBaseEntity, Legion.Model.IEntity
{
	/// <summary>
	/// 00000001-0000-0000-0000-000000000000
	/// </summary>
	public static Guid Disconnected { get; }

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
	public static Guid Disabling { get; }

	/// <summary>
	/// 00000007-0000-0000-0000-000000000000
	/// </summary>
	public static Guid Disabled { get; }

	/// <summary>
	/// 00000008-0000-0000-0000-000000000000
	/// </summary>
	public static Guid Canceling { get; }

	/// <summary>
	/// 00000009-0000-0000-0000-000000000000
	/// </summary>
	public static Guid Stopping { get; }

	static JobStatus()
	{
		Disconnected = new Guid("00000001-0000-0000-0000-000000000000");
		Started = new Guid("00000002-0000-0000-0000-000000000000");
		Idle = new Guid("00000003-0000-0000-0000-000000000000");
		Running = new Guid("00000004-0000-0000-0000-000000000000");
		Error = new Guid("00000005-0000-0000-0000-000000000000");
		Disabling = new Guid("00000006-0000-0000-0000-000000000000");
		Disabled = new Guid("00000007-0000-0000-0000-000000000000");
		Canceling = new Guid("00000008-0000-0000-0000-000000000000");
		Stopping = new Guid("00000009-0000-0000-0000-000000000000");

		DefaultDBValidator = SetDBValidatorRules(new Legion.Validation.ValidatorBuilder<JobStatus>()).Build();
	}

	public static IEnumerable<Guid> AsEnumerable()
	{
		yield return Disconnected;
		yield return Started;
		yield return Idle;
		yield return Running;
		yield return Error;
		yield return Disabling;
		yield return Disabled;
		yield return Canceling;
		yield return Stopping;
	}
}
