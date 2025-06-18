using Legion.ADF.Messaging.MessageBox.SqlServer;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Microsoft.EntityFrameworkCore;

namespace Legion.ADF.Messaging.MessageBox.Queries.BlockedMessageType;

public class GetBlockedMessageTypesByNamespaces :
	QueryDefinition<
		IMessageBoxDbContext,
		MessageBox.Model.BlockedMessageType,
		List<MessageBox.Model.BlockedMessageType>,
		GetBlockedMessageTypesByNamespacesQuery>,
		IGetBlockedMessageTypesByNamespaces
{
	public GetBlockedMessageTypesByNamespaces(
		IEFConnectionProvider connectionProvider,
		GetBlockedMessageTypesByNamespacesQuery GetBlockedMessageTypesByNamespaces)
		: base(connectionProvider, GetBlockedMessageTypesByNamespaces)
	{
	}

	protected override IQueryable<MessageBox.Model.BlockedMessageType> GetDefaultQuery(IScopeContext scopeContext)
	{
		var context = GetContext(scopeContext);
		return context.BlockedMessageType;
	}

	public override IQueryable<MessageBox.Model.BlockedMessageType> GetQuery(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		if (0 < QueryRequest.Namespaces?.Count)
		{
			return ApplyIncludesThenWhere<IMessagingAccessControlManager>(
				scopeContext,
				QueryRequest.QueryableBuilder,
				QueryRequest.CheckReadPermissions,
				QueryRequest.AsNoTracking,
				x => QueryRequest.Namespaces.Contains(x.Namespace));
		}
		else
		{
			return ApplyIncludesThenWhere<IMessagingAccessControlManager>(
				scopeContext,
				QueryRequest.QueryableBuilder,
				QueryRequest.CheckReadPermissions,
				QueryRequest.AsNoTracking,
				null);
		}
	}

	public override async Task<List<MessageBox.Model.BlockedMessageType>> ToResultAsync(
		IScopeContext scopeContext,
		CancellationToken cancellationToken = default)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return await GetQuery(scopeContext)
			.ToListAsync(cancellationToken);
	}

	public List<MessageBox.Model.BlockedMessageType> ToResult(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return GetQuery(scopeContext).ToList();
	}

	public async Task<List<string>> ToNamespacesAsync(
		IScopeContext scopeContext,
		CancellationToken cancellationToken = default)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return await GetQuery(scopeContext)
			.Select(bde => bde.Namespace)
			.ToListAsync(cancellationToken);
	}

	public List<string> ToNamespaces(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return GetQuery(scopeContext)
			.Select(bde => bde.Namespace)
			.ToList();
	}
}
