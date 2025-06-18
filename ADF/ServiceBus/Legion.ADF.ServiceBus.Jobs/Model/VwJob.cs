namespace Legion.ADF.ServiceBus.Jobs.Model;

public sealed partial class VwJob : Jobs.JobsBaseQueryEntity, Legion.Model.IQueryEntity
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
	/// Database DataType: varchar(63) NOT NULL
	/// </summary>
	public string JobRunType { get; private set; }

	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid IdJobStatus { get; private set; }

	/// <summary>
	/// Database DataType: varchar(63) NOT NULL
	/// </summary>
	public string JobStatus { get; private set; }

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
	/// Database DataType: timestamp with time zone NULL
	/// </summary>
	public DateTime? LastProcessingUtc { get; private set; }

	/// <summary>
	/// Database DataType: timestamp with time zone NOT NULL
	/// </summary>
	public DateTime NextProcessinUtc { get; private set; }

	/// <summary>
	/// Database DataType: integer NOT NULL
	/// </summary>
	public int TimeoutForProcessingInSeconds { get; private set; }

	/// <summary>
	/// Database DataType: integer NOT NULL
	/// </summary>
	public int MaxProcessingRetryCount { get; private set; }


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
			{ nameof(JobRunType), JobRunType },
			{ nameof(IdJobStatus), IdJobStatus },
			{ nameof(JobStatus), JobStatus },
			{ nameof(Namespace), Namespace },
			{ nameof(Properties), Properties },
			{ nameof(DelayedStartInSeconds), DelayedStartInSeconds },
			{ nameof(IdleTimeoutInSeconds), IdleTimeoutInSeconds },
			{ nameof(CronExpression), CronExpression },
			{ nameof(CronExpressionIncludeSeconds), CronExpressionIncludeSeconds },
			{ nameof(LastProcessingUtc), LastProcessingUtc },
			{ nameof(NextProcessinUtc), NextProcessinUtc },
			{ nameof(TimeoutForProcessingInSeconds), TimeoutForProcessingInSeconds },
			{ nameof(MaxProcessingRetryCount), MaxProcessingRetryCount },
		};

	public override string? ToString()
	{
		return IdJob.ToString();
	}
}
