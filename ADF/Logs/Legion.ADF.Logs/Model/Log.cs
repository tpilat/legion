using Legion.Validation;

namespace Legion.ADF.Logs.Model;

public sealed partial class Log : Logs.LogsBaseEntity, Legion.Model.IEntity
{
	public static IValidator<Log> DefaultDBValidator { get; }

	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid IdLog { get; private set; }

	/// <summary>
	/// Database DataType: timestamp with time zone NOT NULL
	/// </summary>
	public DateTime CreatedUtc { get; private set; }

	/// <summary>
	/// Database DataType: text NULL
	/// </summary>
	public string? InternalMessage { get; private set; }

	/// <summary>
	/// Database DataType: text NULL
	/// </summary>
	public string? ClientMessage { get; private set; }

	/// <summary>
	/// Database DataType: text NULL
	/// </summary>
	public string? Detail { get; private set; }

	/// <summary>
	/// Database DataType: text NULL
	/// </summary>
	public string? StackTrace { get; private set; }

	/// <summary>
	/// Database DataType: varchar(511) NULL
	/// </summary>
	public string? Component { get; private set; }

	/// <summary>
	/// Database DataType: varchar(1023) NULL
	/// </summary>
	public string? OperationName { get; private set; }

	/// <summary>
	/// Database DataType: varchar(255) NULL
	/// </summary>
	public string? AggregateName { get; private set; }

	/// <summary>
	/// Database DataType: varchar(511) NULL
	/// </summary>
	public string? AggregateIdentifier { get; private set; }

	/// <summary>
	/// Database DataType: varchar(511) NULL
	/// </summary>
	public string? CustomCorrelationId { get; private set; }

	/// <summary>
	/// Database DataType: uuid NULL
	/// </summary>
	public Guid? IdApplicationEntry { get; private set; }

	/// <summary>
	/// Database DataType: uuid NULL
	/// </summary>
	public Guid? CorrelationId { get; private set; }

	/// <summary>
	/// Database DataType: varchar(511) NULL
	/// </summary>
	public string? ExternalCorrelationId { get; private set; }

	/// <summary>
	/// Database DataType: jsonb NULL
	/// </summary>
	public string? ContextProperties { get; private set; }

	/// <summary>
	/// Database DataType: uuid NULL
	/// </summary>
	public Guid? IdUser { get; private set; }

	/// <summary>
	/// Database DataType: uuid NULL
	/// </summary>
	public Guid? TenantIdentifier { get; private set; }

	/// <summary>
	/// Database DataType: integer NOT NULL
	/// </summary>
	public int IdLogLevel { get; private set; }

	/// <summary>
	/// Database DataType: varchar(63) NULL
	/// </summary>
	public string? LogCode { get; private set; }

	/// <summary>
	/// Database DataType: varchar(1023) NULL
	/// </summary>
	public string? SourceSystemName { get; private set; }

	/// <summary>
	/// Database DataType: uuid NULL
	/// </summary>
	public Guid? TraceCorrelationId { get; private set; }

	/// <summary>
	/// Database DataType: text NULL
	/// </summary>
	public string? TraceFrame { get; private set; }

	/// <summary>
	/// Database DataType: text NULL
	/// </summary>
	public string? SourceContext { get; private set; }

	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid RuntimeUniqueKey { get; private set; }

	/// <summary>
	/// Database DataType: boolean NOT NULL
	/// </summary>
	public bool IsValidationError { get; private set; }

	/// <summary>
	/// Database DataType: varchar(255) NULL
	/// </summary>
	public string? PropertyName { get; private set; }

	/// <summary>
	/// Database DataType: varchar(255) NULL
	/// </summary>
	public string? DisplayPropertyName { get; private set; }

	/// <summary>
	/// Database DataType: text NULL
	/// </summary>
	public string? ValidationFailure { get; private set; }

	private Log()
	{
	}

	static Log()
	{
		DefaultDBValidator = SetDBValidatorRules(new ValidatorBuilder<Log>()).Build();
	}

	public Dictionary<string, object?> ToDictionary()
		=> new()
		{
			{ nameof(IdLog), IdLog },
			{ nameof(CreatedUtc), CreatedUtc },
			{ nameof(InternalMessage), InternalMessage },
			{ nameof(ClientMessage), ClientMessage },
			{ nameof(Detail), Detail },
			{ nameof(StackTrace), StackTrace },
			{ nameof(Component), Component },
			{ nameof(OperationName), OperationName },
			{ nameof(AggregateName), AggregateName },
			{ nameof(AggregateIdentifier), AggregateIdentifier },
			{ nameof(CustomCorrelationId), CustomCorrelationId },
			{ nameof(IdApplicationEntry), IdApplicationEntry },
			{ nameof(CorrelationId), CorrelationId },
			{ nameof(ExternalCorrelationId), ExternalCorrelationId },
			{ nameof(ContextProperties), ContextProperties },
			{ nameof(IdUser), IdUser },
			{ nameof(TenantIdentifier), TenantIdentifier },
			{ nameof(IdLogLevel), IdLogLevel },
			{ nameof(LogCode), LogCode },
			{ nameof(SourceSystemName), SourceSystemName },
			{ nameof(TraceCorrelationId), TraceCorrelationId },
			{ nameof(TraceFrame), TraceFrame },
			{ nameof(SourceContext), SourceContext },
			{ nameof(RuntimeUniqueKey), RuntimeUniqueKey },
			{ nameof(IsValidationError), IsValidationError },
			{ nameof(PropertyName), PropertyName },
			{ nameof(DisplayPropertyName), DisplayPropertyName },
			{ nameof(ValidationFailure), ValidationFailure },
		};

	public void TrimStringValuesToFitDatabaseMaxLengths(string? postfix = null)
	{
		Component = Legion.Text.StringHelper.TrimToFitMaxLength(Component, 511, postfix);
		OperationName = Legion.Text.StringHelper.TrimToFitMaxLength(OperationName, 1023, postfix);
		AggregateName = Legion.Text.StringHelper.TrimToFitMaxLength(AggregateName, 255, postfix);
		AggregateIdentifier = Legion.Text.StringHelper.TrimToFitMaxLength(AggregateIdentifier, 511, postfix);
		CustomCorrelationId = Legion.Text.StringHelper.TrimToFitMaxLength(CustomCorrelationId, 511, postfix);
		ExternalCorrelationId = Legion.Text.StringHelper.TrimToFitMaxLength(ExternalCorrelationId, 511, postfix);
		LogCode = Legion.Text.StringHelper.TrimToFitMaxLength(LogCode, 63, postfix);
		SourceSystemName = Legion.Text.StringHelper.TrimToFitMaxLength(SourceSystemName, 1023, postfix);
		PropertyName = Legion.Text.StringHelper.TrimToFitMaxLength(PropertyName, 255, postfix);
		DisplayPropertyName = Legion.Text.StringHelper.TrimToFitMaxLength(DisplayPropertyName, 255, postfix);
	}

	public override string? GetPrimaryKeyValue()
	{
		return IdLog.ToString();
	}

	public override string? ToString()
	{
		return IdLog.ToString();
	}

	public static ValidatorBuilder<Log> SetDBValidatorRules(ValidatorBuilder<Log> builder)
		=> builder
			.ForProperty(x => x.IdLog, v => v.NotDefaultOrEmpty())
			//.ForProperty(x => x.CreatedUtc, v => v.NotDefaultOrEmpty())
			.ForProperty(x => x.Component, v => v.MaxLength(511))
			.ForProperty(x => x.OperationName, v => v.MaxLength(1023))
			.ForProperty(x => x.AggregateName, v => v.MaxLength(255))
			.ForProperty(x => x.AggregateIdentifier, v => v.MaxLength(511))
			.ForProperty(x => x.CustomCorrelationId, v => v.MaxLength(511))
			.ForProperty(x => x.ExternalCorrelationId, v => v.MaxLength(511))
			//.ForProperty(x => x.IdLogLevel, v => v.NotDefaultOrEmpty())
			.ForProperty(x => x.LogCode, v => v.MaxLength(63))
			.ForProperty(x => x.SourceSystemName, v => v.MaxLength(1023))
			//.ForProperty(x => x.RuntimeUniqueKey, v => v.NotDefaultOrEmpty())
			.ForProperty(x => x.PropertyName, v => v.MaxLength(255))
			.ForProperty(x => x.DisplayPropertyName, v => v.MaxLength(255))
		;
}
