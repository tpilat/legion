using Legion.Validation;

namespace Legion.ADF.ESB.Orchestrations.Model;

public sealed partial class OrchestrationStepInstance : Orchestrations.OrchestrationsBaseEntity, Legion.Model.IEntity
{
	private List<Orchestrations.Model.OrchestrationStepLog> _orchestrationStepLogs;
	private List<Orchestrations.Model.StepDirection> _stepDirections;
	private List<Orchestrations.Model.StepDirection> _toStepStepDirections;

	public static IValidator<OrchestrationStepInstance> DefaultDBValidator { get; }

	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid IdOrchestrationStepInstance { get; private set; }

	/// <summary>
	/// Database DataType: uuid NOT NULL | Orchestrations.Model.OrchestrationInstance.Orchestration | FK_OrchestrationStepInstance_IdOrchestration
	/// </summary>
	public Guid IdOrchestration { get; private set; }

	/// <summary>
	/// Database DataType: uuid NOT NULL | Orchestrations.Model.OrchestrationStep.OrchestrationStep | FK_OrchestrationStepInstance_IdOrchestrationStep
	/// </summary>
	public Guid IdOrchestrationStep { get; private set; }

	/// <summary>
	/// Database DataType: uuid NOT NULL | Orchestrations.Model.OrchestrationStepStatus.StepStatus | FK_OrchestrationStepInstance_IdStepStatus
	/// </summary>
	public Guid IdStepStatus { get; private set; }

	/// <summary>
	/// Database DataType: timestamp without time zone NULL
	/// </summary>
	public DateTime? LastProcessedUtc { get; private set; }

	/// <summary>
	/// Database DataType: timestamp without time zone NULL
	/// </summary>
	public DateTime? NextProcessingUtc { get; private set; }

	/// <summary>
	/// Database DataType: integer NOT NULL
	/// </summary>
	public int RetryCount { get; private set; }

	/// <summary>
	/// Database DataType: timestamp without time zone NULL
	/// </summary>
	public DateTime? SucceededUtc { get; private set; }

	/// <summary>
	/// Database DataType: timestamp without time zone NULL
	/// </summary>
	public DateTime? SuspendedUtc { get; private set; }


	/// <summary>
	/// _1:N Guid IdOrchestration | FK_OrchestrationStepInstance_IdOrchestration
	/// </summary>
	public Orchestrations.Model.OrchestrationInstance Orchestration { get; private set; }

	/// <summary>
	/// _1:N Guid IdOrchestrationStep | FK_OrchestrationStepInstance_IdOrchestrationStep
	/// </summary>
	public Orchestrations.Model.OrchestrationStep OrchestrationStep { get; private set; }

	/// <summary>
	/// _1:N Guid IdStepStatus | FK_OrchestrationStepInstance_IdStepStatus
	/// </summary>
	public Orchestrations.Model.OrchestrationStepStatus StepStatus { get; private set; }


	/// <summary>
	/// N:_1 Orchestrations.Model.OrchestrationStepLog.IdOrchestrationStepInstance | FK_OrchestrationStepLog_IdOrchestrationStepInstance
	/// </summary>
	public IReadOnlyList<Orchestrations.Model.OrchestrationStepLog> OrchestrationStepLogs => _orchestrationStepLogs;

	/// <summary>
	/// N:_1 Orchestrations.Model.StepDirection.IdFromStep | FK_StepDirection_IdFromStep
	/// </summary>
	public IReadOnlyList<Orchestrations.Model.StepDirection> StepDirections => _stepDirections;

	/// <summary>
	/// N:_1 Orchestrations.Model.StepDirection.IdToStep | FK_StepDirection_IdToStep
	/// </summary>
	public IReadOnlyList<Orchestrations.Model.StepDirection> ToStepStepDirections => _toStepStepDirections;

	private OrchestrationStepInstance()
	{
		_orchestrationStepLogs = [];
		_stepDirections = [];
		_toStepStepDirections = [];
	}

	static OrchestrationStepInstance()
	{
		DefaultDBValidator = SetDBValidatorRules(new ValidatorBuilder<OrchestrationStepInstance>()).Build();
	}

	public override string? ToString()
	{
		return IdOrchestrationStepInstance.ToString();
	}

	public static ValidatorBuilder<OrchestrationStepInstance> SetDBValidatorRules(ValidatorBuilder<OrchestrationStepInstance> builder)
		=> builder
			.ForProperty(x => x.IdOrchestrationStepInstance, v => v.NotDefaultOrEmpty())
			.ForProperty(x => x.IdOrchestration, v => v.NotDefaultOrEmpty(), x => x.Orchestration == null)
			.ForProperty(x => x.IdOrchestrationStep, v => v.NotDefaultOrEmpty(), x => x.OrchestrationStep == null)
			.ForProperty(x => x.IdStepStatus, v => v.NotDefaultOrEmpty(), x => x.StepStatus == null)
			//.ForProperty(x => x.RetryCount, v => v.NotDefaultOrEmpty())
		;
}
