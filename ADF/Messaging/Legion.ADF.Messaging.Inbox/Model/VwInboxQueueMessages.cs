namespace Legion.ADF.Messaging.Inbox.Model;

public sealed partial class VwInboxQueueMessages : Inbox.InboxBaseQueryEntity, Legion.Model.IQueryEntity
{
	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid IdInboxQueue { get; private set; }

	/// <summary>
	/// Database DataType: varchar(1023) NOT NULL
	/// </summary>
	public string InboxQueueName { get; private set; }

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


	private VwInboxQueueMessages()
	{
	}

	public Dictionary<string, object?> ToDictionary()
		=> new()
		{
			{ nameof(IdInboxQueue), IdInboxQueue },
			{ nameof(InboxQueueName), InboxQueueName },
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
		return IdInboxQueue.ToString();
	}
}
