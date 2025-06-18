using Legion.Validation;

namespace Legion.ADF.Audit.Model;

public sealed partial class AuditEntry : Audit.AuditBaseEntity, Legion.Model.IEntity
{
	public static IValidator<AuditEntry> DefaultDBValidator { get; }

	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid IdAuditEntry { get; private set; }

	/// <summary>
	/// Database DataType: timestamp with time zone NOT NULL
	/// </summary>
	public DateTime CreatedUtc { get; private set; }

	/// <summary>
	/// Database DataType: uuid NOT NULL | Audit.Model.AuditOperation.AuditOperation | FK_AuditEntry_IdAuditOperation
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


	/// <summary>
	/// _1:N Guid IdAuditOperation | FK_AuditEntry_IdAuditOperation
	/// </summary>
	public Audit.Model.AuditOperation AuditOperation { get; private set; }

	private AuditEntry()
	{
	}

	static AuditEntry()
	{
		DefaultDBValidator = SetDBValidatorRules(new ValidatorBuilder<AuditEntry>()).Build();
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

	public void TrimStringValuesToFitDatabaseMaxLengths(string? postfix = null)
	{
		TableName = Legion.Text.StringHelper.TrimToFitMaxLength(TableName, 255, postfix);
	}

	public override string? GetPrimaryKeyValue()
	{
		return IdAuditEntry.ToString();
	}

	public override string? ToString()
	{
		return IdAuditEntry.ToString();
	}

	public static ValidatorBuilder<AuditEntry> SetDBValidatorRules(ValidatorBuilder<AuditEntry> builder)
		=> builder
			.ForProperty(x => x.IdAuditEntry, v => v.NotDefaultOrEmpty())
			//.ForProperty(x => x.CreatedUtc, v => v.NotDefaultOrEmpty())
			.ForProperty(x => x.IdAuditOperation, v => v.NotDefaultOrEmpty(), (x, parent) => x.AuditOperation == null)
			.ForProperty(x => x.TableName, v => v.NotDefaultOrEmpty().MaxLength(255))
			//.ForProperty(x => x.AuditCorrelationId, v => v.NotDefaultOrEmpty())
		;
}
