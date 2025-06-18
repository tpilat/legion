namespace Legion.ADF.ServiceBus.Jobs.Model;

public partial class JobRunType : Jobs.JobsBaseEntity, Legion.Model.IEntity
{
	/// <summary>
	/// 00000001-0000-0000-0000-000000000000
	/// </summary>
	public static Guid SequentialTimer { get; }

	/// <summary>
	/// 00000002-0000-0000-0000-000000000000
	/// </summary>
	public static Guid PeriodicTimer { get; }

	/// <summary>
	/// 00000003-0000-0000-0000-000000000000
	/// </summary>
	public static Guid Cron { get; }

	static JobRunType()
	{
		SequentialTimer = new Guid("00000001-0000-0000-0000-000000000000");
		PeriodicTimer = new Guid("00000002-0000-0000-0000-000000000000");
		Cron = new Guid("00000003-0000-0000-0000-000000000000");

		DefaultDBValidator = SetDBValidatorRules(new Legion.Validation.ValidatorBuilder<JobRunType>()).Build();
	}

	public static IEnumerable<Guid> AsEnumerable()
	{
		yield return SequentialTimer;
		yield return PeriodicTimer;
		yield return Cron;
	}
}
