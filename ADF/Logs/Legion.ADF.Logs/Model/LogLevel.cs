using Legion.Validation;

namespace Legion.ADF.Logs.Model;

public sealed partial class LogLevel : Logs.LogsBaseEntity, Legion.Model.IEntity
{
	public static IValidator<LogLevel> DefaultDBValidator { get; }

	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid IdLogLevel { get; private set; }

	/// <summary>
	/// Database DataType: varchar(31) NOT NULL
	/// </summary>
	public string Code { get; private set; }

	/// <summary>
	/// Database DataType: varchar(31) NOT NULL
	/// </summary>
	public string Name { get; private set; }

	/// <summary>
	/// Database DataType: integer NOT NULL
	/// </summary>
	public int ItemCode { get; private set; }

	private LogLevel()
	{
	}

	public Dictionary<string, object?> ToDictionary()
		=> new()
		{
			{ nameof(IdLogLevel), IdLogLevel },
			{ nameof(Code), Code },
			{ nameof(Name), Name },
			{ nameof(ItemCode), ItemCode },
		};

	public void TrimStringValuesToFitDatabaseMaxLengths(string? postfix = null)
	{
		Code = Legion.Text.StringHelper.TrimToFitMaxLength(Code, 31, postfix);
		Name = Legion.Text.StringHelper.TrimToFitMaxLength(Name, 31, postfix);
	}

	public override string? GetPrimaryKeyValue()
	{
		return IdLogLevel.ToString();
	}

	public override string? ToString()
	{
		return Code;
	}

	public static ValidatorBuilder<LogLevel> SetDBValidatorRules(ValidatorBuilder<LogLevel> builder)
		=> builder
			.ForProperty(x => x.IdLogLevel, v => v.NotDefaultOrEmpty())
			.ForProperty(x => x.Code, v => v.NotDefaultOrEmpty().MaxLength(31))
			.ForProperty(x => x.Name, v => v.NotDefaultOrEmpty().MaxLength(31))
			//.ForProperty(x => x.ItemCode, v => v.NotDefaultOrEmpty())
		;
}
