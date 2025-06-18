using Legion.Validation;

namespace Legion.ADF.ServiceBus.Orchestrations.Model;

public sealed partial class OrchestrationStepProcessingMessageType : Orchestrations.OrchestrationsBaseEntity, Legion.Model.IEntity
{
	private List<Orchestrations.Model.OrchestrationStepProcessingMessage> _orchestrationStepProcessingMessages;

	public static IValidator<OrchestrationStepProcessingMessageType> DefaultDBValidator { get; }

	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid IdOrchestrationStepProcessingMessageType { get; private set; }

	/// <summary>
	/// Database DataType: varchar(63) NOT NULL
	/// </summary>
	public string Code { get; private set; }

	/// <summary>
	/// Database DataType: varchar(63) NOT NULL
	/// </summary>
	public string Name { get; private set; }


	/// <summary>
	/// N:_1 Orchestrations.Model.OrchestrationStepProcessingMessage.IdOrchestrationStepProcessingMessageType | FK_OrchestrationStepProcessingMessage_IdOrchStepProcessingMessa
	/// </summary>
	public IReadOnlyList<Orchestrations.Model.OrchestrationStepProcessingMessage> OrchestrationStepProcessingMessages => _orchestrationStepProcessingMessages;

	private OrchestrationStepProcessingMessageType()
	{
		_orchestrationStepProcessingMessages = [];
	}

	static OrchestrationStepProcessingMessageType()
	{
		DefaultDBValidator = SetDBValidatorRules(new ValidatorBuilder<OrchestrationStepProcessingMessageType>()).Build();
	}

	public Dictionary<string, object?> ToDictionary()
		=> new()
		{
			{ nameof(IdOrchestrationStepProcessingMessageType), IdOrchestrationStepProcessingMessageType },
			{ nameof(Code), Code },
			{ nameof(Name), Name },
		};

	public void TrimStringValuesToFitDatabaseMaxLengths(string? postfix = null)
	{
		Code = Legion.Text.StringHelper.TrimToFitMaxLength(Code, 63, postfix);
		Name = Legion.Text.StringHelper.TrimToFitMaxLength(Name, 63, postfix);
	}

	public override string? GetPrimaryKeyValue()
	{
		return IdOrchestrationStepProcessingMessageType.ToString();
	}

	public override string? ToString()
	{
		return IdOrchestrationStepProcessingMessageType.ToString();
	}

	public static ValidatorBuilder<OrchestrationStepProcessingMessageType> SetDBValidatorRules(ValidatorBuilder<OrchestrationStepProcessingMessageType> builder)
		=> builder
			.ForProperty(x => x.IdOrchestrationStepProcessingMessageType, v => v.NotDefaultOrEmpty())
			.ForProperty(x => x.Code, v => v.NotDefaultOrEmpty().MaxLength(63))
			.ForProperty(x => x.Name, v => v.NotDefaultOrEmpty().MaxLength(63))
		;
}
