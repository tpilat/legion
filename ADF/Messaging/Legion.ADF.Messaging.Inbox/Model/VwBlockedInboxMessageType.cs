namespace Legion.ADF.Messaging.Inbox.Model;

public sealed partial class VwBlockedInboxMessageType : Inbox.InboxBaseQueryEntity, Legion.Model.IQueryEntity
{
	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid IdBlockedInboxMessageType { get; private set; }

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
	public Guid IdInboxInstance { get; private set; }


	private VwBlockedInboxMessageType()
	{
	}

	public Dictionary<string, object?> ToDictionary()
		=> new()
		{
			{ nameof(IdBlockedInboxMessageType), IdBlockedInboxMessageType },
			{ nameof(Namespace), Namespace },
			{ nameof(CreatedUtc), CreatedUtc },
			{ nameof(IdInboxInstance), IdInboxInstance },
		};

	public override string? ToString()
	{
		return IdBlockedInboxMessageType.ToString();
	}
}
