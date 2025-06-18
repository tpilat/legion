using Legion.Validation;

namespace Legion.ADF.Cache.Model;

public sealed partial class ReloadableCacheKey : Cache.CacheBaseEntity, Legion.Model.IEntity
{
	public static IValidator<ReloadableCacheKey> DefaultDBValidator { get; }

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
	/// Database DataType: timestamp with time zone NOT NULL
	/// </summary>
	public DateTime CreatedUtc { get; private set; }

	/// <summary>
	/// Database DataType: timestamp with time zone NOT NULL
	/// </summary>
	public DateTime ReloadAtUtc { get; private set; }

	private ReloadableCacheKey()
	{
	}

	static ReloadableCacheKey()
	{
		DefaultDBValidator = SetDBValidatorRules(new ValidatorBuilder<ReloadableCacheKey>()).Build();
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

	public void TrimStringValuesToFitDatabaseMaxLengths(string? postfix = null)
	{
	}

	public override string? GetPrimaryKeyValue()
	{
		return IdReloadableCacheKey.ToString();
	}

	public override string? ToString()
	{
		return IdReloadableCacheKey.ToString();
	}

	public static ValidatorBuilder<ReloadableCacheKey> SetDBValidatorRules(ValidatorBuilder<ReloadableCacheKey> builder)
		=> builder
			.ForProperty(x => x.IdReloadableCacheKey, v => v.NotDefaultOrEmpty())
			//.ForProperty(x => x.CreatedUtc, v => v.NotDefaultOrEmpty())
			//.ForProperty(x => x.ReloadAtUtc, v => v.NotDefaultOrEmpty())
		;
}
