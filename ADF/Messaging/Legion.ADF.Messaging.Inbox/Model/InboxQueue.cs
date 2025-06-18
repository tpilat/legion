using Legion.Validation;

namespace Legion.ADF.Messaging.Inbox.Model;

public sealed partial class InboxQueue : Inbox.InboxBaseEntity, Legion.Model.IEntity
{
	private List<Inbox.Model.InboxMessageArchive> _inboxMessageArchives;
	private List<Inbox.Model.InboxMessageProcessingLog> _inboxMessageProcessingLogs;
	private List<Inbox.Model.InboxMessage> _inboxMessages;
	private List<Inbox.Model.InboxProcessingLog> _inboxProcessingLogs;

	public static IValidator<InboxQueue> DefaultDBValidator { get; }

	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid IdInboxQueue { get; private set; }

	/// <summary>
	/// Database DataType: varchar(1023) NOT NULL
	/// </summary>
	public string Name { get; private set; }

	/// <summary>
	/// Database DataType: varchar(1023) NOT NULL
	/// </summary>
	public string ReceivedEventNamespace { get; private set; }

	/// <summary>
	/// Database DataType: uuid NULL | Inbox.Model.InboxMessageType.MessageType | FK_InboxQueue_IdMessageType
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
	/// Database DataType: uuid NOT NULL | Inbox.Model.InboxQueueProcessingMode.ProcessingMode | FK_InboxQueue_IdProcessingMode
	/// </summary>
	public Guid IdProcessingMode { get; private set; }

	/// <summary>
	/// Database DataType: uuid NOT NULL | Inbox.Model.InboxQueueProcessingMode.SuspendingMode | FK_InboxQueue_IdSuspendingMode
	/// </summary>
	public Guid IdSuspendingMode { get; private set; }

	/// <summary>
	/// Database DataType: uuid NOT NULL | Inbox.Model.InboxInstance.InboxInstance | FK_InboxQueue_IdInboxInstance
	/// </summary>
	public Guid IdInboxInstance { get; private set; }


	/// <summary>
	/// _1:N Guid IdInboxInstance | FK_InboxQueue_IdInboxInstance
	/// </summary>
	public Inbox.Model.InboxInstance InboxInstance { get; private set; }

	/// <summary>
	/// _1:N Guid? IdMessageType | FK_InboxQueue_IdMessageType
	/// </summary>
	public Inbox.Model.InboxMessageType MessageType { get; private set; }

	/// <summary>
	/// _1:N Guid IdProcessingMode | FK_InboxQueue_IdProcessingMode
	/// </summary>
	public Inbox.Model.InboxQueueProcessingMode ProcessingMode { get; private set; }

	/// <summary>
	/// _1:N Guid IdSuspendingMode | FK_InboxQueue_IdSuspendingMode
	/// </summary>
	public Inbox.Model.InboxQueueProcessingMode SuspendingMode { get; private set; }


	/// <summary>
	/// N:_1 Inbox.Model.InboxMessageArchive.IdInboxQueue | FK_InboxMessageArchive_IdInboxQueue
	/// </summary>
	public IReadOnlyList<Inbox.Model.InboxMessageArchive> InboxMessageArchives => _inboxMessageArchives;

	/// <summary>
	/// N:_1 Inbox.Model.InboxMessageProcessingLog.IdInboxQueue | FK_InboxMessageProcessingLog_IdInboxQueue
	/// </summary>
	public IReadOnlyList<Inbox.Model.InboxMessageProcessingLog> InboxMessageProcessingLogs => _inboxMessageProcessingLogs;

	/// <summary>
	/// N:_1 Inbox.Model.InboxMessage.IdInboxQueue | FK_InboxMessage_IdInboxQueue
	/// </summary>
	public IReadOnlyList<Inbox.Model.InboxMessage> InboxMessages => _inboxMessages;

	/// <summary>
	/// N:_1 Inbox.Model.InboxProcessingLog.IdInboxQueue | FK_InboxProcessingLog_IdInboxQueue
	/// </summary>
	public IReadOnlyList<Inbox.Model.InboxProcessingLog> InboxProcessingLogs => _inboxProcessingLogs;

	private InboxQueue()
	{
		_inboxMessageArchives = [];
		_inboxMessageProcessingLogs = [];
		_inboxMessages = [];
		_inboxProcessingLogs = [];
	}

	static InboxQueue()
	{
		DefaultDBValidator = SetDBValidatorRules(new ValidatorBuilder<InboxQueue>()).Build();
	}

	public Dictionary<string, object?> ToDictionary()
		=> new()
		{
			{ nameof(IdInboxQueue), IdInboxQueue },
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
			{ nameof(IdInboxInstance), IdInboxInstance },
		};

	public void TrimStringValuesToFitDatabaseMaxLengths(string? postfix = null)
	{
		Name = Legion.Text.StringHelper.TrimToFitMaxLength(Name, 1023, postfix);
		ReceivedEventNamespace = Legion.Text.StringHelper.TrimToFitMaxLength(ReceivedEventNamespace, 1023, postfix);
	}

	public override string? GetPrimaryKeyValue()
	{
		return IdInboxQueue.ToString();
	}

	public override string? ToString()
	{
		return IdInboxQueue.ToString();
	}

	public static ValidatorBuilder<InboxQueue> SetDBValidatorRules(ValidatorBuilder<InboxQueue> builder)
		=> builder
			.ForProperty(x => x.IdInboxQueue, v => v.NotDefaultOrEmpty())
			.ForProperty(x => x.Name, v => v.NotDefaultOrEmpty().MaxLength(1023))
			.ForProperty(x => x.ReceivedEventNamespace, v => v.NotDefaultOrEmpty().MaxLength(1023))
			//.ForProperty(x => x.MessagesBatchCount, v => v.NotDefaultOrEmpty())
			//.ForProperty(x => x.TimeoutForMessageProcessing, v => v.NotDefaultOrEmpty())
			//.ForProperty(x => x.MaxMessageProcessingRetryCount, v => v.NotDefaultOrEmpty())
			.ForProperty(x => x.IdProcessingMode, v => v.NotDefaultOrEmpty(), (x, parent) => x.ProcessingMode == null)
			.ForProperty(x => x.IdSuspendingMode, v => v.NotDefaultOrEmpty(), (x, parent) => x.SuspendingMode == null)
			.ForProperty(x => x.IdInboxInstance, v => v.NotDefaultOrEmpty(), (x, parent) => x.InboxInstance == null)
		;
}
