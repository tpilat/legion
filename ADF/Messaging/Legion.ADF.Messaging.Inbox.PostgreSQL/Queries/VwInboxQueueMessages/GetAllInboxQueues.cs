using Legion.ADF.Messaging.Inbox.PostgreSQL;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Microsoft.EntityFrameworkCore;

namespace Legion.ADF.Messaging.Inbox.Queries.VwInboxQueueMessage;

public class GetAllInboxQueues :
	QueryDefinition<
		IInboxQueryDbContext,
		Inbox.Model.VwInboxQueueMessages,
		List<Inbox.Model.VwInboxQueueMessages>,
		GetAllInboxQueuesQuery>,
		IGetAllInboxQueues
{
	public GetAllInboxQueues(
		IEFConnectionProvider connectionProvider,
		GetAllInboxQueuesQuery getAllInboxQueues)
		: base(connectionProvider, getAllInboxQueues)
	{
	}

	protected override IQueryable<Inbox.Model.VwInboxQueueMessages> GetDefaultQuery(IScopeContext scopeContext)
	{
		var context = GetContext(scopeContext);
		return context.VwInboxQueueMessages;
	}

	public override IQueryable<Inbox.Model.VwInboxQueueMessages> GetQuery(IScopeContext scopeContext)
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

	public override async Task<List<Inbox.Model.VwInboxQueueMessages>> ToResultAsync(
		IScopeContext scopeContext,
		CancellationToken cancellationToken = default)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return await GetQuery(scopeContext)
			.ToListAsync(cancellationToken);
	}

	public List<Inbox.Model.VwInboxQueueMessages> ToResult(IScopeContext scopeContext)
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
