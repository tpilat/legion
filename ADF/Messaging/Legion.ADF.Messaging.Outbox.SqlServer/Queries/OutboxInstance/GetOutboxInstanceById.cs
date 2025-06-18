using Legion.ADF.Messaging.Outbox.SqlServer;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Microsoft.EntityFrameworkCore;

namespace Legion.ADF.Messaging.Outbox.Queries.OutboxInstance;

public class GetOutboxInstanceById :
	QueryDefinition<
		IOutboxDbContext,
		Outbox.Model.OutboxInstance,
		Outbox.Model.OutboxInstance?,
		GetOutboxInstanceByIdQuery>,
		IGetOutboxInstanceById
{
	public GetOutboxInstanceById(
		IEFConnectionProvider connectionProvider,
		GetOutboxInstanceByIdQuery getOutboxInstanceById)
		: base(connectionProvider, getOutboxInstanceById)
	{
	}

	protected override IQueryable<Outbox.Model.OutboxInstance> GetDefaultQuery(IScopeContext scopeContext)
	{
		var context = GetContext(scopeContext);
		return context.OutboxInstance;
	}

	public override IQueryable<Outbox.Model.OutboxInstance> GetQuery(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return ApplyIncludesThenWhere<IMessagingAccessControlManager>(
			scopeContext,
			QueryRequest.QueryableBuilder,
			QueryRequest.CheckReadPermissions,
			QueryRequest.AsNoTracking,
			imt => imt.IdOutboxInstance == QueryRequest.IdOutboxInstance);
	}

	public override async Task<Outbox.Model.OutboxInstance?> ToResultAsync(
		IScopeContext scopeContext,
		CancellationToken cancellationToken = default)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return await GetQuery(scopeContext)
			.FirstOrDefaultAsync(cancellationToken);
	}

	public Outbox.Model.OutboxInstance? ToResult(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return GetQuery(scopeContext).FirstOrDefault();
	}
}
