using Legion.Validation;

namespace Legion.ADF.Messaging.Outbox.Model;

public sealed partial class OutboxQueue : Outbox.OutboxBaseEntity, Legion.Model.IEntity
{
	private List<Outbox.Model.OutboxMessageArchive> _outboxMessageArchives;
	private List<Outbox.Model.OutboxMessageProcessingLog> _outboxMessageProcessingLogs;
	private List<Outbox.Model.OutboxMessage> _outboxMessages;
	private List<Outbox.Model.OutboxProcessingLog> _outboxProcessingLogs;

	public static IValidator<OutboxQueue> DefaultDBValidator { get; }

	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid IdOutboxQueue { get; private set; }

	/// <summary>
	/// Database DataType: varchar(1023) NOT NULL
	/// </summary>
	public string Name { get; private set; }

	/// <summary>
	/// Database DataType: varchar(1023) NOT NULL
	/// </summary>
	public string ReceivedEventNamespace { get; private set; }

	/// <summary>
	/// Database DataType: uuid NULL | Outbox.Model.OutboxMessageType.MessageType | FK_OutboxQueue_IdMessageType
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
	/// Database DataType: uuid NOT NULL | Outbox.Model.OutboxQueueProcessingMode.ProcessingMode | FK_OutboxQueue_IdProcessingMode
	/// </summary>
	public Guid IdProcessingMode { get; private set; }

	/// <summary>
	/// Database DataType: uuid NOT NULL | Outbox.Model.OutboxQueueProcessingMode.SuspendingMode | FK_OutboxQueue_IdSuspendingMode
	/// </summary>
	public Guid IdSuspendingMode { get; private set; }

	/// <summary>
	/// Database DataType: uuid NOT NULL | Outbox.Model.OutboxInstance.OutboxInstance | FK_OutboxQueue_IdOutboxInstance
	/// </summary>
	public Guid IdOutboxInstance { get; private set; }


	/// <summary>
	/// _1:N Guid? IdMessageType | FK_OutboxQueue_IdMessageType
	/// </summary>
	public Outbox.Model.OutboxMessageType MessageType { get; private set; }

	/// <summary>
	/// _1:N Guid IdOutboxInstance | FK_OutboxQueue_IdOutboxInstance
	/// </summary>
	public Outbox.Model.OutboxInstance OutboxInstance { get; private set; }

	/// <summary>
	/// _1:N Guid IdProcessingMode | FK_OutboxQueue_IdProcessingMode
	/// </summary>
	public Outbox.Model.OutboxQueueProcessingMode ProcessingMode { get; private set; }

	/// <summary>
	/// _1:N Guid IdSuspendingMode | FK_OutboxQueue_IdSuspendingMode
	/// </summary>
	public Outbox.Model.OutboxQueueProcessingMode SuspendingMode { get; private set; }


	/// <summary>
	/// N:_1 Outbox.Model.OutboxMessageArchive.IdOutboxQueue | FK_OutboxMessageArchive_IdOutboxQueue
	/// </summary>
	public IReadOnlyList<Outbox.Model.OutboxMessageArchive> OutboxMessageArchives => _outboxMessageArchives;

	/// <summary>
	/// N:_1 Outbox.Model.OutboxMessageProcessingLog.IdOutboxQueue | FK_OutboxMessageProcessingLog_IdOutboxQueue
	/// </summary>
	public IReadOnlyList<Outbox.Model.OutboxMessageProcessingLog> OutboxMessageProcessingLogs => _outboxMessageProcessingLogs;

	/// <summary>
	/// N:_1 Outbox.Model.OutboxMessage.IdOutboxQueue | FK_OutboxMessage_IdOutboxQueue
	/// </summary>
	public IReadOnlyList<Outbox.Model.OutboxMessage> OutboxMessages => _outboxMessages;

	/// <summary>
	/// N:_1 Outbox.Model.OutboxProcessingLog.IdOutboxQueue | FK_OutboxProcessingLog_IdOutboxQueue
	/// </summary>
	public IReadOnlyList<Outbox.Model.OutboxProcessingLog> OutboxProcessingLogs => _outboxProcessingLogs;

	private OutboxQueue()
	{
		_outboxMessageArchives = [];
		_outboxMessageProcessingLogs = [];
		_outboxMessages = [];
		_outboxProcessingLogs = [];
	}

	static OutboxQueue()
	{
		DefaultDBValidator = SetDBValidatorRules(new ValidatorBuilder<OutboxQueue>()).Build();
	}

	public Dictionary<string, object?> ToDictionary()
		=> new()
		{
			{ nameof(IdOutboxQueue), IdOutboxQueue },
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
			{ nameof(IdOutboxInstance), IdOutboxInstance },
		};

	public void TrimStringValuesToFitDatabaseMaxLengths(string? postfix = null)
	{
		Name = Legion.Text.StringHelper.TrimToFitMaxLength(Name, 1023, postfix);
		ReceivedEventNamespace = Legion.Text.StringHelper.TrimToFitMaxLength(ReceivedEventNamespace, 1023, postfix);
	}

	public override string? GetPrimaryKeyValue()
	{
		return IdOutboxQueue.ToString();
	}

	public override string? ToString()
	{
		return IdOutboxQueue.ToString();
	}

	public static ValidatorBuilder<OutboxQueue> SetDBValidatorRules(ValidatorBuilder<OutboxQueue> builder)
		=> builder
			.ForProperty(x => x.IdOutboxQueue, v => v.NotDefaultOrEmpty())
			.ForProperty(x => x.Name, v => v.NotDefaultOrEmpty().MaxLength(1023))
			.ForProperty(x => x.ReceivedEventNamespace, v => v.NotDefaultOrEmpty().MaxLength(1023))
			//.ForProperty(x => x.MessagesBatchCount, v => v.NotDefaultOrEmpty())
			//.ForProperty(x => x.TimeoutForMessageProcessing, v => v.NotDefaultOrEmpty())
			//.ForProperty(x => x.MaxMessageProcessingRetryCount, v => v.NotDefaultOrEmpty())
			.ForProperty(x => x.IdProcessingMode, v => v.NotDefaultOrEmpty(), (x, parent) => x.ProcessingMode == null)
			.ForProperty(x => x.IdSuspendingMode, v => v.NotDefaultOrEmpty(), (x, parent) => x.SuspendingMode == null)
			.ForProperty(x => x.IdOutboxInstance, v => v.NotDefaultOrEmpty(), (x, parent) => x.OutboxInstance == null)
		;
}
