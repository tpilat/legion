using Legion.Validation;

namespace Legion.ADF.ServiceBus.Jobs.Model;

public sealed partial class Job : Jobs.JobsBaseEntity, Legion.Model.IEntity
{
	private List<Jobs.Model.JobData> _jobDatas;
	private List<Jobs.Model.JobExecution> _jobExecutions;
	private List<Jobs.Model.JobLog> _jobLogs;
	private List<Jobs.Model.JobMessage> _jobMessages;
	private List<Jobs.Model.JobStatistics> _jobStatistics;

	public static IValidator<Job> DefaultDBValidator { get; }

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
	/// Database DataType: uuid NOT NULL | Jobs.Model.JobRunType.JobRunType | FK_Job_IdJobRunType
	/// </summary>
	public Guid IdJobRunType { get; private set; }

	/// <summary>
	/// Database DataType: uuid NOT NULL | Jobs.Model.JobStatus.JobStatus | FK_Job_IdJobStatus
	/// </summary>
	public Guid IdJobStatus { get; private set; }

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
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid IdCurrentHost { get; private set; }

	/// <summary>
	/// Database DataType: timestamp with time zone NOT NULL
	/// </summary>
	public DateTime AttachedToCurrentHostUtc { get; private set; }

	/// <summary>
	/// Database DataType: timestamp with time zone NULL
	/// </summary>
	public DateTime? LastProcessingUtc { get; private set; }

	/// <summary>
	/// Database DataType: timestamp with time zone NULL
	/// </summary>
	public DateTime? LastProcessingFinishedUtc { get; private set; }

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


	/// <summary>
	/// _1:N Guid IdJobRunType | FK_Job_IdJobRunType
	/// </summary>
	public Jobs.Model.JobRunType JobRunType { get; private set; }

	/// <summary>
	/// _1:N Guid IdJobStatus | FK_Job_IdJobStatus
	/// </summary>
	public Jobs.Model.JobStatus JobStatus { get; private set; }


	/// <summary>
	/// N:_1 Jobs.Model.JobData.IdJob | FK_JobData_IdJob
	/// </summary>
	public IReadOnlyList<Jobs.Model.JobData> JobDatas => _jobDatas;

	/// <summary>
	/// N:_1 Jobs.Model.JobExecution.IdJob | FK_JobExecution_IdJob
	/// </summary>
	public IReadOnlyList<Jobs.Model.JobExecution> JobExecutions => _jobExecutions;

	/// <summary>
	/// N:_1 Jobs.Model.JobLog.IdJob | FK_JobLog_IdJob
	/// </summary>
	public IReadOnlyList<Jobs.Model.JobLog> JobLogs => _jobLogs;

	/// <summary>
	/// N:_1 Jobs.Model.JobMessage.IdJob | FK_JobMessage_IdJob
	/// </summary>
	public IReadOnlyList<Jobs.Model.JobMessage> JobMessages => _jobMessages;

	/// <summary>
	/// N:_1 Jobs.Model.JobStatistics.IdJob | FK_JobStatistics_IdJob
	/// </summary>
	public IReadOnlyList<Jobs.Model.JobStatistics> JobStatistics => _jobStatistics;

	private Job()
	{
		_jobDatas = [];
		_jobExecutions = [];
		_jobLogs = [];
		_jobMessages = [];
		_jobStatistics = [];
	}

	static Job()
	{
		DefaultDBValidator = SetDBValidatorRules(new ValidatorBuilder<Job>()).Build();
	}

	public Dictionary<string, object?> ToDictionary()
		=> new()
		{
			{ nameof(IdJob), IdJob },
			{ nameof(Name), Name },
			{ nameof(Description), Description },
			{ nameof(IdJobRunType), IdJobRunType },
			{ nameof(IdJobStatus), IdJobStatus },
			{ nameof(Namespace), Namespace },
			{ nameof(Properties), Properties },
			{ nameof(DelayedStartInSeconds), DelayedStartInSeconds },
			{ nameof(IdleTimeoutInSeconds), IdleTimeoutInSeconds },
			{ nameof(CronExpression), CronExpression },
			{ nameof(CronExpressionIncludeSeconds), CronExpressionIncludeSeconds },
			{ nameof(IdDefaultHost), IdDefaultHost },
			{ nameof(IdCurrentHost), IdCurrentHost },
			{ nameof(AttachedToCurrentHostUtc), AttachedToCurrentHostUtc },
			{ nameof(LastProcessingUtc), LastProcessingUtc },
			{ nameof(LastProcessingFinishedUtc), LastProcessingFinishedUtc },
			{ nameof(NextProcessinUtc), NextProcessinUtc },
			{ nameof(TimeoutForProcessingInSeconds), TimeoutForProcessingInSeconds },
			{ nameof(MaxProcessingRetryCount), MaxProcessingRetryCount },
		};

	public void TrimStringValuesToFitDatabaseMaxLengths(string? postfix = null)
	{
		Name = Legion.Text.StringHelper.TrimToFitMaxLength(Name, 255, postfix);
		Description = Legion.Text.StringHelper.TrimToFitMaxLength(Description, 1023, postfix);
		Namespace = Legion.Text.StringHelper.TrimToFitMaxLength(Namespace, 1023, postfix);
		CronExpression = Legion.Text.StringHelper.TrimToFitMaxLength(CronExpression, 63, postfix);
	}

	public override string? GetPrimaryKeyValue()
	{
		return IdJob.ToString();
	}

	public override string? ToString()
	{
		return IdJob.ToString();
	}

	public static ValidatorBuilder<Job> SetDBValidatorRules(ValidatorBuilder<Job> builder)
		=> builder
			.ForProperty(x => x.IdJob, v => v.NotDefaultOrEmpty())
			.ForProperty(x => x.Name, v => v.NotDefaultOrEmpty().MaxLength(255))
			.ForProperty(x => x.Description, v => v.MaxLength(1023))
			.ForProperty(x => x.IdJobRunType, v => v.NotDefaultOrEmpty(), (x, parent) => x.JobRunType == null)
			.ForProperty(x => x.IdJobStatus, v => v.NotDefaultOrEmpty(), (x, parent) => x.JobStatus == null)
			.ForProperty(x => x.Namespace, v => v.NotDefaultOrEmpty().MaxLength(1023))
			.ForProperty(x => x.CronExpression, v => v.MaxLength(63))
			//.ForProperty(x => x.IdDefaultHost, v => v.NotDefaultOrEmpty())
			//.ForProperty(x => x.IdCurrentHost, v => v.NotDefaultOrEmpty())
			//.ForProperty(x => x.AttachedToCurrentHostUtc, v => v.NotDefaultOrEmpty())
			//.ForProperty(x => x.NextProcessinUtc, v => v.NotDefaultOrEmpty())
			//.ForProperty(x => x.TimeoutForProcessingInSeconds, v => v.NotDefaultOrEmpty())
			//.ForProperty(x => x.MaxProcessingRetryCount, v => v.NotDefaultOrEmpty())
		;
}
