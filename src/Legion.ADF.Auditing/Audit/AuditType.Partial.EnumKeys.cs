namespace Legion.ADF.Auditing.Audit;

public partial class AuditType : Auditing.EntityBase, Legion.Model.IEntity
{
	/// <summary>
	/// 00000001-0000-0000-0000-000000000000
	/// </summary>
	public static Guid None { get; }

	/// <summary>
	/// 00000002-0000-0000-0000-000000000000
	/// </summary>
	public static Guid Insert { get; }

	/// <summary>
	/// 00000003-0000-0000-0000-000000000000
	/// </summary>
	public static Guid Update { get; }

	/// <summary>
	/// 00000004-0000-0000-0000-000000000000
	/// </summary>
	public static Guid Delete { get; }

	/// <summary>
	/// 00000005-0000-0000-0000-000000000000
	/// </summary>
	public static Guid Read { get; }

	static AuditType()
	{
		None = new Guid("00000001-0000-0000-0000-000000000000");
		Insert = new Guid("00000002-0000-0000-0000-000000000000");
		Update = new Guid("00000003-0000-0000-0000-000000000000");
		Delete = new Guid("00000004-0000-0000-0000-000000000000");
		Read = new Guid("00000005-0000-0000-0000-000000000000");
	}

	public static IEnumerable<Guid> AsEnumerable()
	{
		yield return None;
		yield return Insert;
		yield return Update;
		yield return Delete;
		yield return Read;
	}
}
