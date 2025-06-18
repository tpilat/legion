using Legion.Validation;

namespace Legion.ADF.Messaging.Inbox.Model;

public sealed partial class InboxMessageProcessingLog : Inbox.InboxBaseEntity, Legion.Model.IEntity
{
	public static IValidator<InboxMessageProcessingLog> DefaultDBValidator { get; }

	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid IdInboxMessageProcessingLog { get; private set; }

	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid IdInboxMessage { get; private set; }

	/// <summary>
	/// Database DataType: uuid NOT NULL | Inbox.Model.InboxQueue.InboxQueue | FK_InboxMessageProcessingLog_IdInboxQueue
	/// </summary>
	public Guid IdInboxQueue { get; private set; }

	/// <summary>
	/// Database DataType: timestamp with time zone NOT NULL
	/// </summary>
	public DateTime CreatedUtc { get; private set; }

	/// <summary>
	/// Database DataType: uuid NOT NULL | Inbox.Model.InboxMessageStatus.InboxMessageStatus | FK_InboxMessageProcessingLog_IdInboxMessageStatus
	/// </summary>
	public Guid IdInboxMessageStatus { get; private set; }

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
	/// Database DataType: uuid NOT NULL | Inbox.Model.InboxInstance.InboxInstance | FK_InboxMessageProcessingLog_IdInboxInstance
	/// </summary>
	public Guid IdInboxInstance { get; private set; }


	/// <summary>
	/// _1:N Guid IdInboxInstance | FK_InboxMessageProcessingLog_IdInboxInstance
	/// </summary>
	public Inbox.Model.InboxInstance InboxInstance { get; private set; }

	/// <summary>
	/// _1:N Guid IdInboxMessageStatus | FK_InboxMessageProcessingLog_IdInboxMessageStatus
	/// </summary>
	public Inbox.Model.InboxMessageStatus InboxMessageStatus { get; private set; }

	/// <summary>
	/// _1:N Guid IdInboxQueue | FK_InboxMessageProcessingLog_IdInboxQueue
	/// </summary>
	public Inbox.Model.InboxQueue InboxQueue { get; private set; }

	private InboxMessageProcessingLog()
	{
	}

	static InboxMessageProcessingLog()
	{
		DefaultDBValidator = SetDBValidatorRules(new ValidatorBuilder<InboxMessageProcessingLog>()).Build();
	}

	public Dictionary<string, object?> ToDictionary()
		=> new()
		{
			{ nameof(IdInboxMessageProcessingLog), IdInboxMessageProcessingLog },
			{ nameof(IdInboxMessage), IdInboxMessage },
			{ nameof(IdInboxQueue), IdInboxQueue },
			{ nameof(CreatedUtc), CreatedUtc },
			{ nameof(IdInboxMessageStatus), IdInboxMessageStatus },
			{ nameof(TraceCorrelationId), TraceCorrelationId },
			{ nameof(IdLogMessage), IdLogMessage },
			{ nameof(Code), Code },
			{ nameof(Detail), Detail },
			{ nameof(IdInboxInstance), IdInboxInstance },
		};

	public void TrimStringValuesToFitDatabaseMaxLengths(string? postfix = null)
	{
		Code = Legion.Text.StringHelper.TrimToFitMaxLength(Code, 127, postfix);
	}

	public override string? GetPrimaryKeyValue()
	{
		return IdInboxMessageProcessingLog.ToString();
	}

	public override string? ToString()
	{
		return IdInboxMessageProcessingLog.ToString();
	}

	public static ValidatorBuilder<InboxMessageProcessingLog> SetDBValidatorRules(ValidatorBuilder<InboxMessageProcessingLog> builder)
		=> builder
			.ForProperty(x => x.IdInboxMessageProcessingLog, v => v.NotDefaultOrEmpty())
			//.ForProperty(x => x.IdInboxMessage, v => v.NotDefaultOrEmpty())
			.ForProperty(x => x.IdInboxQueue, v => v.NotDefaultOrEmpty(), (x, parent) => x.InboxQueue == null)
			//.ForProperty(x => x.CreatedUtc, v => v.NotDefaultOrEmpty())
			.ForProperty(x => x.IdInboxMessageStatus, v => v.NotDefaultOrEmpty(), (x, parent) => x.InboxMessageStatus == null)
			//.ForProperty(x => x.TraceCorrelationId, v => v.NotDefaultOrEmpty())
			.ForProperty(x => x.Code, v => v.NotDefaultOrEmpty().MaxLength(127))
			.ForProperty(x => x.IdInboxInstance, v => v.NotDefaultOrEmpty(), (x, parent) => x.InboxInstance == null)
		;
}
