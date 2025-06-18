using Legion.Validation;

namespace Legion.ADF.Messaging.Inbox.Model;

public sealed partial class InboxMessage : Inbox.InboxBaseEntity, Legion.Model.IEntity
{
	public static IValidator<InboxMessage> DefaultDBValidator { get; }

	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid IdInboxMessage { get; private set; }

	/// <summary>
	/// Database DataType: uuid NOT NULL | Inbox.Model.InboxMessageType.MessageType | FK_InboxMessage_IdMessageType
	/// </summary>
	public Guid IdMessageType { get; private set; }

	/// <summary>
	/// Database DataType: uuid NOT NULL | Inbox.Model.InboxMessageStatus.InboxMessageStatus | FK_InboxMessage_IdInboxMessageStatus
	/// </summary>
	public Guid IdInboxMessageStatus { get; private set; }

	/// <summary>
	/// Database DataType: uuid NULL | Inbox.Model.InboxMessageContent.MessageContent | FK_InboxMessage_IdMessageContent
	/// </summary>
	public Guid? IdMessageContent { get; private set; }

	/// <summary>
	/// Database DataType: uuid NOT NULL | Inbox.Model.InboxQueue.InboxQueue | FK_InboxMessage_IdInboxQueue
	/// </summary>
	public Guid IdInboxQueue { get; private set; }

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
	public DateTime? ProcessedUtc { get; private set; }

	/// <summary>
	/// Database DataType: timestamp with time zone NULL
	/// </summary>
	public DateTime? SuspendedUtc { get; private set; }

	/// <summary>
	/// Database DataType: timestamp with time zone NULL
	/// </summary>
	public DateTime? LastProcessingUtc { get; private set; }

	/// <summary>
	/// Database DataType: timestamp with time zone NULL
	/// </summary>
	public DateTime? LastProcessingTimeoutUtc { get; private set; }

	/// <summary>
	/// Database DataType: timestamp with time zone NOT NULL
	/// </summary>
	public DateTime NextProcessingUtc { get; private set; }

	/// <summary>
	/// Database DataType: integer NOT NULL
	/// </summary>
	public int RetryCount { get; private set; }

	/// <summary>
	/// Database DataType: varchar(1023) NULL
	/// </summary>
	public string? TargetTopic { get; private set; }

	/// <summary>
	/// Database DataType: varchar(1023) NULL
	/// </summary>
	public string? TargetQueueName { get; private set; }

	/// <summary>
	/// Database DataType: uuid NOT NULL | Inbox.Model.InboxInstance.InboxInstance | FK_InboxMessage_IdInboxInstance
	/// </summary>
	public Guid IdInboxInstance { get; private set; }


	/// <summary>
	/// _1:N Guid IdInboxInstance | FK_InboxMessage_IdInboxInstance
	/// </summary>
	public Inbox.Model.InboxInstance InboxInstance { get; private set; }

	/// <summary>
	/// _1:N Guid IdInboxMessageStatus | FK_InboxMessage_IdInboxMessageStatus
	/// </summary>
	public Inbox.Model.InboxMessageStatus InboxMessageStatus { get; private set; }

	/// <summary>
	/// _1:N Guid IdInboxQueue | FK_InboxMessage_IdInboxQueue
	/// </summary>
	public Inbox.Model.InboxQueue InboxQueue { get; private set; }

	/// <summary>
	/// UNIQUE INDEX: UQ_InboxMessage_IdMessageContent
	/// _1:1 Guid? IdMessageContent | FK_InboxMessage_IdMessageContent
	/// </summary>
	public Inbox.Model.InboxMessageContent MessageContent { get; private set; }

	/// <summary>
	/// _1:N Guid IdMessageType | FK_InboxMessage_IdMessageType
	/// </summary>
	public Inbox.Model.InboxMessageType MessageType { get; private set; }

	private InboxMessage()
	{
	}

	static InboxMessage()
	{
		DefaultDBValidator = SetDBValidatorRules(new ValidatorBuilder<InboxMessage>()).Build();
	}

	public Dictionary<string, object?> ToDictionary()
		=> new()
		{
			{ nameof(IdInboxMessage), IdInboxMessage },
			{ nameof(IdMessageType), IdMessageType },
			{ nameof(IdInboxMessageStatus), IdInboxMessageStatus },
			{ nameof(IdMessageContent), IdMessageContent },
			{ nameof(IdInboxQueue), IdInboxQueue },
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
			{ nameof(ProcessedUtc), ProcessedUtc },
			{ nameof(SuspendedUtc), SuspendedUtc },
			{ nameof(LastProcessingUtc), LastProcessingUtc },
			{ nameof(LastProcessingTimeoutUtc), LastProcessingTimeoutUtc },
			{ nameof(NextProcessingUtc), NextProcessingUtc },
			{ nameof(RetryCount), RetryCount },
			{ nameof(TargetTopic), TargetTopic },
			{ nameof(TargetQueueName), TargetQueueName },
			{ nameof(IdInboxInstance), IdInboxInstance },
		};

	public void TrimStringValuesToFitDatabaseMaxLengths(string? postfix = null)
	{
		MessageId = Legion.Text.StringHelper.TrimToFitMaxLength(MessageId, 511, postfix);
		BusinessId = Legion.Text.StringHelper.TrimToFitMaxLength(BusinessId, 511, postfix);
		CorrelationId = Legion.Text.StringHelper.TrimToFitMaxLength(CorrelationId, 511, postfix);
		Publisher = Legion.Text.StringHelper.TrimToFitMaxLength(Publisher, 511, postfix);
		PublisherId = Legion.Text.StringHelper.TrimToFitMaxLength(PublisherId, 511, postfix);
		TargetTopic = Legion.Text.StringHelper.TrimToFitMaxLength(TargetTopic, 1023, postfix);
		TargetQueueName = Legion.Text.StringHelper.TrimToFitMaxLength(TargetQueueName, 1023, postfix);
	}

	public override string? GetPrimaryKeyValue()
	{
		return IdInboxMessage.ToString();
	}

	public override string? ToString()
	{
		return IdInboxMessage.ToString();
	}

	public static ValidatorBuilder<InboxMessage> SetDBValidatorRules(ValidatorBuilder<InboxMessage> builder)
		=> builder
			.ForProperty(x => x.IdInboxMessage, v => v.NotDefaultOrEmpty())
			.ForProperty(x => x.IdMessageType, v => v.NotDefaultOrEmpty(), (x, parent) => x.MessageType == null)
			.ForProperty(x => x.IdInboxMessageStatus, v => v.NotDefaultOrEmpty(), (x, parent) => x.InboxMessageStatus == null)
			.ForProperty(x => x.IdInboxQueue, v => v.NotDefaultOrEmpty(), (x, parent) => x.InboxQueue == null)
			.ForProperty(x => x.MessageId, v => v.MaxLength(511))
			.ForProperty(x => x.BusinessId, v => v.MaxLength(511))
			.ForProperty(x => x.CorrelationId, v => v.MaxLength(511))
			//.ForProperty(x => x.TraceCorrelationId, v => v.NotDefaultOrEmpty())
			.ForProperty(x => x.Publisher, v => v.MaxLength(511))
			.ForProperty(x => x.PublisherId, v => v.MaxLength(511))
			//.ForProperty(x => x.CreatedUtc, v => v.NotDefaultOrEmpty())
			//.ForProperty(x => x.NextProcessingUtc, v => v.NotDefaultOrEmpty())
			//.ForProperty(x => x.RetryCount, v => v.NotDefaultOrEmpty())
			.ForProperty(x => x.TargetTopic, v => v.MaxLength(1023))
			.ForProperty(x => x.TargetQueueName, v => v.MaxLength(1023))
			.ForProperty(x => x.IdInboxInstance, v => v.NotDefaultOrEmpty(), (x, parent) => x.InboxInstance == null)
		;
}
