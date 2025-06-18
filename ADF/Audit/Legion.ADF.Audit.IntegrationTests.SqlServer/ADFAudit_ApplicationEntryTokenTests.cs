using Legion.ADF.Audit.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Legion.ADF.Audit.IntegrationTests;

[Category("ADFAudit ApplicationEntryToken tests")]
public class ADFAudit_ApplicationEntryTokenTests : TestBase
{
	[Test]
	public async Task ApplicationEntryToken_ShouldCreateApplicationEntryToken()
	{
		var idUser = Guid.NewGuid();

		var sp = await SetUp.CreateScopedServiceProviderAsync();
		using var auditStore = sp.GetRequiredService<AuditStore>();
		var scopeContext = ScopeContext.Create("TEST ScopeContext")
			.AppendTraceFrameWithIduser(idUser, true);

		var token = "Export";
		var sourceFilePath = "Pages\\Forms\\ZiadostiOPrihlaskuPohladavky\\Create.cshtml.cs";
		var methodInfo = "Disig.EGovOffice.Web.Pages.Forms.ZiadostiOPrihlaskuPohladavky.CreateModel.OnPostExportToJsonAsync(CancellationToken cancellationToken); Disig.EGovOffice.Web, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null";
		var aggregateName = "Podanie prihlášky pohľadávky";
		var description = "description description description";

		var saveResult = await auditStore.SaveApplicationEntryTokenAsync(
			scopeContext,
			token,
			sourceFilePath,
			methodInfo,
			aggregateName,
			description,
			checkPermissions: true,
			cancellationToken: default);

		saveResult.ThrowIfError(scopeContext, null, true);

		await using var uow = CreateAuditUnitOfWork(scopeContext, sp);
		var tokens = await uow.ApplicationEntryTokenRepository
		.AsQueryable(scopeContext)
		.ToListAsync(cancellationToken: default);

		Assert.That(tokens, Has.Count.EqualTo(1));
		var first = tokens.FirstOrDefault();

		Assert.That(first?.Token, Is.EqualTo(token), nameof(first.Token));
		Assert.That(first?.SourceFilePath, Is.EqualTo(sourceFilePath), nameof(first.SourceFilePath));
		Assert.That(first?.MethodInfo, Is.EqualTo(methodInfo), nameof(first.MethodInfo));
		Assert.That(first?.AggregateName, Is.EqualTo(aggregateName), nameof(first.AggregateName));
		Assert.That(first?.Description, Is.EqualTo(description), nameof(first.Description));
	}

	[Test]
	public async Task ApplicationEntryToken_ShouldUpdateApplicationEntryToken()
	{
		var idUser = Guid.NewGuid();

		var sp = await SetUp.CreateScopedServiceProviderAsync();
		using var auditStore = sp.GetRequiredService<AuditStore>();
		var scopeContext = ScopeContext.Create("TEST ScopeContext")
			.AppendTraceFrameWithIduser(idUser, true);

		var token = "Export";
		var sourceFilePath = "Pages\\Forms\\ZiadostiOPrihlaskuPohladavky\\Create.cshtml.cs";
		var methodInfo = "Disig.EGovOffice.Web.Pages.Forms.ZiadostiOPrihlaskuPohladavky.CreateModel.OnPostExportToJsonAsync(CancellationToken cancellationToken); Disig.EGovOffice.Web, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null";
		var mainEntityName = "Podanie prihlášky pohľadávky";
		var description = "description description description";

		var saveResult = await auditStore.SaveApplicationEntryTokenAsync(
			scopeContext,
			token,
			sourceFilePath,
			methodInfo,
			mainEntityName,
			description,
			checkPermissions: true,
			cancellationToken: default);

		saveResult.ThrowIfError(scopeContext, null, true);

		var newMethodInfo = "22222 Disig.EGovOffice.Web.Pages.Forms.ZiadostiOPrihlaskuPohladavky.CreateModel.OnPostExportToJsonAsync(CancellationToken cancellationToken); Disig.EGovOffice.Web, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null";
		var aggregateName = "Podanie2 prihlášky2 pohľadávky2";
		var newDescription = "description2";

		var updateResult = await auditStore.SaveApplicationEntryTokenAsync(
			scopeContext,
			token,
			sourceFilePath,
			newMethodInfo,
			aggregateName,
			newDescription,
			checkPermissions: true,
			cancellationToken: default);

		updateResult.ThrowIfError(scopeContext, null, true);

		await using var uow = CreateAuditUnitOfWork(scopeContext, sp);
		var tokens = await uow.ApplicationEntryTokenRepository
		.AsQueryable(scopeContext)
		.ToListAsync(cancellationToken: default);

		Assert.That(tokens, Has.Count.EqualTo(1));
		var first = tokens.FirstOrDefault();

		Assert.That(first?.Token, Is.EqualTo(token), nameof(first.Token));
		Assert.That(first?.SourceFilePath, Is.EqualTo(sourceFilePath), nameof(first.SourceFilePath));
		Assert.That(first?.MethodInfo, Is.EqualTo(newMethodInfo), nameof(first.MethodInfo));
		Assert.That(first?.AggregateName, Is.EqualTo(aggregateName), nameof(first.AggregateName));
		Assert.That(first?.Description, Is.EqualTo(newDescription), nameof(first.Description));
	}
}
