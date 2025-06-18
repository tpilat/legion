using Legion;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Audit;

namespace Legion.ADF.Auditing.PostgreSQL;

public abstract partial class AuditQueryRepositoryBase : Legion.ADF.Auditing.IAuditQueryRepository
{
	public IEFConnectionProvider ConnectionProvider { get; }
	public IAuditEntriesManager? AuditEntriesManager { get; }

	public AuditQueryRepositoryBase(IEFConnectionProvider connectionProvider, IAuditEntriesManager? auditEntriesManager)
	{
		Throw.IfArgumentNull(connectionProvider);

		ConnectionProvider = connectionProvider;
		AuditEntriesManager = auditEntriesManager;
	}

	protected Legion.ADF.Auditing.PostgreSQL.IAuditQueryDbContext GetContext(IScopeContext scopeContext)
		=> ConnectionProvider.GetOrCreateDbContext<Legion.ADF.Auditing.PostgreSQL.IAuditQueryDbContext>(scopeContext, AuditEntriesManager);
}
