using Legion.ADF.Messaging.Inbox.SqlServer;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Microsoft.EntityFrameworkCore;

namespace Legion.ADF.Messaging.Inbox.Queries.InboxQueue;

public class GetInboxQueueByName :
	QueryDefinition<
		IInboxDbContext,
		Inbox.Model.InboxQueue,
		Inbox.Model.InboxQueue?,
		GetInboxQueueByNameQuery>,
		IGetInboxQueueByName
{
	public GetInboxQueueByName(
		IEFConnectionProvider connectionProvider,
		GetInboxQueueByNameQuery getInboxQueueByName)
		: base(connectionProvider, getInboxQueueByName)
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

		return ApplyIncludesThenWhere<IMessagingAccessControlManager>(
			scopeContext,
			QueryRequest.QueryableBuilder,
			QueryRequest.CheckReadPermissions,
			QueryRequest.AsNoTracking,
			iq => iq.Name == QueryRequest.Name);
	}

	public override async Task<Inbox.Model.InboxQueue?> ToResultAsync(
		IScopeContext scopeContext,
		CancellationToken cancellationToken = default)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return await GetQuery(scopeContext)
			.FirstOrDefaultAsync(cancellationToken);
	}

	public Inbox.Model.InboxQueue? ToResult(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return GetQuery(scopeContext).FirstOrDefault();
	}

	public async Task<Guid?> GetIdInboxQueueAsync(
		IScopeContext scopeContext,
		CancellationToken cancellationToken = default)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return await GetQuery(scopeContext)
			.Select(iq => iq.IdInboxQueue)
			.FirstOrDefaultAsync(cancellationToken);
	}

	public Guid? GetIdInboxQueue(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return GetQuery(scopeContext)
			.Select(iq => iq.IdInboxQueue)
			.FirstOrDefault();
	}

	public async Task<bool> ExistsAsync(
		IScopeContext scopeContext,
		CancellationToken cancellationToken = default)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return await GetQuery(scopeContext)
			.AnyAsync(cancellationToken);
	}

	public bool Exists(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return GetQuery(scopeContext)
			.Any();
	}
}
