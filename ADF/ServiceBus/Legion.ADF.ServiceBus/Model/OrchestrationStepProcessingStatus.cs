using Legion.Validation;

namespace Legion.ADF.ServiceBus.Model;

public sealed partial class OrchestrationStepProcessingStatus : ServiceBus.ServiceBusBaseEntity, Legion.Model.IEntity
{
	private List<ServiceBus.Model.OrchestrationStepProcessingLog> _orchestrationStepProcessingLogs;
	private List<ServiceBus.Model.OrchestrationStepProcessing> _orchestrationStepProcessings;

	public static IValidator<OrchestrationStepProcessingStatus> DefaultDBValidator { get; }

	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid IdOrchestrationStepProcessingStatus { get; private set; }

	/// <summary>
	/// Database DataType: varchar(63) NOT NULL
	/// </summary>
	public string Code { get; private set; }

	/// <summary>
	/// Database DataType: varchar(127) NOT NULL
	/// </summary>
	public string Name { get; private set; }


	/// <summary>
	/// N:_1 ServiceBus.Model.OrchestrationStepProcessingLog.IdOrchestrationStepProcessingStatus | FK_OrchestrationStepProcessingLog_IdOrchStepProcessingStatus
	/// </summary>
	public IReadOnlyList<ServiceBus.Model.OrchestrationStepProcessingLog> OrchestrationStepProcessingLogs => _orchestrationStepProcessingLogs;

	/// <summary>
	/// N:_1 ServiceBus.Model.OrchestrationStepProcessing.IdOrchestrationStepProcessingStatus | FK_OrchestrationStepProcessing_IdOrchestrationStepProcessingSta
	/// </summary>
	public IReadOnlyList<ServiceBus.Model.OrchestrationStepProcessing> OrchestrationStepProcessings => _orchestrationStepProcessings;

	private OrchestrationStepProcessingStatus()
	{
		_orchestrationStepProcessingLogs = [];
		_orchestrationStepProcessings = [];
	}

	static OrchestrationStepProcessingStatus()
	{
		DefaultDBValidator = SetDBValidatorRules(new ValidatorBuilder<OrchestrationStepProcessingStatus>()).Build();
	}

	public Dictionary<string, object?> ToDictionary()
		=> new()
		{
			{ nameof(IdOrchestrationStepProcessingStatus), IdOrchestrationStepProcessingStatus },
			{ nameof(Code), Code },
			{ nameof(Name), Name },
		};

	public void TrimStringValuesToFitDatabaseMaxLengths(string? postfix = null)
	{
		Code = Legion.Text.StringHelper.TrimToFitMaxLength(Code, 63, postfix);
		Name = Legion.Text.StringHelper.TrimToFitMaxLength(Name, 127, postfix);
	}

	public override string? GetPrimaryKeyValue()
	{
		return IdOrchestrationStepProcessingStatus.ToString();
	}

	public override string? ToString()
	{
		return IdOrchestrationStepProcessingStatus.ToString();
	}

	public static ValidatorBuilder<OrchestrationStepProcessingStatus> SetDBValidatorRules(ValidatorBuilder<OrchestrationStepProcessingStatus> builder)
		=> builder
			.ForProperty(x => x.IdOrchestrationStepProcessingStatus, v => v.NotDefaultOrEmpty())
			.ForProperty(x => x.Code, v => v.NotDefaultOrEmpty().MaxLength(63))
			.ForProperty(x => x.Name, v => v.NotDefaultOrEmpty().MaxLength(127))
		;
}
