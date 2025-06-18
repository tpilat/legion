using Legion.Validation;

namespace Legion.ADF.Messaging.MessageBox.Model;

public sealed partial class SubscribedMessage : MessageBox.MessageBoxBaseEntity, Legion.Model.IEntity
{
	private List<MessageBox.Model.MessageProcessingLog> _messageProcessingLogs;

	public static IValidator<SubscribedMessage> DefaultDBValidator { get; }

	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid IdSubscribedMessage { get; private set; }

	/// <summary>
	/// Database DataType: uuid NOT NULL | MessageBox.Model.TopicSubscription.TopicSubscription | FK_SubscribedMessage_IdTopicSubscription
	/// </summary>
	public Guid IdTopicSubscription { get; private set; }

	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid IdMessage { get; private set; }

	/// <summary>
	/// Database DataType: uuid NOT NULL | MessageBox.Model.MessageProcessingStatus.MessageProcessingStatus | FK_SubscribedMessage_IdMessageProcessingStatus
	/// </summary>
	public Guid IdMessageProcessingStatus { get; private set; }

	/// <summary>
	/// Database DataType: timestamp with time zone NOT NULL
	/// </summary>
	public DateTime AssignedUtc { get; private set; }

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
	/// Database DataType: uuid NOT NULL | MessageBox.Model.MessageBoxInstance.MessageBoxInstance | FK_SubscribedMessage_MessageBoxInstance
	/// </summary>
	public Guid IdMessageBoxInstance { get; private set; }


	/// <summary>
	/// _1:N Guid IdMessageBoxInstance | FK_SubscribedMessage_MessageBoxInstance
	/// </summary>
	public MessageBox.Model.MessageBoxInstance MessageBoxInstance { get; private set; }

	/// <summary>
	/// _1:N Guid IdMessageProcessingStatus | FK_SubscribedMessage_IdMessageProcessingStatus
	/// </summary>
	public MessageBox.Model.MessageProcessingStatus MessageProcessingStatus { get; private set; }

	/// <summary>
	/// _1:N Guid IdTopicSubscription | FK_SubscribedMessage_IdTopicSubscription
	/// </summary>
	public MessageBox.Model.TopicSubscription TopicSubscription { get; private set; }


	/// <summary>
	/// N:_1 MessageBox.Model.MessageProcessingLog.IdSubscribedMessage | FK_MessageProcessingLog_IdSubscribedMessage
	/// </summary>
	public IReadOnlyList<MessageBox.Model.MessageProcessingLog> MessageProcessingLogs => _messageProcessingLogs;

	private SubscribedMessage()
	{
		_messageProcessingLogs = [];
	}

	static SubscribedMessage()
	{
		DefaultDBValidator = SetDBValidatorRules(new ValidatorBuilder<SubscribedMessage>()).Build();
	}

	public Dictionary<string, object?> ToDictionary()
		=> new()
		{
			{ nameof(IdSubscribedMessage), IdSubscribedMessage },
			{ nameof(IdTopicSubscription), IdTopicSubscription },
			{ nameof(IdMessage), IdMessage },
			{ nameof(IdMessageProcessingStatus), IdMessageProcessingStatus },
			{ nameof(AssignedUtc), AssignedUtc },
			{ nameof(ProcessedUtc), ProcessedUtc },
			{ nameof(SuspendedUtc), SuspendedUtc },
			{ nameof(LastProcessingUtc), LastProcessingUtc },
			{ nameof(LastProcessingTimeoutUtc), LastProcessingTimeoutUtc },
			{ nameof(NextProcessingUtc), NextProcessingUtc },
			{ nameof(RetryCount), RetryCount },
			{ nameof(IdMessageBoxInstance), IdMessageBoxInstance },
		};

	public void TrimStringValuesToFitDatabaseMaxLengths(string? postfix = null)
	{
	}

	public override string? GetPrimaryKeyValue()
	{
		return IdSubscribedMessage.ToString();
	}

	public override string? ToString()
	{
		return IdSubscribedMessage.ToString();
	}

	public static ValidatorBuilder<SubscribedMessage> SetDBValidatorRules(ValidatorBuilder<SubscribedMessage> builder)
		=> builder
			.ForProperty(x => x.IdSubscribedMessage, v => v.NotDefaultOrEmpty())
			.ForProperty(x => x.IdTopicSubscription, v => v.NotDefaultOrEmpty(), (x, parent) => x.TopicSubscription == null)
			//.ForProperty(x => x.IdMessage, v => v.NotDefaultOrEmpty())
			.ForProperty(x => x.IdMessageProcessingStatus, v => v.NotDefaultOrEmpty(), (x, parent) => x.MessageProcessingStatus == null)
			//.ForProperty(x => x.AssignedUtc, v => v.NotDefaultOrEmpty())
			//.ForProperty(x => x.NextProcessingUtc, v => v.NotDefaultOrEmpty())
			//.ForProperty(x => x.RetryCount, v => v.NotDefaultOrEmpty())
			.ForProperty(x => x.IdMessageBoxInstance, v => v.NotDefaultOrEmpty(), (x, parent) => x.MessageBoxInstance == null)
		;
}
