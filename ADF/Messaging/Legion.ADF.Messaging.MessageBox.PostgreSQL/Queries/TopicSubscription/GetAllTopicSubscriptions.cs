using Legion.ADF.Messaging.MessageBox.PostgreSQL;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Microsoft.EntityFrameworkCore;

namespace Legion.ADF.Messaging.MessageBox.Queries.TopicSubscription;

public class GetAllTopicSubscriptions :
	QueryDefinition<
		IMessageBoxDbContext,
		MessageBox.Model.TopicSubscription,
		List<MessageBox.Model.TopicSubscription>,
		GetAllTopicSubscriptionsQuery>,
		IGetAllTopicSubscriptions
{
	public GetAllTopicSubscriptions(
		IEFConnectionProvider connectionProvider,
		GetAllTopicSubscriptionsQuery getAllTopicSubscriptions)
		: base(connectionProvider, getAllTopicSubscriptions)
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
				
		if (QueryRequest.IncludeInactiveTopics)
		{
			return ApplyIncludesThenWhere<IMessagingAccessControlManager>(
				scopeContext,
				QueryRequest.QueryableBuilder,
				QueryRequest.CheckReadPermissions,
				QueryRequest.AsNoTracking,
				null);
		}
		else
		{
			return ApplyIncludesThenWhere<IMessagingAccessControlManager>(
				scopeContext,
				QueryRequest.QueryableBuilder,
				QueryRequest.CheckReadPermissions,
				QueryRequest.AsNoTracking,
				x => x.IsActive == true);
		}
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
