using Legion.Validation;

namespace Legion.ADF.ESB.Orchestrations.Model;

public sealed partial class OrchestrationStepStatus : Orchestrations.OrchestrationsBaseEntity, Legion.Model.IEntity
{
	private List<Orchestrations.Model.OrchestrationStepInstance> _orchestrationStepInstances;
	private List<Orchestrations.Model.OrchestrationStepLog> _orchestrationStepLogs;

	public static IValidator<OrchestrationStepStatus> DefaultDBValidator { get; }

	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid IdOrchestrationStepStatus { get; private set; }

	/// <summary>
	/// Database DataType: varchar(63) NOT NULL
	/// </summary>
	public string Code { get; private set; }

	/// <summary>
	/// Database DataType: varchar(127) NOT NULL
	/// </summary>
	public string Name { get; private set; }


	/// <summary>
	/// N:_1 Orchestrations.Model.OrchestrationStepInstance.IdStepStatus | FK_OrchestrationStepInstance_IdStepStatus
	/// </summary>
	public IReadOnlyList<Orchestrations.Model.OrchestrationStepInstance> OrchestrationStepInstances => _orchestrationStepInstances;

	/// <summary>
	/// N:_1 Orchestrations.Model.OrchestrationStepLog.IdStepStatus | FK_OrchestrationStepLog_IdStepStatus
	/// </summary>
	public IReadOnlyList<Orchestrations.Model.OrchestrationStepLog> OrchestrationStepLogs => _orchestrationStepLogs;

	private OrchestrationStepStatus()
	{
		_orchestrationStepInstances = [];
		_orchestrationStepLogs = [];
	}

	public override string? ToString()
	{
		return Code;
	}

	public static ValidatorBuilder<OrchestrationStepStatus> SetDBValidatorRules(ValidatorBuilder<OrchestrationStepStatus> builder)
		=> builder
			.ForProperty(x => x.IdOrchestrationStepStatus, v => v.NotDefaultOrEmpty())
			.ForProperty(x => x.Code, v => v.NotDefaultOrEmpty().MaxLength(63))
			.ForProperty(x => x.Name, v => v.NotDefaultOrEmpty().MaxLength(127))
		;
}
