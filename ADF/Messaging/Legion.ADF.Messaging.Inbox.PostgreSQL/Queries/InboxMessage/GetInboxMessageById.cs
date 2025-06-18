using Legion.ADF.Messaging.Inbox.PostgreSQL;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Microsoft.EntityFrameworkCore;

namespace Legion.ADF.Messaging.Inbox.Queries.InboxMessage;

public class GetInboxMessageById :
	QueryDefinition<
		IInboxDbContext,
		Inbox.Model.InboxMessage,
		Inbox.Model.InboxMessage?,
		GetInboxMessageByIdQuery>,
	IGetInboxMessageById
{
	public GetInboxMessageById(
		IEFConnectionProvider connectionProvider,
		GetInboxMessageByIdQuery getInboxMessageById)
		: base(connectionProvider, getInboxMessageById)
	{
	}

	protected override IQueryable<Inbox.Model.InboxMessage> GetDefaultQuery(IScopeContext scopeContext)
	{
		var context = GetContext(scopeContext);
		return QueryRequest.IncludeContent
			? context.InboxMessage.Include(im => im.MessageType).Include(im => im.MessageContent)
			: context.InboxMessage.Include(im => im.MessageType);
	}

	public override IQueryable<Inbox.Model.InboxMessage> GetQuery(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return ApplyIncludesThenWhere<IMessagingAccessControlManager>(
			scopeContext,
			QueryRequest.QueryableBuilder,
			QueryRequest.CheckReadPermissions,
			QueryRequest.AsNoTracking,
			x => x.IdInboxMessage == QueryRequest.IdInboxMessage);
	}

	public override async Task<Inbox.Model.InboxMessage?> ToResultAsync(
		IScopeContext scopeContext,
		CancellationToken cancellationToken = default)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return await GetQuery(scopeContext)
			.FirstOrDefaultAsync(cancellationToken);
	}

	public Inbox.Model.InboxMessage? ToResult(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return GetQuery(scopeContext).FirstOrDefault();
	}
}
