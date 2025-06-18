namespace Legion.ADF.Messaging.Outbox.Model;

public sealed partial class VwOutboxMessageProcessingLog : Outbox.OutboxBaseQueryEntity, Legion.Model.IQueryEntity
{
	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid IdOutboxMessageProcessingLog { get; private set; }

	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid IdOutboxMessage { get; private set; }

	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid IdOutboxQueue { get; private set; }

	/// <summary>
	/// Database DataType: timestamp with time zone NOT NULL
	/// </summary>
	public DateTime CreatedUtc { get; private set; }

	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid IdOutboxMessageStatus { get; private set; }

	/// <summary>
	/// Database DataType: varchar(63) NOT NULL
	/// </summary>
	public string OutboxMessageStatusCode { get; private set; }

	/// <summary>
	/// Database DataType: varchar(127) NOT NULL
	/// </summary>
	public string OutboxMessageStatusName { get; private set; }

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
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid IdOutboxInstance { get; private set; }


	private VwOutboxMessageProcessingLog()
	{
	}

	public Dictionary<string, object?> ToDictionary()
		=> new()
		{
			{ nameof(IdOutboxMessageProcessingLog), IdOutboxMessageProcessingLog },
			{ nameof(IdOutboxMessage), IdOutboxMessage },
			{ nameof(IdOutboxQueue), IdOutboxQueue },
			{ nameof(CreatedUtc), CreatedUtc },
			{ nameof(IdOutboxMessageStatus), IdOutboxMessageStatus },
			{ nameof(OutboxMessageStatusCode), OutboxMessageStatusCode },
			{ nameof(OutboxMessageStatusName), OutboxMessageStatusName },
			{ nameof(TraceCorrelationId), TraceCorrelationId },
			{ nameof(IdLogMessage), IdLogMessage },
			{ nameof(Code), Code },
			{ nameof(Detail), Detail },
			{ nameof(IdOutboxInstance), IdOutboxInstance },
		};

	public override string? ToString()
	{
		return IdOutboxMessageProcessingLog.ToString();
	}
}
