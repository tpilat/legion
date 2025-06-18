using Legion.ADF.Messaging.MessageBox.PostgreSQL;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Microsoft.EntityFrameworkCore;

namespace Legion.ADF.Messaging.MessageBox.Queries.MessageBoxInstance;

public class GetMessageBoxInstanceById :
	QueryDefinition<
		IMessageBoxDbContext,
		MessageBox.Model.MessageBoxInstance,
		MessageBox.Model.MessageBoxInstance?,
		GetMessageBoxInstanceByIdQuery>,
		IGetMessageBoxInstanceById
{
	public GetMessageBoxInstanceById(
		IEFConnectionProvider connectionProvider,
		GetMessageBoxInstanceByIdQuery getMessageBoxInstanceById)
		: base(connectionProvider, getMessageBoxInstanceById)
	{
	}

	protected override IQueryable<MessageBox.Model.MessageBoxInstance> GetDefaultQuery(IScopeContext scopeContext)
	{
		var context = GetContext(scopeContext);
		return context.MessageBoxInstance;
	}

	public override IQueryable<MessageBox.Model.MessageBoxInstance> GetQuery(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return ApplyIncludesThenWhere<IMessagingAccessControlManager>(
			scopeContext,
			QueryRequest.QueryableBuilder,
			QueryRequest.CheckReadPermissions,
			QueryRequest.AsNoTracking,
			imt => imt.IdMessageBoxInstance == QueryRequest.IdMessageBoxInstance);
	}

	public override async Task<MessageBox.Model.MessageBoxInstance?> ToResultAsync(
		IScopeContext scopeContext,
		CancellationToken cancellationToken = default)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return await GetQuery(scopeContext)
			.FirstOrDefaultAsync(cancellationToken);
	}

	public MessageBox.Model.MessageBoxInstance? ToResult(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return GetQuery(scopeContext).FirstOrDefault();
	}
}
