namespace Legion.ADF.ESB.Components.Model;

public partial class VwJob : Components.ComponentsBaseQueryEntity, Legion.Model.IQueryEntity
{
	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid IdJob { get; private set; }

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
	/// Database DataType: uuid NULL
	/// </summary>
	public Guid? IdJobType { get; private set; }

	/// <summary>
	/// Database DataType: uuid NULL
	/// </summary>
	public Guid? IdJobStatus { get; private set; }

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
	/// Database DataType: boolean NULL
	/// </summary>
	public bool? CronExpressionIncludeSeconds { get; private set; }

	/// <summary>
	/// Database DataType: timestamp without time zone NULL
	/// </summary>
	public DateTime? LastExecutionUtc { get; private set; }

	/// <summary>
	/// Database DataType: timestamp without time zone NULL
	/// </summary>
	public DateTime? NextExecutionUtc { get; private set; }

	/// <summary>
	/// Database DataType: integer NULL
	/// </summary>
	public int? ExecutionEstimatedTimeInSeconds { get; private set; }

	/// <summary>
	/// Database DataType: integer NULL
	/// </summary>
	public int? DeclaringOfflineAfterMinutesOfInactivity { get; private set; }


	private VwJob()
	{
	}

	public override string? ToString()
	{
		return IdJob.ToString();
	}
}
