using Legion.ADF.Messaging.MessageBox.SqlServer;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Microsoft.EntityFrameworkCore;

namespace Legion.ADF.Messaging.MessageBox.Queries.TopicSubscription;

public class GetAllTopicSubscriptionsByTopicAndEvents :
	QueryDefinition<
		IMessageBoxDbContext,
		MessageBox.Model.TopicSubscription,
		List<MessageBox.Model.TopicSubscription>,
		GetAllTopicSubscriptionsByTopicAndEventsQuery>,
		IGetAllTopicSubscriptionsByTopicAndEvents
{
	public GetAllTopicSubscriptionsByTopicAndEvents(
		IEFConnectionProvider connectionProvider,
		GetAllTopicSubscriptionsByTopicAndEventsQuery getAllTopicSubscriptionsByTopicAndEvents)
		: base(connectionProvider, getAllTopicSubscriptionsByTopicAndEvents)
	{
	}

	protected override IQueryable<MessageBox.Model.TopicSubscription> GetDefaultQuery(IScopeContext scopeContext)
	{
		var context = GetContext(scopeContext);
		return context.TopicSubscription;
	}

	public override IQueryable<MessageBox.Model.TopicSubscription> GetQuery(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		if (QueryRequest.ReceivedEventNamespaces == null || QueryRequest.ReceivedEventNamespaces.Count == 0)
			return Enumerable.Empty<MessageBox.Model.TopicSubscription>().AsAsyncQueryable();

		return ApplyIncludesThenWhere<IMessagingAccessControlManager>(
			scopeContext,
			QueryRequest.QueryableBuilder,
			QueryRequest.CheckReadPermissions,
			QueryRequest.AsNoTracking,
			x => x.IsActive == true && QueryRequest.ReceivedEventNamespaces.Contains(x.ReceivedEventNamespace));
	}

	public override async Task<List<MessageBox.Model.TopicSubscription>> ToResultAsync(
		IScopeContext scopeContext,
		CancellationToken cancellationToken = default)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return await GetQuery(scopeContext)
			.ToListAsync(cancellationToken);
	}

	public List<MessageBox.Model.TopicSubscription> ToResult(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return GetQuery(scopeContext).ToList();
	}

	public async Task<List<Guid>> GetIdTopicSubscriptionsAsync(
		IScopeContext scopeContext,
		CancellationToken cancellationToken = default)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return await GetQuery(scopeContext)
			.Select(iq => iq.IdTopicSubscription)
			.ToListAsync(cancellationToken);
	}

	public List<Guid> GetIdTopicSubscriptions(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return GetQuery(scopeContext)
			.Select(iq => iq.IdTopicSubscription)
			.ToList();
	}
}
