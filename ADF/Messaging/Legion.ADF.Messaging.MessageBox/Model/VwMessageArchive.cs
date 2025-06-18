namespace Legion.ADF.Messaging.MessageBox.Model;

public sealed partial class VwMessageArchive : MessageBox.MessageBoxBaseQueryEntity, Legion.Model.IQueryEntity
{
	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid IdMessage { get; private set; }

	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid IdMessageType { get; private set; }

	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid IdMessageStatus { get; private set; }

	/// <summary>
	/// Database DataType: varchar(63) NOT NULL
	/// </summary>
	public string MessageStatusCode { get; private set; }

	/// <summary>
	/// Database DataType: varchar(127) NOT NULL
	/// </summary>
	public string MessageStatusName { get; private set; }

	/// <summary>
	/// Database DataType: uuid NULL
	/// </summary>
	public Guid? IdMessageContent { get; private set; }

	/// <summary>
	/// Database DataType: uuid NULL
	/// </summary>
	public Guid? IdQueue { get; private set; }

	/// <summary>
	/// Database DataType: uuid NULL
	/// </summary>
	public Guid? IdTopic { get; private set; }

	/// <summary>
	/// Database DataType: timestamp with time zone NOT NULL
	/// </summary>
	public DateTime CreatedUtc { get; private set; }

	/// <summary>
	/// Database DataType: varchar(511) NULL
	/// </summary>
	public string? MessageId { get; private set; }

	/// <summary>
	/// Database DataType: varchar(511) NULL
	/// </summary>
	public string? BusinessId { get; private set; }

	/// <summary>
	/// Database DataType: varchar(511) NULL
	/// </summary>
	public string? CorrelationId { get; private set; }

	/// <summary>
	/// Database DataType: uuid NULL
	/// </summary>
	public Guid? SessionId { get; private set; }

	/// <summary>
	/// Database DataType: bigint NULL
	/// </summary>
	public long? SessionMessagePartId { get; private set; }

	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid TraceCorrelationId { get; private set; }

	/// <summary>
	/// Database DataType: jsonb NULL
	/// </summary>
	public string? Properties { get; private set; }

	/// <summary>
	/// Database DataType: varchar(511) NULL
	/// </summary>
	public string? Publisher { get; private set; }

	/// <summary>
	/// Database DataType: varchar(511) NULL
	/// </summary>
	public string? PublisherId { get; private set; }

	/// <summary>
	/// Database DataType: timestamp with time zone NULL
	/// </summary>
	public DateTime? ValidToUtc { get; private set; }

	/// <summary>
	/// Database DataType: integer NOT NULL
	/// </summary>
	public int Priority { get; private set; }

	/// <summary>
	/// Database DataType: varchar(127) NOT NULL
	/// </summary>
	public string MessageTypeCode { get; private set; }

	/// <summary>
	/// Database DataType: varchar(127) NOT NULL
	/// </summary>
	public string MessageTypeName { get; private set; }

	/// <summary>
	/// Database DataType: varchar(1023) NOT NULL
	/// </summary>
	public string MessageTypeNamespace { get; private set; }


	private VwMessageArchive()
	{
	}

	public Dictionary<string, object?> ToDictionary()
		=> new()
		{
			{ nameof(IdMessage), IdMessage },
			{ nameof(IdMessageType), IdMessageType },
			{ nameof(IdMessageStatus), IdMessageStatus },
			{ nameof(MessageStatusCode), MessageStatusCode },
			{ nameof(MessageStatusName), MessageStatusName },
			{ nameof(IdMessageContent), IdMessageContent },
			{ nameof(IdQueue), IdQueue },
			{ nameof(IdTopic), IdTopic },
			{ nameof(CreatedUtc), CreatedUtc },
			{ nameof(MessageId), MessageId },
			{ nameof(BusinessId), BusinessId },
			{ nameof(CorrelationId), CorrelationId },
			{ nameof(SessionId), SessionId },
			{ nameof(SessionMessagePartId), SessionMessagePartId },
			{ nameof(TraceCorrelationId), TraceCorrelationId },
			{ nameof(Properties), Properties },
			{ nameof(Publisher), Publisher },
			{ nameof(PublisherId), PublisherId },
			{ nameof(ValidToUtc), ValidToUtc },
			{ nameof(Priority), Priority },
			{ nameof(MessageTypeCode), MessageTypeCode },
			{ nameof(MessageTypeName), MessageTypeName },
			{ nameof(MessageTypeNamespace), MessageTypeNamespace },
		};

	public override string? ToString()
	{
		return IdMessage.ToString();
	}
}
