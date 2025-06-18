using Legion.ADF.Messaging.Outbox.PostgreSQL;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Microsoft.EntityFrameworkCore;

namespace Legion.ADF.Messaging.Outbox.Queries.VwOutboxMessageContent;

public class GetVwOutboxMessageContentById :
	QueryDefinition<
		IOutboxQueryDbContext,
		Outbox.Model.VwOutboxMessageContent,
		Outbox.Model.VwOutboxMessageContent?,
		GetVwOutboxMessageContentByIdQuery>,
	IGetVwOutboxMessageContentById
{
	public GetVwOutboxMessageContentById(
		IEFConnectionProvider connectionProvider,
		GetVwOutboxMessageContentByIdQuery getVwOutboxMessageContentByIdMessage)
		: base(connectionProvider, getVwOutboxMessageContentByIdMessage)
	{
	}

	protected override IQueryable<Outbox.Model.VwOutboxMessageContent> GetDefaultQuery(IScopeContext scopeContext)
	{
		var context = GetContext(scopeContext);
		return context.VwOutboxMessageContent;
	}

	public override IQueryable<Outbox.Model.VwOutboxMessageContent> GetQuery(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return ApplyIncludesThenWhere<IMessagingAccessControlManager>(
			scopeContext,
			QueryRequest.QueryableBuilder,
			QueryRequest.CheckReadPermissions,
			QueryRequest.AsNoTracking,
			imc => imc.IdOutboxMessageContent == QueryRequest.IdOutboxMessage);
	}

	public override async Task<Outbox.Model.VwOutboxMessageContent?> ToResultAsync(
		IScopeContext scopeContext,
		CancellationToken cancellationToken = default)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return await GetQuery(scopeContext)
			.FirstOrDefaultAsync(cancellationToken);
	}

	public Outbox.Model.VwOutboxMessageContent? ToResult(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return GetQuery(scopeContext).FirstOrDefault();
	}
}
