using Legion;
using Legion.EntityFrameworkCore;
using Legion.Model.Audit;

namespace Legion.ADF.ESB.Orchestrations.PostgreSQL;

public abstract partial class OrchestrationsRepositoryBase : Legion.ADF.ESB.Orchestrations.IOrchestrationsRepository
{
	public IEFConnectionProvider ConnectionProvider { get; }
	public IAuditEntryStore? AuditEntryStore { get; }

	public OrchestrationsRepositoryBase(IEFConnectionProvider connectionProvider, IAuditEntryStore? auditEntryStore)
	{
		Throw.IfArgumentNull(connectionProvider);

		ConnectionProvider = connectionProvider;
		AuditEntryStore = auditEntryStore;
	}

	protected Legion.ADF.ESB.Orchestrations.PostgreSQL.IOrchestrationsDbContext GetContext(IScopeContext scopeContext)
		=> ConnectionProvider.GetOrCreateDbContext<Legion.ADF.ESB.Orchestrations.PostgreSQL.IOrchestrationsDbContext>(scopeContext, AuditEntryStore);
}
