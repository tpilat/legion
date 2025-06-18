using Legion.ADF.Messaging.MessageBox.SqlServer;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Microsoft.EntityFrameworkCore;

namespace Legion.ADF.Messaging.MessageBox.Queries.VwMessageArchive;

public class GetAllVwMessageArchivesByIdQueue :
	QueryDefinition<
		IMessageBoxQueryDbContext,
		MessageBox.Model.VwMessageArchive,
		List<MessageBox.Model.VwMessageArchive>,
		GetAllVwMessageArchivesByIdQueueQuery>,
	IGetAllVwMessageArchivesByIdQueue
{
	public GetAllVwMessageArchivesByIdQueue(
		IEFConnectionProvider connectionProvider,
		GetAllVwMessageArchivesByIdQueueQuery getAllVwMessageArchivesByIdQueue)
		: base(connectionProvider, getAllVwMessageArchivesByIdQueue)
	{
	}

	protected override IQueryable<MessageBox.Model.VwMessageArchive> GetDefaultQuery(IScopeContext scopeContext)
	{
		var context = GetContext(scopeContext);
		return context.VwMessageArchive;
	}

	public override IQueryable<MessageBox.Model.VwMessageArchive> GetQuery(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return ApplyIncludesThenWhere<IMessagingAccessControlManager>(
			scopeContext,
			QueryRequest.QueryableBuilder,
			QueryRequest.CheckReadPermissions,
			QueryRequest.AsNoTracking,
			im => im.IdQueue == QueryRequest.IdQueue);
	}

	public override async Task<List<MessageBox.Model.VwMessageArchive>> ToResultAsync(
		IScopeContext scopeContext,
		CancellationToken cancellationToken = default)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return await GetQuery(scopeContext)
			.ToListAsync(cancellationToken);
	}

	public List<MessageBox.Model.VwMessageArchive> ToResult(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return GetQuery(scopeContext).ToList();
	}

	public async Task<long> TotalCountAsync(
		IScopeContext scopeContext,
		CancellationToken cancellationToken = default)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return await GetQuery(scopeContext)
			.CountAsync(cancellationToken);
	}

	public long TotalCount(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return GetQuery(scopeContext).Count();
	}
}
