namespace Legion.ADF.ESB.Components.Model;

public partial class JobType : Components.ComponentsBaseEntity, Legion.Model.IEntity
{
	/// <summary>
	/// 00000001-0000-0000-0000-000000000000
	/// </summary>
	public static Guid SequentialIntervalTimer { get; }

	/// <summary>
	/// 00000002-0000-0000-0000-000000000000
	/// </summary>
	public static Guid ExactPeriodicTimer { get; }

	/// <summary>
	/// 00000003-0000-0000-0000-000000000000
	/// </summary>
	public static Guid CronTimer { get; }

	static JobType()
	{
		SequentialIntervalTimer = new Guid("00000001-0000-0000-0000-000000000000");
		ExactPeriodicTimer = new Guid("00000002-0000-0000-0000-000000000000");
		CronTimer = new Guid("00000003-0000-0000-0000-000000000000");

		DefaultDBValidator = SetDBValidatorRules(new Legion.Validation.ValidatorBuilder<JobType>()).Build();
	}

	public static IEnumerable<Guid> AsEnumerable()
	{
		yield return SequentialIntervalTimer;
		yield return ExactPeriodicTimer;
		yield return CronTimer;
	}

	public JobTypeEnum ToEnum()
	{
		if (IdJobType == SequentialIntervalTimer)
			return JobTypeEnum.SequentialIntervalTimer;

		if (IdJobType == ExactPeriodicTimer)
			return JobTypeEnum.ExactPeriodicTimer;

		if (IdJobType == CronTimer)
			return JobTypeEnum.CronTimer;

		Legion.Throw.NotSupportedException($"Invalid {nameof(IdJobType)} value {IdJobType}");

		return 0;
	}

	public static implicit operator JobTypeEnum(JobType status)
		=> status?.ToEnum() ?? 0;

	public static implicit operator JobType(JobTypeEnum @enum)
		=> @enum switch
		{
			JobTypeEnum.SequentialIntervalTimer => SequentialIntervalTimer_NewObject,
			JobTypeEnum.ExactPeriodicTimer => ExactPeriodicTimer_NewObject,
			JobTypeEnum.CronTimer => CronTimer_NewObject,
			_ => throw new NotSupportedException($"Invalid {nameof(JobTypeEnum)} value {@enum}"),
		};
}
