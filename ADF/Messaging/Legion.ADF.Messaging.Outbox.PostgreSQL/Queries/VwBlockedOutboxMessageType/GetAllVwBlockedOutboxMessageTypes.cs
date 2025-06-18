using Legion.ADF.Messaging.Outbox.PostgreSQL;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Microsoft.EntityFrameworkCore;

namespace Legion.ADF.Messaging.Outbox.Queries.VwBlockedOutboxMessageType;

public class GetAllVwBlockedOutboxMessageTypes :
	QueryDefinition<
		IOutboxQueryDbContext,
		Outbox.Model.VwBlockedOutboxMessageType,
		List<Outbox.Model.VwBlockedOutboxMessageType>,
		GetAllVwBlockedOutboxMessageTypesQuery>,
		IGetAllVwBlockedOutboxMessageTypes
{
	public GetAllVwBlockedOutboxMessageTypes(
		IEFConnectionProvider connectionProvider,
		GetAllVwBlockedOutboxMessageTypesQuery getAllVwBlockedOutboxMessageTypes)
		: base(connectionProvider, getAllVwBlockedOutboxMessageTypes)
	{
	}

	protected override IQueryable<Outbox.Model.VwBlockedOutboxMessageType> GetDefaultQuery(IScopeContext scopeContext)
	{
		var context = GetContext(scopeContext);
		return context.VwBlockedOutboxMessageType;
	}

	public override IQueryable<Outbox.Model.VwBlockedOutboxMessageType> GetQuery(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return ApplyIncludesThenWhere<IMessagingAccessControlManager>(
			scopeContext,
			QueryRequest.QueryableBuilder,
			QueryRequest.CheckReadPermissions,
			QueryRequest.AsNoTracking,
			null);
	}

	public override async Task<List<Outbox.Model.VwBlockedOutboxMessageType>> ToResultAsync(
		IScopeContext scopeContext,
		CancellationToken cancellationToken = default)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return await GetQuery(scopeContext)
			.ToListAsync(cancellationToken);
	}

	public List<Outbox.Model.VwBlockedOutboxMessageType> ToResult(IScopeContext scopeContext)
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
