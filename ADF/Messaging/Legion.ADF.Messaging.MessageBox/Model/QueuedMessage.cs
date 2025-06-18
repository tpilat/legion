using Legion.Validation;

namespace Legion.ADF.Messaging.MessageBox.Model;

public sealed partial class QueuedMessage : MessageBox.MessageBoxBaseEntity, Legion.Model.IEntity
{
	private List<MessageBox.Model.MessageProcessingLog> _messageProcessingLogs;

	public static IValidator<QueuedMessage> DefaultDBValidator { get; }

	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid IdQueuedMessage { get; private set; }

	/// <summary>
	/// Database DataType: uuid NOT NULL | MessageBox.Model.Queue.Queue | FK_QueuedMessage_IdQueue
	/// </summary>
	public Guid IdQueue { get; private set; }

	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid IdMessage { get; private set; }

	/// <summary>
	/// Database DataType: uuid NOT NULL | MessageBox.Model.MessageProcessingStatus.MessageProcessingStatus | FK_QueuedMessage_IdMessageProcessingStatus
	/// </summary>
	public Guid IdMessageProcessingStatus { get; private set; }

	/// <summary>
	/// Database DataType: timestamp with time zone NOT NULL
	/// </summary>
	public DateTime AssignedUtc { get; private set; }

	/// <summary>
	/// Database DataType: timestamp with time zone NULL
	/// </summary>
	public DateTime? ProcessedUtc { get; private set; }

	/// <summary>
	/// Database DataType: timestamp with time zone NULL
	/// </summary>
	public DateTime? SuspendedUtc { get; private set; }

	/// <summary>
	/// Database DataType: timestamp with time zone NULL
	/// </summary>
	public DateTime? LastProcessingUtc { get; private set; }

	/// <summary>
	/// Database DataType: timestamp with time zone NULL
	/// </summary>
	public DateTime? LastProcessingTimeoutUtc { get; private set; }

	/// <summary>
	/// Database DataType: timestamp with time zone NOT NULL
	/// </summary>
	public DateTime NextProcessingUtc { get; private set; }

	/// <summary>
	/// Database DataType: integer NOT NULL
	/// </summary>
	public int RetryCount { get; private set; }

	/// <summary>
	/// Database DataType: uuid NOT NULL | MessageBox.Model.MessageBoxInstance.MessageBoxInstance | FK_QueuedMessage_MessageBoxInstance
	/// </summary>
	public Guid IdMessageBoxInstance { get; private set; }


	/// <summary>
	/// _1:N Guid IdMessageBoxInstance | FK_QueuedMessage_MessageBoxInstance
	/// </summary>
	public MessageBox.Model.MessageBoxInstance MessageBoxInstance { get; private set; }

	/// <summary>
	/// _1:N Guid IdMessageProcessingStatus | FK_QueuedMessage_IdMessageProcessingStatus
	/// </summary>
	public MessageBox.Model.MessageProcessingStatus MessageProcessingStatus { get; private set; }

	/// <summary>
	/// _1:N Guid IdQueue | FK_QueuedMessage_IdQueue
	/// </summary>
	public MessageBox.Model.Queue Queue { get; private set; }


	/// <summary>
	/// N:_1 MessageBox.Model.MessageProcessingLog.IdQueuedMessage | FK_MessageProcessingLog_IdQueuedMessage
	/// </summary>
	public IReadOnlyList<MessageBox.Model.MessageProcessingLog> MessageProcessingLogs => _messageProcessingLogs;

	private QueuedMessage()
	{
		_messageProcessingLogs = [];
	}

	static QueuedMessage()
	{
		DefaultDBValidator = SetDBValidatorRules(new ValidatorBuilder<QueuedMessage>()).Build();
	}

	public Dictionary<string, object?> ToDictionary()
		=> new()
		{
			{ nameof(IdQueuedMessage), IdQueuedMessage },
			{ nameof(IdQueue), IdQueue },
			{ nameof(IdMessage), IdMessage },
			{ nameof(IdMessageProcessingStatus), IdMessageProcessingStatus },
			{ nameof(AssignedUtc), AssignedUtc },
			{ nameof(ProcessedUtc), ProcessedUtc },
			{ nameof(SuspendedUtc), SuspendedUtc },
			{ nameof(LastProcessingUtc), LastProcessingUtc },
			{ nameof(LastProcessingTimeoutUtc), LastProcessingTimeoutUtc },
			{ nameof(NextProcessingUtc), NextProcessingUtc },
			{ nameof(RetryCount), RetryCount },
			{ nameof(IdMessageBoxInstance), IdMessageBoxInstance },
		};

	public void TrimStringValuesToFitDatabaseMaxLengths(string? postfix = null)
	{
	}

	public override string? GetPrimaryKeyValue()
	{
		return IdQueuedMessage.ToString();
	}

	public override string? ToString()
	{
		return IdQueuedMessage.ToString();
	}

	public static ValidatorBuilder<QueuedMessage> SetDBValidatorRules(ValidatorBuilder<QueuedMessage> builder)
		=> builder
			.ForProperty(x => x.IdQueuedMessage, v => v.NotDefaultOrEmpty())
			.ForProperty(x => x.IdQueue, v => v.NotDefaultOrEmpty(), (x, parent) => x.Queue == null)
			//.ForProperty(x => x.IdMessage, v => v.NotDefaultOrEmpty())
			.ForProperty(x => x.IdMessageProcessingStatus, v => v.NotDefaultOrEmpty(), (x, parent) => x.MessageProcessingStatus == null)
			//.ForProperty(x => x.AssignedUtc, v => v.NotDefaultOrEmpty())
			//.ForProperty(x => x.NextProcessingUtc, v => v.NotDefaultOrEmpty())
			//.ForProperty(x => x.RetryCount, v => v.NotDefaultOrEmpty())
			.ForProperty(x => x.IdMessageBoxInstance, v => v.NotDefaultOrEmpty(), (x, parent) => x.MessageBoxInstance == null)
		;
}
