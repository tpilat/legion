using Legion.ADF.Audit.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Legion.ADF.Audit.IntegrationTests;

[Category("ADFAudit AuditEntry tests")]
public class ADFAudit_AuditEntryTests : TestBase
{
	[Test]
	public async Task AuditEntry_ShouldCreateAuditEntry()
	{
		var idUser = Guid.NewGuid();
		var tenantIdentifier = Guid.NewGuid();
		var correlationId = Guid.NewGuid();
		var externalCorrelationId = Guid.NewGuid().ToString();

		var sp = await SetUp.CreateScopedServiceProviderAsync();
		using var auditStore = sp.GetRequiredService<AuditStore>();
		var scopeContext = ScopeContext.Create("TEST ScopeContext", correlationId: correlationId, externalCorrelationId: externalCorrelationId)
			.AppendTraceFrameWithIduser(idUser, true)
			.AppendTraceFrameWithTenantIdentifier(tenantIdentifier, true);

		var idAuditOperation = Model.AuditOperation.Update;
		var tableName = "Message";
		var primaryKey = "{\"IdMessage\": \"ec5c7275-c950-4f09-9c65-5dbc88246f47\"}";
		var oldValues = "{\"ClosedAt\": \"2000-08-03T15:19:24.8527282+02:00\"}";
		var newValues = "{\"ClosedAt\": \"2024-09-03T15:19:24.8527282+02:00\"}";
		var affectedColumns = "[\"ClosedAt\"]";
		var auditCorrelationId = Guid.NewGuid();
		var traceFrame = "trace frames";

		var writeResult = await auditStore.WriteAuditEntryAsync(
			scopeContext,
			auditCorrelationId,
			idAuditOperation,
			tableName,
			primaryKey,
			oldValues,
			newValues,
			affectedColumns,
			traceFrame,
			checkPermissions: true,
			cancellationToken: default);

		writeResult.ThrowIfError(scopeContext, null, true);

		await using var uow = CreateAuditUnitOfWork(scopeContext, sp);
		var auditEntries = await uow.AuditEntryRepository
			.AsQueryable(scopeContext)
			.ToListAsync(cancellationToken: default);

		Assert.That(auditEntries, Has.Count.EqualTo(1));
		var first = auditEntries.FirstOrDefault();

		Assert.That(first?.IdAuditOperation, Is.EqualTo(idAuditOperation), nameof(first.IdAuditOperation));
		Assert.That(first?.TableName, Is.EqualTo(tableName), nameof(first.TableName));
		Assert.That(first?.IdUser, Is.EqualTo(idUser), nameof(first.IdUser));
		Assert.That(first?.PrimaryKey, Is.EqualTo(primaryKey), nameof(first.PrimaryKey));
		Assert.That(first?.OldValues, Is.EqualTo(oldValues), nameof(first.OldValues));
		Assert.That(first?.NewValues, Is.EqualTo(newValues), nameof(first.NewValues));
		Assert.That(first?.AffectedColumns, Is.EqualTo(affectedColumns), nameof(first.AffectedColumns));
		Assert.That(first?.AuditCorrelationId, Is.EqualTo(auditCorrelationId), nameof(first.AuditCorrelationId));
		Assert.That(first?.TraceFrame, Is.EqualTo(traceFrame), nameof(first.TraceFrame));
		Assert.That(first?.CorrelationId, Is.EqualTo(correlationId), nameof(first.CorrelationId));
	}
}
