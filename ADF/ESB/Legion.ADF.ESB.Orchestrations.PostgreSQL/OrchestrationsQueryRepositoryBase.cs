using Legion;
using Legion.EntityFrameworkCore;
using Legion.Model.Audit;

namespace Legion.ADF.ESB.Orchestrations.PostgreSQL;

public abstract partial class OrchestrationsQueryRepositoryBase : Legion.ADF.ESB.Orchestrations.IOrchestrationsQueryRepository
{
	public IEFConnectionProvider ConnectionProvider { get; }
	public IAuditEntryStore? AuditEntryStore { get; }

	public OrchestrationsQueryRepositoryBase(IEFConnectionProvider connectionProvider, IAuditEntryStore? auditEntryStore)
	{
		Throw.IfArgumentNull(connectionProvider);

		ConnectionProvider = connectionProvider;
		AuditEntryStore = auditEntryStore;
	}

	protected Legion.ADF.ESB.Orchestrations.PostgreSQL.IOrchestrationsQueryDbContext GetContext(IScopeContext scopeContext)
		=> ConnectionProvider.GetOrCreateDbContext<Legion.ADF.ESB.Orchestrations.PostgreSQL.IOrchestrationsQueryDbContext>(scopeContext, AuditEntryStore);
}
