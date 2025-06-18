using Legion;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Legion.Exceptions;
using Legion.Transactions;
using Microsoft.EntityFrameworkCore;

namespace Legion.ADF.ESB.MBox.PostgreSQL.Model.Repositories;

public partial class MessageProcessingStatusRepository : Legion.ADF.ESB.MBox.PostgreSQL.MBoxRepositoryBase, Legion.ADF.ESB.MBox.IMBoxRepository<Legion.ADF.ESB.MBox.Model.MessageProcessingStatus>, Legion.ADF.ESB.MBox.Model.Repositories.IMessageProcessingStatusRepository
{
	public MessageProcessingStatusRepository(IEFConnectionProvider connectionProvider, Legion.Model.Audit.IAuditEntryStore? auditEntryStore)
		: base(connectionProvider, auditEntryStore)
	{
	}

	private Legion.ADF.ESB.MBox.PostgreSQL.IMBoxDbContext? context;
	public IQueryable<Legion.ADF.ESB.MBox.Model.MessageProcessingStatus> AsQueryable(IScopeContext scopeContext)
		=> (context ??= ConnectionProvider.GetOrCreateDbContext<Legion.ADF.ESB.MBox.PostgreSQL.IMBoxDbContext>(scopeContext, AuditEntryStore)).MessageProcessingStatus;

	public IQueryable<Legion.ADF.ESB.MBox.Model.MessageProcessingStatus> AsReadOnlyQueryable(IScopeContext scopeContext)
		=> AsQueryable(scopeContext).AsNoTracking();
	
	public void Add(IScopeContext scopeContext, Legion.ADF.ESB.MBox.Model.MessageProcessingStatus entity)
	{
		var dbContext = GetContext(scopeContext);
		Throw.IfNull(dbContext);

		dbContext.MessageProcessingStatus.Add(entity);
	}

	public void Remove(IScopeContext scopeContext, Legion.ADF.ESB.MBox.Model.MessageProcessingStatus entity)
	{
		var dbContext = GetContext(scopeContext);
		Throw.IfNull(dbContext);

		dbContext.MessageProcessingStatus.Remove(entity);
	}
}
