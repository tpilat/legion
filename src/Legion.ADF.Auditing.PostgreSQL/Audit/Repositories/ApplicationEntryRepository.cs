using Legion;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Legion.Exceptions;
using Legion.Transactions;
using Microsoft.EntityFrameworkCore;

namespace Legion.ADF.Auditing.PostgreSQL.Audit.Repositories;

public partial class ApplicationEntryRepository : Legion.ADF.Auditing.PostgreSQL.AuditRepositoryBase, Legion.ADF.Auditing.IAuditRepository<Legion.ADF.Auditing.Audit.ApplicationEntry>, Legion.ADF.Auditing.Audit.Repositories.IApplicationEntryRepository
{
	public ApplicationEntryRepository(IEFConnectionProvider connectionProvider, Legion.EntityFrameworkCore.Audit.IAuditEntriesManager? auditEntriesManager)
		: base(connectionProvider, auditEntriesManager)
	{
	}

	private Legion.ADF.Auditing.PostgreSQL.IAuditDbContext? context;
	public IQueryable<Legion.ADF.Auditing.Audit.ApplicationEntry> AsQueryable(IScopeContext scopeContext)
		=> (context ??= ConnectionProvider.GetOrCreateDbContext<Legion.ADF.Auditing.PostgreSQL.IAuditDbContext>(scopeContext, AuditEntriesManager)).ApplicationEntry;

	public IQueryable<Legion.ADF.Auditing.Audit.ApplicationEntry> AsReadOnlyQueryable(IScopeContext scopeContext)
		=> AsQueryable(scopeContext).AsNoTracking();
	
	public void Add(IScopeContext scopeContext, Legion.ADF.Auditing.Audit.ApplicationEntry entity)
	{
		var dbContext = GetContext(scopeContext);
		Throw.IfNull(dbContext);

		dbContext.ApplicationEntry.Add(entity);
	}

	public void Remove(IScopeContext scopeContext, Legion.ADF.Auditing.Audit.ApplicationEntry entity)
	{
		var dbContext = GetContext(scopeContext);
		Throw.IfNull(dbContext);

		dbContext.ApplicationEntry.Remove(entity);
	}
}
