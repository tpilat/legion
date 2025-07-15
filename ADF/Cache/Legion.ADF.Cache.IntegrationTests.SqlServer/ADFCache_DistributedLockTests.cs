namespace Legion.ADF.Cache.IntegrationTests;

[Category("ADFCache DistributedLock tests")]
public class ADFCache_DistributedLockTests : TestBase
{
	[Test]
	public async Task CacheData_ShouldWriteDistributedLock()
	{
		var idUser = GlobalContext.Instance.NewGuid();

		var sp = await SetUp.CreateScopedServiceProviderAsync();
		var distributedLockProvider = GetDistributedLockProvider(sp);
		var scopeContext = ScopeContext.Create("TEST ScopeContext")
			.AppendTraceFrameWithIduser(idUser, true);

		var key = "My:Key";
		var metadata = "meta";
		var seconds = 1;

		var lockId = await distributedLockProvider.TryAcquireLockAsync(key, TimeSpan.FromSeconds(seconds), metadata, retryDelay: null, maxRetries: null);
		Assert.That(lockId, Is.Not.Null);

		var dbMetadata = await distributedLockProvider.GetMetadataAsync(key);
		Assert.That(dbMetadata, Is.EqualTo(metadata));

		await Task.Delay(TimeSpan.FromSeconds(seconds));

		var exists = await distributedLockProvider.ExistsAsync(key);
		Assert.That(exists, Is.False);
	}

	[Test]
	public async Task CacheData_ShouldNotWriteDistributedLockTwice()
	{
		var idUser = GlobalContext.Instance.NewGuid();

		var sp = await SetUp.CreateScopedServiceProviderAsync();
		var distributedLockProvider = GetDistributedLockProvider(sp);
		var scopeContext = ScopeContext.Create("TEST ScopeContext")
			.AppendTraceFrameWithIduser(idUser, true);

		var key = "My:Key";
		var metadata = "meta";
		var seconds = 1;

		var lockId = await distributedLockProvider.TryAcquireLockAsync(key, TimeSpan.FromSeconds(seconds), metadata, retryDelay: null, maxRetries: null);
		Assert.That(lockId, Is.Not.Null);

		var dbMetadata = await distributedLockProvider.GetMetadataAsync(key);
		Assert.That(dbMetadata, Is.EqualTo(metadata));

		lockId = await distributedLockProvider.TryAcquireLockAsync(key, TimeSpan.FromSeconds(seconds), metadata, retryDelay: null, maxRetries: null);
		Assert.That(lockId, Is.Null);
	}

	[Test]
	public async Task CacheData_ShouldReleaseDistributedLock()
	{
		var idUser = GlobalContext.Instance.NewGuid();

		var sp = await SetUp.CreateScopedServiceProviderAsync();
		var distributedLockProvider = GetDistributedLockProvider(sp);
		var scopeContext = ScopeContext.Create("TEST ScopeContext")
			.AppendTraceFrameWithIduser(idUser, true);

		var key = "My:Key";
		var metadata = "meta";
		var seconds = 10000;

		var lockId = await distributedLockProvider.TryAcquireLockAsync(key, TimeSpan.FromSeconds(seconds), metadata, retryDelay: null, maxRetries: null);
		Assert.That(lockId, Is.Not.Null);

		var dbMetadata = await distributedLockProvider.GetMetadataAsync(key);
		Assert.That(dbMetadata, Is.EqualTo(metadata));

		await distributedLockProvider.ReleaseLockAsync(key, lockId);

		var exists = await distributedLockProvider.ExistsAsync(key);
		Assert.That(exists, Is.False);
	}

	[Test]
	public async Task CacheData_ShouldRenewDistributedLock()
	{
		var idUser = GlobalContext.Instance.NewGuid();

		var sp = await SetUp.CreateScopedServiceProviderAsync();
		var distributedLockProvider = GetDistributedLockProvider(sp);
		var scopeContext = ScopeContext.Create("TEST ScopeContext")
			.AppendTraceFrameWithIduser(idUser, true);

		var key = "My:Key";
		var metadata = "meta";
		var seconds = 1;

		var lockId = await distributedLockProvider.TryAcquireLockAsync(key, TimeSpan.FromSeconds(seconds), metadata, retryDelay: null, maxRetries: null);
		Assert.That(lockId, Is.Not.Null);

		var dbMetadata = await distributedLockProvider.GetMetadataAsync(key);
		Assert.That(dbMetadata, Is.EqualTo(metadata));

		await Task.Delay(TimeSpan.FromSeconds(seconds));

		await distributedLockProvider.RenewLockAsync(key, lockId, TimeSpan.FromSeconds(seconds));

		dbMetadata = await distributedLockProvider.GetMetadataAsync(key);
		Assert.That(dbMetadata, Is.EqualTo(metadata));

		await Task.Delay(TimeSpan.FromSeconds(seconds));

		var exists = await distributedLockProvider.ExistsAsync(key);
		Assert.That(exists, Is.False);
	}
}
