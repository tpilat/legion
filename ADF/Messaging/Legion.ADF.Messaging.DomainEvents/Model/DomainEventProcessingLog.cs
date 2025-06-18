using Legion.Validation;

namespace Legion.ADF.Messaging.DomainEvents.Model;

public sealed partial class DomainEventProcessingLog : DomainEvents.DomainEventsBaseEntity, Legion.Model.IEntity
{
	public static IValidator<DomainEventProcessingLog> DefaultDBValidator { get; }

	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid IdDomainEventProcessingLog { get; private set; }

	/// <summary>
	/// Database DataType: uuid NOT NULL | DomainEvents.Model.DomainEvent.DomainEvent | FK_DomainEventProcessingLog_IdDomainEvent
	/// </summary>
	public Guid IdDomainEvent { get; private set; }

	/// <summary>
	/// Database DataType: timestamp with time zone NOT NULL
	/// </summary>
	public DateTime CreatedUtc { get; private set; }

	/// <summary>
	/// Database DataType: uuid NOT NULL | DomainEvents.Model.DomainEventProcessingStatus.DomainEventProcessingStatus | FK_DomainEventProcessingLog_IdDomainEventProcessingStatus
	/// </summary>
	public Guid IdDomainEventProcessingStatus { get; private set; }

	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid TraceCorrelationId { get; private set; }

	/// <summary>
	/// Database DataType: uuid NULL
	/// </summary>
	public Guid? IdLogMessage { get; private set; }

	/// <summary>
	/// Database DataType: varchar(127) NOT NULL
	/// </summary>
	public string Code { get; private set; }

	/// <summary>
	/// Database DataType: text NULL
	/// </summary>
	public string? Detail { get; private set; }


	/// <summary>
	/// _1:N Guid IdDomainEvent | FK_DomainEventProcessingLog_IdDomainEvent
	/// </summary>
	public DomainEvents.Model.DomainEvent DomainEvent { get; private set; }

	/// <summary>
	/// _1:N Guid IdDomainEventProcessingStatus | FK_DomainEventProcessingLog_IdDomainEventProcessingStatus
	/// </summary>
	public DomainEvents.Model.DomainEventProcessingStatus DomainEventProcessingStatus { get; private set; }

	private DomainEventProcessingLog()
	{
	}

	static DomainEventProcessingLog()
	{
		DefaultDBValidator = SetDBValidatorRules(new ValidatorBuilder<DomainEventProcessingLog>()).Build();
	}

	public Dictionary<string, object?> ToDictionary()
		=> new()
		{
			{ nameof(IdDomainEventProcessingLog), IdDomainEventProcessingLog },
			{ nameof(IdDomainEvent), IdDomainEvent },
			{ nameof(CreatedUtc), CreatedUtc },
			{ nameof(IdDomainEventProcessingStatus), IdDomainEventProcessingStatus },
			{ nameof(TraceCorrelationId), TraceCorrelationId },
			{ nameof(IdLogMessage), IdLogMessage },
			{ nameof(Code), Code },
			{ nameof(Detail), Detail },
		};

	public void TrimStringValuesToFitDatabaseMaxLengths(string? postfix = null)
	{
		Code = Legion.Text.StringHelper.TrimToFitMaxLength(Code, 127, postfix);
	}

	public override string? GetPrimaryKeyValue()
	{
		return IdDomainEventProcessingLog.ToString();
	}

	public override string? ToString()
	{
		return IdDomainEventProcessingLog.ToString();
	}

	public static ValidatorBuilder<DomainEventProcessingLog> SetDBValidatorRules(ValidatorBuilder<DomainEventProcessingLog> builder)
		=> builder
			.ForProperty(x => x.IdDomainEventProcessingLog, v => v.NotDefaultOrEmpty())
			.ForProperty(x => x.IdDomainEvent, v => v.NotDefaultOrEmpty(), (x, parent) => x.DomainEvent == null)
			//.ForProperty(x => x.CreatedUtc, v => v.NotDefaultOrEmpty())
			.ForProperty(x => x.IdDomainEventProcessingStatus, v => v.NotDefaultOrEmpty(), (x, parent) => x.DomainEventProcessingStatus == null)
			//.ForProperty(x => x.TraceCorrelationId, v => v.NotDefaultOrEmpty())
			.ForProperty(x => x.Code, v => v.NotDefaultOrEmpty().MaxLength(127))
		;
}
