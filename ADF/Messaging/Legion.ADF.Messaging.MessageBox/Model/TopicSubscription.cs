using Legion.Validation;

namespace Legion.ADF.Messaging.MessageBox.Model;

public sealed partial class TopicSubscription : MessageBox.MessageBoxBaseEntity, Legion.Model.IEntity
{
	private List<MessageBox.Model.MessageBoxProcessingLog> _messageBoxProcessingLogs;
	private List<MessageBox.Model.SubscribedMessage> _subscribedMessages;

	public static IValidator<TopicSubscription> DefaultDBValidator { get; }

	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid IdTopicSubscription { get; private set; }

	/// <summary>
	/// Database DataType: uuid NOT NULL | MessageBox.Model.Topic.Topic | FK_TopicSubscription_IdTopic
	/// </summary>
	public Guid IdTopic { get; private set; }

	/// <summary>
	/// Database DataType: varchar(511) NOT NULL
	/// </summary>
	public string SubscriptionName { get; private set; }

	/// <summary>
	/// Database DataType: varchar(1023) NOT NULL
	/// </summary>
	public string ReceivedEventNamespace { get; private set; }

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
	/// Database DataType: uuid NOT NULL | MessageBox.Model.QueueProcessingMode.ProcessingMode | FK_TopicSubscription_IdProcessingMode
	/// </summary>
	public Guid IdProcessingMode { get; private set; }

	/// <summary>
	/// Database DataType: uuid NOT NULL | MessageBox.Model.QueueProcessingMode.SuspendingMode | FK_TopicSubscription_IdSuspendingMode
	/// </summary>
	public Guid IdSuspendingMode { get; private set; }

	/// <summary>
	/// Database DataType: uuid NULL
	/// </summary>
	public Guid? IdJob { get; private set; }

	/// <summary>
	/// Database DataType: uuid NULL
	/// </summary>
	public Guid? IdOrchestration { get; private set; }

	/// <summary>
	/// Database DataType: uuid NOT NULL | MessageBox.Model.MessageBoxInstance.MessageBoxInstance | FK_TopicSubscription_MessageBoxInstance
	/// </summary>
	public Guid IdMessageBoxInstance { get; private set; }


	/// <summary>
	/// _1:N Guid IdMessageBoxInstance | FK_TopicSubscription_MessageBoxInstance
	/// </summary>
	public MessageBox.Model.MessageBoxInstance MessageBoxInstance { get; private set; }

	/// <summary>
	/// _1:N Guid IdProcessingMode | FK_TopicSubscription_IdProcessingMode
	/// </summary>
	public MessageBox.Model.QueueProcessingMode ProcessingMode { get; private set; }

	/// <summary>
	/// _1:N Guid IdSuspendingMode | FK_TopicSubscription_IdSuspendingMode
	/// </summary>
	public MessageBox.Model.QueueProcessingMode SuspendingMode { get; private set; }

	/// <summary>
	/// _1:N Guid IdTopic | FK_TopicSubscription_IdTopic
	/// </summary>
	public MessageBox.Model.Topic Topic { get; private set; }


	/// <summary>
	/// N:_1 MessageBox.Model.MessageBoxProcessingLog.IdTopicSubscription | FK_MessageBoxProcessingLog_TopicSubscription
	/// </summary>
	public IReadOnlyList<MessageBox.Model.MessageBoxProcessingLog> MessageBoxProcessingLogs => _messageBoxProcessingLogs;

	/// <summary>
	/// N:_1 MessageBox.Model.SubscribedMessage.IdTopicSubscription | FK_SubscribedMessage_IdTopicSubscription
	/// </summary>
	public IReadOnlyList<MessageBox.Model.SubscribedMessage> SubscribedMessages => _subscribedMessages;

	private TopicSubscription()
	{
		_messageBoxProcessingLogs = [];
		_subscribedMessages = [];
	}

	static TopicSubscription()
	{
		DefaultDBValidator = SetDBValidatorRules(new ValidatorBuilder<TopicSubscription>()).Build();
	}

	public Dictionary<string, object?> ToDictionary()
		=> new()
		{
			{ nameof(IdTopicSubscription), IdTopicSubscription },
			{ nameof(IdTopic), IdTopic },
			{ nameof(SubscriptionName), SubscriptionName },
			{ nameof(ReceivedEventNamespace), ReceivedEventNamespace },
			{ nameof(IsActive), IsActive },
			{ nameof(IsSequentialFIFO), IsSequentialFIFO },
			{ nameof(MessagesBatchCount), MessagesBatchCount },
			{ nameof(MaxDegreeOfParallelism), MaxDegreeOfParallelism },
			{ nameof(TimeoutForMessageProcessing), TimeoutForMessageProcessing },
			{ nameof(MaxMessageProcessingRetryCount), MaxMessageProcessingRetryCount },
			{ nameof(Properties), Properties },
			{ nameof(IdProcessingMode), IdProcessingMode },
			{ nameof(IdSuspendingMode), IdSuspendingMode },
			{ nameof(IdJob), IdJob },
			{ nameof(IdOrchestration), IdOrchestration },
			{ nameof(IdMessageBoxInstance), IdMessageBoxInstance },
		};

	public void TrimStringValuesToFitDatabaseMaxLengths(string? postfix = null)
	{
		SubscriptionName = Legion.Text.StringHelper.TrimToFitMaxLength(SubscriptionName, 511, postfix);
		ReceivedEventNamespace = Legion.Text.StringHelper.TrimToFitMaxLength(ReceivedEventNamespace, 1023, postfix);
	}

	public override string? GetPrimaryKeyValue()
	{
		return IdTopicSubscription.ToString();
	}

	public override string? ToString()
	{
		return IdTopicSubscription.ToString();
	}

	public static ValidatorBuilder<TopicSubscription> SetDBValidatorRules(ValidatorBuilder<TopicSubscription> builder)
		=> builder
			.ForProperty(x => x.IdTopicSubscription, v => v.NotDefaultOrEmpty())
			.ForProperty(x => x.IdTopic, v => v.NotDefaultOrEmpty(), (x, parent) => x.Topic == null)
			.ForProperty(x => x.SubscriptionName, v => v.NotDefaultOrEmpty().MaxLength(511))
			.ForProperty(x => x.ReceivedEventNamespace, v => v.NotDefaultOrEmpty().MaxLength(1023))
			//.ForProperty(x => x.MessagesBatchCount, v => v.NotDefaultOrEmpty())
			//.ForProperty(x => x.TimeoutForMessageProcessing, v => v.NotDefaultOrEmpty())
			//.ForProperty(x => x.MaxMessageProcessingRetryCount, v => v.NotDefaultOrEmpty())
			.ForProperty(x => x.IdProcessingMode, v => v.NotDefaultOrEmpty(), (x, parent) => x.ProcessingMode == null)
			.ForProperty(x => x.IdSuspendingMode, v => v.NotDefaultOrEmpty(), (x, parent) => x.SuspendingMode == null)
			.ForProperty(x => x.IdMessageBoxInstance, v => v.NotDefaultOrEmpty(), (x, parent) => x.MessageBoxInstance == null)
		;
}
