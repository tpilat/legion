using Legion.ADF.Messaging.Inbox.PostgreSQL;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Microsoft.EntityFrameworkCore;

namespace Legion.ADF.Messaging.Inbox.Queries.VwInboxQueue;

public class GetVwInboxQueueById :
	QueryDefinition<
		IInboxQueryDbContext,
		Inbox.Model.VwInboxQueue,
		Inbox.Model.VwInboxQueue?,
		GetVwInboxQueueByIdQuery>,
	IGetVwInboxQueueById
{
	public GetVwInboxQueueById(
		IEFConnectionProvider connectionProvider,
		GetVwInboxQueueByIdQuery getVwInboxQueueById)
		: base(connectionProvider, getVwInboxQueueById)
	{
	}

	protected override IQueryable<Inbox.Model.VwInboxQueue> GetDefaultQuery(IScopeContext scopeContext)
	{
		var context = GetContext(scopeContext);
		return context.VwInboxQueue;
	}

	public override IQueryable<Inbox.Model.VwInboxQueue> GetQuery(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return ApplyIncludesThenWhere<IMessagingAccessControlManager>(
			scopeContext,
			QueryRequest.QueryableBuilder,
			QueryRequest.CheckReadPermissions,
			QueryRequest.AsNoTracking,
			im => im.IdInboxQueue == QueryRequest.IdInboxQueue);
	}

	public override async Task<Inbox.Model.VwInboxQueue?> ToResultAsync(
		IScopeContext scopeContext,
		CancellationToken cancellationToken = default)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return await GetQuery(scopeContext)
			.FirstOrDefaultAsync(cancellationToken);
	}

	public Inbox.Model.VwInboxQueue? ToResult(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return GetQuery(scopeContext).FirstOrDefault();
	}
}
