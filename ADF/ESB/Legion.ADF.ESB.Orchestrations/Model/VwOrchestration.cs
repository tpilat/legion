namespace Legion.ADF.ESB.Orchestrations.Model;

public partial class VwOrchestration : Orchestrations.OrchestrationsBaseQueryEntity, Legion.Model.IQueryEntity
{
	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid IdOrchestration { get; private set; }

	/// <summary>
	/// Database DataType: varchar(127) NULL
	/// </summary>
	public string? Code { get; private set; }

	/// <summary>
	/// Database DataType: varchar(127) NULL
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
	/// Database DataType: varchar(2047) NULL
	/// </summary>
	public string? Class { get; private set; }

	/// <summary>
	/// Database DataType: jsonb NULL
	/// </summary>
	public string? Properties { get; private set; }

	/// <summary>
	/// Database DataType: integer NULL
	/// </summary>
	public int? TimeoutForMessageProcessingInSeconds { get; private set; }

	/// <summary>
	/// Database DataType: integer NULL
	/// </summary>
	public int? MaxMessageProcessingRetryCount { get; private set; }

	/// <summary>
	/// Database DataType: varchar(31) NULL
	/// </summary>
	public string? Version { get; private set; }

	/// <summary>
	/// Database DataType: timestamp without time zone NULL
	/// </summary>
	public DateTime? ValidTo { get; private set; }


	private VwOrchestration()
	{
	}

	public override string? ToString()
	{
		return IdOrchestration.ToString();
	}
}
