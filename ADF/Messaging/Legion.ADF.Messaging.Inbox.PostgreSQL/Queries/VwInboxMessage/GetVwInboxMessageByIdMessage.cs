using Legion.ADF.Messaging.Inbox.PostgreSQL;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Microsoft.EntityFrameworkCore;

namespace Legion.ADF.Messaging.Inbox.Queries.VwInboxMessage;

public class GetVwInboxMessageByIdMessage :
	QueryDefinition<
		IInboxQueryDbContext,
		Inbox.Model.VwInboxMessage,
		Inbox.Model.VwInboxMessage?,
		GetVwInboxMessageByIdMessageQuery>,
	IGetVwInboxMessageByIdMessage
{
	public GetVwInboxMessageByIdMessage(
		IEFConnectionProvider connectionProvider,
		GetVwInboxMessageByIdMessageQuery getVwInboxMessageByIdMessage)
		: base(connectionProvider, getVwInboxMessageByIdMessage)
	{
	}

	protected override IQueryable<Inbox.Model.VwInboxMessage> GetDefaultQuery(IScopeContext scopeContext)
	{
		var context = GetContext(scopeContext);
		return context.VwInboxMessage;
	}

	public override IQueryable<Inbox.Model.VwInboxMessage> GetQuery(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return ApplyIncludesThenWhere<IMessagingAccessControlManager>(
			scopeContext,
			QueryRequest.QueryableBuilder,
			QueryRequest.CheckReadPermissions,
			QueryRequest.AsNoTracking,
			im => im.IdInboxMessage == QueryRequest.IdInboxMessage);
	}

	public override async Task<Inbox.Model.VwInboxMessage?> ToResultAsync(
		IScopeContext scopeContext,
		CancellationToken cancellationToken = default)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return await GetQuery(scopeContext)
			.FirstOrDefaultAsync(cancellationToken);
	}

	public Inbox.Model.VwInboxMessage? ToResult(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return GetQuery(scopeContext).FirstOrDefault();
	}
}
