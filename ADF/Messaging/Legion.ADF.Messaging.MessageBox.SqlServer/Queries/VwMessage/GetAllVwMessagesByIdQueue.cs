using Legion.ADF.Messaging.MessageBox.SqlServer;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Microsoft.EntityFrameworkCore;

namespace Legion.ADF.Messaging.MessageBox.Queries.VwMessage;

public class GetAllVwMessagesByIdQueue :
	QueryDefinition<
		IMessageBoxQueryDbContext,
		MessageBox.Model.VwMessage,
		List<MessageBox.Model.VwMessage>,
		GetAllVwMessagesByIdQueueQuery>,
	IGetAllVwMessagesByIdQueue
{
	public GetAllVwMessagesByIdQueue(
		IEFConnectionProvider connectionProvider,
		GetAllVwMessagesByIdQueueQuery getAllVwMessagesByIdQueue)
		: base(connectionProvider, getAllVwMessagesByIdQueue)
	{
	}

	protected override IQueryable<MessageBox.Model.VwMessage> GetDefaultQuery(IScopeContext scopeContext)
	{
		var context = GetContext(scopeContext);
		return context.VwMessage;
	}

	public override IQueryable<MessageBox.Model.VwMessage> GetQuery(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return ApplyIncludesThenWhere<IMessagingAccessControlManager>(
			scopeContext,
			QueryRequest.QueryableBuilder,
			QueryRequest.CheckReadPermissions,
			QueryRequest.AsNoTracking,
			im => im.IdQueue == QueryRequest.IdQueue);
	}

	public override async Task<List<MessageBox.Model.VwMessage>> ToResultAsync(
		IScopeContext scopeContext,
		CancellationToken cancellationToken = default)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return await GetQuery(scopeContext)
			.ToListAsync(cancellationToken);
	}

	public List<MessageBox.Model.VwMessage> ToResult(IScopeContext scopeContext)
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
