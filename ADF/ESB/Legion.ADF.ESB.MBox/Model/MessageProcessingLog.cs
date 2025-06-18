using Legion.Validation;

namespace Legion.ADF.ESB.MBox.Model;

public sealed partial class MessageProcessingLog : MBox.MBoxBaseEntity, Legion.Model.IEntity
{
	public static IValidator<MessageProcessingLog> DefaultDBValidator { get; }

	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid IdMessageProcessingLog { get; private set; }

	/// <summary>
	/// Database DataType: uuid NOT NULL | MBox.Model.QueuedMessage.QueuedMessage | FK_MessageProcessingLog_IdQueuedMessage
	/// </summary>
	public Guid IdQueuedMessage { get; private set; }

	/// <summary>
	/// Database DataType: timestamp without time zone NOT NULL
	/// </summary>
	public DateTime CreatedUtc { get; private set; }

	/// <summary>
	/// Database DataType: integer NOT NULL
	/// </summary>
	public int IdLogLevel { get; private set; }

	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid LogCorrelationId { get; private set; }

	/// <summary>
	/// Database DataType: uuid NOT NULL | MBox.Model.MessageProcessingStatus.MessageProcessingStatus | FK_MessageProcessingLog_IdMessageProcessingStatus
	/// </summary>
	public Guid IdMessageProcessingStatus { get; private set; }

	/// <summary>
	/// Database DataType: text NOT NULL
	/// </summary>
	public string Detail { get; private set; }

	/// <summary>
	/// Database DataType: jsonb NULL
	/// </summary>
	public string? Data { get; private set; }

	/// <summary>
	/// Database DataType: uuid NULL
	/// </summary>
	public Guid? IdLogMessage { get; private set; }


	/// <summary>
	/// _1:N Guid IdMessageProcessingStatus | FK_MessageProcessingLog_IdMessageProcessingStatus
	/// </summary>
	public MBox.Model.MessageProcessingStatus MessageProcessingStatus { get; private set; }

	/// <summary>
	/// _1:N Guid IdQueuedMessage | FK_MessageProcessingLog_IdQueuedMessage
	/// </summary>
	public MBox.Model.QueuedMessage QueuedMessage { get; private set; }

	private MessageProcessingLog()
	{
	}

	static MessageProcessingLog()
	{
		DefaultDBValidator = SetDBValidatorRules(new ValidatorBuilder<MessageProcessingLog>()).Build();
	}

	public override string? ToString()
	{
		return IdMessageProcessingLog.ToString();
	}

	public static ValidatorBuilder<MessageProcessingLog> SetDBValidatorRules(ValidatorBuilder<MessageProcessingLog> builder)
		=> builder
			.ForProperty(x => x.IdMessageProcessingLog, v => v.NotDefaultOrEmpty())
			.ForProperty(x => x.IdQueuedMessage, v => v.NotDefaultOrEmpty(), x => x.QueuedMessage == null)
			//.ForProperty(x => x.CreatedUtc, v => v.NotDefaultOrEmpty())
			//.ForProperty(x => x.IdLogLevel, v => v.NotDefaultOrEmpty())
			//.ForProperty(x => x.LogCorrelationId, v => v.NotDefaultOrEmpty())
			.ForProperty(x => x.IdMessageProcessingStatus, v => v.NotDefaultOrEmpty(), x => x.MessageProcessingStatus == null)
			.ForProperty(x => x.Detail, v => v.NotDefaultOrEmpty())
		;
}
