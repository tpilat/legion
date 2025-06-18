namespace Legion.ADF.Messaging.MessageBox.Model;

public partial class QueueProcessingMode : MessageBox.MessageBoxBaseEntity, Legion.Model.IEntity
{
	/// <summary>
	/// 00000001-0000-0000-0000-000000000000
	/// </summary>
	public static Guid NoAction { get; }

	/// <summary>
	/// 00000002-0000-0000-0000-000000000000
	/// </summary>
	public static Guid Archivate { get; }

	/// <summary>
	/// 00000003-0000-0000-0000-000000000000
	/// </summary>
	public static Guid Delete { get; }

	static QueueProcessingMode()
	{
		NoAction = new Guid("00000001-0000-0000-0000-000000000000");
		Archivate = new Guid("00000002-0000-0000-0000-000000000000");
		Delete = new Guid("00000003-0000-0000-0000-000000000000");

		DefaultDBValidator = SetDBValidatorRules(new Legion.Validation.ValidatorBuilder<QueueProcessingMode>()).Build();
	}

	public static IEnumerable<Guid> AsEnumerable()
	{
		yield return NoAction;
		yield return Archivate;
		yield return Delete;
	}
}
