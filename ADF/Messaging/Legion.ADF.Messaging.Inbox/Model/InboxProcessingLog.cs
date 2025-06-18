using Legion.Validation;

namespace Legion.ADF.Messaging.Inbox.Model;

public sealed partial class InboxProcessingLog : Inbox.InboxBaseEntity, Legion.Model.IEntity
{
	public static IValidator<InboxProcessingLog> DefaultDBValidator { get; }

	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid IdInboxProcessingLog { get; private set; }

	/// <summary>
	/// Database DataType: uuid NOT NULL | Inbox.Model.InboxInstance.InboxInstance | FK_InboxProcessingLog_IdInboxInstance
	/// </summary>
	public Guid IdInboxInstance { get; private set; }

	/// <summary>
	/// Database DataType: uuid NULL | Inbox.Model.InboxQueue.InboxQueue | FK_InboxProcessingLog_IdInboxQueue
	/// </summary>
	public Guid? IdInboxQueue { get; private set; }

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
	/// _1:N Guid IdInboxInstance | FK_InboxProcessingLog_IdInboxInstance
	/// </summary>
	public Inbox.Model.InboxInstance InboxInstance { get; private set; }

	/// <summary>
	/// _1:N Guid? IdInboxQueue | FK_InboxProcessingLog_IdInboxQueue
	/// </summary>
	public Inbox.Model.InboxQueue InboxQueue { get; private set; }

	private InboxProcessingLog()
	{
	}

	static InboxProcessingLog()
	{
		DefaultDBValidator = SetDBValidatorRules(new ValidatorBuilder<InboxProcessingLog>()).Build();
	}

	public Dictionary<string, object?> ToDictionary()
		=> new()
		{
			{ nameof(IdInboxProcessingLog), IdInboxProcessingLog },
			{ nameof(IdInboxInstance), IdInboxInstance },
			{ nameof(IdInboxQueue), IdInboxQueue },
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
		return IdInboxProcessingLog.ToString();
	}

	public override string? ToString()
	{
		return IdInboxProcessingLog.ToString();
	}

	public static ValidatorBuilder<InboxProcessingLog> SetDBValidatorRules(ValidatorBuilder<InboxProcessingLog> builder)
		=> builder
			.ForProperty(x => x.IdInboxProcessingLog, v => v.NotDefaultOrEmpty())
			.ForProperty(x => x.IdInboxInstance, v => v.NotDefaultOrEmpty(), (x, parent) => x.InboxInstance == null)
			//.ForProperty(x => x.CreatedUtc, v => v.NotDefaultOrEmpty())
			//.ForProperty(x => x.IdLogLevel, v => v.NotDefaultOrEmpty())
			//.ForProperty(x => x.TraceCorrelationId, v => v.NotDefaultOrEmpty())
			.ForProperty(x => x.Code, v => v.NotDefaultOrEmpty().MaxLength(127))
		;
}
