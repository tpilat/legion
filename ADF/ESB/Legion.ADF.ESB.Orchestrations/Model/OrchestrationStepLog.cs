using Legion.Validation;

namespace Legion.ADF.ESB.Orchestrations.Model;

public sealed partial class OrchestrationStepLog : Orchestrations.OrchestrationsBaseEntity, Legion.Model.IEntity
{
	public static IValidator<OrchestrationStepLog> DefaultDBValidator { get; }

	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid IdOrchestrationStepLog { get; private set; }

	/// <summary>
	/// Database DataType: uuid NOT NULL | Orchestrations.Model.OrchestrationStepInstance.OrchestrationStepInstance | FK_OrchestrationStepLog_IdOrchestrationStepInstance
	/// </summary>
	public Guid IdOrchestrationStepInstance { get; private set; }

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
	/// Database DataType: uuid NOT NULL | Orchestrations.Model.OrchestrationStepStatus.StepStatus | FK_OrchestrationStepLog_IdStepStatus
	/// </summary>
	public Guid IdStepStatus { get; private set; }

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
	/// Database DataType: uuid NULL | --NO TARGET-- | FK_OrchestrationStepLog_IdMessageProcessingLog
	/// </summary>
	public Guid? IdMessageProcessingLog { get; private set; }


	/// <summary>
	/// _1:N Guid IdOrchestrationStepInstance | FK_OrchestrationStepLog_IdOrchestrationStepInstance
	/// </summary>
	public Orchestrations.Model.OrchestrationStepInstance OrchestrationStepInstance { get; private set; }

	/// <summary>
	/// _1:N Guid IdStepStatus | FK_OrchestrationStepLog_IdStepStatus
	/// </summary>
	public Orchestrations.Model.OrchestrationStepStatus StepStatus { get; private set; }

	private OrchestrationStepLog()
	{
	}

	static OrchestrationStepLog()
	{
		DefaultDBValidator = SetDBValidatorRules(new ValidatorBuilder<OrchestrationStepLog>()).Build();
	}

	public override string? ToString()
	{
		return IdOrchestrationStepLog.ToString();
	}

	public static ValidatorBuilder<OrchestrationStepLog> SetDBValidatorRules(ValidatorBuilder<OrchestrationStepLog> builder)
		=> builder
			.ForProperty(x => x.IdOrchestrationStepLog, v => v.NotDefaultOrEmpty())
			.ForProperty(x => x.IdOrchestrationStepInstance, v => v.NotDefaultOrEmpty(), x => x.OrchestrationStepInstance == null)
			//.ForProperty(x => x.CreatedUtc, v => v.NotDefaultOrEmpty())
			//.ForProperty(x => x.IdLogLevel, v => v.NotDefaultOrEmpty())
			//.ForProperty(x => x.LogCorrelationId, v => v.NotDefaultOrEmpty())
			.ForProperty(x => x.IdStepStatus, v => v.NotDefaultOrEmpty(), x => x.StepStatus == null)
			.ForProperty(x => x.Detail, v => v.NotDefaultOrEmpty())
		;
}
