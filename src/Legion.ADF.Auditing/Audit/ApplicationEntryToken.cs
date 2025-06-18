namespace Legion.ADF.Auditing.Audit;

public sealed partial class ApplicationEntryToken : Auditing.EntityBase, Legion.Model.IEntity
{

	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid IdApplicationEntryToken { get; private set; }

	/// <summary>
	/// Database DataType: varchar(255) NOT NULL
	/// </summary>
	public string Token { get; private set; }

	/// <summary>
	/// Database DataType: integer NOT NULL
	/// </summary>
	public int Version { get; private set; }

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
	public string? MainEntityName { get; private set; }

	/// <summary>
	/// Database DataType: varchar(511) NULL
	/// </summary>
	public string? Description { get; private set; }

	/// <summary>
	/// Database DataType: text NULL
	/// </summary>
	public string? TokenHistory { get; private set; }

	private ApplicationEntryToken()
	{
	}

	public override string? ToString()
	{
		return Token;
	}
}
