namespace Legion.ADF.ServiceBus.Model;

public sealed partial class VwJob : ServiceBus.ServiceBusBaseQueryEntity, Legion.Model.IQueryEntity
{
	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid IdJob { get; private set; }

	/// <summary>
	/// Database DataType: varchar(255) NOT NULL
	/// </summary>
	public string Name { get; private set; }

	/// <summary>
	/// Database DataType: varchar(1023) NULL
	/// </summary>
	public string? Description { get; private set; }

	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid IdJobRunType { get; private set; }

	/// <summary>
	/// Database DataType: varchar(1023) NOT NULL
	/// </summary>
	public string Namespace { get; private set; }

	/// <summary>
	/// Database DataType: jsonb NULL
	/// </summary>
	public string? Properties { get; private set; }

	/// <summary>
	/// Database DataType: integer NULL
	/// </summary>
	public int? DelayedStartInSeconds { get; private set; }

	/// <summary>
	/// Database DataType: integer NULL
	/// </summary>
	public int? IdleTimeoutInSeconds { get; private set; }

	/// <summary>
	/// Database DataType: varchar(63) NULL
	/// </summary>
	public string? CronExpression { get; private set; }

	/// <summary>
	/// Database DataType: boolean NOT NULL
	/// </summary>
	public bool CronExpressionIncludeSeconds { get; private set; }

	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid IdDefaultHost { get; private set; }

	/// <summary>
	/// Database DataType: boolean NOT NULL
	/// </summary>
	public bool RequestedToDisable { get; private set; }

	/// <summary>
	/// Database DataType: integer NOT NULL
	/// </summary>
	public int TimeoutForProcessingInSeconds { get; private set; }

	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid RowVersion { get; private set; }


	private VwJob()
	{
	}

	public Dictionary<string, object?> ToDictionary()
		=> new()
		{
			{ nameof(IdJob), IdJob },
			{ nameof(Name), Name },
			{ nameof(Description), Description },
			{ nameof(IdJobRunType), IdJobRunType },
			{ nameof(Namespace), Namespace },
			{ nameof(Properties), Properties },
			{ nameof(DelayedStartInSeconds), DelayedStartInSeconds },
			{ nameof(IdleTimeoutInSeconds), IdleTimeoutInSeconds },
			{ nameof(CronExpression), CronExpression },
			{ nameof(CronExpressionIncludeSeconds), CronExpressionIncludeSeconds },
			{ nameof(IdDefaultHost), IdDefaultHost },
			{ nameof(RequestedToDisable), RequestedToDisable },
			{ nameof(TimeoutForProcessingInSeconds), TimeoutForProcessingInSeconds },
			{ nameof(RowVersion), RowVersion },
		};

	public override string? ToString()
	{
		return IdJob.ToString();
	}
}
