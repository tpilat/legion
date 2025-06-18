using Legion.Validation;

namespace Legion.ADF.ESB.Orchestrations.Model;

public sealed partial class OrchestrationInstance : Orchestrations.OrchestrationsBaseEntity, Legion.Model.IEntity
{
	private List<Orchestrations.Model.OrchestrationStepInstance> _orchestrationStepInstances;

	public static IValidator<OrchestrationInstance> DefaultDBValidator { get; }

	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid IdOrchestrationInstance { get; private set; }

	/// <summary>
	/// Database DataType: uuid NOT NULL | Orchestrations.Model.Orchestration.Orchestration | FK_OrchestrationInstance_IdOrchestration
	/// </summary>
	public Guid IdOrchestration { get; private set; }

	/// <summary>
	/// Database DataType: uuid NOT NULL | Orchestrations.Model.OrchestrationStatus.OrchestrationStatus | FK_OrchestrationInstance_IdOrchestrationStatus
	/// </summary>
	public Guid IdOrchestrationStatus { get; private set; }

	/// <summary>
	/// Database DataType: timestamp without time zone NOT NULL
	/// </summary>
	public DateTime CreatedUtc { get; private set; }


	/// <summary>
	/// _1:N Guid IdOrchestration | FK_OrchestrationInstance_IdOrchestration
	/// </summary>
	public Orchestrations.Model.Orchestration Orchestration { get; private set; }

	/// <summary>
	/// _1:N Guid IdOrchestrationStatus | FK_OrchestrationInstance_IdOrchestrationStatus
	/// </summary>
	public Orchestrations.Model.OrchestrationStatus OrchestrationStatus { get; private set; }


	/// <summary>
	/// N:_1 Orchestrations.Model.OrchestrationStepInstance.IdOrchestration | FK_OrchestrationStepInstance_IdOrchestration
	/// </summary>
	public IReadOnlyList<Orchestrations.Model.OrchestrationStepInstance> OrchestrationStepInstances => _orchestrationStepInstances;

	private OrchestrationInstance()
	{
		_orchestrationStepInstances = [];
	}

	static OrchestrationInstance()
	{
		DefaultDBValidator = SetDBValidatorRules(new ValidatorBuilder<OrchestrationInstance>()).Build();
	}

	public override string? ToString()
	{
		return IdOrchestrationInstance.ToString();
	}

	public static ValidatorBuilder<OrchestrationInstance> SetDBValidatorRules(ValidatorBuilder<OrchestrationInstance> builder)
		=> builder
			.ForProperty(x => x.IdOrchestrationInstance, v => v.NotDefaultOrEmpty())
			.ForProperty(x => x.IdOrchestration, v => v.NotDefaultOrEmpty(), x => x.Orchestration == null)
			.ForProperty(x => x.IdOrchestrationStatus, v => v.NotDefaultOrEmpty(), x => x.OrchestrationStatus == null)
			//.ForProperty(x => x.CreatedUtc, v => v.NotDefaultOrEmpty())
		;
}
