using Legion.Validation;

namespace Legion.ADF.Messaging.DomainEvents.Model;

public sealed partial class DomainEventProcessingStatus : DomainEvents.DomainEventsBaseEntity, Legion.Model.IEntity
{
	private List<DomainEvents.Model.DomainEventProcessingLog> _domainEventProcessingLogs;
	private List<DomainEvents.Model.DomainEvent> _domainEvents;

	public static IValidator<DomainEventProcessingStatus> DefaultDBValidator { get; }

	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid IdDomainEventProcessingStatus { get; private set; }

	/// <summary>
	/// Database DataType: varchar(63) NOT NULL
	/// </summary>
	public string Code { get; private set; }

	/// <summary>
	/// Database DataType: varchar(127) NOT NULL
	/// </summary>
	public string Name { get; private set; }


	/// <summary>
	/// N:_1 DomainEvents.Model.DomainEventProcessingLog.IdDomainEventProcessingStatus | FK_DomainEventProcessingLog_IdDomainEventProcessingStatus
	/// </summary>
	public IReadOnlyList<DomainEvents.Model.DomainEventProcessingLog> DomainEventProcessingLogs => _domainEventProcessingLogs;

	/// <summary>
	/// N:_1 DomainEvents.Model.DomainEvent.IdDomainEventProcessingStatus | FK_DomainEvent_IdDomainEventProcessingStatus
	/// </summary>
	public IReadOnlyList<DomainEvents.Model.DomainEvent> DomainEvents => _domainEvents;

	private DomainEventProcessingStatus()
	{
		_domainEventProcessingLogs = [];
		_domainEvents = [];
	}

	public Dictionary<string, object?> ToDictionary()
		=> new()
		{
			{ nameof(IdDomainEventProcessingStatus), IdDomainEventProcessingStatus },
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
		return IdDomainEventProcessingStatus.ToString();
	}

	public override string? ToString()
	{
		return Code;
	}

	public static ValidatorBuilder<DomainEventProcessingStatus> SetDBValidatorRules(ValidatorBuilder<DomainEventProcessingStatus> builder)
		=> builder
			.ForProperty(x => x.IdDomainEventProcessingStatus, v => v.NotDefaultOrEmpty())
			.ForProperty(x => x.Code, v => v.NotDefaultOrEmpty().MaxLength(63))
			.ForProperty(x => x.Name, v => v.NotDefaultOrEmpty().MaxLength(127))
		;
}
