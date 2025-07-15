using Legion.Validation;

namespace Legion.ADF.ServiceBus.Model;

public sealed partial class JobActivity : ServiceBus.ServiceBusBaseEntity, Legion.Model.Concurrence.IConcurrent, Legion.Model.IEntity
{
	public static IValidator<JobActivity> DefaultDBValidator { get; }

	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid IdJobActivity { get; private set; }

	/// <summary>
	/// Database DataType: uuid NOT NULL | ServiceBus.Model.Job.Job | FK_JobActivity_IdJob
	/// </summary>
	public Guid IdJob { get; private set; }

	/// <summary>
	/// Database DataType: uuid NOT NULL | ServiceBus.Model.JobStatus.JobStatus | FK_JobActivity_IdJobStatus
	/// </summary>
	public Guid IdJobStatus { get; private set; }

	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid IdCurrentHost { get; private set; }

	/// <summary>
	/// Database DataType: timestamp with time zone NOT NULL
	/// </summary>
	public DateTime AttachedToCurrentHostUtc { get; private set; }

	/// <summary>
	/// Database DataType: timestamp with time zone NOT NULL
	/// </summary>
	public DateTime LastStatusChangedUtc { get; private set; }

	/// <summary>
	/// Database DataType: timestamp with time zone NULL
	/// </summary>
	public DateTime? LastProcessingStartedUtc { get; private set; }

	/// <summary>
	/// Database DataType: timestamp with time zone NULL
	/// </summary>
	public DateTime? LastProcessingFinishedUtc { get; private set; }

	/// <summary>
	/// Database DataType: timestamp with time zone NULL
	/// </summary>
	public DateTime? DelayedToUtc { get; private set; }

	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid RowVersion { get; set; }


	/// <summary>
	/// UNIQUE INDEX: UQ_JobActivity_IdJob
	/// _1:1 Guid IdJob | FK_JobActivity_IdJob
	/// </summary>
	public ServiceBus.Model.Job Job { get; private set; }

	/// <summary>
	/// _1:N Guid IdJobStatus | FK_JobActivity_IdJobStatus
	/// </summary>
	public ServiceBus.Model.JobStatus JobStatus { get; private set; }

	private JobActivity()
	{
	}

	[System.ComponentModel.DataAnnotations.Schema.NotMapped]
	string Legion.Model.Concurrence.IConcurrent.ConcurrencyTokenPropertyName => nameof(RowVersion);

	public void SetNewConcurrencyToken()
	{
		RowVersion = Legion.GlobalContext.Instance.NewGuid();
	}

	static JobActivity()
	{
		DefaultDBValidator = SetDBValidatorRules(new ValidatorBuilder<JobActivity>()).Build();
	}

	public Dictionary<string, object?> ToDictionary()
		=> new()
		{
			{ nameof(IdJobActivity), IdJobActivity },
			{ nameof(IdJob), IdJob },
			{ nameof(IdJobStatus), IdJobStatus },
			{ nameof(IdCurrentHost), IdCurrentHost },
			{ nameof(AttachedToCurrentHostUtc), AttachedToCurrentHostUtc },
			{ nameof(LastStatusChangedUtc), LastStatusChangedUtc },
			{ nameof(LastProcessingStartedUtc), LastProcessingStartedUtc },
			{ nameof(LastProcessingFinishedUtc), LastProcessingFinishedUtc },
			{ nameof(DelayedToUtc), DelayedToUtc },
			{ nameof(RowVersion), RowVersion },
		};

	public void TrimStringValuesToFitDatabaseMaxLengths(string? postfix = null)
	{
	}

	public override string? GetPrimaryKeyValue()
	{
		return IdJobActivity.ToString();
	}

	public override string? ToString()
	{
		return IdJobActivity.ToString();
	}

	public static ValidatorBuilder<JobActivity> SetDBValidatorRules(ValidatorBuilder<JobActivity> builder)
		=> builder
			.ForProperty(x => x.IdJobActivity, v => v.NotDefaultOrEmpty())
			.ForProperty(x => x.IdJob, v => v.NotDefaultOrEmpty(), (x, parent) => x.Job == null)
			.ForProperty(x => x.IdJobStatus, v => v.NotDefaultOrEmpty(), (x, parent) => x.JobStatus == null)
			//.ForProperty(x => x.IdCurrentHost, v => v.NotDefaultOrEmpty())
			//.ForProperty(x => x.AttachedToCurrentHostUtc, v => v.NotDefaultOrEmpty())
			//.ForProperty(x => x.LastStatusChangedUtc, v => v.NotDefaultOrEmpty())
			//.ForProperty(x => x.RowVersion, v => v.NotDefaultOrEmpty())
		;
}
