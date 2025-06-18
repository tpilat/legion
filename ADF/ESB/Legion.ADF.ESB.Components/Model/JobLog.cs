using Legion.Validation;

namespace Legion.ADF.ESB.Components.Model;

public sealed partial class JobLog : Components.ComponentsBaseEntity, Legion.Model.IEntity
{
	public static IValidator<JobLog> DefaultDBValidator { get; }

	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid IdJobLog { get; private set; }

	/// <summary>
	/// Database DataType: uuid NOT NULL | Components.Model.Job.Job | FK_JobLog_IdJob
	/// </summary>
	public Guid IdJob { get; private set; }

	/// <summary>
	/// Database DataType: timestamp without time zone NOT NULL
	/// </summary>
	public DateTime CreatedUtc { get; private set; }

	/// <summary>
	/// Database DataType: integer NOT NULL
	/// </summary>
	public int IdLogLevel { get; private set; }

	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid LogCorrelationId { get; private set; }

	/// <summary>
	/// Database DataType: uuid NOT NULL | Components.Model.JobStatus.JobStatus | FK_JobLog_IdJobStatus
	/// </summary>
	public Guid IdJobStatus { get; private set; }

	/// <summary>
	/// Database DataType: text NOT NULL
	/// </summary>
	public string Detail { get; private set; }

	/// <summary>
	/// Database DataType: jsonb NULL
	/// </summary>
	public string? Data { get; private set; }

	/// <summary>
	/// Database DataType: uuid NULL
	/// </summary>
	public Guid? IdLogMessage { get; private set; }

	/// <summary>
	/// Database DataType: uuid NULL | --NO TARGET-- | FK_JobLog_IdMessageProcessingLog
	/// </summary>
	public Guid? IdMessageProcessingLog { get; private set; }


	/// <summary>
	/// _1:N Guid IdJob | FK_JobLog_IdJob
	/// </summary>
	public Components.Model.Job Job { get; private set; }

	/// <summary>
	/// _1:N Guid IdJobStatus | FK_JobLog_IdJobStatus
	/// </summary>
	public Components.Model.JobStatus JobStatus { get; private set; }

	private JobLog()
	{
	}

	static JobLog()
	{
		DefaultDBValidator = SetDBValidatorRules(new ValidatorBuilder<JobLog>()).Build();
	}

	public override string? ToString()
	{
		return IdJobLog.ToString();
	}

	public static ValidatorBuilder<JobLog> SetDBValidatorRules(ValidatorBuilder<JobLog> builder)
		=> builder
			.ForProperty(x => x.IdJobLog, v => v.NotDefaultOrEmpty())
			.ForProperty(x => x.IdJob, v => v.NotDefaultOrEmpty(), x => x.Job == null)
			//.ForProperty(x => x.CreatedUtc, v => v.NotDefaultOrEmpty())
			//.ForProperty(x => x.IdLogLevel, v => v.NotDefaultOrEmpty())
			//.ForProperty(x => x.LogCorrelationId, v => v.NotDefaultOrEmpty())
			.ForProperty(x => x.IdJobStatus, v => v.NotDefaultOrEmpty(), x => x.JobStatus == null)
			.ForProperty(x => x.Detail, v => v.NotDefaultOrEmpty())
		;
}
