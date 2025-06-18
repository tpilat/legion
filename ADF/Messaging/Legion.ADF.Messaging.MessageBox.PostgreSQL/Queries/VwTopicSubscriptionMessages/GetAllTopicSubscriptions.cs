using Legion.ADF.Messaging.MessageBox.PostgreSQL;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Microsoft.EntityFrameworkCore;

namespace Legion.ADF.Messaging.MessageBox.Queries.VwTopicSubscriptionMessage;

public class GetAllTopicSubscriptions :
	QueryDefinition<
		IMessageBoxQueryDbContext,
		MessageBox.Model.VwTopicSubscriptionMessages,
		List<MessageBox.Model.VwTopicSubscriptionMessages>,
		GetAllTopicSubscriptionsQuery>,
		IGetAllTopicSubscriptions
{
	public GetAllTopicSubscriptions(
		IEFConnectionProvider connectionProvider,
		GetAllTopicSubscriptionsQuery getAllTopicSubscriptions)
		: base(connectionProvider, getAllTopicSubscriptions)
	{
	}

	protected override IQueryable<MessageBox.Model.VwTopicSubscriptionMessages> GetDefaultQuery(IScopeContext scopeContext)
	{
		var context = GetContext(scopeContext);
		return context.VwTopicSubscriptionMessages;
	}

	public override IQueryable<MessageBox.Model.VwTopicSubscriptionMessages> GetQuery(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		if (QueryRequest.IncludeInactiveTopicSubscriptions)
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
				x => x.TopisIsActive == true && x.SubscriptionIsActive == true);
		}
	}

	public override async Task<List<MessageBox.Model.VwTopicSubscriptionMessages>> ToResultAsync(
		IScopeContext scopeContext,
		CancellationToken cancellationToken = default)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return await GetQuery(scopeContext)
			.ToListAsync(cancellationToken);
	}

	public List<MessageBox.Model.VwTopicSubscriptionMessages> ToResult(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return GetQuery(scopeContext).ToList();
	}

	public async Task<long> TotalCountAsync(
		IScopeContext scopeContext,
		CancellationToken cancellationToken = default)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return await GetQuery(scopeContext)
			.CountAsync(cancellationToken);
	}

	public long TotalCount(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return GetQuery(scopeContext).Count();
	}
}
