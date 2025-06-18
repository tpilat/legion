namespace Legion.ADF.Cache.Model;

public sealed partial class VwReloadableCacheKey : Cache.CacheBaseQueryEntity, Legion.Model.IQueryEntity
{
	/// <summary>
	/// Database DataType: uuid NOT NULL
	/// </summary>
	public Guid IdReloadableCacheKey { get; private set; }

	/// <summary>
	/// Database DataType: text NULL
	/// </summary>
	public string? Key { get; private set; }

	/// <summary>
	/// Database DataType: text[] NULL
	/// </summary>
	public List<string>? Tags { get; private set; }

	/// <summary>
	/// Database DataType: timestamp with time zone NULL
	/// </summary>
	public DateTime? CreatedUtc { get; private set; }

	/// <summary>
	/// Database DataType: timestamp with time zone NULL
	/// </summary>
	public DateTime? ReloadAtUtc { get; private set; }


	private VwReloadableCacheKey()
	{
	}

	public Dictionary<string, object?> ToDictionary()
		=> new()
		{
			{ nameof(IdReloadableCacheKey), IdReloadableCacheKey },
			{ nameof(Key), Key },
			{ nameof(Tags), Tags },
			{ nameof(CreatedUtc), CreatedUtc },
			{ nameof(ReloadAtUtc), ReloadAtUtc },
		};

	public override string? ToString()
	{
		return IdReloadableCacheKey.ToString();
	}
}
