using Legion.Validation;

namespace Legion.ADF.ServiceBus.Model;

public sealed partial class OrchestrationInstance : ServiceBus.ServiceBusBaseEntity, Legion.Model.IEntity
{
	private List<ServiceBus.Model.OrchestrationStepProcessing> _orchestrationStepProcessings;

	public static IValidator<OrchestrationInstance> DefaultDBValidator { get; }

	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid IdOrchestrationInstance { get; private set; }

	/// <summary>
	/// Database DataType: uuid NOT NULL | ServiceBus.Model.Orchestration.Orchestration | FK_OrchestrationInstance_IdOrchestration
	/// </summary>
	public Guid IdOrchestration { get; private set; }

	/// <summary>
	/// Database DataType: uuid NOT NULL | ServiceBus.Model.OrchestrationStatus.OrchestrationStatus | FK_OrchestrationInstance_IdOrchestrationStatus
	/// </summary>
	public Guid IdOrchestrationStatus { get; private set; }

	/// <summary>
	/// Database DataType: timestamp with time zone NOT NULL
	/// </summary>
	public DateTime CreatedUtc { get; private set; }


	/// <summary>
	/// _1:N Guid IdOrchestration | FK_OrchestrationInstance_IdOrchestration
	/// </summary>
	public ServiceBus.Model.Orchestration Orchestration { get; private set; }

	/// <summary>
	/// _1:N Guid IdOrchestrationStatus | FK_OrchestrationInstance_IdOrchestrationStatus
	/// </summary>
	public ServiceBus.Model.OrchestrationStatus OrchestrationStatus { get; private set; }


	/// <summary>
	/// N:_1 ServiceBus.Model.OrchestrationStepProcessing.IdOrchestrationInstance | FK_OrchestrationStepProcessing_IdOrchestrationInstance
	/// </summary>
	public IReadOnlyList<ServiceBus.Model.OrchestrationStepProcessing> OrchestrationStepProcessings => _orchestrationStepProcessings;

	private OrchestrationInstance()
	{
		_orchestrationStepProcessings = [];
	}

	static OrchestrationInstance()
	{
		DefaultDBValidator = SetDBValidatorRules(new ValidatorBuilder<OrchestrationInstance>()).Build();
	}

	public Dictionary<string, object?> ToDictionary()
		=> new()
		{
			{ nameof(IdOrchestrationInstance), IdOrchestrationInstance },
			{ nameof(IdOrchestration), IdOrchestration },
			{ nameof(IdOrchestrationStatus), IdOrchestrationStatus },
			{ nameof(CreatedUtc), CreatedUtc },
		};

	public void TrimStringValuesToFitDatabaseMaxLengths(string? postfix = null)
	{
	}

	public override string? GetPrimaryKeyValue()
	{
		return IdOrchestrationInstance.ToString();
	}

	public override string? ToString()
	{
		return IdOrchestrationInstance.ToString();
	}

	public static ValidatorBuilder<OrchestrationInstance> SetDBValidatorRules(ValidatorBuilder<OrchestrationInstance> builder)
		=> builder
			.ForProperty(x => x.IdOrchestrationInstance, v => v.NotDefaultOrEmpty())
			.ForProperty(x => x.IdOrchestration, v => v.NotDefaultOrEmpty(), (x, parent) => x.Orchestration == null)
			.ForProperty(x => x.IdOrchestrationStatus, v => v.NotDefaultOrEmpty(), (x, parent) => x.OrchestrationStatus == null)
			//.ForProperty(x => x.CreatedUtc, v => v.NotDefaultOrEmpty())
		;
}
