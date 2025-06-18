using Legion.ADF.Messaging.MessageBox.PostgreSQL;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Microsoft.EntityFrameworkCore;

namespace Legion.ADF.Messaging.MessageBox.Queries.MessageType;

public class GetAllMessageTypes :
	QueryDefinition<
		IMessageBoxDbContext,
		MessageBox.Model.MessageType,
		List<MessageBox.Model.MessageType>,
		GetAllMessageTypesQuery>,
		IGetAllMessageTypes
{
	public GetAllMessageTypes(
		IEFConnectionProvider connectionProvider,
		GetAllMessageTypesQuery getAllMessageTypes)
		: base(connectionProvider, getAllMessageTypes)
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
			null);
	}

	public override async Task<List<MessageBox.Model.MessageType>> ToResultAsync(
		IScopeContext scopeContext,
		CancellationToken cancellationToken = default)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return await GetQuery(scopeContext)
			.ToListAsync(cancellationToken);
	}

	public List<MessageBox.Model.MessageType> ToResult(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return GetQuery(scopeContext).ToList();
	}
}
