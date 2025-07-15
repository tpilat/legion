using Legion.Validation;

namespace Legion.ADF.Cache.Model;

public sealed partial class DistributedLock : Cache.CacheBaseEntity, Legion.Model.IEntity
{
	public static IValidator<DistributedLock> DefaultDBValidator { get; }

	/// <summary>
	/// Database DataType: text NOT NULL
	/// </summary>
	public string KeyHash { get; private set; }

	/// <summary>
	/// Database DataType: text NOT NULL
	/// </summary>
	public string LockKey { get; private set; }

	/// <summary>
	/// Database DataType: text NOT NULL
	/// </summary>
	public string LockId { get; private set; }

	/// <summary>
	/// Database DataType: text NULL
	/// </summary>
	public string? Metadata { get; private set; }

	/// <summary>
	/// Database DataType: timestamp with time zone NOT NULL
	/// </summary>
	public DateTime CreatedUtc { get; private set; }

	/// <summary>
	/// Database DataType: timestamp with time zone NOT NULL
	/// </summary>
	public DateTime ExpiresUtc { get; private set; }

	private DistributedLock()
	{
	}

	static DistributedLock()
	{
		DefaultDBValidator = SetDBValidatorRules(new ValidatorBuilder<DistributedLock>()).Build();
	}

	public Dictionary<string, object?> ToDictionary()
		=> new()
		{
			{ nameof(KeyHash), KeyHash },
			{ nameof(LockKey), LockKey },
			{ nameof(LockId), LockId },
			{ nameof(Metadata), Metadata },
			{ nameof(CreatedUtc), CreatedUtc },
			{ nameof(ExpiresUtc), ExpiresUtc },
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

	public static ValidatorBuilder<DistributedLock> SetDBValidatorRules(ValidatorBuilder<DistributedLock> builder)
		=> builder
			.ForProperty(x => x.KeyHash, v => v.NotDefaultOrEmpty())
			.ForProperty(x => x.LockKey, v => v.NotDefaultOrEmpty())
			.ForProperty(x => x.LockId, v => v.NotDefaultOrEmpty())
			//.ForProperty(x => x.CreatedUtc, v => v.NotDefaultOrEmpty())
			//.ForProperty(x => x.ExpiresUtc, v => v.NotDefaultOrEmpty())
		;
}
