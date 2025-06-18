using Legion.ADF.Messaging.Outbox.SqlServer;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Microsoft.EntityFrameworkCore;

namespace Legion.ADF.Messaging.Outbox.Queries.VwOutboxQueue;

public class GetVwOutboxQueueById :
	QueryDefinition<
		IOutboxQueryDbContext,
		Outbox.Model.VwOutboxQueue,
		Outbox.Model.VwOutboxQueue?,
		GetVwOutboxQueueByIdQuery>,
	IGetVwOutboxQueueById
{
	public GetVwOutboxQueueById(
		IEFConnectionProvider connectionProvider,
		GetVwOutboxQueueByIdQuery getVwOutboxQueueById)
		: base(connectionProvider, getVwOutboxQueueById)
	{
	}

	protected override IQueryable<Outbox.Model.VwOutboxQueue> GetDefaultQuery(IScopeContext scopeContext)
	{
		var context = GetContext(scopeContext);
		return context.VwOutboxQueue;
	}

	public override IQueryable<Outbox.Model.VwOutboxQueue> GetQuery(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return ApplyIncludesThenWhere<IMessagingAccessControlManager>(
			scopeContext,
			QueryRequest.QueryableBuilder,
			QueryRequest.CheckReadPermissions,
			QueryRequest.AsNoTracking,
			im => im.IdOutboxQueue == QueryRequest.IdOutboxQueue);
	}

	public override async Task<Outbox.Model.VwOutboxQueue?> ToResultAsync(
		IScopeContext scopeContext,
		CancellationToken cancellationToken = default)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return await GetQuery(scopeContext)
			.FirstOrDefaultAsync(cancellationToken);
	}

	public Outbox.Model.VwOutboxQueue? ToResult(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return GetQuery(scopeContext).FirstOrDefault();
	}
}
