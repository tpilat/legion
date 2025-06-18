using Legion;
using Legion.EntityFrameworkCore;
using Legion.Model.Audit;
using Legion.EntityFrameworkCore.Queries;
using Legion.Exceptions;
using Legion.Transactions;
using Microsoft.Extensions.DependencyInjection;

namespace Legion.ADF.ESB.MBox.PostgreSQL;

public partial class MBoxQueryUnitOfWork : Legion.ADF.ESB.MBox.IMBoxQueryUnitOfWork, Legion.EntityFrameworkCore.Model.Repositories.IDbQueryUnitOfWork, Legion.Model.Repositories.IQueryUnitOfWork
{
	public IEFConnectionProvider ConnectionProvider { get; }
	public IAuditEntryStore? AuditEntryStore { get; }

	public MBoxQueryUnitOfWork(IEFConnectionProvider connectionProvider, IAuditEntryStore? auditEntryStore)
	{
		Throw.IfArgumentNull(connectionProvider);

		ConnectionProvider = connectionProvider;
		AuditEntryStore = auditEntryStore;
	}

	public MBoxQueryUnitOfWork(Legion.EntityFrameworkCore.Model.Repositories.IDbUnitOfWork dbUnitOfWork)
	{
		Throw.IfArgumentNull(dbUnitOfWork);

		ConnectionProvider = dbUnitOfWork.ConnectionProvider;
		AuditEntryStore = dbUnitOfWork.AuditEntryStore;
	}

	public MBoxQueryUnitOfWork(Legion.EntityFrameworkCore.Model.Repositories.IDbQueryUnitOfWork dbQueryUnitOfWork)
	{
		Throw.IfArgumentNull(dbQueryUnitOfWork);

		ConnectionProvider = dbQueryUnitOfWork.ConnectionProvider;
		AuditEntryStore = dbQueryUnitOfWork.AuditEntryStore;
	}

	public MBoxQueryUnitOfWork(IServiceProvider serviceProvider, string connectionStirng, IAuditEntryStore? auditEntryStore)
	{
		Throw.IfArgumentNull(serviceProvider);
		Throw.IfArgumentNullOrWhiteSpace(connectionStirng);

		var connectionProviderFactory = serviceProvider.GetRequiredService<IEFConnectionProviderFactory>();
		ConnectionProvider = connectionProviderFactory.CreateWithoutTransaction(serviceProvider, connectionStirng);
		AuditEntryStore = auditEntryStore;
	}

	protected Legion.ADF.ESB.MBox.PostgreSQL.IMBoxQueryDbContext GetContext(IScopeContext scopeContext)
		=> ConnectionProvider.GetOrCreateDbContext<Legion.ADF.ESB.MBox.PostgreSQL.IMBoxQueryDbContext>(scopeContext, AuditEntryStore);


	private Legion.ADF.ESB.MBox.Model.Repositories.IVwQueuedMessageRepository vwQueuedMessage;
	public Legion.ADF.ESB.MBox.Model.Repositories.IVwQueuedMessageRepository VwQueuedMessageRepository
		=> vwQueuedMessage ??= new Legion.ADF.ESB.MBox.PostgreSQL.Model.Repositories.VwQueuedMessageRepository(ConnectionProvider, AuditEntryStore);
}
