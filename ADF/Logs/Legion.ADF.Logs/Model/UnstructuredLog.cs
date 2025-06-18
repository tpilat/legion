using Legion.Validation;

namespace Legion.ADF.Logs.Model;

public sealed partial class UnstructuredLog : Logs.LogsBaseEntity, Legion.Model.IEntity
{
	public static IValidator<UnstructuredLog> DefaultDBValidator { get; }

	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid IdUnstructuredLog { get; private set; }

	/// <summary>
	/// Database DataType: timestamp with time zone NOT NULL
	/// </summary>
	public DateTime CreatedUtc { get; private set; }

	/// <summary>
	/// Database DataType: integer NOT NULL
	/// </summary>
	public int IdLogLevel { get; private set; }

	/// <summary>
	/// Database DataType: text NULL
	/// </summary>
	public string? Message { get; private set; }

	/// <summary>
	/// Database DataType: text NULL
	/// </summary>
	public string? StackTrace { get; private set; }

	/// <summary>
	/// Database DataType: text NULL
	/// </summary>
	public string? SourceContext { get; private set; }

	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid RuntimeUniqueKey { get; private set; }

	/// <summary>
	/// Database DataType: varchar(511) NULL
	/// </summary>
	public string? EventName { get; private set; }

	/// <summary>
	/// Database DataType: integer NULL
	/// </summary>
	public int? EventId { get; private set; }

	private UnstructuredLog()
	{
	}

	static UnstructuredLog()
	{
		DefaultDBValidator = SetDBValidatorRules(new ValidatorBuilder<UnstructuredLog>()).Build();
	}

	public Dictionary<string, object?> ToDictionary()
		=> new()
		{
			{ nameof(IdUnstructuredLog), IdUnstructuredLog },
			{ nameof(CreatedUtc), CreatedUtc },
			{ nameof(IdLogLevel), IdLogLevel },
			{ nameof(Message), Message },
			{ nameof(StackTrace), StackTrace },
			{ nameof(SourceContext), SourceContext },
			{ nameof(RuntimeUniqueKey), RuntimeUniqueKey },
			{ nameof(EventName), EventName },
			{ nameof(EventId), EventId },
		};

	public void TrimStringValuesToFitDatabaseMaxLengths(string? postfix = null)
	{
		EventName = Legion.Text.StringHelper.TrimToFitMaxLength(EventName, 511, postfix);
	}

	public override string? GetPrimaryKeyValue()
	{
		return IdUnstructuredLog.ToString();
	}

	public override string? ToString()
	{
		return IdUnstructuredLog.ToString();
	}

	public static ValidatorBuilder<UnstructuredLog> SetDBValidatorRules(ValidatorBuilder<UnstructuredLog> builder)
		=> builder
			.ForProperty(x => x.IdUnstructuredLog, v => v.NotDefaultOrEmpty())
			//.ForProperty(x => x.CreatedUtc, v => v.NotDefaultOrEmpty())
			//.ForProperty(x => x.IdLogLevel, v => v.NotDefaultOrEmpty())
			//.ForProperty(x => x.RuntimeUniqueKey, v => v.NotDefaultOrEmpty())
			.ForProperty(x => x.EventName, v => v.MaxLength(511))
		;
}
