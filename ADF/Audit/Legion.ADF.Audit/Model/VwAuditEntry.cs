namespace Legion.ADF.Audit.Model;

public sealed partial class VwAuditEntry : Audit.AuditBaseQueryEntity, Legion.Model.IQueryEntity
{
	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid IdAuditEntry { get; private set; }

	/// <summary>
	/// Database DataType: timestamp with time zone NOT NULL
	/// </summary>
	public DateTime CreatedUtc { get; private set; }

	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid IdAuditOperation { get; private set; }

	/// <summary>
	/// Database DataType: varchar(255) NOT NULL
	/// </summary>
	public string TableName { get; private set; }

	/// <summary>
	/// Database DataType: uuid NULL
	/// </summary>
	public Guid? IdUser { get; private set; }

	/// <summary>
	/// Database DataType: jsonb NULL
	/// </summary>
	public string? PrimaryKey { get; private set; }

	/// <summary>
	/// Database DataType: jsonb NULL
	/// </summary>
	public string? OldValues { get; private set; }

	/// <summary>
	/// Database DataType: jsonb NULL
	/// </summary>
	public string? NewValues { get; private set; }

	/// <summary>
	/// Database DataType: jsonb NULL
	/// </summary>
	public string? AffectedColumns { get; private set; }

	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid AuditCorrelationId { get; private set; }

	/// <summary>
	/// Database DataType: text NULL
	/// </summary>
	public string? TraceFrame { get; private set; }

	/// <summary>
	/// Database DataType: uuid NULL
	/// </summary>
	public Guid? CorrelationId { get; private set; }


	private VwAuditEntry()
	{
	}

	public Dictionary<string, object?> ToDictionary()
		=> new()
		{
			{ nameof(IdAuditEntry), IdAuditEntry },
			{ nameof(CreatedUtc), CreatedUtc },
			{ nameof(IdAuditOperation), IdAuditOperation },
			{ nameof(TableName), TableName },
			{ nameof(IdUser), IdUser },
			{ nameof(PrimaryKey), PrimaryKey },
			{ nameof(OldValues), OldValues },
			{ nameof(NewValues), NewValues },
			{ nameof(AffectedColumns), AffectedColumns },
			{ nameof(AuditCorrelationId), AuditCorrelationId },
			{ nameof(TraceFrame), TraceFrame },
			{ nameof(CorrelationId), CorrelationId },
		};

	public override string? ToString()
	{
		return IdAuditEntry.ToString();
	}
}
