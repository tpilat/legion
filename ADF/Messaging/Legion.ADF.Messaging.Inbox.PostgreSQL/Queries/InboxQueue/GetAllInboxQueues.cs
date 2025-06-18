using Legion.ADF.Messaging.Inbox.PostgreSQL;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Microsoft.EntityFrameworkCore;

namespace Legion.ADF.Messaging.Inbox.Queries.InboxQueue;

public class GetAllInboxQueues :
	QueryDefinition<
		IInboxDbContext,
		Inbox.Model.InboxQueue,
		List<Inbox.Model.InboxQueue>,
		GetAllInboxQueuesQuery>,
		IGetAllInboxQueues
{
	public GetAllInboxQueues(
		IEFConnectionProvider connectionProvider,
		GetAllInboxQueuesQuery getAllInboxQueues)
		: base(connectionProvider, getAllInboxQueues)
	{
	}

	protected override IQueryable<Inbox.Model.InboxQueue> GetDefaultQuery(IScopeContext scopeContext)
	{
		var context = GetContext(scopeContext);
		return context.InboxQueue;
	}

	public override IQueryable<Inbox.Model.InboxQueue> GetQuery(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		if (QueryRequest.IncludeInactiveQueues)
		{
			return ApplyIncludesThenWhere<IMessagingAccessControlManager>(
				scopeContext,
				QueryRequest.QueryableBuilder,
				QueryRequest.CheckReadPermissions,
				QueryRequest.AsNoTracking,
				null);
		}
		else
		{
			return ApplyIncludesThenWhere<IMessagingAccessControlManager>(
				scopeContext,
				QueryRequest.QueryableBuilder,
				QueryRequest.CheckReadPermissions,
				QueryRequest.AsNoTracking,
				x => x.IsActive == true);
		}
	}

	public override async Task<List<Inbox.Model.InboxQueue>> ToResultAsync(
		IScopeContext scopeContext,
		CancellationToken cancellationToken = default)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return await GetQuery(scopeContext)
			.ToListAsync(cancellationToken);
	}

	public Inbox.Model.InboxQueue? ToResult(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return GetQuery(scopeContext).FirstOrDefault();
	}
}
