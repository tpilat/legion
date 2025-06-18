using Legion.ADF.Messaging.MessageBox.SqlServer;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Microsoft.EntityFrameworkCore;

namespace Legion.ADF.Messaging.MessageBox.Queries.SubscribedMessage;

public class GetSubscribedMessagesByIdMessage :
	QueryDefinition<
		IMessageBoxDbContext,
		MessageBox.Model.SubscribedMessage,
		List<MessageBox.Model.SubscribedMessage>,
		GetSubscribedMessagesByIdMessageQuery>,
		IGetSubscribedMessagesByIdMessage
{
	public GetSubscribedMessagesByIdMessage(
		IEFConnectionProvider connectionProvider,
		GetSubscribedMessagesByIdMessageQuery getSubscribedMessagesByIdMessage)
		: base(connectionProvider, getSubscribedMessagesByIdMessage)
	{
	}

	protected override IQueryable<MessageBox.Model.SubscribedMessage> GetDefaultQuery(IScopeContext scopeContext)
	{
		var context = GetContext(scopeContext);
		return context.SubscribedMessage;
	}

	public override IQueryable<MessageBox.Model.SubscribedMessage> GetQuery(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return ApplyIncludesThenWhere<IMessagingAccessControlManager>(
			scopeContext,
			QueryRequest.QueryableBuilder,
			QueryRequest.CheckReadPermissions,
			QueryRequest.AsNoTracking,
			x => x.IdMessage == QueryRequest.IdMessage);
	}

	public override async Task<List<MessageBox.Model.SubscribedMessage>> ToResultAsync(
		IScopeContext scopeContext,
		CancellationToken cancellationToken = default)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return await GetQuery(scopeContext)
			.ToListAsync(cancellationToken);
	}

	public List<MessageBox.Model.SubscribedMessage> ToResult(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return GetQuery(scopeContext).ToList();
	}

	public async Task<Dictionary<Guid, DateTime>> ToMessageIds(
		IScopeContext scopeContext,
		CancellationToken cancellationToken = default)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return await GetQuery(scopeContext)
			.Select(de => new ValueTuple<Guid, DateTime>(de.IdMessage, de.NextProcessingUtc))
			.ToDictionaryAsync(k => k.Item1, v => v.Item2, cancellationToken);
	}

	public Dictionary<Guid, DateTime> ToMessageIds(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return GetQuery(scopeContext)
			.Select(de => new ValueTuple<Guid, DateTime>(de.IdMessage, de.NextProcessingUtc))
			.ToDictionary(k => k.Item1, v => v.Item2);
	}
}
