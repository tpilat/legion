using Legion.Validation;

namespace Legion.ADF.ServiceBus.Orchestrations.Model;

public sealed partial class OrchestrationStep : Orchestrations.OrchestrationsBaseEntity, Legion.Model.IEntity
{
	private List<Orchestrations.Model.OrchestrationStepProcessing> _orchestrationStepProcessings;

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
	/// Database DataType: integer NOT NULL
	/// </summary>
	public int Order { get; private set; }

	/// <summary>
	/// Database DataType: varchar(255) NOT NULL
	/// </summary>
	public string Name { get; private set; }

	/// <summary>
	/// Database DataType: varchar(1023) NULL
	/// </summary>
	public string? Description { get; private set; }

	/// <summary>
	/// Database DataType: varchar(1023) NOT NULL
	/// </summary>
	public string Namespace { get; private set; }

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
	/// _1:N Guid IdOrchestration | FK_OrchestrationStep_IdOrchestration
	/// </summary>
	public Orchestrations.Model.Orchestration Orchestration { get; private set; }


	/// <summary>
	/// N:_1 Orchestrations.Model.OrchestrationStepProcessing.IdOrchestrationStep | FK_OrchestrationStepProcessing_IdOrchestrationStep
	/// </summary>
	public IReadOnlyList<Orchestrations.Model.OrchestrationStepProcessing> OrchestrationStepProcessings => _orchestrationStepProcessings;

	private OrchestrationStep()
	{
		_orchestrationStepProcessings = [];
	}

	static OrchestrationStep()
	{
		DefaultDBValidator = SetDBValidatorRules(new ValidatorBuilder<OrchestrationStep>()).Build();
	}

	public Dictionary<string, object?> ToDictionary()
		=> new()
		{
			{ nameof(IdOrchestrationStep), IdOrchestrationStep },
			{ nameof(IdOrchestration), IdOrchestration },
			{ nameof(IsMainEntry), IsMainEntry },
			{ nameof(Order), Order },
			{ nameof(Name), Name },
			{ nameof(Description), Description },
			{ nameof(Namespace), Namespace },
			{ nameof(Properties), Properties },
			{ nameof(TimeoutForMessageProcessingInSeconds), TimeoutForMessageProcessingInSeconds },
			{ nameof(MaxMessageProcessingRetryCount), MaxMessageProcessingRetryCount },
		};

	public void TrimStringValuesToFitDatabaseMaxLengths(string? postfix = null)
	{
		Name = Legion.Text.StringHelper.TrimToFitMaxLength(Name, 255, postfix);
		Description = Legion.Text.StringHelper.TrimToFitMaxLength(Description, 1023, postfix);
		Namespace = Legion.Text.StringHelper.TrimToFitMaxLength(Namespace, 1023, postfix);
	}

	public override string? GetPrimaryKeyValue()
	{
		return IdOrchestrationStep.ToString();
	}

	public override string? ToString()
	{
		return IdOrchestrationStep.ToString();
	}

	public static ValidatorBuilder<OrchestrationStep> SetDBValidatorRules(ValidatorBuilder<OrchestrationStep> builder)
		=> builder
			.ForProperty(x => x.IdOrchestrationStep, v => v.NotDefaultOrEmpty())
			.ForProperty(x => x.IdOrchestration, v => v.NotDefaultOrEmpty(), (x, parent) => x.Orchestration == null)
			//.ForProperty(x => x.Order, v => v.NotDefaultOrEmpty())
			.ForProperty(x => x.Name, v => v.NotDefaultOrEmpty().MaxLength(255))
			.ForProperty(x => x.Description, v => v.MaxLength(1023))
			.ForProperty(x => x.Namespace, v => v.NotDefaultOrEmpty().MaxLength(1023))
			//.ForProperty(x => x.TimeoutForMessageProcessingInSeconds, v => v.NotDefaultOrEmpty())
			//.ForProperty(x => x.MaxMessageProcessingRetryCount, v => v.NotDefaultOrEmpty())
		;
}
