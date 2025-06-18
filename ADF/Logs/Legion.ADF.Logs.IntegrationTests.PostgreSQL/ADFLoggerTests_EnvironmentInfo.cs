using Legion.MessageBus;
using Microsoft.Extensions.DependencyInjection;

namespace Legion.ADF.Logs.IntegrationTests;

[Category("ADFLogger LogMessage tests")]
public class ADFLoggerTests_EnvironmentInfo : TestBase
{
	[Test]
	public async Task ILogger_ShouldSaveEnvironmentInfo()
	{
		var sourceSystemName = "TEST ScopeContext";
		var appVersion = "1.2.3.4";

		var sp = await SetUp.CreateScopedServiceProviderAsync();
		var messageBus = sp.GetRequiredService<IMessageBus<ConnectionStringProvider>>();
		var scopeContext = ScopeContext.Create(sourceSystemName);

		await using var logsStore = sp.GetRequiredService<Legion.ADF.Logs.Services.LogsStore>();
		var saveResult = await logsStore.SaveEnvironmentInfoAsync(
			scopeContext,
			sourceSystemName,
			appVersion,
			true,
			cancellationToken: default);

		Assert.That(!saveResult.HasError && saveResult.Data != null);

		var query = new Logs.Queries.EnvironmentInfo.GetEnvironmentInfoByIdQuery(Legion.Infrastructure.EnvironmentInfo.RUNTIME_UNIQUE_KEY, CheckReadPermissions: true, AsNoTracking: true);
		var result = await messageBus.SendAsync(scopeContext, query);

		Assert.That(!result.HasError && result.Data != null);

		saveResult = await logsStore.SaveEnvironmentInfoAsync(
			scopeContext,
			sourceSystemName,
			appVersion,
			true,
			cancellationToken: default);

		Assert.That(!saveResult.HasError && saveResult.Data != null);
		Assert.That(result.Data!.CreatedUtc, Is.EqualTo(saveResult.Data!.CreatedUtc));
	}
}
