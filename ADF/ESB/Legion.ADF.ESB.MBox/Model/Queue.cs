using Legion.Validation;

namespace Legion.ADF.ESB.MBox.Model;

public sealed partial class Queue : MBox.MBoxBaseEntity, Legion.Model.IEntity
{
	private List<MBox.Model.QueuedMessage> _queuedMessages;

	public static IValidator<Queue> DefaultDBValidator { get; }

	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid IdQueue { get; private set; }

	/// <summary>
	/// Database DataType: uuid NULL | --NO TARGET-- | FK_Queue_IdOrchestration
	/// </summary>
	public Guid? IdOrchestration { get; private set; }

	/// <summary>
	/// Database DataType: uuid NULL | --NO TARGET-- | FK_Queue_IdJob
	/// </summary>
	public Guid? IdJob { get; private set; }

	/// <summary>
	/// Database DataType: uuid NULL | --NO TARGET-- | FK_Queue_IdAdapter
	/// </summary>
	public Guid? IdAdapter { get; private set; }

	/// <summary>
	/// Database DataType: uuid NOT NULL | MBox.Model.MessageType.MessageType | FK_Queue_IdMessageType
	/// </summary>
	public Guid IdMessageType { get; private set; }

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
	public int TimeoutForMessageProcessingInSeconds { get; private set; }

	/// <summary>
	/// Database DataType: integer NOT NULL
	/// </summary>
	public int MaxMessageProcessingRetryCount { get; private set; }

	/// <summary>
	/// Database DataType: jsonb NULL
	/// </summary>
	public string? Properties { get; private set; }


	/// <summary>
	/// _1:N Guid IdMessageType | FK_Queue_IdMessageType
	/// </summary>
	public MBox.Model.MessageType MessageType { get; private set; }


	/// <summary>
	/// N:_1 MBox.Model.QueuedMessage.IdQueue | FK_QueuedMessage_IdQueue
	/// </summary>
	public IReadOnlyList<MBox.Model.QueuedMessage> QueuedMessages => _queuedMessages;

	private Queue()
	{
		_queuedMessages = [];
	}

	static Queue()
	{
		DefaultDBValidator = SetDBValidatorRules(new ValidatorBuilder<Queue>()).Build();
	}

	public override string? ToString()
	{
		return IdQueue.ToString();
	}

	public static ValidatorBuilder<Queue> SetDBValidatorRules(ValidatorBuilder<Queue> builder)
		=> builder
			.ForProperty(x => x.IdQueue, v => v.NotDefaultOrEmpty())
			.ForProperty(x => x.IdMessageType, v => v.NotDefaultOrEmpty(), x => x.MessageType == null)
			//.ForProperty(x => x.TimeoutForMessageProcessingInSeconds, v => v.NotDefaultOrEmpty())
			//.ForProperty(x => x.MaxMessageProcessingRetryCount, v => v.NotDefaultOrEmpty())
		;
}
