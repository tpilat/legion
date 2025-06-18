using Legion;
using Legion.EntityFrameworkCore;
using Legion.Model.Audit;
using Legion.EntityFrameworkCore.Queries;
using Legion.Exceptions;
using Legion.Transactions;
using Microsoft.Extensions.DependencyInjection;

namespace Legion.ADF.ESB.Orchestrations.PostgreSQL;

public partial class OrchestrationsQueryUnitOfWork : Legion.ADF.ESB.Orchestrations.IOrchestrationsQueryUnitOfWork, Legion.EntityFrameworkCore.Model.Repositories.IDbQueryUnitOfWork, Legion.Model.Repositories.IQueryUnitOfWork
{
	public IEFConnectionProvider ConnectionProvider { get; }
	public IAuditEntryStore? AuditEntryStore { get; }

	public OrchestrationsQueryUnitOfWork(IEFConnectionProvider connectionProvider, IAuditEntryStore? auditEntryStore)
	{
		Throw.IfArgumentNull(connectionProvider);

		ConnectionProvider = connectionProvider;
		AuditEntryStore = auditEntryStore;
	}

	public OrchestrationsQueryUnitOfWork(Legion.EntityFrameworkCore.Model.Repositories.IDbUnitOfWork dbUnitOfWork)
	{
		Throw.IfArgumentNull(dbUnitOfWork);

		ConnectionProvider = dbUnitOfWork.ConnectionProvider;
		AuditEntryStore = dbUnitOfWork.AuditEntryStore;
	}

	public OrchestrationsQueryUnitOfWork(Legion.EntityFrameworkCore.Model.Repositories.IDbQueryUnitOfWork dbQueryUnitOfWork)
	{
		Throw.IfArgumentNull(dbQueryUnitOfWork);

		ConnectionProvider = dbQueryUnitOfWork.ConnectionProvider;
		AuditEntryStore = dbQueryUnitOfWork.AuditEntryStore;
	}

	public OrchestrationsQueryUnitOfWork(IServiceProvider serviceProvider, string connectionStirng, IAuditEntryStore? auditEntryStore)
	{
		Throw.IfArgumentNull(serviceProvider);
		Throw.IfArgumentNullOrWhiteSpace(connectionStirng);

		var connectionProviderFactory = serviceProvider.GetRequiredService<IEFConnectionProviderFactory>();
		ConnectionProvider = connectionProviderFactory.CreateWithoutTransaction(serviceProvider, connectionStirng);
		AuditEntryStore = auditEntryStore;
	}

	protected Legion.ADF.ESB.Orchestrations.PostgreSQL.IOrchestrationsQueryDbContext GetContext(IScopeContext scopeContext)
		=> ConnectionProvider.GetOrCreateDbContext<Legion.ADF.ESB.Orchestrations.PostgreSQL.IOrchestrationsQueryDbContext>(scopeContext, AuditEntryStore);


	private Legion.ADF.ESB.Orchestrations.Model.Repositories.IVwOrchestrationRepository vwOrchestration;
	public Legion.ADF.ESB.Orchestrations.Model.Repositories.IVwOrchestrationRepository VwOrchestrationRepository
		=> vwOrchestration ??= new Legion.ADF.ESB.Orchestrations.PostgreSQL.Model.Repositories.VwOrchestrationRepository(ConnectionProvider, AuditEntryStore);
}
