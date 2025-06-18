using Legion.Validation;

namespace Legion.ADF.Logs.Model;

public sealed partial class LocalResponse : Logs.LogsBaseEntity, Legion.Model.IEntity
{
	private List<Logs.Model.LocalResponsePayload> _localResponsePayloads;

	public static IValidator<LocalResponse> DefaultDBValidator { get; }

	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid IdLocalResponse { get; private set; }

	/// <summary>
	/// Database DataType: uuid NOT NULL | Logs.Model.LocalRequest.LocalRequest | FK_LocalResponse_IdLocalRequest
	/// </summary>
	public Guid IdLocalRequest { get; private set; }

	/// <summary>
	/// Database DataType: timestamp with time zone NOT NULL
	/// </summary>
	public DateTime CreatedUtc { get; private set; }

	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid CorrelationId { get; private set; }

	/// <summary>
	/// Database DataType: varchar(127) NULL
	/// </summary>
	public string? ExternalCorrelationId { get; private set; }

	/// <summary>
	/// Database DataType: varchar(63) NULL
	/// </summary>
	public string? StatusCode { get; private set; }

	/// <summary>
	/// Database DataType: varchar(511) NULL
	/// </summary>
	public string? Reason { get; private set; }

	/// <summary>
	/// Database DataType: jsonb NULL
	/// </summary>
	public string? Headers { get; private set; }

	/// <summary>
	/// Database DataType: varchar(255) NULL
	/// </summary>
	public string? ContentType { get; private set; }

	/// <summary>
	/// Database DataType: text NULL
	/// </summary>
	public string? Error { get; private set; }

	/// <summary>
	/// Database DataType: numeric NULL
	/// </summary>
	public decimal? ElapsedMilliseconds { get; private set; }

	/// <summary>
	/// Database DataType: jsonb NULL
	/// </summary>
	public string? Metadata { get; private set; }

	/// <summary>
	/// Database DataType: varchar(511) NULL
	/// </summary>
	public string? CustomCorrelationId { get; private set; }

	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid RuntimeUniqueKey { get; private set; }


	/// <summary>
	/// _1:N Guid IdLocalRequest | FK_LocalResponse_IdLocalRequest
	/// </summary>
	public Logs.Model.LocalRequest LocalRequest { get; private set; }


	/// <summary>
	/// N:_1 Logs.Model.LocalResponsePayload.IdLocalResponse | FK_LocalResponsePayload_IdLocalResponse
	/// </summary>
	public IReadOnlyList<Logs.Model.LocalResponsePayload> LocalResponsePayloads => _localResponsePayloads;

	private LocalResponse()
	{
		_localResponsePayloads = [];
	}

	static LocalResponse()
	{
		DefaultDBValidator = SetDBValidatorRules(new ValidatorBuilder<LocalResponse>()).Build();
	}

	public Dictionary<string, object?> ToDictionary()
		=> new()
		{
			{ nameof(IdLocalResponse), IdLocalResponse },
			{ nameof(IdLocalRequest), IdLocalRequest },
			{ nameof(CreatedUtc), CreatedUtc },
			{ nameof(CorrelationId), CorrelationId },
			{ nameof(ExternalCorrelationId), ExternalCorrelationId },
			{ nameof(StatusCode), StatusCode },
			{ nameof(Reason), Reason },
			{ nameof(Headers), Headers },
			{ nameof(ContentType), ContentType },
			{ nameof(Error), Error },
			{ nameof(ElapsedMilliseconds), ElapsedMilliseconds },
			{ nameof(Metadata), Metadata },
			{ nameof(CustomCorrelationId), CustomCorrelationId },
			{ nameof(RuntimeUniqueKey), RuntimeUniqueKey },
		};

	public void TrimStringValuesToFitDatabaseMaxLengths(string? postfix = null)
	{
		ExternalCorrelationId = Legion.Text.StringHelper.TrimToFitMaxLength(ExternalCorrelationId, 127, postfix);
		StatusCode = Legion.Text.StringHelper.TrimToFitMaxLength(StatusCode, 63, postfix);
		Reason = Legion.Text.StringHelper.TrimToFitMaxLength(Reason, 511, postfix);
		ContentType = Legion.Text.StringHelper.TrimToFitMaxLength(ContentType, 255, postfix);
		CustomCorrelationId = Legion.Text.StringHelper.TrimToFitMaxLength(CustomCorrelationId, 511, postfix);
	}

	public override string? GetPrimaryKeyValue()
	{
		return IdLocalResponse.ToString();
	}

	public override string? ToString()
	{
		return IdLocalResponse.ToString();
	}

	public static ValidatorBuilder<LocalResponse> SetDBValidatorRules(ValidatorBuilder<LocalResponse> builder)
		=> builder
			.ForProperty(x => x.IdLocalResponse, v => v.NotDefaultOrEmpty())
			.ForProperty(x => x.IdLocalRequest, v => v.NotDefaultOrEmpty(), (x, parent) => x.LocalRequest == null)
			//.ForProperty(x => x.CreatedUtc, v => v.NotDefaultOrEmpty())
			//.ForProperty(x => x.CorrelationId, v => v.NotDefaultOrEmpty())
			.ForProperty(x => x.ExternalCorrelationId, v => v.MaxLength(127))
			.ForProperty(x => x.StatusCode, v => v.MaxLength(63))
			.ForProperty(x => x.Reason, v => v.MaxLength(511))
			.ForProperty(x => x.ContentType, v => v.MaxLength(255))
			.ForProperty(x => x.CustomCorrelationId, v => v.MaxLength(511))
			//.ForProperty(x => x.RuntimeUniqueKey, v => v.NotDefaultOrEmpty())
		;
}
