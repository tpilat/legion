using Legion.Validation;

namespace Legion.ADF.ESB.MBox.Model;

public sealed partial class MessageProcessingStatus : MBox.MBoxBaseEntity, Legion.Model.IEntity
{
	private List<MBox.Model.MessageProcessingLog> _messageProcessingLogs;
	private List<MBox.Model.QueuedMessage> _queuedMessages;

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
	/// Database DataType: varchar(63) NOT NULL
	/// </summary>
	public string Name { get; private set; }


	/// <summary>
	/// N:_1 MBox.Model.MessageProcessingLog.IdMessageProcessingStatus | FK_MessageProcessingLog_IdMessageProcessingStatus
	/// </summary>
	public IReadOnlyList<MBox.Model.MessageProcessingLog> MessageProcessingLogs => _messageProcessingLogs;

	/// <summary>
	/// N:_1 MBox.Model.QueuedMessage.IdMessageProcessingStatus | FK_QueuedMessage_IdMessageProcessingStatus
	/// </summary>
	public IReadOnlyList<MBox.Model.QueuedMessage> QueuedMessages => _queuedMessages;

	private MessageProcessingStatus()
	{
		_messageProcessingLogs = [];
		_queuedMessages = [];
	}

	public override string? ToString()
	{
		return Code;
	}

	public static ValidatorBuilder<MessageProcessingStatus> SetDBValidatorRules(ValidatorBuilder<MessageProcessingStatus> builder)
		=> builder
			.ForProperty(x => x.IdMessageProcessingStatus, v => v.NotDefaultOrEmpty())
			.ForProperty(x => x.Code, v => v.NotDefaultOrEmpty().MaxLength(63))
			.ForProperty(x => x.Name, v => v.NotDefaultOrEmpty().MaxLength(63))
		;
}
