using Legion.Validation;

namespace Legion.ADF.ServiceBus.Model;

public sealed partial class Job : ServiceBus.ServiceBusBaseEntity, Legion.Model.Concurrence.IConcurrent, Legion.Model.IEntity
{
	private List<ServiceBus.Model.JobData> _jobDatas;
	private List<ServiceBus.Model.JobExecution> _jobExecutions;
	private List<ServiceBus.Model.JobLog> _jobLogs;
	private List<ServiceBus.Model.JobMessage> _jobMessages;
	private List<ServiceBus.Model.JobStatistics> _jobStatistics;

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
	/// Database DataType: uuid NOT NULL | ServiceBus.Model.JobRunType.JobRunType | FK_Job_IdJobRunType
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
	public Guid RowVersion { get; set; }


	/// <summary>
	/// _1:N Guid IdJobRunType | FK_Job_IdJobRunType
	/// </summary>
	public ServiceBus.Model.JobRunType JobRunType { get; private set; }


	/// <summary>
	/// 1:_1 JobActivity.IdJob | FK_JobActivity_IdJob
	/// </summary>
	public ServiceBus.Model.JobActivity JobActivity { get; private set; }

	/// <summary>
	/// N:_1 ServiceBus.Model.JobData.IdJob | FK_JobData_IdJob
	/// </summary>
	public IReadOnlyList<ServiceBus.Model.JobData> JobDatas => _jobDatas;

	/// <summary>
	/// N:_1 ServiceBus.Model.JobExecution.IdJob | FK_JobExecution_IdJob
	/// </summary>
	public IReadOnlyList<ServiceBus.Model.JobExecution> JobExecutions => _jobExecutions;

	/// <summary>
	/// N:_1 ServiceBus.Model.JobLog.IdJob | FK_JobLog_IdJob
	/// </summary>
	public IReadOnlyList<ServiceBus.Model.JobLog> JobLogs => _jobLogs;

	/// <summary>
	/// N:_1 ServiceBus.Model.JobMessage.IdJob | FK_JobMessage_IdJob
	/// </summary>
	public IReadOnlyList<ServiceBus.Model.JobMessage> JobMessages => _jobMessages;

	/// <summary>
	/// N:_1 ServiceBus.Model.JobStatistics.IdJob | FK_JobStatistics_IdJob
	/// </summary>
	public IReadOnlyList<ServiceBus.Model.JobStatistics> JobStatistics => _jobStatistics;

	private Job()
	{
		_jobDatas = [];
		_jobExecutions = [];
		_jobLogs = [];
		_jobMessages = [];
		_jobStatistics = [];
	}

	[System.ComponentModel.DataAnnotations.Schema.NotMapped]
	string Legion.Model.Concurrence.IConcurrent.ConcurrencyTokenPropertyName => nameof(RowVersion);

	public void SetNewConcurrencyToken()
	{
		RowVersion = Legion.GlobalContext.Instance.NewGuid();
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
			.ForProperty(x => x.Namespace, v => v.NotDefaultOrEmpty().MaxLength(1023))
			.ForProperty(x => x.CronExpression, v => v.MaxLength(63))
			//.ForProperty(x => x.IdDefaultHost, v => v.NotDefaultOrEmpty())
			//.ForProperty(x => x.TimeoutForProcessingInSeconds, v => v.NotDefaultOrEmpty())
			//.ForProperty(x => x.RowVersion, v => v.NotDefaultOrEmpty())
		;
}
