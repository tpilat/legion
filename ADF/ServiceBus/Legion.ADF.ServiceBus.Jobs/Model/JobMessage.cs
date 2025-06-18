using Legion.Validation;

namespace Legion.ADF.ServiceBus.Jobs.Model;

public sealed partial class JobMessage : Jobs.JobsBaseEntity, Legion.Model.IEntity
{
	public static IValidator<JobMessage> DefaultDBValidator { get; }

	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid IdJobMessage { get; private set; }

	/// <summary>
	/// Database DataType: uuid NOT NULL | Jobs.Model.Job.Job | FK_JobMessage_IdJob
	/// </summary>
	public Guid IdJob { get; private set; }

	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid IdMessage { get; private set; }

	/// <summary>
	/// Database DataType: uuid NOT NULL | Jobs.Model.JobMessageType.JobMessageType | FK_JobMessage_IdJobMessageType
	/// </summary>
	public Guid IdJobMessageType { get; private set; }

	/// <summary>
	/// Database DataType: timestamp with time zone NOT NULL
	/// </summary>
	public DateTime CreatedUtc { get; private set; }


	/// <summary>
	/// _1:N Guid IdJob | FK_JobMessage_IdJob
	/// </summary>
	public Jobs.Model.Job Job { get; private set; }

	/// <summary>
	/// _1:N Guid IdJobMessageType | FK_JobMessage_IdJobMessageType
	/// </summary>
	public Jobs.Model.JobMessageType JobMessageType { get; private set; }

	private JobMessage()
	{
	}

	static JobMessage()
	{
		DefaultDBValidator = SetDBValidatorRules(new ValidatorBuilder<JobMessage>()).Build();
	}

	public Dictionary<string, object?> ToDictionary()
		=> new()
		{
			{ nameof(IdJobMessage), IdJobMessage },
			{ nameof(IdJob), IdJob },
			{ nameof(IdMessage), IdMessage },
			{ nameof(IdJobMessageType), IdJobMessageType },
			{ nameof(CreatedUtc), CreatedUtc },
		};

	public void TrimStringValuesToFitDatabaseMaxLengths(string? postfix = null)
	{
	}

	public override string? GetPrimaryKeyValue()
	{
		return IdJobMessage.ToString();
	}

	public override string? ToString()
	{
		return IdJobMessage.ToString();
	}

	public static ValidatorBuilder<JobMessage> SetDBValidatorRules(ValidatorBuilder<JobMessage> builder)
		=> builder
			.ForProperty(x => x.IdJobMessage, v => v.NotDefaultOrEmpty())
			.ForProperty(x => x.IdJob, v => v.NotDefaultOrEmpty(), (x, parent) => x.Job == null)
			//.ForProperty(x => x.IdMessage, v => v.NotDefaultOrEmpty())
			.ForProperty(x => x.IdJobMessageType, v => v.NotDefaultOrEmpty(), (x, parent) => x.JobMessageType == null)
			//.ForProperty(x => x.CreatedUtc, v => v.NotDefaultOrEmpty())
		;
}
