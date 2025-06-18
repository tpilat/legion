using Legion.ADF.Messaging.Outbox.PostgreSQL;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Microsoft.EntityFrameworkCore;

namespace Legion.ADF.Messaging.Outbox.Queries.OutboxQueue;

public class GetAllOutboxQueuesByEvents :
	QueryDefinition<
		IOutboxDbContext,
		Outbox.Model.OutboxQueue,
		List<Outbox.Model.OutboxQueue>,
		GetAllOutboxQueuesByEventsQuery>,
		IGetAllOutboxQueuesByEvents
{
	public GetAllOutboxQueuesByEvents(
		IEFConnectionProvider connectionProvider,
		GetAllOutboxQueuesByEventsQuery getAllOutboxQueuesByEvents)
		: base(connectionProvider, getAllOutboxQueuesByEvents)
	{
	}

	protected override IQueryable<Outbox.Model.OutboxQueue> GetDefaultQuery(IScopeContext scopeContext)
	{
		var context = GetContext(scopeContext);
		return context.OutboxQueue;
	}

	public override IQueryable<Outbox.Model.OutboxQueue> GetQuery(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		if (QueryRequest.ReceivedEventNamespaces == null || QueryRequest.ReceivedEventNamespaces.Count == 0)
			return Enumerable.Empty<Outbox.Model.OutboxQueue>().AsAsyncQueryable();

		return ApplyIncludesThenWhere<IMessagingAccessControlManager>(
			scopeContext,
			QueryRequest.QueryableBuilder,
			QueryRequest.CheckReadPermissions,
			QueryRequest.AsNoTracking,
			x => x.IsActive == true && QueryRequest.ReceivedEventNamespaces.Contains(x.ReceivedEventNamespace));
	}

	public override async Task<List<Outbox.Model.OutboxQueue>> ToResultAsync(
		IScopeContext scopeContext,
		CancellationToken cancellationToken = default)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return await GetQuery(scopeContext)
			.ToListAsync(cancellationToken);
	}

	public Outbox.Model.OutboxQueue? ToResult(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return GetQuery(scopeContext).FirstOrDefault();
	}
}
