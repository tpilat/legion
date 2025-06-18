using Legion.Validation;

namespace Legion.ADF.Logs.Model;

public sealed partial class RemoteRequest : Logs.LogsBaseEntity, Legion.Model.IEntity
{
	private List<Logs.Model.RemoteRequestPayload> _remoteRequestPayloads;
	private List<Logs.Model.RemoteResponse> _remoteResponses;

	public static IValidator<RemoteRequest> DefaultDBValidator { get; }

	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid IdRemoteRequest { get; private set; }

	/// <summary>
	/// Database DataType: uuid NOT NULL | Logs.Model.RemoteSystem.RemoteSystem | FK_RemoteRequest_IdRemoteSystem
	/// </summary>
	public Guid IdRemoteSystem { get; private set; }

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
	/// _1:N Guid IdRemoteSystem | FK_RemoteRequest_IdRemoteSystem
	/// </summary>
	public Logs.Model.RemoteSystem RemoteSystem { get; private set; }


	/// <summary>
	/// N:_1 Logs.Model.RemoteRequestPayload.IdRemoteRequest | FK_RemoteRequestPayload_IdRemoteRequest
	/// </summary>
	public IReadOnlyList<Logs.Model.RemoteRequestPayload> RemoteRequestPayloads => _remoteRequestPayloads;

	/// <summary>
	/// N:_1 Logs.Model.RemoteResponse.IdRemoteRequest | FK_RemoteResponse_IdRemoteRequest
	/// </summary>
	public IReadOnlyList<Logs.Model.RemoteResponse> RemoteResponses => _remoteResponses;

	private RemoteRequest()
	{
		_remoteRequestPayloads = [];
		_remoteResponses = [];
	}

	static RemoteRequest()
	{
		DefaultDBValidator = SetDBValidatorRules(new ValidatorBuilder<RemoteRequest>()).Build();
	}

	public Dictionary<string, object?> ToDictionary()
		=> new()
		{
			{ nameof(IdRemoteRequest), IdRemoteRequest },
			{ nameof(IdRemoteSystem), IdRemoteSystem },
			{ nameof(CreatedUtc), CreatedUtc },
			{ nameof(CorrelationId), CorrelationId },
			{ nameof(ExternalCorrelationId), ExternalCorrelationId },
			{ nameof(SourceClientIdentifier), SourceClientIdentifier },
			{ nameof(Url), Url },
			{ nameof(Method), Method },
			{ nameof(Headers), Headers },
			{ nameof(ContentType), ContentType },
			{ nameof(Metadata), Metadata },
			{ nameof(CustomCorrelationId), CustomCorrelationId },
			{ nameof(RuntimeUniqueKey), RuntimeUniqueKey },
		};

	public void TrimStringValuesToFitDatabaseMaxLengths(string? postfix = null)
	{
		ExternalCorrelationId = Legion.Text.StringHelper.TrimToFitMaxLength(ExternalCorrelationId, 127, postfix);
		SourceClientIdentifier = Legion.Text.StringHelper.TrimToFitMaxLength(SourceClientIdentifier, 127, postfix);
		Url = Legion.Text.StringHelper.TrimToFitMaxLength(Url, 2047, postfix);
		Method = Legion.Text.StringHelper.TrimToFitMaxLength(Method, 15, postfix);
		ContentType = Legion.Text.StringHelper.TrimToFitMaxLength(ContentType, 255, postfix);
		CustomCorrelationId = Legion.Text.StringHelper.TrimToFitMaxLength(CustomCorrelationId, 511, postfix);
	}

	public override string? GetPrimaryKeyValue()
	{
		return IdRemoteRequest.ToString();
	}

	public override string? ToString()
	{
		return IdRemoteRequest.ToString();
	}

	public static ValidatorBuilder<RemoteRequest> SetDBValidatorRules(ValidatorBuilder<RemoteRequest> builder)
		=> builder
			.ForProperty(x => x.IdRemoteRequest, v => v.NotDefaultOrEmpty())
			.ForProperty(x => x.IdRemoteSystem, v => v.NotDefaultOrEmpty(), (x, parent) => x.RemoteSystem == null)
			//.ForProperty(x => x.CreatedUtc, v => v.NotDefaultOrEmpty())
			//.ForProperty(x => x.CorrelationId, v => v.NotDefaultOrEmpty())
			.ForProperty(x => x.ExternalCorrelationId, v => v.MaxLength(127))
			.ForProperty(x => x.SourceClientIdentifier, v => v.NotDefaultOrEmpty().MaxLength(127))
			.ForProperty(x => x.Url, v => v.NotDefaultOrEmpty().MaxLength(2047))
			.ForProperty(x => x.Method, v => v.MaxLength(15))
			.ForProperty(x => x.ContentType, v => v.MaxLength(255))
			.ForProperty(x => x.CustomCorrelationId, v => v.MaxLength(511))
			//.ForProperty(x => x.RuntimeUniqueKey, v => v.NotDefaultOrEmpty())
		;
}
