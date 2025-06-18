namespace Legion.ADF.Auditing.Audit;

public partial class VwApplicationEntryToken : Auditing.QueryEntityBase, Legion.Model.IQueryEntity
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
	/// Database DataType: integer NULL
	/// </summary>
	public int? Version { get; private set; }

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


	private VwApplicationEntryToken()
	{
	}

	public override string? ToString()
	{
		return IdApplicationEntryToken.ToString();
	}
}
