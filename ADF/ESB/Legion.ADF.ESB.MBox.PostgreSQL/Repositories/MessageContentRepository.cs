using Legion;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Legion.Exceptions;
using Legion.Transactions;
using Microsoft.EntityFrameworkCore;

namespace Legion.ADF.ESB.MBox.PostgreSQL.Model.Repositories;

public partial class MessageContentRepository : Legion.ADF.ESB.MBox.PostgreSQL.MBoxRepositoryBase, Legion.ADF.ESB.MBox.IMBoxRepository<Legion.ADF.ESB.MBox.Model.MessageContent>, Legion.ADF.ESB.MBox.Model.Repositories.IMessageContentRepository
{
	public MessageContentRepository(IEFConnectionProvider connectionProvider, Legion.Model.Audit.IAuditEntryStore? auditEntryStore)
		: base(connectionProvider, auditEntryStore)
	{
	}

	private Legion.ADF.ESB.MBox.PostgreSQL.IMBoxDbContext? context;
	public IQueryable<Legion.ADF.ESB.MBox.Model.MessageContent> AsQueryable(IScopeContext scopeContext)
		=> (context ??= ConnectionProvider.GetOrCreateDbContext<Legion.ADF.ESB.MBox.PostgreSQL.IMBoxDbContext>(scopeContext, AuditEntryStore)).MessageContent;

	public IQueryable<Legion.ADF.ESB.MBox.Model.MessageContent> AsReadOnlyQueryable(IScopeContext scopeContext)
		=> AsQueryable(scopeContext).AsNoTracking();
	
	public void Add(IScopeContext scopeContext, Legion.ADF.ESB.MBox.Model.MessageContent entity)
	{
		var dbContext = GetContext(scopeContext);
		Throw.IfNull(dbContext);

		dbContext.MessageContent.Add(entity);
	}

	public void Remove(IScopeContext scopeContext, Legion.ADF.ESB.MBox.Model.MessageContent entity)
	{
		var dbContext = GetContext(scopeContext);
		Throw.IfNull(dbContext);

		dbContext.MessageContent.Remove(entity);
	}
}
