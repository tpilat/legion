using Legion.ADF.Messaging.Outbox.SqlServer;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Microsoft.EntityFrameworkCore;

namespace Legion.ADF.Messaging.Outbox.Queries.VwOutboxMessageArchive;

public class GetVwOutboxMessageArchiveByIdMessage :
	QueryDefinition<
		IOutboxQueryDbContext,
		Outbox.Model.VwOutboxMessageArchive,
		Outbox.Model.VwOutboxMessageArchive?,
		GetVwOutboxMessageArchiveByIdMessageQuery>,
	IGetVwOutboxMessageArchiveByIdMessage
{
	public GetVwOutboxMessageArchiveByIdMessage(
		IEFConnectionProvider connectionProvider,
		GetVwOutboxMessageArchiveByIdMessageQuery getVwOutboxMessageArchiveByIdMessage)
		: base(connectionProvider, getVwOutboxMessageArchiveByIdMessage)
	{
	}

	protected override IQueryable<Outbox.Model.VwOutboxMessageArchive> GetDefaultQuery(IScopeContext scopeContext)
	{
		var context = GetContext(scopeContext);
		return context.VwOutboxMessageArchive;
	}

	public override IQueryable<Outbox.Model.VwOutboxMessageArchive> GetQuery(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return ApplyIncludesThenWhere<IMessagingAccessControlManager>(
			scopeContext,
			QueryRequest.QueryableBuilder,
			QueryRequest.CheckReadPermissions,
			QueryRequest.AsNoTracking,
			im => im.IdOutboxMessage == QueryRequest.IdOutboxMessage);
	}

	public override async Task<Outbox.Model.VwOutboxMessageArchive?> ToResultAsync(
		IScopeContext scopeContext,
		CancellationToken cancellationToken = default)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return await GetQuery(scopeContext)
			.FirstOrDefaultAsync(cancellationToken);
	}

	public Outbox.Model.VwOutboxMessageArchive? ToResult(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return GetQuery(scopeContext).FirstOrDefault();
	}
}
