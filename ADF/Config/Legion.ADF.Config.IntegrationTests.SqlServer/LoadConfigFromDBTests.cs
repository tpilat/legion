using Legion.ADF.Config.Configuration;
using Legion.ADF.Config.Extensions;
using Legion.ADF.Config.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Legion.ADF.Config.IntegrationTests;

[Category("Load configuration from DB tests")]
public class LoadConfigFromDBTests : TestBase
{
	[Test]
	public async Task LoadConfiguration_ShouldLoadFromDB()
	{
		var idUser = GlobalContext.Instance.NewGuid();

		var sp = await SetUp.CreateScopedServiceProviderAsync();
		using var configStore = sp.GetRequiredService<ConfigStore>();
		var scopeContext = ScopeContext.Create("TEST ScopeContext")
			.AppendTraceFrameWithIduser(idUser, true);

		var key = "My:Key";
		var value = "va lu e";

		var saveResult = await configStore.SaveConfigKeyValueAsync(scopeContext, key, value, checkPermissions: true, cancellationToken: default);
		saveResult.ThrowIfError(scopeContext, null, true);

		var configuration = new ConfigurationBuilder()
			.SetBasePath(Directory.GetCurrentDirectory())
			.AddDBConfiguration(() => new DBConfigurationManager(sp, SetUp.ConncetionString))
			.Build();

		Assert.That(configuration.GetSection(key).Value, Is.EqualTo(value));
	}
}
