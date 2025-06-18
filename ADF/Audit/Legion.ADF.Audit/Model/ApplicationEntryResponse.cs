using Legion.Validation;

namespace Legion.ADF.Audit.Model;

public sealed partial class ApplicationEntryResponse : Audit.AuditBaseEntity, Legion.Model.IEntity
{
	public static IValidator<ApplicationEntryResponse> DefaultDBValidator { get; }

	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid IdApplicationEntryResponse { get; private set; }

	/// <summary>
	/// Database DataType: uuid NOT NULL | Audit.Model.ApplicationEntry.ApplicationEntry | FK_ApplicationEntryResponse_IdApplicationEntry
	/// </summary>
	public Guid IdApplicationEntry { get; private set; }

	/// <summary>
	/// Database DataType: timestamp with time zone NOT NULL
	/// </summary>
	public DateTime CreatedUtc { get; private set; }

	/// <summary>
	/// Database DataType: numeric NOT NULL
	/// </summary>
	public decimal ElapsedMilliseconds { get; private set; }

	/// <summary>
	/// Database DataType: varchar(63) NULL
	/// </summary>
	public string? StatusCode { get; private set; }

	/// <summary>
	/// Database DataType: jsonb NULL
	/// </summary>
	public string? Metadata { get; private set; }

	/// <summary>
	/// Database DataType: text NULL
	/// </summary>
	public string? Error { get; private set; }

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
	/// _1:N Guid IdApplicationEntry | FK_ApplicationEntryResponse_IdApplicationEntry
	/// </summary>
	public Audit.Model.ApplicationEntry ApplicationEntry { get; private set; }

	private ApplicationEntryResponse()
	{
	}

	static ApplicationEntryResponse()
	{
		DefaultDBValidator = SetDBValidatorRules(new ValidatorBuilder<ApplicationEntryResponse>()).Build();
	}

	public Dictionary<string, object?> ToDictionary()
		=> new()
		{
			{ nameof(IdApplicationEntryResponse), IdApplicationEntryResponse },
			{ nameof(IdApplicationEntry), IdApplicationEntry },
			{ nameof(CreatedUtc), CreatedUtc },
			{ nameof(ElapsedMilliseconds), ElapsedMilliseconds },
			{ nameof(StatusCode), StatusCode },
			{ nameof(Metadata), Metadata },
			{ nameof(Error), Error },
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
		StatusCode = Legion.Text.StringHelper.TrimToFitMaxLength(StatusCode, 63, postfix);
		MimeType = Legion.Text.StringHelper.TrimToFitMaxLength(MimeType, 1023, postfix);
		ContentEncoding = Legion.Text.StringHelper.TrimToFitMaxLength(ContentEncoding, 63, postfix);
		Name = Legion.Text.StringHelper.TrimToFitMaxLength(Name, 511, postfix);
		RelativePath = Legion.Text.StringHelper.TrimToFitMaxLength(RelativePath, 1023, postfix);
	}

	public override string? GetPrimaryKeyValue()
	{
		return IdApplicationEntryResponse.ToString();
	}

	public override string? ToString()
	{
		return IdApplicationEntryResponse.ToString();
	}

	public static ValidatorBuilder<ApplicationEntryResponse> SetDBValidatorRules(ValidatorBuilder<ApplicationEntryResponse> builder)
		=> builder
			.ForProperty(x => x.IdApplicationEntryResponse, v => v.NotDefaultOrEmpty())
			.ForProperty(x => x.IdApplicationEntry, v => v.NotDefaultOrEmpty(), (x, parent) => x.ApplicationEntry == null)
			//.ForProperty(x => x.CreatedUtc, v => v.NotDefaultOrEmpty())
			//.ForProperty(x => x.ElapsedMilliseconds, v => v.NotDefaultOrEmpty())
			.ForProperty(x => x.StatusCode, v => v.MaxLength(63))
			.ForProperty(x => x.MimeType, v => v.NotDefaultOrEmpty().MaxLength(1023))
			.ForProperty(x => x.ContentEncoding, v => v.MaxLength(63))
			.ForProperty(x => x.Name, v => v.MaxLength(511))
			.ForProperty(x => x.RelativePath, v => v.MaxLength(1023))
		;
}
