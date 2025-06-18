using Legion.ADF.Messaging.Outbox.PostgreSQL;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Microsoft.EntityFrameworkCore;

namespace Legion.ADF.Messaging.Outbox.Queries.VwOutboxMessageArchive;

public class GetAllVwOutboxMessageArchivesByIdQueue :
	QueryDefinition<
		IOutboxQueryDbContext,
		Outbox.Model.VwOutboxMessageArchive,
		List<Outbox.Model.VwOutboxMessageArchive>,
		GetAllVwOutboxMessageArchivesByIdQueueQuery>,
	IGetAllVwOutboxMessageArchivesByIdQueue
{
	public GetAllVwOutboxMessageArchivesByIdQueue(
		IEFConnectionProvider connectionProvider,
		GetAllVwOutboxMessageArchivesByIdQueueQuery getAllVwOutboxMessageArchivesByIdQueue)
		: base(connectionProvider, getAllVwOutboxMessageArchivesByIdQueue)
	{
	}

	protected override IQueryable<Outbox.Model.VwOutboxMessageArchive> GetDefaultQuery(IScopeContext scopeContext)
	{
		var context = GetContext(scopeContext);
		return context.VwOutboxMessageArchive;
	}

	public override IQueryable<Outbox.Model.VwOutboxMessageArchive> GetQuery(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return ApplyIncludesThenWhere<IMessagingAccessControlManager>(
			scopeContext,
			QueryRequest.QueryableBuilder,
			QueryRequest.CheckReadPermissions,
			QueryRequest.AsNoTracking,
			im => im.IdOutboxQueue == QueryRequest.IdOutboxQueue);
	}

	public override async Task<List<Outbox.Model.VwOutboxMessageArchive>> ToResultAsync(
		IScopeContext scopeContext,
		CancellationToken cancellationToken = default)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return await GetQuery(scopeContext)
			.ToListAsync(cancellationToken);
	}

	public List<Outbox.Model.VwOutboxMessageArchive> ToResult(IScopeContext scopeContext)
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
