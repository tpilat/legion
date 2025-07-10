using Legion.Validation;

namespace Legion.ADF.ServiceBus.Model;

public sealed partial class Orchestration : ServiceBus.ServiceBusBaseEntity, Legion.Model.IEntity
{
	private List<ServiceBus.Model.OrchestrationInstance> _orchestrationInstances;
	private List<ServiceBus.Model.OrchestrationStep> _orchestrationSteps;

	public static IValidator<Orchestration> DefaultDBValidator { get; }

	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid IdOrchestration { get; private set; }

	/// <summary>
	/// Database DataType: varchar(255) NOT NULL
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
	/// Database DataType: varchar(1023) NOT NULL
	/// </summary>
	public string Namespace { get; private set; }

	/// <summary>
	/// Database DataType: varchar(31) NOT NULL
	/// </summary>
	public string Version { get; private set; }

	/// <summary>
	/// Database DataType: jsonb NULL
	/// </summary>
	public string? Properties { get; private set; }


	/// <summary>
	/// N:_1 ServiceBus.Model.OrchestrationInstance.IdOrchestration | FK_OrchestrationInstance_IdOrchestration
	/// </summary>
	public IReadOnlyList<ServiceBus.Model.OrchestrationInstance> OrchestrationInstances => _orchestrationInstances;

	/// <summary>
	/// N:_1 ServiceBus.Model.OrchestrationStep.IdOrchestration | FK_OrchestrationStep_IdOrchestration
	/// </summary>
	public IReadOnlyList<ServiceBus.Model.OrchestrationStep> OrchestrationSteps => _orchestrationSteps;

	private Orchestration()
	{
		_orchestrationInstances = [];
		_orchestrationSteps = [];
	}

	static Orchestration()
	{
		DefaultDBValidator = SetDBValidatorRules(new ValidatorBuilder<Orchestration>()).Build();
	}

	public Dictionary<string, object?> ToDictionary()
		=> new()
		{
			{ nameof(IdOrchestration), IdOrchestration },
			{ nameof(Name), Name },
			{ nameof(Description), Description },
			{ nameof(IsSingleton), IsSingleton },
			{ nameof(Namespace), Namespace },
			{ nameof(Version), Version },
			{ nameof(Properties), Properties },
		};

	public void TrimStringValuesToFitDatabaseMaxLengths(string? postfix = null)
	{
		Name = Legion.Text.StringHelper.TrimToFitMaxLength(Name, 255, postfix);
		Description = Legion.Text.StringHelper.TrimToFitMaxLength(Description, 1023, postfix);
		Namespace = Legion.Text.StringHelper.TrimToFitMaxLength(Namespace, 1023, postfix);
		Version = Legion.Text.StringHelper.TrimToFitMaxLength(Version, 31, postfix);
	}

	public override string? GetPrimaryKeyValue()
	{
		return IdOrchestration.ToString();
	}

	public override string? ToString()
	{
		return IdOrchestration.ToString();
	}

	public static ValidatorBuilder<Orchestration> SetDBValidatorRules(ValidatorBuilder<Orchestration> builder)
		=> builder
			.ForProperty(x => x.IdOrchestration, v => v.NotDefaultOrEmpty())
			.ForProperty(x => x.Name, v => v.NotDefaultOrEmpty().MaxLength(255))
			.ForProperty(x => x.Description, v => v.MaxLength(1023))
			.ForProperty(x => x.Namespace, v => v.NotDefaultOrEmpty().MaxLength(1023))
			.ForProperty(x => x.Version, v => v.NotDefaultOrEmpty().MaxLength(31))
		;
}
