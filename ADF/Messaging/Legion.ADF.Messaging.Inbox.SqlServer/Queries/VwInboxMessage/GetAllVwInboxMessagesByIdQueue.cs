using Legion.ADF.Messaging.Inbox.SqlServer;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Microsoft.EntityFrameworkCore;

namespace Legion.ADF.Messaging.Inbox.Queries.VwInboxMessage;

public class GetAllVwInboxMessagesByIdQueue :
	QueryDefinition<
		IInboxQueryDbContext,
		Inbox.Model.VwInboxMessage,
		List<Inbox.Model.VwInboxMessage>,
		GetAllVwInboxMessagesByIdQueueQuery>,
	IGetAllVwInboxMessagesByIdQueue
{
	public GetAllVwInboxMessagesByIdQueue(
		IEFConnectionProvider connectionProvider,
		GetAllVwInboxMessagesByIdQueueQuery getAllVwInboxMessagesByIdQueue)
		: base(connectionProvider, getAllVwInboxMessagesByIdQueue)
	{
	}

	protected override IQueryable<Inbox.Model.VwInboxMessage> GetDefaultQuery(IScopeContext scopeContext)
	{
		var context = GetContext(scopeContext);
		return context.VwInboxMessage;
	}

	public override IQueryable<Inbox.Model.VwInboxMessage> GetQuery(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return ApplyIncludesThenWhere<IMessagingAccessControlManager>(
			scopeContext,
			QueryRequest.QueryableBuilder,
			QueryRequest.CheckReadPermissions,
			QueryRequest.AsNoTracking,
			im => im.IdInboxQueue == QueryRequest.IdInboxQueue);
	}

	public override async Task<List<Inbox.Model.VwInboxMessage>> ToResultAsync(
		IScopeContext scopeContext,
		CancellationToken cancellationToken = default)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return await GetQuery(scopeContext)
			.ToListAsync(cancellationToken);
	}

	public List<Inbox.Model.VwInboxMessage> ToResult(IScopeContext scopeContext)
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
