using Legion.ADF.Messaging.Outbox.SqlServer;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Microsoft.EntityFrameworkCore;

namespace Legion.ADF.Messaging.Outbox.Queries.OutboxMessageType;

public class GetAllOutboxMessageTypes :
	QueryDefinition<
		IOutboxDbContext,
		Outbox.Model.OutboxMessageType,
		List<Outbox.Model.OutboxMessageType>,
		GetAllOutboxMessageTypesQuery>,
		IGetAllOutboxMessageTypes
{
	public GetAllOutboxMessageTypes(
		IEFConnectionProvider connectionProvider,
		GetAllOutboxMessageTypesQuery getAllOutboxMessageTypes)
		: base(connectionProvider, getAllOutboxMessageTypes)
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
			null);
	}

	public override async Task<List<Outbox.Model.OutboxMessageType>> ToResultAsync(
		IScopeContext scopeContext,
		CancellationToken cancellationToken = default)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return await GetQuery(scopeContext)
			.ToListAsync(cancellationToken);
	}

	public List<Outbox.Model.OutboxMessageType> ToResult(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return GetQuery(scopeContext).ToList();
	}
}
