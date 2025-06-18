using Legion.ADF.Messaging.MessageBox.PostgreSQL;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Microsoft.EntityFrameworkCore;

namespace Legion.ADF.Messaging.MessageBox.Queries.Queue;

public class GetAllQueuesByEvents :
	QueryDefinition<
		IMessageBoxDbContext,
		MessageBox.Model.Queue,
		List<MessageBox.Model.Queue>,
		GetAllQueuesByEventsQuery>,
		IGetAllQueuesByEvents
{
	public GetAllQueuesByEvents(
		IEFConnectionProvider connectionProvider,
		GetAllQueuesByEventsQuery getAllQueuesByEvents)
		: base(connectionProvider, getAllQueuesByEvents)
	{
	}

	protected override IQueryable<MessageBox.Model.Queue> GetDefaultQuery(IScopeContext scopeContext)
	{
		var context = GetContext(scopeContext);
		return context.Queue;
	}

	public override IQueryable<MessageBox.Model.Queue> GetQuery(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		if (QueryRequest.ReceivedEventNamespaces == null || QueryRequest.ReceivedEventNamespaces.Count == 0)
			return Enumerable.Empty<MessageBox.Model.Queue>().AsAsyncQueryable();

		return ApplyIncludesThenWhere<IMessagingAccessControlManager>(
			scopeContext,
			QueryRequest.QueryableBuilder,
			QueryRequest.CheckReadPermissions,
			QueryRequest.AsNoTracking,
			x => x.IsActive == true && QueryRequest.ReceivedEventNamespaces.Contains(x.ReceivedEventNamespace));
	}

	public override async Task<List<MessageBox.Model.Queue>> ToResultAsync(
		IScopeContext scopeContext,
		CancellationToken cancellationToken = default)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return await GetQuery(scopeContext)
			.ToListAsync(cancellationToken);
	}

	public MessageBox.Model.Queue? ToResult(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return GetQuery(scopeContext).FirstOrDefault();
	}
}
