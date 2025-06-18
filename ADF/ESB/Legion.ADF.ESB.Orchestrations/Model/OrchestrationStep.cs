using Legion.Validation;

namespace Legion.ADF.ESB.Orchestrations.Model;

public sealed partial class OrchestrationStep : Orchestrations.OrchestrationsBaseEntity, Legion.Model.IEntity
{
	private List<Orchestrations.Model.OrchestrationStepInstance> _orchestrationStepInstances;

	public static IValidator<OrchestrationStep> DefaultDBValidator { get; }

	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid IdOrchestrationStep { get; private set; }

	/// <summary>
	/// Database DataType: uuid NOT NULL | Orchestrations.Model.Orchestration.Orchestration | FK_OrchestrationStep_IdOrchestration
	/// </summary>
	public Guid IdOrchestration { get; private set; }

	/// <summary>
	/// Database DataType: boolean NOT NULL
	/// </summary>
	public bool IsMainEntry { get; private set; }

	/// <summary>
	/// Database DataType: varchar(127) NOT NULL
	/// </summary>
	public string Code { get; private set; }

	/// <summary>
	/// Database DataType: varchar(127) NOT NULL
	/// </summary>
	public string Name { get; private set; }

	/// <summary>
	/// Database DataType: varchar(1023) NULL
	/// </summary>
	public string? Description { get; private set; }

	/// <summary>
	/// Database DataType: varchar(2047) NOT NULL
	/// </summary>
	public string Class { get; private set; }

	/// <summary>
	/// Database DataType: jsonb NULL
	/// </summary>
	public string? Properties { get; private set; }

	/// <summary>
	/// Database DataType: integer NOT NULL
	/// </summary>
	public int Order { get; private set; }


	/// <summary>
	/// _1:N Guid IdOrchestration | FK_OrchestrationStep_IdOrchestration
	/// </summary>
	public Orchestrations.Model.Orchestration Orchestration { get; private set; }


	/// <summary>
	/// N:_1 Orchestrations.Model.OrchestrationStepInstance.IdOrchestrationStep | FK_OrchestrationStepInstance_IdOrchestrationStep
	/// </summary>
	public IReadOnlyList<Orchestrations.Model.OrchestrationStepInstance> OrchestrationStepInstances => _orchestrationStepInstances;

	private OrchestrationStep()
	{
		_orchestrationStepInstances = [];
	}

	static OrchestrationStep()
	{
		DefaultDBValidator = SetDBValidatorRules(new ValidatorBuilder<OrchestrationStep>()).Build();
	}

	public override string? ToString()
	{
		return IdOrchestrationStep.ToString();
	}

	public static ValidatorBuilder<OrchestrationStep> SetDBValidatorRules(ValidatorBuilder<OrchestrationStep> builder)
		=> builder
			.ForProperty(x => x.IdOrchestrationStep, v => v.NotDefaultOrEmpty())
			.ForProperty(x => x.IdOrchestration, v => v.NotDefaultOrEmpty(), x => x.Orchestration == null)
			.ForProperty(x => x.Code, v => v.NotDefaultOrEmpty().MaxLength(127))
			.ForProperty(x => x.Name, v => v.NotDefaultOrEmpty().MaxLength(127))
			.ForProperty(x => x.Description, v => v.MaxLength(1023))
			.ForProperty(x => x.Class, v => v.NotDefaultOrEmpty().MaxLength(2047))
			//.ForProperty(x => x.Order, v => v.NotDefaultOrEmpty())
		;
}
