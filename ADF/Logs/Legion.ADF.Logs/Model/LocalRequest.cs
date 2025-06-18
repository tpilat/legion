using Legion.Validation;

namespace Legion.ADF.Logs.Model;

public sealed partial class LocalRequest : Logs.LogsBaseEntity, Legion.Model.IEntity
{
	private List<Logs.Model.LocalRequestPayload> _localRequestPayloads;
	private List<Logs.Model.LocalResponse> _localResponses;

	public static IValidator<LocalRequest> DefaultDBValidator { get; }

	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid IdLocalRequest { get; private set; }

	/// <summary>
	/// Database DataType: uuid NULL | Logs.Model.RemoteSystem.RemoteSystem | FK_LocalRequest_IdRemoteSystem
	/// </summary>
	public Guid? IdRemoteSystem { get; private set; }

	/// <summary>
	/// Database DataType: varchar(63) NULL
	/// </summary>
	public string? RemoteIp { get; private set; }

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
	/// Database DataType: varchar(127) NOT NULL
	/// </summary>
	public string SourceClientIdentifier { get; private set; }

	/// <summary>
	/// Database DataType: varchar(2047) NOT NULL
	/// </summary>
	public string Url { get; private set; }

	/// <summary>
	/// Database DataType: varchar(1023) NULL
	/// </summary>
	public string? Path { get; private set; }

	/// <summary>
	/// Database DataType: varchar(1023) NULL
	/// </summary>
	public string? QueryString { get; private set; }

	/// <summary>
	/// Database DataType: varchar(15) NULL
	/// </summary>
	public string? Method { get; private set; }

	/// <summary>
	/// Database DataType: jsonb NULL
	/// </summary>
	public string? Headers { get; private set; }

	/// <summary>
	/// Database DataType: varchar(255) NULL
	/// </summary>
	public string? ContentType { get; private set; }

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
	/// _1:N Guid? IdRemoteSystem | FK_LocalRequest_IdRemoteSystem
	/// </summary>
	public Logs.Model.RemoteSystem RemoteSystem { get; private set; }


	/// <summary>
	/// N:_1 Logs.Model.LocalRequestPayload.IdLocalRequest | FK_LocalRequestPayload_IdLocalRequest
	/// </summary>
	public IReadOnlyList<Logs.Model.LocalRequestPayload> LocalRequestPayloads => _localRequestPayloads;

	/// <summary>
	/// N:_1 Logs.Model.LocalResponse.IdLocalRequest | FK_LocalResponse_IdLocalRequest
	/// </summary>
	public IReadOnlyList<Logs.Model.LocalResponse> LocalResponses => _localResponses;

	private LocalRequest()
	{
		_localRequestPayloads = [];
		_localResponses = [];
	}

	static LocalRequest()
	{
		DefaultDBValidator = SetDBValidatorRules(new ValidatorBuilder<LocalRequest>()).Build();
	}

	public Dictionary<string, object?> ToDictionary()
		=> new()
		{
			{ nameof(IdLocalRequest), IdLocalRequest },
			{ nameof(IdRemoteSystem), IdRemoteSystem },
			{ nameof(RemoteIp), RemoteIp },
			{ nameof(CreatedUtc), CreatedUtc },
			{ nameof(CorrelationId), CorrelationId },
			{ nameof(ExternalCorrelationId), ExternalCorrelationId },
			{ nameof(SourceClientIdentifier), SourceClientIdentifier },
			{ nameof(Url), Url },
			{ nameof(Path), Path },
			{ nameof(QueryString), QueryString },
			{ nameof(Method), Method },
			{ nameof(Headers), Headers },
			{ nameof(ContentType), ContentType },
			{ nameof(Metadata), Metadata },
			{ nameof(CustomCorrelationId), CustomCorrelationId },
			{ nameof(RuntimeUniqueKey), RuntimeUniqueKey },
		};

	public void TrimStringValuesToFitDatabaseMaxLengths(string? postfix = null)
	{
		RemoteIp = Legion.Text.StringHelper.TrimToFitMaxLength(RemoteIp, 63, postfix);
		ExternalCorrelationId = Legion.Text.StringHelper.TrimToFitMaxLength(ExternalCorrelationId, 127, postfix);
		SourceClientIdentifier = Legion.Text.StringHelper.TrimToFitMaxLength(SourceClientIdentifier, 127, postfix);
		Url = Legion.Text.StringHelper.TrimToFitMaxLength(Url, 2047, postfix);
		Path = Legion.Text.StringHelper.TrimToFitMaxLength(Path, 1023, postfix);
		QueryString = Legion.Text.StringHelper.TrimToFitMaxLength(QueryString, 1023, postfix);
		Method = Legion.Text.StringHelper.TrimToFitMaxLength(Method, 15, postfix);
		ContentType = Legion.Text.StringHelper.TrimToFitMaxLength(ContentType, 255, postfix);
		CustomCorrelationId = Legion.Text.StringHelper.TrimToFitMaxLength(CustomCorrelationId, 511, postfix);
	}

	public override string? GetPrimaryKeyValue()
	{
		return IdLocalRequest.ToString();
	}

	public override string? ToString()
	{
		return IdLocalRequest.ToString();
	}

	public static ValidatorBuilder<LocalRequest> SetDBValidatorRules(ValidatorBuilder<LocalRequest> builder)
		=> builder
			.ForProperty(x => x.IdLocalRequest, v => v.NotDefaultOrEmpty())
			.ForProperty(x => x.RemoteIp, v => v.MaxLength(63))
			//.ForProperty(x => x.CreatedUtc, v => v.NotDefaultOrEmpty())
			//.ForProperty(x => x.CorrelationId, v => v.NotDefaultOrEmpty())
			.ForProperty(x => x.ExternalCorrelationId, v => v.MaxLength(127))
			.ForProperty(x => x.SourceClientIdentifier, v => v.NotDefaultOrEmpty().MaxLength(127))
			.ForProperty(x => x.Url, v => v.NotDefaultOrEmpty().MaxLength(2047))
			.ForProperty(x => x.Path, v => v.MaxLength(1023))
			.ForProperty(x => x.QueryString, v => v.MaxLength(1023))
			.ForProperty(x => x.Method, v => v.MaxLength(15))
			.ForProperty(x => x.ContentType, v => v.MaxLength(255))
			.ForProperty(x => x.CustomCorrelationId, v => v.MaxLength(511))
			//.ForProperty(x => x.RuntimeUniqueKey, v => v.NotDefaultOrEmpty())
		;
}
