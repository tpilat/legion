using Legion.ADF.Messaging.MessageBox.PostgreSQL;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Microsoft.EntityFrameworkCore;

namespace Legion.ADF.Messaging.MessageBox.Queries.Message;

public class ExistsMessageByTopicMessageId :
	QueryDefinition<
		IMessageBoxDbContext,
		MessageBox.Model.Message,
		bool,
		ExistsMessageByTopicMessageIdQuery>,
		IExistsMessageByTopicMessageId
{
	public ExistsMessageByTopicMessageId(
		IEFConnectionProvider connectionProvider,
		ExistsMessageByTopicMessageIdQuery existsMessageByTopicMessageId)
		: base(connectionProvider, existsMessageByTopicMessageId)
	{
	}

	protected override IQueryable<MessageBox.Model.Message> GetDefaultQuery(IScopeContext scopeContext)
	{
		var context = GetContext(scopeContext);
		return context.Message;
	}

	public override IQueryable<MessageBox.Model.Message> GetQuery(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return ApplyIncludesThenWhere<IMessagingAccessControlManager>(
			scopeContext,
			QueryRequest.QueryableBuilder,
			QueryRequest.CheckReadPermissions,
			QueryRequest.AsNoTracking,
			im => im.IdTopic == QueryRequest.IdTopic
				&& im.MessageId == QueryRequest.MessageId);
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

	public async Task<Guid?> GetIdMessageAsync(
		IScopeContext scopeContext,
		CancellationToken cancellationToken = default)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return await GetQuery(scopeContext)
			.Select(iq => iq.IdMessage)
			.FirstOrDefaultAsync(cancellationToken);
	}

	public Guid? GetIdMessage(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return GetQuery(scopeContext)
			.Select(iq => iq.IdMessage)
			.FirstOrDefault();
	}
}
