using Legion.ADF.Messaging.MessageBox.PostgreSQL;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Microsoft.EntityFrameworkCore;

namespace Legion.ADF.Messaging.MessageBox.Queries.Queue;

public class GetAllQueues :
	QueryDefinition<
		IMessageBoxDbContext,
		MessageBox.Model.Queue,
		List<MessageBox.Model.Queue>,
		GetAllQueuesQuery>,
		IGetAllQueues
{
	public GetAllQueues(
		IEFConnectionProvider connectionProvider,
		GetAllQueuesQuery getAllQueues)
		: base(connectionProvider, getAllQueues)
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
