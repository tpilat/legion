using Legion.ADF.Audit.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Text;

namespace Legion.ADF.Audit.IntegrationTests;

[Category("ADFAudit ApplicationEntry tests")]
public class ADFAudit_ApplicationEntryTests : TestBase
{
	[Test]
	public async Task ApplicationEntry_ShouldCreateApplicationEntry()
	{
		var idUser = GlobalContext.Instance.NewGuid();
		var tenantIdentifier = GlobalContext.Instance.NewGuid();
		var correlationId = GlobalContext.Instance.NewGuid();
		var externalCorrelationId = GlobalContext.Instance.NewGuid().ToString();

		var sp = await SetUp.CreateScopedServiceProviderAsync();
		using var auditStore = sp.GetRequiredService<AuditStore>();
		var scopeContext = ScopeContext.Create("TEST ScopeContext", correlationId: correlationId, externalCorrelationId: externalCorrelationId)
			.AppendTraceFrameWithIduser(idUser, true)
			.AppendTraceFrameWithTenantIdentifier(tenantIdentifier, true);

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

		var appEntryToken = saveResult.Data!;

		var idAuditOperation = Model.AuditOperation.Update;
		var aggregateIdentifier = GlobalContext.Instance.NewGuid().ToString();
		var uri = "http://10.60.20.15:31003/sk/Forms/ZiadostiOPrihlaskuPohladavky/Update?handler=Import";

		var requestString = "My audit info";
		var responseString = "My audit reponse";

		var writeResult = await auditStore.WriteApplicationEntryAsync(
			scopeContext,
			appEntryToken.IdApplicationEntryToken,
			idAuditOperation,
			aggregateIdentifier,
			uri,
			checkPermissions: true,
			[
				new DTOs.StringContent(requestString, Encoding.UTF8)
			],
			cancellationToken: default);

		writeResult.ThrowIfError(scopeContext, null, true);

		var writeResponseResult = await auditStore.WriteApplicationEntryResponseAsync(
			scopeContext,
			writeResult.Data,
			"200",
			error: null,
			elapsedMilliseconds: 123.456M,
			[
				new DTOs.StringContent(responseString, Encoding.UTF8)
			],
			cancellationToken: default);

		writeResponseResult.ThrowIfError(scopeContext, null, true);

		await using var uow = CreateAuditUnitOfWork(scopeContext, sp);
		var appEntries = await uow.ApplicationEntryRepository
			.AsQueryable(scopeContext)
			.ToListAsync(cancellationToken: default);

		Assert.That(appEntries, Has.Count.EqualTo(1));
		var first = appEntries.FirstOrDefault();

		Assert.That(first?.IdApplicationEntryToken, Is.EqualTo(appEntryToken.IdApplicationEntryToken), nameof(first.IdApplicationEntryToken));
		Assert.That(first?.IdAuditOperation, Is.EqualTo(idAuditOperation), nameof(first.IdAuditOperation));
		Assert.That(first?.RuntimeUniqueKey, Is.EqualTo(Infrastructure.EnvironmentInfo.RUNTIME_UNIQUE_KEY), nameof(first.RuntimeUniqueKey));
		Assert.That(first?.CorrelationId, Is.EqualTo(correlationId), nameof(first.CorrelationId));
		Assert.That(first?.ExternalCorrelationId, Is.EqualTo(externalCorrelationId), nameof(first.ExternalCorrelationId));
		Assert.That(first?.AggregateIdentifier, Is.EqualTo(aggregateIdentifier), nameof(first.AggregateIdentifier));
		Assert.That(first?.Uri, Is.EqualTo(uri), nameof(first.Uri));
		Assert.That(first?.IdUser, Is.EqualTo(idUser), nameof(first.IdUser));
		Assert.That(first?.TenantIdentifier, Is.EqualTo(tenantIdentifier), nameof(first.TenantIdentifier));

		var appEntryRequests = await uow.ApplicationEntryRequestRepository
			.AsQueryable(scopeContext)
			.ToListAsync(cancellationToken: default);

		Assert.That(appEntryRequests, Has.Count.EqualTo(1));
		var firstRequest = appEntryRequests.FirstOrDefault();

		Assert.That(firstRequest?.IdApplicationEntry, Is.EqualTo(first.IdApplicationEntry), nameof(firstRequest.IdApplicationEntry));
		Assert.That(firstRequest?.StringContent, Is.EqualTo(requestString), nameof(firstRequest.StringContent));

		var appEntryResponses = await uow.ApplicationEntryResponseRepository
			.AsQueryable(scopeContext)
			.ToListAsync(cancellationToken: default);

		Assert.That(appEntryResponses, Has.Count.EqualTo(1));
		var firstResponse = appEntryResponses.FirstOrDefault();

		Assert.That(firstResponse?.IdApplicationEntry, Is.EqualTo(first.IdApplicationEntry), nameof(firstResponse.IdApplicationEntry));
		Assert.That(firstResponse?.StringContent, Is.EqualTo(responseString), nameof(firstResponse.StringContent));
		Assert.That(firstResponse?.StatusCode, Is.EqualTo("200"), nameof(firstResponse.StatusCode));
		Assert.That(firstResponse?.Error, Is.Null, nameof(firstResponse.Error));
	}
}
