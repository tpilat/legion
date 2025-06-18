using Legion.ADF.Cache.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Legion.ADF.Cache.IntegrationTests;

[Category("ADFCache ReloadableCacheKey by unordered tags tests")]
public class ADFCache_ReloadableCacheKeyByTagsOrderTests : TestBase
{
	[Test]
	public async Task ReloadableCacheKey_ShouldWriteReloadableCacheKey()
	{
		var idUser = Guid.NewGuid();

		var sp = await SetUp.CreateScopedServiceProviderAsync();
		using var cacheStore = sp.GetRequiredService<ReloadableCacheKeyStore>();
		var scopeContext = ScopeContext.Create("TEST ScopeContext")
			.AppendTraceFrameWithIduser(idUser, true);

		var tags = new List<string> { "tag2", "tag3", "tag1" };
		var reloadAtUtc = GlobalContext.Instance.UtcNow;

		var saveResult = await cacheStore.SaveReloadableCacheKeyAsync(scopeContext, key: null, tags, reloadAtUtc, checkPermissions: true, cancellationToken: default);
		saveResult.ThrowIfError(scopeContext, null, true);

		var cacheKeysResult = await cacheStore.GetAllReloadableCacheKeyAsync(scopeContext, checkPermissions: true, cancellationToken: default);
		cacheKeysResult.ThrowIfErrorOrNullData(scopeContext, null, true);
		var cacheKeys = cacheKeysResult.Data;

		Assert.That(cacheKeys, Has.Count.EqualTo(1));
		var first = cacheKeys.FirstOrDefault();
		
		tags.Reverse();

		Assert.That(first?.Tags, Is.EquivalentTo(tags), nameof(first.Tags));
		Assert.That(first?.ReloadAtUtc.ToString("yyyy-MM-dd-HH-mm-ss-fff"), Is.EqualTo(reloadAtUtc.ToString("yyyy-MM-dd-HH-mm-ss-fff")), nameof(first.ReloadAtUtc));
	}

	[Test]
	public async Task ReloadableCacheKey_ShouldUpdateReloadableCacheKey()
	{
		var idUser = Guid.NewGuid();

		var sp = await SetUp.CreateScopedServiceProviderAsync();
		using var cacheStore = sp.GetRequiredService<ReloadableCacheKeyStore>();
		var scopeContext = ScopeContext.Create("TEST ScopeContext")
			.AppendTraceFrameWithIduser(idUser, true);

		var tags = new List<string> { "tag2", "tag3", "tag1" };
		var reloadAtUtc = GlobalContext.Instance.UtcNow;

		var saveResult = await cacheStore.SaveReloadableCacheKeyAsync(scopeContext, key: null, tags, reloadAtUtc, checkPermissions: true, cancellationToken: default);
		saveResult.ThrowIfError(scopeContext, null, true);

		var newReloadAtUtc = reloadAtUtc.AddYears(-10);

		tags.Reverse();

		saveResult = await cacheStore.SaveReloadableCacheKeyAsync(scopeContext, key: null, tags, newReloadAtUtc, checkPermissions: true, cancellationToken: default);
		saveResult.ThrowIfError(scopeContext, null, true);

		var cacheKeysResult = await cacheStore.GetAllReloadableCacheKeyAsync(scopeContext, checkPermissions: true, cancellationToken: default);
		cacheKeysResult.ThrowIfErrorOrNullData(scopeContext, null, true);
		var cacheKeys = cacheKeysResult.Data;

		Assert.That(cacheKeys, Has.Count.EqualTo(1));
		var first = cacheKeys.FirstOrDefault();

		Assert.That(first?.Tags, Is.EquivalentTo(tags), nameof(first.Tags));
		Assert.That(first?.ReloadAtUtc.ToString("yyyy-MM-dd-HH-mm-ss-fff"), Is.EqualTo(newReloadAtUtc.ToString("yyyy-MM-dd-HH-mm-ss-fff")), nameof(first.ReloadAtUtc));
	}

	[Test]
	public async Task ReloadableCacheKey_ShouldRemoveReloadableCacheKey()
	{
		var idUser = Guid.NewGuid();

		var sp = await SetUp.CreateScopedServiceProviderAsync();
		using var cacheStore = sp.GetRequiredService<ReloadableCacheKeyStore>();
		var scopeContext = ScopeContext.Create("TEST ScopeContext")
			.AppendTraceFrameWithIduser(idUser, true);

		var tags = new List<string> { "tag2", "tag3", "tag1" };
		var reloadAtUtc = GlobalContext.Instance.UtcNow;

		tags.Reverse();

		var saveResult = await cacheStore.SaveReloadableCacheKeyAsync(scopeContext, key: null, tags, reloadAtUtc, checkPermissions: true, cancellationToken: default);
		saveResult.ThrowIfError(scopeContext, null, true);

		var cacheKeysResult = await cacheStore.GetAllReloadableCacheKeyAsync(scopeContext, checkPermissions: true, cancellationToken: default);
		cacheKeysResult.ThrowIfErrorOrNullData(scopeContext, null, true);
		var cacheKeys = cacheKeysResult.Data;

		Assert.That(cacheKeys, Has.Count.EqualTo(1));

		var removeResult = await cacheStore.RemoveReloadableCacheKeyAsync(scopeContext, tags, checkPermissions: true, cancellationToken: default);
		removeResult.ThrowIfError(scopeContext, null, true);

		cacheKeysResult = await cacheStore.GetAllReloadableCacheKeyAsync(scopeContext, checkPermissions: true, cancellationToken: default);
		cacheKeysResult.ThrowIfErrorOrNullData(scopeContext, null, true);
		cacheKeys = cacheKeysResult.Data;

		Assert.That(cacheKeys, Has.Count.EqualTo(0));
	}
}
