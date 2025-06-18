using Legion.ADF.Config.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Legion.ADF.Config.IntegrationTests;

[Category("ADFConfig ConfigurationKeyValue tests")]
public class ADFConfig_ConfigurationKeyValueTests : TestBase
{
	[Test]
	public async Task ConfigurationKeyValue_ShouldWriteConfigurationKeyValue()
	{
		var idUser = Guid.NewGuid();

		var sp = await SetUp.CreateScopedServiceProviderAsync();
		using var configStore = sp.GetRequiredService<ConfigStore>();
		var scopeContext = ScopeContext.Create("TEST ScopeContext")
			.AppendTraceFrameWithIduser(idUser, true);

		var key = "My:Key";
		var value = "va lu e";

		var saveResult = await configStore.SaveConfigKeyValueAsync(scopeContext, key, value, checkPermissions: true, cancellationToken: default);
		saveResult.ThrowIfError(scopeContext, null, true);

		await using var uow = CreateConfigUnitOfWork(scopeContext, sp);
		var keyValues = await uow.ConfigurationKeyValueRepository
			.GetAllConfigurationKeyValues(new Queries.ConfigurationKeyValue.GetAllConfigurationKeyValuesQuery(CheckReadPermissions: true, AsNoTracking: true))
			.ToResultAsync(scopeContext, cancellationToken: default);

		Assert.That(keyValues, Has.Count.EqualTo(1));
		var first = keyValues.FirstOrDefault();

		Assert.That(first?.Key, Is.EqualTo(key), nameof(first.Key));
		Assert.That(first?.Value, Is.EqualTo(value), nameof(first.Value));
		Assert.That(first?.IdAuditCreatedBy, Is.EqualTo(idUser), nameof(first.IdAuditCreatedBy));
	}

	[Test]
	public async Task ConfigurationKeyValue_ShouldUpdateConfigurationKeyValue()
	{
		var idUser = Guid.NewGuid();

		var sp = await SetUp.CreateScopedServiceProviderAsync();
		using var configStore = sp.GetRequiredService<ConfigStore>();
		var scopeContext = ScopeContext.Create("TEST ScopeContext")
			.AppendTraceFrameWithIduser(idUser, true);

		var key = "My:Key";
		var value = "va lu e";

		var saveResult = await configStore.SaveConfigKeyValueAsync(scopeContext, key, value, checkPermissions: true, cancellationToken: default);
		saveResult.ThrowIfError(scopeContext, null, true);

		var newValue = "UPDATED va lu e";

		saveResult = await configStore.SaveConfigKeyValueAsync(scopeContext, key, newValue, checkPermissions: true, cancellationToken: default);
		saveResult.ThrowIfError(scopeContext, null, true);

		await using var uow = CreateConfigUnitOfWork(scopeContext, sp);
		var keyValues = await uow.ConfigurationKeyValueRepository
			.GetAllConfigurationKeyValues(new Queries.ConfigurationKeyValue.GetAllConfigurationKeyValuesQuery(CheckReadPermissions: true, AsNoTracking: true))
			.ToResultAsync(scopeContext, cancellationToken: default);

		Assert.That(keyValues, Has.Count.EqualTo(1));
		var first = keyValues.FirstOrDefault();

		Assert.That(first?.Key, Is.EqualTo(key), nameof(first.Key));
		Assert.That(first?.Value, Is.EqualTo(newValue), nameof(first.Value));
		Assert.That(first?.IdAuditCreatedBy, Is.EqualTo(idUser), nameof(first.IdAuditCreatedBy));
		Assert.That(first?.IdAuditModifiedBy, Is.EqualTo(idUser), nameof(first.IdAuditModifiedBy));
	}

	[Test]
	public async Task ConfigurationKeyValue_ShouldRemoveConfigurationKeyValue()
	{
		var idUser = Guid.NewGuid();

		var sp = await SetUp.CreateScopedServiceProviderAsync();
		using var configStore = sp.GetRequiredService<ConfigStore>();
		var scopeContext = ScopeContext.Create("TEST ScopeContext")
			.AppendTraceFrameWithIduser(idUser, true);

		var key = "My:Key";
		var value = "va lu e";

		var saveResult = await configStore.SaveConfigKeyValueAsync(scopeContext, key, value, checkPermissions: true, cancellationToken: default);
		saveResult.ThrowIfError(scopeContext, null, true);

		await using var uow = CreateConfigUnitOfWork(scopeContext, sp);
		var keyValues = await uow.ConfigurationKeyValueRepository
			.GetAllConfigurationKeyValues(new Queries.ConfigurationKeyValue.GetAllConfigurationKeyValuesQuery(CheckReadPermissions: true, AsNoTracking: true))
			.ToResultAsync(scopeContext, cancellationToken: default);

		Assert.That(keyValues, Has.Count.EqualTo(1));

		var removeResult = await configStore.RemoveConfigKeyValueAsync(scopeContext, key, checkPermissions: true, cancellationToken: default);
		removeResult.ThrowIfError(scopeContext, null, true);

		keyValues = await uow.ConfigurationKeyValueRepository
			.GetAllConfigurationKeyValues(new Queries.ConfigurationKeyValue.GetAllConfigurationKeyValuesQuery(CheckReadPermissions: true, AsNoTracking: true))
			.ToResultAsync(scopeContext, cancellationToken: default);

		Assert.That(keyValues, Has.Count.EqualTo(0));
	}
}
