using Legion.ADF.Messaging.Outbox.SqlServer;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Microsoft.EntityFrameworkCore;

namespace Legion.ADF.Messaging.Outbox.Queries.VwOutboxMessage;

public class GetAllVwOutboxMessagesByIdQueue :
	QueryDefinition<
		IOutboxQueryDbContext,
		Outbox.Model.VwOutboxMessage,
		List<Outbox.Model.VwOutboxMessage>,
		GetAllVwOutboxMessagesByIdQueueQuery>,
	IGetAllVwOutboxMessagesByIdQueue
{
	public GetAllVwOutboxMessagesByIdQueue(
		IEFConnectionProvider connectionProvider,
		GetAllVwOutboxMessagesByIdQueueQuery getAllVwOutboxMessagesByIdQueue)
		: base(connectionProvider, getAllVwOutboxMessagesByIdQueue)
	{
	}

	protected override IQueryable<Outbox.Model.VwOutboxMessage> GetDefaultQuery(IScopeContext scopeContext)
	{
		var context = GetContext(scopeContext);
		return context.VwOutboxMessage;
	}

	public override IQueryable<Outbox.Model.VwOutboxMessage> GetQuery(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return ApplyIncludesThenWhere<IMessagingAccessControlManager>(
			scopeContext,
			QueryRequest.QueryableBuilder,
			QueryRequest.CheckReadPermissions,
			QueryRequest.AsNoTracking,
			im => im.IdOutboxQueue == QueryRequest.IdOutboxQueue);
	}

	public override async Task<List<Outbox.Model.VwOutboxMessage>> ToResultAsync(
		IScopeContext scopeContext,
		CancellationToken cancellationToken = default)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return await GetQuery(scopeContext)
			.ToListAsync(cancellationToken);
	}

	public List<Outbox.Model.VwOutboxMessage> ToResult(IScopeContext scopeContext)
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
