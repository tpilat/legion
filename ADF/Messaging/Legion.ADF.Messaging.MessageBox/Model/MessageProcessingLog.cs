using Legion.Validation;

namespace Legion.ADF.Messaging.MessageBox.Model;

public sealed partial class MessageProcessingLog : MessageBox.MessageBoxBaseEntity, Legion.Model.IEntity
{
	public static IValidator<MessageProcessingLog> DefaultDBValidator { get; }

	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid IdMessageProcessingLog { get; private set; }

	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid IdMessage { get; private set; }

	/// <summary>
	/// Database DataType: uuid NULL | MessageBox.Model.QueuedMessage.QueuedMessage | FK_MessageProcessingLog_IdQueuedMessage
	/// </summary>
	public Guid? IdQueuedMessage { get; private set; }

	/// <summary>
	/// Database DataType: uuid NULL | MessageBox.Model.SubscribedMessage.SubscribedMessage | FK_MessageProcessingLog_IdSubscribedMessage
	/// </summary>
	public Guid? IdSubscribedMessage { get; private set; }

	/// <summary>
	/// Database DataType: timestamp with time zone NOT NULL
	/// </summary>
	public DateTime CreatedUtc { get; private set; }

	/// <summary>
	/// Database DataType: uuid NOT NULL | MessageBox.Model.MessageProcessingStatus.MessageProcessingStatus | FK_MessageProcessingLog_IdMessageProcessingStatus
	/// </summary>
	public Guid IdMessageProcessingStatus { get; private set; }

	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid TraceCorrelationId { get; private set; }

	/// <summary>
	/// Database DataType: uuid NULL
	/// </summary>
	public Guid? IdLogMessage { get; private set; }

	/// <summary>
	/// Database DataType: varchar(127) NOT NULL
	/// </summary>
	public string Code { get; private set; }

	/// <summary>
	/// Database DataType: text NULL
	/// </summary>
	public string? Detail { get; private set; }

	/// <summary>
	/// Database DataType: uuid NOT NULL | MessageBox.Model.MessageBoxInstance.MessageBoxInstance | FK_MessageProcessingLog_MessageBoxInstance
	/// </summary>
	public Guid IdMessageBoxInstance { get; private set; }


	/// <summary>
	/// _1:N Guid IdMessageBoxInstance | FK_MessageProcessingLog_MessageBoxInstance
	/// </summary>
	public MessageBox.Model.MessageBoxInstance MessageBoxInstance { get; private set; }

	/// <summary>
	/// _1:N Guid IdMessageProcessingStatus | FK_MessageProcessingLog_IdMessageProcessingStatus
	/// </summary>
	public MessageBox.Model.MessageProcessingStatus MessageProcessingStatus { get; private set; }

	/// <summary>
	/// _1:N Guid? IdQueuedMessage | FK_MessageProcessingLog_IdQueuedMessage
	/// </summary>
	public MessageBox.Model.QueuedMessage QueuedMessage { get; private set; }

	/// <summary>
	/// _1:N Guid? IdSubscribedMessage | FK_MessageProcessingLog_IdSubscribedMessage
	/// </summary>
	public MessageBox.Model.SubscribedMessage SubscribedMessage { get; private set; }

	private MessageProcessingLog()
	{
	}

	static MessageProcessingLog()
	{
		DefaultDBValidator = SetDBValidatorRules(new ValidatorBuilder<MessageProcessingLog>()).Build();
	}

	public Dictionary<string, object?> ToDictionary()
		=> new()
		{
			{ nameof(IdMessageProcessingLog), IdMessageProcessingLog },
			{ nameof(IdMessage), IdMessage },
			{ nameof(IdQueuedMessage), IdQueuedMessage },
			{ nameof(IdSubscribedMessage), IdSubscribedMessage },
			{ nameof(CreatedUtc), CreatedUtc },
			{ nameof(IdMessageProcessingStatus), IdMessageProcessingStatus },
			{ nameof(TraceCorrelationId), TraceCorrelationId },
			{ nameof(IdLogMessage), IdLogMessage },
			{ nameof(Code), Code },
			{ nameof(Detail), Detail },
			{ nameof(IdMessageBoxInstance), IdMessageBoxInstance },
		};

	public void TrimStringValuesToFitDatabaseMaxLengths(string? postfix = null)
	{
		Code = Legion.Text.StringHelper.TrimToFitMaxLength(Code, 127, postfix);
	}

	public override string? GetPrimaryKeyValue()
	{
		return IdMessageProcessingLog.ToString();
	}

	public override string? ToString()
	{
		return IdMessageProcessingLog.ToString();
	}

	public static ValidatorBuilder<MessageProcessingLog> SetDBValidatorRules(ValidatorBuilder<MessageProcessingLog> builder)
		=> builder
			.ForProperty(x => x.IdMessageProcessingLog, v => v.NotDefaultOrEmpty())
			//.ForProperty(x => x.IdMessage, v => v.NotDefaultOrEmpty())
			//.ForProperty(x => x.CreatedUtc, v => v.NotDefaultOrEmpty())
			.ForProperty(x => x.IdMessageProcessingStatus, v => v.NotDefaultOrEmpty(), (x, parent) => x.MessageProcessingStatus == null)
			//.ForProperty(x => x.TraceCorrelationId, v => v.NotDefaultOrEmpty())
			.ForProperty(x => x.Code, v => v.NotDefaultOrEmpty().MaxLength(127))
			.ForProperty(x => x.IdMessageBoxInstance, v => v.NotDefaultOrEmpty(), (x, parent) => x.MessageBoxInstance == null)
		;
}
