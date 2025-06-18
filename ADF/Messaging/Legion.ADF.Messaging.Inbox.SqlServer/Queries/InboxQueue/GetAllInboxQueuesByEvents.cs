using Legion.ADF.Messaging.Inbox.SqlServer;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Microsoft.EntityFrameworkCore;

namespace Legion.ADF.Messaging.Inbox.Queries.InboxQueue;

public class GetAllInboxQueuesByEvents :
	QueryDefinition<
		IInboxDbContext,
		Inbox.Model.InboxQueue,
		List<Inbox.Model.InboxQueue>,
		GetAllInboxQueuesByEventsQuery>,
		IGetAllInboxQueuesByEvents
{
	public GetAllInboxQueuesByEvents(
		IEFConnectionProvider connectionProvider,
		GetAllInboxQueuesByEventsQuery getAllInboxQueuesByEvents)
		: base(connectionProvider, getAllInboxQueuesByEvents)
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

		if (QueryRequest.ReceivedEventNamespaces == null || QueryRequest.ReceivedEventNamespaces.Count == 0)
			return Enumerable.Empty<Inbox.Model.InboxQueue>().AsAsyncQueryable();

		return ApplyIncludesThenWhere<IMessagingAccessControlManager>(
			scopeContext,
			QueryRequest.QueryableBuilder,
			QueryRequest.CheckReadPermissions,
			QueryRequest.AsNoTracking,
			x => x.IsActive == true && QueryRequest.ReceivedEventNamespaces.Contains(x.ReceivedEventNamespace));
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
