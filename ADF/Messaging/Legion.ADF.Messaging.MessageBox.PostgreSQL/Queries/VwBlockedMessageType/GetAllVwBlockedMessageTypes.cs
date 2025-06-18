using Legion.ADF.Messaging.MessageBox.PostgreSQL;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Microsoft.EntityFrameworkCore;

namespace Legion.ADF.Messaging.MessageBox.Queries.VwBlockedMessageType;

public class GetAllVwBlockedMessageTypes :
	QueryDefinition<
		IMessageBoxQueryDbContext,
		MessageBox.Model.VwBlockedMessageType,
		List<MessageBox.Model.VwBlockedMessageType>,
		GetAllVwBlockedMessageTypesQuery>,
		IGetAllVwBlockedMessageTypes
{
	public GetAllVwBlockedMessageTypes(
		IEFConnectionProvider connectionProvider,
		GetAllVwBlockedMessageTypesQuery getAllVwBlockedMessageTypes)
		: base(connectionProvider, getAllVwBlockedMessageTypes)
	{
	}

	protected override IQueryable<MessageBox.Model.VwBlockedMessageType> GetDefaultQuery(IScopeContext scopeContext)
	{
		var context = GetContext(scopeContext);
		return context.VwBlockedMessageType;
	}

	public override IQueryable<MessageBox.Model.VwBlockedMessageType> GetQuery(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return ApplyIncludesThenWhere<IMessagingAccessControlManager>(
			scopeContext,
			QueryRequest.QueryableBuilder,
			QueryRequest.CheckReadPermissions,
			QueryRequest.AsNoTracking,
			null);
	}

	public override async Task<List<MessageBox.Model.VwBlockedMessageType>> ToResultAsync(
		IScopeContext scopeContext,
		CancellationToken cancellationToken = default)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return await GetQuery(scopeContext)
			.ToListAsync(cancellationToken);
	}

	public List<MessageBox.Model.VwBlockedMessageType> ToResult(IScopeContext scopeContext)
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
