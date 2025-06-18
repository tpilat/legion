using Legion.ADF.Messaging.Inbox.SqlServer;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Microsoft.EntityFrameworkCore;

namespace Legion.ADF.Messaging.Inbox.Queries.VwInboxMessageProcessingLog;

public class GetVwInboxMessageProcessingLogsByIdMessage :
	QueryDefinition<
		IInboxQueryDbContext,
		Inbox.Model.VwInboxMessageProcessingLog,
		List<Inbox.Model.VwInboxMessageProcessingLog>,
		GetVwInboxMessageProcessingLogsByIdMessageQuery>,
	IGetVwInboxMessageProcessingLogsByIdMessage
{
	public GetVwInboxMessageProcessingLogsByIdMessage(
		IEFConnectionProvider connectionProvider,
		GetVwInboxMessageProcessingLogsByIdMessageQuery getVwInboxMessageProcessingLogByIdMessage)
		: base(connectionProvider, getVwInboxMessageProcessingLogByIdMessage)
	{
	}

	protected override IQueryable<Inbox.Model.VwInboxMessageProcessingLog> GetDefaultQuery(IScopeContext scopeContext)
	{
		var context = GetContext(scopeContext);
		return context.VwInboxMessageProcessingLog;
	}

	public override IQueryable<Inbox.Model.VwInboxMessageProcessingLog> GetQuery(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return ApplyIncludesThenWhere<IMessagingAccessControlManager>(
			scopeContext,
			QueryRequest.QueryableBuilder,
			QueryRequest.CheckReadPermissions,
			QueryRequest.AsNoTracking,
			impl => impl.IdInboxMessage == QueryRequest.IdInboxMessage);
	}

	public override async Task<List<Inbox.Model.VwInboxMessageProcessingLog>> ToResultAsync(
		IScopeContext scopeContext,
		CancellationToken cancellationToken = default)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return await GetQuery(scopeContext)
			.ToListAsync(cancellationToken);
	}

	public List<Inbox.Model.VwInboxMessageProcessingLog> ToResult(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return GetQuery(scopeContext).ToList();
	}
}
