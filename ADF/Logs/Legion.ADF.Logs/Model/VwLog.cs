namespace Legion.ADF.Logs.Model;

public sealed partial class VwLog : Logs.LogsBaseQueryEntity, Legion.Model.IQueryEntity
{
	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid IdLog { get; private set; }

	/// <summary>
	/// Database DataType: timestamp with time zone NULL
	/// </summary>
	public DateTime? CreatedUtc { get; private set; }

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
	/// Database DataType: integer NULL
	/// </summary>
	public int? IdLogLevel { get; private set; }

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
	/// Database DataType: uuid NULL
	/// </summary>
	public Guid? RuntimeUniqueKey { get; private set; }

	/// <summary>
	/// Database DataType: boolean NULL
	/// </summary>
	public bool? IsValidationError { get; private set; }

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


	private VwLog()
	{
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

	public override string? ToString()
	{
		return IdLog.ToString();
	}
}
