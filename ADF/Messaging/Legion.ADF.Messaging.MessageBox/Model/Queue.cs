using Legion.Validation;

namespace Legion.ADF.Messaging.MessageBox.Model;

public sealed partial class Queue : MessageBox.MessageBoxBaseEntity, Legion.Model.IEntity
{
	private List<MessageBox.Model.MessageArchive> _messageArchives;
	private List<MessageBox.Model.MessageBoxProcessingLog> _messageBoxProcessingLogs;
	private List<MessageBox.Model.Message> _messages;
	private List<MessageBox.Model.QueuedMessage> _queuedMessages;

	public static IValidator<Queue> DefaultDBValidator { get; }

	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid IdQueue { get; private set; }

	/// <summary>
	/// Database DataType: varchar(1023) NOT NULL
	/// </summary>
	public string Name { get; private set; }

	/// <summary>
	/// Database DataType: varchar(1023) NOT NULL
	/// </summary>
	public string ReceivedEventNamespace { get; private set; }

	/// <summary>
	/// Database DataType: uuid NULL | MessageBox.Model.MessageType.MessageType | FK_Queue_IdMessageType
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
	/// Database DataType: uuid NOT NULL | MessageBox.Model.QueueProcessingMode.ProcessingMode | FK_Queue_IdProcessingMode
	/// </summary>
	public Guid IdProcessingMode { get; private set; }

	/// <summary>
	/// Database DataType: uuid NOT NULL | MessageBox.Model.QueueProcessingMode.SuspendingMode | FK_Queue_IdSuspendingMode
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
	/// Database DataType: uuid NOT NULL | MessageBox.Model.MessageBoxInstance.MessageBoxInstance | FK_Queue_MessageBoxInstance
	/// </summary>
	public Guid IdMessageBoxInstance { get; private set; }


	/// <summary>
	/// _1:N Guid IdMessageBoxInstance | FK_Queue_MessageBoxInstance
	/// </summary>
	public MessageBox.Model.MessageBoxInstance MessageBoxInstance { get; private set; }

	/// <summary>
	/// _1:N Guid? IdMessageType | FK_Queue_IdMessageType
	/// </summary>
	public MessageBox.Model.MessageType MessageType { get; private set; }

	/// <summary>
	/// _1:N Guid IdProcessingMode | FK_Queue_IdProcessingMode
	/// </summary>
	public MessageBox.Model.QueueProcessingMode ProcessingMode { get; private set; }

	/// <summary>
	/// _1:N Guid IdSuspendingMode | FK_Queue_IdSuspendingMode
	/// </summary>
	public MessageBox.Model.QueueProcessingMode SuspendingMode { get; private set; }


	/// <summary>
	/// N:_1 MessageBox.Model.MessageArchive.IdQueue | FK_MessageArchive_IdQueue
	/// </summary>
	public IReadOnlyList<MessageBox.Model.MessageArchive> MessageArchives => _messageArchives;

	/// <summary>
	/// N:_1 MessageBox.Model.MessageBoxProcessingLog.IdQueue | FK_MessageBoxProcessingLog_Queue
	/// </summary>
	public IReadOnlyList<MessageBox.Model.MessageBoxProcessingLog> MessageBoxProcessingLogs => _messageBoxProcessingLogs;

	/// <summary>
	/// N:_1 MessageBox.Model.Message.IdQueue | FK_Message_IdQueue
	/// </summary>
	public IReadOnlyList<MessageBox.Model.Message> Messages => _messages;

	/// <summary>
	/// N:_1 MessageBox.Model.QueuedMessage.IdQueue | FK_QueuedMessage_IdQueue
	/// </summary>
	public IReadOnlyList<MessageBox.Model.QueuedMessage> QueuedMessages => _queuedMessages;

	private Queue()
	{
		_messageArchives = [];
		_messageBoxProcessingLogs = [];
		_messages = [];
		_queuedMessages = [];
	}

	static Queue()
	{
		DefaultDBValidator = SetDBValidatorRules(new ValidatorBuilder<Queue>()).Build();
	}

	public Dictionary<string, object?> ToDictionary()
		=> new()
		{
			{ nameof(IdQueue), IdQueue },
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
			{ nameof(IdSuspendingMode), IdSuspendingMode },
			{ nameof(IdJob), IdJob },
			{ nameof(IdOrchestration), IdOrchestration },
			{ nameof(IdMessageBoxInstance), IdMessageBoxInstance },
		};

	public void TrimStringValuesToFitDatabaseMaxLengths(string? postfix = null)
	{
		Name = Legion.Text.StringHelper.TrimToFitMaxLength(Name, 1023, postfix);
		ReceivedEventNamespace = Legion.Text.StringHelper.TrimToFitMaxLength(ReceivedEventNamespace, 1023, postfix);
	}

	public override string? GetPrimaryKeyValue()
	{
		return IdQueue.ToString();
	}

	public override string? ToString()
	{
		return IdQueue.ToString();
	}

	public static ValidatorBuilder<Queue> SetDBValidatorRules(ValidatorBuilder<Queue> builder)
		=> builder
			.ForProperty(x => x.IdQueue, v => v.NotDefaultOrEmpty())
			.ForProperty(x => x.Name, v => v.NotDefaultOrEmpty().MaxLength(1023))
			.ForProperty(x => x.ReceivedEventNamespace, v => v.NotDefaultOrEmpty().MaxLength(1023))
			//.ForProperty(x => x.MessagesBatchCount, v => v.NotDefaultOrEmpty())
			//.ForProperty(x => x.TimeoutForMessageProcessing, v => v.NotDefaultOrEmpty())
			//.ForProperty(x => x.MaxMessageProcessingRetryCount, v => v.NotDefaultOrEmpty())
			.ForProperty(x => x.IdProcessingMode, v => v.NotDefaultOrEmpty(), (x, parent) => x.ProcessingMode == null)
			.ForProperty(x => x.IdSuspendingMode, v => v.NotDefaultOrEmpty(), (x, parent) => x.SuspendingMode == null)
			.ForProperty(x => x.IdMessageBoxInstance, v => v.NotDefaultOrEmpty(), (x, parent) => x.MessageBoxInstance == null)
		;
}
