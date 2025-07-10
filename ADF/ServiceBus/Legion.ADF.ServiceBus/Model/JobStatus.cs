using Legion.Validation;

namespace Legion.ADF.ServiceBus.Model;

public sealed partial class JobStatus : ServiceBus.ServiceBusBaseEntity, Legion.Model.IEntity
{
	private List<ServiceBus.Model.JobExecution> _jobExecutions;
	private List<ServiceBus.Model.JobLog> _jobLogs;
	private List<ServiceBus.Model.Job> _jobs;

	public static IValidator<JobStatus> DefaultDBValidator { get; }

	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid IdJobStatus { get; private set; }

	/// <summary>
	/// Database DataType: varchar(63) NOT NULL
	/// </summary>
	public string Code { get; private set; }

	/// <summary>
	/// Database DataType: varchar(63) NOT NULL
	/// </summary>
	public string Name { get; private set; }


	/// <summary>
	/// N:_1 ServiceBus.Model.JobExecution.IdJobStatus | FK_JobExecution_IdJobStatus
	/// </summary>
	public IReadOnlyList<ServiceBus.Model.JobExecution> JobExecutions => _jobExecutions;

	/// <summary>
	/// N:_1 ServiceBus.Model.JobLog.IdJobStatus | FK_JobLog_IdJobStatus
	/// </summary>
	public IReadOnlyList<ServiceBus.Model.JobLog> JobLogs => _jobLogs;

	/// <summary>
	/// N:_1 ServiceBus.Model.Job.IdJobStatus | FK_Job_IdJobStatus
	/// </summary>
	public IReadOnlyList<ServiceBus.Model.Job> Jobs => _jobs;

	private JobStatus()
	{
		_jobExecutions = [];
		_jobLogs = [];
		_jobs = [];
	}

	public Dictionary<string, object?> ToDictionary()
		=> new()
		{
			{ nameof(IdJobStatus), IdJobStatus },
			{ nameof(Code), Code },
			{ nameof(Name), Name },
		};

	public void TrimStringValuesToFitDatabaseMaxLengths(string? postfix = null)
	{
		Code = Legion.Text.StringHelper.TrimToFitMaxLength(Code, 63, postfix);
		Name = Legion.Text.StringHelper.TrimToFitMaxLength(Name, 63, postfix);
	}

	public override string? GetPrimaryKeyValue()
	{
		return IdJobStatus.ToString();
	}

	public override string? ToString()
	{
		return Code;
	}

	public static ValidatorBuilder<JobStatus> SetDBValidatorRules(ValidatorBuilder<JobStatus> builder)
		=> builder
			.ForProperty(x => x.IdJobStatus, v => v.NotDefaultOrEmpty())
			.ForProperty(x => x.Code, v => v.NotDefaultOrEmpty().MaxLength(63))
			.ForProperty(x => x.Name, v => v.NotDefaultOrEmpty().MaxLength(63))
		;
}
