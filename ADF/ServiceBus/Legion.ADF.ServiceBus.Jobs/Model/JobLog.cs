using Legion.Validation;

namespace Legion.ADF.ServiceBus.Jobs.Model;

public sealed partial class JobLog : Jobs.JobsBaseEntity, Legion.Model.IEntity
{
	public static IValidator<JobLog> DefaultDBValidator { get; }

	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid IdJobLog { get; private set; }

	/// <summary>
	/// Database DataType: uuid NOT NULL | Jobs.Model.Job.Job | FK_JobLog_IdJob
	/// </summary>
	public Guid IdJob { get; private set; }

	/// <summary>
	/// Database DataType: integer NOT NULL
	/// </summary>
	public int IdLogLevel { get; private set; }

	/// <summary>
	/// Database DataType: timestamp with time zone NOT NULL
	/// </summary>
	public DateTime CreatedUtc { get; private set; }

	/// <summary>
	/// Database DataType: uuid NOT NULL | Jobs.Model.JobStatus.JobStatus | FK_JobLog_IdJobStatus
	/// </summary>
	public Guid IdJobStatus { get; private set; }

	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid TraceCorrelationId { get; private set; }

	/// <summary>
	/// Database DataType: uuid NULL
	/// </summary>
	public Guid? IdLogMessage { get; private set; }

	/// <summary>
	/// Database DataType: varchar(127) NOT NULL
	/// </summary>
	public string Code { get; private set; }

	/// <summary>
	/// Database DataType: text NULL
	/// </summary>
	public string? Detail { get; private set; }

	/// <summary>
	/// Database DataType: uuid NULL
	/// </summary>
	public Guid? IdMessageProcessingLog { get; private set; }


	/// <summary>
	/// _1:N Guid IdJob | FK_JobLog_IdJob
	/// </summary>
	public Jobs.Model.Job Job { get; private set; }

	/// <summary>
	/// _1:N Guid IdJobStatus | FK_JobLog_IdJobStatus
	/// </summary>
	public Jobs.Model.JobStatus JobStatus { get; private set; }

	private JobLog()
	{
	}

	static JobLog()
	{
		DefaultDBValidator = SetDBValidatorRules(new ValidatorBuilder<JobLog>()).Build();
	}

	public Dictionary<string, object?> ToDictionary()
		=> new()
		{
			{ nameof(IdJobLog), IdJobLog },
			{ nameof(IdJob), IdJob },
			{ nameof(IdLogLevel), IdLogLevel },
			{ nameof(CreatedUtc), CreatedUtc },
			{ nameof(IdJobStatus), IdJobStatus },
			{ nameof(TraceCorrelationId), TraceCorrelationId },
			{ nameof(IdLogMessage), IdLogMessage },
			{ nameof(Code), Code },
			{ nameof(Detail), Detail },
			{ nameof(IdMessageProcessingLog), IdMessageProcessingLog },
		};

	public void TrimStringValuesToFitDatabaseMaxLengths(string? postfix = null)
	{
		Code = Legion.Text.StringHelper.TrimToFitMaxLength(Code, 127, postfix);
	}

	public override string? GetPrimaryKeyValue()
	{
		return IdJobLog.ToString();
	}

	public override string? ToString()
	{
		return IdJobLog.ToString();
	}

	public static ValidatorBuilder<JobLog> SetDBValidatorRules(ValidatorBuilder<JobLog> builder)
		=> builder
			.ForProperty(x => x.IdJobLog, v => v.NotDefaultOrEmpty())
			.ForProperty(x => x.IdJob, v => v.NotDefaultOrEmpty(), (x, parent) => x.Job == null)
			//.ForProperty(x => x.IdLogLevel, v => v.NotDefaultOrEmpty())
			//.ForProperty(x => x.CreatedUtc, v => v.NotDefaultOrEmpty())
			.ForProperty(x => x.IdJobStatus, v => v.NotDefaultOrEmpty(), (x, parent) => x.JobStatus == null)
			//.ForProperty(x => x.TraceCorrelationId, v => v.NotDefaultOrEmpty())
			.ForProperty(x => x.Code, v => v.NotDefaultOrEmpty().MaxLength(127))
		;
}
