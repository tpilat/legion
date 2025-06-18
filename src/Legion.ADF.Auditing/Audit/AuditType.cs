namespace Legion.ADF.Auditing.Audit;

public sealed partial class AuditType : Auditing.EntityBase, Legion.Model.IEntity
{

	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid IdAuditType { get; private set; }

	/// <summary>
	/// Database DataType: varchar(15) NOT NULL
	/// </summary>
	public string Code { get; private set; }

	/// <summary>
	/// Database DataType: varchar(15) NOT NULL
	/// </summary>
	public string Name { get; private set; }

	/// <summary>
	/// Database DataType: integer NOT NULL
	/// </summary>
	public int ItemCode { get; private set; }

	private AuditType()
	{
	}

	public override string? ToString()
	{
		return Code;
	}
}
