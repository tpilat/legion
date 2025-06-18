using Legion.ADF.Messaging.Inbox.PostgreSQL;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Microsoft.EntityFrameworkCore;

namespace Legion.ADF.Messaging.Inbox.Queries.BlockedInboxMessageType;

public class GetAllBlockedInboxMessageTypes :
	QueryDefinition<
		IInboxDbContext,
		Inbox.Model.BlockedInboxMessageType,
		List<Inbox.Model.BlockedInboxMessageType>,
		GetAllBlockedInboxMessageTypesQuery>,
		IGetAllBlockedInboxMessageTypes
{
	public GetAllBlockedInboxMessageTypes(
		IEFConnectionProvider connectionProvider,
		GetAllBlockedInboxMessageTypesQuery getAllBlockedInboxMessageTypes)
		: base(connectionProvider, getAllBlockedInboxMessageTypes)
	{
	}

	protected override IQueryable<Inbox.Model.BlockedInboxMessageType> GetDefaultQuery(IScopeContext scopeContext)
	{
		var context = GetContext(scopeContext);
		return context.BlockedInboxMessageType;
	}

	public override IQueryable<Inbox.Model.BlockedInboxMessageType> GetQuery(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return ApplyIncludesThenWhere<IMessagingAccessControlManager>(
			scopeContext,
			QueryRequest.QueryableBuilder,
			QueryRequest.CheckReadPermissions,
			QueryRequest.AsNoTracking,
			null);
	}

	public override async Task<List<Inbox.Model.BlockedInboxMessageType>> ToResultAsync(
		IScopeContext scopeContext,
		CancellationToken cancellationToken = default)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return await GetQuery(scopeContext)
			.ToListAsync(cancellationToken);
	}

	public List<Inbox.Model.BlockedInboxMessageType> ToResult(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return GetQuery(scopeContext).ToList();
	}

	public async Task<List<string>> ToNamespacesAsync(
		IScopeContext scopeContext,
		CancellationToken cancellationToken = default)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return await GetQuery(scopeContext)
			.Select(bde => bde.Namespace)
			.ToListAsync(cancellationToken);
	}

	public List<string> ToNamespaces(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return GetQuery(scopeContext)
			.Select(bde => bde.Namespace)
			.ToList();
	}
}
