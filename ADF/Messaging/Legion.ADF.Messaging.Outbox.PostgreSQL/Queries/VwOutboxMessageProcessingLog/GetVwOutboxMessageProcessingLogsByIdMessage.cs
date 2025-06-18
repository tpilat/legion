using Legion.ADF.Messaging.Outbox.PostgreSQL;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Microsoft.EntityFrameworkCore;

namespace Legion.ADF.Messaging.Outbox.Queries.VwOutboxMessageProcessingLog;

public class GetVwOutboxMessageProcessingLogsByIdMessage :
	QueryDefinition<
		IOutboxQueryDbContext,
		Outbox.Model.VwOutboxMessageProcessingLog,
		List<Outbox.Model.VwOutboxMessageProcessingLog>,
		GetVwOutboxMessageProcessingLogsByIdMessageQuery>,
	IGetVwOutboxMessageProcessingLogsByIdMessage
{
	public GetVwOutboxMessageProcessingLogsByIdMessage(
		IEFConnectionProvider connectionProvider,
		GetVwOutboxMessageProcessingLogsByIdMessageQuery getVwOutboxMessageProcessingLogByIdMessage)
		: base(connectionProvider, getVwOutboxMessageProcessingLogByIdMessage)
	{
	}

	protected override IQueryable<Outbox.Model.VwOutboxMessageProcessingLog> GetDefaultQuery(IScopeContext scopeContext)
	{
		var context = GetContext(scopeContext);
		return context.VwOutboxMessageProcessingLog;
	}

	public override IQueryable<Outbox.Model.VwOutboxMessageProcessingLog> GetQuery(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return ApplyIncludesThenWhere<IMessagingAccessControlManager>(
			scopeContext,
			QueryRequest.QueryableBuilder,
			QueryRequest.CheckReadPermissions,
			QueryRequest.AsNoTracking,
			impl => impl.IdOutboxMessage == QueryRequest.IdOutboxMessage);
	}

	public override async Task<List<Outbox.Model.VwOutboxMessageProcessingLog>> ToResultAsync(
		IScopeContext scopeContext,
		CancellationToken cancellationToken = default)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return await GetQuery(scopeContext)
			.ToListAsync(cancellationToken);
	}

	public List<Outbox.Model.VwOutboxMessageProcessingLog> ToResult(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return GetQuery(scopeContext).ToList();
	}
}
