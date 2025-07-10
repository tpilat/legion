using Legion.Cryptography;

namespace Legion.ADF.Cache.Model;

public sealed partial class DistributedLock : Cache.CacheBaseEntity, Legion.Model.IEntity
{
	public static IResult<DistributedLock?> Create(
		IScopeContext scopeContext,
		string key,
		TimeSpan timeout,
		string? metadata)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<DistributedLock?>();

		if (result.IsArgumentNullOrWhiteSpace(scopeContext, key))
			return result.Build();

		if (result.IsArgumentLessThanOrEqual(scopeContext, timeout, TimeSpan.Zero))
			return result.Build();

		var distributedLock = new DistributedLock
		{
			__IsNewObject = true,
			KeyHash = HashHelper.ComputeMD5Hash(key),
			LockKey = key,
			LockId = HashHelper.ComputeMD5Hash(Guid.NewGuid().ToString()),
			Metadata = metadata,
			ExpiresUtc = GlobalContext.Instance.UtcNow.Add(timeout)
		};

		var validationResult =
			DefaultDBValidator
				.Validate(distributedLock);

		if (result.MergeHasError(scopeContext, validationResult, true))
			return result.Build();

		return result.WithData(distributedLock).Build();
	}
}
