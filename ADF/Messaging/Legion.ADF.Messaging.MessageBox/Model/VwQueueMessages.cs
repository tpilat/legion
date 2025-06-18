namespace Legion.ADF.Messaging.MessageBox.Model;

public sealed partial class VwQueueMessages : MessageBox.MessageBoxBaseQueryEntity, Legion.Model.IQueryEntity
{
	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid IdQueue { get; private set; }

	/// <summary>
	/// Database DataType: varchar(1023) NOT NULL
	/// </summary>
	public string QueueName { get; private set; }

	/// <summary>
	/// Database DataType: boolean NOT NULL
	/// </summary>
	public bool IsActive { get; private set; }

	/// <summary>
	/// Database DataType: boolean NOT NULL
	/// </summary>
	public bool IsSequentialFIFO { get; private set; }

	/// <summary>
	/// Database DataType: integer NULL
	/// </summary>
	public int? MaxDegreeOfParallelism { get; private set; }

	/// <summary>
	/// Database DataType: uuid NULL
	/// </summary>
	public Guid? IdJob { get; private set; }

	/// <summary>
	/// Database DataType: uuid NULL
	/// </summary>
	public Guid? IdOrchestration { get; private set; }

	/// <summary>
	/// Database DataType: bigint NOT NULL
	/// </summary>
	public long AssignedMessageCount { get; private set; }

	/// <summary>
	/// Database DataType: bigint NOT NULL
	/// </summary>
	public long ProcessingMessageCount { get; private set; }

	/// <summary>
	/// Database DataType: bigint NOT NULL
	/// </summary>
	public long ProcessedMessageCount { get; private set; }

	/// <summary>
	/// Database DataType: bigint NOT NULL
	/// </summary>
	public long FailedMessageCount { get; private set; }

	/// <summary>
	/// Database DataType: bigint NOT NULL
	/// </summary>
	public long SuspendedMessageCount { get; private set; }


	private VwQueueMessages()
	{
	}

	public Dictionary<string, object?> ToDictionary()
		=> new()
		{
			{ nameof(IdQueue), IdQueue },
			{ nameof(QueueName), QueueName },
			{ nameof(IsActive), IsActive },
			{ nameof(IsSequentialFIFO), IsSequentialFIFO },
			{ nameof(MaxDegreeOfParallelism), MaxDegreeOfParallelism },
			{ nameof(IdJob), IdJob },
			{ nameof(IdOrchestration), IdOrchestration },
			{ nameof(AssignedMessageCount), AssignedMessageCount },
			{ nameof(ProcessingMessageCount), ProcessingMessageCount },
			{ nameof(ProcessedMessageCount), ProcessedMessageCount },
			{ nameof(FailedMessageCount), FailedMessageCount },
			{ nameof(SuspendedMessageCount), SuspendedMessageCount },
		};

	public override string? ToString()
	{
		return IdQueue.ToString();
	}
}
