using Legion.Cryptography;

namespace Legion.ADF.Cache.Model;

public sealed partial class CacheData : Cache.CacheBaseEntity, Legion.Model.IEntity
{
	public static IResult<CacheData?> Create(
		IScopeContext scopeContext,
		string key,
		string value,
		DateTime utcNow,
		DateTime? expiresUtc,
		TimeSpan? slidingTime)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<CacheData?>();

		if (result.IsArgumentNullOrWhiteSpace(scopeContext, key))
			return result.Build();

		if (result.IsArgumentNullOrWhiteSpace(scopeContext, value))
			return result.Build();

		if (expiresUtc.HasValue && result.IsArgumentLessThanOrEqual(scopeContext, expiresUtc.Value, utcNow))
			return result.Build();

		if (slidingTime.HasValue && result.IsArgumentLessThanOrEqual(scopeContext, slidingTime.Value, TimeSpan.Zero))
			return result.Build();

		var id = GlobalContext.Instance.NewGuid();
		var cacheData = new CacheData
		{
			__IsNewObject = true,
			KeyHash = HashHelper.ComputeMD5Hash(key),
			Key = key,
			KeyPrefix450 = 450 < key.Length ? key[..450] : key,
			Value = value,
			ValueHash = HashHelper.ComputeMD5Hash(value),
			SlidingTime = slidingTime,
			ExpiresUtc = expiresUtc,
			CreatedUtc = utcNow,
			LastAccessedUtc = utcNow,
			RowVersion = GlobalContext.Instance.NewGuid()
		};

		var validationResult =
			DefaultDBValidator
				.Validate(cacheData);

		if (result.MergeHasError(scopeContext, validationResult, true))
			return result.Build();

		return result.WithData(cacheData).Build();
	}

	public IResult UpdateLastAccess(
		IScopeContext scopeContext,
		DateTime nowUtc)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder();

		LastAccessedUtc = nowUtc;

		var validationResult =
			DefaultDBValidator
				.Validate(this);

		if (result.MergeHasError(scopeContext, validationResult, true))
			return result.Build();

		return result.Build();
	}

	public IResult UpdateSlidingAccess(
		IScopeContext scopeContext,
		DateTime nowUtc)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder();

		if (SlidingTime.HasValue)
			ExpiresUtc = nowUtc + SlidingTime.Value;

		var validationResult =
			DefaultDBValidator
				.Validate(this);

		if (result.MergeHasError(scopeContext, validationResult, true))
			return result.Build();

		return result.Build();
	}
}
