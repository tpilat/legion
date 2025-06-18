using Legion.Validation;

namespace Legion.ADF.ServiceBus.Jobs.Model;

public sealed partial class JobStatistics : Jobs.JobsBaseEntity, Legion.Model.IEntity
{
	public static IValidator<JobStatistics> DefaultDBValidator { get; }

	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid IdJobStatistics { get; private set; }

	/// <summary>
	/// Database DataType: uuid NOT NULL | Jobs.Model.Job.Job | FK_JobStatistics_IdJob
	/// </summary>
	public Guid IdJob { get; private set; }

	/// <summary>
	/// Database DataType: timestamp with time zone NOT NULL
	/// </summary>
	public DateTime StartHourUtc { get; private set; }

	/// <summary>
	/// Database DataType: integer NOT NULL
	/// </summary>
	public int ExecutionCount { get; private set; }

	/// <summary>
	/// Database DataType: integer NOT NULL
	/// </summary>
	public int ErrorCount { get; private set; }

	/// <summary>
	/// Database DataType: numeric NOT NULL
	/// </summary>
	public decimal AverageDuration { get; private set; }


	/// <summary>
	/// _1:N Guid IdJob | FK_JobStatistics_IdJob
	/// </summary>
	public Jobs.Model.Job Job { get; private set; }

	private JobStatistics()
	{
	}

	static JobStatistics()
	{
		DefaultDBValidator = SetDBValidatorRules(new ValidatorBuilder<JobStatistics>()).Build();
	}

	public Dictionary<string, object?> ToDictionary()
		=> new()
		{
			{ nameof(IdJobStatistics), IdJobStatistics },
			{ nameof(IdJob), IdJob },
			{ nameof(StartHourUtc), StartHourUtc },
			{ nameof(ExecutionCount), ExecutionCount },
			{ nameof(ErrorCount), ErrorCount },
			{ nameof(AverageDuration), AverageDuration },
		};

	public void TrimStringValuesToFitDatabaseMaxLengths(string? postfix = null)
	{
	}

	public override string? GetPrimaryKeyValue()
	{
		return IdJobStatistics.ToString();
	}

	public override string? ToString()
	{
		return IdJobStatistics.ToString();
	}

	public static ValidatorBuilder<JobStatistics> SetDBValidatorRules(ValidatorBuilder<JobStatistics> builder)
		=> builder
			.ForProperty(x => x.IdJobStatistics, v => v.NotDefaultOrEmpty())
			.ForProperty(x => x.IdJob, v => v.NotDefaultOrEmpty(), (x, parent) => x.Job == null)
			//.ForProperty(x => x.StartHourUtc, v => v.NotDefaultOrEmpty())
			//.ForProperty(x => x.ExecutionCount, v => v.NotDefaultOrEmpty())
			//.ForProperty(x => x.ErrorCount, v => v.NotDefaultOrEmpty())
			//.ForProperty(x => x.AverageDuration, v => v.NotDefaultOrEmpty())
		;
}
