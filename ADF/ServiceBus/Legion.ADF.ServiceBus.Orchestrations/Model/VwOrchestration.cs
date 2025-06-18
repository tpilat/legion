namespace Legion.ADF.ServiceBus.Orchestrations.Model;

public sealed partial class VwOrchestration : Orchestrations.OrchestrationsBaseQueryEntity, Legion.Model.IQueryEntity
{
	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid IdOrchestration { get; private set; }

	/// <summary>
	/// Database DataType: varchar(255) NULL
	/// </summary>
	public string? Name { get; private set; }

	/// <summary>
	/// Database DataType: varchar(1023) NULL
	/// </summary>
	public string? Description { get; private set; }

	/// <summary>
	/// Database DataType: boolean NULL
	/// </summary>
	public bool? IsSingleton { get; private set; }

	/// <summary>
	/// Database DataType: varchar(1023) NULL
	/// </summary>
	public string? Namespace { get; private set; }

	/// <summary>
	/// Database DataType: varchar(31) NULL
	/// </summary>
	public string? Version { get; private set; }

	/// <summary>
	/// Database DataType: jsonb NULL
	/// </summary>
	public string? Properties { get; private set; }


	private VwOrchestration()
	{
	}

	public Dictionary<string, object?> ToDictionary()
		=> new()
		{
			{ nameof(IdOrchestration), IdOrchestration },
			{ nameof(Name), Name },
			{ nameof(Description), Description },
			{ nameof(IsSingleton), IsSingleton },
			{ nameof(Namespace), Namespace },
			{ nameof(Version), Version },
			{ nameof(Properties), Properties },
		};

	public override string? ToString()
	{
		return IdOrchestration.ToString();
	}
}
