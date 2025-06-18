using Legion.ADF.Messaging.MessageBox.PostgreSQL;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Microsoft.EntityFrameworkCore;

namespace Legion.ADF.Messaging.MessageBox.Queries.TopicSubscription;

public class GetTopicSubscriptionByTopicAndName :
	QueryDefinition<
		IMessageBoxDbContext,
		MessageBox.Model.TopicSubscription,
		MessageBox.Model.TopicSubscription?,
		GetTopicSubscriptionByTopicAndNameQuery>,
		IGetTopicSubscriptionByTopicAndName
{
	public GetTopicSubscriptionByTopicAndName(
		IEFConnectionProvider connectionProvider,
		GetTopicSubscriptionByTopicAndNameQuery getTopicSubscriptionByTopicAndName)
		: base(connectionProvider, getTopicSubscriptionByTopicAndName)
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

		return ApplyIncludesThenWhere<IMessagingAccessControlManager>(
			scopeContext,
			QueryRequest.QueryableBuilder,
			QueryRequest.CheckReadPermissions,
			QueryRequest.AsNoTracking,
			x => x.IdTopic == QueryRequest.IdTopic && x.SubscriptionName == QueryRequest.SubscriptionName);
	}

	public override async Task<MessageBox.Model.TopicSubscription?> ToResultAsync(
		IScopeContext scopeContext,
		CancellationToken cancellationToken = default)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return await GetQuery(scopeContext)
			.FirstOrDefaultAsync(cancellationToken);
	}

	public MessageBox.Model.TopicSubscription? ToResult(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return GetQuery(scopeContext).FirstOrDefault();
	}

	public async Task<Guid?> GetIdTopicSubscriptionAsync(
		IScopeContext scopeContext,
		CancellationToken cancellationToken = default)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return await GetQuery(scopeContext)
			.Select(iq => iq.IdTopicSubscription)
			.FirstOrDefaultAsync(cancellationToken);
	}

	public Guid? GetIdTopicSubscription(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return GetQuery(scopeContext)
			.Select(iq => iq.IdTopicSubscription)
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
