using Legion.Validation;

namespace Legion.ADF.Messaging.Inbox.Model;

public sealed partial class InboxMessageStatus : Inbox.InboxBaseEntity, Legion.Model.IEntity
{
	private List<Inbox.Model.InboxMessageArchive> _inboxMessageArchives;
	private List<Inbox.Model.InboxMessageProcessingLog> _inboxMessageProcessingLogs;
	private List<Inbox.Model.InboxMessage> _inboxMessages;

	public static IValidator<InboxMessageStatus> DefaultDBValidator { get; }

	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid IdInboxMessageStatus { get; private set; }

	/// <summary>
	/// Database DataType: varchar(63) NOT NULL
	/// </summary>
	public string Code { get; private set; }

	/// <summary>
	/// Database DataType: varchar(127) NOT NULL
	/// </summary>
	public string Name { get; private set; }


	/// <summary>
	/// N:_1 Inbox.Model.InboxMessageArchive.IdInboxMessageStatus | FK_InboxMessageArchive_IdInboxMessageStatus
	/// </summary>
	public IReadOnlyList<Inbox.Model.InboxMessageArchive> InboxMessageArchives => _inboxMessageArchives;

	/// <summary>
	/// N:_1 Inbox.Model.InboxMessageProcessingLog.IdInboxMessageStatus | FK_InboxMessageProcessingLog_IdInboxMessageStatus
	/// </summary>
	public IReadOnlyList<Inbox.Model.InboxMessageProcessingLog> InboxMessageProcessingLogs => _inboxMessageProcessingLogs;

	/// <summary>
	/// N:_1 Inbox.Model.InboxMessage.IdInboxMessageStatus | FK_InboxMessage_IdInboxMessageStatus
	/// </summary>
	public IReadOnlyList<Inbox.Model.InboxMessage> InboxMessages => _inboxMessages;

	private InboxMessageStatus()
	{
		_inboxMessageArchives = [];
		_inboxMessageProcessingLogs = [];
		_inboxMessages = [];
	}

	public Dictionary<string, object?> ToDictionary()
		=> new()
		{
			{ nameof(IdInboxMessageStatus), IdInboxMessageStatus },
			{ nameof(Code), Code },
			{ nameof(Name), Name },
		};

	public void TrimStringValuesToFitDatabaseMaxLengths(string? postfix = null)
	{
		Code = Legion.Text.StringHelper.TrimToFitMaxLength(Code, 63, postfix);
		Name = Legion.Text.StringHelper.TrimToFitMaxLength(Name, 127, postfix);
	}

	public override string? GetPrimaryKeyValue()
	{
		return IdInboxMessageStatus.ToString();
	}

	public override string? ToString()
	{
		return Code;
	}

	public static ValidatorBuilder<InboxMessageStatus> SetDBValidatorRules(ValidatorBuilder<InboxMessageStatus> builder)
		=> builder
			.ForProperty(x => x.IdInboxMessageStatus, v => v.NotDefaultOrEmpty())
			.ForProperty(x => x.Code, v => v.NotDefaultOrEmpty().MaxLength(63))
			.ForProperty(x => x.Name, v => v.NotDefaultOrEmpty().MaxLength(127))
		;
}
