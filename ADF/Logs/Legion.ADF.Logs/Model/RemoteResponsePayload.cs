using Legion.Validation;

namespace Legion.ADF.Logs.Model;

public sealed partial class RemoteResponsePayload : Logs.LogsBaseEntity, Legion.Model.IEntity
{
	public static IValidator<RemoteResponsePayload> DefaultDBValidator { get; }

	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid IdRemoteResponsePayload { get; private set; }

	/// <summary>
	/// Database DataType: uuid NOT NULL | Logs.Model.RemoteResponse.RemoteResponse | FK_RemoteResponsePayload_IdRemoteResponse
	/// </summary>
	public Guid IdRemoteResponse { get; private set; }

	/// <summary>
	/// Database DataType: timestamp with time zone NULL
	/// </summary>
	public DateTime? CreatedUtc { get; private set; }

	/// <summary>
	/// Database DataType: varchar(63) NOT NULL
	/// </summary>
	public string ResponseContentType { get; private set; }

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
	/// Database DataType: boolean NULL
	/// </summary>
	public bool? IsCompressed { get; private set; }

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
	/// _1:N Guid IdRemoteResponse | FK_RemoteResponsePayload_IdRemoteResponse
	/// </summary>
	public Logs.Model.RemoteResponse RemoteResponse { get; private set; }

	private RemoteResponsePayload()
	{
	}

	static RemoteResponsePayload()
	{
		DefaultDBValidator = SetDBValidatorRules(new ValidatorBuilder<RemoteResponsePayload>()).Build();
	}

	public Dictionary<string, object?> ToDictionary()
		=> new()
		{
			{ nameof(IdRemoteResponsePayload), IdRemoteResponsePayload },
			{ nameof(IdRemoteResponse), IdRemoteResponse },
			{ nameof(CreatedUtc), CreatedUtc },
			{ nameof(ResponseContentType), ResponseContentType },
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
		ResponseContentType = Legion.Text.StringHelper.TrimToFitMaxLength(ResponseContentType, 63, postfix);
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
		return IdRemoteResponsePayload.ToString();
	}

	public override string? ToString()
	{
		return IdRemoteResponsePayload.ToString();
	}

	public static ValidatorBuilder<RemoteResponsePayload> SetDBValidatorRules(ValidatorBuilder<RemoteResponsePayload> builder)
		=> builder
			.ForProperty(x => x.IdRemoteResponsePayload, v => v.NotDefaultOrEmpty())
			.ForProperty(x => x.IdRemoteResponse, v => v.NotDefaultOrEmpty(), (x, parent) => x.RemoteResponse == null)
			.ForProperty(x => x.ResponseContentType, v => v.NotDefaultOrEmpty().MaxLength(63))
			.ForProperty(x => x.FileName, v => v.MaxLength(511))
			.ForProperty(x => x.RelativePath, v => v.MaxLength(1023))
			.ForProperty(x => x.ContentEncoding, v => v.MaxLength(63))
			.ForProperty(x => x.MediaType, v => v.MaxLength(255))
			.ForProperty(x => x.MultipartFormDataContentName, v => v.MaxLength(511))
			.ForProperty(x => x.MultipartFormDataFileName, v => v.MaxLength(511))
			.ForProperty(x => x.JsonInputCSharpType, v => v.MaxLength(1023))
		;
}
