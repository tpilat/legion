using Legion.ADF.Messaging.Outbox.PostgreSQL;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Microsoft.EntityFrameworkCore;

namespace Legion.ADF.Messaging.Outbox.Queries.OutboxMessage;

public class ExistsOutboxMessageByQueueMessageId :
	QueryDefinition<
		IOutboxDbContext,
		Outbox.Model.OutboxMessage,
		bool,
		ExistsOutboxMessageByQueueMessageIdQuery>,
	IExistsOutboxMessageByQueueMessageId
{
	public ExistsOutboxMessageByQueueMessageId(
		IEFConnectionProvider connectionProvider,
		ExistsOutboxMessageByQueueMessageIdQuery existsOutboxMessageByQueueMessageId)
		: base(connectionProvider, existsOutboxMessageByQueueMessageId)
	{
	}

	protected override IQueryable<Outbox.Model.OutboxMessage> GetDefaultQuery(IScopeContext scopeContext)
	{
		var context = GetContext(scopeContext);
		return context.OutboxMessage;
	}

	public override IQueryable<Outbox.Model.OutboxMessage> GetQuery(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return ApplyIncludesThenWhere<IMessagingAccessControlManager>(
			scopeContext,
			QueryRequest.QueryableBuilder,
			QueryRequest.CheckReadPermissions,
			QueryRequest.AsNoTracking,
			im => im.IdOutboxQueue == QueryRequest.IdOutboxQueue
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

	public async Task<Guid?> GetIdOutboxMessageAsync(
		IScopeContext scopeContext,
		CancellationToken cancellationToken = default)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return await GetQuery(scopeContext)
			.Select(iq => iq.IdOutboxMessage)
			.FirstOrDefaultAsync(cancellationToken);
	}

	public Guid? GetIdOutboxMessage(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return GetQuery(scopeContext)
			.Select(iq => iq.IdOutboxMessage)
			.FirstOrDefault();
	}
}
