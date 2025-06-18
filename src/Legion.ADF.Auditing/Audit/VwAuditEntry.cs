namespace Legion.ADF.Auditing.Audit;

public partial class VwAuditEntry : Auditing.QueryEntityBase, Legion.Model.IQueryEntity
{
	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid IdAuditEntry { get; private set; }

	/// <summary>
	/// Database DataType: timestamp without time zone NOT NULL
	/// </summary>
	public DateTime CreatedUtc { get; private set; }

	/// <summary>
	/// Database DataType: timestamp without time zone NOT NULL
	/// </summary>
	public DateTime CreatedAt { get; private set; }

	/// <summary>
	/// Database DataType: integer NOT NULL
	/// </summary>
	public int IdAuditType { get; private set; }

	/// <summary>
	/// Database DataType: varchar(15) NOT NULL
	/// </summary>
	public string AuditType { get; private set; }

	/// <summary>
	/// Database DataType: varchar(255) NOT NULL
	/// </summary>
	public string TableName { get; private set; }

	/// <summary>
	/// Database DataType: uuid NULL
	/// </summary>
	public Guid? IdUser { get; private set; }

	/// <summary>
	/// Database DataType: varchar(255) NULL
	/// </summary>
	public string? Login { get; private set; }

	/// <summary>
	/// Database DataType: text NULL
	/// </summary>
	public string? PrimaryKey { get; private set; }

	/// <summary>
	/// Database DataType: text NULL
	/// </summary>
	public string? OldValues { get; private set; }

	/// <summary>
	/// Database DataType: text NULL
	/// </summary>
	public string? NewValues { get; private set; }

	/// <summary>
	/// Database DataType: text NULL
	/// </summary>
	public string? AffectedColumns { get; private set; }

	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid AuditCorrelationId { get; private set; }

	/// <summary>
	/// Database DataType: varchar(1023) NULL
	/// </summary>
	public string? CommandQueryName { get; private set; }

	/// <summary>
	/// Database DataType: uuid NULL
	/// </summary>
	public Guid? IdCommandQuery { get; private set; }

	/// <summary>
	/// Database DataType: uuid NULL
	/// </summary>
	public Guid? CorrelationId { get; private set; }


	private VwAuditEntry()
	{
	}

	public override string? ToString()
	{
		return IdAuditEntry.ToString();
	}
}
