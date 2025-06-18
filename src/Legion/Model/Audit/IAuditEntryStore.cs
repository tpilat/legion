using System.Runtime.CompilerServices;

namespace Legion.Model.Audit;

public interface IAuditEntryStore: IDisposable, IAsyncDisposable
{
	void AddAuditEntries(IScopeContext scopeContext, List<IAuditEntry> auditEntries);

	int Save(
		IScopeContext scopeContext,
		bool autoCommit,
		bool acceptAllChangesOnSuccess,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0);

	Task<int> SaveAsync(
		IScopeContext scopeContext,
		bool autoCommit,
		bool acceptAllChangesOnSuccess,
		CancellationToken cancellationToken = default,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0);
}
