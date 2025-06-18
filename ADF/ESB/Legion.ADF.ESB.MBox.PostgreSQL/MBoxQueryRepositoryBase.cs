using Legion;
using Legion.EntityFrameworkCore;
using Legion.Model.Audit;

namespace Legion.ADF.ESB.MBox.PostgreSQL;

public abstract partial class MBoxQueryRepositoryBase : Legion.ADF.ESB.MBox.IMBoxQueryRepository
{
	public IEFConnectionProvider ConnectionProvider { get; }
	public IAuditEntryStore? AuditEntryStore { get; }

	public MBoxQueryRepositoryBase(IEFConnectionProvider connectionProvider, IAuditEntryStore? auditEntryStore)
	{
		Throw.IfArgumentNull(connectionProvider);

		ConnectionProvider = connectionProvider;
		AuditEntryStore = auditEntryStore;
	}

	protected Legion.ADF.ESB.MBox.PostgreSQL.IMBoxQueryDbContext GetContext(IScopeContext scopeContext)
		=> ConnectionProvider.GetOrCreateDbContext<Legion.ADF.ESB.MBox.PostgreSQL.IMBoxQueryDbContext>(scopeContext, AuditEntryStore);
}
