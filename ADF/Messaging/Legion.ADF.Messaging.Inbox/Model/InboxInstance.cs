using Legion.Validation;

namespace Legion.ADF.Messaging.Inbox.Model;

public sealed partial class InboxInstance : Inbox.InboxBaseEntity, Legion.Model.IEntity
{
	private List<Inbox.Model.BlockedInboxMessageType> _blockedInboxMessageTypes;
	private List<Inbox.Model.InboxMessageArchive> _inboxMessageArchives;
	private List<Inbox.Model.InboxMessageProcessingLog> _inboxMessageProcessingLogs;
	private List<Inbox.Model.InboxMessage> _inboxMessages;
	private List<Inbox.Model.InboxMessageType> _inboxMessageTypes;
	private List<Inbox.Model.InboxProcessingLog> _inboxProcessingLogs;
	private List<Inbox.Model.InboxQueue> _inboxQueues;

	public static IValidator<InboxInstance> DefaultDBValidator { get; }

	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid IdInboxInstance { get; private set; }

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
	/// N:_1 Inbox.Model.BlockedInboxMessageType.IdInboxInstance | FK_BlockedInboxMessageType_InboxInstance
	/// </summary>
	public IReadOnlyList<Inbox.Model.BlockedInboxMessageType> BlockedInboxMessageTypes => _blockedInboxMessageTypes;

	/// <summary>
	/// N:_1 Inbox.Model.InboxMessageArchive.IdInboxInstance | FK_InboxMessageArchive_IdInboxInstance
	/// </summary>
	public IReadOnlyList<Inbox.Model.InboxMessageArchive> InboxMessageArchives => _inboxMessageArchives;

	/// <summary>
	/// N:_1 Inbox.Model.InboxMessageProcessingLog.IdInboxInstance | FK_InboxMessageProcessingLog_IdInboxInstance
	/// </summary>
	public IReadOnlyList<Inbox.Model.InboxMessageProcessingLog> InboxMessageProcessingLogs => _inboxMessageProcessingLogs;

	/// <summary>
	/// N:_1 Inbox.Model.InboxMessage.IdInboxInstance | FK_InboxMessage_IdInboxInstance
	/// </summary>
	public IReadOnlyList<Inbox.Model.InboxMessage> InboxMessages => _inboxMessages;

	/// <summary>
	/// N:_1 Inbox.Model.InboxMessageType.IdInboxInstance | FK_InboxMessageType_IdInboxInstance
	/// </summary>
	public IReadOnlyList<Inbox.Model.InboxMessageType> InboxMessageTypes => _inboxMessageTypes;

	/// <summary>
	/// N:_1 Inbox.Model.InboxProcessingLog.IdInboxInstance | FK_InboxProcessingLog_IdInboxInstance
	/// </summary>
	public IReadOnlyList<Inbox.Model.InboxProcessingLog> InboxProcessingLogs => _inboxProcessingLogs;

	/// <summary>
	/// N:_1 Inbox.Model.InboxQueue.IdInboxInstance | FK_InboxQueue_IdInboxInstance
	/// </summary>
	public IReadOnlyList<Inbox.Model.InboxQueue> InboxQueues => _inboxQueues;

	private InboxInstance()
	{
		_blockedInboxMessageTypes = [];
		_inboxMessageArchives = [];
		_inboxMessageProcessingLogs = [];
		_inboxMessages = [];
		_inboxMessageTypes = [];
		_inboxProcessingLogs = [];
		_inboxQueues = [];
	}

	static InboxInstance()
	{
		DefaultDBValidator = SetDBValidatorRules(new ValidatorBuilder<InboxInstance>()).Build();
	}

	public Dictionary<string, object?> ToDictionary()
		=> new()
		{
			{ nameof(IdInboxInstance), IdInboxInstance },
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
		return IdInboxInstance.ToString();
	}

	public override string? ToString()
	{
		return IdInboxInstance.ToString();
	}

	public static ValidatorBuilder<InboxInstance> SetDBValidatorRules(ValidatorBuilder<InboxInstance> builder)
		=> builder
			.ForProperty(x => x.IdInboxInstance, v => v.NotDefaultOrEmpty())
			//.ForProperty(x => x.CreatedUtc, v => v.NotDefaultOrEmpty())
			.ForProperty(x => x.Name, v => v.NotDefaultOrEmpty().MaxLength(255))
			.ForProperty(x => x.Version, v => v.NotDefaultOrEmpty().MaxLength(15))
			//.ForProperty(x => x.MaxDegreeOfQueueParallelism, v => v.NotDefaultOrEmpty())
			//.ForProperty(x => x.IdLogLevel, v => v.NotDefaultOrEmpty())
		;
}
