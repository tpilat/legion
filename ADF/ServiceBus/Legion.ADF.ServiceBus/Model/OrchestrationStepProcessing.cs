using Legion.Validation;

namespace Legion.ADF.ServiceBus.Model;

public sealed partial class OrchestrationStepProcessing : ServiceBus.ServiceBusBaseEntity, Legion.Model.IEntity
{
	private List<ServiceBus.Model.OrchestrationStepProcessingDirection> _orchestrationStepProcessingDirections;
	private List<ServiceBus.Model.OrchestrationStepProcessingLog> _orchestrationStepProcessingLogs;
	private List<ServiceBus.Model.OrchestrationStepProcessingMessage> _orchestrationStepProcessingMessages;
	private List<ServiceBus.Model.OrchestrationStepProcessingDirection> _toStepOrchestrationStepProcessingDirections;

	public static IValidator<OrchestrationStepProcessing> DefaultDBValidator { get; }

	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid IdOrchestrationStepProcessing { get; private set; }

	/// <summary>
	/// Database DataType: uuid NOT NULL | ServiceBus.Model.OrchestrationStep.OrchestrationStep | FK_OrchestrationStepProcessing_IdOrchestrationStep
	/// </summary>
	public Guid IdOrchestrationStep { get; private set; }

	/// <summary>
	/// Database DataType: uuid NOT NULL | ServiceBus.Model.OrchestrationInstance.OrchestrationInstance | FK_OrchestrationStepProcessing_IdOrchestrationInstance
	/// </summary>
	public Guid IdOrchestrationInstance { get; private set; }

	/// <summary>
	/// Database DataType: uuid NOT NULL | ServiceBus.Model.OrchestrationStepProcessingStatus.OrchestrationStepProcessingStatus | FK_OrchestrationStepProcessing_IdOrchestrationStepProcessingSta
	/// </summary>
	public Guid IdOrchestrationStepProcessingStatus { get; private set; }

	/// <summary>
	/// Database DataType: timestamp with time zone NOT NULL
	/// </summary>
	public DateTime CreatedUtc { get; private set; }

	/// <summary>
	/// Database DataType: timestamp with time zone NULL
	/// </summary>
	public DateTime? ProcessedUtc { get; private set; }

	/// <summary>
	/// Database DataType: timestamp with time zone NULL
	/// </summary>
	public DateTime? SuspendedUtc { get; private set; }

	/// <summary>
	/// Database DataType: timestamp with time zone NULL
	/// </summary>
	public DateTime? LastProcessingUtc { get; private set; }

	/// <summary>
	/// Database DataType: timestamp with time zone NOT NULL
	/// </summary>
	public DateTime NextProcessingUtc { get; private set; }

	/// <summary>
	/// Database DataType: integer NOT NULL
	/// </summary>
	public int RetryCount { get; private set; }


	/// <summary>
	/// _1:N Guid IdOrchestrationInstance | FK_OrchestrationStepProcessing_IdOrchestrationInstance
	/// </summary>
	public ServiceBus.Model.OrchestrationInstance OrchestrationInstance { get; private set; }

	/// <summary>
	/// _1:N Guid IdOrchestrationStep | FK_OrchestrationStepProcessing_IdOrchestrationStep
	/// </summary>
	public ServiceBus.Model.OrchestrationStep OrchestrationStep { get; private set; }

	/// <summary>
	/// _1:N Guid IdOrchestrationStepProcessingStatus | FK_OrchestrationStepProcessing_IdOrchestrationStepProcessingSta
	/// </summary>
	public ServiceBus.Model.OrchestrationStepProcessingStatus OrchestrationStepProcessingStatus { get; private set; }


	/// <summary>
	/// N:_1 ServiceBus.Model.OrchestrationStepProcessingDirection.IdFromStep | FK_OrchestrationStepProcessingDirection_IdFromStep
	/// </summary>
	public IReadOnlyList<ServiceBus.Model.OrchestrationStepProcessingDirection> OrchestrationStepProcessingDirections => _orchestrationStepProcessingDirections;

	/// <summary>
	/// N:_1 ServiceBus.Model.OrchestrationStepProcessingLog.IdOrchestrationStepProcessing | FK_OrchestrationStepProcessingLog_IdOrchStepProcessing
	/// </summary>
	public IReadOnlyList<ServiceBus.Model.OrchestrationStepProcessingLog> OrchestrationStepProcessingLogs => _orchestrationStepProcessingLogs;

	/// <summary>
	/// N:_1 ServiceBus.Model.OrchestrationStepProcessingMessage.IdOrchestrationStepProcessing | FK_OrchestrationStepProcessingMessage_IdOrchStepProcessing
	/// </summary>
	public IReadOnlyList<ServiceBus.Model.OrchestrationStepProcessingMessage> OrchestrationStepProcessingMessages => _orchestrationStepProcessingMessages;

	/// <summary>
	/// N:_1 ServiceBus.Model.OrchestrationStepProcessingDirection.IdToStep | FK_OrchestrationStepProcessingDirection_IdToStep
	/// </summary>
	public IReadOnlyList<ServiceBus.Model.OrchestrationStepProcessingDirection> ToStepOrchestrationStepProcessingDirections => _toStepOrchestrationStepProcessingDirections;

	private OrchestrationStepProcessing()
	{
		_orchestrationStepProcessingDirections = [];
		_orchestrationStepProcessingLogs = [];
		_orchestrationStepProcessingMessages = [];
		_toStepOrchestrationStepProcessingDirections = [];
	}

	static OrchestrationStepProcessing()
	{
		DefaultDBValidator = SetDBValidatorRules(new ValidatorBuilder<OrchestrationStepProcessing>()).Build();
	}

	public Dictionary<string, object?> ToDictionary()
		=> new()
		{
			{ nameof(IdOrchestrationStepProcessing), IdOrchestrationStepProcessing },
			{ nameof(IdOrchestrationStep), IdOrchestrationStep },
			{ nameof(IdOrchestrationInstance), IdOrchestrationInstance },
			{ nameof(IdOrchestrationStepProcessingStatus), IdOrchestrationStepProcessingStatus },
			{ nameof(CreatedUtc), CreatedUtc },
			{ nameof(ProcessedUtc), ProcessedUtc },
			{ nameof(SuspendedUtc), SuspendedUtc },
			{ nameof(LastProcessingUtc), LastProcessingUtc },
			{ nameof(NextProcessingUtc), NextProcessingUtc },
			{ nameof(RetryCount), RetryCount },
		};

	public void TrimStringValuesToFitDatabaseMaxLengths(string? postfix = null)
	{
	}

	public override string? GetPrimaryKeyValue()
	{
		return IdOrchestrationStepProcessing.ToString();
	}

	public override string? ToString()
	{
		return IdOrchestrationStepProcessing.ToString();
	}

	public static ValidatorBuilder<OrchestrationStepProcessing> SetDBValidatorRules(ValidatorBuilder<OrchestrationStepProcessing> builder)
		=> builder
			.ForProperty(x => x.IdOrchestrationStepProcessing, v => v.NotDefaultOrEmpty())
			.ForProperty(x => x.IdOrchestrationStep, v => v.NotDefaultOrEmpty(), (x, parent) => x.OrchestrationStep == null)
			.ForProperty(x => x.IdOrchestrationInstance, v => v.NotDefaultOrEmpty(), (x, parent) => x.OrchestrationInstance == null)
			.ForProperty(x => x.IdOrchestrationStepProcessingStatus, v => v.NotDefaultOrEmpty(), (x, parent) => x.OrchestrationStepProcessingStatus == null)
			//.ForProperty(x => x.CreatedUtc, v => v.NotDefaultOrEmpty())
			//.ForProperty(x => x.NextProcessingUtc, v => v.NotDefaultOrEmpty())
			//.ForProperty(x => x.RetryCount, v => v.NotDefaultOrEmpty())
		;
}
