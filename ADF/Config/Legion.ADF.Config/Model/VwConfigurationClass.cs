namespace Legion.ADF.Config.Model;

public sealed partial class VwConfigurationClass : Config.ConfigBaseQueryEntity, Legion.Model.IQueryEntity
{
	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid IdConfigurationClass { get; private set; }

	/// <summary>
	/// Database DataType: text NULL
	/// </summary>
	public string? RootPath { get; private set; }

	/// <summary>
	/// Database DataType: varchar(255) NULL
	/// </summary>
	public string? DisplayName { get; private set; }

	/// <summary>
	/// Database DataType: text NULL
	/// </summary>
	public string? Class { get; private set; }


	private VwConfigurationClass()
	{
	}

	public Dictionary<string, object?> ToDictionary()
		=> new()
		{
			{ nameof(IdConfigurationClass), IdConfigurationClass },
			{ nameof(RootPath), RootPath },
			{ nameof(DisplayName), DisplayName },
			{ nameof(Class), Class },
		};

	public override string? ToString()
	{
		return IdConfigurationClass.ToString();
	}
}
