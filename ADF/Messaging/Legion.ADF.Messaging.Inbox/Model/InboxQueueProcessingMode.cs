using Legion.Validation;

namespace Legion.ADF.Messaging.Inbox.Model;

public sealed partial class InboxQueueProcessingMode : Inbox.InboxBaseEntity, Legion.Model.IEntity
{
	private List<Inbox.Model.InboxQueue> _inboxQueues;
	private List<Inbox.Model.InboxQueue> _suspendingModeInboxQueues;

	public static IValidator<InboxQueueProcessingMode> DefaultDBValidator { get; }

	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid IdInboxQueueProcessingMode { get; private set; }

	/// <summary>
	/// Database DataType: varchar(63) NOT NULL
	/// </summary>
	public string Code { get; private set; }

	/// <summary>
	/// Database DataType: varchar(127) NOT NULL
	/// </summary>
	public string Name { get; private set; }


	/// <summary>
	/// N:_1 Inbox.Model.InboxQueue.IdProcessingMode | FK_InboxQueue_IdProcessingMode
	/// </summary>
	public IReadOnlyList<Inbox.Model.InboxQueue> InboxQueues => _inboxQueues;

	/// <summary>
	/// N:_1 Inbox.Model.InboxQueue.IdSuspendingMode | FK_InboxQueue_IdSuspendingMode
	/// </summary>
	public IReadOnlyList<Inbox.Model.InboxQueue> SuspendingModeInboxQueues => _suspendingModeInboxQueues;

	private InboxQueueProcessingMode()
	{
		_inboxQueues = [];
		_suspendingModeInboxQueues = [];
	}

	public Dictionary<string, object?> ToDictionary()
		=> new()
		{
			{ nameof(IdInboxQueueProcessingMode), IdInboxQueueProcessingMode },
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
		return IdInboxQueueProcessingMode.ToString();
	}

	public override string? ToString()
	{
		return Code;
	}

	public static ValidatorBuilder<InboxQueueProcessingMode> SetDBValidatorRules(ValidatorBuilder<InboxQueueProcessingMode> builder)
		=> builder
			.ForProperty(x => x.IdInboxQueueProcessingMode, v => v.NotDefaultOrEmpty())
			.ForProperty(x => x.Code, v => v.NotDefaultOrEmpty().MaxLength(63))
			.ForProperty(x => x.Name, v => v.NotDefaultOrEmpty().MaxLength(127))
		;
}
