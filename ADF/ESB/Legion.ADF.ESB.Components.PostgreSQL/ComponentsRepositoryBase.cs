using Legion;
using Legion.EntityFrameworkCore;
using Legion.Model.Audit;

namespace Legion.ADF.ESB.Components.PostgreSQL;

public abstract partial class ComponentsRepositoryBase : Legion.ADF.ESB.Components.IComponentsRepository
{
	public IEFConnectionProvider ConnectionProvider { get; }
	public IAuditEntryStore? AuditEntryStore { get; }

	public ComponentsRepositoryBase(IEFConnectionProvider connectionProvider, IAuditEntryStore? auditEntryStore)
	{
		Throw.IfArgumentNull(connectionProvider);

		ConnectionProvider = connectionProvider;
		AuditEntryStore = auditEntryStore;
	}

	protected Legion.ADF.ESB.Components.PostgreSQL.IComponentsDbContext GetContext(IScopeContext scopeContext)
		=> ConnectionProvider.GetOrCreateDbContext<Legion.ADF.ESB.Components.PostgreSQL.IComponentsDbContext>(scopeContext, AuditEntryStore);
}
