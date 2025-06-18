using Legion.ADF.Messaging.Outbox.PostgreSQL;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Microsoft.EntityFrameworkCore;

namespace Legion.ADF.Messaging.Outbox.Queries.VwOutboxQueueMessage;

public class GetAllOutboxQueues :
	QueryDefinition<
		IOutboxQueryDbContext,
		Outbox.Model.VwOutboxQueueMessages,
		List<Outbox.Model.VwOutboxQueueMessages>,
		GetAllOutboxQueuesQuery>,
		IGetAllOutboxQueues
{
	public GetAllOutboxQueues(
		IEFConnectionProvider connectionProvider,
		GetAllOutboxQueuesQuery getAllOutboxQueues)
		: base(connectionProvider, getAllOutboxQueues)
	{
	}

	protected override IQueryable<Outbox.Model.VwOutboxQueueMessages> GetDefaultQuery(IScopeContext scopeContext)
	{
		var context = GetContext(scopeContext);
		return context.VwOutboxQueueMessages;
	}

	public override IQueryable<Outbox.Model.VwOutboxQueueMessages> GetQuery(IScopeContext scopeContext)
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

	public override async Task<List<Outbox.Model.VwOutboxQueueMessages>> ToResultAsync(
		IScopeContext scopeContext,
		CancellationToken cancellationToken = default)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return await GetQuery(scopeContext)
			.ToListAsync(cancellationToken);
	}

	public List<Outbox.Model.VwOutboxQueueMessages> ToResult(IScopeContext scopeContext)
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
