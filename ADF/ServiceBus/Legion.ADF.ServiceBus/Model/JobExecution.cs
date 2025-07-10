using Legion.Validation;

namespace Legion.ADF.ServiceBus.Model;

public sealed partial class JobExecution : ServiceBus.ServiceBusBaseEntity, Legion.Model.IEntity
{
	public static IValidator<JobExecution> DefaultDBValidator { get; }

	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid IdJobExecution { get; private set; }

	/// <summary>
	/// Database DataType: uuid NOT NULL | ServiceBus.Model.Job.Job | FK_JobExecution_IdJob
	/// </summary>
	public Guid IdJob { get; private set; }

	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid TraceCorrelationId { get; private set; }

	/// <summary>
	/// Database DataType: timestamp with time zone NOT NULL
	/// </summary>
	public DateTime StartUtc { get; private set; }

	/// <summary>
	/// Database DataType: timestamp with time zone NULL
	/// </summary>
	public DateTime? EndUtc { get; private set; }

	/// <summary>
	/// Database DataType: uuid NOT NULL | ServiceBus.Model.JobStatus.JobStatus | FK_JobExecution_IdJobStatus
	/// </summary>
	public Guid IdJobStatus { get; private set; }


	/// <summary>
	/// _1:N Guid IdJob | FK_JobExecution_IdJob
	/// </summary>
	public ServiceBus.Model.Job Job { get; private set; }

	/// <summary>
	/// _1:N Guid IdJobStatus | FK_JobExecution_IdJobStatus
	/// </summary>
	public ServiceBus.Model.JobStatus JobStatus { get; private set; }

	private JobExecution()
	{
	}

	static JobExecution()
	{
		DefaultDBValidator = SetDBValidatorRules(new ValidatorBuilder<JobExecution>()).Build();
	}

	public Dictionary<string, object?> ToDictionary()
		=> new()
		{
			{ nameof(IdJobExecution), IdJobExecution },
			{ nameof(IdJob), IdJob },
			{ nameof(TraceCorrelationId), TraceCorrelationId },
			{ nameof(StartUtc), StartUtc },
			{ nameof(EndUtc), EndUtc },
			{ nameof(IdJobStatus), IdJobStatus },
		};

	public void TrimStringValuesToFitDatabaseMaxLengths(string? postfix = null)
	{
	}

	public override string? GetPrimaryKeyValue()
	{
		return IdJobExecution.ToString();
	}

	public override string? ToString()
	{
		return IdJobExecution.ToString();
	}

	public static ValidatorBuilder<JobExecution> SetDBValidatorRules(ValidatorBuilder<JobExecution> builder)
		=> builder
			.ForProperty(x => x.IdJobExecution, v => v.NotDefaultOrEmpty())
			.ForProperty(x => x.IdJob, v => v.NotDefaultOrEmpty(), (x, parent) => x.Job == null)
			//.ForProperty(x => x.TraceCorrelationId, v => v.NotDefaultOrEmpty())
			//.ForProperty(x => x.StartUtc, v => v.NotDefaultOrEmpty())
			.ForProperty(x => x.IdJobStatus, v => v.NotDefaultOrEmpty(), (x, parent) => x.JobStatus == null)
		;
}
