using Legion.Validation;

namespace Legion.ADF.ESB.Orchestrations.Model;

public sealed partial class Orchestration : Orchestrations.OrchestrationsBaseEntity, Legion.Model.IEntity
{
	private List<Orchestrations.Model.OrchestrationInstance> _orchestrationInstances;
	private List<Orchestrations.Model.OrchestrationStep> _orchestrationSteps;

	public static IValidator<Orchestration> DefaultDBValidator { get; }

	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid IdOrchestration { get; private set; }

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
	/// Database DataType: boolean NOT NULL
	/// </summary>
	public bool IsSingleton { get; private set; }

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
	public int TimeoutForMessageProcessingInSeconds { get; private set; }

	/// <summary>
	/// Database DataType: integer NOT NULL
	/// </summary>
	public int MaxMessageProcessingRetryCount { get; private set; }

	/// <summary>
	/// Database DataType: varchar(31) NOT NULL
	/// </summary>
	public string Version { get; private set; }

	/// <summary>
	/// Database DataType: timestamp without time zone NULL
	/// </summary>
	public DateTime? ValidTo { get; private set; }


	/// <summary>
	/// N:_1 Orchestrations.Model.OrchestrationInstance.IdOrchestration | FK_OrchestrationInstance_IdOrchestration
	/// </summary>
	public IReadOnlyList<Orchestrations.Model.OrchestrationInstance> OrchestrationInstances => _orchestrationInstances;

	/// <summary>
	/// N:_1 Orchestrations.Model.OrchestrationStep.IdOrchestration | FK_OrchestrationStep_IdOrchestration
	/// </summary>
	public IReadOnlyList<Orchestrations.Model.OrchestrationStep> OrchestrationSteps => _orchestrationSteps;

	private Orchestration()
	{
		_orchestrationInstances = [];
		_orchestrationSteps = [];
	}

	static Orchestration()
	{
		DefaultDBValidator = SetDBValidatorRules(new ValidatorBuilder<Orchestration>()).Build();
	}

	public override string? ToString()
	{
		return IdOrchestration.ToString();
	}

	public static ValidatorBuilder<Orchestration> SetDBValidatorRules(ValidatorBuilder<Orchestration> builder)
		=> builder
			.ForProperty(x => x.IdOrchestration, v => v.NotDefaultOrEmpty())
			.ForProperty(x => x.Code, v => v.NotDefaultOrEmpty().MaxLength(127))
			.ForProperty(x => x.Name, v => v.NotDefaultOrEmpty().MaxLength(127))
			.ForProperty(x => x.Description, v => v.MaxLength(1023))
			.ForProperty(x => x.Class, v => v.NotDefaultOrEmpty().MaxLength(2047))
			//.ForProperty(x => x.TimeoutForMessageProcessingInSeconds, v => v.NotDefaultOrEmpty())
			//.ForProperty(x => x.MaxMessageProcessingRetryCount, v => v.NotDefaultOrEmpty())
			.ForProperty(x => x.Version, v => v.NotDefaultOrEmpty().MaxLength(31))
		;
}
