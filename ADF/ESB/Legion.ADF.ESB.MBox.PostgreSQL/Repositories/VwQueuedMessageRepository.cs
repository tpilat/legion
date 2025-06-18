using Legion;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Legion.Exceptions;
using Legion.Transactions;
using Microsoft.EntityFrameworkCore;

namespace Legion.ADF.ESB.MBox.PostgreSQL.Model.Repositories;

public partial class VwQueuedMessageRepository : Legion.ADF.ESB.MBox.PostgreSQL.MBoxQueryRepositoryBase, Legion.ADF.ESB.MBox.IMBoxQueryRepository<Legion.ADF.ESB.MBox.Model.VwQueuedMessage>, Legion.ADF.ESB.MBox.Model.Repositories.IVwQueuedMessageRepository
{
	public VwQueuedMessageRepository(IEFConnectionProvider connectionProvider, Legion.Model.Audit.IAuditEntryStore? auditEntryStore)
		: base(connectionProvider, auditEntryStore)
	{
	}

	private Legion.ADF.ESB.MBox.PostgreSQL.IMBoxQueryDbContext? context;
	public IQueryable<Legion.ADF.ESB.MBox.Model.VwQueuedMessage> AsQueryable(IScopeContext scopeContext)
		=> (context ??= ConnectionProvider.GetOrCreateDbContext<Legion.ADF.ESB.MBox.PostgreSQL.IMBoxQueryDbContext>(scopeContext, AuditEntryStore)).VwQueuedMessage;

	public IQueryable<Legion.ADF.ESB.MBox.Model.VwQueuedMessage> AsReadOnlyQueryable(IScopeContext scopeContext)
		=> AsQueryable(scopeContext).AsNoTracking();
	}
