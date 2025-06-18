using Legion.EntityFrameworkCore.Audit;
using Legion.Model.Audit;
using System.Runtime.CompilerServices;

namespace Legion.ADF.Auditing.PostgreSQL;

public partial class AuditUnitOfWork : Legion.ADF.Auditing.IAuditUnitOfWork, Legion.EntityFrameworkCore.Model.Repositories.IDbUnitOfWork, Legion.Model.Repositories.IUnitOfWork, IAuditEntriesManager
{
	[Obsolete("Use SaveAsync() method instead.", DiagnosticId = "LADF_AudUoW_Save")]
	public virtual int Save(
		IScopeContext scopeContext,
		bool autoCommit,
		bool acceptAllChangesOnSuccess,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0)
	{
		var dbContext = GetContext(scopeContext);
		Throw.IfNull(dbContext);
		var result = dbContext.Save(scopeContext.CreateNew(memberName, sourceFilePath, sourceLineNumber), acceptAllChangesOnSuccess);

		if (autoCommit)
		{
			var commitResult = ConnectionProvider.TransactionsController.CommitAll(scopeContext);
			commitResult.ThrowIfError(scopeContext, null/*//TODO*/, true);
		}

		return result;
	}

	public void AddAuditEntries(IScopeContext scopeContext, List<IAuditEntry> auditEntries)
	{
		if (0 < auditEntries?.Count)
		{
			foreach (var ae in auditEntries)
			{
				var createResult = Auditing.Audit.AuditEntry.Create(scopeContext, ae);
				//TODO: uncomment createResult.ThrowIfErrorOrNullData(scopeContext);

				AuditEntryRepository.Add(scopeContext, createResult.Data!);
			}
		}
	}
}
