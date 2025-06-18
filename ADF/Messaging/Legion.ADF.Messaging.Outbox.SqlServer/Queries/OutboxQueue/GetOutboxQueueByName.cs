using Legion.ADF.Messaging.Outbox.SqlServer;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Microsoft.EntityFrameworkCore;

namespace Legion.ADF.Messaging.Outbox.Queries.OutboxQueue;

public class GetOutboxQueueByName :
	QueryDefinition<
		IOutboxDbContext,
		Outbox.Model.OutboxQueue,
		Outbox.Model.OutboxQueue?,
		GetOutboxQueueByNameQuery>,
		IGetOutboxQueueByName
{
	public GetOutboxQueueByName(
		IEFConnectionProvider connectionProvider,
		GetOutboxQueueByNameQuery getOutboxQueueByName)
		: base(connectionProvider, getOutboxQueueByName)
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

		return ApplyIncludesThenWhere<IMessagingAccessControlManager>(
			scopeContext,
			QueryRequest.QueryableBuilder,
			QueryRequest.CheckReadPermissions,
			QueryRequest.AsNoTracking,
			iq => iq.Name == QueryRequest.Name);
	}

	public override async Task<Outbox.Model.OutboxQueue?> ToResultAsync(
		IScopeContext scopeContext,
		CancellationToken cancellationToken = default)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return await GetQuery(scopeContext)
			.FirstOrDefaultAsync(cancellationToken);
	}

	public Outbox.Model.OutboxQueue? ToResult(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return GetQuery(scopeContext).FirstOrDefault();
	}

	public async Task<Guid?> GetIdOutboxQueueAsync(
		IScopeContext scopeContext,
		CancellationToken cancellationToken = default)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return await GetQuery(scopeContext)
			.Select(iq => iq.IdOutboxQueue)
			.FirstOrDefaultAsync(cancellationToken);
	}

	public Guid? GetIdOutboxQueue(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return GetQuery(scopeContext)
			.Select(iq => iq.IdOutboxQueue)
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
