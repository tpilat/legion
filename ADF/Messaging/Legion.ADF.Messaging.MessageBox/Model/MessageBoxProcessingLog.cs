using Legion.Validation;

namespace Legion.ADF.Messaging.MessageBox.Model;

public sealed partial class MessageBoxProcessingLog : MessageBox.MessageBoxBaseEntity, Legion.Model.IEntity
{
	public static IValidator<MessageBoxProcessingLog> DefaultDBValidator { get; }

	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid IdMessageBoxProcessingLog { get; private set; }

	/// <summary>
	/// Database DataType: uuid NOT NULL | MessageBox.Model.MessageBoxInstance.MessageBoxInstance | FK_MessageBoxProcessingLog_MessageBoxInstance
	/// </summary>
	public Guid IdMessageBoxInstance { get; private set; }

	/// <summary>
	/// Database DataType: uuid NULL | MessageBox.Model.Queue.Queue | FK_MessageBoxProcessingLog_Queue
	/// </summary>
	public Guid? IdQueue { get; private set; }

	/// <summary>
	/// Database DataType: uuid NULL | MessageBox.Model.Topic.Topic | FK_MessageBoxProcessingLog_Topic
	/// </summary>
	public Guid? IdTopic { get; private set; }

	/// <summary>
	/// Database DataType: uuid NULL | MessageBox.Model.TopicSubscription.TopicSubscription | FK_MessageBoxProcessingLog_TopicSubscription
	/// </summary>
	public Guid? IdTopicSubscription { get; private set; }

	/// <summary>
	/// Database DataType: timestamp with time zone NOT NULL
	/// </summary>
	public DateTime CreatedUtc { get; private set; }

	/// <summary>
	/// Database DataType: integer NOT NULL
	/// </summary>
	public int IdLogLevel { get; private set; }

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
	/// _1:N Guid IdMessageBoxInstance | FK_MessageBoxProcessingLog_MessageBoxInstance
	/// </summary>
	public MessageBox.Model.MessageBoxInstance MessageBoxInstance { get; private set; }

	/// <summary>
	/// _1:N Guid? IdQueue | FK_MessageBoxProcessingLog_Queue
	/// </summary>
	public MessageBox.Model.Queue Queue { get; private set; }

	/// <summary>
	/// _1:N Guid? IdTopic | FK_MessageBoxProcessingLog_Topic
	/// </summary>
	public MessageBox.Model.Topic Topic { get; private set; }

	/// <summary>
	/// _1:N Guid? IdTopicSubscription | FK_MessageBoxProcessingLog_TopicSubscription
	/// </summary>
	public MessageBox.Model.TopicSubscription TopicSubscription { get; private set; }

	private MessageBoxProcessingLog()
	{
	}

	static MessageBoxProcessingLog()
	{
		DefaultDBValidator = SetDBValidatorRules(new ValidatorBuilder<MessageBoxProcessingLog>()).Build();
	}

	public Dictionary<string, object?> ToDictionary()
		=> new()
		{
			{ nameof(IdMessageBoxProcessingLog), IdMessageBoxProcessingLog },
			{ nameof(IdMessageBoxInstance), IdMessageBoxInstance },
			{ nameof(IdQueue), IdQueue },
			{ nameof(IdTopic), IdTopic },
			{ nameof(IdTopicSubscription), IdTopicSubscription },
			{ nameof(CreatedUtc), CreatedUtc },
			{ nameof(IdLogLevel), IdLogLevel },
			{ nameof(TraceCorrelationId), TraceCorrelationId },
			{ nameof(IdLogMessage), IdLogMessage },
			{ nameof(Code), Code },
			{ nameof(Detail), Detail },
		};

	public void TrimStringValuesToFitDatabaseMaxLengths(string? postfix = null)
	{
		Code = Legion.Text.StringHelper.TrimToFitMaxLength(Code, 127, postfix);
	}

	public override string? GetPrimaryKeyValue()
	{
		return IdMessageBoxProcessingLog.ToString();
	}

	public override string? ToString()
	{
		return IdMessageBoxProcessingLog.ToString();
	}

	public static ValidatorBuilder<MessageBoxProcessingLog> SetDBValidatorRules(ValidatorBuilder<MessageBoxProcessingLog> builder)
		=> builder
			.ForProperty(x => x.IdMessageBoxProcessingLog, v => v.NotDefaultOrEmpty())
			.ForProperty(x => x.IdMessageBoxInstance, v => v.NotDefaultOrEmpty(), (x, parent) => x.MessageBoxInstance == null)
			//.ForProperty(x => x.CreatedUtc, v => v.NotDefaultOrEmpty())
			//.ForProperty(x => x.IdLogLevel, v => v.NotDefaultOrEmpty())
			//.ForProperty(x => x.TraceCorrelationId, v => v.NotDefaultOrEmpty())
			.ForProperty(x => x.Code, v => v.NotDefaultOrEmpty().MaxLength(127))
		;
}
