namespace Legion.ADF.ESB.Components.Model;

public partial class JobStatus : Components.ComponentsBaseEntity, Legion.Model.IEntity
{
	/// <summary>
	/// 00000001-0000-0000-0000-000000000000
	/// </summary>
	public static Guid Disabled { get; }

	/// <summary>
	/// 00000002-0000-0000-0000-000000000000
	/// </summary>
	public static Guid Offline { get; }

	/// <summary>
	/// 00000003-0000-0000-0000-000000000000
	/// </summary>
	public static Guid Running { get; }

	/// <summary>
	/// 00000004-0000-0000-0000-000000000000
	/// </summary>
	public static Guid Idle { get; }

	/// <summary>
	/// 00000005-0000-0000-0000-000000000000
	/// </summary>
	public static Guid Error { get; }

	/// <summary>
	/// 00000006-0000-0000-0000-000000000000
	/// </summary>
	public static Guid Suspended { get; }

	static JobStatus()
	{
		Disabled = new Guid("00000001-0000-0000-0000-000000000000");
		Offline = new Guid("00000002-0000-0000-0000-000000000000");
		Running = new Guid("00000003-0000-0000-0000-000000000000");
		Idle = new Guid("00000004-0000-0000-0000-000000000000");
		Error = new Guid("00000005-0000-0000-0000-000000000000");
		Suspended = new Guid("00000006-0000-0000-0000-000000000000");

		DefaultDBValidator = SetDBValidatorRules(new Legion.Validation.ValidatorBuilder<JobStatus>()).Build();
	}

	public static IEnumerable<Guid> AsEnumerable()
	{
		yield return Disabled;
		yield return Offline;
		yield return Running;
		yield return Idle;
		yield return Error;
		yield return Suspended;
	}

	public JobStatusEnum ToEnum()
	{
		if (IdJobStatus == Disabled)
			return JobStatusEnum.Disabled;

		if (IdJobStatus == Offline)
			return JobStatusEnum.Offline;

		if (IdJobStatus == Running)
			return JobStatusEnum.Running;

		if (IdJobStatus == Idle)
			return JobStatusEnum.Idle;

		if (IdJobStatus == Error)
			return JobStatusEnum.Error;

		if (IdJobStatus == Suspended)
			return JobStatusEnum.Suspended;

		Legion.Throw.NotSupportedException($"Invalid {nameof(IdJobStatus)} value {IdJobStatus}");

		return 0;
	}

	public static implicit operator JobStatusEnum(JobStatus status)
		=> status?.ToEnum() ?? 0;

	public static implicit operator JobStatus(JobStatusEnum @enum)
		=> @enum switch
		{
			JobStatusEnum.Disabled => Disabled_NewObject,
			JobStatusEnum.Offline => Offline_NewObject,
			JobStatusEnum.Running => Running_NewObject,
			JobStatusEnum.Idle => Idle_NewObject,
			JobStatusEnum.Error => Error_NewObject,
			JobStatusEnum.Suspended => Suspended_NewObject,
			_ => throw new NotSupportedException($"Invalid {nameof(JobStatusEnum)} value {@enum}"),
		};
}
