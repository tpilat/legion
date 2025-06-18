namespace Legion.ADF.ServiceBus.Jobs.Model;

public partial class JobMessageType : Jobs.JobsBaseEntity, Legion.Model.IEntity
{
	/// <summary>
	/// 00000001-0000-0000-0000-000000000000
	/// </summary>
	public static Guid Published { get; }

	/// <summary>
	/// 00000002-0000-0000-0000-000000000000
	/// </summary>
	public static Guid SubscribedFromQueue { get; }

	/// <summary>
	/// 00000003-0000-0000-0000-000000000000
	/// </summary>
	public static Guid SubscribedFromTopic { get; }

	static JobMessageType()
	{
		Published = new Guid("00000001-0000-0000-0000-000000000000");
		SubscribedFromQueue = new Guid("00000002-0000-0000-0000-000000000000");
		SubscribedFromTopic = new Guid("00000003-0000-0000-0000-000000000000");

		DefaultDBValidator = SetDBValidatorRules(new Legion.Validation.ValidatorBuilder<JobMessageType>()).Build();
	}

	public static IEnumerable<Guid> AsEnumerable()
	{
		yield return Published;
		yield return SubscribedFromQueue;
		yield return SubscribedFromTopic;
	}
}
