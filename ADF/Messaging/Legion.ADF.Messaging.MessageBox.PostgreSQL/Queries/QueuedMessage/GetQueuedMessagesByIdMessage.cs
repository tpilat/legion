using Legion.ADF.Messaging.MessageBox.PostgreSQL;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Microsoft.EntityFrameworkCore;

namespace Legion.ADF.Messaging.MessageBox.Queries.QueuedMessage;

public class GetQueuedMessagesByIdMessage :
	QueryDefinition<
		IMessageBoxDbContext,
		MessageBox.Model.QueuedMessage,
		List<MessageBox.Model.QueuedMessage>,
		GetQueuedMessagesByIdMessageQuery>,
		IGetQueuedMessagesByIdMessage
{
	public GetQueuedMessagesByIdMessage(
		IEFConnectionProvider connectionProvider,
		GetQueuedMessagesByIdMessageQuery getQueuedMessagesByIdMessage)
		: base(connectionProvider, getQueuedMessagesByIdMessage)
	{
	}

	protected override IQueryable<MessageBox.Model.QueuedMessage> GetDefaultQuery(IScopeContext scopeContext)
	{
		var context = GetContext(scopeContext);
		return context.QueuedMessage;
	}

	public override IQueryable<MessageBox.Model.QueuedMessage> GetQuery(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return ApplyIncludesThenWhere<IMessagingAccessControlManager>(
			scopeContext,
			QueryRequest.QueryableBuilder,
			QueryRequest.CheckReadPermissions,
			QueryRequest.AsNoTracking,
			x => x.IdMessage == QueryRequest.IdMessage);
	}

	public override async Task<List<MessageBox.Model.QueuedMessage>> ToResultAsync(
		IScopeContext scopeContext,
		CancellationToken cancellationToken = default)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return await GetQuery(scopeContext)
			.ToListAsync(cancellationToken);
	}

	public List<MessageBox.Model.QueuedMessage> ToResult(IScopeContext scopeContext)
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
			.Select(de => new ValueTuple<Guid, DateTime>(de.IdQueuedMessage, de.NextProcessingUtc))
			.ToDictionaryAsync(k => k.Item1, v => v.Item2, cancellationToken);
	}

	public Dictionary<Guid, DateTime> ToMessageIds(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return GetQuery(scopeContext)
			.Select(de => new ValueTuple<Guid, DateTime>(de.IdQueuedMessage, de.NextProcessingUtc))
			.ToDictionary(k => k.Item1, v => v.Item2);
	}
}
