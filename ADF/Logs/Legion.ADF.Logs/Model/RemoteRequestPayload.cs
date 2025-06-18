using Legion.Validation;

namespace Legion.ADF.Logs.Model;

public sealed partial class RemoteRequestPayload : Logs.LogsBaseEntity, Legion.Model.IEntity
{
	public static IValidator<RemoteRequestPayload> DefaultDBValidator { get; }

	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid IdRemoteRequestPayload { get; private set; }

	/// <summary>
	/// Database DataType: uuid NOT NULL | Logs.Model.RemoteRequest.RemoteRequest | FK_RemoteRequestPayload_IdRemoteRequest
	/// </summary>
	public Guid IdRemoteRequest { get; private set; }

	/// <summary>
	/// Database DataType: timestamp with time zone NULL
	/// </summary>
	public DateTime? CreatedUtc { get; private set; }

	/// <summary>
	/// Database DataType: varchar(127) NOT NULL
	/// </summary>
	public string RequestContentType { get; private set; }

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
	/// Database DataType: jsonb NULL
	/// </summary>
	public string? ContentHeaders { get; private set; }

	/// <summary>
	/// Database DataType: bigint NULL
	/// </summary>
	public long? DbOid { get; private set; }

	/// <summary>
	/// Database DataType: varchar(511) NULL
	/// </summary>
	public string? FileName { get; private set; }

	/// <summary>
	/// Database DataType: varchar(1023) NULL
	/// </summary>
	public string? RelativePath { get; private set; }

	/// <summary>
	/// Database DataType: jsonb NULL
	/// </summary>
	public string? Metadata { get; private set; }

	/// <summary>
	/// Database DataType: boolean NOT NULL
	/// </summary>
	public bool IsCompressed { get; private set; }

	/// <summary>
	/// Database DataType: text NULL
	/// </summary>
	public string? EncryptionKey { get; private set; }

	/// <summary>
	/// Database DataType: varchar(63) NULL
	/// </summary>
	public string? ContentEncoding { get; private set; }

	/// <summary>
	/// Database DataType: varchar(255) NULL
	/// </summary>
	public string? MediaType { get; private set; }

	/// <summary>
	/// Database DataType: varchar(511) NULL
	/// </summary>
	public string? MultipartFormDataContentName { get; private set; }

	/// <summary>
	/// Database DataType: varchar(511) NULL
	/// </summary>
	public string? MultipartFormDataFileName { get; private set; }

	/// <summary>
	/// Database DataType: varchar(1023) NULL
	/// </summary>
	public string? JsonInputCSharpType { get; private set; }


	/// <summary>
	/// _1:N Guid IdRemoteRequest | FK_RemoteRequestPayload_IdRemoteRequest
	/// </summary>
	public Logs.Model.RemoteRequest RemoteRequest { get; private set; }

	private RemoteRequestPayload()
	{
	}

	static RemoteRequestPayload()
	{
		DefaultDBValidator = SetDBValidatorRules(new ValidatorBuilder<RemoteRequestPayload>()).Build();
	}

	public Dictionary<string, object?> ToDictionary()
		=> new()
		{
			{ nameof(IdRemoteRequestPayload), IdRemoteRequestPayload },
			{ nameof(IdRemoteRequest), IdRemoteRequest },
			{ nameof(CreatedUtc), CreatedUtc },
			{ nameof(RequestContentType), RequestContentType },
			{ nameof(ByteArrayContent), ByteArrayContent },
			{ nameof(JsonContent), JsonContent },
			{ nameof(StringContent), StringContent },
			{ nameof(ContentHeaders), ContentHeaders },
			{ nameof(DbOid), DbOid },
			{ nameof(FileName), FileName },
			{ nameof(RelativePath), RelativePath },
			{ nameof(Metadata), Metadata },
			{ nameof(IsCompressed), IsCompressed },
			{ nameof(EncryptionKey), EncryptionKey },
			{ nameof(ContentEncoding), ContentEncoding },
			{ nameof(MediaType), MediaType },
			{ nameof(MultipartFormDataContentName), MultipartFormDataContentName },
			{ nameof(MultipartFormDataFileName), MultipartFormDataFileName },
			{ nameof(JsonInputCSharpType), JsonInputCSharpType },
		};

	public void TrimStringValuesToFitDatabaseMaxLengths(string? postfix = null)
	{
		RequestContentType = Legion.Text.StringHelper.TrimToFitMaxLength(RequestContentType, 127, postfix);
		FileName = Legion.Text.StringHelper.TrimToFitMaxLength(FileName, 511, postfix);
		RelativePath = Legion.Text.StringHelper.TrimToFitMaxLength(RelativePath, 1023, postfix);
		ContentEncoding = Legion.Text.StringHelper.TrimToFitMaxLength(ContentEncoding, 63, postfix);
		MediaType = Legion.Text.StringHelper.TrimToFitMaxLength(MediaType, 255, postfix);
		MultipartFormDataContentName = Legion.Text.StringHelper.TrimToFitMaxLength(MultipartFormDataContentName, 511, postfix);
		MultipartFormDataFileName = Legion.Text.StringHelper.TrimToFitMaxLength(MultipartFormDataFileName, 511, postfix);
		JsonInputCSharpType = Legion.Text.StringHelper.TrimToFitMaxLength(JsonInputCSharpType, 1023, postfix);
	}

	public override string? GetPrimaryKeyValue()
	{
		return IdRemoteRequestPayload.ToString();
	}

	public override string? ToString()
	{
		return IdRemoteRequestPayload.ToString();
	}

	public static ValidatorBuilder<RemoteRequestPayload> SetDBValidatorRules(ValidatorBuilder<RemoteRequestPayload> builder)
		=> builder
			.ForProperty(x => x.IdRemoteRequestPayload, v => v.NotDefaultOrEmpty())
			.ForProperty(x => x.IdRemoteRequest, v => v.NotDefaultOrEmpty(), (x, parent) => x.RemoteRequest == null)
			.ForProperty(x => x.RequestContentType, v => v.NotDefaultOrEmpty().MaxLength(127))
			.ForProperty(x => x.FileName, v => v.MaxLength(511))
			.ForProperty(x => x.RelativePath, v => v.MaxLength(1023))
			.ForProperty(x => x.ContentEncoding, v => v.MaxLength(63))
			.ForProperty(x => x.MediaType, v => v.MaxLength(255))
			.ForProperty(x => x.MultipartFormDataContentName, v => v.MaxLength(511))
			.ForProperty(x => x.MultipartFormDataFileName, v => v.MaxLength(511))
			.ForProperty(x => x.JsonInputCSharpType, v => v.MaxLength(1023))
		;
}
