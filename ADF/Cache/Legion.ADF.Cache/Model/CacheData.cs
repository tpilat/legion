using Legion.Validation;

namespace Legion.ADF.Cache.Model;

public sealed partial class CacheData : Cache.CacheBaseEntity, Legion.Model.IEntity
{
	public static IValidator<CacheData> DefaultDBValidator { get; }

	/// <summary>
	/// Database DataType: text NOT NULL
	/// </summary>
	public string KeyHash { get; private set; }

	/// <summary>
	/// Database DataType: text NOT NULL
	/// </summary>
	public string ValueHash { get; private set; }

	/// <summary>
	/// Database DataType: text NOT NULL
	/// </summary>
	public string Key { get; private set; }

	/// <summary>
	/// Database DataType: text NOT NULL
	/// </summary>
	public string Value { get; private set; }

	/// <summary>
	/// Database DataType: text NOT NULL
	/// </summary>
	public string KeyPrefix450 { get; private set; }

	/// <summary>
	/// Database DataType: timestamp with time zone NULL
	/// </summary>
	public DateTime? ExpiresUtc { get; private set; }

	/// <summary>
	/// Database DataType: interval NULL
	/// </summary>
	public TimeSpan? SlidingTime { get; private set; }

	/// <summary>
	/// Database DataType: timestamp with time zone NOT NULL
	/// </summary>
	public DateTime LastAccessedUtc { get; private set; }

	/// <summary>
	/// Database DataType: bigint NOT NULL
	/// </summary>
	public long RowVersion { get; private set; }

	private CacheData()
	{
	}

	static CacheData()
	{
		DefaultDBValidator = SetDBValidatorRules(new ValidatorBuilder<CacheData>()).Build();
	}

	public Dictionary<string, object?> ToDictionary()
		=> new()
		{
			{ nameof(KeyHash), KeyHash },
			{ nameof(ValueHash), ValueHash },
			{ nameof(Key), Key },
			{ nameof(Value), Value },
			{ nameof(KeyPrefix450), KeyPrefix450 },
			{ nameof(ExpiresUtc), ExpiresUtc },
			{ nameof(SlidingTime), SlidingTime },
			{ nameof(LastAccessedUtc), LastAccessedUtc },
			{ nameof(RowVersion), RowVersion },
		};

	public void TrimStringValuesToFitDatabaseMaxLengths(string? postfix = null)
	{
	}

	public override string? GetPrimaryKeyValue()
	{
		return KeyHash;
	}

	public override string? ToString()
	{
		return KeyHash;
	}

	public static ValidatorBuilder<CacheData> SetDBValidatorRules(ValidatorBuilder<CacheData> builder)
		=> builder
			.ForProperty(x => x.KeyHash, v => v.NotDefaultOrEmpty())
			.ForProperty(x => x.ValueHash, v => v.NotDefaultOrEmpty())
			.ForProperty(x => x.Key, v => v.NotDefaultOrEmpty())
			.ForProperty(x => x.Value, v => v.NotDefaultOrEmpty())
			.ForProperty(x => x.KeyPrefix450, v => v.NotDefaultOrEmpty())
			//.ForProperty(x => x.LastAccessedUtc, v => v.NotDefaultOrEmpty())
			//.ForProperty(x => x.RowVersion, v => v.NotDefaultOrEmpty())
		;
}
