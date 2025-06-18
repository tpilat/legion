namespace Legion.ADF.Messaging.Inbox.Model;

public sealed partial class VwInboxMessageContent : Inbox.InboxBaseQueryEntity, Legion.Model.IQueryEntity
{
	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid IdInboxMessageContent { get; private set; }

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


	private VwInboxMessageContent()
	{
	}

	public Dictionary<string, object?> ToDictionary()
		=> new()
		{
			{ nameof(IdInboxMessageContent), IdInboxMessageContent },
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

	public override string? ToString()
	{
		return IdInboxMessageContent.ToString();
	}
}
