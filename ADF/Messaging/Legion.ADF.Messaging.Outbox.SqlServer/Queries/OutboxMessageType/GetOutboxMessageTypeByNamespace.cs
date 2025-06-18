using Legion.ADF.Messaging.Outbox.SqlServer;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Microsoft.EntityFrameworkCore;

namespace Legion.ADF.Messaging.Outbox.Queries.OutboxMessageType;

public class GetOutboxMessageTypeByNamespace :
	QueryDefinition<
		IOutboxDbContext,
		Outbox.Model.OutboxMessageType,
		Outbox.Model.OutboxMessageType?,
		GetOutboxMessageTypeByNamespaceQuery>,
		IGetOutboxMessageTypeByNamespace
{
	public GetOutboxMessageTypeByNamespace(
		IEFConnectionProvider connectionProvider,
		GetOutboxMessageTypeByNamespaceQuery getOutboxMessageTypeByNamespace)
		: base(connectionProvider, getOutboxMessageTypeByNamespace)
	{
	}

	protected override IQueryable<Outbox.Model.OutboxMessageType> GetDefaultQuery(IScopeContext scopeContext)
	{
		var context = GetContext(scopeContext);
		return context.OutboxMessageType;
	}

	public override IQueryable<Outbox.Model.OutboxMessageType> GetQuery(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return ApplyIncludesThenWhere<IMessagingAccessControlManager>(
			scopeContext,
			QueryRequest.QueryableBuilder,
			QueryRequest.CheckReadPermissions,
			QueryRequest.AsNoTracking,
			imt => imt.Namespace == QueryRequest.Namespace);
	}

	public override async Task<Outbox.Model.OutboxMessageType?> ToResultAsync(
		IScopeContext scopeContext,
		CancellationToken cancellationToken = default)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return await GetQuery(scopeContext)
			.FirstOrDefaultAsync(cancellationToken);
	}

	public Outbox.Model.OutboxMessageType? ToResult(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return GetQuery(scopeContext).FirstOrDefault();
	}

	public async Task<Guid?> GetIdOutboxMessageTypeAsync(
		IScopeContext scopeContext,
		CancellationToken cancellationToken = default)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return await GetQuery(scopeContext)
			.Select(iq => iq.IdOutboxMessageType)
			.FirstOrDefaultAsync(cancellationToken);
	}

	public Guid? GetIdOutboxMessageType(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return GetQuery(scopeContext)
			.Select(iq => iq.IdOutboxMessageType)
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
