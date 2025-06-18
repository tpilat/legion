using Legion.Validation;

namespace Legion.ADF.Messaging.Outbox.Model;

public sealed partial class OutboxInstance : Outbox.OutboxBaseEntity, Legion.Model.IEntity
{
	private List<Outbox.Model.BlockedOutboxMessageType> _blockedOutboxMessageTypes;
	private List<Outbox.Model.OutboxMessageArchive> _outboxMessageArchives;
	private List<Outbox.Model.OutboxMessageProcessingLog> _outboxMessageProcessingLogs;
	private List<Outbox.Model.OutboxMessage> _outboxMessages;
	private List<Outbox.Model.OutboxMessageType> _outboxMessageTypes;
	private List<Outbox.Model.OutboxProcessingLog> _outboxProcessingLogs;
	private List<Outbox.Model.OutboxQueue> _outboxQueues;

	public static IValidator<OutboxInstance> DefaultDBValidator { get; }

	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid IdOutboxInstance { get; private set; }

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
	public int IdLogLevel { get; private set; }


	/// <summary>
	/// N:_1 Outbox.Model.BlockedOutboxMessageType.IdOutboxInstance | FK_BlockedOutboxMessageType_OutboxInstance
	/// </summary>
	public IReadOnlyList<Outbox.Model.BlockedOutboxMessageType> BlockedOutboxMessageTypes => _blockedOutboxMessageTypes;

	/// <summary>
	/// N:_1 Outbox.Model.OutboxMessageArchive.IdOutboxInstance | FK_OutboxMessageArchive_IdOutboxInstance
	/// </summary>
	public IReadOnlyList<Outbox.Model.OutboxMessageArchive> OutboxMessageArchives => _outboxMessageArchives;

	/// <summary>
	/// N:_1 Outbox.Model.OutboxMessageProcessingLog.IdOutboxInstance | FK_OutboxMessageProcessingLog_IdOutboxInstance
	/// </summary>
	public IReadOnlyList<Outbox.Model.OutboxMessageProcessingLog> OutboxMessageProcessingLogs => _outboxMessageProcessingLogs;

	/// <summary>
	/// N:_1 Outbox.Model.OutboxMessage.IdOutboxInstance | FK_OutboxMessage_IdOutboxInstance
	/// </summary>
	public IReadOnlyList<Outbox.Model.OutboxMessage> OutboxMessages => _outboxMessages;

	/// <summary>
	/// N:_1 Outbox.Model.OutboxMessageType.IdOutboxInstance | FK_OutboxMessageType_IdOutboxInstance
	/// </summary>
	public IReadOnlyList<Outbox.Model.OutboxMessageType> OutboxMessageTypes => _outboxMessageTypes;

	/// <summary>
	/// N:_1 Outbox.Model.OutboxProcessingLog.IdOutboxInstance | FK_OutboxProcessingLog_IdOutboxInstance
	/// </summary>
	public IReadOnlyList<Outbox.Model.OutboxProcessingLog> OutboxProcessingLogs => _outboxProcessingLogs;

	/// <summary>
	/// N:_1 Outbox.Model.OutboxQueue.IdOutboxInstance | FK_OutboxQueue_IdOutboxInstance
	/// </summary>
	public IReadOnlyList<Outbox.Model.OutboxQueue> OutboxQueues => _outboxQueues;

	private OutboxInstance()
	{
		_blockedOutboxMessageTypes = [];
		_outboxMessageArchives = [];
		_outboxMessageProcessingLogs = [];
		_outboxMessages = [];
		_outboxMessageTypes = [];
		_outboxProcessingLogs = [];
		_outboxQueues = [];
	}

	static OutboxInstance()
	{
		DefaultDBValidator = SetDBValidatorRules(new ValidatorBuilder<OutboxInstance>()).Build();
	}

	public Dictionary<string, object?> ToDictionary()
		=> new()
		{
			{ nameof(IdOutboxInstance), IdOutboxInstance },
			{ nameof(CreatedUtc), CreatedUtc },
			{ nameof(Name), Name },
			{ nameof(Version), Version },
			{ nameof(MaxDegreeOfQueueParallelism), MaxDegreeOfQueueParallelism },
			{ nameof(IdLogLevel), IdLogLevel },
		};

	public void TrimStringValuesToFitDatabaseMaxLengths(string? postfix = null)
	{
		Name = Legion.Text.StringHelper.TrimToFitMaxLength(Name, 255, postfix);
		Version = Legion.Text.StringHelper.TrimToFitMaxLength(Version, 15, postfix);
	}

	public override string? GetPrimaryKeyValue()
	{
		return IdOutboxInstance.ToString();
	}

	public override string? ToString()
	{
		return IdOutboxInstance.ToString();
	}

	public static ValidatorBuilder<OutboxInstance> SetDBValidatorRules(ValidatorBuilder<OutboxInstance> builder)
		=> builder
			.ForProperty(x => x.IdOutboxInstance, v => v.NotDefaultOrEmpty())
			//.ForProperty(x => x.CreatedUtc, v => v.NotDefaultOrEmpty())
			.ForProperty(x => x.Name, v => v.NotDefaultOrEmpty().MaxLength(255))
			.ForProperty(x => x.Version, v => v.NotDefaultOrEmpty().MaxLength(15))
			//.ForProperty(x => x.MaxDegreeOfQueueParallelism, v => v.NotDefaultOrEmpty())
			//.ForProperty(x => x.IdLogLevel, v => v.NotDefaultOrEmpty())
		;
}
