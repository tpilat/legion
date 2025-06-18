namespace Legion.ADF.Messaging.Outbox.Model;

public sealed partial class VwBlockedOutboxMessageType : Outbox.OutboxBaseQueryEntity, Legion.Model.IQueryEntity
{
	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid IdBlockedOutboxMessageType { get; private set; }

	/// <summary>
	/// Database DataType: varchar(1023) NOT NULL
	/// </summary>
	public string Namespace { get; private set; }

	/// <summary>
	/// Database DataType: timestamp with time zone NOT NULL
	/// </summary>
	public DateTime CreatedUtc { get; private set; }

	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid IdOutboxInstance { get; private set; }


	private VwBlockedOutboxMessageType()
	{
	}

	public Dictionary<string, object?> ToDictionary()
		=> new()
		{
			{ nameof(IdBlockedOutboxMessageType), IdBlockedOutboxMessageType },
			{ nameof(Namespace), Namespace },
			{ nameof(CreatedUtc), CreatedUtc },
			{ nameof(IdOutboxInstance), IdOutboxInstance },
		};

	public override string? ToString()
	{
		return IdBlockedOutboxMessageType.ToString();
	}
}
