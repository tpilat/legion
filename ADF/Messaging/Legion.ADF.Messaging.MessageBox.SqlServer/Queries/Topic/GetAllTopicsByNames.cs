using Legion.ADF.Messaging.MessageBox.SqlServer;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Microsoft.EntityFrameworkCore;

namespace Legion.ADF.Messaging.MessageBox.Queries.Topic;

public class GetAllTopicsByNames :
	QueryDefinition<
		IMessageBoxDbContext,
		MessageBox.Model.Topic,
		List<MessageBox.Model.Topic>,
		GetAllTopicsByNamesQuery>,
		IGetAllTopicsByNames
{
	public GetAllTopicsByNames(
		IEFConnectionProvider connectionProvider,
		GetAllTopicsByNamesQuery getAllTopicsByNames)
		: base(connectionProvider, getAllTopicsByNames)
	{
	}

	protected override IQueryable<MessageBox.Model.Topic> GetDefaultQuery(IScopeContext scopeContext)
	{
		var context = GetContext(scopeContext);
		return context.Topic;
	}

	public override IQueryable<MessageBox.Model.Topic> GetQuery(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		if (QueryRequest.TopicNames == null || QueryRequest.TopicNames.Count == 0)
			return Enumerable.Empty<MessageBox.Model.Topic>().AsAsyncQueryable();

		return ApplyIncludesThenWhere<IMessagingAccessControlManager>(
			scopeContext,
			QueryRequest.QueryableBuilder,
			QueryRequest.CheckReadPermissions,
			QueryRequest.AsNoTracking,
			iq => QueryRequest.TopicNames.Contains(iq.Name));
	}

	public override async Task<List<MessageBox.Model.Topic>> ToResultAsync(
		IScopeContext scopeContext,
		CancellationToken cancellationToken = default)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return await GetQuery(scopeContext)
			.ToListAsync(cancellationToken);
	}

	public List<MessageBox.Model.Topic> ToResult(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return GetQuery(scopeContext).ToList();
	}
}
