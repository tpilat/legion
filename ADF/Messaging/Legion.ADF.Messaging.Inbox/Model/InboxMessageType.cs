using Legion.Validation;

namespace Legion.ADF.Messaging.Inbox.Model;

public sealed partial class InboxMessageType : Inbox.InboxBaseEntity, Legion.Model.IEntity
{
	private List<Inbox.Model.InboxMessageArchive> _inboxMessageArchives;
	private List<Inbox.Model.InboxMessage> _inboxMessages;
	private List<Inbox.Model.InboxQueue> _inboxQueues;

	public static IValidator<InboxMessageType> DefaultDBValidator { get; }

	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid IdInboxMessageType { get; private set; }

	/// <summary>
	/// Database DataType: varchar(127) NOT NULL
	/// </summary>
	public string Code { get; private set; }

	/// <summary>
	/// Database DataType: varchar(127) NOT NULL
	/// </summary>
	public string Name { get; private set; }

	/// <summary>
	/// Database DataType: varchar(1023) NOT NULL
	/// </summary>
	public string Namespace { get; private set; }

	/// <summary>
	/// Database DataType: timestamp with time zone NOT NULL
	/// </summary>
	public DateTime CreatedUtc { get; private set; }

	/// <summary>
	/// Database DataType: uuid NOT NULL | Inbox.Model.InboxInstance.InboxInstance | FK_InboxMessageType_IdInboxInstance
	/// </summary>
	public Guid IdInboxInstance { get; private set; }


	/// <summary>
	/// _1:N Guid IdInboxInstance | FK_InboxMessageType_IdInboxInstance
	/// </summary>
	public Inbox.Model.InboxInstance InboxInstance { get; private set; }


	/// <summary>
	/// N:_1 Inbox.Model.InboxMessageArchive.IdMessageType | FK_InboxMessageArchive_IdMessageType
	/// </summary>
	public IReadOnlyList<Inbox.Model.InboxMessageArchive> InboxMessageArchives => _inboxMessageArchives;

	/// <summary>
	/// N:_1 Inbox.Model.InboxMessage.IdMessageType | FK_InboxMessage_IdMessageType
	/// </summary>
	public IReadOnlyList<Inbox.Model.InboxMessage> InboxMessages => _inboxMessages;

	/// <summary>
	/// N:_1 Inbox.Model.InboxQueue.IdMessageType | FK_InboxQueue_IdMessageType
	/// </summary>
	public IReadOnlyList<Inbox.Model.InboxQueue> InboxQueues => _inboxQueues;

	private InboxMessageType()
	{
		_inboxMessageArchives = [];
		_inboxMessages = [];
		_inboxQueues = [];
	}

	static InboxMessageType()
	{
		DefaultDBValidator = SetDBValidatorRules(new ValidatorBuilder<InboxMessageType>()).Build();
	}

	public Dictionary<string, object?> ToDictionary()
		=> new()
		{
			{ nameof(IdInboxMessageType), IdInboxMessageType },
			{ nameof(Code), Code },
			{ nameof(Name), Name },
			{ nameof(Namespace), Namespace },
			{ nameof(CreatedUtc), CreatedUtc },
			{ nameof(IdInboxInstance), IdInboxInstance },
		};

	public void TrimStringValuesToFitDatabaseMaxLengths(string? postfix = null)
	{
		Code = Legion.Text.StringHelper.TrimToFitMaxLength(Code, 127, postfix);
		Name = Legion.Text.StringHelper.TrimToFitMaxLength(Name, 127, postfix);
		Namespace = Legion.Text.StringHelper.TrimToFitMaxLength(Namespace, 1023, postfix);
	}

	public override string? GetPrimaryKeyValue()
	{
		return IdInboxMessageType.ToString();
	}

	public override string? ToString()
	{
		return IdInboxMessageType.ToString();
	}

	public static ValidatorBuilder<InboxMessageType> SetDBValidatorRules(ValidatorBuilder<InboxMessageType> builder)
		=> builder
			.ForProperty(x => x.IdInboxMessageType, v => v.NotDefaultOrEmpty())
			.ForProperty(x => x.Code, v => v.NotDefaultOrEmpty().MaxLength(127))
			.ForProperty(x => x.Name, v => v.NotDefaultOrEmpty().MaxLength(127))
			.ForProperty(x => x.Namespace, v => v.NotDefaultOrEmpty().MaxLength(1023))
			//.ForProperty(x => x.CreatedUtc, v => v.NotDefaultOrEmpty())
			.ForProperty(x => x.IdInboxInstance, v => v.NotDefaultOrEmpty(), (x, parent) => x.InboxInstance == null)
		;
}
