using Legion.Validation;

namespace Legion.ADF.ESB.Components.Model;

public sealed partial class AdapterResponsePayload : Components.ComponentsBaseEntity, Legion.Model.IEntity
{
	public static IValidator<AdapterResponsePayload> DefaultDBValidator { get; }

	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid IdAdapterResponsePayload { get; private set; }

	/// <summary>
	/// Database DataType: uuid NOT NULL | Components.Model.AdapterResponse.AdapterResponse | FK_AdapterResponsePayload_IdAdapterResponse
	/// </summary>
	public Guid IdAdapterResponse { get; private set; }

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
	public string? Name { get; private set; }

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
	/// _1:N Guid IdAdapterResponse | FK_AdapterResponsePayload_IdAdapterResponse
	/// </summary>
	public Components.Model.AdapterResponse AdapterResponse { get; private set; }

	private AdapterResponsePayload()
	{
	}

	static AdapterResponsePayload()
	{
		DefaultDBValidator = SetDBValidatorRules(new ValidatorBuilder<AdapterResponsePayload>()).Build();
	}

	public override string? ToString()
	{
		return IdAdapterResponsePayload.ToString();
	}

	public static ValidatorBuilder<AdapterResponsePayload> SetDBValidatorRules(ValidatorBuilder<AdapterResponsePayload> builder)
		=> builder
			.ForProperty(x => x.IdAdapterResponsePayload, v => v.NotDefaultOrEmpty())
			.ForProperty(x => x.IdAdapterResponse, v => v.NotDefaultOrEmpty(), x => x.AdapterResponse == null)
			.ForProperty(x => x.ResponseContentType, v => v.NotDefaultOrEmpty().MaxLength(63))
			.ForProperty(x => x.Name, v => v.MaxLength(511))
			.ForProperty(x => x.RelativePath, v => v.MaxLength(1023))
			.ForProperty(x => x.ContentEncoding, v => v.MaxLength(63))
			.ForProperty(x => x.MediaType, v => v.MaxLength(255))
			.ForProperty(x => x.MultipartFormDataContentName, v => v.MaxLength(511))
			.ForProperty(x => x.MultipartFormDataFileName, v => v.MaxLength(511))
			.ForProperty(x => x.JsonInputCSharpType, v => v.MaxLength(1023))
		;
}
