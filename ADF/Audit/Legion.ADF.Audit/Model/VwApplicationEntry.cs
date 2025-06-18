namespace Legion.ADF.Audit.Model;

public sealed partial class VwApplicationEntry : Audit.AuditBaseQueryEntity, Legion.Model.IQueryEntity
{
	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid IdApplicationEntry { get; private set; }

	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid IdApplicationEntryToken { get; private set; }

	/// <summary>
	/// Database DataType: varchar(255) NOT NULL
	/// </summary>
	public string Token { get; private set; }

	/// <summary>
	/// Database DataType: varchar(511) NOT NULL
	/// </summary>
	public string SourceFilePath { get; private set; }

	/// <summary>
	/// Database DataType: varchar(511) NULL
	/// </summary>
	public string? MethodInfo { get; private set; }

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
	public string? Description { get; private set; }

	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid IdAuditOperation { get; private set; }

	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid RuntimeUniqueKey { get; private set; }

	/// <summary>
	/// Database DataType: timestamp with time zone NOT NULL
	/// </summary>
	public DateTime CreatedUtc { get; private set; }

	/// <summary>
	/// Database DataType: uuid NULL
	/// </summary>
	public Guid? CorrelationId { get; private set; }

	/// <summary>
	/// Database DataType: varchar(127) NULL
	/// </summary>
	public string? ExternalCorrelationId { get; private set; }

	/// <summary>
	/// Database DataType: varchar(15) NULL
	/// </summary>
	public string? HttpMethod { get; private set; }

	/// <summary>
	/// Database DataType: varchar(1023) NULL
	/// </summary>
	public string? Uri { get; private set; }

	/// <summary>
	/// Database DataType: uuid NULL
	/// </summary>
	public Guid? IdUser { get; private set; }

	/// <summary>
	/// Database DataType: uuid NULL
	/// </summary>
	public Guid? TenantIdentifier { get; private set; }

	/// <summary>
	/// Database DataType: varchar(63) NULL
	/// </summary>
	public string? RemoteIP { get; private set; }

	/// <summary>
	/// Database DataType: uuid NULL
	/// </summary>
	public Guid? IdApplicationEntryRequest { get; private set; }

	/// <summary>
	/// Database DataType: uuid NULL
	/// </summary>
	public Guid? IdApplicationEntryResponse { get; private set; }

	/// <summary>
	/// Database DataType: varchar(63) NULL
	/// </summary>
	public string? StatusCode { get; private set; }

	/// <summary>
	/// Database DataType: text NULL
	/// </summary>
	public string? Error { get; private set; }

	/// <summary>
	/// Database DataType: numeric NULL
	/// </summary>
	public decimal? ElapsedMilliseconds { get; private set; }


	private VwApplicationEntry()
	{
	}

	public Dictionary<string, object?> ToDictionary()
		=> new()
		{
			{ nameof(IdApplicationEntry), IdApplicationEntry },
			{ nameof(IdApplicationEntryToken), IdApplicationEntryToken },
			{ nameof(Token), Token },
			{ nameof(SourceFilePath), SourceFilePath },
			{ nameof(MethodInfo), MethodInfo },
			{ nameof(AggregateName), AggregateName },
			{ nameof(AggregateIdentifier), AggregateIdentifier },
			{ nameof(Description), Description },
			{ nameof(IdAuditOperation), IdAuditOperation },
			{ nameof(RuntimeUniqueKey), RuntimeUniqueKey },
			{ nameof(CreatedUtc), CreatedUtc },
			{ nameof(CorrelationId), CorrelationId },
			{ nameof(ExternalCorrelationId), ExternalCorrelationId },
			{ nameof(HttpMethod), HttpMethod },
			{ nameof(Uri), Uri },
			{ nameof(IdUser), IdUser },
			{ nameof(TenantIdentifier), TenantIdentifier },
			{ nameof(RemoteIP), RemoteIP },
			{ nameof(IdApplicationEntryRequest), IdApplicationEntryRequest },
			{ nameof(IdApplicationEntryResponse), IdApplicationEntryResponse },
			{ nameof(StatusCode), StatusCode },
			{ nameof(Error), Error },
			{ nameof(ElapsedMilliseconds), ElapsedMilliseconds },
		};

	public override string? ToString()
	{
		return IdApplicationEntry.ToString();
	}
}
