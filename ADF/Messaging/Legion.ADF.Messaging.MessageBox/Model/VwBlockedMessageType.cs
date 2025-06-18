namespace Legion.ADF.Messaging.MessageBox.Model;

public sealed partial class VwBlockedMessageType : MessageBox.MessageBoxBaseQueryEntity, Legion.Model.IQueryEntity
{
	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid IdBlockedMessageType { get; private set; }

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
	public Guid IdMessageBoxInstance { get; private set; }


	private VwBlockedMessageType()
	{
	}

	public Dictionary<string, object?> ToDictionary()
		=> new()
		{
			{ nameof(IdBlockedMessageType), IdBlockedMessageType },
			{ nameof(Namespace), Namespace },
			{ nameof(CreatedUtc), CreatedUtc },
			{ nameof(IdMessageBoxInstance), IdMessageBoxInstance },
		};

	public override string? ToString()
	{
		return IdBlockedMessageType.ToString();
	}
}
