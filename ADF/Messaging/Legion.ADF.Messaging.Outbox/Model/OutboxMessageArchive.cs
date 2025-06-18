using Legion.Validation;

namespace Legion.ADF.Messaging.Outbox.Model;

public sealed partial class OutboxMessageArchive : Outbox.OutboxBaseEntity, Legion.Model.IEntity
{
	public static IValidator<OutboxMessageArchive> DefaultDBValidator { get; }

	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid IdOutboxMessage { get; private set; }

	/// <summary>
	/// Database DataType: uuid NOT NULL | Outbox.Model.OutboxMessageType.MessageType | FK_OutboxMessageArchive_IdMessageType
	/// </summary>
	public Guid IdMessageType { get; private set; }

	/// <summary>
	/// Database DataType: uuid NOT NULL | Outbox.Model.OutboxMessageStatus.OutboxMessageStatus | FK_OutboxMessageArchive_IdOutboxMessageStatus
	/// </summary>
	public Guid IdOutboxMessageStatus { get; private set; }

	/// <summary>
	/// Database DataType: uuid NULL | Outbox.Model.OutboxMessageContent.MessageContent | FK_OutboxMessageArchive_IdMessageContent
	/// </summary>
	public Guid? IdMessageContent { get; private set; }

	/// <summary>
	/// Database DataType: uuid NOT NULL | Outbox.Model.OutboxQueue.OutboxQueue | FK_OutboxMessageArchive_IdOutboxQueue
	/// </summary>
	public Guid IdOutboxQueue { get; private set; }

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
	/// Database DataType: uuid NOT NULL | Outbox.Model.OutboxInstance.OutboxInstance | FK_OutboxMessageArchive_IdOutboxInstance
	/// </summary>
	public Guid IdOutboxInstance { get; private set; }


	/// <summary>
	/// UNIQUE INDEX: UQ_OutboxMessageArchive_IdMessageContent
	/// _1:1 Guid? IdMessageContent | FK_OutboxMessageArchive_IdMessageContent
	/// </summary>
	public Outbox.Model.OutboxMessageContent MessageContent { get; private set; }

	/// <summary>
	/// _1:N Guid IdMessageType | FK_OutboxMessageArchive_IdMessageType
	/// </summary>
	public Outbox.Model.OutboxMessageType MessageType { get; private set; }

	/// <summary>
	/// _1:N Guid IdOutboxInstance | FK_OutboxMessageArchive_IdOutboxInstance
	/// </summary>
	public Outbox.Model.OutboxInstance OutboxInstance { get; private set; }

	/// <summary>
	/// _1:N Guid IdOutboxMessageStatus | FK_OutboxMessageArchive_IdOutboxMessageStatus
	/// </summary>
	public Outbox.Model.OutboxMessageStatus OutboxMessageStatus { get; private set; }

	/// <summary>
	/// _1:N Guid IdOutboxQueue | FK_OutboxMessageArchive_IdOutboxQueue
	/// </summary>
	public Outbox.Model.OutboxQueue OutboxQueue { get; private set; }

	private OutboxMessageArchive()
	{
	}

	static OutboxMessageArchive()
	{
		DefaultDBValidator = SetDBValidatorRules(new ValidatorBuilder<OutboxMessageArchive>()).Build();
	}

	public Dictionary<string, object?> ToDictionary()
		=> new()
		{
			{ nameof(IdOutboxMessage), IdOutboxMessage },
			{ nameof(IdMessageType), IdMessageType },
			{ nameof(IdOutboxMessageStatus), IdOutboxMessageStatus },
			{ nameof(IdMessageContent), IdMessageContent },
			{ nameof(IdOutboxQueue), IdOutboxQueue },
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
			{ nameof(IdOutboxInstance), IdOutboxInstance },
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
		return IdOutboxMessage.ToString();
	}

	public override string? ToString()
	{
		return IdOutboxMessage.ToString();
	}

	public static ValidatorBuilder<OutboxMessageArchive> SetDBValidatorRules(ValidatorBuilder<OutboxMessageArchive> builder)
		=> builder
			.ForProperty(x => x.IdOutboxMessage, v => v.NotDefaultOrEmpty())
			.ForProperty(x => x.IdMessageType, v => v.NotDefaultOrEmpty(), (x, parent) => x.MessageType == null)
			.ForProperty(x => x.IdOutboxMessageStatus, v => v.NotDefaultOrEmpty(), (x, parent) => x.OutboxMessageStatus == null)
			.ForProperty(x => x.IdOutboxQueue, v => v.NotDefaultOrEmpty(), (x, parent) => x.OutboxQueue == null)
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
			.ForProperty(x => x.IdOutboxInstance, v => v.NotDefaultOrEmpty(), (x, parent) => x.OutboxInstance == null)
		;
}
