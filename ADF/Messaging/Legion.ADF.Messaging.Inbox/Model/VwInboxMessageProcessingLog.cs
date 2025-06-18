namespace Legion.ADF.Messaging.Inbox.Model;

public sealed partial class VwInboxMessageProcessingLog : Inbox.InboxBaseQueryEntity, Legion.Model.IQueryEntity
{
	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid IdInboxMessageProcessingLog { get; private set; }

	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid IdInboxMessage { get; private set; }

	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid IdInboxQueue { get; private set; }

	/// <summary>
	/// Database DataType: timestamp with time zone NOT NULL
	/// </summary>
	public DateTime CreatedUtc { get; private set; }

	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid IdInboxMessageStatus { get; private set; }

	/// <summary>
	/// Database DataType: varchar(63) NOT NULL
	/// </summary>
	public string InboxMessageStatusCode { get; private set; }

	/// <summary>
	/// Database DataType: varchar(127) NOT NULL
	/// </summary>
	public string InboxMessageStatusName { get; private set; }

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
	public Guid IdInboxInstance { get; private set; }


	private VwInboxMessageProcessingLog()
	{
	}

	public Dictionary<string, object?> ToDictionary()
		=> new()
		{
			{ nameof(IdInboxMessageProcessingLog), IdInboxMessageProcessingLog },
			{ nameof(IdInboxMessage), IdInboxMessage },
			{ nameof(IdInboxQueue), IdInboxQueue },
			{ nameof(CreatedUtc), CreatedUtc },
			{ nameof(IdInboxMessageStatus), IdInboxMessageStatus },
			{ nameof(InboxMessageStatusCode), InboxMessageStatusCode },
			{ nameof(InboxMessageStatusName), InboxMessageStatusName },
			{ nameof(TraceCorrelationId), TraceCorrelationId },
			{ nameof(IdLogMessage), IdLogMessage },
			{ nameof(Code), Code },
			{ nameof(Detail), Detail },
			{ nameof(IdInboxInstance), IdInboxInstance },
		};

	public override string? ToString()
	{
		return IdInboxMessageProcessingLog.ToString();
	}
}
