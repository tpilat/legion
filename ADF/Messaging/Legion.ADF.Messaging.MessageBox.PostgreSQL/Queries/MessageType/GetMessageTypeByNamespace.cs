using Legion.ADF.Messaging.MessageBox.PostgreSQL;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Microsoft.EntityFrameworkCore;

namespace Legion.ADF.Messaging.MessageBox.Queries.MessageType;

public class GetMessageTypeByNamespace :
	QueryDefinition<
		IMessageBoxDbContext,
		MessageBox.Model.MessageType,
		MessageBox.Model.MessageType?,
		GetMessageTypeByNamespaceQuery>,
		IGetMessageTypeByNamespace
{
	public GetMessageTypeByNamespace(
		IEFConnectionProvider connectionProvider,
		GetMessageTypeByNamespaceQuery getMessageTypeByNamespace)
		: base(connectionProvider, getMessageTypeByNamespace)
	{
	}

	protected override IQueryable<MessageBox.Model.MessageType> GetDefaultQuery(IScopeContext scopeContext)
	{
		var context = GetContext(scopeContext);
		return context.MessageType;
	}

	public override IQueryable<MessageBox.Model.MessageType> GetQuery(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return ApplyIncludesThenWhere<IMessagingAccessControlManager>(
			scopeContext,
			QueryRequest.QueryableBuilder,
			QueryRequest.CheckReadPermissions,
			QueryRequest.AsNoTracking,
			imt => imt.Namespace == QueryRequest.Namespace);
	}

	public override async Task<MessageBox.Model.MessageType?> ToResultAsync(
		IScopeContext scopeContext,
		CancellationToken cancellationToken = default)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return await GetQuery(scopeContext)
			.FirstOrDefaultAsync(cancellationToken);
	}

	public MessageBox.Model.MessageType? ToResult(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return GetQuery(scopeContext).FirstOrDefault();
	}

	public async Task<Guid?> GetIdMessageTypeAsync(
		IScopeContext scopeContext,
		CancellationToken cancellationToken = default)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return await GetQuery(scopeContext)
			.Select(iq => iq.IdMessageType)
			.FirstOrDefaultAsync(cancellationToken);
	}

	public Guid? GetIdMessageType(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return GetQuery(scopeContext)
			.Select(iq => iq.IdMessageType)
			.FirstOrDefault();
	}

	public async Task<bool> ExistsAsync(
		IScopeContext scopeContext,
		CancellationToken cancellationToken = default)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return await GetQuery(scopeContext)
			.AnyAsync(cancellationToken);
	}

	public bool Exists(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return GetQuery(scopeContext)
			.Any();
	}
}
