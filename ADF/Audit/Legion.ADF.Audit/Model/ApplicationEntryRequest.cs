using Legion.Validation;

namespace Legion.ADF.Audit.Model;

public sealed partial class ApplicationEntryRequest : Audit.AuditBaseEntity, Legion.Model.IEntity
{
	public static IValidator<ApplicationEntryRequest> DefaultDBValidator { get; }

	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid IdApplicationEntryRequest { get; private set; }

	/// <summary>
	/// Database DataType: uuid NOT NULL | Audit.Model.ApplicationEntry.ApplicationEntry | FK_ApplicationEntryRequest_IdApplicationEntry
	/// </summary>
	public Guid IdApplicationEntry { get; private set; }

	/// <summary>
	/// Database DataType: timestamp with time zone NOT NULL
	/// </summary>
	public DateTime CreatedUtc { get; private set; }

	/// <summary>
	/// Database DataType: jsonb NULL
	/// </summary>
	public string? Metadata { get; private set; }

	/// <summary>
	/// Database DataType: varchar(1023) NOT NULL
	/// </summary>
	public string MimeType { get; private set; }

	/// <summary>
	/// Database DataType: varchar(63) NULL
	/// </summary>
	public string? ContentEncoding { get; private set; }

	/// <summary>
	/// Database DataType: bytea NULL
	/// </summary>
	public byte[]? ByteArrayContent { get; private set; }

	/// <summary>
	/// Database DataType: jsonb NULL
	/// </summary>
	public string? JsonContent { get; private set; }

	/// <summary>
	/// Database DataType: text NULL
	/// </summary>
	public string? StringContent { get; private set; }

	/// <summary>
	/// Database DataType: bigint NULL
	/// </summary>
	public long? DbOid { get; private set; }

	/// <summary>
	/// Database DataType: varchar(511) NULL
	/// </summary>
	public string? Name { get; private set; }

	/// <summary>
	/// Database DataType: varchar(1023) NULL
	/// </summary>
	public string? RelativePath { get; private set; }

	/// <summary>
	/// Database DataType: boolean NOT NULL
	/// </summary>
	public bool IsCompressed { get; private set; }

	/// <summary>
	/// Database DataType: text NULL
	/// </summary>
	public string? EncryptionKey { get; private set; }


	/// <summary>
	/// _1:N Guid IdApplicationEntry | FK_ApplicationEntryRequest_IdApplicationEntry
	/// </summary>
	public Audit.Model.ApplicationEntry ApplicationEntry { get; private set; }

	private ApplicationEntryRequest()
	{
	}

	static ApplicationEntryRequest()
	{
		DefaultDBValidator = SetDBValidatorRules(new ValidatorBuilder<ApplicationEntryRequest>()).Build();
	}

	public Dictionary<string, object?> ToDictionary()
		=> new()
		{
			{ nameof(IdApplicationEntryRequest), IdApplicationEntryRequest },
			{ nameof(IdApplicationEntry), IdApplicationEntry },
			{ nameof(CreatedUtc), CreatedUtc },
			{ nameof(Metadata), Metadata },
			{ nameof(MimeType), MimeType },
			{ nameof(ContentEncoding), ContentEncoding },
			{ nameof(ByteArrayContent), ByteArrayContent },
			{ nameof(JsonContent), JsonContent },
			{ nameof(StringContent), StringContent },
			{ nameof(DbOid), DbOid },
			{ nameof(Name), Name },
			{ nameof(RelativePath), RelativePath },
			{ nameof(IsCompressed), IsCompressed },
			{ nameof(EncryptionKey), EncryptionKey },
		};

	public void TrimStringValuesToFitDatabaseMaxLengths(string? postfix = null)
	{
		MimeType = Legion.Text.StringHelper.TrimToFitMaxLength(MimeType, 1023, postfix);
		ContentEncoding = Legion.Text.StringHelper.TrimToFitMaxLength(ContentEncoding, 63, postfix);
		Name = Legion.Text.StringHelper.TrimToFitMaxLength(Name, 511, postfix);
		RelativePath = Legion.Text.StringHelper.TrimToFitMaxLength(RelativePath, 1023, postfix);
	}

	public override string? GetPrimaryKeyValue()
	{
		return IdApplicationEntryRequest.ToString();
	}

	public override string? ToString()
	{
		return IdApplicationEntryRequest.ToString();
	}

	public static ValidatorBuilder<ApplicationEntryRequest> SetDBValidatorRules(ValidatorBuilder<ApplicationEntryRequest> builder)
		=> builder
			.ForProperty(x => x.IdApplicationEntryRequest, v => v.NotDefaultOrEmpty())
			.ForProperty(x => x.IdApplicationEntry, v => v.NotDefaultOrEmpty(), (x, parent) => x.ApplicationEntry == null)
			//.ForProperty(x => x.CreatedUtc, v => v.NotDefaultOrEmpty())
			.ForProperty(x => x.MimeType, v => v.NotDefaultOrEmpty().MaxLength(1023))
			.ForProperty(x => x.ContentEncoding, v => v.MaxLength(63))
			.ForProperty(x => x.Name, v => v.MaxLength(511))
			.ForProperty(x => x.RelativePath, v => v.MaxLength(1023))
		;
}
