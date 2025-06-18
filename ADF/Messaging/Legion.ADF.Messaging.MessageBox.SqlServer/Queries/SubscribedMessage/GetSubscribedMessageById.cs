using Legion.ADF.Messaging.MessageBox.SqlServer;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Microsoft.EntityFrameworkCore;

namespace Legion.ADF.Messaging.MessageBox.Queries.SubscribedMessage;

public class GetSubscribedMessageById :
	QueryDefinition<
		IMessageBoxDbContext,
		MessageBox.Model.SubscribedMessage,
		MessageBox.Model.SubscribedMessage?,
		GetSubscribedMessageByIdQuery>,
		IGetSubscribedMessageById
{
	public GetSubscribedMessageById(
		IEFConnectionProvider connectionProvider,
		GetSubscribedMessageByIdQuery getSubscribedMessageById)
		: base(connectionProvider, getSubscribedMessageById)
	{
	}

	protected override IQueryable<MessageBox.Model.SubscribedMessage> GetDefaultQuery(IScopeContext scopeContext)
	{
		var context = GetContext(scopeContext);
		return context.SubscribedMessage;
	}

	public override IQueryable<MessageBox.Model.SubscribedMessage> GetQuery(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return ApplyIncludesThenWhere<IMessagingAccessControlManager>(
			scopeContext,
			QueryRequest.QueryableBuilder,
			QueryRequest.CheckReadPermissions,
			QueryRequest.AsNoTracking,
			im => im.IdSubscribedMessage == QueryRequest.IdSubscribedMessage);
	}

	public override async Task<MessageBox.Model.SubscribedMessage?> ToResultAsync(
		IScopeContext scopeContext,
		CancellationToken cancellationToken = default)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return await GetQuery(scopeContext)
			.FirstOrDefaultAsync(cancellationToken);
	}

	public MessageBox.Model.SubscribedMessage? ToResult(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return GetQuery(scopeContext).FirstOrDefault();
	}
}
