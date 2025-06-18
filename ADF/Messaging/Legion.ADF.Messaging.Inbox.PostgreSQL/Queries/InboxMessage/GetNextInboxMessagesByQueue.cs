using Legion.ADF.Messaging.Inbox.PostgreSQL;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Microsoft.EntityFrameworkCore;

namespace Legion.ADF.Messaging.Inbox.Queries.InboxMessage;

public class GetNextInboxMessagesByQueue :
	QueryDefinition<
		IInboxDbContext,
		Inbox.Model.InboxMessage,
		List<Inbox.Model.InboxMessage>,
		GetNextInboxMessagesByQueueQuery>,
		IGetNextInboxMessagesByQueue
{
	public GetNextInboxMessagesByQueue(
		IEFConnectionProvider connectionProvider,
		GetNextInboxMessagesByQueueQuery getNextInboxMessagesByQueue)
		: base(connectionProvider, getNextInboxMessagesByQueue)
	{
	}

	protected override IQueryable<Inbox.Model.InboxMessage> GetDefaultQuery(IScopeContext scopeContext)
	{
		var context = GetContext(scopeContext);
		return context.InboxMessage;
	}

	public override IQueryable<Inbox.Model.InboxMessage> GetQuery(IScopeContext scopeContext)
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
					im.IdInboxQueue == QueryRequest.IdInboxQueue
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
					im.IdInboxQueue == QueryRequest.IdInboxQueue
					&& !im.ProcessedUtc.HasValue
					&& !im.SuspendedUtc.HasValue
					&& im.NextProcessingUtc <= QueryRequest.NowUtc)
				.OrderBy(de => de.NextProcessingUtc)
				.Take(QueryRequest.BatchCount);
		}
	}

	public override async Task<List<Inbox.Model.InboxMessage>> ToResultAsync(
		IScopeContext scopeContext,
		CancellationToken cancellationToken = default)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return await GetQuery(scopeContext)
			.ToListAsync(cancellationToken);
	}

	public List<Inbox.Model.InboxMessage> ToResult(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return GetQuery(scopeContext).ToList();
	}

	public async Task<Dictionary<Guid, DateTime>> ToInboxMessageIds(
		IScopeContext scopeContext,
		CancellationToken cancellationToken = default)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return await GetQuery(scopeContext)
			.Select(de => new ValueTuple<Guid, DateTime>(de.IdInboxMessage, de.NextProcessingUtc))
			.ToDictionaryAsync(k => k.Item1, v => v.Item2, cancellationToken);
	}

	public Dictionary<Guid, DateTime> ToInboxMessageIds(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return GetQuery(scopeContext)
			.Select(de => new ValueTuple<Guid, DateTime>(de.IdInboxMessage, de.NextProcessingUtc))
			.ToDictionary(k => k.Item1, v => v.Item2);
	}
}
