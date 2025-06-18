using Legion.Validation;

namespace Legion.ADF.Messaging.Outbox.Model;

public sealed partial class OutboxProcessingLog : Outbox.OutboxBaseEntity, Legion.Model.IEntity
{
	public static IValidator<OutboxProcessingLog> DefaultDBValidator { get; }

	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid IdOutboxProcessingLog { get; private set; }

	/// <summary>
	/// Database DataType: uuid NOT NULL | Outbox.Model.OutboxInstance.OutboxInstance | FK_OutboxProcessingLog_IdOutboxInstance
	/// </summary>
	public Guid IdOutboxInstance { get; private set; }

	/// <summary>
	/// Database DataType: uuid NULL | Outbox.Model.OutboxQueue.OutboxQueue | FK_OutboxProcessingLog_IdOutboxQueue
	/// </summary>
	public Guid? IdOutboxQueue { get; private set; }

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
	/// _1:N Guid IdOutboxInstance | FK_OutboxProcessingLog_IdOutboxInstance
	/// </summary>
	public Outbox.Model.OutboxInstance OutboxInstance { get; private set; }

	/// <summary>
	/// _1:N Guid? IdOutboxQueue | FK_OutboxProcessingLog_IdOutboxQueue
	/// </summary>
	public Outbox.Model.OutboxQueue OutboxQueue { get; private set; }

	private OutboxProcessingLog()
	{
	}

	static OutboxProcessingLog()
	{
		DefaultDBValidator = SetDBValidatorRules(new ValidatorBuilder<OutboxProcessingLog>()).Build();
	}

	public Dictionary<string, object?> ToDictionary()
		=> new()
		{
			{ nameof(IdOutboxProcessingLog), IdOutboxProcessingLog },
			{ nameof(IdOutboxInstance), IdOutboxInstance },
			{ nameof(IdOutboxQueue), IdOutboxQueue },
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
		return IdOutboxProcessingLog.ToString();
	}

	public override string? ToString()
	{
		return IdOutboxProcessingLog.ToString();
	}

	public static ValidatorBuilder<OutboxProcessingLog> SetDBValidatorRules(ValidatorBuilder<OutboxProcessingLog> builder)
		=> builder
			.ForProperty(x => x.IdOutboxProcessingLog, v => v.NotDefaultOrEmpty())
			.ForProperty(x => x.IdOutboxInstance, v => v.NotDefaultOrEmpty(), (x, parent) => x.OutboxInstance == null)
			//.ForProperty(x => x.CreatedUtc, v => v.NotDefaultOrEmpty())
			//.ForProperty(x => x.IdLogLevel, v => v.NotDefaultOrEmpty())
			//.ForProperty(x => x.TraceCorrelationId, v => v.NotDefaultOrEmpty())
			.ForProperty(x => x.Code, v => v.NotDefaultOrEmpty().MaxLength(127))
		;
}
