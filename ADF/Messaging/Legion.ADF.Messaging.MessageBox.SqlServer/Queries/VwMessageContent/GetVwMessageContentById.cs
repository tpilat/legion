using Legion.ADF.Messaging.MessageBox.SqlServer;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Microsoft.EntityFrameworkCore;

namespace Legion.ADF.Messaging.MessageBox.Queries.VwMessageContent;

public class GetVwMessageContentById :
	QueryDefinition<
		IMessageBoxQueryDbContext,
		MessageBox.Model.VwMessageContent,
		MessageBox.Model.VwMessageContent?,
		GetVwMessageContentByIdQuery>,
	IGetVwMessageContentById
{
	public GetVwMessageContentById(
		IEFConnectionProvider connectionProvider,
		GetVwMessageContentByIdQuery getVwMessageContentByIdMessage)
		: base(connectionProvider, getVwMessageContentByIdMessage)
	{
	}

	protected override IQueryable<MessageBox.Model.VwMessageContent> GetDefaultQuery(IScopeContext scopeContext)
	{
		var context = GetContext(scopeContext);
		return context.VwMessageContent;
	}

	public override IQueryable<MessageBox.Model.VwMessageContent> GetQuery(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return ApplyIncludesThenWhere<IMessagingAccessControlManager>(
			scopeContext,
			QueryRequest.QueryableBuilder,
			QueryRequest.CheckReadPermissions,
			QueryRequest.AsNoTracking,
			imc => imc.IdMessageContent == QueryRequest.IdMessage);
	}

	public override async Task<MessageBox.Model.VwMessageContent?> ToResultAsync(
		IScopeContext scopeContext,
		CancellationToken cancellationToken = default)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return await GetQuery(scopeContext)
			.FirstOrDefaultAsync(cancellationToken);
	}

	public MessageBox.Model.VwMessageContent? ToResult(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return GetQuery(scopeContext).FirstOrDefault();
	}
}
