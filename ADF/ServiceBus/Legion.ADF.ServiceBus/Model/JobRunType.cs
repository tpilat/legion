using Legion.Validation;

namespace Legion.ADF.ServiceBus.Model;

public sealed partial class JobRunType : ServiceBus.ServiceBusBaseEntity, Legion.Model.IEntity
{
	private List<ServiceBus.Model.Job> _jobs;

	public static IValidator<JobRunType> DefaultDBValidator { get; }

	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid IdJobRunType { get; private set; }

	/// <summary>
	/// Database DataType: varchar(63) NOT NULL
	/// </summary>
	public string Code { get; private set; }

	/// <summary>
	/// Database DataType: varchar(63) NOT NULL
	/// </summary>
	public string Name { get; private set; }


	/// <summary>
	/// N:_1 ServiceBus.Model.Job.IdJobRunType | FK_Job_IdJobRunType
	/// </summary>
	public IReadOnlyList<ServiceBus.Model.Job> Jobs => _jobs;

	private JobRunType()
	{
		_jobs = [];
	}

	public Dictionary<string, object?> ToDictionary()
		=> new()
		{
			{ nameof(IdJobRunType), IdJobRunType },
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
		return IdJobRunType.ToString();
	}

	public override string? ToString()
	{
		return Code;
	}

	public static ValidatorBuilder<JobRunType> SetDBValidatorRules(ValidatorBuilder<JobRunType> builder)
		=> builder
			.ForProperty(x => x.IdJobRunType, v => v.NotDefaultOrEmpty())
			.ForProperty(x => x.Code, v => v.NotDefaultOrEmpty().MaxLength(63))
			.ForProperty(x => x.Name, v => v.NotDefaultOrEmpty().MaxLength(63))
		;
}
