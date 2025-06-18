namespace Legion.ADF.Messaging.Outbox.Model;

public sealed partial class VwOutboxQueue : Outbox.OutboxBaseQueryEntity, Legion.Model.IQueryEntity
{
	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid IdOutboxQueue { get; private set; }

	/// <summary>
	/// Database DataType: varchar(1023) NOT NULL
	/// </summary>
	public string Name { get; private set; }

	/// <summary>
	/// Database DataType: varchar(1023) NOT NULL
	/// </summary>
	public string ReceivedEventNamespace { get; private set; }

	/// <summary>
	/// Database DataType: uuid NULL
	/// </summary>
	public Guid? IdMessageType { get; private set; }

	/// <summary>
	/// Database DataType: boolean NOT NULL
	/// </summary>
	public bool IsActive { get; private set; }

	/// <summary>
	/// Database DataType: boolean NOT NULL
	/// </summary>
	public bool IsSequentialFIFO { get; private set; }

	/// <summary>
	/// Database DataType: integer NOT NULL
	/// </summary>
	public int MessagesBatchCount { get; private set; }

	/// <summary>
	/// Database DataType: integer NULL
	/// </summary>
	public int? MaxDegreeOfParallelism { get; private set; }

	/// <summary>
	/// Database DataType: interval NOT NULL
	/// </summary>
	public TimeSpan TimeoutForMessageProcessing { get; private set; }

	/// <summary>
	/// Database DataType: integer NOT NULL
	/// </summary>
	public int MaxMessageProcessingRetryCount { get; private set; }

	/// <summary>
	/// Database DataType: jsonb NULL
	/// </summary>
	public string? Properties { get; private set; }

	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid IdProcessingMode { get; private set; }

	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid IdOutboxInstance { get; private set; }

	/// <summary>
	/// Database DataType: varchar(63) NOT NULL
	/// </summary>
	public string ProcessingModeCode { get; private set; }

	/// <summary>
	/// Database DataType: varchar(127) NOT NULL
	/// </summary>
	public string ProcessingModeName { get; private set; }

	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid IdSuspendingMode { get; private set; }

	/// <summary>
	/// Database DataType: varchar(63) NOT NULL
	/// </summary>
	public string SuspendingModeCode { get; private set; }

	/// <summary>
	/// Database DataType: varchar(127) NOT NULL
	/// </summary>
	public string SuspendingModeName { get; private set; }

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


	private VwOutboxQueue()
	{
	}

	public Dictionary<string, object?> ToDictionary()
		=> new()
		{
			{ nameof(IdOutboxQueue), IdOutboxQueue },
			{ nameof(Name), Name },
			{ nameof(ReceivedEventNamespace), ReceivedEventNamespace },
			{ nameof(IdMessageType), IdMessageType },
			{ nameof(IsActive), IsActive },
			{ nameof(IsSequentialFIFO), IsSequentialFIFO },
			{ nameof(MessagesBatchCount), MessagesBatchCount },
			{ nameof(MaxDegreeOfParallelism), MaxDegreeOfParallelism },
			{ nameof(TimeoutForMessageProcessing), TimeoutForMessageProcessing },
			{ nameof(MaxMessageProcessingRetryCount), MaxMessageProcessingRetryCount },
			{ nameof(Properties), Properties },
			{ nameof(IdProcessingMode), IdProcessingMode },
			{ nameof(IdOutboxInstance), IdOutboxInstance },
			{ nameof(ProcessingModeCode), ProcessingModeCode },
			{ nameof(ProcessingModeName), ProcessingModeName },
			{ nameof(IdSuspendingMode), IdSuspendingMode },
			{ nameof(SuspendingModeCode), SuspendingModeCode },
			{ nameof(SuspendingModeName), SuspendingModeName },
			{ nameof(MessageTypeCode), MessageTypeCode },
			{ nameof(MessageTypeName), MessageTypeName },
			{ nameof(MessageTypeNamespace), MessageTypeNamespace },
		};

	public override string? ToString()
	{
		return IdOutboxQueue.ToString();
	}
}
