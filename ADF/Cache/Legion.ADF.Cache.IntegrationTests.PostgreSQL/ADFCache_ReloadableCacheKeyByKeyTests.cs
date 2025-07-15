using Legion.ADF.Cache.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Legion.ADF.Cache.IntegrationTests;

[Category("ADFCache ReloadableCacheKey by key tests")]
public class ADFCache_ReloadableCacheKeyByKeyTests : TestBase
{
	[Test]
	public async Task ReloadableCacheKey_ShouldWriteReloadableCacheKey()
	{
		var idUser = GlobalContext.Instance.NewGuid();

		var sp = await SetUp.CreateScopedServiceProviderAsync();
		using var cacheStore = sp.GetRequiredService<ReloadableCacheKeyStore>();
		var scopeContext = ScopeContext.Create("TEST ScopeContext")
			.AppendTraceFrameWithIduser(idUser, true);

		var key = "My:Key";
		var reloadAtUtc = GlobalContext.Instance.UtcNow;

		var saveResult = await cacheStore.SaveReloadableCacheKeyAsync(scopeContext, key, tags: null, reloadAtUtc, checkPermissions: true, cancellationToken: default);
		saveResult.ThrowIfError(scopeContext, null, true);

		var cacheKeysResult = await cacheStore.GetAllReloadableCacheKeyAsync(scopeContext, checkPermissions: true, cancellationToken: default);
		cacheKeysResult.ThrowIfErrorOrNullData(scopeContext, null, true);
		var cacheKeys = cacheKeysResult.Data;

		Assert.That(cacheKeys, Has.Count.EqualTo(1));
		var first = cacheKeys.FirstOrDefault();

		Assert.That(first?.Key, Is.EqualTo(key), nameof(first.Key));
		Assert.That(first?.ReloadAtUtc.ToString("yyyy-MM-dd-HH-mm-ss-fff"), Is.EqualTo(reloadAtUtc.ToString("yyyy-MM-dd-HH-mm-ss-fff")), nameof(first.ReloadAtUtc));
	}

	[Test]
	public async Task ReloadableCacheKey_ShouldUpdateReloadableCacheKey()
	{
		var idUser = GlobalContext.Instance.NewGuid();

		var sp = await SetUp.CreateScopedServiceProviderAsync();
		using var cacheStore = sp.GetRequiredService<ReloadableCacheKeyStore>();
		var scopeContext = ScopeContext.Create("TEST ScopeContext")
			.AppendTraceFrameWithIduser(idUser, true);

		var key = "My:Key";
		var reloadAtUtc = GlobalContext.Instance.UtcNow;

		var saveResult = await cacheStore.SaveReloadableCacheKeyAsync(scopeContext, key, tags: null, reloadAtUtc, checkPermissions: true, cancellationToken: default);
		saveResult.ThrowIfError(scopeContext, null, true);

		var newReloadAtUtc = reloadAtUtc.AddYears(-10);

		saveResult = await cacheStore.SaveReloadableCacheKeyAsync(scopeContext, key, tags: null, newReloadAtUtc, checkPermissions: true, cancellationToken: default);
		saveResult.ThrowIfError(scopeContext, null, true);

		var cacheKeysResult = await cacheStore.GetAllReloadableCacheKeyAsync(scopeContext, checkPermissions: true, cancellationToken: default);
		cacheKeysResult.ThrowIfErrorOrNullData(scopeContext, null, true);
		var cacheKeys = cacheKeysResult.Data;

		Assert.That(cacheKeys, Has.Count.EqualTo(1));
		var first = cacheKeys.FirstOrDefault();

		Assert.That(first?.Key, Is.EqualTo(key), nameof(first.Key));
		Assert.That(first?.ReloadAtUtc.ToString("yyyy-MM-dd-HH-mm-ss-fff"), Is.EqualTo(newReloadAtUtc.ToString("yyyy-MM-dd-HH-mm-ss-fff")), nameof(first.ReloadAtUtc));
	}

	[Test]
	public async Task ReloadableCacheKey_ShouldRemoveReloadableCacheKey()
	{
		var idUser = GlobalContext.Instance.NewGuid();

		var sp = await SetUp.CreateScopedServiceProviderAsync();
		using var cacheStore = sp.GetRequiredService<ReloadableCacheKeyStore>();
		var scopeContext = ScopeContext.Create("TEST ScopeContext")
			.AppendTraceFrameWithIduser(idUser, true);

		var key = "My:Key";
		var reloadAtUtc = GlobalContext.Instance.UtcNow;

		var saveResult = await cacheStore.SaveReloadableCacheKeyAsync(scopeContext, key, tags: null, reloadAtUtc, checkPermissions: true, cancellationToken: default);
		saveResult.ThrowIfError(scopeContext, null, true);

		var cacheKeysResult = await cacheStore.GetAllReloadableCacheKeyAsync(scopeContext, checkPermissions: true, cancellationToken: default);
		cacheKeysResult.ThrowIfErrorOrNullData(scopeContext, null, true);
		var cacheKeys = cacheKeysResult.Data;

		Assert.That(cacheKeys, Has.Count.EqualTo(1));

		var removeResult = await cacheStore.RemoveReloadableCacheKeyAsync(scopeContext, key, checkPermissions: true, cancellationToken: default);
		removeResult.ThrowIfError(scopeContext, null, true);

		cacheKeysResult = await cacheStore.GetAllReloadableCacheKeyAsync(scopeContext, checkPermissions: true, cancellationToken: default);
		cacheKeysResult.ThrowIfErrorOrNullData(scopeContext, null, true);
		cacheKeys = cacheKeysResult.Data;

		Assert.That(cacheKeys, Has.Count.EqualTo(0));
	}
}
