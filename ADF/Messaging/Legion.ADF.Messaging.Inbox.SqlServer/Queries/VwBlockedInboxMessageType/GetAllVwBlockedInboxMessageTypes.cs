using Legion.ADF.Messaging.Inbox.SqlServer;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Microsoft.EntityFrameworkCore;

namespace Legion.ADF.Messaging.Inbox.Queries.VwBlockedInboxMessageType;

public class GetAllVwBlockedInboxMessageTypes :
	QueryDefinition<
		IInboxQueryDbContext,
		Inbox.Model.VwBlockedInboxMessageType,
		List<Inbox.Model.VwBlockedInboxMessageType>,
		GetAllVwBlockedInboxMessageTypesQuery>,
		IGetAllVwBlockedInboxMessageTypes
{
	public GetAllVwBlockedInboxMessageTypes(
		IEFConnectionProvider connectionProvider,
		GetAllVwBlockedInboxMessageTypesQuery getAllVwBlockedInboxMessageTypes)
		: base(connectionProvider, getAllVwBlockedInboxMessageTypes)
	{
	}

	protected override IQueryable<Inbox.Model.VwBlockedInboxMessageType> GetDefaultQuery(IScopeContext scopeContext)
	{
		var context = GetContext(scopeContext);
		return context.VwBlockedInboxMessageType;
	}

	public override IQueryable<Inbox.Model.VwBlockedInboxMessageType> GetQuery(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return ApplyIncludesThenWhere<IMessagingAccessControlManager>(
			scopeContext,
			QueryRequest.QueryableBuilder,
			QueryRequest.CheckReadPermissions,
			QueryRequest.AsNoTracking,
			null);
	}

	public override async Task<List<Inbox.Model.VwBlockedInboxMessageType>> ToResultAsync(
		IScopeContext scopeContext,
		CancellationToken cancellationToken = default)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return await GetQuery(scopeContext)
			.ToListAsync(cancellationToken);
	}

	public List<Inbox.Model.VwBlockedInboxMessageType> ToResult(IScopeContext scopeContext)
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
