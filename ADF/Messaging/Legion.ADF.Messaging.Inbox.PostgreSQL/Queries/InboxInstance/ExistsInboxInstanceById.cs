using Legion.ADF.Messaging.Inbox.PostgreSQL;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Microsoft.EntityFrameworkCore;

namespace Legion.ADF.Messaging.Inbox.Queries.InboxInstance;

public class ExistsInboxInstanceById :
	QueryDefinition<
		IInboxDbContext,
		Inbox.Model.InboxInstance,
		bool,
		ExistsInboxInstanceByIdQuery>,
		IExistsInboxInstanceById
{
	public ExistsInboxInstanceById(
		IEFConnectionProvider connectionProvider,
		ExistsInboxInstanceByIdQuery existsInboxInstanceById)
		: base(connectionProvider, existsInboxInstanceById)
	{
	}

	protected override IQueryable<Inbox.Model.InboxInstance> GetDefaultQuery(IScopeContext scopeContext)
	{
		var context = GetContext(scopeContext);
		return context.InboxInstance;
	}

	public override IQueryable<Inbox.Model.InboxInstance> GetQuery(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return ApplyIncludesThenWhere<IMessagingAccessControlManager>(
			scopeContext,
			QueryRequest.QueryableBuilder,
			QueryRequest.CheckReadPermissions,
			QueryRequest.AsNoTracking,
			imt => imt.IdInboxInstance == QueryRequest.IdInboxInstance);
	}

	public override async Task<bool> ToResultAsync(
		IScopeContext scopeContext,
		CancellationToken cancellationToken = default)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return await GetQuery(scopeContext)
			.AnyAsync(cancellationToken);
	}

	public bool ToResult(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return GetQuery(scopeContext).Any();
	}
}
