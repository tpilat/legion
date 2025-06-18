using Legion.ADF.Messaging.MessageBox.SqlServer;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Microsoft.EntityFrameworkCore;

namespace Legion.ADF.Messaging.MessageBox.Queries.VwMessageArchive;

public class GetVwMessageArchiveByIdMessage :
	QueryDefinition<
		IMessageBoxQueryDbContext,
		MessageBox.Model.VwMessageArchive,
		MessageBox.Model.VwMessageArchive?,
		GetVwMessageArchiveByIdMessageQuery>,
	IGetVwMessageArchiveByIdMessage
{
	public GetVwMessageArchiveByIdMessage(
		IEFConnectionProvider connectionProvider,
		GetVwMessageArchiveByIdMessageQuery getVwMessageArchiveByIdMessage)
		: base(connectionProvider, getVwMessageArchiveByIdMessage)
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
			im => im.IdMessage == QueryRequest.IdMessage);
	}

	public override async Task<MessageBox.Model.VwMessageArchive?> ToResultAsync(
		IScopeContext scopeContext,
		CancellationToken cancellationToken = default)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return await GetQuery(scopeContext)
			.FirstOrDefaultAsync(cancellationToken);
	}

	public MessageBox.Model.VwMessageArchive? ToResult(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return GetQuery(scopeContext).FirstOrDefault();
	}
}
