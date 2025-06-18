using Legion.Validation;

namespace Legion.ADF.ESB.Components.Model;

public sealed partial class Job : Components.ComponentsBaseEntity, Legion.Model.IEntity
{
	private List<Components.Model.JobData> _jobData;
	private List<Components.Model.JobLog> _jobLogs;

	public static IValidator<Job> DefaultDBValidator { get; }

	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid IdJob { get; private set; }

	/// <summary>
	/// Database DataType: varchar(127) NOT NULL
	/// </summary>
	public string Code { get; private set; }

	/// <summary>
	/// Database DataType: varchar(127) NOT NULL
	/// </summary>
	public string Name { get; private set; }

	/// <summary>
	/// Database DataType: varchar(1023) NULL
	/// </summary>
	public string? Description { get; private set; }

	/// <summary>
	/// Database DataType: uuid NOT NULL | Components.Model.JobType.JobType | FK_Job_IdJobType
	/// </summary>
	public Guid IdJobType { get; private set; }

	/// <summary>
	/// Database DataType: uuid NOT NULL | Components.Model.JobStatus.JobStatus | FK_Job_IdJobStatus
	/// </summary>
	public Guid IdJobStatus { get; private set; }

	/// <summary>
	/// Database DataType: varchar(2047) NOT NULL
	/// </summary>
	public string Class { get; private set; }

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
	/// Database DataType: timestamp without time zone NOT NULL
	/// </summary>
	public DateTime LastExecutionUtc { get; private set; }

	/// <summary>
	/// Database DataType: timestamp without time zone NOT NULL
	/// </summary>
	public DateTime NextExecutionUtc { get; private set; }

	/// <summary>
	/// Database DataType: integer NOT NULL
	/// </summary>
	public int ExecutionEstimatedTimeInSeconds { get; private set; }

	/// <summary>
	/// Database DataType: integer NOT NULL
	/// </summary>
	public int DeclaringOfflineAfterMinutesOfInactivity { get; private set; }


	/// <summary>
	/// _1:N Guid IdJobStatus | FK_Job_IdJobStatus
	/// </summary>
	public Components.Model.JobStatus JobStatus { get; private set; }

	/// <summary>
	/// _1:N Guid IdJobType | FK_Job_IdJobType
	/// </summary>
	public Components.Model.JobType JobType { get; private set; }


	/// <summary>
	/// N:_1 Components.Model.JobData.IdJob | FK_JobData_IdJob
	/// </summary>
	public IReadOnlyList<Components.Model.JobData> JobData => _jobData;

	/// <summary>
	/// N:_1 Components.Model.JobLog.IdJob | FK_JobLog_IdJob
	/// </summary>
	public IReadOnlyList<Components.Model.JobLog> JobLogs => _jobLogs;

	private Job()
	{
		_jobData = [];
		_jobLogs = [];
	}

	static Job()
	{
		DefaultDBValidator = SetDBValidatorRules(new ValidatorBuilder<Job>()).Build();
	}

	public override string? ToString()
	{
		return IdJob.ToString();
	}

	public static ValidatorBuilder<Job> SetDBValidatorRules(ValidatorBuilder<Job> builder)
		=> builder
			.ForProperty(x => x.IdJob, v => v.NotDefaultOrEmpty())
			.ForProperty(x => x.Code, v => v.NotDefaultOrEmpty().MaxLength(127))
			.ForProperty(x => x.Name, v => v.NotDefaultOrEmpty().MaxLength(127))
			.ForProperty(x => x.Description, v => v.MaxLength(1023))
			.ForProperty(x => x.IdJobType, v => v.NotDefaultOrEmpty(), x => x.JobType == null)
			.ForProperty(x => x.IdJobStatus, v => v.NotDefaultOrEmpty(), x => x.JobStatus == null)
			.ForProperty(x => x.Class, v => v.NotDefaultOrEmpty().MaxLength(2047))
			.ForProperty(x => x.CronExpression, v => v.MaxLength(63))
			//.ForProperty(x => x.LastExecutionUtc, v => v.NotDefaultOrEmpty())
			//.ForProperty(x => x.NextExecutionUtc, v => v.NotDefaultOrEmpty())
			//.ForProperty(x => x.ExecutionEstimatedTimeInSeconds, v => v.NotDefaultOrEmpty())
			//.ForProperty(x => x.DeclaringOfflineAfterMinutesOfInactivity, v => v.NotDefaultOrEmpty())
		;
}
