using Legion.ADF.Messaging.MessageBox.PostgreSQL;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Microsoft.EntityFrameworkCore;

namespace Legion.ADF.Messaging.MessageBox.Queries.VwQueue;

public class GetVwQueueById :
	QueryDefinition<
		IMessageBoxQueryDbContext,
		MessageBox.Model.VwQueue,
		MessageBox.Model.VwQueue?,
		GetVwQueueByIdQuery>,
	IGetVwQueueById
{
	public GetVwQueueById(
		IEFConnectionProvider connectionProvider,
		GetVwQueueByIdQuery getVwQueueById)
		: base(connectionProvider, getVwQueueById)
	{
	}

	protected override IQueryable<MessageBox.Model.VwQueue> GetDefaultQuery(IScopeContext scopeContext)
	{
		var context = GetContext(scopeContext);
		return context.VwQueue;
	}

	public override IQueryable<MessageBox.Model.VwQueue> GetQuery(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return ApplyIncludesThenWhere<IMessagingAccessControlManager>(
			scopeContext,
			QueryRequest.QueryableBuilder,
			QueryRequest.CheckReadPermissions,
			QueryRequest.AsNoTracking,
			im => im.IdQueue == QueryRequest.IdQueue);
	}

	public override async Task<MessageBox.Model.VwQueue?> ToResultAsync(
		IScopeContext scopeContext,
		CancellationToken cancellationToken = default)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return await GetQuery(scopeContext)
			.FirstOrDefaultAsync(cancellationToken);
	}

	public MessageBox.Model.VwQueue? ToResult(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return GetQuery(scopeContext).FirstOrDefault();
	}
}
