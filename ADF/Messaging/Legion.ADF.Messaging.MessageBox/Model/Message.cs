using Legion.Validation;

namespace Legion.ADF.Messaging.MessageBox.Model;

public sealed partial class Message : MessageBox.MessageBoxBaseEntity, Legion.Model.IEntity
{
	public static IValidator<Message> DefaultDBValidator { get; }

	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid IdMessage { get; private set; }

	/// <summary>
	/// Database DataType: uuid NOT NULL | MessageBox.Model.MessageType.MessageType | FK_Message_IdMessageType
	/// </summary>
	public Guid IdMessageType { get; private set; }

	/// <summary>
	/// Database DataType: uuid NOT NULL | MessageBox.Model.MessageStatus.MessageStatus | FK_Message_IdMessageStatus
	/// </summary>
	public Guid IdMessageStatus { get; private set; }

	/// <summary>
	/// Database DataType: uuid NULL | MessageBox.Model.MessageContent.MessageContent | FK_Message_IdMessageContent
	/// </summary>
	public Guid? IdMessageContent { get; private set; }

	/// <summary>
	/// Database DataType: uuid NULL | MessageBox.Model.Queue.Queue | FK_Message_IdQueue
	/// </summary>
	public Guid? IdQueue { get; private set; }

	/// <summary>
	/// Database DataType: uuid NULL | MessageBox.Model.Topic.Topic | FK_Message_IdTopic
	/// </summary>
	public Guid? IdTopic { get; private set; }

	/// <summary>
	/// Database DataType: varchar(511) NULL
	/// </summary>
	public string? MessageId { get; private set; }

	/// <summary>
	/// Database DataType: varchar(511) NULL
	/// </summary>
	public string? BusinessId { get; private set; }

	/// <summary>
	/// Database DataType: varchar(511) NULL
	/// </summary>
	public string? CorrelationId { get; private set; }

	/// <summary>
	/// Database DataType: uuid NULL
	/// </summary>
	public Guid? SessionId { get; private set; }

	/// <summary>
	/// Database DataType: bigint NULL
	/// </summary>
	public long? SessionMessagePartId { get; private set; }

	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid TraceCorrelationId { get; private set; }

	/// <summary>
	/// Database DataType: jsonb NULL
	/// </summary>
	public string? Properties { get; private set; }

	/// <summary>
	/// Database DataType: varchar(511) NULL
	/// </summary>
	public string? Publisher { get; private set; }

	/// <summary>
	/// Database DataType: varchar(511) NULL
	/// </summary>
	public string? PublisherId { get; private set; }

	/// <summary>
	/// Database DataType: timestamp with time zone NOT NULL
	/// </summary>
	public DateTime CreatedUtc { get; private set; }

	/// <summary>
	/// Database DataType: timestamp with time zone NULL
	/// </summary>
	public DateTime? ValidToUtc { get; private set; }

	/// <summary>
	/// Database DataType: integer NOT NULL
	/// </summary>
	public int Priority { get; private set; }

	/// <summary>
	/// Database DataType: uuid NOT NULL | MessageBox.Model.MessageBoxInstance.MessageBoxInstance | FK_Message_MessageBoxInstance
	/// </summary>
	public Guid IdMessageBoxInstance { get; private set; }


	/// <summary>
	/// _1:N Guid IdMessageBoxInstance | FK_Message_MessageBoxInstance
	/// </summary>
	public MessageBox.Model.MessageBoxInstance MessageBoxInstance { get; private set; }

	/// <summary>
	/// UNIQUE INDEX: UQ_Message_IdMessageContent
	/// _1:1 Guid? IdMessageContent | FK_Message_IdMessageContent
	/// </summary>
	public MessageBox.Model.MessageContent MessageContent { get; private set; }

	/// <summary>
	/// _1:N Guid IdMessageStatus | FK_Message_IdMessageStatus
	/// </summary>
	public MessageBox.Model.MessageStatus MessageStatus { get; private set; }

	/// <summary>
	/// _1:N Guid IdMessageType | FK_Message_IdMessageType
	/// </summary>
	public MessageBox.Model.MessageType MessageType { get; private set; }

	/// <summary>
	/// _1:N Guid? IdQueue | FK_Message_IdQueue
	/// </summary>
	public MessageBox.Model.Queue Queue { get; private set; }

	/// <summary>
	/// _1:N Guid? IdTopic | FK_Message_IdTopic
	/// </summary>
	public MessageBox.Model.Topic Topic { get; private set; }

	private Message()
	{
	}

	static Message()
	{
		DefaultDBValidator = SetDBValidatorRules(new ValidatorBuilder<Message>()).Build();
	}

	public Dictionary<string, object?> ToDictionary()
		=> new()
		{
			{ nameof(IdMessage), IdMessage },
			{ nameof(IdMessageType), IdMessageType },
			{ nameof(IdMessageStatus), IdMessageStatus },
			{ nameof(IdMessageContent), IdMessageContent },
			{ nameof(IdQueue), IdQueue },
			{ nameof(IdTopic), IdTopic },
			{ nameof(MessageId), MessageId },
			{ nameof(BusinessId), BusinessId },
			{ nameof(CorrelationId), CorrelationId },
			{ nameof(SessionId), SessionId },
			{ nameof(SessionMessagePartId), SessionMessagePartId },
			{ nameof(TraceCorrelationId), TraceCorrelationId },
			{ nameof(Properties), Properties },
			{ nameof(Publisher), Publisher },
			{ nameof(PublisherId), PublisherId },
			{ nameof(CreatedUtc), CreatedUtc },
			{ nameof(ValidToUtc), ValidToUtc },
			{ nameof(Priority), Priority },
			{ nameof(IdMessageBoxInstance), IdMessageBoxInstance },
		};

	public void TrimStringValuesToFitDatabaseMaxLengths(string? postfix = null)
	{
		MessageId = Legion.Text.StringHelper.TrimToFitMaxLength(MessageId, 511, postfix);
		BusinessId = Legion.Text.StringHelper.TrimToFitMaxLength(BusinessId, 511, postfix);
		CorrelationId = Legion.Text.StringHelper.TrimToFitMaxLength(CorrelationId, 511, postfix);
		Publisher = Legion.Text.StringHelper.TrimToFitMaxLength(Publisher, 511, postfix);
		PublisherId = Legion.Text.StringHelper.TrimToFitMaxLength(PublisherId, 511, postfix);
	}

	public override string? GetPrimaryKeyValue()
	{
		return IdMessage.ToString();
	}

	public override string? ToString()
	{
		return IdMessage.ToString();
	}

	public static ValidatorBuilder<Message> SetDBValidatorRules(ValidatorBuilder<Message> builder)
		=> builder
			.ForProperty(x => x.IdMessage, v => v.NotDefaultOrEmpty())
			.ForProperty(x => x.IdMessageType, v => v.NotDefaultOrEmpty(), (x, parent) => x.MessageType == null)
			.ForProperty(x => x.IdMessageStatus, v => v.NotDefaultOrEmpty(), (x, parent) => x.MessageStatus == null)
			.ForProperty(x => x.MessageId, v => v.MaxLength(511))
			.ForProperty(x => x.BusinessId, v => v.MaxLength(511))
			.ForProperty(x => x.CorrelationId, v => v.MaxLength(511))
			//.ForProperty(x => x.TraceCorrelationId, v => v.NotDefaultOrEmpty())
			.ForProperty(x => x.Publisher, v => v.MaxLength(511))
			.ForProperty(x => x.PublisherId, v => v.MaxLength(511))
			//.ForProperty(x => x.CreatedUtc, v => v.NotDefaultOrEmpty())
			//.ForProperty(x => x.Priority, v => v.NotDefaultOrEmpty())
			.ForProperty(x => x.IdMessageBoxInstance, v => v.NotDefaultOrEmpty(), (x, parent) => x.MessageBoxInstance == null)
		;
}
