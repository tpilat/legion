namespace Legion.ADF.Messaging.MessageBox.Model;

public sealed partial class VwQueue : MessageBox.MessageBoxBaseQueryEntity, Legion.Model.IQueryEntity
{
	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid IdQueue { get; private set; }

	/// <summary>
	/// Database DataType: varchar(1023) NOT NULL
	/// </summary>
	public string Name { get; private set; }

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

	/// <summary>
	/// Database DataType: uuid NULL
	/// </summary>
	public Guid? IdJob { get; private set; }

	/// <summary>
	/// Database DataType: uuid NULL
	/// </summary>
	public Guid? IdOrchestration { get; private set; }


	private VwQueue()
	{
	}

	public Dictionary<string, object?> ToDictionary()
		=> new()
		{
			{ nameof(IdQueue), IdQueue },
			{ nameof(Name), Name },
			{ nameof(IdMessageType), IdMessageType },
			{ nameof(IsActive), IsActive },
			{ nameof(IsSequentialFIFO), IsSequentialFIFO },
			{ nameof(MessagesBatchCount), MessagesBatchCount },
			{ nameof(MaxDegreeOfParallelism), MaxDegreeOfParallelism },
			{ nameof(TimeoutForMessageProcessing), TimeoutForMessageProcessing },
			{ nameof(MaxMessageProcessingRetryCount), MaxMessageProcessingRetryCount },
			{ nameof(Properties), Properties },
			{ nameof(IdProcessingMode), IdProcessingMode },
			{ nameof(ProcessingModeCode), ProcessingModeCode },
			{ nameof(ProcessingModeName), ProcessingModeName },
			{ nameof(IdSuspendingMode), IdSuspendingMode },
			{ nameof(SuspendingModeCode), SuspendingModeCode },
			{ nameof(SuspendingModeName), SuspendingModeName },
			{ nameof(MessageTypeCode), MessageTypeCode },
			{ nameof(MessageTypeName), MessageTypeName },
			{ nameof(MessageTypeNamespace), MessageTypeNamespace },
			{ nameof(IdJob), IdJob },
			{ nameof(IdOrchestration), IdOrchestration },
		};

	public override string? ToString()
	{
		return IdQueue.ToString();
	}
}
