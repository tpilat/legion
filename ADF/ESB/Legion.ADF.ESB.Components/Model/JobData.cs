using Legion.Validation;

namespace Legion.ADF.ESB.Components.Model;

public sealed partial class JobData : Components.ComponentsBaseEntity, Legion.Model.IEntity
{
	public static IValidator<JobData> DefaultDBValidator { get; }

	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid IdJobData { get; private set; }

	/// <summary>
	/// Database DataType: uuid NOT NULL | Components.Model.Job.Job | FK_JobData_IdJob
	/// </summary>
	public Guid IdJob { get; private set; }

	/// <summary>
	/// Database DataType: varchar(63) NOT NULL
	/// </summary>
	public string Key { get; private set; }

	/// <summary>
	/// Database DataType: timestamp without time zone NOT NULL
	/// </summary>
	public DateTime CreatedUtc { get; private set; }

	/// <summary>
	/// Database DataType: timestamp without time zone NOT NULL
	/// </summary>
	public DateTime LastModifiedUtc { get; private set; }

	/// <summary>
	/// Database DataType: varchar(63) NULL
	/// </summary>
	public string? ContentEncoding { get; private set; }

	/// <summary>
	/// Database DataType: bytea NULL
	/// </summary>
	public byte[]? ByteArrayContent { get; private set; }

	/// <summary>
	/// Database DataType: jsonb NULL
	/// </summary>
	public string? JsonContent { get; private set; }

	/// <summary>
	/// Database DataType: text NULL
	/// </summary>
	public string? StringContent { get; private set; }

	/// <summary>
	/// Database DataType: bigint NULL
	/// </summary>
	public long? DbOid { get; private set; }

	/// <summary>
	/// Database DataType: varchar(511) NULL
	/// </summary>
	public string? Name { get; private set; }

	/// <summary>
	/// Database DataType: varchar(1023) NULL
	/// </summary>
	public string? RelaltivePath { get; private set; }

	/// <summary>
	/// Database DataType: jsonb NULL
	/// </summary>
	public string? Metadata { get; private set; }

	/// <summary>
	/// Database DataType: boolean NOT NULL
	/// </summary>
	public bool IsCompressed { get; private set; }

	/// <summary>
	/// Database DataType: text NULL
	/// </summary>
	public string? EncryptionKey { get; private set; }


	/// <summary>
	/// _1:N Guid IdJob | FK_JobData_IdJob
	/// </summary>
	public Components.Model.Job Job { get; private set; }

	private JobData()
	{
	}

	static JobData()
	{
		DefaultDBValidator = SetDBValidatorRules(new ValidatorBuilder<JobData>()).Build();
	}

	public override string? ToString()
	{
		return IdJobData.ToString();
	}

	public static ValidatorBuilder<JobData> SetDBValidatorRules(ValidatorBuilder<JobData> builder)
		=> builder
			.ForProperty(x => x.IdJobData, v => v.NotDefaultOrEmpty())
			.ForProperty(x => x.IdJob, v => v.NotDefaultOrEmpty(), x => x.Job == null)
			.ForProperty(x => x.Key, v => v.NotDefaultOrEmpty().MaxLength(63))
			//.ForProperty(x => x.CreatedUtc, v => v.NotDefaultOrEmpty())
			//.ForProperty(x => x.LastModifiedUtc, v => v.NotDefaultOrEmpty())
			.ForProperty(x => x.ContentEncoding, v => v.MaxLength(63))
			.ForProperty(x => x.Name, v => v.MaxLength(511))
			.ForProperty(x => x.RelaltivePath, v => v.MaxLength(1023))
		;
}
