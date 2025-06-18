using Legion.ADF.Messaging.Outbox.PostgreSQL;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Microsoft.EntityFrameworkCore;

namespace Legion.ADF.Messaging.Outbox.Queries.BlockedOutboxMessageType;

public class GetBlockedOutboxMessageTypesByNamespaces :
	QueryDefinition<
		IOutboxDbContext,
		Outbox.Model.BlockedOutboxMessageType,
		List<Outbox.Model.BlockedOutboxMessageType>,
		GetBlockedOutboxMessageTypesByNamespacesQuery>,
		IGetBlockedOutboxMessageTypesByNamespaces
{
	public GetBlockedOutboxMessageTypesByNamespaces(
		IEFConnectionProvider connectionProvider,
		GetBlockedOutboxMessageTypesByNamespacesQuery GetBlockedOutboxMessageTypesByNamespaces)
		: base(connectionProvider, GetBlockedOutboxMessageTypesByNamespaces)
	{
	}

	protected override IQueryable<Outbox.Model.BlockedOutboxMessageType> GetDefaultQuery(IScopeContext scopeContext)
	{
		var context = GetContext(scopeContext);
		return context.BlockedOutboxMessageType;
	}

	public override IQueryable<Outbox.Model.BlockedOutboxMessageType> GetQuery(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		if (0 < QueryRequest.Namespaces?.Count)
		{
			return ApplyIncludesThenWhere<IMessagingAccessControlManager>(
				scopeContext,
				QueryRequest.QueryableBuilder,
				QueryRequest.CheckReadPermissions,
				QueryRequest.AsNoTracking,
				x => QueryRequest.Namespaces.Contains(x.Namespace));
		}
		else
		{
			return ApplyIncludesThenWhere<IMessagingAccessControlManager>(
				scopeContext,
				QueryRequest.QueryableBuilder,
				QueryRequest.CheckReadPermissions,
				QueryRequest.AsNoTracking,
				null);
		}
	}

	public override async Task<List<Outbox.Model.BlockedOutboxMessageType>> ToResultAsync(
		IScopeContext scopeContext,
		CancellationToken cancellationToken = default)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return await GetQuery(scopeContext)
			.ToListAsync(cancellationToken);
	}

	public List<Outbox.Model.BlockedOutboxMessageType> ToResult(IScopeContext scopeContext)
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
