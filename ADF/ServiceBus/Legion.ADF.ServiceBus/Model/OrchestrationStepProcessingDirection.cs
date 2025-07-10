using Legion.Validation;

namespace Legion.ADF.ServiceBus.Model;

public sealed partial class OrchestrationStepProcessingDirection : ServiceBus.ServiceBusBaseEntity, Legion.Model.IEntity
{
	public static IValidator<OrchestrationStepProcessingDirection> DefaultDBValidator { get; }

	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid IdOrchestrationStepProcessingDirection { get; private set; }

	/// <summary>
	/// Database DataType: uuid NOT NULL | ServiceBus.Model.OrchestrationStepProcessing.FromStep | FK_OrchestrationStepProcessingDirection_IdFromStep
	/// </summary>
	public Guid IdFromStep { get; private set; }

	/// <summary>
	/// Database DataType: uuid NOT NULL | ServiceBus.Model.OrchestrationStepProcessing.ToStep | FK_OrchestrationStepProcessingDirection_IdToStep
	/// </summary>
	public Guid IdToStep { get; private set; }

	/// <summary>
	/// Database DataType: timestamp with time zone NOT NULL
	/// </summary>
	public DateTime CreatedUtc { get; private set; }


	/// <summary>
	/// _1:N Guid IdFromStep | FK_OrchestrationStepProcessingDirection_IdFromStep
	/// </summary>
	public ServiceBus.Model.OrchestrationStepProcessing FromStep { get; private set; }

	/// <summary>
	/// _1:N Guid IdToStep | FK_OrchestrationStepProcessingDirection_IdToStep
	/// </summary>
	public ServiceBus.Model.OrchestrationStepProcessing ToStep { get; private set; }

	private OrchestrationStepProcessingDirection()
	{
	}

	static OrchestrationStepProcessingDirection()
	{
		DefaultDBValidator = SetDBValidatorRules(new ValidatorBuilder<OrchestrationStepProcessingDirection>()).Build();
	}

	public Dictionary<string, object?> ToDictionary()
		=> new()
		{
			{ nameof(IdOrchestrationStepProcessingDirection), IdOrchestrationStepProcessingDirection },
			{ nameof(IdFromStep), IdFromStep },
			{ nameof(IdToStep), IdToStep },
			{ nameof(CreatedUtc), CreatedUtc },
		};

	public void TrimStringValuesToFitDatabaseMaxLengths(string? postfix = null)
	{
	}

	public override string? GetPrimaryKeyValue()
	{
		return IdOrchestrationStepProcessingDirection.ToString();
	}

	public override string? ToString()
	{
		return IdOrchestrationStepProcessingDirection.ToString();
	}

	public static ValidatorBuilder<OrchestrationStepProcessingDirection> SetDBValidatorRules(ValidatorBuilder<OrchestrationStepProcessingDirection> builder)
		=> builder
			.ForProperty(x => x.IdOrchestrationStepProcessingDirection, v => v.NotDefaultOrEmpty())
			.ForProperty(x => x.IdFromStep, v => v.NotDefaultOrEmpty(), (x, parent) => x.FromStep == null)
			.ForProperty(x => x.IdToStep, v => v.NotDefaultOrEmpty(), (x, parent) => x.ToStep == null)
			//.ForProperty(x => x.CreatedUtc, v => v.NotDefaultOrEmpty())
		;
}
