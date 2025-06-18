using Legion.ADF.Messaging.Inbox.PostgreSQL;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Microsoft.EntityFrameworkCore;

namespace Legion.ADF.Messaging.Inbox.Queries.InboxMessageType;

public class GetInboxMessageTypeByNamespace :
	QueryDefinition<
		IInboxDbContext,
		Inbox.Model.InboxMessageType,
		Inbox.Model.InboxMessageType?,
		GetInboxMessageTypeByNamespaceQuery>,
		IGetInboxMessageTypeByNamespace
{
	public GetInboxMessageTypeByNamespace(
		IEFConnectionProvider connectionProvider,
		GetInboxMessageTypeByNamespaceQuery getInboxMessageTypeByNamespace)
		: base(connectionProvider, getInboxMessageTypeByNamespace)
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
			imt => imt.Namespace == QueryRequest.Namespace);
	}

	public override async Task<Inbox.Model.InboxMessageType?> ToResultAsync(
		IScopeContext scopeContext,
		CancellationToken cancellationToken = default)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return await GetQuery(scopeContext)
			.FirstOrDefaultAsync(cancellationToken);
	}

	public Inbox.Model.InboxMessageType? ToResult(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return GetQuery(scopeContext).FirstOrDefault();
	}

	public async Task<Guid?> GetIdInboxMessageTypeAsync(
		IScopeContext scopeContext,
		CancellationToken cancellationToken = default)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return await GetQuery(scopeContext)
			.Select(iq => iq.IdInboxMessageType)
			.FirstOrDefaultAsync(cancellationToken);
	}

	public Guid? GetIdInboxMessageType(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return GetQuery(scopeContext)
			.Select(iq => iq.IdInboxMessageType)
			.FirstOrDefault();
	}

	public async Task<bool> ExistsAsync(
		IScopeContext scopeContext,
		CancellationToken cancellationToken = default)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return await GetQuery(scopeContext)
			.AnyAsync(cancellationToken);
	}

	public bool Exists(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return GetQuery(scopeContext)
			.Any();
	}
}
