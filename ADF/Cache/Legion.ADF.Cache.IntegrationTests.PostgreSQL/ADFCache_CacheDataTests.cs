namespace Legion.ADF.Cache.IntegrationTests;

[Category("ADFCache CacheData tests")]
public class ADFCache_CacheDataByKeyTests : TestBase
{
	[Test]
	public async Task CacheData_CheckIsDBAlive()
	{
		var idUser = GlobalContext.Instance.NewGuid();

		var sp = await SetUp.CreateScopedServiceProviderAsync();
		var cache = GetSimplePersistentCache(sp);
		var scopeContext = ScopeContext.Create("TEST ScopeContext")
			.AppendTraceFrameWithIduser(idUser, true);

		var saved = await cache.IsAliveAsync();
		Assert.That(saved, Is.EqualTo(true));
	}

	[Test]
	public async Task CacheData_ShouldWriteCacheData()
	{
		var idUser = GlobalContext.Instance.NewGuid();

		var sp = await SetUp.CreateScopedServiceProviderAsync();
		var cache = GetSimplePersistentCache(sp);
		var scopeContext = ScopeContext.Create("TEST ScopeContext")
			.AppendTraceFrameWithIduser(idUser, true);

		var key = "My:Key";
		var value = "My value";

		var saved = await cache.SetValuePermanentlyAsync(key, value);
		Assert.That(saved, Is.EqualTo(true));

		var cachedData = await cache.GetValueAsync(key);

		Assert.That(cachedData.Value, Is.EqualTo(value));

		saved = await cache.SetValuePermanentlyAsync(key, value);
		Assert.That(saved, Is.EqualTo(true));

		cachedData = await cache.GetValueAsync(key);

		Assert.That(cachedData.Value, Is.EqualTo(value));
	}

	[Test]
	public async Task CacheData_ShouldWriteSlidingCacheDataTwice()
	{
		var idUser = GlobalContext.Instance.NewGuid();

		var sp = await SetUp.CreateScopedServiceProviderAsync();
		var cache = GetSimplePersistentCache(sp);
		var scopeContext = ScopeContext.Create("TEST ScopeContext")
			.AppendTraceFrameWithIduser(idUser, true);

		var key = "My:Key";
		var value = "My value";
		var seconds = 1;

		var saved = await cache.SetValueWithSlidingExpirationAsync(key, value, TimeSpan.FromSeconds(seconds));
		Assert.That(saved, Is.EqualTo(true));

		var cachedData = await cache.GetValueAsync(key);

		Assert.That(cachedData.Value, Is.EqualTo(value));

		await Task.Delay(TimeSpan.FromSeconds(seconds));

		cachedData = await cache.GetValueAsync(key);

		Assert.That(cachedData.Value, Is.Null);
	}

	[Test]
	public async Task CacheData_ShouldWriteTimeoutCacheDataTwice()
	{
		var idUser = GlobalContext.Instance.NewGuid();

		var sp = await SetUp.CreateScopedServiceProviderAsync();
		var cache = GetSimplePersistentCache(sp);
		var scopeContext = ScopeContext.Create("TEST ScopeContext")
			.AppendTraceFrameWithIduser(idUser, true);

		var key = "My:Key";
		var value = "My value";
		var nowUtc = GlobalContext.Instance.UtcNow;
		var seconds = 1;
		var unitl = nowUtc.AddSeconds(seconds);

		var saved = await cache.SetValueWithAbsoluteExpirationAsync(key, value, unitl);
		Assert.That(saved, Is.EqualTo(true));

		var cachedData = await cache.GetValueAsync(key);

		Assert.That(cachedData.Value, Is.EqualTo(value));

		await Task.Delay(TimeSpan.FromSeconds(seconds));

		cachedData = await cache.GetValueAsync(key);

		Assert.That(cachedData.Value, Is.Null);
	}

	[Test]
	public async Task CacheData_ShouldWriteServerTimeoutCacheDataTwice()
	{
		var idUser = GlobalContext.Instance.NewGuid();

		var sp = await SetUp.CreateScopedServiceProviderAsync();
		var cache = GetSimplePersistentCache(sp);
		var scopeContext = ScopeContext.Create("TEST ScopeContext")
			.AppendTraceFrameWithIduser(idUser, true);

		var key = "My:Key";
		var value = "My value";
		var nowUtc = GlobalContext.Instance.UtcNow;
		var seconds = 1;

		var saved = await cache.SetValueWithAbsoluteServerSideExpirationAsync(key, value, TimeSpan.FromSeconds(seconds));
		Assert.That(saved, Is.EqualTo(true));

		var cachedData = await cache.GetValueAsync(key);

		Assert.That(cachedData.Value, Is.EqualTo(value));

		await Task.Delay(TimeSpan.FromSeconds(seconds));

		cachedData = await cache.GetValueAsync(key);

		Assert.That(cachedData.Value, Is.Null);
	}

	[Test]
	public async Task CacheData_ShouldUpdateCacheData()
	{
		var idUser = GlobalContext.Instance.NewGuid();

		var sp = await SetUp.CreateScopedServiceProviderAsync();
		var cache = GetSimplePersistentCache(sp);
		var scopeContext = ScopeContext.Create("TEST ScopeContext")
			.AppendTraceFrameWithIduser(idUser, true);

		var key = "My:Key";
		var value = "My value";
		var newValue = "NEW My value";

		var saved = await cache.SetValuePermanentlyAsync(key, value);
		Assert.That(saved, Is.EqualTo(true));

		var cachedData = await cache.GetValueAsync(key);

		Assert.That(cachedData.Value, Is.EqualTo(value));

		saved = await cache.TryUpdateValuePermanentlyAsync(key, value, newValue, cachedData.RowVersion.Value);
		Assert.That(saved, Is.EqualTo(true));

		cachedData = await cache.GetValueAsync(key);

		Assert.That(cachedData.Value, Is.EqualTo(newValue));
	}

	[Test]
	public async Task CacheData_ShouldUpdateSlidingCacheDataTwice()
	{
		var idUser = GlobalContext.Instance.NewGuid();

		var sp = await SetUp.CreateScopedServiceProviderAsync();
		var cache = GetSimplePersistentCache(sp);
		var scopeContext = ScopeContext.Create("TEST ScopeContext")
			.AppendTraceFrameWithIduser(idUser, true);

		var key = "My:Key";
		var value = "My value";
		var newValue = "NEW My value";
		var seconds = 1;

		var saved = await cache.SetValuePermanentlyAsync(key, value);
		Assert.That(saved, Is.EqualTo(true));

		var cachedData = await cache.GetValueAsync(key);

		Assert.That(cachedData.Value, Is.EqualTo(value));

		saved = await cache.TryUpdateValueWithSlidingExpirationAsync(key, value, newValue, cachedData.RowVersion.Value, TimeSpan.FromSeconds(seconds));
		Assert.That(saved, Is.EqualTo(true));

		cachedData = await cache.GetValueAsync(key);

		Assert.That(cachedData.Value, Is.EqualTo(newValue));

		await Task.Delay(TimeSpan.FromSeconds(seconds));

		cachedData = await cache.GetValueAsync(key);

		Assert.That(cachedData.Value, Is.Null);
	}

	[Test]
	public async Task CacheData_ShouldOverwriteExpiredSlidingCacheDataTwice()
	{
		var idUser = GlobalContext.Instance.NewGuid();

		var sp = await SetUp.CreateScopedServiceProviderAsync();
		var cache = GetSimplePersistentCache(sp);
		var scopeContext = ScopeContext.Create("TEST ScopeContext")
			.AppendTraceFrameWithIduser(idUser, true);

		var key = "My:Key";
		var value = "My value";
		var newValue = "NEW My value";
		var seconds = 1;

		var saved = await cache.SetValueWithSlidingExpirationAsync(key, value, TimeSpan.FromMilliseconds(1));
		Assert.That(saved, Is.EqualTo(true));

		saved = await cache.SetValueWithSlidingExpirationAsync(key, newValue, TimeSpan.FromSeconds(seconds));
		Assert.That(saved, Is.EqualTo(true));

		var cachedData = await cache.GetValueAsync(key);

		Assert.That(cachedData.Value, Is.EqualTo(newValue));

		await Task.Delay(TimeSpan.FromSeconds(seconds));

		cachedData = await cache.GetValueAsync(key);

		Assert.That(cachedData.Value, Is.Null);
	}

	[Test]
	public async Task CacheData_ShouldUpdateTimeoutCacheDataTwice()
	{
		var idUser = GlobalContext.Instance.NewGuid();

		var sp = await SetUp.CreateScopedServiceProviderAsync();
		var cache = GetSimplePersistentCache(sp);
		var scopeContext = ScopeContext.Create("TEST ScopeContext")
			.AppendTraceFrameWithIduser(idUser, true);

		var key = "My:Key";
		var value = "My value";
		var newValue = "NEW My value";
		var nowUtc = GlobalContext.Instance.UtcNow;
		var seconds = 1;
		var unitl = nowUtc.AddSeconds(seconds);

		var saved = await cache.SetValuePermanentlyAsync(key, value);
		Assert.That(saved, Is.EqualTo(true));

		var cachedData = await cache.GetValueAsync(key);

		Assert.That(cachedData.Value, Is.EqualTo(value));

		saved = await cache.TryUpdateValueWithAbsoluteExpirationAsync(key, value, newValue, cachedData.RowVersion.Value, unitl);
		Assert.That(saved, Is.EqualTo(true));

		cachedData = await cache.GetValueAsync(key);

		Assert.That(cachedData.Value, Is.EqualTo(newValue));

		await Task.Delay(TimeSpan.FromSeconds(seconds));

		cachedData = await cache.GetValueAsync(key);

		Assert.That(cachedData.Value, Is.Null);
	}

	[Test]
	public async Task CacheData_ShouldUpdateServerTimeoutCacheDataTwice()
	{
		var idUser = GlobalContext.Instance.NewGuid();

		var sp = await SetUp.CreateScopedServiceProviderAsync();
		var cache = GetSimplePersistentCache(sp);
		var scopeContext = ScopeContext.Create("TEST ScopeContext")
			.AppendTraceFrameWithIduser(idUser, true);

		var key = "My:Key";
		var value = "My value";
		var newValue = "NEW My value";
		var nowUtc = GlobalContext.Instance.UtcNow;
		var seconds = 1;

		var saved = await cache.SetValuePermanentlyAsync(key, value);
		Assert.That(saved, Is.EqualTo(true));

		var cachedData = await cache.GetValueAsync(key);

		Assert.That(cachedData.Value, Is.EqualTo(value));

		saved = await cache.TryUpdateValueWithAbsoluteServerSideExpirationAsync(key, value, newValue, cachedData.RowVersion.Value, TimeSpan.FromSeconds(1));
		Assert.That(saved, Is.EqualTo(true));

		cachedData = await cache.GetValueAsync(key);

		Assert.That(cachedData.Value, Is.EqualTo(newValue));

		await Task.Delay(TimeSpan.FromSeconds(seconds));

		cachedData = await cache.GetValueAsync(key);

		Assert.That(cachedData.Value, Is.Null);
	}

	[Test]
	public async Task CacheData_ShouldRemoveCacheData()
	{
		var idUser = GlobalContext.Instance.NewGuid();

		var sp = await SetUp.CreateScopedServiceProviderAsync();
		var cache = GetSimplePersistentCache(sp);
		var scopeContext = ScopeContext.Create("TEST ScopeContext")
			.AppendTraceFrameWithIduser(idUser, true);

		var key = "My:Key";
		var value = "My value";

		var saved = await cache.SetValuePermanentlyAsync(key, value);
		Assert.That(saved, Is.EqualTo(true));

		var cachedData = await cache.GetValueAsync(key);

		Assert.That(cachedData.Value, Is.EqualTo(value));

		await cache.RemoveValueAsync(key);

		cachedData = await cache.GetValueAsync(key);

		Assert.That(cachedData.Value, Is.Null);
	}
}
