using Legion.ADF.Messaging.Inbox.PostgreSQL;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Microsoft.EntityFrameworkCore;

namespace Legion.ADF.Messaging.Inbox.Queries.InboxMessageType;

public class GetAllInboxMessageTypes :
	QueryDefinition<
		IInboxDbContext,
		Inbox.Model.InboxMessageType,
		List<Inbox.Model.InboxMessageType>,
		GetAllInboxMessageTypesQuery>,
		IGetAllInboxMessageTypes
{
	public GetAllInboxMessageTypes(
		IEFConnectionProvider connectionProvider,
		GetAllInboxMessageTypesQuery getAllInboxMessageTypes)
		: base(connectionProvider, getAllInboxMessageTypes)
	{
	}

	protected override IQueryable<Inbox.Model.InboxMessageType> GetDefaultQuery(IScopeContext scopeContext)
	{
		var context = GetContext(scopeContext);
		return context.InboxMessageType;
	}

	public override IQueryable<Inbox.Model.InboxMessageType> GetQuery(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return ApplyIncludesThenWhere<IMessagingAccessControlManager>(
			scopeContext,
			QueryRequest.QueryableBuilder,
			QueryRequest.CheckReadPermissions,
			QueryRequest.AsNoTracking,
			null);
	}

	public override async Task<List<Inbox.Model.InboxMessageType>> ToResultAsync(
		IScopeContext scopeContext,
		CancellationToken cancellationToken = default)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return await GetQuery(scopeContext)
			.ToListAsync(cancellationToken);
	}

	public List<Inbox.Model.InboxMessageType> ToResult(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return GetQuery(scopeContext).ToList();
	}
}
