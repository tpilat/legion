using Legion.ADF.Messaging.MessageBox.PostgreSQL;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Microsoft.EntityFrameworkCore;

namespace Legion.ADF.Messaging.MessageBox.Queries.VwMessage;

public class GetVwMessageByIdMessage :
	QueryDefinition<
		IMessageBoxQueryDbContext,
		MessageBox.Model.VwMessage,
		MessageBox.Model.VwMessage?,
		GetVwMessageByIdMessageQuery>,
	IGetVwMessageByIdMessage
{
	public GetVwMessageByIdMessage(
		IEFConnectionProvider connectionProvider,
		GetVwMessageByIdMessageQuery getVwMessageByIdMessage)
		: base(connectionProvider, getVwMessageByIdMessage)
	{
	}

	protected override IQueryable<MessageBox.Model.VwMessage> GetDefaultQuery(IScopeContext scopeContext)
	{
		var context = GetContext(scopeContext);
		return context.VwMessage;
	}

	public override IQueryable<MessageBox.Model.VwMessage> GetQuery(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return ApplyIncludesThenWhere<IMessagingAccessControlManager>(
			scopeContext,
			QueryRequest.QueryableBuilder,
			QueryRequest.CheckReadPermissions,
			QueryRequest.AsNoTracking,
			im => im.IdMessage == QueryRequest.IdMessage);
	}

	public override async Task<MessageBox.Model.VwMessage?> ToResultAsync(
		IScopeContext scopeContext,
		CancellationToken cancellationToken = default)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return await GetQuery(scopeContext)
			.FirstOrDefaultAsync(cancellationToken);
	}

	public MessageBox.Model.VwMessage? ToResult(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return GetQuery(scopeContext).FirstOrDefault();
	}
}
