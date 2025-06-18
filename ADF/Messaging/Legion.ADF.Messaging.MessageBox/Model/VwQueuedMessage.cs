namespace Legion.ADF.Messaging.MessageBox.Model;

public sealed partial class VwQueuedMessage : MessageBox.MessageBoxBaseQueryEntity, Legion.Model.IQueryEntity
{
	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid IdQueuedMessage { get; private set; }

	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid IdQueue { get; private set; }

	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid IdMessage { get; private set; }

	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid IdMessageProcessingStatus { get; private set; }

	/// <summary>
	/// Database DataType: varchar(63) NOT NULL
	/// </summary>
	public string MessageProcessingStatusCode { get; private set; }

	/// <summary>
	/// Database DataType: varchar(127) NOT NULL
	/// </summary>
	public string MessageProcessingStatusName { get; private set; }

	/// <summary>
	/// Database DataType: timestamp with time zone NOT NULL
	/// </summary>
	public DateTime AssignedUtc { get; private set; }

	/// <summary>
	/// Database DataType: timestamp with time zone NULL
	/// </summary>
	public DateTime? ProcessedUtc { get; private set; }

	/// <summary>
	/// Database DataType: timestamp with time zone NULL
	/// </summary>
	public DateTime? SuspendedUtc { get; private set; }

	/// <summary>
	/// Database DataType: timestamp with time zone NULL
	/// </summary>
	public DateTime? LastProcessingUtc { get; private set; }

	/// <summary>
	/// Database DataType: timestamp with time zone NULL
	/// </summary>
	public DateTime? LastProcessingTimeoutUtc { get; private set; }

	/// <summary>
	/// Database DataType: timestamp with time zone NOT NULL
	/// </summary>
	public DateTime NextProcessingUtc { get; private set; }

	/// <summary>
	/// Database DataType: integer NOT NULL
	/// </summary>
	public int RetryCount { get; private set; }

	/// <summary>
	/// Database DataType: uuid NULL
	/// </summary>
	public Guid? IdJob { get; private set; }

	/// <summary>
	/// Database DataType: uuid NULL
	/// </summary>
	public Guid? IdOrchestration { get; private set; }

	/// <summary>
	/// Database DataType: integer NOT NULL
	/// </summary>
	public int IsArchived { get; private set; }

	/// <summary>
	/// Database DataType: uuid NULL
	/// </summary>
	public Guid? IdMessageType { get; private set; }

	/// <summary>
	/// Database DataType: uuid NULL
	/// </summary>
	public Guid? IdMessageStatus { get; private set; }

	/// <summary>
	/// Database DataType: varchar(63) NULL
	/// </summary>
	public string? MessageStatusCode { get; private set; }

	/// <summary>
	/// Database DataType: varchar(127) NULL
	/// </summary>
	public string? MessageStatusName { get; private set; }

	/// <summary>
	/// Database DataType: uuid NULL
	/// </summary>
	public Guid? IdMessageContent { get; private set; }

	/// <summary>
	/// Database DataType: uuid NULL
	/// </summary>
	public Guid? IdQueueMessage { get; private set; }

	/// <summary>
	/// Database DataType: uuid NULL
	/// </summary>
	public Guid? IdTopicMessage { get; private set; }

	/// <summary>
	/// Database DataType: timestamp with time zone NULL
	/// </summary>
	public DateTime? CreatedUtc { get; private set; }

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
	/// Database DataType: uuid NULL
	/// </summary>
	public Guid? TraceCorrelationId { get; private set; }

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
	/// Database DataType: integer NULL
	/// </summary>
	public int? Priority { get; private set; }

	/// <summary>
	/// Database DataType: varchar(127) NULL
	/// </summary>
	public string? MessageTypeCode { get; private set; }

	/// <summary>
	/// Database DataType: varchar(127) NULL
	/// </summary>
	public string? MessageTypeName { get; private set; }

	/// <summary>
	/// Database DataType: varchar(1023) NULL
	/// </summary>
	public string? MessageTypeNamespace { get; private set; }


	private VwQueuedMessage()
	{
	}

	public Dictionary<string, object?> ToDictionary()
		=> new()
		{
			{ nameof(IdQueuedMessage), IdQueuedMessage },
			{ nameof(IdQueue), IdQueue },
			{ nameof(IdMessage), IdMessage },
			{ nameof(IdMessageProcessingStatus), IdMessageProcessingStatus },
			{ nameof(MessageProcessingStatusCode), MessageProcessingStatusCode },
			{ nameof(MessageProcessingStatusName), MessageProcessingStatusName },
			{ nameof(AssignedUtc), AssignedUtc },
			{ nameof(ProcessedUtc), ProcessedUtc },
			{ nameof(SuspendedUtc), SuspendedUtc },
			{ nameof(LastProcessingUtc), LastProcessingUtc },
			{ nameof(LastProcessingTimeoutUtc), LastProcessingTimeoutUtc },
			{ nameof(NextProcessingUtc), NextProcessingUtc },
			{ nameof(RetryCount), RetryCount },
			{ nameof(IdJob), IdJob },
			{ nameof(IdOrchestration), IdOrchestration },
			{ nameof(IsArchived), IsArchived },
			{ nameof(IdMessageType), IdMessageType },
			{ nameof(IdMessageStatus), IdMessageStatus },
			{ nameof(MessageStatusCode), MessageStatusCode },
			{ nameof(MessageStatusName), MessageStatusName },
			{ nameof(IdMessageContent), IdMessageContent },
			{ nameof(IdQueueMessage), IdQueueMessage },
			{ nameof(IdTopicMessage), IdTopicMessage },
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
		return IdQueuedMessage.ToString();
	}
}
