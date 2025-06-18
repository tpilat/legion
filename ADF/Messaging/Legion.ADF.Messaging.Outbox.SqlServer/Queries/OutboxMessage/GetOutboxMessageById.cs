using Legion.ADF.Messaging.Outbox.SqlServer;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Microsoft.EntityFrameworkCore;

namespace Legion.ADF.Messaging.Outbox.Queries.OutboxMessage;

public class GetOutboxMessageById :
	QueryDefinition<
		IOutboxDbContext,
		Outbox.Model.OutboxMessage,
		Outbox.Model.OutboxMessage?,
		GetOutboxMessageByIdQuery>,
	IGetOutboxMessageById
{
	public GetOutboxMessageById(
		IEFConnectionProvider connectionProvider,
		GetOutboxMessageByIdQuery getOutboxMessageById)
		: base(connectionProvider, getOutboxMessageById)
	{
	}

	protected override IQueryable<Outbox.Model.OutboxMessage> GetDefaultQuery(IScopeContext scopeContext)
	{
		var context = GetContext(scopeContext);
		return QueryRequest.IncludeContent
			? context.OutboxMessage.Include(im => im.MessageType).Include(im => im.MessageContent)
			: context.OutboxMessage.Include(im => im.MessageType);
	}

	public override IQueryable<Outbox.Model.OutboxMessage> GetQuery(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return ApplyIncludesThenWhere<IMessagingAccessControlManager>(
			scopeContext,
			QueryRequest.QueryableBuilder,
			QueryRequest.CheckReadPermissions,
			QueryRequest.AsNoTracking,
			x => x.IdOutboxMessage == QueryRequest.IdOutboxMessage);
	}

	public override async Task<Outbox.Model.OutboxMessage?> ToResultAsync(
		IScopeContext scopeContext,
		CancellationToken cancellationToken = default)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return await GetQuery(scopeContext)
			.FirstOrDefaultAsync(cancellationToken);
	}

	public Outbox.Model.OutboxMessage? ToResult(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return GetQuery(scopeContext).FirstOrDefault();
	}
}
