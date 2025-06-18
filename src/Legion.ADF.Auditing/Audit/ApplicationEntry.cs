namespace Legion.ADF.Auditing.Audit;

public sealed partial class ApplicationEntry : Auditing.EntityBase, Legion.Model.IEntity
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
	/// Database DataType: integer NOT NULL
	/// </summary>
	public int AuditOperation { get; private set; }

	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid RuntimeUniqueKey { get; private set; }

	/// <summary>
	/// Database DataType: timestamp without time zone NOT NULL
	/// </summary>
	public DateTime CreatedUtc { get; private set; }

	/// <summary>
	/// Database DataType: uuid NULL
	/// </summary>
	public Guid? CorrelationId { get; private set; }

	/// <summary>
	/// Database DataType: varchar(127) NULL
	/// </summary>
	public string? ExternalCorrelationId { get; private set; }

	/// <summary>
	/// Database DataType: varchar(511) NULL
	/// </summary>
	public string? MainEntityIdentifier { get; private set; }

	/// <summary>
	/// Database DataType: varchar(1023) NULL
	/// </summary>
	public string? Uri { get; private set; }

	/// <summary>
	/// Database DataType: uuid NULL
	/// </summary>
	public Guid? IdUser { get; private set; }

	private ApplicationEntry()
	{
	}

	public override string? ToString()
	{
		return Uri;
	}
}
