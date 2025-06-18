using Legion.Validation;

namespace Legion.ADF.Audit.Model;

public sealed partial class AuditOperation : Audit.AuditBaseEntity, Legion.Model.IEntity
{
	private List<Audit.Model.ApplicationEntry> _applicationEntries;
	private List<Audit.Model.AuditEntry> _auditEntries;

	public static IValidator<AuditOperation> DefaultDBValidator { get; }

	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid IdAuditOperation { get; private set; }

	/// <summary>
	/// Database DataType: varchar(15) NOT NULL
	/// </summary>
	public string Code { get; private set; }

	/// <summary>
	/// Database DataType: varchar(15) NOT NULL
	/// </summary>
	public string Name { get; private set; }


	/// <summary>
	/// N:_1 Audit.Model.ApplicationEntry.IdAuditOperation | FK_ApplicationEntry_IdAuditOperation
	/// </summary>
	public IReadOnlyList<Audit.Model.ApplicationEntry> ApplicationEntries => _applicationEntries;

	/// <summary>
	/// N:_1 Audit.Model.AuditEntry.IdAuditOperation | FK_AuditEntry_IdAuditOperation
	/// </summary>
	public IReadOnlyList<Audit.Model.AuditEntry> AuditEntries => _auditEntries;

	private AuditOperation()
	{
		_applicationEntries = [];
		_auditEntries = [];
	}

	public Dictionary<string, object?> ToDictionary()
		=> new()
		{
			{ nameof(IdAuditOperation), IdAuditOperation },
			{ nameof(Code), Code },
			{ nameof(Name), Name },
		};

	public void TrimStringValuesToFitDatabaseMaxLengths(string? postfix = null)
	{
		Code = Legion.Text.StringHelper.TrimToFitMaxLength(Code, 15, postfix);
		Name = Legion.Text.StringHelper.TrimToFitMaxLength(Name, 15, postfix);
	}

	public override string? GetPrimaryKeyValue()
	{
		return IdAuditOperation.ToString();
	}

	public override string? ToString()
	{
		return Code;
	}

	public static ValidatorBuilder<AuditOperation> SetDBValidatorRules(ValidatorBuilder<AuditOperation> builder)
		=> builder
			.ForProperty(x => x.IdAuditOperation, v => v.NotDefaultOrEmpty())
			.ForProperty(x => x.Code, v => v.NotDefaultOrEmpty().MaxLength(15))
			.ForProperty(x => x.Name, v => v.NotDefaultOrEmpty().MaxLength(15))
		;
}
