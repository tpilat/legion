namespace Legion.ADF.Messaging.MessageBox.Model;

public partial class MessageStatus : MessageBox.MessageBoxBaseEntity, Legion.Model.IEntity
{
	/// <summary>
	/// 00000001-0000-0000-0000-000000000000
	/// </summary>
	public static Guid Created { get; }

	/// <summary>
	/// 00000002-0000-0000-0000-000000000000
	/// </summary>
	public static Guid Processed { get; }

	/// <summary>
	/// 00000003-0000-0000-0000-000000000000
	/// </summary>
	public static Guid Dropped { get; }

	static MessageStatus()
	{
		Created = new Guid("00000001-0000-0000-0000-000000000000");
		Processed = new Guid("00000002-0000-0000-0000-000000000000");
		Dropped = new Guid("00000003-0000-0000-0000-000000000000");

		DefaultDBValidator = SetDBValidatorRules(new Legion.Validation.ValidatorBuilder<MessageStatus>()).Build();
	}

	public static IEnumerable<Guid> AsEnumerable()
	{
		yield return Created;
		yield return Processed;
		yield return Dropped;
	}
}
