using Legion.ADF.Messaging.Inbox.SqlServer;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Microsoft.EntityFrameworkCore;

namespace Legion.ADF.Messaging.Inbox.Queries.VwInboxMessageArchive;

public class GetVwInboxMessageArchiveByIdMessage :
	QueryDefinition<
		IInboxQueryDbContext,
		Inbox.Model.VwInboxMessageArchive,
		Inbox.Model.VwInboxMessageArchive?,
		GetVwInboxMessageArchiveByIdMessageQuery>,
	IGetVwInboxMessageArchiveByIdMessage
{
	public GetVwInboxMessageArchiveByIdMessage(
		IEFConnectionProvider connectionProvider,
		GetVwInboxMessageArchiveByIdMessageQuery getVwInboxMessageArchiveByIdMessage)
		: base(connectionProvider, getVwInboxMessageArchiveByIdMessage)
	{
	}

	protected override IQueryable<Inbox.Model.VwInboxMessageArchive> GetDefaultQuery(IScopeContext scopeContext)
	{
		var context = GetContext(scopeContext);
		return context.VwInboxMessageArchive;
	}

	public override IQueryable<Inbox.Model.VwInboxMessageArchive> GetQuery(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return ApplyIncludesThenWhere<IMessagingAccessControlManager>(
			scopeContext,
			QueryRequest.QueryableBuilder,
			QueryRequest.CheckReadPermissions,
			QueryRequest.AsNoTracking,
			im => im.IdInboxMessage == QueryRequest.IdInboxMessage);
	}

	public override async Task<Inbox.Model.VwInboxMessageArchive?> ToResultAsync(
		IScopeContext scopeContext,
		CancellationToken cancellationToken = default)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return await GetQuery(scopeContext)
			.FirstOrDefaultAsync(cancellationToken);
	}

	public Inbox.Model.VwInboxMessageArchive? ToResult(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return GetQuery(scopeContext).FirstOrDefault();
	}
}
