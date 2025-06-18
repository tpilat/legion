using Legion.ADF.Messaging.Inbox.PostgreSQL;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Microsoft.EntityFrameworkCore;

namespace Legion.ADF.Messaging.Inbox.Queries.InboxInstance;

public class GetInboxInstanceById :
	QueryDefinition<
		IInboxDbContext,
		Inbox.Model.InboxInstance,
		Inbox.Model.InboxInstance?,
		GetInboxInstanceByIdQuery>,
		IGetInboxInstanceById
{
	public GetInboxInstanceById(
		IEFConnectionProvider connectionProvider,
		GetInboxInstanceByIdQuery getInboxInstanceById)
		: base(connectionProvider, getInboxInstanceById)
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

	public override async Task<Inbox.Model.InboxInstance?> ToResultAsync(
		IScopeContext scopeContext,
		CancellationToken cancellationToken = default)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return await GetQuery(scopeContext)
			.FirstOrDefaultAsync(cancellationToken);
	}

	public Inbox.Model.InboxInstance? ToResult(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return GetQuery(scopeContext).FirstOrDefault();
	}
}
