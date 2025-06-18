using Legion.ADF.Messaging.Inbox.PostgreSQL;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Microsoft.EntityFrameworkCore;

namespace Legion.ADF.Messaging.Inbox.Queries.InboxMessage;

public class ExistsInboxMessageByQueueMessageId :
	QueryDefinition<
		IInboxDbContext,
		Inbox.Model.InboxMessage,
		bool,
		ExistsInboxMessageByQueueMessageIdQuery>,
	IExistsInboxMessageByQueueMessageId
{
	public ExistsInboxMessageByQueueMessageId(
		IEFConnectionProvider connectionProvider,
		ExistsInboxMessageByQueueMessageIdQuery existsInboxMessageByQueueMessageId)
		: base(connectionProvider, existsInboxMessageByQueueMessageId)
	{
	}

	protected override IQueryable<Inbox.Model.InboxMessage> GetDefaultQuery(IScopeContext scopeContext)
	{
		var context = GetContext(scopeContext);
		return context.InboxMessage;
	}

	public override IQueryable<Inbox.Model.InboxMessage> GetQuery(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return ApplyIncludesThenWhere<IMessagingAccessControlManager>(
			scopeContext,
			QueryRequest.QueryableBuilder,
			QueryRequest.CheckReadPermissions,
			QueryRequest.AsNoTracking,
			im => im.IdInboxQueue == QueryRequest.IdInboxQueue
				&& im.MessageId == QueryRequest.MessageId);
	}

	public override async Task<bool> ToResultAsync(
		IScopeContext scopeContext,
		CancellationToken cancellationToken = default)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return await GetQuery(scopeContext)
			.AnyAsync(cancellationToken);
	}

	public bool ToResult(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return GetQuery(scopeContext).Any();
	}

	public async Task<Guid?> GetIdInboxMessageAsync(
		IScopeContext scopeContext,
		CancellationToken cancellationToken = default)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return await GetQuery(scopeContext)
			.Select(iq => iq.IdInboxMessage)
			.FirstOrDefaultAsync(cancellationToken);
	}

	public Guid? GetIdInboxMessage(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return GetQuery(scopeContext)
			.Select(iq => iq.IdInboxMessage)
			.FirstOrDefault();
	}
}
