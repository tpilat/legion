using Legion;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Audit;

namespace Legion.ADF.Auditing.PostgreSQL;

public abstract partial class AuditRepositoryBase : Legion.ADF.Auditing.IAuditRepository
{
	public IEFConnectionProvider ConnectionProvider { get; }
	public IAuditEntriesManager? AuditEntriesManager { get; }

	public AuditRepositoryBase(IEFConnectionProvider connectionProvider, IAuditEntriesManager? auditEntriesManager)
	{
		Throw.IfArgumentNull(connectionProvider);

		ConnectionProvider = connectionProvider;
		AuditEntriesManager = auditEntriesManager;
	}

	protected Legion.ADF.Auditing.PostgreSQL.IAuditDbContext GetContext(IScopeContext scopeContext)
		=> ConnectionProvider.GetOrCreateDbContext<Legion.ADF.Auditing.PostgreSQL.IAuditDbContext>(scopeContext, AuditEntriesManager);
}
