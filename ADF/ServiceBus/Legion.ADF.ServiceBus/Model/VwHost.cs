namespace Legion.ADF.ServiceBus.Model;

public sealed partial class VwHost : ServiceBus.ServiceBusBaseQueryEntity, Legion.Model.IQueryEntity
{
	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid IdHost { get; private set; }

	/// <summary>
	/// Database DataType: varchar(255) NOT NULL
	/// </summary>
	public string Name { get; private set; }

	/// <summary>
	/// Database DataType: varchar(511) NOT NULL
	/// </summary>
	public string Description { get; private set; }

	/// <summary>
	/// Database DataType: timestamp with time zone NOT NULL
	/// </summary>
	public DateTime CreatedUtc { get; private set; }

	/// <summary>
	/// Database DataType: boolean NOT NULL
	/// </summary>
	public bool IsEnabled { get; private set; }

	/// <summary>
	/// Database DataType: timestamp with time zone NULL
	/// </summary>
	public DateTime? StartedUtc { get; private set; }

	/// <summary>
	/// Database DataType: timestamp with time zone NOT NULL
	/// </summary>
	public DateTime LastActivityUtc { get; private set; }

	/// <summary>
	/// Database DataType: timestamp with time zone NULL
	/// </summary>
	public DateTime? StoppedUtc { get; private set; }

	/// <summary>
	/// Database DataType: jsonb NOT NULL
	/// </summary>
	public string Configuration { get; private set; }

	/// <summary>
	/// Database DataType: boolean NULL
	/// </summary>
	public bool? IsDistributedManagerAvailable { get; private set; }


	private VwHost()
	{
	}

	public Dictionary<string, object?> ToDictionary()
		=> new()
		{
			{ nameof(IdHost), IdHost },
			{ nameof(Name), Name },
			{ nameof(Description), Description },
			{ nameof(CreatedUtc), CreatedUtc },
			{ nameof(IsEnabled), IsEnabled },
			{ nameof(StartedUtc), StartedUtc },
			{ nameof(LastActivityUtc), LastActivityUtc },
			{ nameof(StoppedUtc), StoppedUtc },
			{ nameof(Configuration), Configuration },
			{ nameof(IsDistributedManagerAvailable), IsDistributedManagerAvailable },
		};

	public override string? ToString()
	{
		return IdHost.ToString();
	}
}
