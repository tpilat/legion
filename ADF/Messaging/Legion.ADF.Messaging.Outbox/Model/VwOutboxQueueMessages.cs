namespace Legion.ADF.Messaging.Outbox.Model;

public sealed partial class VwOutboxQueueMessages : Outbox.OutboxBaseQueryEntity, Legion.Model.IQueryEntity
{
	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid IdOutboxQueue { get; private set; }

	/// <summary>
	/// Database DataType: varchar(1023) NOT NULL
	/// </summary>
	public string OutboxQueueName { get; private set; }

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
	/// Database DataType: bigint NOT NULL
	/// </summary>
	public long CreatedMessageCount { get; private set; }

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


	private VwOutboxQueueMessages()
	{
	}

	public Dictionary<string, object?> ToDictionary()
		=> new()
		{
			{ nameof(IdOutboxQueue), IdOutboxQueue },
			{ nameof(OutboxQueueName), OutboxQueueName },
			{ nameof(IsActive), IsActive },
			{ nameof(IsSequentialFIFO), IsSequentialFIFO },
			{ nameof(MaxDegreeOfParallelism), MaxDegreeOfParallelism },
			{ nameof(CreatedMessageCount), CreatedMessageCount },
			{ nameof(ProcessingMessageCount), ProcessingMessageCount },
			{ nameof(ProcessedMessageCount), ProcessedMessageCount },
			{ nameof(FailedMessageCount), FailedMessageCount },
			{ nameof(SuspendedMessageCount), SuspendedMessageCount },
		};

	public override string? ToString()
	{
		return IdOutboxQueue.ToString();
	}
}
