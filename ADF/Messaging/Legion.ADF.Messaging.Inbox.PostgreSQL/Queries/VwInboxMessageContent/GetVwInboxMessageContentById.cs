using Legion.ADF.Messaging.Inbox.PostgreSQL;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Microsoft.EntityFrameworkCore;

namespace Legion.ADF.Messaging.Inbox.Queries.VwInboxMessageContent;

public class GetVwInboxMessageContentById :
	QueryDefinition<
		IInboxQueryDbContext,
		Inbox.Model.VwInboxMessageContent,
		Inbox.Model.VwInboxMessageContent?,
		GetVwInboxMessageContentByIdQuery>,
	IGetVwInboxMessageContentById
{
	public GetVwInboxMessageContentById(
		IEFConnectionProvider connectionProvider,
		GetVwInboxMessageContentByIdQuery getVwInboxMessageContentByIdMessage)
		: base(connectionProvider, getVwInboxMessageContentByIdMessage)
	{
	}

	protected override IQueryable<Inbox.Model.VwInboxMessageContent> GetDefaultQuery(IScopeContext scopeContext)
	{
		var context = GetContext(scopeContext);
		return context.VwInboxMessageContent;
	}

	public override IQueryable<Inbox.Model.VwInboxMessageContent> GetQuery(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return ApplyIncludesThenWhere<IMessagingAccessControlManager>(
			scopeContext,
			QueryRequest.QueryableBuilder,
			QueryRequest.CheckReadPermissions,
			QueryRequest.AsNoTracking,
			imc => imc.IdInboxMessageContent == QueryRequest.IdInboxMessage);
	}

	public override async Task<Inbox.Model.VwInboxMessageContent?> ToResultAsync(
		IScopeContext scopeContext,
		CancellationToken cancellationToken = default)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return await GetQuery(scopeContext)
			.FirstOrDefaultAsync(cancellationToken);
	}

	public Inbox.Model.VwInboxMessageContent? ToResult(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return GetQuery(scopeContext).FirstOrDefault();
	}
}
