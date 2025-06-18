using Legion.Validation;

namespace Legion.ADF.Messaging.MessageBox.Model;

public sealed partial class MessageBoxInstance : MessageBox.MessageBoxBaseEntity, Legion.Model.IEntity
{
	private List<MessageBox.Model.BlockedMessageType> _blockedMessageTypes;
	private List<MessageBox.Model.MessageArchive> _messageArchives;
	private List<MessageBox.Model.MessageBoxProcessingLog> _messageBoxProcessingLogs;
	private List<MessageBox.Model.MessageProcessingLog> _messageProcessingLogs;
	private List<MessageBox.Model.Message> _messages;
	private List<MessageBox.Model.MessageType> _messageTypes;
	private List<MessageBox.Model.QueuedMessage> _queuedMessages;
	private List<MessageBox.Model.Queue> _queues;
	private List<MessageBox.Model.SubscribedMessage> _subscribedMessages;
	private List<MessageBox.Model.Topic> _topics;
	private List<MessageBox.Model.TopicSubscription> _topicSubscriptions;

	public static IValidator<MessageBoxInstance> DefaultDBValidator { get; }

	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid IdMessageBoxInstance { get; private set; }

	/// <summary>
	/// Database DataType: timestamp with time zone NOT NULL
	/// </summary>
	public DateTime CreatedUtc { get; private set; }

	/// <summary>
	/// Database DataType: varchar(255) NOT NULL
	/// </summary>
	public string Name { get; private set; }

	/// <summary>
	/// Database DataType: varchar(15) NOT NULL
	/// </summary>
	public string Version { get; private set; }

	/// <summary>
	/// Database DataType: integer NOT NULL
	/// </summary>
	public int MaxDegreeOfQueueParallelism { get; private set; }

	/// <summary>
	/// Database DataType: integer NOT NULL
	/// </summary>
	public int MaxDegreeOfTopicParallelism { get; private set; }

	/// <summary>
	/// Database DataType: integer NOT NULL
	/// </summary>
	public int IdLogLevel { get; private set; }


	/// <summary>
	/// N:_1 MessageBox.Model.BlockedMessageType.IdMessageBoxInstance | FK_BlockedMessageType_MessageBoxInstance
	/// </summary>
	public IReadOnlyList<MessageBox.Model.BlockedMessageType> BlockedMessageTypes => _blockedMessageTypes;

	/// <summary>
	/// N:_1 MessageBox.Model.MessageArchive.IdMessageBoxInstance | FK_MessageArchive_MessageBoxInstance
	/// </summary>
	public IReadOnlyList<MessageBox.Model.MessageArchive> MessageArchives => _messageArchives;

	/// <summary>
	/// N:_1 MessageBox.Model.MessageBoxProcessingLog.IdMessageBoxInstance | FK_MessageBoxProcessingLog_MessageBoxInstance
	/// </summary>
	public IReadOnlyList<MessageBox.Model.MessageBoxProcessingLog> MessageBoxProcessingLogs => _messageBoxProcessingLogs;

	/// <summary>
	/// N:_1 MessageBox.Model.MessageProcessingLog.IdMessageBoxInstance | FK_MessageProcessingLog_MessageBoxInstance
	/// </summary>
	public IReadOnlyList<MessageBox.Model.MessageProcessingLog> MessageProcessingLogs => _messageProcessingLogs;

	/// <summary>
	/// N:_1 MessageBox.Model.Message.IdMessageBoxInstance | FK_Message_MessageBoxInstance
	/// </summary>
	public IReadOnlyList<MessageBox.Model.Message> Messages => _messages;

	/// <summary>
	/// N:_1 MessageBox.Model.MessageType.IdMessageBoxInstance | FK_MessageType_MessageBoxInstance
	/// </summary>
	public IReadOnlyList<MessageBox.Model.MessageType> MessageTypes => _messageTypes;

	/// <summary>
	/// N:_1 MessageBox.Model.QueuedMessage.IdMessageBoxInstance | FK_QueuedMessage_MessageBoxInstance
	/// </summary>
	public IReadOnlyList<MessageBox.Model.QueuedMessage> QueuedMessages => _queuedMessages;

	/// <summary>
	/// N:_1 MessageBox.Model.Queue.IdMessageBoxInstance | FK_Queue_MessageBoxInstance
	/// </summary>
	public IReadOnlyList<MessageBox.Model.Queue> Queues => _queues;

	/// <summary>
	/// N:_1 MessageBox.Model.SubscribedMessage.IdMessageBoxInstance | FK_SubscribedMessage_MessageBoxInstance
	/// </summary>
	public IReadOnlyList<MessageBox.Model.SubscribedMessage> SubscribedMessages => _subscribedMessages;

	/// <summary>
	/// N:_1 MessageBox.Model.Topic.IdMessageBoxInstance | FK_Topic_MessageBoxInstance
	/// </summary>
	public IReadOnlyList<MessageBox.Model.Topic> Topics => _topics;

	/// <summary>
	/// N:_1 MessageBox.Model.TopicSubscription.IdMessageBoxInstance | FK_TopicSubscription_MessageBoxInstance
	/// </summary>
	public IReadOnlyList<MessageBox.Model.TopicSubscription> TopicSubscriptions => _topicSubscriptions;

	private MessageBoxInstance()
	{
		_blockedMessageTypes = [];
		_messageArchives = [];
		_messageBoxProcessingLogs = [];
		_messageProcessingLogs = [];
		_messages = [];
		_messageTypes = [];
		_queuedMessages = [];
		_queues = [];
		_subscribedMessages = [];
		_topics = [];
		_topicSubscriptions = [];
	}

	static MessageBoxInstance()
	{
		DefaultDBValidator = SetDBValidatorRules(new ValidatorBuilder<MessageBoxInstance>()).Build();
	}

	public Dictionary<string, object?> ToDictionary()
		=> new()
		{
			{ nameof(IdMessageBoxInstance), IdMessageBoxInstance },
			{ nameof(CreatedUtc), CreatedUtc },
			{ nameof(Name), Name },
			{ nameof(Version), Version },
			{ nameof(MaxDegreeOfQueueParallelism), MaxDegreeOfQueueParallelism },
			{ nameof(MaxDegreeOfTopicParallelism), MaxDegreeOfTopicParallelism },
			{ nameof(IdLogLevel), IdLogLevel },
		};

	public void TrimStringValuesToFitDatabaseMaxLengths(string? postfix = null)
	{
		Name = Legion.Text.StringHelper.TrimToFitMaxLength(Name, 255, postfix);
		Version = Legion.Text.StringHelper.TrimToFitMaxLength(Version, 15, postfix);
	}

	public override string? GetPrimaryKeyValue()
	{
		return IdMessageBoxInstance.ToString();
	}

	public override string? ToString()
	{
		return IdMessageBoxInstance.ToString();
	}

	public static ValidatorBuilder<MessageBoxInstance> SetDBValidatorRules(ValidatorBuilder<MessageBoxInstance> builder)
		=> builder
			.ForProperty(x => x.IdMessageBoxInstance, v => v.NotDefaultOrEmpty())
			//.ForProperty(x => x.CreatedUtc, v => v.NotDefaultOrEmpty())
			.ForProperty(x => x.Name, v => v.NotDefaultOrEmpty().MaxLength(255))
			.ForProperty(x => x.Version, v => v.NotDefaultOrEmpty().MaxLength(15))
			//.ForProperty(x => x.MaxDegreeOfQueueParallelism, v => v.NotDefaultOrEmpty())
			//.ForProperty(x => x.MaxDegreeOfTopicParallelism, v => v.NotDefaultOrEmpty())
			//.ForProperty(x => x.IdLogLevel, v => v.NotDefaultOrEmpty())
		;
}
