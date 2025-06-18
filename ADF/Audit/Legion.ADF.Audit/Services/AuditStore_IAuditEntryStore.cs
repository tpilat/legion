using Legion.Model.Audit;
using System.Runtime.CompilerServices;

namespace Legion.ADF.Audit.Services;

public partial class AuditStore : IAuditEntryStore, IDisposable, IAsyncDisposable
{
	public void AddAuditEntries(IScopeContext scopeContext, List<IAuditEntry> auditEntries)
	{
		var createResult = Model.AuditEntry.CreateRange(scopeContext, auditEntries);
		createResult.ThrowIfErrorOrNullData(scopeContext, null, true);

		UoW.AuditEntryRepository
			.AddRange(scopeContext, createResult.Data!);
	}

	public int Save(
		IScopeContext scopeContext,
		bool autoCommit,
		bool acceptAllChangesOnSuccess,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0)
	{
		var saveResult = SaveInternal(scopeContext, force: false);
		saveResult.ThrowIfError(scopeContext, null, true);
		return saveResult.Data;
	}

	public async Task<int> SaveAsync(
		IScopeContext scopeContext,
		bool autoCommit,
		bool acceptAllChangesOnSuccess,
		CancellationToken cancellationToken = default,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0)
	{
		var saveResult = await SaveInternalAsync(scopeContext, force: false, cancellationToken);
		saveResult.ThrowIfError(scopeContext, null, true);
		return saveResult.Data;
	}
}
