using Legion.Validation;

namespace Legion.ADF.Messaging.MessageBox.Model;

public sealed partial class MessageProcessingStatus : MessageBox.MessageBoxBaseEntity, Legion.Model.IEntity
{
	private List<MessageBox.Model.MessageProcessingLog> _messageProcessingLogs;
	private List<MessageBox.Model.QueuedMessage> _queuedMessages;
	private List<MessageBox.Model.SubscribedMessage> _subscribedMessages;

	public static IValidator<MessageProcessingStatus> DefaultDBValidator { get; }

	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid IdMessageProcessingStatus { get; private set; }

	/// <summary>
	/// Database DataType: varchar(63) NOT NULL
	/// </summary>
	public string Code { get; private set; }

	/// <summary>
	/// Database DataType: varchar(127) NOT NULL
	/// </summary>
	public string Name { get; private set; }


	/// <summary>
	/// N:_1 MessageBox.Model.MessageProcessingLog.IdMessageProcessingStatus | FK_MessageProcessingLog_IdMessageProcessingStatus
	/// </summary>
	public IReadOnlyList<MessageBox.Model.MessageProcessingLog> MessageProcessingLogs => _messageProcessingLogs;

	/// <summary>
	/// N:_1 MessageBox.Model.QueuedMessage.IdMessageProcessingStatus | FK_QueuedMessage_IdMessageProcessingStatus
	/// </summary>
	public IReadOnlyList<MessageBox.Model.QueuedMessage> QueuedMessages => _queuedMessages;

	/// <summary>
	/// N:_1 MessageBox.Model.SubscribedMessage.IdMessageProcessingStatus | FK_SubscribedMessage_IdMessageProcessingStatus
	/// </summary>
	public IReadOnlyList<MessageBox.Model.SubscribedMessage> SubscribedMessages => _subscribedMessages;

	private MessageProcessingStatus()
	{
		_messageProcessingLogs = [];
		_queuedMessages = [];
		_subscribedMessages = [];
	}

	public Dictionary<string, object?> ToDictionary()
		=> new()
		{
			{ nameof(IdMessageProcessingStatus), IdMessageProcessingStatus },
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
		return IdMessageProcessingStatus.ToString();
	}

	public override string? ToString()
	{
		return Code;
	}

	public static ValidatorBuilder<MessageProcessingStatus> SetDBValidatorRules(ValidatorBuilder<MessageProcessingStatus> builder)
		=> builder
			.ForProperty(x => x.IdMessageProcessingStatus, v => v.NotDefaultOrEmpty())
			.ForProperty(x => x.Code, v => v.NotDefaultOrEmpty().MaxLength(63))
			.ForProperty(x => x.Name, v => v.NotDefaultOrEmpty().MaxLength(127))
		;
}
