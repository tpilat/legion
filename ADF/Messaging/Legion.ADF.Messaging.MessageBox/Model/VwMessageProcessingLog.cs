namespace Legion.ADF.Messaging.MessageBox.Model;

public sealed partial class VwMessageProcessingLog : MessageBox.MessageBoxBaseQueryEntity, Legion.Model.IQueryEntity
{
	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid IdMessageProcessingLog { get; private set; }

	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid IdMessage { get; private set; }

	/// <summary>
	/// Database DataType: uuid NULL
	/// </summary>
	public Guid? IdQueuedMessage { get; private set; }

	/// <summary>
	/// Database DataType: uuid NULL
	/// </summary>
	public Guid? IdSubscribedMessage { get; private set; }

	/// <summary>
	/// Database DataType: timestamp with time zone NOT NULL
	/// </summary>
	public DateTime CreatedUtc { get; private set; }

	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid IdMessageProcessingStatus { get; private set; }

	/// <summary>
	/// Database DataType: varchar(63) NOT NULL
	/// </summary>
	public string MessageProcessingStatusCode { get; private set; }

	/// <summary>
	/// Database DataType: varchar(127) NOT NULL
	/// </summary>
	public string MessageProcessingStatusName { get; private set; }

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


	private VwMessageProcessingLog()
	{
	}

	public Dictionary<string, object?> ToDictionary()
		=> new()
		{
			{ nameof(IdMessageProcessingLog), IdMessageProcessingLog },
			{ nameof(IdMessage), IdMessage },
			{ nameof(IdQueuedMessage), IdQueuedMessage },
			{ nameof(IdSubscribedMessage), IdSubscribedMessage },
			{ nameof(CreatedUtc), CreatedUtc },
			{ nameof(IdMessageProcessingStatus), IdMessageProcessingStatus },
			{ nameof(MessageProcessingStatusCode), MessageProcessingStatusCode },
			{ nameof(MessageProcessingStatusName), MessageProcessingStatusName },
			{ nameof(TraceCorrelationId), TraceCorrelationId },
			{ nameof(IdLogMessage), IdLogMessage },
			{ nameof(Code), Code },
			{ nameof(Detail), Detail },
		};

	public override string? ToString()
	{
		return IdMessageProcessingLog.ToString();
	}
}
