using Legion.Validation;

namespace Legion.ADF.ESB.Components.Model;

public sealed partial class AdapterRequestPayload : Components.ComponentsBaseEntity, Legion.Model.IEntity
{
	public static IValidator<AdapterRequestPayload> DefaultDBValidator { get; }

	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid IdAdapterRequestPayload { get; private set; }

	/// <summary>
	/// Database DataType: uuid NOT NULL | Components.Model.AdapterRequest.AdapterRequest | FK_AdapterRequestPayload_IdAdapterRequest
	/// </summary>
	public Guid IdAdapterRequest { get; private set; }

	/// <summary>
	/// Database DataType: varchar(63) NOT NULL
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
	/// _1:N Guid IdAdapterRequest | FK_AdapterRequestPayload_IdAdapterRequest
	/// </summary>
	public Components.Model.AdapterRequest AdapterRequest { get; private set; }

	private AdapterRequestPayload()
	{
	}

	static AdapterRequestPayload()
	{
		DefaultDBValidator = SetDBValidatorRules(new ValidatorBuilder<AdapterRequestPayload>()).Build();
	}

	public override string? ToString()
	{
		return IdAdapterRequestPayload.ToString();
	}

	public static ValidatorBuilder<AdapterRequestPayload> SetDBValidatorRules(ValidatorBuilder<AdapterRequestPayload> builder)
		=> builder
			.ForProperty(x => x.IdAdapterRequestPayload, v => v.NotDefaultOrEmpty())
			.ForProperty(x => x.IdAdapterRequest, v => v.NotDefaultOrEmpty(), x => x.AdapterRequest == null)
			.ForProperty(x => x.RequestContentType, v => v.NotDefaultOrEmpty().MaxLength(63))
			.ForProperty(x => x.Name, v => v.MaxLength(511))
			.ForProperty(x => x.RelativePath, v => v.MaxLength(1023))
			.ForProperty(x => x.ContentEncoding, v => v.MaxLength(63))
			.ForProperty(x => x.MediaType, v => v.MaxLength(255))
			.ForProperty(x => x.MultipartFormDataContentName, v => v.MaxLength(511))
			.ForProperty(x => x.MultipartFormDataFileName, v => v.MaxLength(511))
			.ForProperty(x => x.JsonInputCSharpType, v => v.MaxLength(1023))
		;
}
