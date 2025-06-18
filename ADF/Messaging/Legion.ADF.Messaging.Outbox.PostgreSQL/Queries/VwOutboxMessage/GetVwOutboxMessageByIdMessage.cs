using Legion.ADF.Messaging.Outbox.PostgreSQL;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Microsoft.EntityFrameworkCore;

namespace Legion.ADF.Messaging.Outbox.Queries.VwOutboxMessage;

public class GetVwOutboxMessageByIdMessage :
	QueryDefinition<
		IOutboxQueryDbContext,
		Outbox.Model.VwOutboxMessage,
		Outbox.Model.VwOutboxMessage?,
		GetVwOutboxMessageByIdMessageQuery>,
	IGetVwOutboxMessageByIdMessage
{
	public GetVwOutboxMessageByIdMessage(
		IEFConnectionProvider connectionProvider,
		GetVwOutboxMessageByIdMessageQuery getVwOutboxMessageByIdMessage)
		: base(connectionProvider, getVwOutboxMessageByIdMessage)
	{
	}

	protected override IQueryable<Outbox.Model.VwOutboxMessage> GetDefaultQuery(IScopeContext scopeContext)
	{
		var context = GetContext(scopeContext);
		return context.VwOutboxMessage;
	}

	public override IQueryable<Outbox.Model.VwOutboxMessage> GetQuery(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return ApplyIncludesThenWhere<IMessagingAccessControlManager>(
			scopeContext,
			QueryRequest.QueryableBuilder,
			QueryRequest.CheckReadPermissions,
			QueryRequest.AsNoTracking,
			im => im.IdOutboxMessage == QueryRequest.IdOutboxMessage);
	}

	public override async Task<Outbox.Model.VwOutboxMessage?> ToResultAsync(
		IScopeContext scopeContext,
		CancellationToken cancellationToken = default)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return await GetQuery(scopeContext)
			.FirstOrDefaultAsync(cancellationToken);
	}

	public Outbox.Model.VwOutboxMessage? ToResult(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return GetQuery(scopeContext).FirstOrDefault();
	}
}
