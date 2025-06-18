using Legion.ADF.Messaging.Outbox.SqlServer;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Microsoft.EntityFrameworkCore;

namespace Legion.ADF.Messaging.Outbox.Queries.OutboxMessage;

public class GetNextOutboxMessagesByQueue :
	QueryDefinition<
		IOutboxDbContext,
		Outbox.Model.OutboxMessage,
		List<Outbox.Model.OutboxMessage>,
		GetNextOutboxMessagesByQueueQuery>,
		IGetNextOutboxMessagesByQueue
{
	public GetNextOutboxMessagesByQueue(
		IEFConnectionProvider connectionProvider,
		GetNextOutboxMessagesByQueueQuery getNextOutboxMessagesByQueue)
		: base(connectionProvider, getNextOutboxMessagesByQueue)
	{
	}

	protected override IQueryable<Outbox.Model.OutboxMessage> GetDefaultQuery(IScopeContext scopeContext)
	{
		var context = GetContext(scopeContext);
		return context.OutboxMessage;
	}

	public override IQueryable<Outbox.Model.OutboxMessage> GetQuery(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		if (QueryRequest.IsFIFO)
		{
			return ApplyIncludesThenWhere<IMessagingAccessControlManager>(
				scopeContext,
				QueryRequest.QueryableBuilder,
				QueryRequest.CheckReadPermissions,
				QueryRequest.AsNoTracking,
				im =>
					im.IdOutboxQueue == QueryRequest.IdOutboxQueue
					&& !im.ProcessedUtc.HasValue
					&& !im.SuspendedUtc.HasValue
					&& im.CreatedUtc <= QueryRequest.NowUtc)
				.OrderBy(de => de.CreatedUtc)
				.Take(QueryRequest.BatchCount);
		}
		else
		{
			return ApplyIncludesThenWhere<IMessagingAccessControlManager>(
				scopeContext,
				QueryRequest.QueryableBuilder,
				QueryRequest.CheckReadPermissions,
				QueryRequest.AsNoTracking,
				im =>
					im.IdOutboxQueue == QueryRequest.IdOutboxQueue
					&& !im.ProcessedUtc.HasValue
					&& !im.SuspendedUtc.HasValue
					&& im.NextProcessingUtc <= QueryRequest.NowUtc)
				.OrderBy(de => de.NextProcessingUtc)
				.Take(QueryRequest.BatchCount);
		}
	}

	public override async Task<List<Outbox.Model.OutboxMessage>> ToResultAsync(
		IScopeContext scopeContext,
		CancellationToken cancellationToken = default)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return await GetQuery(scopeContext)
			.ToListAsync(cancellationToken);
	}

	public List<Outbox.Model.OutboxMessage> ToResult(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return GetQuery(scopeContext).ToList();
	}

	public async Task<Dictionary<Guid, DateTime>> ToOutboxMessageIds(
		IScopeContext scopeContext,
		CancellationToken cancellationToken = default)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return await GetQuery(scopeContext)
			.Select(de => new ValueTuple<Guid, DateTime>(de.IdOutboxMessage, de.NextProcessingUtc))
			.ToDictionaryAsync(k => k.Item1, v => v.Item2, cancellationToken);
	}

	public Dictionary<Guid, DateTime> ToOutboxMessageIds(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return GetQuery(scopeContext)
			.Select(de => new ValueTuple<Guid, DateTime>(de.IdOutboxMessage, de.NextProcessingUtc))
			.ToDictionary(k => k.Item1, v => v.Item2);
	}
}
