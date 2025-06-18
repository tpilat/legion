using Legion.ADF.Messaging.Inbox.PostgreSQL;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Microsoft.EntityFrameworkCore;

namespace Legion.ADF.Messaging.Inbox.Queries.VwInboxMessageArchive;

public class GetAllVwInboxMessageArchivesByIdQueue :
	QueryDefinition<
		IInboxQueryDbContext,
		Inbox.Model.VwInboxMessageArchive,
		List<Inbox.Model.VwInboxMessageArchive>,
		GetAllVwInboxMessageArchivesByIdQueueQuery>,
	IGetAllVwInboxMessageArchivesByIdQueue
{
	public GetAllVwInboxMessageArchivesByIdQueue(
		IEFConnectionProvider connectionProvider,
		GetAllVwInboxMessageArchivesByIdQueueQuery getAllVwInboxMessageArchivesByIdQueue)
		: base(connectionProvider, getAllVwInboxMessageArchivesByIdQueue)
	{
	}

	protected override IQueryable<Inbox.Model.VwInboxMessageArchive> GetDefaultQuery(IScopeContext scopeContext)
	{
		var context = GetContext(scopeContext);
		return context.VwInboxMessageArchive;
	}

	public override IQueryable<Inbox.Model.VwInboxMessageArchive> GetQuery(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return ApplyIncludesThenWhere<IMessagingAccessControlManager>(
			scopeContext,
			QueryRequest.QueryableBuilder,
			QueryRequest.CheckReadPermissions,
			QueryRequest.AsNoTracking,
			im => im.IdInboxQueue == QueryRequest.IdInboxQueue);
	}

	public override async Task<List<Inbox.Model.VwInboxMessageArchive>> ToResultAsync(
		IScopeContext scopeContext,
		CancellationToken cancellationToken = default)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return await GetQuery(scopeContext)
			.ToListAsync(cancellationToken);
	}

	public List<Inbox.Model.VwInboxMessageArchive> ToResult(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return GetQuery(scopeContext).ToList();
	}

	public async Task<long> TotalCountAsync(
		IScopeContext scopeContext,
		CancellationToken cancellationToken = default)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return await GetQuery(scopeContext)
			.CountAsync(cancellationToken);
	}

	public long TotalCount(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return GetQuery(scopeContext).Count();
	}
}
