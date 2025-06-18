namespace Legion.ADF.Auditing.Audit;

public partial class VwApplicationEntry : Auditing.QueryEntityBase, Legion.Model.IQueryEntity
{
	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid IdApplicationEntry { get; private set; }

	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid IdApplicationEntryToken { get; private set; }

	/// <summary>
	/// Database DataType: varchar(255) NOT NULL
	/// </summary>
	public string Token { get; private set; }

	/// <summary>
	/// Database DataType: integer NOT NULL
	/// </summary>
	public int Version { get; private set; }

	/// <summary>
	/// Database DataType: varchar(511) NOT NULL
	/// </summary>
	public string SourceFilePath { get; private set; }

	/// <summary>
	/// Database DataType: integer NOT NULL
	/// </summary>
	public int AuditOperation { get; private set; }

	/// <summary>
	/// Database DataType: varchar(1023) NULL
	/// </summary>
	public string? Uri { get; private set; }

	/// <summary>
	/// Database DataType: varchar(511) NULL
	/// </summary>
	public string? MethodInfo { get; private set; }

	/// <summary>
	/// Database DataType: varchar(255) NULL
	/// </summary>
	public string? MainEntityName { get; private set; }

	/// <summary>
	/// Database DataType: varchar(511) NULL
	/// </summary>
	public string? MainEntityIdentifier { get; private set; }

	/// <summary>
	/// Database DataType: varchar(511) NULL
	/// </summary>
	public string? Description { get; private set; }

	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid RuntimeUniqueKey { get; private set; }

	/// <summary>
	/// Database DataType: timestamp without time zone NOT NULL
	/// </summary>
	public DateTime CreatedUtc { get; private set; }

	/// <summary>
	/// Database DataType: timestamp without time zone NOT NULL
	/// </summary>
	public DateTime CreatedAt { get; private set; }

	/// <summary>
	/// Database DataType: uuid NULL
	/// </summary>
	public Guid? CorrelationId { get; private set; }

	/// <summary>
	/// Database DataType: varchar(127) NULL
	/// </summary>
	public string? ExternalCorrelationId { get; private set; }

	/// <summary>
	/// Database DataType: uuid NULL
	/// </summary>
	public Guid? IdUser { get; private set; }

	/// <summary>
	/// Database DataType: varchar(255) NULL
	/// </summary>
	public string? Login { get; private set; }

	/// <summary>
	/// Database DataType: varchar(127) NULL
	/// </summary>
	public string? ApplicationName { get; private set; }


	private VwApplicationEntry()
	{
	}

	public override string? ToString()
	{
		return IdApplicationEntry.ToString();
	}
}
