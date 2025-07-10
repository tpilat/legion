namespace Legion.ADF.Cache.IntegrationTests;

[Category("ADFCache CacheData tests")]
public class ADFCache_CacheDataByKeyTests : TestBase
{
	[Test]
	public async Task CacheData_ShouldWriteCacheData()
	{
		var idUser = Guid.NewGuid();

		var sp = await SetUp.CreateScopedServiceProviderAsync();
		var cache = GetSimplePersistentCache(sp);
		var scopeContext = ScopeContext.Create("TEST ScopeContext")
			.AppendTraceFrameWithIduser(idUser, true);

		var key = "My:Key";
		var value = "My value";

		var saved = await cache.SetValuePermanentlyAsync(key, value);
		Assert.That(saved, Is.EqualTo(true));

		var cachedData = await cache.GetValue(key);

		Assert.That(cachedData.Value, Is.EqualTo(value));
		Assert.That(cachedData.RowVersion, Is.EqualTo(0));

		saved = await cache.SetValuePermanentlyAsync(key, value);
		Assert.That(saved, Is.EqualTo(true));

		cachedData = await cache.GetValue(key);

		Assert.That(cachedData.Value, Is.EqualTo(value));
		Assert.That(cachedData.RowVersion, Is.EqualTo(1));
	}

	[Test]
	public async Task CacheData_ShouldWriteSlidingCacheDataTwice()
	{
		var idUser = Guid.NewGuid();

		var sp = await SetUp.CreateScopedServiceProviderAsync();
		var cache = GetSimplePersistentCache(sp);
		var scopeContext = ScopeContext.Create("TEST ScopeContext")
			.AppendTraceFrameWithIduser(idUser, true);

		var key = "My:Key";
		var value = "My value";
		var seconds = 1;

		var saved = await cache.SetValueWithSlidingExpirationAsync(key, value, TimeSpan.FromSeconds(seconds));
		Assert.That(saved, Is.EqualTo(true));

		var cachedData = await cache.GetValue(key);

		Assert.That(cachedData.Value, Is.EqualTo(value));
		Assert.That(cachedData.RowVersion, Is.EqualTo(0));

		await Task.Delay(TimeSpan.FromSeconds(seconds));

		cachedData = await cache.GetValue(key);

		Assert.That(cachedData.Value, Is.Null);
	}

	[Test]
	public async Task CacheData_ShouldWriteTimeoutCacheDataTwice()
	{
		var idUser = Guid.NewGuid();

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

		var cachedData = await cache.GetValue(key);

		Assert.That(cachedData.Value, Is.EqualTo(value));
		Assert.That(cachedData.RowVersion, Is.EqualTo(0));

		await Task.Delay(TimeSpan.FromSeconds(seconds));

		cachedData = await cache.GetValue(key);

		Assert.That(cachedData.Value, Is.Null);
	}

	[Test]
	public async Task CacheData_ShouldUpdateCacheData()
	{
		var idUser = Guid.NewGuid();

		var sp = await SetUp.CreateScopedServiceProviderAsync();
		var cache = GetSimplePersistentCache(sp);
		var scopeContext = ScopeContext.Create("TEST ScopeContext")
			.AppendTraceFrameWithIduser(idUser, true);

		var key = "My:Key";
		var value = "My value";
		var newValue = "NEW My value";

		var saved = await cache.SetValuePermanentlyAsync(key, value);
		Assert.That(saved, Is.EqualTo(true));

		var cachedData = await cache.GetValue(key);

		Assert.That(cachedData.Value, Is.EqualTo(value));
		Assert.That(cachedData.RowVersion, Is.EqualTo(0));

		saved = await cache.TryUpdateValuePermanentlyAsync(key, value, newValue, cachedData.RowVersion.Value);
		Assert.That(saved, Is.EqualTo(true));

		cachedData = await cache.GetValue(key);

		Assert.That(cachedData.Value, Is.EqualTo(newValue));
		Assert.That(cachedData.RowVersion, Is.EqualTo(1));
	}

	[Test]
	public async Task CacheData_ShouldUpdateSlidingCacheDataTwice()
	{
		var idUser = Guid.NewGuid();

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

		var cachedData = await cache.GetValue(key);

		Assert.That(cachedData.Value, Is.EqualTo(value));
		Assert.That(cachedData.RowVersion, Is.EqualTo(0));

		saved = await cache.TryUpdateValueWithSlidingExpirationAsync(key, value, newValue, cachedData.RowVersion.Value, TimeSpan.FromSeconds(seconds));
		Assert.That(saved, Is.EqualTo(true));

		cachedData = await cache.GetValue(key);

		Assert.That(cachedData.Value, Is.EqualTo(newValue));
		Assert.That(cachedData.RowVersion, Is.EqualTo(1));

		await Task.Delay(TimeSpan.FromSeconds(seconds));

		cachedData = await cache.GetValue(key);

		Assert.That(cachedData.Value, Is.Null);
	}

	[Test]
	public async Task CacheData_ShouldUpdateTimeoutCacheDataTwice()
	{
		var idUser = Guid.NewGuid();

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

		var cachedData = await cache.GetValue(key);

		Assert.That(cachedData.Value, Is.EqualTo(value));
		Assert.That(cachedData.RowVersion, Is.EqualTo(0));

		saved = await cache.TryUpdateValueWithAbsoluteExpirationAsync(key, value, newValue, cachedData.RowVersion.Value, unitl);
		Assert.That(saved, Is.EqualTo(true));

		cachedData = await cache.GetValue(key);

		Assert.That(cachedData.Value, Is.EqualTo(newValue));
		Assert.That(cachedData.RowVersion, Is.EqualTo(1));

		await Task.Delay(TimeSpan.FromSeconds(seconds));

		cachedData = await cache.GetValue(key);

		Assert.That(cachedData.Value, Is.Null);
	}

	[Test]
	public async Task CacheData_ShouldRemoveCacheData()
	{
		var idUser = Guid.NewGuid();

		var sp = await SetUp.CreateScopedServiceProviderAsync();
		var cache = GetSimplePersistentCache(sp);
		var scopeContext = ScopeContext.Create("TEST ScopeContext")
			.AppendTraceFrameWithIduser(idUser, true);

		var key = "My:Key";
		var value = "My value";

		var saved = await cache.SetValuePermanentlyAsync(key, value);
		Assert.That(saved, Is.EqualTo(true));

		var cachedData = await cache.GetValue(key);

		Assert.That(cachedData.Value, Is.EqualTo(value));
		Assert.That(cachedData.RowVersion, Is.EqualTo(0));

		await cache.RemoveValueAsync(key);

		cachedData = await cache.GetValue(key);

		Assert.That(cachedData.Value, Is.Null);
	}
}
