using Legion.Validation;

namespace Legion.ADF.Audit.Model;

public sealed partial class ApplicationEntry : Audit.AuditBaseEntity, Legion.Model.IEntity
{
	private List<Audit.Model.ApplicationEntryRequest> _applicationEntryRequests;
	private List<Audit.Model.ApplicationEntryResponse> _applicationEntryResponses;

	public static IValidator<ApplicationEntry> DefaultDBValidator { get; }

	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid IdApplicationEntry { get; private set; }

	/// <summary>
	/// Database DataType: uuid NOT NULL | Audit.Model.ApplicationEntryToken.ApplicationEntryToken | FK_ApplicationEntry_IdApplicationEntryToken
	/// </summary>
	public Guid IdApplicationEntryToken { get; private set; }

	/// <summary>
	/// Database DataType: uuid NOT NULL | Audit.Model.AuditOperation.AuditOperation | FK_ApplicationEntry_IdAuditOperation
	/// </summary>
	public Guid IdAuditOperation { get; private set; }

	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid RuntimeUniqueKey { get; private set; }

	/// <summary>
	/// Database DataType: timestamp with time zone NOT NULL
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
	public string? AggregateIdentifier { get; private set; }

	/// <summary>
	/// Database DataType: varchar(15) NULL
	/// </summary>
	public string? HttpMethod { get; private set; }

	/// <summary>
	/// Database DataType: varchar(1023) NULL
	/// </summary>
	public string? Uri { get; private set; }

	/// <summary>
	/// Database DataType: uuid NULL
	/// </summary>
	public Guid? IdUser { get; private set; }

	/// <summary>
	/// Database DataType: uuid NULL
	/// </summary>
	public Guid? TenantIdentifier { get; private set; }

	/// <summary>
	/// Database DataType: varchar(63) NULL
	/// </summary>
	public string? RemoteIP { get; private set; }


	/// <summary>
	/// _1:N Guid IdApplicationEntryToken | FK_ApplicationEntry_IdApplicationEntryToken
	/// </summary>
	public Audit.Model.ApplicationEntryToken ApplicationEntryToken { get; private set; }

	/// <summary>
	/// _1:N Guid IdAuditOperation | FK_ApplicationEntry_IdAuditOperation
	/// </summary>
	public Audit.Model.AuditOperation AuditOperation { get; private set; }


	/// <summary>
	/// N:_1 Audit.Model.ApplicationEntryRequest.IdApplicationEntry | FK_ApplicationEntryRequest_IdApplicationEntry
	/// </summary>
	public IReadOnlyList<Audit.Model.ApplicationEntryRequest> ApplicationEntryRequests => _applicationEntryRequests;

	/// <summary>
	/// N:_1 Audit.Model.ApplicationEntryResponse.IdApplicationEntry | FK_ApplicationEntryResponse_IdApplicationEntry
	/// </summary>
	public IReadOnlyList<Audit.Model.ApplicationEntryResponse> ApplicationEntryResponses => _applicationEntryResponses;

	private ApplicationEntry()
	{
		_applicationEntryRequests = [];
		_applicationEntryResponses = [];
	}

	static ApplicationEntry()
	{
		DefaultDBValidator = SetDBValidatorRules(new ValidatorBuilder<ApplicationEntry>()).Build();
	}

	public Dictionary<string, object?> ToDictionary()
		=> new()
		{
			{ nameof(IdApplicationEntry), IdApplicationEntry },
			{ nameof(IdApplicationEntryToken), IdApplicationEntryToken },
			{ nameof(IdAuditOperation), IdAuditOperation },
			{ nameof(RuntimeUniqueKey), RuntimeUniqueKey },
			{ nameof(CreatedUtc), CreatedUtc },
			{ nameof(CorrelationId), CorrelationId },
			{ nameof(ExternalCorrelationId), ExternalCorrelationId },
			{ nameof(AggregateIdentifier), AggregateIdentifier },
			{ nameof(HttpMethod), HttpMethod },
			{ nameof(Uri), Uri },
			{ nameof(IdUser), IdUser },
			{ nameof(TenantIdentifier), TenantIdentifier },
			{ nameof(RemoteIP), RemoteIP },
		};

	public void TrimStringValuesToFitDatabaseMaxLengths(string? postfix = null)
	{
		ExternalCorrelationId = Legion.Text.StringHelper.TrimToFitMaxLength(ExternalCorrelationId, 127, postfix);
		AggregateIdentifier = Legion.Text.StringHelper.TrimToFitMaxLength(AggregateIdentifier, 511, postfix);
		HttpMethod = Legion.Text.StringHelper.TrimToFitMaxLength(HttpMethod, 15, postfix);
		Uri = Legion.Text.StringHelper.TrimToFitMaxLength(Uri, 1023, postfix);
		RemoteIP = Legion.Text.StringHelper.TrimToFitMaxLength(RemoteIP, 63, postfix);
	}

	public override string? GetPrimaryKeyValue()
	{
		return IdApplicationEntry.ToString();
	}

	public override string? ToString()
	{
		return IdApplicationEntry.ToString();
	}

	public static ValidatorBuilder<ApplicationEntry> SetDBValidatorRules(ValidatorBuilder<ApplicationEntry> builder)
		=> builder
			.ForProperty(x => x.IdApplicationEntry, v => v.NotDefaultOrEmpty())
			.ForProperty(x => x.IdApplicationEntryToken, v => v.NotDefaultOrEmpty(), (x, parent) => x.ApplicationEntryToken == null)
			.ForProperty(x => x.IdAuditOperation, v => v.NotDefaultOrEmpty(), (x, parent) => x.AuditOperation == null)
			//.ForProperty(x => x.RuntimeUniqueKey, v => v.NotDefaultOrEmpty())
			//.ForProperty(x => x.CreatedUtc, v => v.NotDefaultOrEmpty())
			.ForProperty(x => x.ExternalCorrelationId, v => v.MaxLength(127))
			.ForProperty(x => x.AggregateIdentifier, v => v.MaxLength(511))
			.ForProperty(x => x.HttpMethod, v => v.MaxLength(15))
			.ForProperty(x => x.Uri, v => v.MaxLength(1023))
			.ForProperty(x => x.RemoteIP, v => v.MaxLength(63))
		;
}
