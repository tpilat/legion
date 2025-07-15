using Legion.ADF.Config.Services;
using Legion.Caching;
using Microsoft.Extensions.DependencyInjection;

namespace Legion.ADF.Config.IntegrationTests;

[Category("ADFConfig ConfigurationClass tests")]
public class ADFConfig_ConfigurationClassTests : TestBase
{
	[Test]
	public async Task ConfigurationClass_ShouldWriteConfigurationClass()
	{
		var idUser = GlobalContext.Instance.NewGuid();

		var sp = await SetUp.CreateScopedServiceProviderAsync();
		using var configStore = sp.GetRequiredService<ConfigStore>();
		var scopeContext = ScopeContext.Create("TEST ScopeContext")
			.AppendTraceFrameWithIduser(idUser, true);

		var rootPath = "Project::Root:Test";
		var displayName = "Test cfg";
		var csharpClassTypeToDeserialize = "My.Project.ClassToDeserialize";

		var saveResult = await configStore.SaveConfigurationClassAsync(scopeContext, rootPath, displayName, csharpClassTypeToDeserialize, checkPermissions: true, cancellationToken: default);
		saveResult.ThrowIfError(scopeContext, null, true);

		await using var uow = CreateConfigUnitOfWork(scopeContext, sp);
		var configs = await uow.ConfigurationClassRepository
			.GetAllConfigurationClasses(new Queries.ConfigurationClass.GetAllConfigurationClassesQuery(CheckReadPermissions: true, AsNoTracking: true))
			.ToResultAsync(scopeContext, cancellationToken: default);

		Assert.That(configs, Has.Count.EqualTo(1));
		var first = configs.FirstOrDefault();

		Assert.That(first?.RootPath, Is.EqualTo(rootPath), nameof(first.RootPath));
		Assert.That(first?.DisplayName, Is.EqualTo(displayName), nameof(first.DisplayName));
		Assert.That(first?.Class, Is.EqualTo(csharpClassTypeToDeserialize), nameof(first.Class));
	}

	[Test]
	public async Task ConfigurationClass_ShouldUpdateConfigurationClass()
	{
		var idUser = GlobalContext.Instance.NewGuid();

		var sp = await SetUp.CreateScopedServiceProviderAsync();
		using var configStore = sp.GetRequiredService<ConfigStore>();
		var scopeContext = ScopeContext.Create("TEST ScopeContext")
			.AppendTraceFrameWithIduser(idUser, true);

		var rootPath = "Project::Root:Test";
		var displayName = "Test cfg";
		var csharpClassTypeToDeserialize = "My.Project.ClassToDeserialize";

		var saveResult = await configStore.SaveConfigurationClassAsync(scopeContext, rootPath, displayName, csharpClassTypeToDeserialize, checkPermissions: true, cancellationToken: default);
		saveResult.ThrowIfError(scopeContext, null, true);

		await using var uow = CreateConfigUnitOfWork(scopeContext, sp);
		var configs = await uow.ConfigurationClassRepository
			.GetAllConfigurationClasses(new Queries.ConfigurationClass.GetAllConfigurationClassesQuery(CheckReadPermissions: true, AsNoTracking: true))
			.ToResultAsync(scopeContext, cancellationToken: default);

		var newDisplayName = "UPDATED va lu e";
		var newCSharpClassTypeToDeserialize = "My.Project.ClassToDeserialize.V2";

		saveResult = await configStore.SaveConfigurationClassAsync(scopeContext, rootPath, newDisplayName, newCSharpClassTypeToDeserialize, checkPermissions: true, cancellationToken: default);
		saveResult.ThrowIfError(scopeContext, null, true);

		configs = await uow.ConfigurationClassRepository
			.GetAllConfigurationClasses(new Queries.ConfigurationClass.GetAllConfigurationClassesQuery(CheckReadPermissions: true, AsNoTracking: true))
			.ToResultAsync(scopeContext, cancellationToken: default);

		Assert.That(configs, Has.Count.EqualTo(1));
		var first = configs.FirstOrDefault();

		Assert.That(first?.RootPath, Is.EqualTo(rootPath), nameof(first.RootPath));
		Assert.That(first?.DisplayName, Is.EqualTo(newDisplayName), nameof(first.DisplayName));
		Assert.That(first?.Class, Is.EqualTo(newCSharpClassTypeToDeserialize), nameof(first.Class));
	}

	[Test]
	public async Task ConfigurationClass_ShouldRemoveConfigurationClass()
	{
		var idUser = GlobalContext.Instance.NewGuid();

		var sp = await SetUp.CreateScopedServiceProviderAsync();
		using var configStore = sp.GetRequiredService<ConfigStore>();
		var scopeContext = ScopeContext.Create("TEST ScopeContext")
			.AppendTraceFrameWithIduser(idUser, true);

		var rootPath = "Project::Root:Test";
		var displayName = "Test cfg";
		var csharpClassTypeToDeserialize = "My.Project.ClassToDeserialize";

		var saveResult = await configStore.SaveConfigurationClassAsync(scopeContext, rootPath, displayName, csharpClassTypeToDeserialize, checkPermissions: true, cancellationToken: default);
		saveResult.ThrowIfError(scopeContext, null, true);

		await using var uow = CreateConfigUnitOfWork(scopeContext, sp);
		var configs = await uow.ConfigurationClassRepository
			.GetAllConfigurationClasses(new Queries.ConfigurationClass.GetAllConfigurationClassesQuery(CheckReadPermissions: true, AsNoTracking: true))
			.ToResultAsync(scopeContext, cancellationToken: default);

		Assert.That(configs, Has.Count.EqualTo(1));

		var removeResult = await configStore.RemoveConfigurationAsync(scopeContext, rootPath, checkPermissions: true, cancellationToken: default);
		removeResult.ThrowIfError(scopeContext, null, true);

		configs = await uow.ConfigurationClassRepository
			.GetAllConfigurationClasses(new Queries.ConfigurationClass.GetAllConfigurationClassesQuery(CheckReadPermissions: true, AsNoTracking: true))
			.ToResultAsync(scopeContext, cancellationToken: default);

		Assert.That(configs, Has.Count.EqualTo(0));
	}
}
