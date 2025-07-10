using Legion.Validation;

namespace Legion.ADF.ServiceBus.Model;

public sealed partial class OrchestrationStepProcessingLog : ServiceBus.ServiceBusBaseEntity, Legion.Model.IEntity
{
	public static IValidator<OrchestrationStepProcessingLog> DefaultDBValidator { get; }

	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid IdOrchestrationStepProcessingLog { get; private set; }

	/// <summary>
	/// Database DataType: uuid NOT NULL | ServiceBus.Model.OrchestrationStepProcessing.OrchestrationStepProcessing | FK_OrchestrationStepProcessingLog_IdOrchStepProcessing
	/// </summary>
	public Guid IdOrchestrationStepProcessing { get; private set; }

	/// <summary>
	/// Database DataType: integer NOT NULL
	/// </summary>
	public int IdLogLevel { get; private set; }

	/// <summary>
	/// Database DataType: timestamp with time zone NOT NULL
	/// </summary>
	public DateTime CreatedUtc { get; private set; }

	/// <summary>
	/// Database DataType: uuid NOT NULL | ServiceBus.Model.OrchestrationStepProcessingStatus.OrchestrationStepProcessingStatus | FK_OrchestrationStepProcessingLog_IdOrchStepProcessingStatus
	/// </summary>
	public Guid IdOrchestrationStepProcessingStatus { get; private set; }

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
	/// Database DataType: uuid NULL
	/// </summary>
	public Guid? IdMessageProcessingLog { get; private set; }


	/// <summary>
	/// _1:N Guid IdOrchestrationStepProcessing | FK_OrchestrationStepProcessingLog_IdOrchStepProcessing
	/// </summary>
	public ServiceBus.Model.OrchestrationStepProcessing OrchestrationStepProcessing { get; private set; }

	/// <summary>
	/// _1:N Guid IdOrchestrationStepProcessingStatus | FK_OrchestrationStepProcessingLog_IdOrchStepProcessingStatus
	/// </summary>
	public ServiceBus.Model.OrchestrationStepProcessingStatus OrchestrationStepProcessingStatus { get; private set; }

	private OrchestrationStepProcessingLog()
	{
	}

	static OrchestrationStepProcessingLog()
	{
		DefaultDBValidator = SetDBValidatorRules(new ValidatorBuilder<OrchestrationStepProcessingLog>()).Build();
	}

	public Dictionary<string, object?> ToDictionary()
		=> new()
		{
			{ nameof(IdOrchestrationStepProcessingLog), IdOrchestrationStepProcessingLog },
			{ nameof(IdOrchestrationStepProcessing), IdOrchestrationStepProcessing },
			{ nameof(IdLogLevel), IdLogLevel },
			{ nameof(CreatedUtc), CreatedUtc },
			{ nameof(IdOrchestrationStepProcessingStatus), IdOrchestrationStepProcessingStatus },
			{ nameof(TraceCorrelationId), TraceCorrelationId },
			{ nameof(IdLogMessage), IdLogMessage },
			{ nameof(Code), Code },
			{ nameof(Detail), Detail },
			{ nameof(IdMessageProcessingLog), IdMessageProcessingLog },
		};

	public void TrimStringValuesToFitDatabaseMaxLengths(string? postfix = null)
	{
		Code = Legion.Text.StringHelper.TrimToFitMaxLength(Code, 127, postfix);
	}

	public override string? GetPrimaryKeyValue()
	{
		return IdOrchestrationStepProcessingLog.ToString();
	}

	public override string? ToString()
	{
		return IdOrchestrationStepProcessingLog.ToString();
	}

	public static ValidatorBuilder<OrchestrationStepProcessingLog> SetDBValidatorRules(ValidatorBuilder<OrchestrationStepProcessingLog> builder)
		=> builder
			.ForProperty(x => x.IdOrchestrationStepProcessingLog, v => v.NotDefaultOrEmpty())
			.ForProperty(x => x.IdOrchestrationStepProcessing, v => v.NotDefaultOrEmpty(), (x, parent) => x.OrchestrationStepProcessing == null)
			//.ForProperty(x => x.IdLogLevel, v => v.NotDefaultOrEmpty())
			//.ForProperty(x => x.CreatedUtc, v => v.NotDefaultOrEmpty())
			.ForProperty(x => x.IdOrchestrationStepProcessingStatus, v => v.NotDefaultOrEmpty(), (x, parent) => x.OrchestrationStepProcessingStatus == null)
			//.ForProperty(x => x.TraceCorrelationId, v => v.NotDefaultOrEmpty())
			.ForProperty(x => x.Code, v => v.NotDefaultOrEmpty().MaxLength(127))
		;
}
