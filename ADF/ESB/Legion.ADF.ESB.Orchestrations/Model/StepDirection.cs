using Legion.Validation;

namespace Legion.ADF.ESB.Orchestrations.Model;

public sealed partial class StepDirection : Orchestrations.OrchestrationsBaseEntity, Legion.Model.IEntity
{
	public static IValidator<StepDirection> DefaultDBValidator { get; }

	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid IdStepDirection { get; private set; }

	/// <summary>
	/// Database DataType: uuid NOT NULL | Orchestrations.Model.OrchestrationStepInstance.FromStep | FK_StepDirection_IdFromStep
	/// </summary>
	public Guid IdFromStep { get; private set; }

	/// <summary>
	/// Database DataType: uuid NOT NULL | Orchestrations.Model.OrchestrationStepInstance.ToStep | FK_StepDirection_IdToStep
	/// </summary>
	public Guid IdToStep { get; private set; }

	/// <summary>
	/// Database DataType: timestamp without time zone NOT NULL
	/// </summary>
	public DateTime CreatedUtc { get; private set; }


	/// <summary>
	/// _1:N Guid IdFromStep | FK_StepDirection_IdFromStep
	/// </summary>
	public Orchestrations.Model.OrchestrationStepInstance FromStep { get; private set; }

	/// <summary>
	/// _1:N Guid IdToStep | FK_StepDirection_IdToStep
	/// </summary>
	public Orchestrations.Model.OrchestrationStepInstance ToStep { get; private set; }

	private StepDirection()
	{
	}

	static StepDirection()
	{
		DefaultDBValidator = SetDBValidatorRules(new ValidatorBuilder<StepDirection>()).Build();
	}

	public override string? ToString()
	{
		return IdStepDirection.ToString();
	}

	public static ValidatorBuilder<StepDirection> SetDBValidatorRules(ValidatorBuilder<StepDirection> builder)
		=> builder
			.ForProperty(x => x.IdStepDirection, v => v.NotDefaultOrEmpty())
			.ForProperty(x => x.IdFromStep, v => v.NotDefaultOrEmpty(), x => x.FromStep == null)
			.ForProperty(x => x.IdToStep, v => v.NotDefaultOrEmpty(), x => x.ToStep == null)
			//.ForProperty(x => x.CreatedUtc, v => v.NotDefaultOrEmpty())
		;
}
