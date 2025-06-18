using Legion;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Legion.Exceptions;
using Legion.Transactions;
using Microsoft.EntityFrameworkCore;

namespace Legion.ADF.Auditing.PostgreSQL.Audit.Repositories;

public partial class AuditEntryRepository : Legion.ADF.Auditing.PostgreSQL.AuditRepositoryBase, Legion.ADF.Auditing.IAuditRepository<Legion.ADF.Auditing.Audit.AuditEntry>, Legion.ADF.Auditing.Audit.Repositories.IAuditEntryRepository
{
	public AuditEntryRepository(IEFConnectionProvider connectionProvider, Legion.EntityFrameworkCore.Audit.IAuditEntriesManager? auditEntriesManager)
		: base(connectionProvider, auditEntriesManager)
	{
	}

	private Legion.ADF.Auditing.PostgreSQL.IAuditDbContext? context;
	public IQueryable<Legion.ADF.Auditing.Audit.AuditEntry> AsQueryable(IScopeContext scopeContext)
		=> (context ??= ConnectionProvider.GetOrCreateDbContext<Legion.ADF.Auditing.PostgreSQL.IAuditDbContext>(scopeContext, AuditEntriesManager)).AuditEntry;

	public IQueryable<Legion.ADF.Auditing.Audit.AuditEntry> AsReadOnlyQueryable(IScopeContext scopeContext)
		=> AsQueryable(scopeContext).AsNoTracking();
	
	public void Add(IScopeContext scopeContext, Legion.ADF.Auditing.Audit.AuditEntry entity)
	{
		var dbContext = GetContext(scopeContext);
		Throw.IfNull(dbContext);

		dbContext.AuditEntry.Add(entity);
	}

	public void Remove(IScopeContext scopeContext, Legion.ADF.Auditing.Audit.AuditEntry entity)
	{
		var dbContext = GetContext(scopeContext);
		Throw.IfNull(dbContext);

		dbContext.AuditEntry.Remove(entity);
	}
}
