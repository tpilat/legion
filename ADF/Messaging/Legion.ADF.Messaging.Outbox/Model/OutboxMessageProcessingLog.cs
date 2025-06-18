using Legion.Validation;

namespace Legion.ADF.Messaging.Outbox.Model;

public sealed partial class OutboxMessageProcessingLog : Outbox.OutboxBaseEntity, Legion.Model.IEntity
{
	public static IValidator<OutboxMessageProcessingLog> DefaultDBValidator { get; }

	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid IdOutboxMessageProcessingLog { get; private set; }

	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid IdOutboxMessage { get; private set; }

	/// <summary>
	/// Database DataType: uuid NOT NULL | Outbox.Model.OutboxQueue.OutboxQueue | FK_OutboxMessageProcessingLog_IdOutboxQueue
	/// </summary>
	public Guid IdOutboxQueue { get; private set; }

	/// <summary>
	/// Database DataType: timestamp with time zone NOT NULL
	/// </summary>
	public DateTime CreatedUtc { get; private set; }

	/// <summary>
	/// Database DataType: uuid NOT NULL | Outbox.Model.OutboxMessageStatus.OutboxMessageStatus | FK_OutboxMessageProcessingLog_IdOutboxMessageStatus
	/// </summary>
	public Guid IdOutboxMessageStatus { get; private set; }

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
	/// Database DataType: uuid NOT NULL | Outbox.Model.OutboxInstance.OutboxInstance | FK_OutboxMessageProcessingLog_IdOutboxInstance
	/// </summary>
	public Guid IdOutboxInstance { get; private set; }


	/// <summary>
	/// _1:N Guid IdOutboxInstance | FK_OutboxMessageProcessingLog_IdOutboxInstance
	/// </summary>
	public Outbox.Model.OutboxInstance OutboxInstance { get; private set; }

	/// <summary>
	/// _1:N Guid IdOutboxMessageStatus | FK_OutboxMessageProcessingLog_IdOutboxMessageStatus
	/// </summary>
	public Outbox.Model.OutboxMessageStatus OutboxMessageStatus { get; private set; }

	/// <summary>
	/// _1:N Guid IdOutboxQueue | FK_OutboxMessageProcessingLog_IdOutboxQueue
	/// </summary>
	public Outbox.Model.OutboxQueue OutboxQueue { get; private set; }

	private OutboxMessageProcessingLog()
	{
	}

	static OutboxMessageProcessingLog()
	{
		DefaultDBValidator = SetDBValidatorRules(new ValidatorBuilder<OutboxMessageProcessingLog>()).Build();
	}

	public Dictionary<string, object?> ToDictionary()
		=> new()
		{
			{ nameof(IdOutboxMessageProcessingLog), IdOutboxMessageProcessingLog },
			{ nameof(IdOutboxMessage), IdOutboxMessage },
			{ nameof(IdOutboxQueue), IdOutboxQueue },
			{ nameof(CreatedUtc), CreatedUtc },
			{ nameof(IdOutboxMessageStatus), IdOutboxMessageStatus },
			{ nameof(TraceCorrelationId), TraceCorrelationId },
			{ nameof(IdLogMessage), IdLogMessage },
			{ nameof(Code), Code },
			{ nameof(Detail), Detail },
			{ nameof(IdOutboxInstance), IdOutboxInstance },
		};

	public void TrimStringValuesToFitDatabaseMaxLengths(string? postfix = null)
	{
		Code = Legion.Text.StringHelper.TrimToFitMaxLength(Code, 127, postfix);
	}

	public override string? GetPrimaryKeyValue()
	{
		return IdOutboxMessageProcessingLog.ToString();
	}

	public override string? ToString()
	{
		return IdOutboxMessageProcessingLog.ToString();
	}

	public static ValidatorBuilder<OutboxMessageProcessingLog> SetDBValidatorRules(ValidatorBuilder<OutboxMessageProcessingLog> builder)
		=> builder
			.ForProperty(x => x.IdOutboxMessageProcessingLog, v => v.NotDefaultOrEmpty())
			//.ForProperty(x => x.IdOutboxMessage, v => v.NotDefaultOrEmpty())
			.ForProperty(x => x.IdOutboxQueue, v => v.NotDefaultOrEmpty(), (x, parent) => x.OutboxQueue == null)
			//.ForProperty(x => x.CreatedUtc, v => v.NotDefaultOrEmpty())
			.ForProperty(x => x.IdOutboxMessageStatus, v => v.NotDefaultOrEmpty(), (x, parent) => x.OutboxMessageStatus == null)
			//.ForProperty(x => x.TraceCorrelationId, v => v.NotDefaultOrEmpty())
			.ForProperty(x => x.Code, v => v.NotDefaultOrEmpty().MaxLength(127))
			.ForProperty(x => x.IdOutboxInstance, v => v.NotDefaultOrEmpty(), (x, parent) => x.OutboxInstance == null)
		;
}
