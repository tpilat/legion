using Legion.ADF.Messaging.MessageBox.PostgreSQL;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Microsoft.EntityFrameworkCore;

namespace Legion.ADF.Messaging.MessageBox.Queries.SubscribedMessage;

public class GetNextSubscribedMessagesBySubscription :
	QueryDefinition<
		IMessageBoxDbContext,
		MessageBox.Model.SubscribedMessage,
		List<MessageBox.Model.SubscribedMessage>,
		GetNextSubscribedMessagesBySubscriptionQuery>,
		IGetNextSubscribedMessagesBySubscription
{
	public GetNextSubscribedMessagesBySubscription(
		IEFConnectionProvider connectionProvider,
		GetNextSubscribedMessagesBySubscriptionQuery getNextSubscribedMessagesBySubscription)
		: base(connectionProvider, getNextSubscribedMessagesBySubscription)
	{
	}

	protected override IQueryable<MessageBox.Model.SubscribedMessage> GetDefaultQuery(IScopeContext scopeContext)
	{
		var context = GetContext(scopeContext);
		return context.SubscribedMessage;
	}

	public override IQueryable<MessageBox.Model.SubscribedMessage> GetQuery(IScopeContext scopeContext)
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
					im.IdTopicSubscription == QueryRequest.IdTopicSubscription
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
					im.IdTopicSubscription == QueryRequest.IdTopicSubscription
					&& !im.ProcessedUtc.HasValue
					&& !im.SuspendedUtc.HasValue
					&& im.NextProcessingUtc <= QueryRequest.NowUtc)
				.OrderBy(de => de.NextProcessingUtc)
				.Take(QueryRequest.BatchCount);
		}
	}

	public override async Task<List<MessageBox.Model.SubscribedMessage>> ToResultAsync(
		IScopeContext scopeContext,
		CancellationToken cancellationToken = default)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return await GetQuery(scopeContext)
			.ToListAsync(cancellationToken);
	}

	public List<MessageBox.Model.SubscribedMessage> ToResult(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return GetQuery(scopeContext).ToList();
	}

	public async Task<Dictionary<Guid, DateTime>> ToSubscribedMessageIds(
		IScopeContext scopeContext,
		CancellationToken cancellationToken = default)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return await GetQuery(scopeContext)
			.Select(de => new ValueTuple<Guid, DateTime>(de.IdSubscribedMessage, de.NextProcessingUtc))
			.ToDictionaryAsync(k => k.Item1, v => v.Item2, cancellationToken);
	}

	public Dictionary<Guid, DateTime> ToSubscribedMessageIds(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return GetQuery(scopeContext)
			.Select(de => new ValueTuple<Guid, DateTime>(de.IdMessage, de.NextProcessingUtc))
			.ToDictionary(k => k.Item1, v => v.Item2);
	}
}
