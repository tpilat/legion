using Legion;
using Legion.EntityFrameworkCore;
using Legion.Model.Audit;

namespace Legion.ADF.ESB.MBox.PostgreSQL;

public abstract partial class MBoxRepositoryBase : Legion.ADF.ESB.MBox.IMBoxRepository
{
	public IEFConnectionProvider ConnectionProvider { get; }
	public IAuditEntryStore? AuditEntryStore { get; }

	public MBoxRepositoryBase(IEFConnectionProvider connectionProvider, IAuditEntryStore? auditEntryStore)
	{
		Throw.IfArgumentNull(connectionProvider);

		ConnectionProvider = connectionProvider;
		AuditEntryStore = auditEntryStore;
	}

	protected Legion.ADF.ESB.MBox.PostgreSQL.IMBoxDbContext GetContext(IScopeContext scopeContext)
		=> ConnectionProvider.GetOrCreateDbContext<Legion.ADF.ESB.MBox.PostgreSQL.IMBoxDbContext>(scopeContext, AuditEntryStore);
}
