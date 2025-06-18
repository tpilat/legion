using Legion.Validation;

namespace Legion.ADF.ESB.MBox.Model;

public sealed partial class Message : MBox.MBoxBaseEntity, Legion.Model.IEntity
{
	private List<MBox.Model.MessagePublishing> _messagePublishings;
	private List<MBox.Model.Message> _nextMessages;
	private List<MBox.Model.QueuedMessage> _queuedMessages;

	public static IValidator<Message> DefaultDBValidator { get; }

	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid IdMessage { get; private set; }

	/// <summary>
	/// Database DataType: uuid NOT NULL | MBox.Model.MessageType.MessageType | FK_Message_IdMessageType
	/// </summary>
	public Guid IdMessageType { get; private set; }

	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid BusinessId { get; private set; }

	/// <summary>
	/// Database DataType: uuid NOT NULL | MBox.Model.MessageStatus.MessageStatus | FK_Message_IdMessageStatus
	/// </summary>
	public Guid IdMessageStatus { get; private set; }

	/// <summary>
	/// Database DataType: jsonb NULL
	/// </summary>
	public string? SelfProperties { get; private set; }

	/// <summary>
	/// Database DataType: jsonb NULL
	/// </summary>
	public string? ContextProperties { get; private set; }

	/// <summary>
	/// Database DataType: timestamp without time zone NOT NULL
	/// </summary>
	public DateTime CreatedUtc { get; private set; }

	/// <summary>
	/// Database DataType: uuid NULL | MBox.Model.Message.PreviousMessage | FK_Message_IdPreviousMessage
	/// </summary>
	public Guid? IdPreviousMessage { get; private set; }

	/// <summary>
	/// Database DataType: varchar(511) NULL
	/// </summary>
	public string? ExternalId { get; private set; }

	/// <summary>
	/// Database DataType: uuid NULL | MBox.Model.MessageContent.MessageContent | FK_Message_IdMessageContent
	/// </summary>
	public Guid? IdMessageContent { get; private set; }

	/// <summary>
	/// Database DataType: timestamp without time zone NULL
	/// </summary>
	public DateTime? ValidToUtc { get; private set; }

	/// <summary>
	/// Database DataType: integer NOT NULL
	/// </summary>
	public int Priority { get; private set; }


	/// <summary>
	/// _1:1 Guid? IdMessageContent | FK_Message_IdMessageContent
	/// </summary>
	public MBox.Model.MessageContent MessageContent { get; private set; }

	/// <summary>
	/// _1:N Guid IdMessageStatus | FK_Message_IdMessageStatus
	/// </summary>
	public MBox.Model.MessageStatus MessageStatus { get; private set; }

	/// <summary>
	/// _1:N Guid IdMessageType | FK_Message_IdMessageType
	/// </summary>
	public MBox.Model.MessageType MessageType { get; private set; }

	/// <summary>
	/// _1:N Guid? IdPreviousMessage | FK_Message_IdPreviousMessage
	/// </summary>
	public MBox.Model.Message PreviousMessage { get; private set; }


	/// <summary>
	/// N:_1 MBox.Model.MessagePublishing.IdMessage | FK_MessagePublishing_IdMessage
	/// </summary>
	public IReadOnlyList<MBox.Model.MessagePublishing> MessagePublishings => _messagePublishings;

	/// <summary>
	/// N:_1 MBox.Model.Message.IdPreviousMessage | FK_Message_IdPreviousMessage
	/// </summary>
	public IReadOnlyList<MBox.Model.Message> NextMessages => _nextMessages;

	/// <summary>
	/// N:_1 MBox.Model.QueuedMessage.IdMessage | FK_QueuedMessage_IdMessage
	/// </summary>
	public IReadOnlyList<MBox.Model.QueuedMessage> QueuedMessages => _queuedMessages;

	private Message()
	{
		_messagePublishings = [];
		_nextMessages = [];
		_queuedMessages = [];
	}

	static Message()
	{
		DefaultDBValidator = SetDBValidatorRules(new ValidatorBuilder<Message>()).Build();
	}

	public override string? ToString()
	{
		return IdMessage.ToString();
	}

	public static ValidatorBuilder<Message> SetDBValidatorRules(ValidatorBuilder<Message> builder)
		=> builder
			.ForProperty(x => x.IdMessage, v => v.NotDefaultOrEmpty())
			.ForProperty(x => x.IdMessageType, v => v.NotDefaultOrEmpty(), x => x.MessageType == null)
			//.ForProperty(x => x.BusinessId, v => v.NotDefaultOrEmpty())
			.ForProperty(x => x.IdMessageStatus, v => v.NotDefaultOrEmpty(), x => x.MessageStatus == null)
			//.ForProperty(x => x.CreatedUtc, v => v.NotDefaultOrEmpty())
			.ForProperty(x => x.ExternalId, v => v.MaxLength(511))
			//.ForProperty(x => x.Priority, v => v.NotDefaultOrEmpty())
		;
}
