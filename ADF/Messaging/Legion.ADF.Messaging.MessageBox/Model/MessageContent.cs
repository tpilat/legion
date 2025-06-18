using Legion.Validation;

namespace Legion.ADF.Messaging.MessageBox.Model;

public sealed partial class MessageContent : MessageBox.MessageBoxBaseEntity, Legion.Model.IEntity
{
	public static IValidator<MessageContent> DefaultDBValidator { get; }

	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid IdMessageContent { get; private set; }

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
	/// 1:_1 Message.IdMessageContent | FK_Message_IdMessageContent
	/// </summary>
	public MessageBox.Model.Message Message { get; private set; }

	/// <summary>
	/// 1:_1 MessageArchive.IdMessageContent | FK_MessageArchive_IdMessageContent
	/// </summary>
	public MessageBox.Model.MessageArchive MessageArchive { get; private set; }

	private MessageContent()
	{
	}

	static MessageContent()
	{
		DefaultDBValidator = SetDBValidatorRules(new ValidatorBuilder<MessageContent>()).Build();
	}

	public Dictionary<string, object?> ToDictionary()
		=> new()
		{
			{ nameof(IdMessageContent), IdMessageContent },
			{ nameof(MimeType), MimeType },
			{ nameof(ContentEncoding), ContentEncoding },
			{ nameof(ByteArrayContent), ByteArrayContent },
			{ nameof(JsonContent), JsonContent },
			{ nameof(StringContent), StringContent },
			{ nameof(DbOid), DbOid },
			{ nameof(Name), Name },
			{ nameof(RelativePath), RelativePath },
			{ nameof(Metadata), Metadata },
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
		return IdMessageContent.ToString();
	}

	public override string? ToString()
	{
		return IdMessageContent.ToString();
	}

	public static ValidatorBuilder<MessageContent> SetDBValidatorRules(ValidatorBuilder<MessageContent> builder)
		=> builder
			.ForProperty(x => x.IdMessageContent, v => v.NotDefaultOrEmpty())
			.ForProperty(x => x.MimeType, v => v.NotDefaultOrEmpty().MaxLength(1023))
			.ForProperty(x => x.ContentEncoding, v => v.MaxLength(63))
			.ForProperty(x => x.Name, v => v.MaxLength(511))
			.ForProperty(x => x.RelativePath, v => v.MaxLength(1023))
		;
}
