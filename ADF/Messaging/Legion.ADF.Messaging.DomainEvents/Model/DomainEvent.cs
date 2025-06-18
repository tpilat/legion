using Legion.Validation;

namespace Legion.ADF.Messaging.DomainEvents.Model;

public sealed partial class DomainEvent : DomainEvents.DomainEventsBaseEntity, Legion.Model.IEntity
{
	private List<DomainEvents.Model.DomainEventProcessingLog> _domainEventProcessingLogs;

	public static IValidator<DomainEvent> DefaultDBValidator { get; }

	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid IdDomainEvent { get; private set; }

	/// <summary>
	/// Database DataType: uuid NOT NULL | DomainEvents.Model.DomainEventContent.Content | FK_DomainEvent_IdDomainEventContent
	/// </summary>
	public Guid IdContent { get; private set; }

	/// <summary>
	/// Database DataType: uuid NOT NULL | DomainEvents.Model.DomainEventProcessingStatus.DomainEventProcessingStatus | FK_DomainEvent_IdDomainEventProcessingStatus
	/// </summary>
	public Guid IdDomainEventProcessingStatus { get; private set; }

	/// <summary>
	/// Database DataType: varchar(1023) NOT NULL
	/// </summary>
	public string Namespace { get; private set; }

	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid TraceCorrelationId { get; private set; }

	/// <summary>
	/// Database DataType: jsonb NULL
	/// </summary>
	public string? Properties { get; private set; }

	/// <summary>
	/// Database DataType: varchar(511) NULL
	/// </summary>
	public string? Publisher { get; private set; }

	/// <summary>
	/// Database DataType: varchar(511) NULL
	/// </summary>
	public string? PublisherId { get; private set; }

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
	/// Database DataType: timestamp with time zone NULL
	/// </summary>
	public DateTime? LastProcessingTimeoutUtc { get; private set; }

	/// <summary>
	/// Database DataType: timestamp with time zone NOT NULL
	/// </summary>
	public DateTime NextProcessingUtc { get; private set; }

	/// <summary>
	/// Database DataType: integer NOT NULL
	/// </summary>
	public int RetryCount { get; private set; }

	/// <summary>
	/// Database DataType: integer NOT NULL
	/// </summary>
	public int Priority { get; private set; }


	/// <summary>
	/// UNIQUE INDEX: UQ_DomainEvent_IdContent
	/// _1:1 Guid IdContent | FK_DomainEvent_IdDomainEventContent
	/// </summary>
	public DomainEvents.Model.DomainEventContent Content { get; private set; }

	/// <summary>
	/// _1:N Guid IdDomainEventProcessingStatus | FK_DomainEvent_IdDomainEventProcessingStatus
	/// </summary>
	public DomainEvents.Model.DomainEventProcessingStatus DomainEventProcessingStatus { get; private set; }


	/// <summary>
	/// N:_1 DomainEvents.Model.DomainEventProcessingLog.IdDomainEvent | FK_DomainEventProcessingLog_IdDomainEvent
	/// </summary>
	public IReadOnlyList<DomainEvents.Model.DomainEventProcessingLog> DomainEventProcessingLogs => _domainEventProcessingLogs;

	private DomainEvent()
	{
		_domainEventProcessingLogs = [];
	}

	static DomainEvent()
	{
		DefaultDBValidator = SetDBValidatorRules(new ValidatorBuilder<DomainEvent>()).Build();
	}

	public Dictionary<string, object?> ToDictionary()
		=> new()
		{
			{ nameof(IdDomainEvent), IdDomainEvent },
			{ nameof(IdContent), IdContent },
			{ nameof(IdDomainEventProcessingStatus), IdDomainEventProcessingStatus },
			{ nameof(Namespace), Namespace },
			{ nameof(TraceCorrelationId), TraceCorrelationId },
			{ nameof(Properties), Properties },
			{ nameof(Publisher), Publisher },
			{ nameof(PublisherId), PublisherId },
			{ nameof(CreatedUtc), CreatedUtc },
			{ nameof(ProcessedUtc), ProcessedUtc },
			{ nameof(SuspendedUtc), SuspendedUtc },
			{ nameof(LastProcessingUtc), LastProcessingUtc },
			{ nameof(LastProcessingTimeoutUtc), LastProcessingTimeoutUtc },
			{ nameof(NextProcessingUtc), NextProcessingUtc },
			{ nameof(RetryCount), RetryCount },
			{ nameof(Priority), Priority },
		};

	public void TrimStringValuesToFitDatabaseMaxLengths(string? postfix = null)
	{
		Namespace = Legion.Text.StringHelper.TrimToFitMaxLength(Namespace, 1023, postfix);
		Publisher = Legion.Text.StringHelper.TrimToFitMaxLength(Publisher, 511, postfix);
		PublisherId = Legion.Text.StringHelper.TrimToFitMaxLength(PublisherId, 511, postfix);
	}

	public override string? GetPrimaryKeyValue()
	{
		return IdDomainEvent.ToString();
	}

	public override string? ToString()
	{
		return IdDomainEvent.ToString();
	}

	public static ValidatorBuilder<DomainEvent> SetDBValidatorRules(ValidatorBuilder<DomainEvent> builder)
		=> builder
			.ForProperty(x => x.IdDomainEvent, v => v.NotDefaultOrEmpty())
			.ForProperty(x => x.IdContent, v => v.NotDefaultOrEmpty(), (x, parent) => x.Content == null)
			.ForProperty(x => x.IdDomainEventProcessingStatus, v => v.NotDefaultOrEmpty(), (x, parent) => x.DomainEventProcessingStatus == null)
			.ForProperty(x => x.Namespace, v => v.NotDefaultOrEmpty().MaxLength(1023))
			//.ForProperty(x => x.TraceCorrelationId, v => v.NotDefaultOrEmpty())
			.ForProperty(x => x.Publisher, v => v.MaxLength(511))
			.ForProperty(x => x.PublisherId, v => v.MaxLength(511))
			//.ForProperty(x => x.CreatedUtc, v => v.NotDefaultOrEmpty())
			//.ForProperty(x => x.NextProcessingUtc, v => v.NotDefaultOrEmpty())
			//.ForProperty(x => x.RetryCount, v => v.NotDefaultOrEmpty())
			//.ForProperty(x => x.Priority, v => v.NotDefaultOrEmpty())
		;
}
