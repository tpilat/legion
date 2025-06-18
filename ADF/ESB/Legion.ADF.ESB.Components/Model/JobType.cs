using Legion.Validation;

namespace Legion.ADF.ESB.Components.Model;

public sealed partial class JobType : Components.ComponentsBaseEntity, Legion.Model.IEntity
{
	private List<Components.Model.Job> _jobs;

	public static IValidator<JobType> DefaultDBValidator { get; }

	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid IdJobType { get; private set; }

	/// <summary>
	/// Database DataType: varchar(63) NOT NULL
	/// </summary>
	public string Code { get; private set; }

	/// <summary>
	/// Database DataType: varchar(127) NOT NULL
	/// </summary>
	public string Name { get; private set; }


	/// <summary>
	/// N:_1 Components.Model.Job.IdJobType | FK_Job_IdJobType
	/// </summary>
	public IReadOnlyList<Components.Model.Job> Jobs => _jobs;

	private JobType()
	{
		_jobs = [];
	}

	public override string? ToString()
	{
		return Code;
	}

	public static ValidatorBuilder<JobType> SetDBValidatorRules(ValidatorBuilder<JobType> builder)
		=> builder
			.ForProperty(x => x.IdJobType, v => v.NotDefaultOrEmpty())
			.ForProperty(x => x.Code, v => v.NotDefaultOrEmpty().MaxLength(63))
			.ForProperty(x => x.Name, v => v.NotDefaultOrEmpty().MaxLength(127))
		;
}
