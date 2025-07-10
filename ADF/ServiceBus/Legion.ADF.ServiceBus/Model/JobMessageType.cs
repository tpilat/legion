using Legion.Validation;

namespace Legion.ADF.ServiceBus.Model;

public sealed partial class JobMessageType : ServiceBus.ServiceBusBaseEntity, Legion.Model.IEntity
{
	private List<ServiceBus.Model.JobMessage> _jobMessages;

	public static IValidator<JobMessageType> DefaultDBValidator { get; }

	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid IdJobMessageType { get; private set; }

	/// <summary>
	/// Database DataType: varchar(63) NOT NULL
	/// </summary>
	public string Code { get; private set; }

	/// <summary>
	/// Database DataType: varchar(63) NOT NULL
	/// </summary>
	public string Name { get; private set; }


	/// <summary>
	/// N:_1 ServiceBus.Model.JobMessage.IdJobMessageType | FK_JobMessage_IdJobMessageType
	/// </summary>
	public IReadOnlyList<ServiceBus.Model.JobMessage> JobMessages => _jobMessages;

	private JobMessageType()
	{
		_jobMessages = [];
	}

	public Dictionary<string, object?> ToDictionary()
		=> new()
		{
			{ nameof(IdJobMessageType), IdJobMessageType },
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
		return IdJobMessageType.ToString();
	}

	public override string? ToString()
	{
		return Code;
	}

	public static ValidatorBuilder<JobMessageType> SetDBValidatorRules(ValidatorBuilder<JobMessageType> builder)
		=> builder
			.ForProperty(x => x.IdJobMessageType, v => v.NotDefaultOrEmpty())
			.ForProperty(x => x.Code, v => v.NotDefaultOrEmpty().MaxLength(63))
			.ForProperty(x => x.Name, v => v.NotDefaultOrEmpty().MaxLength(63))
		;
}
