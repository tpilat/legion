namespace Legion.ADF.Messaging.DomainEvents.Model;

public partial class DomainEventProcessingStatus : DomainEvents.DomainEventsBaseEntity, Legion.Model.IEntity
{
	/// <summary>
	/// 00000001-0000-0000-0000-000000000000
	/// </summary>
	public static Guid Created { get; }

	/// <summary>
	/// 00000002-0000-0000-0000-000000000000
	/// </summary>
	public static Guid Processing { get; }

	/// <summary>
	/// 00000003-0000-0000-0000-000000000000
	/// </summary>
	public static Guid Processed { get; }

	/// <summary>
	/// 00000004-0000-0000-0000-000000000000
	/// </summary>
	public static Guid Failed { get; }

	/// <summary>
	/// 00000005-0000-0000-0000-000000000000
	/// </summary>
	public static Guid Suspended { get; }

	/// <summary>
	/// 00000006-0000-0000-0000-000000000000
	/// </summary>
	public static Guid NoHandler { get; }

	/// <summary>
	/// 00000007-0000-0000-0000-000000000000
	/// </summary>
	public static Guid Blocked { get; }

	static DomainEventProcessingStatus()
	{
		Created = new Guid("00000001-0000-0000-0000-000000000000");
		Processing = new Guid("00000002-0000-0000-0000-000000000000");
		Processed = new Guid("00000003-0000-0000-0000-000000000000");
		Failed = new Guid("00000004-0000-0000-0000-000000000000");
		Suspended = new Guid("00000005-0000-0000-0000-000000000000");
		NoHandler = new Guid("00000006-0000-0000-0000-000000000000");
		Blocked = new Guid("00000007-0000-0000-0000-000000000000");

		DefaultDBValidator = SetDBValidatorRules(new Legion.Validation.ValidatorBuilder<DomainEventProcessingStatus>()).Build();
	}

	public static IEnumerable<Guid> AsEnumerable()
	{
		yield return Created;
		yield return Processing;
		yield return Processed;
		yield return Failed;
		yield return Suspended;
		yield return NoHandler;
		yield return Blocked;
	}
}
