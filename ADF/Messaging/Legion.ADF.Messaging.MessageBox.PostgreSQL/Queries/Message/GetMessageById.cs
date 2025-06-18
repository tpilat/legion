using Legion.ADF.Messaging.MessageBox.PostgreSQL;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Microsoft.EntityFrameworkCore;

namespace Legion.ADF.Messaging.MessageBox.Queries.Message;

public class GetMessageById :
	QueryDefinition<
		IMessageBoxDbContext,
		MessageBox.Model.Message,
		MessageBox.Model.Message?,
		GetMessageByIdQuery>,
	IGetMessageById
{
	public GetMessageById(
		IEFConnectionProvider connectionProvider,
		GetMessageByIdQuery getMessageById)
		: base(connectionProvider, getMessageById)
	{
	}

	protected override IQueryable<MessageBox.Model.Message> GetDefaultQuery(IScopeContext scopeContext)
	{
		var context = GetContext(scopeContext);
		return QueryRequest.IncludeContent
			? context.Message.Include(im => im.MessageType).Include(im => im.MessageContent)
			: context.Message.Include(im => im.MessageType);
	}

	public override IQueryable<MessageBox.Model.Message> GetQuery(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return ApplyIncludesThenWhere<IMessagingAccessControlManager>(
			scopeContext,
			QueryRequest.QueryableBuilder,
			QueryRequest.CheckReadPermissions,
			QueryRequest.AsNoTracking,
			x => x.IdMessage == QueryRequest.IdMessage);
	}

	public override async Task<MessageBox.Model.Message?> ToResultAsync(
		IScopeContext scopeContext,
		CancellationToken cancellationToken = default)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return await GetQuery(scopeContext)
			.FirstOrDefaultAsync(cancellationToken);
	}

	public MessageBox.Model.Message? ToResult(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return GetQuery(scopeContext).FirstOrDefault();
	}
}
