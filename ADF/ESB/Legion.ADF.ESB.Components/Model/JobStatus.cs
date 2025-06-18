using Legion.Validation;

namespace Legion.ADF.ESB.Components.Model;

public sealed partial class JobStatus : Components.ComponentsBaseEntity, Legion.Model.IEntity
{
	private List<Components.Model.JobLog> _jobLogs;
	private List<Components.Model.Job> _jobs;

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
	/// Database DataType: varchar(127) NOT NULL
	/// </summary>
	public string Name { get; private set; }


	/// <summary>
	/// N:_1 Components.Model.JobLog.IdJobStatus | FK_JobLog_IdJobStatus
	/// </summary>
	public IReadOnlyList<Components.Model.JobLog> JobLogs => _jobLogs;

	/// <summary>
	/// N:_1 Components.Model.Job.IdJobStatus | FK_Job_IdJobStatus
	/// </summary>
	public IReadOnlyList<Components.Model.Job> Jobs => _jobs;

	private JobStatus()
	{
		_jobLogs = [];
		_jobs = [];
	}

	public override string? ToString()
	{
		return Code;
	}

	public static ValidatorBuilder<JobStatus> SetDBValidatorRules(ValidatorBuilder<JobStatus> builder)
		=> builder
			.ForProperty(x => x.IdJobStatus, v => v.NotDefaultOrEmpty())
			.ForProperty(x => x.Code, v => v.NotDefaultOrEmpty().MaxLength(63))
			.ForProperty(x => x.Name, v => v.NotDefaultOrEmpty().MaxLength(127))
		;
}
