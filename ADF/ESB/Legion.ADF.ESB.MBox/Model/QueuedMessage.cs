using Legion.Validation;

namespace Legion.ADF.ESB.MBox.Model;

public sealed partial class QueuedMessage : MBox.MBoxBaseEntity, Legion.Model.IEntity
{
	private List<MBox.Model.MessageProcessingLog> _messageProcessingLogs;

	public static IValidator<QueuedMessage> DefaultDBValidator { get; }

	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid IdQueuedMessage { get; private set; }

	/// <summary>
	/// Database DataType: uuid NOT NULL | MBox.Model.Queue.Queue | FK_QueuedMessage_IdQueue
	/// </summary>
	public Guid IdQueue { get; private set; }

	/// <summary>
	/// Database DataType: uuid NOT NULL | MBox.Model.Message.Message | FK_QueuedMessage_IdMessage
	/// </summary>
	public Guid IdMessage { get; private set; }

	/// <summary>
	/// Database DataType: timestamp without time zone NOT NULL
	/// </summary>
	public DateTime QueuedUtc { get; private set; }

	/// <summary>
	/// Database DataType: uuid NOT NULL | MBox.Model.MessageProcessingStatus.MessageProcessingStatus | FK_QueuedMessage_IdMessageProcessingStatus
	/// </summary>
	public Guid IdMessageProcessingStatus { get; private set; }

	/// <summary>
	/// Database DataType: timestamp without time zone NOT NULL
	/// </summary>
	public DateTime LastProcessedUtc { get; private set; }

	/// <summary>
	/// Database DataType: timestamp without time zone NOT NULL
	/// </summary>
	public DateTime NextProcessingUtc { get; private set; }

	/// <summary>
	/// Database DataType: integer NOT NULL
	/// </summary>
	public int RetryCount { get; private set; }

	/// <summary>
	/// Database DataType: timestamp without time zone NULL
	/// </summary>
	public DateTime? ProcessedUtc { get; private set; }

	/// <summary>
	/// Database DataType: timestamp without time zone NULL
	/// </summary>
	public DateTime? TerminatedUtc { get; private set; }


	/// <summary>
	/// _1:N Guid IdMessage | FK_QueuedMessage_IdMessage
	/// </summary>
	public MBox.Model.Message Message { get; private set; }

	/// <summary>
	/// _1:N Guid IdMessageProcessingStatus | FK_QueuedMessage_IdMessageProcessingStatus
	/// </summary>
	public MBox.Model.MessageProcessingStatus MessageProcessingStatus { get; private set; }

	/// <summary>
	/// _1:N Guid IdQueue | FK_QueuedMessage_IdQueue
	/// </summary>
	public MBox.Model.Queue Queue { get; private set; }


	/// <summary>
	/// N:_1 MBox.Model.MessageProcessingLog.IdQueuedMessage | FK_MessageProcessingLog_IdQueuedMessage
	/// </summary>
	public IReadOnlyList<MBox.Model.MessageProcessingLog> MessageProcessingLogs => _messageProcessingLogs;

	private QueuedMessage()
	{
		_messageProcessingLogs = [];
	}

	static QueuedMessage()
	{
		DefaultDBValidator = SetDBValidatorRules(new ValidatorBuilder<QueuedMessage>()).Build();
	}

	public override string? ToString()
	{
		return IdQueuedMessage.ToString();
	}

	public static ValidatorBuilder<QueuedMessage> SetDBValidatorRules(ValidatorBuilder<QueuedMessage> builder)
		=> builder
			.ForProperty(x => x.IdQueuedMessage, v => v.NotDefaultOrEmpty())
			.ForProperty(x => x.IdQueue, v => v.NotDefaultOrEmpty(), x => x.Queue == null)
			.ForProperty(x => x.IdMessage, v => v.NotDefaultOrEmpty(), x => x.Message == null)
			//.ForProperty(x => x.QueuedUtc, v => v.NotDefaultOrEmpty())
			.ForProperty(x => x.IdMessageProcessingStatus, v => v.NotDefaultOrEmpty(), x => x.MessageProcessingStatus == null)
			//.ForProperty(x => x.LastProcessedUtc, v => v.NotDefaultOrEmpty())
			//.ForProperty(x => x.NextProcessingUtc, v => v.NotDefaultOrEmpty())
			//.ForProperty(x => x.RetryCount, v => v.NotDefaultOrEmpty())
		;
}
