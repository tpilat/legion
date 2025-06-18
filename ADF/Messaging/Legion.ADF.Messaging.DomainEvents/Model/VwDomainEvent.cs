namespace Legion.ADF.Messaging.DomainEvents.Model;

public sealed partial class VwDomainEvent : DomainEvents.DomainEventsBaseQueryEntity, Legion.Model.IQueryEntity
{
	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid IdDomainEvent { get; private set; }

	/// <summary>
	/// Database DataType: uuid NULL
	/// </summary>
	public Guid? IdContent { get; private set; }

	/// <summary>
	/// Database DataType: uuid NULL
	/// </summary>
	public Guid? IdDomainEventProcessingStatus { get; private set; }

	/// <summary>
	/// Database DataType: varchar(1023) NULL
	/// </summary>
	public string? Namespace { get; private set; }

	/// <summary>
	/// Database DataType: uuid NULL
	/// </summary>
	public Guid? TraceCorrelationId { get; private set; }

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
	/// Database DataType: timestamp with time zone NULL
	/// </summary>
	public DateTime? CreatedUtc { get; private set; }

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
	/// Database DataType: timestamp with time zone NULL
	/// </summary>
	public DateTime? NextProcessingUtc { get; private set; }

	/// <summary>
	/// Database DataType: integer NULL
	/// </summary>
	public int? RetryCount { get; private set; }

	/// <summary>
	/// Database DataType: integer NULL
	/// </summary>
	public int? Priority { get; private set; }


	private VwDomainEvent()
	{
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

	public override string? ToString()
	{
		return IdDomainEvent.ToString();
	}
}
