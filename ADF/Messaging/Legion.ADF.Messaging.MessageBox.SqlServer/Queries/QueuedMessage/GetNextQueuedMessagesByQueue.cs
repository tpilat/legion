using Legion.ADF.Messaging.MessageBox.SqlServer;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Microsoft.EntityFrameworkCore;

namespace Legion.ADF.Messaging.MessageBox.Queries.QueuedMessage;

public class GetNextQueuedMessagesByQueue :
	QueryDefinition<
		IMessageBoxDbContext,
		MessageBox.Model.QueuedMessage,
		List<MessageBox.Model.QueuedMessage>,
		GetNextQueuedMessagesByQueueQuery>,
		IGetNextQueuedMessagesByQueue
{
	public GetNextQueuedMessagesByQueue(
		IEFConnectionProvider connectionProvider,
		GetNextQueuedMessagesByQueueQuery getNextQueuedMessagesByQueue)
		: base(connectionProvider, getNextQueuedMessagesByQueue)
	{
	}

	protected override IQueryable<MessageBox.Model.QueuedMessage> GetDefaultQuery(IScopeContext scopeContext)
	{
		var context = GetContext(scopeContext);
		return context.QueuedMessage;
	}

	public override IQueryable<MessageBox.Model.QueuedMessage> GetQuery(IScopeContext scopeContext)
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
					im.IdQueue == QueryRequest.IdQueue
					&& !im.ProcessedUtc.HasValue
					&& !im.SuspendedUtc.HasValue
					&& im.AssignedUtc <= QueryRequest.NowUtc)
				.OrderBy(de => de.AssignedUtc)
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
					im.IdQueue == QueryRequest.IdQueue
					&& !im.ProcessedUtc.HasValue
					&& !im.SuspendedUtc.HasValue
					&& im.NextProcessingUtc <= QueryRequest.NowUtc)
				.OrderBy(de => de.NextProcessingUtc)
				.Take(QueryRequest.BatchCount);
		}
	}

	public override async Task<List<MessageBox.Model.QueuedMessage>> ToResultAsync(
		IScopeContext scopeContext,
		CancellationToken cancellationToken = default)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return await GetQuery(scopeContext)
			.ToListAsync(cancellationToken);
	}

	public List<MessageBox.Model.QueuedMessage> ToResult(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return GetQuery(scopeContext).ToList();
	}

	public async Task<Dictionary<Guid, DateTime>> ToMessageIds(
		IScopeContext scopeContext,
		CancellationToken cancellationToken = default)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return await GetQuery(scopeContext)
			.Select(de => new ValueTuple<Guid, DateTime>(de.IdQueuedMessage, de.NextProcessingUtc))
			.ToDictionaryAsync(k => k.Item1, v => v.Item2, cancellationToken);
	}

	public Dictionary<Guid, DateTime> ToMessageIds(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return GetQuery(scopeContext)
			.Select(de => new ValueTuple<Guid, DateTime>(de.IdQueuedMessage, de.NextProcessingUtc))
			.ToDictionary(k => k.Item1, v => v.Item2);
	}
}
