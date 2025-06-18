using Legion.Validation;

namespace Legion.ADF.ServiceBus.Orchestrations.Model;

public sealed partial class OrchestrationStepProcessingMessage : Orchestrations.OrchestrationsBaseEntity, Legion.Model.IEntity
{
	public static IValidator<OrchestrationStepProcessingMessage> DefaultDBValidator { get; }

	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid IdOrchestrationStepProcessingMessage { get; private set; }

	/// <summary>
	/// Database DataType: uuid NOT NULL | Orchestrations.Model.OrchestrationStepProcessing.OrchestrationStepProcessing | FK_OrchestrationStepProcessingMessage_IdOrchStepProcessing
	/// </summary>
	public Guid IdOrchestrationStepProcessing { get; private set; }

	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid IdMessage { get; private set; }

	/// <summary>
	/// Database DataType: uuid NOT NULL | Orchestrations.Model.OrchestrationStepProcessingMessageType.OrchestrationStepProcessingMessageType | FK_OrchestrationStepProcessingMessage_IdOrchStepProcessingMessa
	/// </summary>
	public Guid IdOrchestrationStepProcessingMessageType { get; private set; }

	/// <summary>
	/// Database DataType: timestamp with time zone NOT NULL
	/// </summary>
	public DateTime CreatedUtc { get; private set; }


	/// <summary>
	/// _1:N Guid IdOrchestrationStepProcessing | FK_OrchestrationStepProcessingMessage_IdOrchStepProcessing
	/// </summary>
	public Orchestrations.Model.OrchestrationStepProcessing OrchestrationStepProcessing { get; private set; }

	/// <summary>
	/// _1:N Guid IdOrchestrationStepProcessingMessageType | FK_OrchestrationStepProcessingMessage_IdOrchStepProcessingMessa
	/// </summary>
	public Orchestrations.Model.OrchestrationStepProcessingMessageType OrchestrationStepProcessingMessageType { get; private set; }

	private OrchestrationStepProcessingMessage()
	{
	}

	static OrchestrationStepProcessingMessage()
	{
		DefaultDBValidator = SetDBValidatorRules(new ValidatorBuilder<OrchestrationStepProcessingMessage>()).Build();
	}

	public Dictionary<string, object?> ToDictionary()
		=> new()
		{
			{ nameof(IdOrchestrationStepProcessingMessage), IdOrchestrationStepProcessingMessage },
			{ nameof(IdOrchestrationStepProcessing), IdOrchestrationStepProcessing },
			{ nameof(IdMessage), IdMessage },
			{ nameof(IdOrchestrationStepProcessingMessageType), IdOrchestrationStepProcessingMessageType },
			{ nameof(CreatedUtc), CreatedUtc },
		};

	public void TrimStringValuesToFitDatabaseMaxLengths(string? postfix = null)
	{
	}

	public override string? GetPrimaryKeyValue()
	{
		return IdOrchestrationStepProcessingMessage.ToString();
	}

	public override string? ToString()
	{
		return IdOrchestrationStepProcessingMessage.ToString();
	}

	public static ValidatorBuilder<OrchestrationStepProcessingMessage> SetDBValidatorRules(ValidatorBuilder<OrchestrationStepProcessingMessage> builder)
		=> builder
			.ForProperty(x => x.IdOrchestrationStepProcessingMessage, v => v.NotDefaultOrEmpty())
			.ForProperty(x => x.IdOrchestrationStepProcessing, v => v.NotDefaultOrEmpty(), (x, parent) => x.OrchestrationStepProcessing == null)
			//.ForProperty(x => x.IdMessage, v => v.NotDefaultOrEmpty())
			.ForProperty(x => x.IdOrchestrationStepProcessingMessageType, v => v.NotDefaultOrEmpty(), (x, parent) => x.OrchestrationStepProcessingMessageType == null)
			//.ForProperty(x => x.CreatedUtc, v => v.NotDefaultOrEmpty())
		;
}
