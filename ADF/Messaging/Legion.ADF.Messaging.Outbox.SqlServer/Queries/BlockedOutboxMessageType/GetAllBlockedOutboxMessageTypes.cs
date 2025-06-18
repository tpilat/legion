using Legion.ADF.Messaging.Outbox.SqlServer;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Microsoft.EntityFrameworkCore;

namespace Legion.ADF.Messaging.Outbox.Queries.BlockedOutboxMessageType;

public class GetAllBlockedOutboxMessageTypes :
	QueryDefinition<
		IOutboxDbContext,
		Outbox.Model.BlockedOutboxMessageType,
		List<Outbox.Model.BlockedOutboxMessageType>,
		GetAllBlockedOutboxMessageTypesQuery>,
		IGetAllBlockedOutboxMessageTypes
{
	public GetAllBlockedOutboxMessageTypes(
		IEFConnectionProvider connectionProvider,
		GetAllBlockedOutboxMessageTypesQuery getAllBlockedOutboxMessageTypes)
		: base(connectionProvider, getAllBlockedOutboxMessageTypes)
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

		return ApplyIncludesThenWhere<IMessagingAccessControlManager>(
			scopeContext,
			QueryRequest.QueryableBuilder,
			QueryRequest.CheckReadPermissions,
			QueryRequest.AsNoTracking,
			null);
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
