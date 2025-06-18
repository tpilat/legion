using Legion.Validation;

namespace Legion.ADF.ESB.Orchestrations.Model;

public sealed partial class OrchestrationStatus : Orchestrations.OrchestrationsBaseEntity, Legion.Model.IEntity
{
	private List<Orchestrations.Model.OrchestrationInstance> _orchestrationInstances;

	public static IValidator<OrchestrationStatus> DefaultDBValidator { get; }

	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid IdOrchestrationStatus { get; private set; }

	/// <summary>
	/// Database DataType: varchar(63) NOT NULL
	/// </summary>
	public string Code { get; private set; }

	/// <summary>
	/// Database DataType: varchar(127) NOT NULL
	/// </summary>
	public string Name { get; private set; }


	/// <summary>
	/// N:_1 Orchestrations.Model.OrchestrationInstance.IdOrchestrationStatus | FK_OrchestrationInstance_IdOrchestrationStatus
	/// </summary>
	public IReadOnlyList<Orchestrations.Model.OrchestrationInstance> OrchestrationInstances => _orchestrationInstances;

	private OrchestrationStatus()
	{
		_orchestrationInstances = [];
	}

	public override string? ToString()
	{
		return Code;
	}

	public static ValidatorBuilder<OrchestrationStatus> SetDBValidatorRules(ValidatorBuilder<OrchestrationStatus> builder)
		=> builder
			.ForProperty(x => x.IdOrchestrationStatus, v => v.NotDefaultOrEmpty())
			.ForProperty(x => x.Code, v => v.NotDefaultOrEmpty().MaxLength(63))
			.ForProperty(x => x.Name, v => v.NotDefaultOrEmpty().MaxLength(127))
		;
}
