using Legion.ADF.Messaging.MessageBox.SqlServer;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Microsoft.EntityFrameworkCore;

namespace Legion.ADF.Messaging.MessageBox.Queries.VwQueueMessage;

public class GetAllQueues :
	QueryDefinition<
		IMessageBoxQueryDbContext,
		MessageBox.Model.VwQueueMessages,
		List<MessageBox.Model.VwQueueMessages>,
		GetAllQueuesQuery>,
		IGetAllQueues
{
	public GetAllQueues(
		IEFConnectionProvider connectionProvider,
		GetAllQueuesQuery getAllQueues)
		: base(connectionProvider, getAllQueues)
	{
	}

	protected override IQueryable<MessageBox.Model.VwQueueMessages> GetDefaultQuery(IScopeContext scopeContext)
	{
		var context = GetContext(scopeContext);
		return context.VwQueueMessages;
	}

	public override IQueryable<MessageBox.Model.VwQueueMessages> GetQuery(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		if (QueryRequest.IncludeInactiveQueues)
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

	public override async Task<List<MessageBox.Model.VwQueueMessages>> ToResultAsync(
		IScopeContext scopeContext,
		CancellationToken cancellationToken = default)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return await GetQuery(scopeContext)
			.ToListAsync(cancellationToken);
	}

	public List<MessageBox.Model.VwQueueMessages> ToResult(IScopeContext scopeContext)
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
