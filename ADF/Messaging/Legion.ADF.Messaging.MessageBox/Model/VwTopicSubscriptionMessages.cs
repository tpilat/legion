namespace Legion.ADF.Messaging.MessageBox.Model;

public sealed partial class VwTopicSubscriptionMessages : MessageBox.MessageBoxBaseQueryEntity, Legion.Model.IQueryEntity
{
	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid IdTopicSubscription { get; private set; }

	/// <summary>
	/// Database DataType: varchar(511) NOT NULL
	/// </summary>
	public string SubscriptionName { get; private set; }

	/// <summary>
	/// Database DataType: boolean NOT NULL
	/// </summary>
	public bool SubscriptionIsActive { get; private set; }

	/// <summary>
	/// Database DataType: boolean NOT NULL
	/// </summary>
	public bool SubscriptionIsSequentialFIFO { get; private set; }

	/// <summary>
	/// Database DataType: integer NOT NULL
	/// </summary>
	public int SubscriptionMessagesBatchCount { get; private set; }

	/// <summary>
	/// Database DataType: integer NULL
	/// </summary>
	public int? SubscriptionMaxDegreeOfParallelism { get; private set; }

	/// <summary>
	/// Database DataType: interval NOT NULL
	/// </summary>
	public TimeSpan SubscriptionTimeoutForMessageProcessing { get; private set; }

	/// <summary>
	/// Database DataType: integer NOT NULL
	/// </summary>
	public int SubscriptionMaxMessageProcessingRetryCount { get; private set; }

	/// <summary>
	/// Database DataType: uuid NULL
	/// </summary>
	public Guid? IdJob { get; private set; }

	/// <summary>
	/// Database DataType: uuid NULL
	/// </summary>
	public Guid? IdOrchestration { get; private set; }

	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid IdTopic { get; private set; }

	/// <summary>
	/// Database DataType: varchar(1023) NOT NULL
	/// </summary>
	public string TopicName { get; private set; }

	/// <summary>
	/// Database DataType: boolean NOT NULL
	/// </summary>
	public bool TopisIsActive { get; private set; }

	/// <summary>
	/// Database DataType: boolean NOT NULL
	/// </summary>
	public bool TopicIsSequentialFIFO { get; private set; }

	/// <summary>
	/// Database DataType: integer NOT NULL
	/// </summary>
	public int TopicMessagesBatchCount { get; private set; }

	/// <summary>
	/// Database DataType: integer NULL
	/// </summary>
	public int? TopicMaxDegreeOfParallelism { get; private set; }

	/// <summary>
	/// Database DataType: interval NOT NULL
	/// </summary>
	public TimeSpan TopicTimeoutForMessageProcessing { get; private set; }

	/// <summary>
	/// Database DataType: integer NOT NULL
	/// </summary>
	public int TopicMaxMessageProcessingRetryCount { get; private set; }

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


	private VwTopicSubscriptionMessages()
	{
	}

	public Dictionary<string, object?> ToDictionary()
		=> new()
		{
			{ nameof(IdTopicSubscription), IdTopicSubscription },
			{ nameof(SubscriptionName), SubscriptionName },
			{ nameof(SubscriptionIsActive), SubscriptionIsActive },
			{ nameof(SubscriptionIsSequentialFIFO), SubscriptionIsSequentialFIFO },
			{ nameof(SubscriptionMessagesBatchCount), SubscriptionMessagesBatchCount },
			{ nameof(SubscriptionMaxDegreeOfParallelism), SubscriptionMaxDegreeOfParallelism },
			{ nameof(SubscriptionTimeoutForMessageProcessing), SubscriptionTimeoutForMessageProcessing },
			{ nameof(SubscriptionMaxMessageProcessingRetryCount), SubscriptionMaxMessageProcessingRetryCount },
			{ nameof(IdJob), IdJob },
			{ nameof(IdOrchestration), IdOrchestration },
			{ nameof(IdTopic), IdTopic },
			{ nameof(TopicName), TopicName },
			{ nameof(TopisIsActive), TopisIsActive },
			{ nameof(TopicIsSequentialFIFO), TopicIsSequentialFIFO },
			{ nameof(TopicMessagesBatchCount), TopicMessagesBatchCount },
			{ nameof(TopicMaxDegreeOfParallelism), TopicMaxDegreeOfParallelism },
			{ nameof(TopicTimeoutForMessageProcessing), TopicTimeoutForMessageProcessing },
			{ nameof(TopicMaxMessageProcessingRetryCount), TopicMaxMessageProcessingRetryCount },
			{ nameof(AssignedMessageCount), AssignedMessageCount },
			{ nameof(ProcessingMessageCount), ProcessingMessageCount },
			{ nameof(ProcessedMessageCount), ProcessedMessageCount },
			{ nameof(FailedMessageCount), FailedMessageCount },
			{ nameof(SuspendedMessageCount), SuspendedMessageCount },
		};

	public override string? ToString()
	{
		return IdTopicSubscription.ToString();
	}
}
