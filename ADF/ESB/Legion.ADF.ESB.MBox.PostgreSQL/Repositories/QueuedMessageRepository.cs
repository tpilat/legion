using Legion;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Legion.Exceptions;
using Legion.Transactions;
using Microsoft.EntityFrameworkCore;

namespace Legion.ADF.ESB.MBox.PostgreSQL.Model.Repositories;

public partial class QueuedMessageRepository : Legion.ADF.ESB.MBox.PostgreSQL.MBoxRepositoryBase, Legion.ADF.ESB.MBox.IMBoxRepository<Legion.ADF.ESB.MBox.Model.QueuedMessage>, Legion.ADF.ESB.MBox.Model.Repositories.IQueuedMessageRepository
{
	public QueuedMessageRepository(IEFConnectionProvider connectionProvider, Legion.Model.Audit.IAuditEntryStore? auditEntryStore)
		: base(connectionProvider, auditEntryStore)
	{
	}

	private Legion.ADF.ESB.MBox.PostgreSQL.IMBoxDbContext? context;
	public IQueryable<Legion.ADF.ESB.MBox.Model.QueuedMessage> AsQueryable(IScopeContext scopeContext)
		=> (context ??= ConnectionProvider.GetOrCreateDbContext<Legion.ADF.ESB.MBox.PostgreSQL.IMBoxDbContext>(scopeContext, AuditEntryStore)).QueuedMessage;

	public IQueryable<Legion.ADF.ESB.MBox.Model.QueuedMessage> AsReadOnlyQueryable(IScopeContext scopeContext)
		=> AsQueryable(scopeContext).AsNoTracking();
	
	public void Add(IScopeContext scopeContext, Legion.ADF.ESB.MBox.Model.QueuedMessage entity)
	{
		var dbContext = GetContext(scopeContext);
		Throw.IfNull(dbContext);

		dbContext.QueuedMessage.Add(entity);
	}

	public void Remove(IScopeContext scopeContext, Legion.ADF.ESB.MBox.Model.QueuedMessage entity)
	{
		var dbContext = GetContext(scopeContext);
		Throw.IfNull(dbContext);

		dbContext.QueuedMessage.Remove(entity);
	}
}
