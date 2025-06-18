using Legion.ADF.Messaging.MessageBox.SqlServer;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Microsoft.EntityFrameworkCore;

namespace Legion.ADF.Messaging.MessageBox.Queries.QueuedMessage;

public class GetQueuedMessageById :
	QueryDefinition<
		IMessageBoxDbContext,
		MessageBox.Model.QueuedMessage,
		MessageBox.Model.QueuedMessage?,
		GetQueuedMessageByIdQuery>,
		IGetQueuedMessageById
{
	public GetQueuedMessageById(
		IEFConnectionProvider connectionProvider,
		GetQueuedMessageByIdQuery getQueuedMessageByIdQuery)
		: base(connectionProvider, getQueuedMessageByIdQuery)
	{
	}

	protected override IQueryable<MessageBox.Model.QueuedMessage> GetDefaultQuery(IScopeContext scopeContext)
	{
		var context = GetContext(scopeContext);
		return context.QueuedMessage;
	}

	public override IQueryable<MessageBox.Model.QueuedMessage> GetQuery(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return ApplyIncludesThenWhere<IMessagingAccessControlManager>(
			scopeContext,
			QueryRequest.QueryableBuilder,
			QueryRequest.CheckReadPermissions,
			QueryRequest.AsNoTracking,
			im => im.IdQueuedMessage == QueryRequest.IdQueuedMessage);
	}

	public override async Task<MessageBox.Model.QueuedMessage?> ToResultAsync(
		IScopeContext scopeContext,
		CancellationToken cancellationToken = default)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return await GetQuery(scopeContext)
			.FirstOrDefaultAsync(cancellationToken);
	}

	public MessageBox.Model.QueuedMessage? ToResult(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return GetQuery(scopeContext).FirstOrDefault();
	}
}
