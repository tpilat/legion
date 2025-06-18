using Legion.ADF.Messaging.MessageBox.PostgreSQL;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Microsoft.EntityFrameworkCore;

namespace Legion.ADF.Messaging.MessageBox.Queries.VwTopicSubscription;

public class GetVwTopicSubscriptionById :
	QueryDefinition<
		IMessageBoxQueryDbContext,
		MessageBox.Model.VwTopicSubscription,
		MessageBox.Model.VwTopicSubscription?,
		GetVwTopicSubscriptionByIdQuery>,
		IGetVwTopicSubscriptionById
{
	public GetVwTopicSubscriptionById(
		IEFConnectionProvider connectionProvider,
		GetVwTopicSubscriptionByIdQuery getVwTopicSubscriptionById)
		: base(connectionProvider, getVwTopicSubscriptionById)
	{
	}

	protected override IQueryable<MessageBox.Model.VwTopicSubscription> GetDefaultQuery(IScopeContext scopeContext)
	{
		var context = GetContext(scopeContext);
		return context.VwTopicSubscription;
	}

	public override IQueryable<MessageBox.Model.VwTopicSubscription> GetQuery(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return ApplyIncludesThenWhere<IMessagingAccessControlManager>(
			scopeContext,
			QueryRequest.QueryableBuilder,
			QueryRequest.CheckReadPermissions,
			QueryRequest.AsNoTracking,
			im => im.IdTopicSubscription == QueryRequest.IdTopicSubscription);
	}

	public override async Task<MessageBox.Model.VwTopicSubscription?> ToResultAsync(
		IScopeContext scopeContext,
		CancellationToken cancellationToken = default)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return await GetQuery(scopeContext)
			.FirstOrDefaultAsync(cancellationToken);
	}

	public MessageBox.Model.VwTopicSubscription? ToResult(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return GetQuery(scopeContext).FirstOrDefault();
	}
}
