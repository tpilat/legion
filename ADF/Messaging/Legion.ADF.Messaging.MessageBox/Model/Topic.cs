using Legion.Validation;

namespace Legion.ADF.Messaging.MessageBox.Model;

public sealed partial class Topic : MessageBox.MessageBoxBaseEntity, Legion.Model.IEntity
{
	private List<MessageBox.Model.MessageArchive> _messageArchives;
	private List<MessageBox.Model.MessageBoxProcessingLog> _messageBoxProcessingLogs;
	private List<MessageBox.Model.Message> _messages;
	private List<MessageBox.Model.TopicSubscription> _topicSubscriptions;

	public static IValidator<Topic> DefaultDBValidator { get; }

	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid IdTopic { get; private set; }

	/// <summary>
	/// Database DataType: varchar(1023) NOT NULL
	/// </summary>
	public string Name { get; private set; }

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
	/// Database DataType: uuid NOT NULL | MessageBox.Model.QueueProcessingMode.ProcessingMode | FK_Topic_IdProcessingMode
	/// </summary>
	public Guid IdProcessingMode { get; private set; }

	/// <summary>
	/// Database DataType: uuid NOT NULL | MessageBox.Model.QueueProcessingMode.SuspendingMode | FK_Topic_IdSuspendingMode
	/// </summary>
	public Guid IdSuspendingMode { get; private set; }

	/// <summary>
	/// Database DataType: uuid NOT NULL | MessageBox.Model.MessageBoxInstance.MessageBoxInstance | FK_Topic_MessageBoxInstance
	/// </summary>
	public Guid IdMessageBoxInstance { get; private set; }


	/// <summary>
	/// _1:N Guid IdMessageBoxInstance | FK_Topic_MessageBoxInstance
	/// </summary>
	public MessageBox.Model.MessageBoxInstance MessageBoxInstance { get; private set; }

	/// <summary>
	/// _1:N Guid IdProcessingMode | FK_Topic_IdProcessingMode
	/// </summary>
	public MessageBox.Model.QueueProcessingMode ProcessingMode { get; private set; }

	/// <summary>
	/// _1:N Guid IdSuspendingMode | FK_Topic_IdSuspendingMode
	/// </summary>
	public MessageBox.Model.QueueProcessingMode SuspendingMode { get; private set; }


	/// <summary>
	/// N:_1 MessageBox.Model.MessageArchive.IdTopic | FK_MessageArchive_IdTopic
	/// </summary>
	public IReadOnlyList<MessageBox.Model.MessageArchive> MessageArchives => _messageArchives;

	/// <summary>
	/// N:_1 MessageBox.Model.MessageBoxProcessingLog.IdTopic | FK_MessageBoxProcessingLog_Topic
	/// </summary>
	public IReadOnlyList<MessageBox.Model.MessageBoxProcessingLog> MessageBoxProcessingLogs => _messageBoxProcessingLogs;

	/// <summary>
	/// N:_1 MessageBox.Model.Message.IdTopic | FK_Message_IdTopic
	/// </summary>
	public IReadOnlyList<MessageBox.Model.Message> Messages => _messages;

	/// <summary>
	/// N:_1 MessageBox.Model.TopicSubscription.IdTopic | FK_TopicSubscription_IdTopic
	/// </summary>
	public IReadOnlyList<MessageBox.Model.TopicSubscription> TopicSubscriptions => _topicSubscriptions;

	private Topic()
	{
		_messageArchives = [];
		_messageBoxProcessingLogs = [];
		_messages = [];
		_topicSubscriptions = [];
	}

	static Topic()
	{
		DefaultDBValidator = SetDBValidatorRules(new ValidatorBuilder<Topic>()).Build();
	}

	public Dictionary<string, object?> ToDictionary()
		=> new()
		{
			{ nameof(IdTopic), IdTopic },
			{ nameof(Name), Name },
			{ nameof(IsActive), IsActive },
			{ nameof(IsSequentialFIFO), IsSequentialFIFO },
			{ nameof(MessagesBatchCount), MessagesBatchCount },
			{ nameof(MaxDegreeOfParallelism), MaxDegreeOfParallelism },
			{ nameof(TimeoutForMessageProcessing), TimeoutForMessageProcessing },
			{ nameof(MaxMessageProcessingRetryCount), MaxMessageProcessingRetryCount },
			{ nameof(Properties), Properties },
			{ nameof(IdProcessingMode), IdProcessingMode },
			{ nameof(IdSuspendingMode), IdSuspendingMode },
			{ nameof(IdMessageBoxInstance), IdMessageBoxInstance },
		};

	public void TrimStringValuesToFitDatabaseMaxLengths(string? postfix = null)
	{
		Name = Legion.Text.StringHelper.TrimToFitMaxLength(Name, 1023, postfix);
	}

	public override string? GetPrimaryKeyValue()
	{
		return IdTopic.ToString();
	}

	public override string? ToString()
	{
		return IdTopic.ToString();
	}

	public static ValidatorBuilder<Topic> SetDBValidatorRules(ValidatorBuilder<Topic> builder)
		=> builder
			.ForProperty(x => x.IdTopic, v => v.NotDefaultOrEmpty())
			.ForProperty(x => x.Name, v => v.NotDefaultOrEmpty().MaxLength(1023))
			//.ForProperty(x => x.MessagesBatchCount, v => v.NotDefaultOrEmpty())
			//.ForProperty(x => x.TimeoutForMessageProcessing, v => v.NotDefaultOrEmpty())
			//.ForProperty(x => x.MaxMessageProcessingRetryCount, v => v.NotDefaultOrEmpty())
			.ForProperty(x => x.IdProcessingMode, v => v.NotDefaultOrEmpty(), (x, parent) => x.ProcessingMode == null)
			.ForProperty(x => x.IdSuspendingMode, v => v.NotDefaultOrEmpty(), (x, parent) => x.SuspendingMode == null)
			.ForProperty(x => x.IdMessageBoxInstance, v => v.NotDefaultOrEmpty(), (x, parent) => x.MessageBoxInstance == null)
		;
}
